using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用
using Tsubasa;//CameraControllerを使用

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private Transform parentTran;//エフェクトの親

    [SerializeField]
    private GameObject enemy;//敵

    private GameManager gameManager;//GameManager

    private CameraController cameraController;//CameraController

    private float damage=0f;//蓄積ダメージ（初期値は0）

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //自身が試合範囲内にいたら
        if (CheckGameRange())
        {
            //以降の処理を行わない
            return;
        }

        //敵が既に死亡しているなら
        if (enemy == null)
        {
            //以降の処理を行わない
            return;
        }

        //死亡処理を行う
        KillMe();
    }

    /// <summary>
    /// 他のコライダーがすり抜けた際に呼び出される
    /// </summary>
    /// <param name="other">触れた相手</param>
    private void OnTriggerEnter(Collider other)
    {
        //触れた相手が敵のアタックポイントなら
        if(other.gameObject.CompareTag("AttackPoint"))
        {
            //攻撃を受けた際の処理を行う
            Attacked(other.transform);

            //触れた相手を無効化（重複処理防止）
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// CharacterHealthの初期設定を行う
    /// </summary>
    /// <param name="gameManager">GameManager</param>
    public void SetUpCharacterHealth(GameManager gameManager,CameraController cameraController)
    {
        //GameManagerを取得
        this.gameManager = gameManager;

        //CameraControllerを設定
        this.cameraController = cameraController;
    }

    /// <summary>
    /// 攻撃された際の処理を行う
    /// </summary>
    /// <param name="enemyTran">攻撃相手の位置情報</param>
    private void Attacked(Transform enemyTran)
    {
        //ダメージを増やす
        damage += 10f;

        //効果音を再生
        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Explosion).clip);

        //攻撃相手が自身より左にいるなら
        if (enemyTran.position.x > transform.position.x)
        {
            //横に吹っ飛ばされる
            transform.DOMoveX(transform.position.x - (damage * GameData.instance.powerRatio), 0.5f);
        }
        //攻撃相手が自身より右にいるなら
        else if (enemyTran.position.x < transform.position.x)
        {
            //吹っ飛ばされる
            transform.DOMoveX(transform.position.x + (damage * GameData.instance.powerRatio), 0.5f);
        }

        //上に吹っ飛ばされる
        transform.DOMoveY(transform.position.y + (damage * GameData.instance.powerRatio), 0.5f);

        //エフェクトを生成
        GameObject effect = Instantiate(GameData.instance.attackEffect, enemyTran.position, Quaternion.identity,parentTran);

        //一定時間後に、生成したエフェクトを消す   
        Destroy(effect, 1f);
    }

    /// <summary>
    /// ダメージを取得する
    /// </summary>
    /// <returns>ダメージ</returns>
    public float GetDamage()
    {
        //ダメージの値を返す
        return damage;
    }

    /// <summary>
    /// 自身が試合範囲内にいるかどうか調べる
    /// </summary>
    /// <returns>自身が試合範囲内にいたらtrue</returns>
    private bool CheckGameRange()
    {
        //自身が試合範囲内にいたら
        if(transform.position.x<=15f&&transform.position.x>=-15f&& transform.position.y <= 7f && transform.position.y >= -7f)
        {
            //trueを返す
            return true;
        }

        //falseを返す
        return false;
    }

    /// <summary>
    /// 死亡処理を行う
    /// </summary>
    private void KillMe()
    {
        //効果音を再生
        SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Dead).clip);

        //エフェクトを生成
        GameObject effect = Instantiate(GameData.instance.deadEffect,transform.position,Quaternion.identity,parentTran);

        //一定時間後に、生成したエフェクトを消す   
        Destroy(effect, 1f);

        //ゲーム終了の準備を行う
        gameManager.SetUpEndGame();

        //カメラの対象物のリストから自身を削除する
        cameraController.targetTransList.Remove(transform);

        //自身を消す
        Destroy(gameObject);
    }
}
