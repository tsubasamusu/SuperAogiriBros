using System.Collections;//IEnumerator���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

namespace Tsubasa
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private GameObject attackPoint;//�U���ʒu

        [SerializeField]
        private Rigidbody rb;//RigidBody

        [SerializeField]
        private Animator animator;//Animator

        [SerializeField]
        private CharacterManager.CharaName myName;//�����̖��O

        private float moveDirection;//�ړ�����

        private float cliffTimer;//�R�ɂ����݂��Ă��鑍����

        private bool isjumping;//�W�����v���Ă��邩�ǂ���

        private bool isAttack;//�U�����Ă��邩�ǂ���

        private bool jumped;//�R����W�����v�������ǂ���

        private bool soundFlag;//�R�̌��ʉ��p

        /// <summary>
        /// CharacterController�̏����ݒ���s��
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        public void SetUpCharacterController(CharacterManager characterManager)
        {
            //�U���ʒu�𖳌���
            attackPoint.SetActive(false);

            //�v���C���[�̈ړ����J�n����
            StartCoroutine(Move(characterManager));
        }

        /// <summary>
        /// �v���C���[�̈ړ������s����
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        /// <returns>�҂�����</returns>
        private IEnumerator Move(CharacterManager characterManager)
        {
            //0.5�b�҂�
            yield return new WaitForSeconds(0.5f);

            //�����ɌJ��Ԃ�
            while (true)
            {
                //�R�ɂ����݂��Ă���Ȃ�
                if (CheckCliff())
                {
                    //���Ԃ��v������
                    cliffTimer += Time.fixedDeltaTime;

                    //���ԓI�ɂ܂������݂��Ă�����Ȃ�
                    if (cliffTimer < GameData.instance.maxCliffTime)
                    {
                        //�R�ɂ����݂�
                        ClingingCliff(characterManager);
                    }
                    //�����݂��Ă�����ō����Ԃ𒴂�����
                    else
                    {
                        //�R�ɂ����݂��A�j���[�V��������߂�
                        animator.SetBool("Cliff", false);
                    }

                    //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                    yield return new WaitForSeconds(Time.fixedDeltaTime);

                    //���̌J��Ԃ������֔�΂�
                    continue;
                }

                //soundFlag��false������
                soundFlag = false;

                //�R�ɂ����݂��Ă����鎞�Ԃ�����������
                cliffTimer = 0f;

                //�v���[���[�̍s���𐧌䂷��
                StartCoroutine(ControlMovement(characterManager));

                //�U���ȊO�̃A�j���[�V�����𐧌䂷��
                ControlAnimation();

                //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// �v���[���[�̍s���𐧌䂷��
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        /// <returns>�҂�����</returns>
        private IEnumerator ControlMovement(CharacterManager characterManager)
        {
            //�E��󂪉�����Ă����
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Right)))
            {
                //�E������
                transform.eulerAngles = new Vector3(0f, -90f, 0f);

                //�ړ�������ݒ�
                moveDirection = 1f;
            }
            //����󂪉�����Ă����
            else if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Left)))
            {
                //��������
                transform.eulerAngles = new Vector3(0f, 90f, 0f);

                //�ړ�������ݒ�
                moveDirection = -1f;
            }
            //�ړ��w�����Ȃ����
            else
            {
                //�ړ����Ȃ�
                moveDirection = 0f;
            }

            rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * GameData.instance.moveSpeed);

            //����󂪉�����A�U�����ł͂Ȃ��Ȃ�
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Down)) && !isAttack)
            {
                //�U������
                StartCoroutine(Attack());
            }

            //���󂪉�����A�W�����v���ł͂Ȃ��Ȃ�
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Up)) && !isjumping)
            {
                //�W�����v���ɐ؂�ւ���
                isjumping = true;

                //���ʉ����Đ�
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //�W�����v����
                rb.AddForce(transform.up * GameData.instance.jumpPower);

                //���S�ɗ�������܂ő҂�
                yield return new WaitForSeconds(1.8f);

                //�W�����v���I������
                isjumping = false;
            }
        }

        /// <summary>
        /// �ڒn������s��
        /// </summary>
        /// <returns>�ڒn���Ă�����true</returns>
        private bool CheckGrounded()
        {
            //�����̏����ʒu�ƌ�����ݒ�
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            //�����̒�����ݒ�
            float tolerance = 0.3f;

            //�����̔����Ԃ�
            return Physics.Raycast(ray, tolerance);
        }

        /// <summary>
        /// �U���ȊO�̃A�j���[�V�����𐧌䂷��
        /// </summary>
        private void ControlAnimation()
        {
            //�R�ɂ����݂��A�j���[�V�������~�߂�
            animator.SetBool("Cliff", false);

            //�U�����Ȃ�
            if (isAttack)
            {
                //�W�����v�̃A�j���[�V�������~�߂�
                animator.SetBool("Jump", false);

                //����A�j���[�V�������~�߂�
                animator.SetBool("Run", false);

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�W�����v���Ȃ�
            if (isjumping)
            {
                //����A�j���[�V�������~�߂�
                animator.SetBool("Run", false);

                //�W�����v�̃A�j���[�V�������s��
                animator.SetBool("Jump", true);

                //�ȍ~�̏������s��Ȃ�
                return;
            }
            //�W�����v���ł͂Ȃ��Ȃ�
            else
            {
                //�W�����v�̃A�j���[�V�������~�߂�
                animator.SetBool("Jump", false);
            }

            //�ڒn���Ă��Ȃ��Ȃ�
            if (!CheckGrounded())
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�R����W�����v���Ă��Ȃ���Ԃɐ؂�ւ���
            jumped = false;

            //�ړ��L�[��������Ă���Ȃ�
            if (moveDirection != 0f)
            {
                //����A�j���[�V�������s��
                animator.SetBool("Run", true);

                //�ȍ~�̏������s��Ȃ�
                return;
            }
            //�ړ��L�[��������Ă��Ȃ��Ȃ�
            else
            {
                //����A�j���[�V�������~�߂�
                animator.SetBool("Run", false);
            }
        }

        /// <summary>
        /// �U������
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator Attack()
        {
            //�U�����ɐ؂�ւ���
            isAttack = true;

            //�������Đ�
            SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetCharacterVoiceData(myName).clip);

            //�U���A�j���[�V�������s��
            animator.SetBool("Attack", true);

            //�ܐ悪�A�U���ʒu�ɗ���܂ő҂�
            yield return new WaitForSeconds(0.3f);

            //�U���ʒu��L����
            attackPoint.SetActive(true);

            //�������S�ɏオ��܂ő҂�
            yield return new WaitForSeconds(0.2f);

            //�U���ʒu�𖳌���
            attackPoint.SetActive(false);

            //�U���̃A�j���[�V�������~�߂�
            animator.SetBool("Attack", false);

            //���̎p���ɖ߂�܂ő҂�
            yield return new WaitForSeconds(0.5f);

            //�U�����I������
            isAttack = false;
        }

        /// <summary>
        /// �R�ɂ����݂��Ă��邩�ǂ������ׂ�
        /// </summary>
        /// <returns>�R�ɂ����݂��Ă�����true</returns>
        private bool CheckCliff()
        {
            //�v���C���[���R���ォ���ɂ���Ȃ�
            if (transform.position.y > -1f || transform.position.y < -3f)
            {
                //�ȍ~�̏������s��Ȃ�
                return false;
            }

            //�v���C���[���R���O���ɂ���Ȃ�
            if (transform.position.x < -9f || transform.position.x > 9f)
            {
                //�ȍ~�̏������s��Ȃ�
                return false;
            }

            //true��Ԃ�
            return true;
        }

        /// <summary>
        /// �R�ɂ����݂�
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        private void ClingingCliff(CharacterManager characterManager)
        {
            //���ɊR����W�����v�����Ȃ�
            if (jumped)
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //soundFlag��false�Ȃ�
            if (!soundFlag)
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Cliff).clip);

                //soundFlag��true������
                soundFlag = true;
            }

            //�U���̃A�j���[�V�������~�߂�
            animator.SetBool("Attack", false);

            //�W�����v�̃A�j���[�V�������~�߂�
            animator.SetBool("Jump", false);

            //����A�j���[�V�������~�߂�
            animator.SetBool("Run", false);

            //�R�ɂ����݂��A�j���[�V�������s��
            animator.SetBool("Cliff", true);

            //���󂪉����ꂽ��
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Up)))
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //�W�����v����
                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

                //�W�����v������Ԃɐ؂�ւ���
                jumped = true;

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�v���C���[���R�̈ʒu�Ɉړ�������
            transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

            //�v���C���[�̌������R�ɍ��킹��
            transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
        }

    }
}
