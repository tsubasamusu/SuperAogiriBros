using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uIManager;//UIManager

    [SerializeField]
    private TamakoController tamakoController;//TamakoController

    [SerializeField]
    private MashiroController mashiroController;//MashiroController

    [SerializeField]
    private NPCController tamakoNpcController;//���q�p��NPCController

    [SerializeField]
    private NPCController mashiroNpcController;//�^���p��NPCController

    private bool isSolo;//�\�����ǂ���

    private bool useMashiro;//�\���v���[���[���^�����g�p���邩�ǂ���

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�L�����N�^�[�p�̃X�N���v�g�̏����ݒ���s��
        SetUpCharaScripts();

        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return StartCoroutine(uIManager.PlayGameStart());

        //���[�h�I����ʂւ̈ڍs���I���܂ő҂�
        yield return StartCoroutine(uIManager.SetModeSelect());

        //���[�h�I�����I���܂ő҂�
        yield return StartCoroutine(CheckModeSelect());

        //�f���I�Ȃ�
        if (!isSolo)
        {
            //�������n�܂�܂ő҂�
            yield return StartCoroutine(StartGame());

            //�I�����ꂽ���[�h�E�L�����N�^�[�ɉ����āA�L�����N�^�[�p�̃X�N���v�g�̗L�������s��
            SetCharaScripts();

            //�ȍ~�̏������s��Ȃ�
            yield break;
        }

        //�L�����N�^�[�I����ʂւ̈ڍs���I���܂ő҂�
        yield return StartCoroutine(uIManager.SetCharaSelect());

        //�L�����N�^�I�����I���܂ő҂�
        yield return StartCoroutine(CheckCharaSelect());

        //�������n�܂�܂ő҂�
        yield return StartCoroutine(StartGame());

        //�I�����ꂽ���[�h�E�L�����N�^�[�ɉ����āA�L�����N�^�[�p�̃X�N���v�g�̗L�������s��
        SetCharaScripts();
    }

    /// <summary>
    /// �L�����N�^�[�p�̃X�N���v�g�̏����ݒ���s��
    /// </summary>
    private void SetUpCharaScripts()
    {
        //TamakoController�𖳌���
        tamakoController.enabled = false;

        //MashiroController�𖳌���
        mashiroController.enabled=false;

        //���q�p��NPCController�𖳌���
        tamakoNpcController.enabled = false;

        //�^���p��NPCController�𖳌���
        mashiroNpcController.enabled=false;
    }

    /// <summary>
    /// �I�����ꂽ���[�h�E�L�����N�^�[�ɉ����āA�L�����N�^�[�p�̃X�N���v�g�̗L�������s��
    /// </summary>
    private void SetCharaScripts()
    {
        //�f���I�Ȃ�
        if(!isSolo)
        {
            //TamakoController��L����
            tamakoController.enabled = true;

            //MashiroController��L����
            mashiroController.enabled = true;

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�\���v���[���[���^�����g�p����Ȃ�
        if (useMashiro)
        {
            //MashiroController��L����
            mashiroController.enabled = true;

            //���q�p��NPCController��L����
            tamakoNpcController.enabled = true;
        }
        //�\���v���[���[�����q���g�p����Ȃ�
        else
        {
            //TamakoController��L����
            tamakoController.enabled = true;

            //�^���p��NPCController��L����
            mashiroNpcController.enabled = true;
        }
    }

    /// <summary>
    /// �ǂ���̃��[�h��I�����ꂽ���m�F����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator CheckModeSelect()
    {
        //�����ɌJ��Ԃ�
        while(true)
        {
            //�u1�v�������ꂽ��
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //�I�����ꂽ���[�h���L������
                isSolo = true;

                //�J��Ԃ��������I������
                break;
            }
            //�u2�v�������ꂽ��
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //�I�����ꂽ���[�h���L������
                isSolo = false;

                //�J��Ԃ��������I������
                break;
            }

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// �ǂ���̃L�����N�^�[��I�����ꂽ���m�F����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator CheckCharaSelect()
    {
        //�����ɌJ��Ԃ�
        while (true)
        {
            //�u1�v�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //�I�����ꂽ�L�����N�^�[���L������
                useMashiro = true;

                //�J��Ԃ��������I������
                break;
            }
            //�u2�v�������ꂽ��
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //�I�����ꂽ�L�����N�^�[���L������
                useMashiro = false;

                //�J��Ԃ��������I������
                break;
            }

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// �������J�n����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator StartGame()
    {
        //������ʂւ̈ڍs���I���܂ő҂�
        yield return StartCoroutine(uIManager.GoToGame());

        //�����O�̃J�E���g�_�E�����I���܂ő҂�
        yield return StartCoroutine(uIManager.CountDown());
    }
}
