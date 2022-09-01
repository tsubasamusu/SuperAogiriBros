using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

public GameObject attackEffect;//�U�������������ۂ̃G�t�F�N�g

    public GameObject deadEffect;//���ʍۂ̃G�t�F�N�g

    public float powerRatio;//�_���[�W�䗦

    public float damageTime;//������΂���鎞��

    public float moveSpeed;//�ړ����x

    public float jumpPower;//�W�����v�̗�

    [Tooltip("�R���畜������Ƃ��̃W�����v��")]
    public float jumpHeight;//�R���畜������Ƃ��̃W�����v�̍���

    [Tooltip("�R�ɂ����݂��Ă����鎞��")]
    public float maxCliffTime;//�R�ɂ����݂��Ă����鎞��

    public float npcMoveSpeed;//NPC�̈ړ����x

    public float npcJumpPower;//NPC�̃W�����v��

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȉ��A�V���O���g���ɕK�{�̋L�q
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
