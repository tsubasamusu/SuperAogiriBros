using System.Collections;//IEnumeratorを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

namespace Tsubasa
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private GameObject attackPoint;//攻撃位置

        [SerializeField]
        private Rigidbody rb;//RigidBody

        [SerializeField]
        private Animator animator;//Animator

        [SerializeField]
        private CharacterManager.CharaName myName;//自分の名前

        private float moveDirection;//移動方向

        private float cliffTimer;//崖にしがみついている総時間

        private bool isjumping;//ジャンプしているかどうか

        private bool isAttack;//攻撃しているかどうか

        private bool jumped;//崖からジャンプしたかどうか

        private bool soundFlag;//崖の効果音用

        /// <summary>
        /// CharacterControllerの初期設定を行う
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        public void SetUpCharacterController(CharacterManager characterManager)
        {
            //攻撃位置を無効化
            attackPoint.SetActive(false);

            //プレイヤーの移動を開始する
            StartCoroutine(Move(characterManager));
        }

        /// <summary>
        /// プレイヤーの移動を実行する
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        /// <returns>待ち時間</returns>
        private IEnumerator Move(CharacterManager characterManager)
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

                    //時間的にまだしがみついていられるなら
                    if (cliffTimer < GameData.instance.maxCliffTime)
                    {
                        //崖にしがみつく
                        ClingingCliff(characterManager);
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

                //soundFlagにfalseを入れる
                soundFlag = false;

                //崖にしがみついていられる時間を初期化する
                cliffTimer = 0f;

                //プレーヤーの行動を制御する
                StartCoroutine(ControlMovement(characterManager));

                //攻撃以外のアニメーションを制御する
                ControlAnimation();

                //一定時間待つ（実質、FixedUpdateメソッド）
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// プレーヤーの行動を制御する
        /// </summary>
        /// <param name="characterManager">CharacterManager</param>
        /// <returns>待ち時間</returns>
        private IEnumerator ControlMovement(CharacterManager characterManager)
        {
            //右矢印が押されている間
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Right)))
            {
                //右を向く
                transform.eulerAngles = new Vector3(0f, -90f, 0f);

                //移動方向を設定
                moveDirection = 1f;
            }
            //左矢印が押されている間
            else if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Left)))
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

            rb.AddForce(transform.forward * Mathf.Abs(moveDirection) * GameData.instance.moveSpeed);

            //下矢印が押され、攻撃中ではないなら
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Down)) && !isAttack)
            {
                //攻撃する
                StartCoroutine(Attack());
            }

            //上矢印が押され、ジャンプ中ではないなら
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Up)) && !isjumping)
            {
                //ジャンプ中に切り替える
                isjumping = true;

                //効果音を再生
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //ジャンプする
                rb.AddForce(transform.up * GameData.instance.jumpPower);

                //完全に離着するまで待つ
                yield return new WaitForSeconds(1.8f);

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
            if (isjumping)
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

            //音声を再生
            SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetCharacterVoiceData(myName).clip);

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
        /// <param name="characterManager">CharacterManager</param>
        private void ClingingCliff(CharacterManager characterManager)
        {
            //既に崖からジャンプしたなら
            if (jumped)
            {
                //以降の処理を行わない
                return;
            }

            //soundFlagがfalseなら
            if (!soundFlag)
            {
                //効果音を再生
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Cliff).clip);

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

            //上矢印が押されたら
            if (Input.GetKey(characterManager.GetCharacterControllerKey(myName, CharacterManager.KeyType.Up)))
            {
                //効果音を再生
                SoundManager.instance.PlaySoundByAudioSource(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.jump).clip);

                //ジャンプする
                transform.DOMoveY(transform.position.y + GameData.instance.jumpHeight, 0.5f);

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
}
