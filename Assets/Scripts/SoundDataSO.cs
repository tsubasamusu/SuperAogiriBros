using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

//�A�Z�b�g���j���[�ŁuCreate SoundDataSO�v��I������ƁuSoundDataSO�v������
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// BGM�̖��O
    /// </summary>
    public enum BgmName
    {
        Main,//�����ȊO�ŗ����BGM
        Game//�������ɗ����BGM
    }

    /// <summary>
    /// BGM�̃f�[�^���Ǘ�����
    /// </summary>
    [Serializable]
    public class BgmData
    {�@�@�@�@�@�@�@�@
        public BgmName bgmType;//BGM�̖��O
        public AudioClip clip;//�N���b�v
    }

    //BGM�̃f�[�^�̃��X�g
    public List<BgmData> bgmDataList = new List<BgmData>();

    /// <summary>
    /// ���ʉ��̖��O
    /// </summary>
    public enum SoundEffectName
    {
        Select,//�I����
        Cliff,//�R�ɒ͂܂�Ƃ��̉�
        jump,//�W�����v����Ƃ��̉�
        Explosion,//������
        Dead,//���S��
    }

    /// <summary>
    /// ���ʉ��̃f�[�^���Ǘ�����
    /// </summary>
    [Serializable]
    public class SoundEffectData
    {
        public SoundEffectName soundEffectName;//���ʉ��̖��O
        public AudioClip clip;//�N���b�v
    }

    //���ʉ��̃f�[�^�̃��X�g
    public List <SoundEffectData> soundEffectDataList = new List<SoundEffectData>();

    /// <summary>
    /// �����̖��O
    /// </summary>
    public enum VoiceName
    {
        MashiroName,//�^���̖��O
        TamakoName,//���q�̖��O
        MashiroVoice,//�^�����U������Ƃ��̐�
        TamakoVoice,//���q���U������ۂ̐�
        CountDown,//�����J�n�O�̃J�E���g�_�E��
        GameSet,//�uGaneSet�v
    }

    /// <summary>
    /// �����̃f�[�^���Ǘ�����
    /// </summary>
    [Serializable]
    public class VoiceData
    {
        public VoiceName voiceName;//�����̖��O
        public AudioClip clip;//�N���b�v
    }

    //�����̃f�[�^�̃��X�g
    public List<VoiceData> voiceDataList = new List<VoiceData>();
}
