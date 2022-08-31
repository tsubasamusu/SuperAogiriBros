using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;//Rigidbody

    [SerializeField]
    private Transform forceTran;//吹っ飛ばされる時に力を加える位置

    private float damage=0f;//蓄積ダメージ（初期値は0）

    private float forcePower;//吹っ飛ばされる時に加わる力の大きさ

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
            Attacked();
        }
    }

    /// <summary>
    /// 攻撃された際の処理
    /// </summary>
    private void Attacked()
    {
        //ダメージを増やす
        damage += 10f;


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
}
