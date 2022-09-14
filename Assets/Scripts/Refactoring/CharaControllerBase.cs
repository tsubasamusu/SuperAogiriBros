using System.Collections;
using UnityEngine;



public enum OwnerType {
    Player,
    NPC
}

public class CharaControllerBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject attackPoint;//攻撃位置

    ////[SerializeField]
    protected Rigidbody rb;//RigidBody

    //[SerializeField]
    protected Animator animator;//Animator

    //[SerializeField]
    //protected CharaName myName;//自分の名前

    protected bool isjumping;//ジャンプしているかどうか

    protected bool isAttack;//攻撃しているかどうか

    protected bool jumped;//崖からジャンプしたかどうか

    protected bool soundFlag;//崖の効果音用

    protected CharaData charaData;

    protected OwnerType ownerType;

    protected bool isSetUp;


    /// <summary>
    /// CharacterControllerの初期設定を行う
    /// </summary>
    /// <param name="characterManager">CharacterManager</param>
    public virtual void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc = null) {

        this.charaData = charaData;
        this.ownerType = ownerType;

        if(!TryGetComponent(out rb)){
            Debug.Log("Rigidbody 取得出来ません。");
        }
        if (!TryGetComponent(out animator)) {
            Debug.Log("Animator 取得出来ません。");
        }

        //攻撃位置を無効化
        attackPoint.SetActive(false);

        //プレイヤーの移動を開始する
        StartCoroutine(Move());

        isSetUp = true;
    }


    /// <summary>
    /// プレイヤーの移動を実行する
    /// </summary>
    /// <param name="characterManager">CharacterManager</param>
    /// <returns>待ち時間</returns>
    protected IEnumerator Move() {
        //0.5秒待つ
        yield return new WaitForSeconds(0.5f);

        //無限に繰り返す
        while (true) {

            // Player か NPC でメソッドの中身を変える
            StartCoroutine(ObserveMove());

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// プレイヤーかNPC で移動の制御を書き換えるためのメソッド
    /// </summary>
    /// <param name="characterManager"></param>
    /// <returns></returns>

    protected virtual IEnumerator ObserveMove() {
        yield return null;
    }

    /// <summary>
    /// 接地判定を行う
    /// </summary>
    /// <returns>接地していたらtrue</returns>
    protected bool CheckGrounded() {
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
    protected IEnumerator Attack() {
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
    protected bool CheckCliff() {
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
    protected IEnumerator ClingingCliff() {
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

        soundFlag = false;    //  <=  false にしている場所がなかったので、ここに追加

        // ジャンプの後処理
        AfterJump();
    }

    /// <summary>
    /// ジャンプの後処理
    /// </summary>
    protected virtual void AfterJump() {

        // 各子クラスで上書きして設定する
    }

    /// <summary>
    /// プレイヤーの位置と向きを崖の位置と向きに合わせる
    /// AfterJump の中で適宜なタイミングで実行する
    /// </summary>
    protected void AdjustmentPlayerToCliffTran() {
        //プレイヤーを崖の位置に移動させる
        transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        //プレイヤーの向きを崖に合わせる
        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);
    }
}
