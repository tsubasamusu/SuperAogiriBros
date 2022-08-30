using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//移動速度（仮）

    [SerializeField]
    private Rigidbody rb;//RigidBody

    private float moveDirection;//移動方向

    private Vector3 movement;//移動位置

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// 一定時間ごとに呼び出される
    /// </summary>
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// プレーヤーの行動を制御する
    /// </summary>
    private void Move()
    {
        //移動方向を取得
        moveDirection = Input.GetAxis("Horizontal");

        //TODO:GameDataから移動速度を取得する処理

        //移動位置を設定
        movement = new Vector3(-1f,transform.position.y,0f) * moveDirection * moveSpeed * Time.fixedDeltaTime;

        //ジャンプキーが押されたら
        if(Input.GetAxis("Vertical")>0)
        {

        }
    }
}
