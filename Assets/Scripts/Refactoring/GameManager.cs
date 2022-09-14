using System.Collections;//IEnumeratorを使用
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用
using Tsubasa;//CameraControllerを使用

namespace yamap 
{
    public class GameManager : MonoBehaviour 
    {
        [SerializeField]
        private UIManager uIManager;//UIManager

        [SerializeField]
        private CharacterManager characterManager;//CharacterManager

        [SerializeField]
        private CameraController cameraController;//CameraController

        [SerializeField]
        private Transform effectTran;//エフェクトの親

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
            SoundManager.instance.PlaySound(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Main).clip, true);

            //ゲームスタート演出が終わるまで待つ
            yield return uIManager.PlayGameStart();

            //モード選択画面への移行が終わるまで待つ
            yield return uIManager.SetModeSelect();

            //モード選択が終わるまで待つ
            yield return CheckModeSelect();

            //デュオなら
            if (!isSolo) {
                //BGMを切り替える
                SoundManager.instance.ChangeBgmMainToGame();

                //試合が始まるまで待つ
                yield return StartGame();

                //キャラクターの数だけ繰り返す
                for (int i = 0; i < 2; i++) 
                {
                    // 不要な方を削除する
                    Destroy(characterManager.charaList[i].charaControllerBase.GetComponent<NPCController>());

                    //CharacterControllerを取得
                    characterManager.charaList[i].charaControllerBase.GetComponent<CharacterController>().
                        //CharacterControllerの初期設定を行う
                        SetUpCharacterController(characterManager.GetCharaData((CharaName)i), OwnerType.Player);
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

            //使用キャラ、NPCキャラを取得
            (CharaName myChara, CharaName npcChara) useChara = useTamako ? (CharaName.Tamako, CharaName.Mashiro) : (CharaName.Mashiro, CharaName.Tamako);

            //プレイヤー側の設定
            {
                //myCharaのNPCControllerを削除
                Destroy(characterManager.GetCharaControllerBase(useChara.myChara).GetComponent<NPCController>());

                //myCharaCharacterControllerを設定
                characterManager.GetCharaControllerBase(useChara.myChara).GetComponent<CharacterController>().SetUpCharacterController(characterManager.GetCharaData(useChara.myChara), OwnerType.Player);

                //myCharaのHealthを設定
                characterManager.GetCharacterHealth(useChara.myChara).SetUpCharacterHealth(this, cameraController, effectTran, characterManager.GetCharaControllerBase(useChara.npcChara));
            }

            //NPC側の設定
            {
                //npcCharaのCharacterControllerを削除
                Destroy(characterManager.GetCharaControllerBase(useChara.npcChara).GetComponent<CharacterController>());

                //npcCharaのCharacterControllerを設定
                characterManager.GetCharaControllerBase(useChara.npcChara).GetComponent<NPCController>().SetUpCharacterController(characterManager.GetCharaData(useChara.npcChara), OwnerType.NPC, characterManager.GetCharaControllerBase(useChara.myChara));

                //npcCharaのHealthを設定
                characterManager.GetCharacterHealth(useChara.npcChara).SetUpCharacterHealth(this, cameraController, effectTran, characterManager.GetCharaControllerBase(useChara.myChara));
            }
        }

        /// <summary>
        /// 全てのコントローラーを非活性化する
        /// </summary>
        private void SetControllersFalse() 
        {
            //キャラクターの数だけ繰り返す
            for (int i = 0; i < characterManager.charaList.Count; i++) 
            {
                //取得したキャラクターのCharacterControlerBaseを非活性化する
                characterManager.charaList[i].charaControllerBase.enabled = false;
            }
        }

        /// <summary>
        /// どちらのモードを選択されたか確認する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator CheckModeSelect() 
        {
            //無限に繰り返す
            while (true) 
            {
                //「1」を押されたら
                if (Input.GetKeyDown(KeyCode.Alpha1)) 
                {
                    //選択音を再生する
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                    //選択されたモードを記憶する
                    isSolo = true;

                    //繰り返し処理を終了する
                    break;
                }
                //「2」を押されたら
                else if (Input.GetKeyDown(KeyCode.Alpha2)) 
                {
                    //選択音を再生する
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

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
                    SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.MashiroName).clip);

                    //選択音を再生する
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                    //選択されたキャラクターを記憶する
                    useTamako = false;

                    //繰り返し処理を終了する
                    break;
                }
                //「2」を押されたら
                else if (Input.GetKeyDown(KeyCode.Alpha2)) 
                {
                    //音声を再生
                    SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.TamakoName).clip);

                    //選択音を再生する
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

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
            SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.CountDown).clip);

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
            SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.GameSet).clip);

            //BGMをフェードアウトさせる
            SoundManager.instance.StopSound(0.5f);

            //ゲーム終了演出が終わるまで待つ
            yield return uIManager.EndGame();

            //Mainシーンを読み込む
            SceneManager.LoadScene("Main");
        }
    }
}