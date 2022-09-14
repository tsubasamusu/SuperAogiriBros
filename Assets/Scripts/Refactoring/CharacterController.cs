using System.Collections;//IEnumeratorを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

namespace yamap
{
    /// <summary>
    /// 子クラス（プレイヤーが操作する用）
    /// </summary>
    public class CharacterController : CharaControllerBase 
    {
        private float moveDirection;//移動方向

        private float cliffTimer;//崖にしがみついている時間

        /// <summary>
        /// 所有者特有の動きを実行（while文の中で呼び出される）
        /// </summary>
        /// <returns>待ち時間</returns>
        protected override IEnumerator ObserveMove() 
        {
            //崖にしがみついているなら
            if (CheckCliff()) 
            {
                //時間を計測する
                cliffTimer += Time.fixedDeltaTime;

                //時間的にまだしがみついていられるなら
                if (cliffTimer < GameData.instance.maxCliffTime) 
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
                yield break;
            }

            //soundFlagにfalseを入れる
            soundFlag = false;

            //崖にしがみついていられる時間を初期化する
            cliffTimer = 0f;

            //プレーヤーの行動を制御する
            StartCoroutine(ControlMovement());

            //攻撃以外のアニメーションを制御する
            ControlAnimation();

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        /// <summary>
        /// プレーヤーの行動を制御する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator ControlMovement() 
        {
            //右矢印が押されている間
            if (Input.GetKey(charaData.keys[0])) 
            {
                //右を向く
                transform.eulerAngles = new Vector3(0f, -90f, 0f);

                //移動方向を設定
                moveDirection = 1f;
            }
            //左矢印が押されている間
            else if (Input.GetKey(charaData.keys[1])) 
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

            //力を加える
            rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * GameData.instance.moveSpeed);

            //下矢印が押され、攻撃中ではないなら
            if (Input.GetKey(charaData.keys[2]) && !isAttack) 
            {
                //攻撃する
                StartCoroutine(Attack());
            }

            //上矢印が押され、ジャンプ中ではないなら
            if (Input.GetKey(charaData.keys[3]) && !isjumping) 
            {
                //ジャンプ中に切り替える
                isjumping = true;

                //効果音を再生
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //ジャンプする
                rb.AddForce(transform.up * GameData.instance.jumpPower);

                //完全に離着するまで待つ
                yield return new WaitForSeconds(1.8f);

                //ジャンプを終了する
                isjumping = false;
            }
        }

        /// <summary>
        /// 攻撃以外のアニメーションを制御する
        /// </summary>
        private void ControlAnimation() 
        {
            //崖にしがみつくアニメーションを止める
            animator.SetBool("Cliff", false);

            //攻撃中なら
            if (isAttack) {
                //ジャンプのアニメーションを止める
                animator.SetBool("Jump", false);

                //走るアニメーションを止める
                animator.SetBool("Run", false);

                //以降の処理を行わない
                return;
            }

            //ジャンプ中なら
            if (isjumping) {
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
        /// ジャンプ後の処理を行う
        /// </summary>
        protected override void AfterJump() 
        {
            //上矢印が押されたら
            if (Input.GetKey(charaData.keys[3])) 
            {
                //効果音を再生
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //ジャンプする
                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

                //ジャンプした状態に切り替える
                jumped = true;

                //以降の処理を行わない
                return;
            }

            //キャラクターの向きと位置を、崖の向きと位置に合わせる
            AdjustmentPlayerToCliffTran();
        }
    }
}