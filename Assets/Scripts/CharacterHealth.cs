using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private Transform parentTran;//�G�t�F�N�g�̐e

    [SerializeField]
    private GameObject enemy;//�G

    private GameManager gameManager;//GameManager

    private CameraController cameraController;//CameraController

    private float damage=0f;//�~�σ_���[�W�i�����l��0�j

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //���g�������͈͓��ɂ�����
        if (CheckGameRange())
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�G�����Ɏ��S���Ă���Ȃ�
        if (enemy == null)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���S�������s��
        KillMe();
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

            //�G�ꂽ����𖳌����i�d�������h�~�j
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// CharacterHealth�̏����ݒ���s��
    /// </summary>
    /// <param name="gameManager">GameManager</param>
    public void SetUpCharacterHealth(GameManager gameManager,CameraController cameraController)
    {
        //GameManager���擾
        this.gameManager = gameManager;

        //CameraController��ݒ�
        this.cameraController = cameraController;
    }

    /// <summary>
    /// �U�����ꂽ�ۂ̏������s��
    /// </summary>
    /// <param name="enemyTran">�U������̈ʒu���</param>
    private void Attacked(Transform enemyTran)
    {
        //�_���[�W�𑝂₷
        damage += 10f;

        //���ʉ����Đ�
        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Explosion).clip);

        //�U�����肪���g��荶�ɂ���Ȃ�
        if (enemyTran.position.x > transform.position.x)
        {
            //���ɐ�����΂����
            transform.DOMoveX(transform.position.x - (damage * GameData.instance.powerRatio), 0.5f);
        }
        //�U�����肪���g���E�ɂ���Ȃ�
        else if (enemyTran.position.x < transform.position.x)
        {
            //������΂����
            transform.DOMoveX(transform.position.x + (damage * GameData.instance.powerRatio), 0.5f);
        }

        //��ɐ�����΂����
        transform.DOMoveY(transform.position.y + (damage * GameData.instance.powerRatio), 0.5f);

        //�G�t�F�N�g�𐶐�
        GameObject effect = Instantiate(GameData.instance.attackEffect, enemyTran.position, Quaternion.identity,parentTran);

        //��莞�Ԍ�ɁA���������G�t�F�N�g������   
        Destroy(effect, 1f);
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
        //���ʉ����Đ�
        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Dead).clip);

        //�G�t�F�N�g�𐶐�
        GameObject effect = Instantiate(GameData.instance.deadEffect,transform.position,Quaternion.identity,parentTran);

        //��莞�Ԍ�ɁA���������G�t�F�N�g������   
        Destroy(effect, 1f);

        //�Q�[���I���̏������s��
        gameManager.SetUpEndGame();

        //�J�����̑Ώە��̃��X�g���玩�g���폜����
        cameraController.targetTransList.Remove(transform);

        //���g������
        Destroy(gameObject);
    }
}
