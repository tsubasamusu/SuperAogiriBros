using System.Collections;//IEnumeratorを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class MashiroController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//移動速度（仮）

    [SerializeField]
    private float jumpPower;//ジャンプの力（仮）

    [SerializeField, Tooltip("崖から復活するときのジャンプ力")]
    private float jumpHeight;//崖から復活するときのジャンプの高さ（仮）

    [SerializeField, Tooltip("崖にしがみついていられる時間")]
    private float maxCliffTime;//崖にしがみついていられる時間（仮）

    [SerializeField]
    private GameObject attackPoint;//攻撃位置

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private float moveDirection;//移動方向

    private float cliffTimer;//崖にしがみついている総時間

    private bool isJumping;//ジャンプしているかどうか

    private bool isAttack;//攻撃しているかどうか

    private bool jumped;//崖からジャンプしたかどうか

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //攻撃位置を無効化
        attackPoint.SetActive(false);

        //プレイヤーの移動を開始する
        StartCoroutine(Move());
    }

    /// <summary>
    /// プレイヤーの移動を実行する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Move()
    {
        //0.5秒待つ
        yield return new WaitForSeconds(0.5f);

        //無限に繰り返す
        while (true)
        {
            //崖にしがみついているなら
            if (CheckCliff())
            {
                //時間を計測する
                cliffTimer += Time.fixedDeltaTime;

                //TODO:GameDataから「崖にしがみついていられる時間」を取得する処理

                //時間的にまだしがみついていられるなら
                if (cliffTimer < maxCliffTime)
                {
                    //崖にしがみつく
                    ClingingCliff();
                }
                //しがみついていられる最高時間を超えたら
                else
                {
                    //崖にしがみつくアニメーションをやめる
                    animator.SetBool("Cliff", false);
                }

                //一定時間待つ（実質、FixedUpdateメソッド）
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //次の繰り返し処理へ飛ばす
                continue;
            }

            //崖にしがみついていられる時間を初期化する
            cliffTimer = 0f;

            //プレーヤーの行動を制御する
            StartCoroutine(ControlMovement());

            //攻撃以外のアニメーションを制御する
            ControlAnimation();

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// プレーヤーの行動を制御する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator ControlMovement()
    {
        //Dが押されている間
        if (Input.GetKey(KeyCode.D))
        {
            //右を向く
            transform.eulerAngles = new Vector3(0f, -90f, 0f);

            //移動方向を設定
            moveDirection = 1f;
        }
        //Aが押されている間
        else if (Input.GetKey(KeyCode.A))
        {
            //左を向く
            transform.eulerAngles = new Vector3(0f, 90f, 0f);

            //移動方向を設定
            moveDirection = -1f;
        }
        //移動指示がなければ
        else
        {
            //移動しない
            moveDirection = 0f;
        }

        //TODO:GameDataから移動速度を取得する処理

        rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * moveSpeed);

        //Sが押され、攻撃中ではないなら
        if (Input.GetKey(KeyCode.S) && !isAttack)
        {
            //攻撃する
            StartCoroutine(Attack());
        }

        //Wが押され、ジャンプ中ではないなら
        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            //ジャンプ中に切り替える
            isJumping = true;

            //TODO:GameDataからジャンプ力を取得する処理

            //ジャンプする
            rb.AddForce(transform.up * jumpPower);

            //完全に離着するまで待つ
            yield return new WaitForSeconds(1.8f);

            //ジャンプを終了する
            isJumping = false;
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
        //崖にしがみつくアニメーションを止める
        animator.SetBool("Cliff", false);

        //攻撃中なら
        if (isAttack)
        {
            //ジャンプのアニメーションを止める
            animator.SetBool("Jump", false);

            //走るアニメーションを止める
            animator.SetBool("Run", false);

            //以降の処理を行わない
            return;
        }

        //ジャンプ中なら
        if (isJumping)
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
        if (!CheckGrounded())
        {
            //以降の処理を行わない
            return;
        }

        //崖からジャンプしていない状態に切り替える
        jumped = false;

        //移動キーが押されているなら
        if (moveDirection != 0f)
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

    /// <summary>
    /// 崖にしがみついているかどうか調べて、崖にしがみつく処理を行う
    /// </summary>
    /// <returns>崖にしがみついていたらtrue</returns>
    private bool CheckCliff()
    {
        //プレイヤーが崖より上か下にいるなら
        if (transform.position.y > -1f || transform.position.y < -3f)
        {
            //以降の処理を行わない
            return false;
        }

        //プレイヤーが崖より外側にいるなら
        if (transform.position.x < -9f || transform.position.x > 9f)
        {
            //以降の処理を行わない
            return false;
        }

        //trueを返す
        return true;
    }

    /// <summary>
    /// 崖にしがみつく
    /// </summary>
    private void ClingingCliff()
    {
        //既に崖からジャンプしたなら
        if (jumped)
        {
            //以降の処理を行わない
            return;
        }

        //攻撃のアニメーションを止める
        animator.SetBool("Attack", false);

        //ジャンプのアニメーションを止める
        animator.SetBool("Jump", false);

        //走るアニメーションを止める
        animator.SetBool("Run", false);

        //崖にしがみつくアニメーションを行う
        animator.SetBool("Cliff", true);

        //Wが押されたら
        if (Input.GetKey(KeyCode.W))
        {
            //TODO:GameDataから「崖から復活するときのジャンプの高さ」を取得する処理

            //ジャンプする
            transform.DOMoveY(transform.position.y + jumpHeight, 0.5f);

            //ジャンプした状態に切り替える
            jumped = true;

            //以降の処理を行わない
            return;
        }

        //プレイヤーを崖の位置に移動させる
        transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        //プレイヤーの向きを崖に合わせる
        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
    }
}

