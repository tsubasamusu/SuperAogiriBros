using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;//�ړ����x�i���j

    [SerializeField]
    private Rigidbody rb;//RigidBody

    private float moveDirection;//�ړ�����

    private Vector3 movement;//�ړ��ʒu

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// �v���[���[�̍s���𐧌䂷��
    /// </summary>
    private void Move()
    {
        //�ړ��������擾
        moveDirection = Input.GetAxis("Horizontal");

        //TODO:GameData����ړ����x���擾���鏈��

        //�ړ��ʒu��ݒ�
        movement = new Vector3(-1f,transform.position.y,0f) * moveDirection * moveSpeed * Time.fixedDeltaTime;

        //�W�����v�L�[�������ꂽ��
        if(Input.GetAxis("Vertical")>0)
        {

        }
    }
}
