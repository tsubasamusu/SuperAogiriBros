using UnityEngine;
using System;//Serializable�������g�p

/// <summary>
/// �L�����̖��O
/// </summary>
public enum CharaName {
    Tamako,//���q
    Mashiro//�^��
}

/// <summary>
/// �L�����f�[�^
/// </summary>
[Serializable]
public class CharaData
{
    public CharaName charaName;//�L�����̖��O
    public KeyCode[] keys;//�L�[
}