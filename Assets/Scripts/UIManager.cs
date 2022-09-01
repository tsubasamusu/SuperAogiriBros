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
    private Sprite sprTitle;//タイトル

    [SerializeField]
    private Sprite sprModeSelect;//モード選択画面

    [SerializeField]
    private Sprite sprCharaSelect;//キャラクター選択画面

    private IEnumerator Start()
    {
        yield return StartCoroutine(PlayGameStart());

        yield return StartCoroutine(SetModeSelect());

        yield return StartCoroutine(SetCharaSelect());

        yield return StartCoroutine(GoToGame());

        Debug.Log("end");
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
}
