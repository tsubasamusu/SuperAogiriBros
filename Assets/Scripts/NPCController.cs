using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private Transform enemyTran;//敵の位置情報

    [SerializeField]
    private float npcMoveSpeed;//移動速度（仮）

    [SerializeField]
    private GameObject attackPoint;//攻撃位置

    [SerializeField]
    private Rigidbody rb;//RigidBody

    [SerializeField]
    private Animator animator;//Animator

    private bool isAttack;//攻撃しているかどうか

    private float currentMoveSpeed;//現在の移動速度

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //攻撃位置を無効化
        attackPoint.SetActive(false);

        //TODO:GameDataからnpcMoveSpeedを取得する処理

        //現在の移動速度を初期値に設定
        currentMoveSpeed = npcMoveSpeed;

        //移動を開始する
        StartCoroutine(Move());
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //敵が既に死んでいるなら
        if(enemyTran==null)
        {
            //以降の処理を行わない
            return;
        }

        //敵が場外にいるなら
        if (enemyTran.position.x > 7f || enemyTran.position.x < -7f)
        {
            //NPCの動きを止める
            currentMoveSpeed = 0f;

            //走るアニメーションを止める
            animator.SetBool("Run", false);

            //以降の処理を行わない
            return;
        }

        //敵が攻撃圏内に入り、攻撃中ではないなら
        if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f&&!isAttack)
        {
            //走るアニメーションを止める
            animator.SetBool("Run", false);

            //NPCの動きを止める
            currentMoveSpeed = 0f;

            //攻撃する
            StartCoroutine(Attack());

            //以降の処理を行わない
            return;
        }

        //現在の移動速度を初期値に設定
        currentMoveSpeed = npcMoveSpeed;

        //敵が自身より右にいるなら
        if (enemyTran.position.x < transform.position.x)
        {
            //右を向く
            transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
        //敵が自身より左にいるなら
        else if (enemyTran.position.x > transform.position.x)
        {
            //左を向く
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        //攻撃中ではないなら
        if(!isAttack)
        {
            //走るアニメーションを行う
            animator.SetBool("Run", true);
        }
    }

    /// <summary>
    /// 移動を実行する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Move()
    {
        //ゲーム開始直後に少し待つ
        yield return new WaitForSeconds(0.5f);

        //無限に繰り返す
        while(true)
        {
            //移動する
            rb.AddForce(transform.forward * currentMoveSpeed);

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);
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

