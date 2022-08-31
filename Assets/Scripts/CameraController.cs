using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> targetTransList=new List<Transform>();//�f���Ώۂ̃I�u�W�F�N�g�̈ʒu���̃��X�g

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�J�������ړ�������
        transform.position = new Vector3(GetCenterPos().x, GetCenterPos().y, transform.position.z);
    }

    /// <summary>
    /// �f���Ώۂ̒����̍��W���擾����
    /// </summary>
    /// <returns>�f���Ώۂ̒����̍��W</returns>
    private Vector3 GetCenterPos()
    {
        //�Ώە��̍��W�̍��v
        Vector3 totalPos = Vector3.zero;

        //�Ώە��̐������J��Ԃ�
        for (int i = 0; i < targetTransList.Count; i++)
        {
            //�Ώە��̍��W�𑫂��Ă���
            totalPos+=targetTransList[i].position;
        }

        //�����̍��W��Ԃ�
        return totalPos/targetTransList.Count;
    }
}

