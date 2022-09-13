using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;//�C���X�^���X

    [SerializeField]
    private SoundDataSO soundDataSO;//SoundDataSO

    [SerializeField]
    private AudioSource mainAudioSource;//���C����AudioSource

    [SerializeField]
    private AudioSource subAudioSource;//�T�u��AudioSource

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȉ��A�V���O���g���ɕK�{�̋L�q
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �w�肵�����O��BGM�̃f�[�^��Ԃ�
    /// </summary>
    /// <param name="bgmName">BGM�̖��O</param>
    /// <returns>BGM�̃f�[�^</returns>
    public SoundDataSO.BgmData GetBgmData(SoundDataSO.BgmName bgmName)
    {
        //�w�肵�����O��BGM�̃f�[�^��Ԃ�
        return soundDataSO.bgmDataList.Find(x=>x.bgmName == bgmName);
    }

    /// <summary>
    /// �w�肵�����O�̌��ʉ��̃f�[�^��Ԃ�
    /// </summary>
    /// <param name="soundEffectName">���ʉ��̖��O</param>
    /// <returns>���ʉ��̃f�[�^</returns>
    public SoundDataSO.SoundEffectData GetSoundEffectData(SoundDataSO.SoundEffectName soundEffectName)
    {
        //�w�肵�����O�̌��ʉ��̃f�[�^��Ԃ�
        return soundDataSO.soundEffectDataList.Find(x=>x.soundEffectName == soundEffectName);
    }

    /// <summary>
    /// �w�肵�����O�̉����̃f�[�^��Ԃ�
    /// </summary>
    /// <param name="voiceName">�����̖��O</param>
    /// <returns>�����̃f�[�^</returns>
    public SoundDataSO.VoiceData GetVoiceData(SoundDataSO.VoiceName voiceName)
    {
        //�w�肵�����O�̉����̃f�[�^��Ԃ�
        return soundDataSO.voiceDataList.Find(x=>x.voiceName == voiceName);
    }

    /// <summary>
    /// �w�肵�����O�̃L�����N�^�[�̉����̃f�[�^���擾����
    /// </summary>
    /// <param name="charaName">�L�����N�^�[�̖��O</param>
    /// <returns>�w�肵�����O�̃L�����N�^�[�̉����̃f�[�^</returns>
    public SoundDataSO.CharacterVoiceData GetCharacterVoiceData(CharaName charaName)
    {
        //�w�肵�����O�̃L�����N�^�[�̉����̃f�[�^��Ԃ�
        return soundDataSO.characterVoiceDataList.Find(x=>x.charaName == charaName);
    }

    /// <summary>
    /// AudioSource���g���āA�����Đ�����
    /// </summary>
    /// <param name="clip">�N���b�v</param>
    /// <param name="loop">�J��Ԃ����ǂ���</param>
    /// <returns>�g�p����AudioSource</returns>
    public AudioSource PlaySoundByAudioSource(AudioClip clip,bool loop=false)
    {
        //�J��Ԃ��Ȃ�
        if(loop==true)
        {
            //�N���b�v��ݒ�
            mainAudioSource.clip= clip;

            //�J��Ԃ��悤�ɐݒ�
            mainAudioSource.loop= loop;

            //�����Đ�
            mainAudioSource.Play();

            //���C����AudioSource��Ԃ�
            return mainAudioSource;
        }

        //�����Đ�����
        subAudioSource.PlayOneShot(clip);

        //�g�p����AudioSource��Ԃ�
        return subAudioSource;
    }

    /// <summary>
    /// BGM��Main����Game�ɐ؂�ւ���
    /// </summary>
    private void ChangeBgm() {
        //BGM���t�F�[�h�A�E�g������
        mainAudioSource.DOFade(0f, 1f).

            //BGM��؂�ւ���
            OnComplete(() => {
                { mainAudioSource = SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Game).clip, true); }

                //BGM���t�F�[�h�C��������
                { mainAudioSource.DOFade(1f, 1f); }
            });
    }
}
