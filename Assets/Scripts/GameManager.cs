using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p
using DG.Tweening;//DOTween���g�p

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uIManager;//UIManager

    [SerializeField]
    private NPCController tamakoNpcController;//���q�p��NPCController

    [SerializeField]
    private NPCController mashiroNpcController;//�^���p��NPCController

    private AudioSource audioSource;//�g�p���Ă���AudioSource

    private bool isSolo;//�\�����ǂ���

    private bool useMashiro;//�\���v���[���[���^�����g�p���邩�ǂ���

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�}�E�X�J�[�\�����\���ɂ���
        uIManager.HideCursor();

        //BGM���Đ�
        audioSource = SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Main).clip, true);

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
            //BGM��؂�ւ���
            ChangeBgm();

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

        //BGM��؂�ւ���
        ChangeBgm();

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
        ////TamakoController�𖳌���
        //tamakoController.enabled = false;

        ////MashiroController�𖳌���
        //mashiroController.enabled=false;

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
            ////TamakoController��L����
            //tamakoController.enabled = true;

            ////MashiroController��L����
            //mashiroController.enabled = true;

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�\���v���[���[���^�����g�p����Ȃ�
        if (useMashiro)
        {
            ////MashiroController��L����
            //mashiroController.enabled = true;

            //���q�p��NPCController��L����
            tamakoNpcController.enabled = true;
        }
        //�\���v���[���[�����q���g�p����Ȃ�
        else
        {
            ////TamakoController��L����
            //tamakoController.enabled = true;

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
                //�I�������Đ�����
                PlaySelectSound();

                //�I�����ꂽ���[�h���L������
                isSolo = true;

                //�J��Ԃ��������I������
                break;
            }
            //�u2�v�������ꂽ��
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //�I�������Đ�����
                PlaySelectSound();

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
                //�������Đ�
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.MashiroName).clip);

                //�I�������Đ�����
                PlaySelectSound();

                //�I�����ꂽ�L�����N�^�[���L������
                useMashiro = true;

                //�J��Ԃ��������I������
                break;
            }
            //�u2�v�������ꂽ��
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //�������Đ�
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.TamakoName).clip);

                //�I�������Đ�����
                PlaySelectSound();

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

        //�������Đ�
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.CountDown).clip);

        //�����O�̃J�E���g�_�E�����I���܂ő҂�
        yield return StartCoroutine(uIManager.CountDown());
    }

    /// <summary>
    /// �Q�[���I���̏������s��
    /// </summary>
    public void SetUpEndGame()
    {
        //�Q�[�����I������
        StartCoroutine(EndGame());
    }

    /// <summary>
    /// �Q�[�����I������
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator EndGame()
    {
        //�������Đ�
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.GameSet).clip);

        //BGM���t�F�[�h�A�E�g������
        audioSource.DOFade(0f, 1f);

        //�Q�[���I�����o���I���܂ő҂�
        yield return StartCoroutine(uIManager.EndGame());

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �I�������Đ�����
    /// </summary>
    private void PlaySelectSound()
    {
        //�I�������Đ�����
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);
    }

    /// <summary>
    /// BGM��Main����Game�ɐ؂�ւ���
    /// </summary>
    private void ChangeBgm()
    {
        //BGM���t�F�[�h�A�E�g������
        audioSource.DOFade(0f, 1f).

            //BGM��؂�ւ���
            OnComplete(() =>
            {
                { audioSource = SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Game).clip, true); }

                //BGM���t�F�[�h�C��������
                { audioSource.DOFade(1f, 1f); }
            });
    }
}
