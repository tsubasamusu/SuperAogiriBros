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
    private GameObject attackPoint;//�U���ʒu

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private bool isAttack;//�U�����Ă��邩�ǂ���

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

        //�G���U�������ɓ���A�U�����ł͂Ȃ��Ȃ�
        if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f&&!isAttack)
        {
            //����A�j���[�V�������~�߂�
            animator.SetBool("Run", false);

            //NPC�̓������~�߂�
            currentMoveSpeed = 0f;

            //�U������
            StartCoroutine(Attack());

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

        //�U�����ł͂Ȃ��Ȃ�
        if(!isAttack)
        {
            //����A�j���[�V�������s��
            animator.SetBool("Run", true);
        }
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
                //�ȍ~�̏������s��Ȃ�
                yield return null;
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

