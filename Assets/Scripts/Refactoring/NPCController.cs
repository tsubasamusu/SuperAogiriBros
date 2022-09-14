using System.Collections;//IEnumerator���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

namespace yamap 
{
    public class NPCController : CharaControllerBase 
    {
        private Transform enemyTran = null;//�G�̈ʒu���

        private bool isJumping;//�W�����v���Ă��邩�ǂ���

        private float currentMoveSpeed;//���݂̈ړ����x

        /// <summary>
        /// CharacterController�̏����ݒ���s��
        /// </summary>
        /// <param name="charaData">�L�����N�^�[�̃f�[�^</param>
        /// <param name="ownerType">���L�҂̎��</param>
        /// <param name="npc">CharaControllerBase</param>
        public override void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc) 
        {
            base.SetUpCharacterController(charaData, ownerType);

            //���݂̈ړ����x�������l�ɐݒ�
            currentMoveSpeed = GameData.instance.npcMoveSpeed;

            //�G�̈ʒu�����擾
            enemyTran = npc.transform;
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() 
        {
            //�����ݒ肪�������Ă��Ȃ��Ȃ�
            if (!isSetUp) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G�����Ɏ���ł���Ȃ�
            if (enemyTran == null) {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�R�ɂ����݂��Ă���Ȃ�
            if (CheckCliff()) 
            {
                //�R�ɂ����݂�
                StartCoroutine(ClingingCliff());

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G����O�ɂ���Ȃ�
            if (Mathf.Abs(enemyTran.position.x) > 7f) 
            {
                //NPC�̓������~�߂�
                currentMoveSpeed = 0f;

                //����A�j���[�V�������~�߂�
                animator.SetBool("Run", false);

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G�������̐^��or�^���ɂ���Ȃ�
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) <= 0.5f) 
            {
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
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f && Mathf.Abs(enemyTran.position.y - transform.position.y) < 2) 
            {
                //�U�����ł͂Ȃ��Ȃ�
                if (!isAttack) 
                {
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
            if (enemyTran.position.x < transform.position.x) 
            {
                //�E������
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
            }
            //�G�����g��荶�ɂ���Ȃ�
            else if (enemyTran.position.x > transform.position.x) 
            {
                //��������
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
            }

            //�U�����Ȃ�
            if (isAttack) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�ڒn���Ă���Ȃ�
            if (CheckGrounded()) 
            {
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
        private IEnumerator Jump() 
        {
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

        /// <summary>
        /// �v���C���[��NPC �ňړ��̐�������������邽�߂̃��\�b�h
        /// NPC �̈ړ�����������
        /// </summary>
        /// <returns>�҂�����</returns>
        protected override IEnumerator ObserveMove() 
        {
            //�G�����Ɏ��S�����Ȃ�
            if (enemyTran == null) 
            {
                //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //���̌J��Ԃ������Ɉڂ�
                yield break;
            }

            //���g����O�ɂ���Ȃ�
            if (Mathf.Abs(transform.position.x) > 7f && transform.position.y < 0f) 
            {
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

        /// <summary>
        /// �W�����v��̏�����   �s��
        /// </summary>
        protected override void AfterJump() 
        {
            //�܂��R����W�����v���Ă��Ȃ��Ȃ�
            if (!jumped) 
            {
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