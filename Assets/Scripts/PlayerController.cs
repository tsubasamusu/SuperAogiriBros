using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//�ړ����x�i���j

    [SerializeField]
    private float jumpHeight;//�W�����v�̍����i���j

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private float moveDirection;//�ړ�����

    private bool isjumping;//�W�����v���Ă��邩�ǂ���

    private bool isAttack;//�U�����Ă��邩�ǂ���

    private Vector3 movement;//�ړ��ʒu

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�v���[���[�̍s���𐧌䂷��
        StartCoroutine(ControlMovement());

        //�A�j���[�V�����𐧌䂷��
        ControlAnimation();
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()
    {
        //�ړ������s����
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// �v���[���[�̍s���𐧌䂷��
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator ControlMovement()
    {
        //�ړ��������擾
        moveDirection = Input.GetAxis("Horizontal");

        //TODO:GameData����ړ����x���擾���鏈��

        //�ړ��ʒu��ݒ�
        movement = new Vector3(-1f,transform.position.y,0f) * moveDirection * moveSpeed * Time.fixedDeltaTime;

        //�W�����v�L�[��������A�W�����v���ł͂Ȃ��Ȃ�
        if(Input.GetAxis("Vertical")>0f&&!isjumping)
        {
            //�W�����v���ɐ؂�ւ���
            isjumping = true;

            //TODO:GameData����W�����v�̍������擾���鏈��

            //�W�����v����
            transform.DOLocalMoveY(jumpHeight,1.25f/2f).SetLoops(2,LoopType.Yoyo).OnComplete(() => {isjumping=false;});
        }

        //�U���L�[��������A�U�����ł͂Ȃ��Ȃ�
        if(Input.GetAxis("Vertical")<0f&&!isAttack)
        {
            //�U�����ɐ؂�ւ���
            isAttack = true;

            //�U������܂ő҂�
            yield return new WaitForSeconds(0.5f);

            //�U�����I������
            isAttack = false;
        }
    }

    /// <summary>
    /// �A�j���[�V�����𐧌䂷��
    /// </summary>
    private void ControlAnimation()
    {
        //�U�����Ȃ�
        if(isAttack)
        {
            //�U���A�j���[�V�������s��
            animator.SetBool("Attack", true);

            //�ȍ~�̏������s��Ȃ�
            return;
        }
        //�U�����ł͂Ȃ��Ȃ�
        else
        {
            //�U���A�j���[�V�������~�߂�
            animator.SetBool("Attack", false);
        }

        //�W�����v���Ȃ�
        if(isjumping)
        {
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

        //TODO:�ڒn���Ă��Ȃ��Ȃ�ȍ~�̏������s��Ȃ�

        //�ړ��L�[��������Ă���Ȃ�
        if(moveDirection!=0f)
        {
            //����A�j���[�V�������~�߂�
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
}
