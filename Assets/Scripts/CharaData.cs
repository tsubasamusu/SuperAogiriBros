using UnityEngine;

/// <summary>
/// �L�����̖��O
/// </summary>
public enum CharaName {
    Tamako,
    Mashiro
}

/// <summary>
/// �L�����f�[�^
/// </summary>
[System.Serializable]
public class CharaData
{
    public CharaName charaName;
    public KeyCode[] keys;
}