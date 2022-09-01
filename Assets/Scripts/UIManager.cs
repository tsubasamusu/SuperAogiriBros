using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image imgBackground;//背景

    [SerializeField]
    private Image imgLogo;//ロゴ

    [SerializeField]
    private Image imgPicture;//PNG表示用

    [SerializeField]
    private Text txtMessage;//メッセージ

    [SerializeField]
    private Sprite sprTitle;//タイトル

    [SerializeField]
    private Sprite sprModeSelect;//モード選択画面

    [SerializeField]
    private Sprite sprCharaSelect;//キャラクター選択画面

    private float time = 3.5f;//カウントダウン用

    private IEnumerator Start()
    {
        yield return StartCoroutine(PlayGameStart());

        yield return StartCoroutine(SetModeSelect());

        yield return StartCoroutine(SetCharaSelect());

        yield return StartCoroutine(GoToGame());

        yield return StartCoroutine(CountDown());

        Debug.Log("GameStart");

        yield return StartCoroutine(EndGame());

        Debug.Log("End");
    }

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameStart()
    {
        //ゲームスタート演出が終わったかどうか
        bool endGameStart = false;

        //ロゴをタイトルに設定
        imgLogo.sprite = sprTitle;

        //ロゴを非表示にする
        imgLogo.DOFade(0f, 0f);

        //背景を白色に設定
        imgBackground.color = Color.white;

        //背景を表示
        imgBackground.DOFade(1f, 0f);

        //ロゴを表示して、消す
        imgLogo.DOFade(1f, 1f).SetLoops(2,LoopType.Yoyo).OnComplete(() => { endGameStart = true; });

        //ゲームスタート演出が終わるまで待つ
        yield return new WaitUntil(()=>endGameStart);
    }

    /// <summary>
    /// モード選択画面を表示する
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator SetModeSelect()
    {
        //演出が終わったかどうか
        bool end=false;

        //モード選択画面の画像を設定
        imgPicture.sprite = sprModeSelect;

        //設定した画像を表示
        imgPicture.DOFade(1f,1f).OnComplete(() => end = true);

        //演出が終わるまで待つ
        yield return new WaitUntil(() => end);
    }

    /// <summary>
    /// キャラクター選択画面を表示する
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator SetCharaSelect()
    {
        //演出が終わったかどうか
        bool end = false;

        //モード選択画面を非表示にする
        imgPicture.DOFade(0f, 0.5f).

            //画面をキャラクター選択画面に設定し、表示
            OnComplete(() =>{ { imgPicture.sprite = sprCharaSelect; }{ imgPicture.DOFade(1f, 0.5f).OnComplete(()=>end=true); } });

        //演出が終わるまで待つ
        yield return new WaitUntil(()=>end);
    }

    /// <summary>
    /// 試合画面へ以降する
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator GoToGame()
    {
        //演出が終わったかどうか
        bool end = false;

        //選択画面を非表示にする
        imgPicture.DOFade(0f, 1f);

        //背景を透明にする
        imgBackground.DOFade(0f, 1f).OnComplete(()=>end=true);

        //演出が終わるまで待つ
        yield return new WaitUntil(()=>end);
    }

    /// <summary>
    /// 試合開始前のカウントダウンを行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator CountDown()
    {
        //メッセージの色を緑色に設定
        txtMessage.color = Color.green;

        //メッセージを表示する
        txtMessage.DOFade(1f, 0f);

        //無限に繰り返す
        while(true)
        {
            //カウントダウンする
            time -= Time.deltaTime;

            //カウントダウンが0.5以下になったら
            if(time<=0.5f)
            {
                //GOと表示する
                txtMessage.text = "GO";

                //メッセージを非表示にする
                txtMessage.DOFade(0f, 1f).OnComplete(() => txtMessage.text = "");

                //繰り返し処理を終了する
                break;
            }

            //カウントダウンを表示する
            txtMessage.text = time.ToString("F0");

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// 「Game Set」と表示する
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator EndGame()
    {
        //演出が終わったかどうか
        bool end = false;

        //メッセージの色を青色に設定
        txtMessage.color = Color.blue;

        //「Game Set」という文字を設定する
        txtMessage.text = "Game Set";

        //文字を表示して消す
        txtMessage.DOFade(1f, 0.5f).OnComplete(()=>txtMessage.DOFade(0f,1f).OnComplete(() => end = true));

        //演出が終わるまで待つ
        yield return new WaitUntil(() => end);
    }
}
