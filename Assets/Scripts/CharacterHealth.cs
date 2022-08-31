using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private Transform forceTran;//������΂���鎞�ɗ͂�������ʒu

    private float damage=0f;//�~�σ_���[�W�i�����l��0�j

    private float forcePower;//������΂���鎞�ɉ����͂̑傫��

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
            Attacked();
        }
    }

    /// <summary>
    /// �U�����ꂽ�ۂ̏���
    /// </summary>
    private void Attacked()
    {
        //�_���[�W�𑝂₷
        damage += 10f;


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
