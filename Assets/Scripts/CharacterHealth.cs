using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private CameraController cameraController;//CameraController

    private float damage=0f;//蓄積ダメージ（初期値は0）

    [SerializeField]
    private float powerRatio;//ダメージ比率（仮）

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

        //力を加える
        rb.AddForce(enemyTran.forward * damage * powerRatio, ForceMode.Impulse);
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
        //カメラの対象物のリストから自身を削除する
        cameraController.targetTransList.Remove(transform);

        //自身を消す
        Destroy(gameObject);
    }
}
