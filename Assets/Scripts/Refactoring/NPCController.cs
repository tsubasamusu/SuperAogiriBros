using System.Collections;//IEnumeratorを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

namespace yamap 
{
    public class NPCController : CharaControllerBase 
    {
        private Transform enemyTran = null;//敵の位置情報

        private bool isJumping;//ジャンプしているかどうか

        private float currentMoveSpeed;//現在の移動速度

        /// <summary>
        /// CharacterControllerの初期設定を行う
        /// </summary>
        /// <param name="charaData">キャラクターのデータ</param>
        /// <param name="ownerType">所有者の種類</param>
        /// <param name="npc">CharaControllerBase</param>
        public override void SetUpCharacterController(CharaData charaData, OwnerType ownerType, CharaControllerBase npc) 
        {
            base.SetUpCharacterController(charaData, ownerType);

            //現在の移動速度を初期値に設定
            currentMoveSpeed = GameData.instance.npcMoveSpeed;

            //敵の位置情報を取得
            enemyTran = npc.transform;
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //初期設定が完了していないなら
            if (!isSetUp) 
            {
                //以降の処理を行わない
                return;
            }

            //敵が既に死んでいるなら
            if (enemyTran == null) {
                //以降の処理を行わない
                return;
            }

            //崖にしがみついているなら
            if (CheckCliff()) 
            {
                //崖にしがみつく
                StartCoroutine(ClingingCliff());

                //以降の処理を行わない
                return;
            }

            //敵が場外にいるなら
            if (Mathf.Abs(enemyTran.position.x) > 7f) 
            {
                //NPCの動きを止める
                currentMoveSpeed = 0f;

                //走るアニメーションを止める
                animator.SetBool("Run", false);

                //以降の処理を行わない
                return;
            }

            //敵が自分の真上or真下にいるなら
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) <= 0.5f) 
            {
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
            if (Mathf.Abs(enemyTran.position.x - transform.position.x) < 2f && Mathf.Abs(enemyTran.position.y - transform.position.y) < 2) 
            {
                //攻撃中ではないなら
                if (!isAttack) 
                {
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

            //攻撃中なら
            if (isAttack) 
            {
                //以降の処理を行わない
                return;
            }

            //接地しているなら
            if (CheckGrounded()) 
            {
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
        private IEnumerator Jump() 
        {
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

        /// <summary>
        /// プレイヤーかNPC で移動の制御を書き換えるためのメソッド
        /// NPC の移動処理を書く
        /// </summary>
        /// <returns>待ち時間</returns>
        protected override IEnumerator ObserveMove() 
        {
            //敵が既に死亡したなら
            if (enemyTran == null) 
            {
                //一定時間待つ（実質、FixedUpdateメソッド）
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                //次の繰り返し処理に移る
                yield break;
            }

            //自身が場外にいるなら
            if (Mathf.Abs(transform.position.x) > 7f && transform.position.y < 0f) 
            {
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

        /// <summary>
        /// ジャンプ後の処理を   行う
        /// </summary>
        protected override void AfterJump() 
        {
            //まだ崖からジャンプしていないなら
            if (!jumped) 
            {
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