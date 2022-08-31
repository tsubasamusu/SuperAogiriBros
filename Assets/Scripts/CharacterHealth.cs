using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private CameraController cameraController;//CameraController

    private float damage=0f;//�~�σ_���[�W�i�����l��0�j

    [SerializeField]
    private float powerRatio;//�_���[�W�䗦�i���j

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //���g�������͈͓��ɂ��Ȃ�������
        if(!CheckGameRange())
        {
            //���S�������s��
            KillMe();
        }
    }

    /// <summary>
    /// ���̃R���C�_�[�����蔲�����ۂɌĂяo�����
    /// </summary>
    /// <param name="other">�G�ꂽ����</param>
    private void OnTriggerEnter(Collider other)
    {
        //�G�ꂽ���肪�G�̃A�^�b�N�|�C���g�Ȃ�
        if(other.gameObject.CompareTag("AttackPoint"))
        {
            //�U�����󂯂��ۂ̏������s��
            Attacked(other.transform);
        }
    }

    /// <summary>
    /// �U�����ꂽ�ۂ̏������s��
    /// </summary>
    /// <param name="enemyTran">�U������̈ʒu���</param>
    private void Attacked(Transform enemyTran)
    {
        //�_���[�W�𑝂₷
        damage += 10f;

        //TODO:GameData����u�_���[�W�䗦�v���擾���鏈��

        //�͂�������
        rb.AddForce(enemyTran.forward * damage * powerRatio, ForceMode.Impulse);
    }

    /// <summary>
    /// �_���[�W���擾����
    /// </summary>
    /// <returns>�_���[�W</returns>
    public float GetDamage()
    {
        //�_���[�W�̒l��Ԃ�
        return damage;
    }

    /// <summary>
    /// ���g�������͈͓��ɂ��邩�ǂ������ׂ�
    /// </summary>
    /// <returns>���g�������͈͓��ɂ�����true</returns>
    private bool CheckGameRange()
    {
        //���g�������͈͓��ɂ�����
        if(transform.position.x<=15f&&transform.position.x>=-15f&& transform.position.y <= 7f && transform.position.y >= -7f)
        {
            //true��Ԃ�
            return true;
        }

        //false��Ԃ�
        return false;
    }

    /// <summary>
    /// ���S�������s��
    /// </summary>
    private void KillMe()
    {
        //�J�����̑Ώە��̃��X�g���玩�g���폜����
        cameraController.targetTransList.Remove(transform);

        //���g������
        Destroy(gameObject);
    }
}
