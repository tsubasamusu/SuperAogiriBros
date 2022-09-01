using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private CameraController cameraController;//CameraController

    [SerializeField]
    private GameManager gameManager;//GameManager

    [SerializeField]
    private GameObject attackEffect;//攻撃が当たった際のエフェクト（仮）

    [SerializeField]
    private Transform parentTran;//エフェクトの親

    [SerializeField]
    private GameObject deadEffect;//死ぬ際のエフェクト（仮）

    [SerializeField]
    private float powerRatio;//ダメージ比率（仮）

    [SerializeField]
    private float damageTime;//吹っ飛ばされる時間（仮）

    private float damage=0f;//蓄積ダメージ（初期値は0）

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //自身が試合範囲内にいなかったら
        if(!CheckGameRange())
        {
            //死亡処理を行う
            KillMe();
        }
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
    /// 攻撃された際の処理を行う
    /// </summary>
    /// <param name="enemyTran">攻撃相手の位置情報</param>
    private void Attacked(Transform enemyTran)
    {
        //ダメージを増やす
        damage += 10f;

        //TODO:GameDataから「ダメージ比率」を取得する処理

        //TODO:GameDataから「吹っ飛ばされる時間」を取得する処理

        //攻撃相手が自身より左にいるなら
        if (enemyTran.position.x > transform.position.x)
        {
            //横に吹っ飛ばされる
            transform.DOMoveX(transform.position.x - (damage * powerRatio), 0.5f);
        }
        //攻撃相手が自身より右にいるなら
        else if (enemyTran.position.x < transform.position.x)
        {
            //吹っ飛ばされる
            transform.DOMoveX(transform.position.x + (damage * powerRatio), 0.5f);
        }

        //上に吹っ飛ばされる
        transform.DOMoveY(transform.position.y + (damage * powerRatio), 0.5f);

        //TODO:GameDataから「攻撃が当たった際のエフェクト」を取得する処理

        //エフェクトを生成
        GameObject effect = Instantiate(attackEffect, enemyTran.position, Quaternion.identity,parentTran);

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
        //TODO:GameDataから死ぬ際のエフェクトを取得する処理

        //エフェクトを生成
        GameObject effect = Instantiate(deadEffect,transform.position,Quaternion.identity,parentTran);

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
