using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;//インスタンス

    [SerializeField]
    private SoundDataSO soundDataSO;//SoundDataSO

    [SerializeField]
    private AudioSource mainAudioSource;//メインのAudioSource

    [SerializeField]
    private AudioSource subAudioSource;//サブのAudioSource

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    private void Awake()
    {
        //以下、シングルトンに必須の記述
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
    /// 指定した名前のBGMのデータを返す
    /// </summary>
    /// <param name="bgmName">BGMの名前</param>
    /// <returns>BGMのデータ</returns>
    public SoundDataSO.BgmData GetBgmData(SoundDataSO.BgmName bgmName)
    {
        //指定した名前のBGMのデータを返す
        return soundDataSO.bgmDataList.Find(x=>x.bgmName == bgmName);
    }

    /// <summary>
    /// 指定した名前の効果音のデータを返す
    /// </summary>
    /// <param name="soundEffectName">効果音の名前</param>
    /// <returns>効果音のデータ</returns>
    public SoundDataSO.SoundEffectData GetSoundEffectData(SoundDataSO.SoundEffectName soundEffectName)
    {
        //指定した名前の効果音のデータを返す
        return soundDataSO.soundEffectDataList.Find(x=>x.soundEffectName == soundEffectName);
    }

    /// <summary>
    /// 指定した名前の音声のデータを返す
    /// </summary>
    /// <param name="voiceName">音声の名前</param>
    /// <returns>音声のデータ</returns>
    public SoundDataSO.VoiceData GetVoiceData(SoundDataSO.VoiceName voiceName)
    {
        //指定した名前の音声のデータを返す
        return soundDataSO.voiceDataList.Find(x=>x.voiceName == voiceName);
    }

    /// <summary>
    /// AudioSourceを使って、音を再生する
    /// </summary>
    /// <param name="clip">クリップ</param>
    /// <param name="loop">繰り返すかどうか</param>
    /// <returns>使用したAudioSource</returns>
    public AudioSource PlaySoundByAudioSource(AudioClip clip,bool loop=false)
    {
        //繰り返すなら
        if(loop==true)
        {
            //クリップを設定
            mainAudioSource.clip= clip;

            //繰り返すように設定
            mainAudioSource.loop= loop;

            //音を再生
            mainAudioSource.Play();

            //メインのAudioSourceを返す
            return mainAudioSource;
        }

        //音を再生する
        subAudioSource.PlayOneShot(clip);

        //使用したAudioSourceを返す
        return subAudioSource;
    }
}
