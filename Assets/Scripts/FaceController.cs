using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTran;//カメラの位置情報

    [SerializeField]
    private Transform charaTran;//対象のキャラクターの位置情報

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //顔を常にカメラに向ける
        transform.root.LookAt(cameraTran);

        //顔の位置を対象のキャラクターに合わせる
        transform.position = new Vector3(charaTran.position.x,charaTran.position.y+1.8f,0f);
    }
}
