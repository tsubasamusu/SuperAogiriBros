using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

//アセットメニューで「Create SoundDataSO」を選択すると「SoundDataSO」を作れる
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// BGMの名前
    /// </summary>
    public enum BgmName
    {
        Main,//試合以外で流れるBGM
        Game//試合中に流れるBGM
    }

    /// <summary>
    /// BGMのデータを管理する
    /// </summary>
    [Serializable]
    public class BgmData
    {　　　　　　　　
        public BgmName bgmType;//BGMの名前
        public AudioClip clip;//クリップ
    }

    //BGMのデータのリスト
    public List<BgmData> bgmDataList = new List<BgmData>();

    /// <summary>
    /// 効果音の名前
    /// </summary>
    public enum SoundEffectName
    {
        Select,//選択音
        Cliff,//崖に掴まるときの音
        jump,//ジャンプするときの音
        Explosion,//爆発音
        Dead,//死亡音
    }

    /// <summary>
    /// 効果音のデータを管理する
    /// </summary>
    [Serializable]
    public class SoundEffectData
    {
        public SoundEffectName soundEffectName;//効果音の名前
        public AudioClip clip;//クリップ
    }

    //効果音のデータのリスト
    public List <SoundEffectData> soundEffectDataList = new List<SoundEffectData>();

    /// <summary>
    /// 音声の名前
    /// </summary>
    public enum VoiceName
    {
        MashiroName,//真白の名前
        TamakoName,//魂子の名前
        MashiroVoice,//真白が攻撃するときの声
        TamakoVoice,//魂子が攻撃する際の声
        CountDown,//試合開始前のカウントダウン
        GameSet,//「GaneSet」
    }

    /// <summary>
    /// 音声のデータを管理する
    /// </summary>
    [Serializable]
    public class VoiceData
    {
        public VoiceName voiceName;//音声の名前
        public AudioClip clip;//クリップ
    }

    //音声のデータのリスト
    public List<VoiceData> voiceDataList = new List<VoiceData>();
}
