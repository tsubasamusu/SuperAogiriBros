using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//移動速度（仮）

    [SerializeField]
    private float jumpHeight;//ジャンプの高さ（仮）

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private float moveDirection;//移動方向

    private bool isjumping;//ジャンプしているかどうか

    private bool isAttack;//攻撃しているかどうか

    private Vector3 movement;//移動位置

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //プレーヤーの行動を制御する
        StartCoroutine(ControlMovement());

        //アニメーションを制御する
        ControlAnimation();
    }

    /// <summary>
    /// 一定時間ごとに呼び出される
    /// </summary>
    private void FixedUpdate()
    {
        //移動を実行する
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// プレーヤーの行動を制御する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator ControlMovement()
    {
        //移動方向を取得
        moveDirection = Input.GetAxis("Horizontal");

        //TODO:GameDataから移動速度を取得する処理

        //移動位置を設定
        movement = new Vector3(-1f,transform.position.y,0f) * moveDirection * moveSpeed * Time.fixedDeltaTime;

        //ジャンプキーが押され、ジャンプ中ではないなら
        if(Input.GetAxis("Vertical")>0f&&!isjumping)
        {
            //ジャンプ中に切り替える
            isjumping = true;

            //TODO:GameDataからジャンプの高さを取得する処理

            //ジャンプする
            transform.DOLocalMoveY(jumpHeight,1.25f/2f).SetLoops(2,LoopType.Yoyo).OnComplete(() => {isjumping=false;});
        }

        //攻撃キーが押され、攻撃中ではないなら
        if(Input.GetAxis("Vertical")<0f&&!isAttack)
        {
            //攻撃中に切り替える
            isAttack = true;

            //攻撃するまで待つ
            yield return new WaitForSeconds(0.5f);

            //攻撃を終了する
            isAttack = false;
        }
    }

    /// <summary>
    /// アニメーションを制御する
    /// </summary>
    private void ControlAnimation()
    {
        //攻撃中なら
        if(isAttack)
        {
            //攻撃アニメーションを行う
            animator.SetBool("Attack", true);

            //以降の処理を行わない
            return;
        }
        //攻撃中ではないなら
        else
        {
            //攻撃アニメーションを止める
            animator.SetBool("Attack", false);
        }

        //ジャンプ中なら
        if(isjumping)
        {
            //ジャンプのアニメーションを行う
            animator.SetBool("Jump", true);

            //以降の処理を行わない
            return;
        }
        //ジャンプ中ではないなら
        else
        {
            //ジャンプのアニメーションを止める
            animator.SetBool("Jump", false);
        }

        //TODO:接地していないなら以降の処理を行わない

        //移動キーが押されているなら
        if(moveDirection!=0f)
        {
            //走るアニメーションを止める
            animator.SetBool("Run", true);

            //以降の処理を行わない
            return;
        }
        //移動キーが押されていないなら
        else
        {
            //走るアニメーションを止める
            animator.SetBool("Run", false);
        }
    }
}
