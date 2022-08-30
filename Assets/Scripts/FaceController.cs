using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTran;//�J�����̈ʒu���

    [SerializeField]
    private Transform charaTran;//�Ώۂ̃L�����N�^�[�̈ʒu���

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�����ɃJ�����Ɍ�����
        transform.root.LookAt(cameraTran);

        //��̈ʒu��Ώۂ̃L�����N�^�[�ɍ��킹��
        transform.position = new Vector3(charaTran.position.x,charaTran.position.y+1.8f,0f);
    }
}
