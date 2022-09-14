using UnityEngine;

/// <summary>
/// キャラの名前
/// </summary>
public enum CharaName {
    Tamako,
    Mashiro
}

/// <summary>
/// キャラデータ
/// </summary>
[System.Serializable]
public class CharaData
{
    public CharaName charaName;
    public KeyCode[] keys;
}