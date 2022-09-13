using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<Transform> targetTransList=new();//�f���Ώۂ̃I�u�W�F�N�g�̈ʒu���̃��X�g

    [SerializeField]
    private float smooth;//���_�ړ��̊��炩��

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()
    {
        //�Ώە����S�ď�������
        if(targetTransList.Count==0)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�J�����̈ړ��ʒu���擾
        Vector3 pos = new Vector3(GetCenterPos().x, GetCenterPos().y, transform.position.z);

        //�J���������炩�Ɉړ�������
        transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * smooth);
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

