using System.Collections;
using UnityEngine;

/// <summary>
/// 所有者の種類
/// </summary>
public enum OwnerType {
    Player,//プレイヤー
    NPC//コンピューター（NPC）
}

/// <summary>
/// 親クラス
/// </summary>
public class CharaControllerBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject attackPoint;//攻撃位置

    protected Rigidbody rb;//RigidBody

    protected Animator animator;//Animator

    protected bool isjumping;//ジャンプしているかどうか

    protected bool isAttack;//攻撃しているかどうか

    protected bool jumped;//崖からジャンプしたかどうか

    protected bool soundFlag;//崖の効果音用

    protected CharaData charaData;//キャラクターのデータ

    protected OwnerType ownerType;//所有者の種類

    protected bool isSetUp;//初期設定が完了したかどうか

    /// <summary>
    /// CharacterControllerの初期設定を行う
    /// </summary>
    /// <param name="charaData">キャラクターのデータ</param>
    /// <param name="ownerType">所有者の種類</param>
    /// <param name="npc">CharaControllerBase</param>
    public virtual void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc = null)
    { 
        //キャラクターのデータを取得
        this.charaData = charaData;

        //所有者の種類を取得
        this.ownerType = ownerType;

        //Rigidbodyの取得に失敗したら
        if(!TryGetComponent(out rb))
        {
            //問題を報告 
            Debug.Log("Rigidbodyの取得に失敗");
        }

        //Animatorの取得に失敗したら
        if (!TryGetComponent(out animator))
        {
            //問題を報告
            Debug.Log("Animator 取得出来ません。");
        }

        //攻撃位置を無効化
        attackPoint.SetActive(false);

        //プレイヤーの移動を開始する
        StartCoroutine(Move());

        //初期設定が完了した状態に切り替える
        isSetUp = true;
    }


    /// <summary>
    /// プレイヤーの移動を実行する
    /// </summary>
    /// <returns>待ち時間</returns>
    protected IEnumerator Move() 
    {
        //0.5秒待つ（瞬間移動防止）
        yield return new WaitForSeconds(0.5f);

        //無限に繰り返す
        while (true) 
        {
            //移動する（プレイヤーかNPCでメソッドの中身を変える）
            StartCoroutine(ObserveMove());

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// プレイヤーかNPCで移動の制御を書き換えるためのメソッド
    /// </summary>
    /// <returns>待ち時間</returns>
    protected virtual IEnumerator ObserveMove() 
    {
        //（仮）
        yield return null;
    }

    /// <summary>
    /// 接地判定を行う
    /// </summary>
    /// <returns>接地していたらtrue</returns>
    protected bool CheckGrounded() 
    {
        //光線の初期位置と向きを設定
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //光線の長さを設定
        float tolerance = 0.3f;

        //光線の判定を返す
        return Physics.Raycast(ray, tolerance);
    }

    /// <summary>
    /// 攻撃する
    /// </summary>
    /// <returns>待ち時間</returns>
    protected IEnumerator Attack() 
    {
        //攻撃中に切り替える
        isAttack = true;

        //音声を再生
        yamap.SoundManager.instance.PlaySound(yamap.SoundManager.instance.GetCharacterVoiceData(charaData.charaName).clip);

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
    /// 崖にしがみついているかどうか調べる
    /// </summary>
    /// <returns>崖にしがみついていたらtrue</returns>
    protected bool CheckCliff() 
    {
        //プレイヤーが崖より上か下にいるなら
        if (transform.position.y > -1f || transform.position.y < -3f) {
            //以降の処理を行わない
            return false;
        }

        //プレイヤーが崖より外側にいるなら
        if (transform.position.x < -9f || transform.position.x > 9f) {
            //以降の処理を行わない
            return false;
        }

        //trueを返す
        return true;
    }

    /// <summary>
    /// 崖にしがみつく
    /// </summary>
    /// <returns>待ち時間</returns>
    protected IEnumerator ClingingCliff() 
    {
        //既に崖からジャンプしたなら
        if (jumped) {
            //以降の処理を行わない
            yield break;
        }

        //soundFlagがfalseなら
        if (!soundFlag) {
            //効果音を再生
            yamap.SoundManager.instance.PlaySound(yamap.SoundManager.instance.GetCharacterVoiceData(charaData.charaName).clip);

            //soundFlagにtrueを入れる
            soundFlag = true;
        }

        //攻撃のアニメーションを止める
        animator.SetBool("Attack", false);

        //ジャンプのアニメーションを止める
        animator.SetBool("Jump", false);

        //走るアニメーションを止める
        animator.SetBool("Run", false);

        //崖にしがみつくアニメーションを行う
        animator.SetBool("Cliff", true);

        // NPC なら
        if(ownerType == OwnerType.NPC) {
            // プレイヤーの位置と向きを崖の位置と向きに合わせる
            AdjustmentPlayerToCliffTran();

            //1秒待つ
            yield return new WaitForSeconds(1f);
        }

        //soundFlagにfalseを入れる
        soundFlag = false;

        //ジャンプの後の処理を行う
        AfterJump();
    }

    /// <summary>
    /// ジャンプの後の処理を行う
    /// </summary>
    protected virtual void AfterJump() 
    {
        //（各子クラスで上書きして設定しろ）
    }

    /// <summary>
    /// キャラクターの向きと位置を、崖の向きと位置に合わせる
    /// </summary>
    protected void AdjustmentPlayerToCliffTran() 
    {
        //プレイヤーを崖の位置に移動させる
        transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        //プレイヤーの向きを崖に合わせる
        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
    }
}
