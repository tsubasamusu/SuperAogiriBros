using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p
using System.Runtime.InteropServices.WindowsRuntime;

namespace yamap {

    public class NPCController : CharaControllerBase {

        //[SerializeField]
        private Transform enemyTran = null;//�G�̈ʒu���

        //[SerializeField]
        //private GameObject attackPoint;//�U���ʒu

        //[SerializeField]
        //private Rigidbody rb;//RigidBody

        //[SerializeField]
        //private Animator animator;//Animator

        //[SerializeField]
        //private CharacterManager.CharaName myName;//�����̖��O

        //private bool isAttack;//�U�����Ă��邩�ǂ���

        private bool isJumping;//�W�����v���Ă��邩�ǂ���

        private float currentMoveSpeed;//���݂̈ړ����x

        //private bool soundFlag;//�W�����v�̌��ʉ��p

        //private bool jumped;//�R����W�����v�������ǂ���


        /// <summary>
        /// CharacterController�̏����ݒ���s��
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        public override void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc) {
            base.SetUpCharacterController(charaData, ownerType);

            //���݂̈ړ����x�������l�ɐݒ�
            currentMoveSpeed = GameData.instance.npcMoveSpeed;
            enemyTran = npc.transform;
        }

        ///// <summary>
        ///// �Q�[���J�n����ɌĂяo�����
        ///// </summary>
        //private void Start()
        //{
        //    //�U���ʒu�𖳌���
        //    attackPoint.SetActive(false);

        //    //���݂̈ړ����x�������l�ɐݒ�
        //    currentMoveSpeed = GameData.instance.npcMoveSpeed;

        //    //�ړ����J�n����
        //    StartCoroutine(Move());
        //}

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() {
            if (!isSetUp) {
                return;
            }

            //�G�����Ɏ���ł���Ȃ�
            if (enemyTran == null) {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�R�ɂ����݂��Ă���Ȃ�
            if (CheckCliff()) {
                StartCoroutine(ClingingCliff());

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G����O�ɂ���Ȃ�
            if (Mathf.Abs(enemyTran.position.x) > 7f) {
                //NPC�̓������~�߂�
                currentMoveSpeed = 0f;

                //����A�j���[�V�������~�߂�
                animator.SetBool("Run", false);

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G�������̐^��or�^���ɂ���Ȃ�
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) <= 0.5f) {
                //NPC�̓������~�߂�
                currentMoveSpeed = 0f;

                //�܂��W�����v���Ă��Ȃ��Ȃ�
                if (!isJumping) {
                    //�W�����v����
                    StartCoroutine(Jump());
                }

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G���U�������ɓ����Ă���Ȃ�
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f && Mathf.Abs(enemyTran.position.y - transform.position.y) < 2) {
                //�U�����ł͂Ȃ��Ȃ�
                if (!isAttack) {
                    //����A�j���[�V�������~�߂�
                    animator.SetBool("Run", false);

                    //�W�����v�̃A�j���[�V�������~�߂�
                    animator.SetBool("Jump", false);

                    //NPC�̓������~�߂�
                    currentMoveSpeed = 0f;

                    //�U������
                    StartCoroutine(Attack());
                }

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //���݂̈ړ����x�������l�ɐݒ�
            currentMoveSpeed = GameData.instance.npcMoveSpeed;

            //�G�����g���E�ɂ���Ȃ�
            if (enemyTran.position.x < transform.position.x) {
                //�E������
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
            }
            //�G�����g��荶�ɂ���Ȃ�
            else if (enemyTran.position.x > transform.position.x) {
                //��������
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
            }

            //�U�����Ȃ�
            if (isAttack) {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�ڒn���Ă���Ȃ�
            if (CheckGrounded()) {
                //soundflag��false������
                soundFlag = false;

                //�R����W�����v���Ă��Ȃ���Ԃɐ؂�ւ���
                jumped = false;

                //����A�j���[�V�������s��
                animator.SetBool("Run", true);
            }
        }

        /// <summary>
        /// �W�����v����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator Jump() {
            //�W�����v���ɐ؂�ւ���
            isJumping = true;

            //���ʉ����Đ�
            SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

            //�W�����v�̃A�j���[�V�������s��
            animator.SetBool("Jump", true);

            //�W�����v����
            rb.AddForce(transform.up * GameData.instance.npcJumpPower);

            //���S�ɗ�������܂ő҂�
            yield return new WaitForSeconds(1.8f);

            //�W�����v�̃A�j���[�V�������~�߂�
            animator.SetBool("Jump", false);

            //�W�����v���I������
            isJumping = false;
        }

        ///// <summary>
        ///// �ڒn������s��
        ///// </summary>
        ///// <returns>�ڒn���Ă�����true</returns>
        //private bool CheckGrounded()
        //{
        //    //�����̏����ʒu�ƌ�����ݒ�
        //    Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //    //�����̒�����ݒ�
        //    float tolerance = 0.3f;

        //    //�����̔����Ԃ�
        //    return Physics.Raycast(ray, tolerance);
        //}

        ///// <summary>
        ///// �ړ������s����
        ///// </summary>
        ///// <returns>�҂�����</returns>
        //private IEnumerator Move()
        //{
        //    //�Q�[���J�n����ɏ����҂�
        //    yield return new WaitForSeconds(0.5f);

        //    //�����ɌJ��Ԃ�
        //    while(true)
        //    {
        //        //�G�����Ɏ��S�����Ȃ�
        //        if(enemyTran==null)
        //        {
        //            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
        //            yield return new WaitForSeconds(Time.fixedDeltaTime);

        //            //���̌J��Ԃ������Ɉڂ�
        //            continue;
        //        }

        //        //���g����O�ɂ���Ȃ�
        //        if (Mathf.Abs(transform.position.x)>7f&&transform.position.y<0f)
        //        {
        //            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
        //            yield return new WaitForSeconds(Time.fixedDeltaTime);

        //            //���̌J��Ԃ������Ɉڂ�
        //            continue;
        //        }

        //        //�ړ�����
        //        rb.AddForce(transform.forward * currentMoveSpeed);

        //        //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
        //        yield return new WaitForSeconds(Time.fixedDeltaTime);
        //    }
        //}

        /// <summary>
        /// �v���C���[��NPC �ňړ��̐�������������邽�߂̃��\�b�h
        /// NPC �̈ړ�����������
        /// </summary>
        /// <param name="characterManager"></param>
        /// <returns></returns>
        protected override IEnumerator ObserveMove() {

            //�G�����Ɏ��S�����Ȃ�
            if (enemyTran == null) {
                //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //���̌J��Ԃ������Ɉڂ�
                yield break;
            }

            //���g����O�ɂ���Ȃ�
            if (Mathf.Abs(transform.position.x) > 7f && transform.position.y < 0f) {
                //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //���̌J��Ԃ������Ɉڂ�
                yield break;
            }

            //�ړ�����
            rb.AddForce(transform.forward * currentMoveSpeed);

            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
            yield return new WaitForSeconds(Time.fixedDeltaTime);

        }

        ///// <summary>
        ///// �U������
        ///// </summary>
        ///// <returns>�҂�����</returns>
        //private IEnumerator Attack()
        //{
        //    //�U�����ɐ؂�ւ���
        //    isAttack = true;

        //    //�������Đ�
        //    SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetCharacterVoiceData(myName).clip);

        //    //�U���A�j���[�V�������s��
        //    animator.SetBool("Attack", true);

        //    //�ܐ悪�A�U���ʒu�ɗ���܂ő҂�
        //    yield return new WaitForSeconds(0.3f);

        //    //�U���ʒu��L����
        //    attackPoint.SetActive(true);

        //    //�������S�ɏオ��܂ő҂�
        //    yield return new WaitForSeconds(0.2f);

        //    //�U���ʒu�𖳌���
        //    attackPoint.SetActive(false);

        //    //�U���̃A�j���[�V�������~�߂�
        //    animator.SetBool("Attack", false);

        //    //���̎p���ɖ߂�܂ő҂�
        //    yield return new WaitForSeconds(0.5f);

        //    //�U�����I������
        //    isAttack = false;
        //}

        ///// <summary>
        ///// �R�ɂ����݂��Ă��邩�ǂ������ׂ�
        ///// </summary>
        ///// <returns>�R�ɂ����݂��Ă�����true</returns>
        //private bool CheckCliff()
        //{
        //    //�v���C���[���R���ォ���ɂ���Ȃ�
        //    if (transform.position.y > -1f || transform.position.y < -3f)
        //    {
        //        //�ȍ~�̏������s��Ȃ�
        //        return false;
        //    }

        //    //�v���C���[���R���O���ɂ���Ȃ�
        //    if (transform.position.x < -9f || transform.position.x > 9f)
        //    {
        //        //�ȍ~�̏������s��Ȃ�
        //        return false;
        //    }

        //    //true��Ԃ�
        //    return true;
        //}

        ///// <summary>
        ///// �R�ɂ����݂�
        ///// </summary>
        ///// <returns>�҂�����</returns>
        //private IEnumerator ClingingCliff()
        //{
        //    //���ɊR����W�����v�����Ȃ�
        //    if (jumped)
        //    {
        //        //�ȍ~�̏������s��Ȃ�
        //        yield break;
        //    }

        //    //soundFlag��false�Ȃ�
        //    if (!soundFlag)
        //    {
        //        //���ʉ����Đ�
        //        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Cliff).clip);

        //        //soundFlag��true������
        //        soundFlag = true;
        //    }

        //    //�U���̃A�j���[�V�������~�߂�
        //    animator.SetBool("Attack", false);

        //    //�W�����v�̃A�j���[�V�������~�߂�
        //    animator.SetBool("Jump", false);

        //    //����A�j���[�V�������~�߂�
        //    animator.SetBool("Run", false);

        //    //�R�ɂ����݂��A�j���[�V�������s��
        //    animator.SetBool("Cliff", true);

        //    //�L�����N�^�[���R�̈ʒu�Ɉړ�������
        //    transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        //    //�L�����N�^�[�̌������R�ɍ��킹��
        //    transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);

        //    //1�b�҂�
        //    yield return new WaitForSeconds(1f);

        //    //�܂��R����W�����v���Ă��Ȃ��Ȃ�
        //    if (!jumped)
        //    {
        //        //���ʉ����Đ�
        //        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

        //        //�W�����v����
        //        transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

        //        //�R�ɂ����݂A�j���[�V�������~�߂�
        //        animator.SetBool("Cliff", false);

        //        ///�W�����v�̃A�j���[�V�������s��
        //        animator.SetBool("Jump", true);

        //        //�W�����v������Ԃɐ؂�ւ���
        //        jumped = true;
        //    }
        //}


        protected override void AfterJump() {
            //�܂��R����W�����v���Ă��Ȃ��Ȃ�
            if (!jumped) {
                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //�W�����v����
                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

                //�R�ɂ����݂A�j���[�V�������~�߂�
                animator.SetBool("Cliff", false);

                ///�W�����v�̃A�j���[�V�������s��
                animator.SetBool("Jump", true);

                //�W�����v������Ԃɐ؂�ւ���
                jumped = true;
            }
        }
    }
}