using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用
using DG.Tweening;//DOTweenを使用

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uIManager;//UIManager

    [SerializeField]
    private NPCController tamakoNpcController;//魂子用のNPCController

    [SerializeField]
    private NPCController mashiroNpcController;//真白用のNPCController

    private AudioSource audioSource;//使用しているAudioSource

    private bool isSolo;//ソロかどうか

    private bool useMashiro;//ソロプレーヤーが真白を使用するかどうか

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Start()
    {
        //マウスカーソルを非表示にする
        uIManager.HideCursor();

        //BGMを再生
        audioSource = SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Main).clip, true);

        //キャラクター用のスクリプトの初期設定を行う
        SetUpCharaScripts();

        //ゲームスタート演出が終わるまで待つ
        yield return StartCoroutine(uIManager.PlayGameStart());

        //モード選択画面への移行が終わるまで待つ
        yield return StartCoroutine(uIManager.SetModeSelect());

        //モード選択が終わるまで待つ
        yield return StartCoroutine(CheckModeSelect());

        //デュオなら
        if (!isSolo)
        {
            //BGMを切り替える
            ChangeBgm();

            //試合が始まるまで待つ
            yield return StartCoroutine(StartGame());

            //選択されたモード・キャラクターに応じて、キャラクター用のスクリプトの有効化を行う
            SetCharaScripts();

            //以降の処理を行わない
            yield break;
        }

        //キャラクター選択画面への移行が終わるまで待つ
        yield return StartCoroutine(uIManager.SetCharaSelect());

        //キャラクタ選択が終わるまで待つ
        yield return StartCoroutine(CheckCharaSelect());

        //BGMを切り替える
        ChangeBgm();

        //試合が始まるまで待つ
        yield return StartCoroutine(StartGame());

        //選択されたモード・キャラクターに応じて、キャラクター用のスクリプトの有効化を行う
        SetCharaScripts();
    }

    /// <summary>
    /// キャラクター用のスクリプトの初期設定を行う
    /// </summary>
    private void SetUpCharaScripts()
    {
        ////TamakoControllerを無効化
        //tamakoController.enabled = false;

        ////MashiroControllerを無効化
        //mashiroController.enabled=false;

        //魂子用のNPCControllerを無効化
        tamakoNpcController.enabled = false;

        //真白用のNPCControllerを無効化
        mashiroNpcController.enabled=false;
    }

    /// <summary>
    /// 選択されたモード・キャラクターに応じて、キャラクター用のスクリプトの有効化を行う
    /// </summary>
    private void SetCharaScripts()
    {
        //デュオなら
        if(!isSolo)
        {
            ////TamakoControllerを有効化
            //tamakoController.enabled = true;

            ////MashiroControllerを有効化
            //mashiroController.enabled = true;

            //以降の処理を行わない
            return;
        }

        //ソロプレーヤーが真白を使用するなら
        if (useMashiro)
        {
            ////MashiroControllerを有効化
            //mashiroController.enabled = true;

            //魂子用のNPCControllerを有効化
            tamakoNpcController.enabled = true;
        }
        //ソロプレーヤーが魂子を使用するなら
        else
        {
            ////TamakoControllerを有効化
            //tamakoController.enabled = true;

            //真白用のNPCControllerを有効化
            mashiroNpcController.enabled = true;
        }
    }

    /// <summary>
    /// どちらのモードを選択されたか確認する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator CheckModeSelect()
    {
        //無限に繰り返す
        while(true)
        {
            //「1」を押されたら
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //選択音を再生する
                PlaySelectSound();

                //選択されたモードを記憶する
                isSolo = true;

                //繰り返し処理を終了する
                break;
            }
            //「2」を押されたら
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //選択音を再生する
                PlaySelectSound();

                //選択されたモードを記憶する
                isSolo = false;

                //繰り返し処理を終了する
                break;
            }

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// どちらのキャラクターを選択されたか確認する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator CheckCharaSelect()
    {
        //無限に繰り返す
        while (true)
        {
            //「1」を押されたら
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //音声を再生
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.MashiroName).clip);

                //選択音を再生する
                PlaySelectSound();

                //選択されたキャラクターを記憶する
                useMashiro = true;

                //繰り返し処理を終了する
                break;
            }
            //「2」を押されたら
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //音声を再生
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.TamakoName).clip);

                //選択音を再生する
                PlaySelectSound();

                //選択されたキャラクターを記憶する
                useMashiro = false;

                //繰り返し処理を終了する
                break;
            }

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// 試合を開始する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator StartGame()
    {
        //試合画面への移行が終わるまで待つ
        yield return StartCoroutine(uIManager.GoToGame());

        //音声を再生
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.CountDown).clip);

        //試合前のカウントダウンが終わるまで待つ
        yield return StartCoroutine(uIManager.CountDown());
    }

    /// <summary>
    /// ゲーム終了の準備を行う
    /// </summary>
    public void SetUpEndGame()
    {
        //ゲームを終了する
        StartCoroutine(EndGame());
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator EndGame()
    {
        //音声を再生
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.GameSet).clip);

        //BGMをフェードアウトさせる
        audioSource.DOFade(0f, 1f);

        //ゲーム終了演出が終わるまで待つ
        yield return StartCoroutine(uIManager.EndGame());

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 選択音を再生する
    /// </summary>
    private void PlaySelectSound()
    {
        //選択音を再生する
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);
    }

    /// <summary>
    /// BGMをMainからGameに切り替える
    /// </summary>
    private void ChangeBgm()
    {
        //BGMをフェードアウトさせる
        audioSource.DOFade(0f, 1f).

            //BGMを切り替える
            OnComplete(() =>
            {
                { audioSource = SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Game).clip, true); }

                //BGMをフェードインさせる
                { audioSource.DOFade(1f, 1f); }
            });
    }
}
