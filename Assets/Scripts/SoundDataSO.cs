using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

//�A�Z�b�g���j���[�ŁuCreate SoundDataSO�v��I������ƁuSoundDataSO�v������
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// BGM�̎��
    /// </summary>
    public enum BgmType
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
        public BgmType bgmType;//BGM�̎��
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
        MashiroName,//�^���̖��O
        TamakoName,//���q�̖��O

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
}
