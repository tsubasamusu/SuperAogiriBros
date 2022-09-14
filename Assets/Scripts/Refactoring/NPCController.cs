using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用
using System.Runtime.InteropServices.WindowsRuntime;

namespace yamap {

    public class NPCController : CharaControllerBase {

        //[SerializeField]
        private Transform enemyTran = null;//敵の位置情報

        //[SerializeField]
        //private GameObject attackPoint;//攻撃位置

        //[SerializeField]
        //private Rigidbody rb;//RigidBody

        //[SerializeField]
        //private Animator animator;//Animator

        //[SerializeField]
        //private CharacterManager.CharaName myName;//自分の名前

        //private bool isAttack;//攻撃しているかどうか

        private bool isJumping;//ジャンプしているかどうか

        private float currentMoveSpeed;//現在の移動速度

        //private bool soundFlag;//ジャンプの効果音用

        //private bool jumped;//崖からジャンプしたかどうか


        /// <summary>
        /// CharacterControllerの初期設定を行う
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        public override void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc) {
            base.SetUpCharacterController(charaData, ownerType);

            //現在の移動速度を初期値に設定
            currentMoveSpeed = GameData.instance.npcMoveSpeed;
            enemyTran = npc.transform;
        }

        ///// <summary>
        ///// ゲーム開始直後に呼び出される
        ///// </summary>
        //private void Start()
        //{
        //    //攻撃位置を無効化
        //    attackPoint.SetActive(false);

        //    //現在の移動速度を初期値に設定
        //    currentMoveSpeed = GameData.instance.npcMoveSpeed;

        //    //移動を開始する
        //    StartCoroutine(Move());
        //}

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() {
            if (!isSetUp) {
                return;
            }

            //敵が既に死んでいるなら
            if (enemyTran == null) {
                //以降の処理を行わない
                return;
            }

            //崖にしがみついているなら
            if (CheckCliff()) {
                StartCoroutine(ClingingCliff());

                //以降の処理を行わない
                return;
            }

            //敵が場外にいるなら
            if (Mathf.Abs(enemyTran.position.x) > 7f) {
                //NPCの動きを止める
                currentMoveSpeed = 0f;

                //走るアニメーションを止める
                animator.SetBool("Run", false);

                //以降の処理を行わない
                return;
            }

            //敵が自分の真上or真下にいるなら
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) <= 0.5f) {
                //NPCの動きを止める
                currentMoveSpeed = 0f;

                //まだジャンプしていないなら
                if (!isJumping) {
                    //ジャンプする
                    StartCoroutine(Jump());
                }

                //以降の処理を行わない
                return;
            }

            //敵が攻撃圏内に入っているなら
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f && Mathf.Abs(enemyTran.position.y - transform.position.y) < 2) {
                //攻撃中ではないなら
                if (!isAttack) {
                    //走るアニメーションを止める
                    animator.SetBool("Run", false);

                    //ジャンプのアニメーションを止める
                    animator.SetBool("Jump", false);

                    //NPCの動きを止める
                    currentMoveSpeed = 0f;

                    //攻撃する
                    StartCoroutine(Attack());
                }

                //以降の処理を行わない
                return;
            }

            //現在の移動速度を初期値に設定
            currentMoveSpeed = GameData.instance.npcMoveSpeed;

            //敵が自身より右にいるなら
            if (enemyTran.position.x < transform.position.x) {
                //右を向く
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
            }
            //敵が自身より左にいるなら
            else if (enemyTran.position.x > transform.position.x) {
                //左を向く
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
            }

            //攻撃中なら
            if (isAttack) {
                //以降の処理を行わない
                return;
            }

            //接地しているなら
            if (CheckGrounded()) {
                //soundflagにfalseを入れる
                soundFlag = false;

                //崖からジャンプしていない状態に切り替える
                jumped = false;

                //走るアニメーションを行う
                animator.SetBool("Run", true);
            }
        }

        /// <summary>
        /// ジャンプする
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator Jump() {
            //ジャンプ中に切り替える
            isJumping = true;

            //効果音を再生
            SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

            //ジャンプのアニメーションを行う
            animator.SetBool("Jump", true);

            //ジャンプする
            rb.AddForce(transform.up * GameData.instance.npcJumpPower);

            //完全に離着するまで待つ
            yield return new WaitForSeconds(1.8f);

            //ジャンプのアニメーションを止める
            animator.SetBool("Jump", false);

            //ジャンプを終了する
            isJumping = false;
        }

        ///// <summary>
        ///// 接地判定を行う
        ///// </summary>
        ///// <returns>接地していたらtrue</returns>
        //private bool CheckGrounded()
        //{
        //    //光線の初期位置と向きを設定
        //    Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //    //光線の長さを設定
        //    float tolerance = 0.3f;

        //    //光線の判定を返す
        //    return Physics.Raycast(ray, tolerance);
        //}

        ///// <summary>
        ///// 移動を実行する
        ///// </summary>
        ///// <returns>待ち時間</returns>
        //private IEnumerator Move()
        //{
        //    //ゲーム開始直後に少し待つ
        //    yield return new WaitForSeconds(0.5f);

        //    //無限に繰り返す
        //    while(true)
        //    {
        //        //敵が既に死亡したなら
        //        if(enemyTran==null)
        //        {
        //            //一定時間待つ（実質、FixedUpdateメソッド）
        //            yield return new WaitForSeconds(Time.fixedDeltaTime);

        //            //次の繰り返し処理に移る
        //            continue;
        //        }

        //        //自身が場外にいるなら
        //        if (Mathf.Abs(transform.position.x)>7f&&transform.position.y<0f)
        //        {
        //            //一定時間待つ（実質、FixedUpdateメソッド）
        //            yield return new WaitForSeconds(Time.fixedDeltaTime);

        //            //次の繰り返し処理に移る
        //            continue;
        //        }

        //        //移動する
        //        rb.AddForce(transform.forward * currentMoveSpeed);

        //        //一定時間待つ（実質、FixedUpdateメソッド）
        //        yield return new WaitForSeconds(Time.fixedDeltaTime);
        //    }
        //}

        /// <summary>
        /// プレイヤーかNPC で移動の制御を書き換えるためのメソッド
        /// NPC の移動処理を書く
        /// </summary>
        /// <param name="characterManager"></param>
        /// <returns></returns>
        protected override IEnumerator ObserveMove() {

            //敵が既に死亡したなら
            if (enemyTran == null) {
                //一定時間待つ（実質、FixedUpdateメソッド）
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //次の繰り返し処理に移る
                yield break;
            }

            //自身が場外にいるなら
            if (Mathf.Abs(transform.position.x) > 7f && transform.position.y < 0f) {
                //一定時間待つ（実質、FixedUpdateメソッド）
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //次の繰り返し処理に移る
                yield break;
            }

            //移動する
            rb.AddForce(transform.forward * currentMoveSpeed);

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);

        }

        ///// <summary>
        ///// 攻撃する
        ///// </summary>
        ///// <returns>待ち時間</returns>
        //private IEnumerator Attack()
        //{
        //    //攻撃中に切り替える
        //    isAttack = true;

        //    //音声を再生
        //    SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetCharacterVoiceData(myName).clip);

        //    //攻撃アニメーションを行う
        //    animator.SetBool("Attack", true);

        //    //つま先が、攻撃位置に来るまで待つ
        //    yield return new WaitForSeconds(0.3f);

        //    //攻撃位置を有効化
        //    attackPoint.SetActive(true);

        //    //足が完全に上がるまで待つ
        //    yield return new WaitForSeconds(0.2f);

        //    //攻撃位置を無効化
        //    attackPoint.SetActive(false);

        //    //攻撃のアニメーションを止める
        //    animator.SetBool("Attack", false);

        //    //元の姿勢に戻るまで待つ
        //    yield return new WaitForSeconds(0.5f);

        //    //攻撃を終了する
        //    isAttack = false;
        //}

        ///// <summary>
        ///// 崖にしがみついているかどうか調べる
        ///// </summary>
        ///// <returns>崖にしがみついていたらtrue</returns>
        //private bool CheckCliff()
        //{
        //    //プレイヤーが崖より上か下にいるなら
        //    if (transform.position.y > -1f || transform.position.y < -3f)
        //    {
        //        //以降の処理を行わない
        //        return false;
        //    }

        //    //プレイヤーが崖より外側にいるなら
        //    if (transform.position.x < -9f || transform.position.x > 9f)
        //    {
        //        //以降の処理を行わない
        //        return false;
        //    }

        //    //trueを返す
        //    return true;
        //}

        ///// <summary>
        ///// 崖にしがみつく
        ///// </summary>
        ///// <returns>待ち時間</returns>
        //private IEnumerator ClingingCliff()
        //{
        //    //既に崖からジャンプしたなら
        //    if (jumped)
        //    {
        //        //以降の処理を行わない
        //        yield break;
        //    }

        //    //soundFlagがfalseなら
        //    if (!soundFlag)
        //    {
        //        //効果音を再生
        //        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Cliff).clip);

        //        //soundFlagにtrueを入れる
        //        soundFlag = true;
        //    }

        //    //攻撃のアニメーションを止める
        //    animator.SetBool("Attack", false);

        //    //ジャンプのアニメーションを止める
        //    animator.SetBool("Jump", false);

        //    //走るアニメーションを止める
        //    animator.SetBool("Run", false);

        //    //崖にしがみつくアニメーションを行う
        //    animator.SetBool("Cliff", true);

        //    //キャラクターを崖の位置に移動させる
        //    transform.position = transform.position.x > 0 ? new Vector3(7.5f, -2f, 0f) : new Vector3(-7.5f, -2f, 0f);

        //    //キャラクターの向きを崖に合わせる
        //    transform.eulerAngles = transform.position.x > 0 ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f);

        //    //1秒待つ
        //    yield return new WaitForSeconds(1f);

        //    //まだ崖からジャンプしていないなら
        //    if (!jumped)
        //    {
        //        //効果音を再生
        //        SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

        //        //ジャンプする
        //        transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

        //        //崖にしがみつアニメーションを止める
        //        animator.SetBool("Cliff", false);

        //        ///ジャンプのアニメーションを行う
        //        animator.SetBool("Jump", true);

        //        //ジャンプした状態に切り替える
        //        jumped = true;
        //    }
        //}


        protected override void AfterJump() {
            //まだ崖からジャンプしていないなら
            if (!jumped) {
                //効果音を再生
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //ジャンプする
                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

                //崖にしがみつアニメーションを止める
                animator.SetBool("Cliff", false);

                ///ジャンプのアニメーションを行う
                animator.SetBool("Jump", true);

                //ジャンプした状態に切り替える
                jumped = true;
            }
        }
    }
}