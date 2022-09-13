using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

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
    /// 指定した名前のキャラクターの音声のデータを取得する
    /// </summary>
    /// <param name="charaName">キャラクターの名前</param>
    /// <returns>指定した名前のキャラクターの音声のデータ</returns>
    public SoundDataSO.CharacterVoiceData GetCharacterVoiceData(CharacterManager.CharaName charaName)
    {
        //指定した名前のキャラクターの音声のデータを返す
        return soundDataSO.characterVoiceDataList.Find(x=>x.charaName == charaName);
    }

    /// <summary>
    /// AudioSourceを使って、音を再生する
    /// </summary>
    /// <param name="clip">クリップ</param>
    /// <param name="loop">繰り返すかどうか</param>
    public void PlaySoundByAudioSource(AudioClip clip,bool loop=false)
    {
        //繰り返すなら
        if (loop == true)
        {
            //クリップを設定
            mainAudioSource.clip = clip;

            //繰り返すように設定
            mainAudioSource.loop = loop;

            //音を再生
            mainAudioSource.Play();
        }
        //繰り返さないなら
        else
        {
            //音を再生する
            subAudioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 音を止める
    /// </summary>
    /// <param name="time">フェードアウト時間</param>
    /// <param name="isMain">メインのAudioSourceで再生されている音かどうか</param>
    public void StopSound(float time,bool isMain=true)
    {
        //メインのAudioSourceで再生されている音を止めるなら
        if (isMain)
        {
            //メインのAudioSourceで再生されている音をフェードアウトさせる
            mainAudioSource.DOFade(0f, time);
        }
        //サブのAudioSourceで再生されている音を止めるなら
        else
        {
            //サブのAudioSourceで再生されている音をフェードアウトさせる
            subAudioSource.DOFade(0f, time);
        }
    }

    /// <summary>
    /// BGMをMainからGameに切り替える
    /// </summary>
    public void ChangeBgmMainToGame()
    {
        //メインのAudioSourceに設定されている音がMainではないなら（MainのBGMが流れていないなら）
        if (mainAudioSource.clip != soundDataSO.bgmDataList.Find(x => x.bgmName == SoundDataSO.BgmName.Main).clip)
        {
            //以降の処理を行わない
            return;
        }

        //BGMをフェードアウトさせる
        mainAudioSource.DOFade(0f, 1f).

            //BGMを切り替える
            OnComplete(() =>
            {
                { PlaySoundByAudioSource(GetBgmData(SoundDataSO.BgmName.Game).clip, true); }

                //BGMをフェードインさせる
                { mainAudioSource.DOFade(1f, 1f); }
            });
    }
}
