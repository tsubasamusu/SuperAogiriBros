using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    private float damage=0f;//�~�σ_���[�W�i�����l��0�j

    private float forcePower;//������΂���鎞�ɉ����͂̑傫��

    [SerializeField]
    private float powerRatio;//�_���[�W�䗦�i���j

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
}
