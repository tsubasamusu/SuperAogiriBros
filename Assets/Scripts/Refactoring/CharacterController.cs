using System.Collections;//IEnumerator���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

namespace yamap
{
    /// <summary>
    /// �q�N���X�i�v���C���[�����삷��p�j
    /// </summary>
    public class CharacterController : CharaControllerBase 
    {
        private float moveDirection;//�ړ�����

        private float cliffTimer;//�R�ɂ����݂��Ă��鎞��

        /// <summary>
        /// ���L�ғ��L�̓��������s�iwhile���̒��ŌĂяo�����j
        /// </summary>
        /// <returns>�҂�����</returns>
        protected override IEnumerator ObserveMove() 
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
                    ClingingCliff();
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
                yield break;
            }

            //soundFlag��false������
            soundFlag = false;

            //�R�ɂ����݂��Ă����鎞�Ԃ�����������
            cliffTimer = 0f;

            //�v���[���[�̍s���𐧌䂷��
            StartCoroutine(ControlMovement());

            //�U���ȊO�̃A�j���[�V�����𐧌䂷��
            ControlAnimation();

            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        /// <summary>
        /// �v���[���[�̍s���𐧌䂷��
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator ControlMovement() 
        {
            //�E��󂪉�����Ă����
            if (Input.GetKey(charaData.keys[0])) 
            {
                //�E������
                transform.eulerAngles = new Vector3(0f, -90f, 0f);

                //�ړ�������ݒ�
                moveDirection = 1f;
            }
            //����󂪉�����Ă����
            else if (Input.GetKey(charaData.keys[1])) 
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

            //�͂�������
            rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * GameData.instance.moveSpeed);

            //����󂪉�����A�U�����ł͂Ȃ��Ȃ�
            if (Input.GetKey(charaData.keys[2]) && !isAttack) 
            {
                //�U������
                StartCoroutine(Attack());
            }

            //���󂪉�����A�W�����v���ł͂Ȃ��Ȃ�
            if (Input.GetKey(charaData.keys[3]) && !isjumping) 
            {
                //�W�����v���ɐ؂�ւ���
                isjumping = true;

                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //�W�����v����
                rb.AddForce(transform.up * GameData.instance.jumpPower);

                //���S�ɗ�������܂ő҂�
                yield return new WaitForSeconds(1.8f);

                //�W�����v���I������
                isjumping = false;
            }
        }

        /// <summary>
        /// �U���ȊO�̃A�j���[�V�����𐧌䂷��
        /// </summary>
        private void ControlAnimation() 
        {
            //�R�ɂ����݂��A�j���[�V�������~�߂�
            animator.SetBool("Cliff", false);

            //�U�����Ȃ�
            if (isAttack) {
                //�W�����v�̃A�j���[�V�������~�߂�
                animator.SetBool("Jump", false);

                //����A�j���[�V�������~�߂�
                animator.SetBool("Run", false);

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�W�����v���Ȃ�
            if (isjumping) {
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
        /// �W�����v��̏������s��
        /// </summary>
        protected override void AfterJump() 
        {
            //���󂪉����ꂽ��
            if (Input.GetKey(charaData.keys[3])) 
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //�W�����v����
                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

                //�W�����v������Ԃɐ؂�ւ���
                jumped = true;

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�L�����N�^�[�̌����ƈʒu���A�R�̌����ƈʒu�ɍ��킹��
            AdjustmentPlayerToCliffTran();
        }
    }
}