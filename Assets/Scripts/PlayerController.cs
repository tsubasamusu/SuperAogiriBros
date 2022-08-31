using System.Collections;//IEnumerator���g�p
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//�ړ����x�i���j

    [SerializeField]
    private float jumpPower;//�W�����v�̗́i���j

    [SerializeField,Tooltip("�R���畜������Ƃ��̃W�����v��")]
    private float specialJumpPower;//�R���畜������Ƃ��̃W�����v�́i���j

    [SerializeField, Tooltip("1�����ŊR�ɂ����݂��Ă����鑍����")]
    private float maxCliffTime;//1�����ŊR�ɂ����݂��Ă����鑍���ԁi���j

    [SerializeField]
    private GameObject attackPoint;//�U���ʒu

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private float moveDirection;//�ړ�����

    private float cliffTimer;//�R�ɂ����݂��Ă��鑍����

    private bool isjumping;//�W�����v���Ă��邩�ǂ���

    private bool isAttack;//�U�����Ă��邩�ǂ���

    private bool jumped;//�R����W�����v�������ǂ���

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�U���ʒu�𖳌���
        attackPoint.SetActive(false);
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()
    {
        //�R�ɂ����݂��Ă���Ȃ�
        if(CheckCliff())
        {
            //���Ԃ��v������
            cliffTimer += Time.deltaTime;

            //TODO:GameData����u1�����ŊR�ɂ����݂��Ă����鑍���ԁv���擾���鏈��

            //���ԓI�ɂ܂������݂��Ă�����Ȃ�
            if (cliffTimer < maxCliffTime)
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

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�v���[���[�̍s���𐧌䂷��
        StartCoroutine(ControlMovement());

        //�U���ȊO�̃A�j���[�V�����𐧌䂷��
        ControlAnimation();
    }

    /// <summary>
    /// �v���[���[�̍s���𐧌䂷��
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator ControlMovement()
    {
        //�ړ��������擾
        moveDirection = Input.GetAxis("Horizontal");

        //�E�����ֈړ�����Ȃ�
        if (moveDirection > 0f)
        {
            //�E������
            transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
        //�������ֈړ�����Ȃ�
        else if (moveDirection < 0f)
        {
            //��������
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        //TODO:GameData����ړ����x���擾���鏈��

        rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * moveSpeed);

        //�U���L�[��������A�U�����ł͂Ȃ��Ȃ�
        if (Input.GetAxis("Vertical") < 0f && !isAttack)
        {
            //�U������
            StartCoroutine(Attack());
        }

        //�W�����v�L�[��������A�W�����v���ł͂Ȃ��Ȃ�
        if (Input.GetAxis("Vertical") > 0f && !isjumping)
        {
            //�W�����v���ɐ؂�ւ���
            isjumping = true;

            //TODO:GameData����W�����v�͂��擾���鏈��

            //�W�����v����
            rb.AddForce(transform.up * jumpPower);

            //���S�ɗ�������܂ő҂�
            yield return new WaitForSeconds(1.5f);

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
        if(isAttack)
        {
            //�W�����v�̃A�j���[�V�������~�߂�
            animator.SetBool("Jump", false);

            //����A�j���[�V�������~�߂�
            animator.SetBool("Run", false);

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�W�����v���Ȃ�
        if(isjumping)
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
        if(!CheckGrounded())
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�R����W�����v���Ă��Ȃ���Ԃɐ؂�ւ���
        jumped = false;

        //�ړ��L�[��������Ă���Ȃ�
        if(moveDirection!=0f)
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
    /// �R�ɂ����݂��Ă��邩�ǂ������ׂāA�R�ɂ����݂��������s��
    /// </summary>
    /// <returns>�R�ɂ����݂��Ă�����true</returns>
    private bool CheckCliff()
    {
        //�v���C���[���R���ォ���ɂ���Ȃ�
        if(transform.position.y>-1f||transform.position.y<-3f)
        {
            //�ȍ~�̏������s��Ȃ�
            return false;
        }

        //�v���C���[���R���O���ɂ���Ȃ�
        if(transform.position.x<-9f||transform.position.x>9f)
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
    private void ClingingCliff()
    {
        //�U���̃A�j���[�V�������~�߂�
        animator.SetBool("Attack", false);

        //�W�����v�̃A�j���[�V�������~�߂�
        animator.SetBool("Jump", false);

        //����A�j���[�V�������~�߂�
        animator.SetBool("Run", false);

        //�R�ɂ����݂��A�j���[�V�������s��
        animator.SetBool("Cliff", true);

        //�W�����v�L�[��������āA�܂��W�����v���Ă��Ȃ��Ȃ�
        if (Input.GetAxis("Vertical") > 0&&!jumped)
        {
            //TODO:GameData����u�R���畜������Ƃ��̃W�����v�́v���擾���鏈��

            //�W�����v����
            rb.AddForce(transform.up * specialJumpPower);

            //�W�����v������Ԃɐ؂�ւ���
            jumped = true;

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�v���C���[���R�̈ʒu�Ɉړ�������
        transform.position = transform.position.x > 0 ? new Vector3(8.2f, -2f, 0f) : new Vector3(-8.2f, -2f, 0f);

        //�v���C���[�̌������R�ɍ��킹��
        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
    }
}
