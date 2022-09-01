using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

public GameObject attackEffect;//攻撃が当たった際のエフェクト

    public GameObject deadEffect;//死ぬ際のエフェクト

    public float powerRatio;//ダメージ比率

    public float damageTime;//吹っ飛ばされる時間

    public float moveSpeed;//移動速度

    public float jumpPower;//ジャンプの力

    [Tooltip("崖から復活するときのジャンプ力")]
    public float jumpHeight;//崖から復活するときのジャンプの高さ

    [Tooltip("崖にしがみついていられる時間")]
    public float maxCliffTime;//崖にしがみついていられる時間

    public float npcMoveSpeed;//NPCの移動速度

    public float npcJumpPower;//NPCのジャンプ力

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    private void Awake()
    {
        //以下、シングルトンに必須の記述
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
