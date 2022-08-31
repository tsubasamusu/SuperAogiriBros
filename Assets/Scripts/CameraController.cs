using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<Transform> targetTransList=new List<Transform>();//映す対象のオブジェクトの位置情報のリスト

    [SerializeField]
    private float smooth;//視点移動の滑らかさ

    /// <summary>
    /// 一定時間ごとに呼び出される
    /// </summary>
    private void FixedUpdate()
    {
        //カメラの移動位置を取得
        Vector3 pos = new Vector3(GetCenterPos().x, GetCenterPos().y, transform.position.z);

        //カメラを滑らかに移動させる
        transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * smooth);
    }

    /// <summary>
    /// 映す対象の中央の座標を取得する
    /// </summary>
    /// <returns>映す対象の中央の座標</returns>
    private Vector3 GetCenterPos()
    {
        //対象物の座標の合計
        Vector3 totalPos = Vector3.zero;

        //対象物の数だけ繰り返す
        for (int i = 0; i < targetTransList.Count; i++)
        {
            //対象物の座標を足していく
            totalPos+=targetTransList[i].position;
        }

        //中央の座標を返す
        return totalPos/targetTransList.Count;
    }
}

