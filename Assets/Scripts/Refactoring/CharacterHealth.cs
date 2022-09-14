using System.Collections;
using UnityEngine;
using DG.Tweening;//DOTweenを使用
using Tsubasa;//CameraControllerを使用

namespace yamap 
{
    /// <summary>
    /// キャラクターの体力等を管理する
    /// </summary>
    public class CharacterHealth : MonoBehaviour 
    {
        private Transform parentTran = null;//エフェクトの親

        private GameObject enemy = null;//敵

        private float damage = 0f;//蓄積ダメージ（初期値は0）

        /// <summary>
        /// 他のコライダーがすり抜けた際に呼び出される
        /// </summary>
        /// <param name="other">触れた相手</param>
        private void OnTriggerEnter(Collider other) 
        {
            //触れた相手が敵のアタックポイントなら
            if (other.gameObject.CompareTag("AttackPoint")) 
            {
                //攻撃を受けた際の処理を行う
                Attacked(other.transform);

                //触れた相手を無効化（重複処理防止）
                other.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// CharacterHealthの初期設定を行う
        /// </summary>
        /// <param name="gameManager">GameManager</param>
        /// <param name="cameraController">CameraController</param>
        /// <param name="effectTran">エフェクトの親</param>
        /// <param name="npc">CharaControllerBase</param>
        public void SetUpCharacterHealth(GameManager gameManager, CameraController cameraController, Transform effectTran, CharaControllerBase npc) 
        {
            //エフェクトの親を取得
            parentTran = effectTran;

            //敵のゲームオブジェクトを取得
            enemy = npc.gameObject;

            //Healthを監視する
            StartCoroutine(ObserveHealth(gameManager, cameraController));

            /// <summary>
            /// Healthを監視する
            /// </summary>
            /// <param name="gameManager">GameManager</param>
            /// <param name="cameraController">CameraController</param>
            /// <returns>待ち時間</returns>
            IEnumerator ObserveHealth(GameManager gameManager, CameraController cameraController) 
            {
                //自身が試合範囲内にいるなら繰り返す
                while (CheckGameRange()) 
                {
                    //次のフレームへ飛ばす（実質、Updateメソッド）
                    yield return null;

                    //敵が既に死亡しているなら
                    if (enemy == null) 
                    {
                        //以降の処理を行わない
                        break;
                    }         
                }

                //死亡処理を行う
                KillMe(gameManager, cameraController);
            }
        }

        /// <summary>
        /// 攻撃された際の処理を行う
        /// </summary>
        /// <param name="enemyTran">攻撃相手の位置情報</param>
        private void Attacked(Transform enemyTran) 
        {
            //ダメージを増やす
            damage += 10f;

            //効果音を再生
            SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Explosion).clip);

            //攻撃相手が自身より左にいるなら
            if (enemyTran.position.x > transform.position.x) 
            {
                //横に吹っ飛ばされる
                transform.DOMoveX(transform.position.x - (damage * GameData.instance.powerRatio), 0.5f);
            }
            //攻撃相手が自身より右にいるなら
            else if (enemyTran.position.x < transform.position.x) 
            {
                //吹っ飛ばされる
                transform.DOMoveX(transform.position.x + (damage * GameData.instance.powerRatio), 0.5f);
            }

            //上に吹っ飛ばされる
            transform.DOMoveY(transform.position.y + (damage * GameData.instance.powerRatio), 0.5f);

            //エフェクトを生成
            GameObject effect = Instantiate(GameData.instance.attackEffect, enemyTran.position, Quaternion.identity, parentTran);

            //一定時間後に、生成したエフェクトを消す   
            Destroy(effect, 1f);
        }

        /// <summary>
        /// 自身が試合範囲内にいるかどうか調べる
        /// </summary>
        /// <returns>自身が試合範囲内にいたらtrue</returns>
        private bool CheckGameRange() 
        {
            //自身が試合範囲内にいたら
            if (transform.position.x <= 15f && transform.position.x >= -15f && transform.position.y <= 7f && transform.position.y >= -7f) 
            {
                //trueを返す
                return true;
            }

            //falseを返す
            return false;
        }

        /// <summary>
        /// 死亡処理を行う
        /// </summary>
        private void KillMe(GameManager gameManager, CameraController cameraController) 
        {
            //効果音を再生
            SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Dead).clip);

            //エフェクトを生成
            GameObject effect = Instantiate(GameData.instance.deadEffect, transform.position, Quaternion.identity, parentTran);

            //一定時間後に、生成したエフェクトを消す   
            Destroy(effect, 1f);

            //ゲーム終了の準備を行う
            gameManager.SetUpEndGame();

            //カメラの対象物のリストから自身を削除する
            cameraController.targetTransList.Remove(transform);

            //自身を消す
            Destroy(gameObject);
        }
    }
}