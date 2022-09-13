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
    private CharacterManager characterManager;//CharacterManager

    private bool isSolo;//ソロかどうか

    private bool useTamako;//ソロプレーヤーが魂子を使用するかどうか

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Start()
    {
        //全てのコントローラーを非活性化する（試合前にキャラクターが動かないようにするため）
        SetControllersFalse();

        //マウスカーソルを非表示にする
        uIManager.HideCursor();

        //BGMを再生
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Main).clip, true);

        //ゲームスタート演出が終わるまで待つ
        yield return uIManager.PlayGameStart();

        //モード選択画面への移行が終わるまで待つ
        yield return uIManager.SetModeSelect();

        //モード選択が終わるまで待つ
        yield return CheckModeSelect();

        //デュオなら
        if (!isSolo)
        {
            //BGMを切り替える
            SoundManager.instance.ChangeBgmMainToGame();

            //試合が始まるまで待つ
            yield return StartGame();

            //2回繰り返す
            for(int i=0;i<2;i++)
            {
                //CharacterControllerを活性化する
                characterManager.GetCharacterController((CharacterManager.CharaName)i).enabled = true;

                //CharacterControllerの初期設定を行う
                characterManager.GetCharacterController((CharacterManager.CharaName)i).SetUpCharacterController(characterManager);
            }

            //以降の処理を行わない
            yield break;
        }

        //キャラクター選択画面への移行が終わるまで待つ
        yield return uIManager.SetCharaSelect();

        //キャラクタ選択が終わるまで待つ
        yield return CheckCharaSelect();

        //BGMを切り替える
        SoundManager.instance.ChangeBgmMainToGame();

        //試合が始まるまで待つ
        yield return StartGame();

        //ソロプレーヤーが魂子を使用するなら
        if(useTamako)
        {
            //魂子のCharacterControllerを活性化する
            characterManager.GetCharacterController(CharacterManager.CharaName.Tamako).enabled = true;

            //CharacterControllerの初期設定を行う
            characterManager.GetCharacterController(CharacterManager.CharaName.Tamako).SetUpCharacterController(characterManager);

            //真白のNPCControllerを活性化する
            characterManager.GetNpcController(CharacterManager.CharaName.Mashiro).enabled = true;
        }
        //ソロプレーヤーが真白を使用するなら
        else
        {
            //真白のCharacterControllerを活性化する
            characterManager.GetCharacterController(CharacterManager.CharaName.Mashiro).enabled = true;

            //CharacterControllerの初期設定を行う
            characterManager.GetCharacterController(CharacterManager.CharaName.Mashiro).SetUpCharacterController(characterManager);

            //魂子のNPCControllerを活性化する
            characterManager.GetNpcController(CharacterManager.CharaName.Tamako).enabled = true;
        }
    }

    /// <summary>
    /// 全てのコントローラーを非活性化する
    /// </summary>
    private void SetControllersFalse()
    {
        //2回繰り返す
        for(int i = 0; i < 2; i++)
        {
            //CharacterControllerを非活性化する
            characterManager.GetCharacterController((CharacterManager.CharaName)i).enabled=false;

            //NPCControllerを非活性化する
            characterManager.GetNpcController((CharacterManager.CharaName)i).enabled=false;
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
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                //選択されたモードを記憶する
                isSolo = true;

                //繰り返し処理を終了する
                break;
            }
            //「2」を押されたら
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //選択音を再生する
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

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
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                //選択されたキャラクターを記憶する
                useTamako = false;

                //繰り返し処理を終了する
                break;
            }
            //「2」を押されたら
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //音声を再生
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.TamakoName).clip);

                //選択音を再生する
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                //選択されたキャラクターを記憶する
                useTamako = true;

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
        yield return uIManager.GoToGame();

        //音声を再生
        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.CountDown).clip);

        //試合前のカウントダウンが終わるまで待つ
        yield return uIManager.CountDown();
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
        SoundManager.instance.StopSound(0.5f);

        //ゲーム終了演出が終わるまで待つ
        yield return uIManager.EndGame();

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }
}
