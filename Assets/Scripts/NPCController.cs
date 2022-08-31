using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private Transform enemyTran;//�G�̈ʒu���

    [SerializeField]
    private float npcMoveSpeed;//�ړ����x�i���j

    [SerializeField]
    private float npcJumpPower;//�W�����v�́i���j

    [SerializeField]
    private GameObject attackPoint;//�U���ʒu

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private bool isAttack;//�U�����Ă��邩�ǂ���

    private bool isJumping;//�W�����v���Ă��邩�ǂ���

    private float currentMoveSpeed;//���݂̈ړ����x

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�U���ʒu�𖳌���
        attackPoint.SetActive(false);

        //TODO:GameData����npcMoveSpeed���擾���鏈��

        //���݂̈ړ����x�������l�ɐݒ�
        currentMoveSpeed = npcMoveSpeed;

        //�ړ����J�n����
        StartCoroutine(Move());
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�G�����Ɏ���ł���Ȃ�
        if(enemyTran==null)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�G����O�ɂ���Ȃ�
        if (enemyTran.position.x > 7f || enemyTran.position.x < -7f)
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
            if(!isJumping)
            {
                //�W�����v����
                StartCoroutine(Jump());
            }

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�G���������ōU�������ɓ����Ă���Ȃ�
        if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f)
        {
            //�G���c�����ōU�������ɓ����Ă��Ȃ�������
            if(enemyTran.position.y<(transform.position.y-2f)&&enemyTran.position.y>(transform.position.y+1f))
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

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
        currentMoveSpeed = npcMoveSpeed;

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
        if(isAttack)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�ڒn���Ă���Ȃ�
        if(CheckGrounded())
        {
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

        //�W�����v�̃A�j���[�V�������s��
        animator.SetBool("Jump", true);

        //TODO:GameData����W�����v�͂��擾���鏈��

        //�W�����v����
        rb.AddForce(transform.up * npcJumpPower);

        //���S�ɗ�������܂ő҂�
        yield return new WaitForSeconds(1.8f);

        //�W�����v�̃A�j���[�V�������~�߂�
        animator.SetBool("Jump", false);

        //�W�����v���I������
        isJumping = false;
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
    /// �ړ������s����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Move()
    {
        //�Q�[���J�n����ɏ����҂�
        yield return new WaitForSeconds(0.5f);

        //�����ɌJ��Ԃ�
        while(true)
        {
            //�G�����Ɏ��S�����Ȃ�
            if(enemyTran==null)
            {
                //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //���̌J��Ԃ������Ɉڂ�
                continue;
            }

            //���g����O�ɂ���Ȃ�
            if (transform.position.x < -7f || transform.position.x > 7f)
            {
                //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //���̌J��Ԃ������Ɉڂ�
                continue;
            }

            //�ړ�����
            rb.AddForce(transform.forward * currentMoveSpeed);

            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
            yield return new WaitForSeconds(Time.fixedDeltaTime);
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
}

