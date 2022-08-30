using System.Collections;//IEnumeratorを使用
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//移動速度（仮）

    [SerializeField]
    private float jumpPower;//ジャンプの力（仮）

    [SerializeField]
    private GameObject attackPoint;//攻撃位置

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private float moveDirection;//移動方向

    private bool isjumping;//ジャンプしているかどうか

    private bool isAttack;//攻撃しているかどうか

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //攻撃位置を無効化
        attackPoint.SetActive(false);
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //プレーヤーの行動を制御する
        StartCoroutine(ControlMovement());

        //攻撃以外のアニメーションを制御する
        ControlAnimation();
    }

    /// <summary>
    /// プレーヤーの行動を制御する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator ControlMovement()
    {
        //移動方向を取得
        moveDirection = Input.GetAxis("Horizontal");

        //右方向へ移動するなら
        if (moveDirection > 0f)
        {
            //右を向く
            transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
        //左方向へ移動するなら
        else if (moveDirection < 0f)
        {
            //左を向く
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        //TODO:GameDataから移動速度を取得する処理

        rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * moveSpeed);

        //攻撃キーが押され、攻撃中ではないなら
        if (Input.GetAxis("Vertical") < 0f && !isAttack)
        {
            //攻撃する
            StartCoroutine(Attack());
        }

        //ジャンプキーが押され、ジャンプ中ではないなら
        if (Input.GetAxis("Vertical") > 0f && !isjumping)
        {
            //ジャンプ中に切り替える
            isjumping = true;

            //TODO:GameDataからジャンプ力を取得する処理

            //ジャンプする
            rb.AddForce(transform.up * jumpPower);

            //完全に離着するまで待つ
            yield return new WaitForSeconds(1.5f);

            //ジャンプを終了する
            isjumping = false;
        }
    }

    /// <summary>
    /// 接地判定を行う
    /// </summary>
    /// <returns>接地していたらtrue</returns>
    private bool CheckGrounded()
    {
        //光線の初期位置と向きを設定
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //光線の長さを設定
        float tolerance = 0.3f;

        //光線の判定を返す
        return Physics.Raycast(ray, tolerance);
    }

    /// <summary>
    /// 攻撃以外のアニメーションを制御する
    /// </summary>
    private void ControlAnimation()
    {
        //攻撃中なら
        if(isAttack)
        {
            //ジャンプのアニメーションを止める
            animator.SetBool("Jump", false);

            //走るアニメーションを止める
            animator.SetBool("Run", false);

            //以降の処理を行わない
            return;
        }

        //ジャンプ中なら
        if(isjumping)
        {
            //走るアニメーションを止める
            animator.SetBool("Run", false);

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

        //接地していないなら
        if(!CheckGrounded())
        {
            //以降の処理を行わない
            return;
        }

        //移動キーが押されているなら
        if(moveDirection!=0f)
        {
            //走るアニメーションを行う
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

    /// <summary>
    /// 攻撃する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Attack()
    {
        //攻撃中に切り替える
        isAttack = true;

        //攻撃アニメーションを行う
        animator.SetBool("Attack", true);

        //つま先が、攻撃位置に来るまで待つ
        yield return new WaitForSeconds(0.3f);

        //攻撃位置を有効化
        attackPoint.SetActive(true);

        //足が完全に上がるまで待つ
        yield return new WaitForSeconds(0.2f);

        //攻撃位置を無効化
        attackPoint.SetActive(false);

        //攻撃のアニメーションを止める
        animator.SetBool("Attack", false);

        //元の姿勢に戻るまで待つ
        yield return new WaitForSeconds(0.5f);

        //攻撃を終了する
        isAttack = false;
    }
}
