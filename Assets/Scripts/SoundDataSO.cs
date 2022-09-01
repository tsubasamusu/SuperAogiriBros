using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

//アセットメニューで「Create SoundDataSO」を選択すると「SoundDataSO」を作れる
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// BGMの種類
    /// </summary>
    public enum BgmType
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
        public BgmType bgmType;//BGMの種類
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
        MashiroName,//真白の名前
        TamakoName,//魂子の名前

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
}
