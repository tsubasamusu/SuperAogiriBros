using System.Collections;
using UnityEngine;



public enum OwnerType {
    Player,
    NPC
}

public class CharaControllerBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject attackPoint;//�U���ʒu

    ////[SerializeField]
    protected Rigidbody rb;//RigidBody

    //[SerializeField]
    protected Animator animator;//Animator

    //[SerializeField]
    //protected CharaName myName;//�����̖��O

    protected bool isjumping;//�W�����v���Ă��邩�ǂ���

    protected bool isAttack;//�U�����Ă��邩�ǂ���

    protected bool jumped;//�R����W�����v�������ǂ���

    protected bool soundFlag;//�R�̌��ʉ��p

    protected CharaData charaData;

    protected OwnerType ownerType;

    protected bool isSetUp;


    /// <summary>
    /// CharacterController�̏����ݒ���s��
    /// </summary>
    /// <param name="characterManager">CharacterManager</param>
    public virtual void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc = null) {

        this.charaData = charaData;
        this.ownerType = ownerType;

        if(!TryGetComponent(out rb)){
            Debug.Log("Rigidbody �擾�o���܂���B");
        }
        if (!TryGetComponent(out animator)) {
            Debug.Log("Animator �擾�o���܂���B");
        }

        //�U���ʒu�𖳌���
        attackPoint.SetActive(false);

        //�v���C���[�̈ړ����J�n����
        StartCoroutine(Move());

        isSetUp = true;
    }


    /// <summary>
    /// �v���C���[�̈ړ������s����
    /// </summary>
    /// <param name="characterManager">CharacterManager</param>
    /// <returns>�҂�����</returns>
    protected IEnumerator Move() {
        //0.5�b�҂�
        yield return new WaitForSeconds(0.5f);

        //�����ɌJ��Ԃ�
        while (true) {

            // Player �� NPC �Ń��\�b�h�̒��g��ς���
            StartCoroutine(ObserveMove());

            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// �v���C���[��NPC �ňړ��̐�������������邽�߂̃��\�b�h
    /// </summary>
    /// <param name="characterManager"></param>
    /// <returns></returns>

    protected virtual IEnumerator ObserveMove() {
        yield return null;
    }

    /// <summary>
    /// �ڒn������s��
    /// </summary>
    /// <returns>�ڒn���Ă�����true</returns>
    protected bool CheckGrounded() {
        //�����̏����ʒu�ƌ�����ݒ�
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //�����̒�����ݒ�
        float tolerance = 0.3f;

        //�����̔����Ԃ�
        return Physics.Raycast(ray, tolerance);
    }

    /// <summary>
    /// �U������
    /// </summary>
    /// <returns>�҂�����</returns>
    protected IEnumerator Attack() {
        //�U�����ɐ؂�ւ���
        isAttack = true;

        //�������Đ�
        yamap.SoundManager.instance.PlaySound(yamap.SoundManager.instance.GetCharacterVoiceData(charaData.charaName).clip);

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
    protected bool CheckCliff() {
        //�v���C���[���R���ォ���ɂ���Ȃ�
        if (transform.position.y > -1f || transform.position.y < -3f) {
            //�ȍ~�̏������s��Ȃ�
            return false;
        }

        //�v���C���[���R���O���ɂ���Ȃ�
        if (transform.position.x < -9f || transform.position.x > 9f) {
            //�ȍ~�̏������s��Ȃ�
            return false;
        }

        //true��Ԃ�
        return true;
    }

    /// <summary>
    /// �R�ɂ����݂�
    /// </summary>
    protected IEnumerator ClingingCliff() {
        //���ɊR����W�����v�����Ȃ�
        if (jumped) {
            //�ȍ~�̏������s��Ȃ�
            yield break;
        }

        //soundFlag��false�Ȃ�
        if (!soundFlag) {
            //���ʉ����Đ�
            yamap.SoundManager.instance.PlaySound(yamap.SoundManager.instance.GetCharacterVoiceData(charaData.charaName).clip);

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

        // NPC �Ȃ�
        if(ownerType == OwnerType.NPC) {
            // �v���C���[�̈ʒu�ƌ������R�̈ʒu�ƌ����ɍ��킹��
            AdjustmentPlayerToCliffTran();

            //1�b�҂�
            yield return new WaitForSeconds(1f);
        }

        soundFlag = false;    //  <=  false �ɂ��Ă���ꏊ���Ȃ������̂ŁA�����ɒǉ�

        // �W�����v�̌㏈��
        AfterJump();
    }

    /// <summary>
    /// �W�����v�̌㏈��
    /// </summary>
    protected virtual void AfterJump() {

        // �e�q�N���X�ŏ㏑�����Đݒ肷��
    }

    /// <summary>
    /// �v���C���[�̈ʒu�ƌ������R�̈ʒu�ƌ����ɍ��킹��
    /// AfterJump �̒��œK�X�ȃ^�C�~���O�Ŏ��s����
    /// </summary>
    protected void AdjustmentPlayerToCliffTran() {
        //�v���C���[���R�̈ʒu�Ɉړ�������
        transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        //�v���C���[�̌������R�ɍ��킹��
        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
    }
}
