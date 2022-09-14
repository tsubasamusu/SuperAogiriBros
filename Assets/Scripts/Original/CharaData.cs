using UnityEngine;
using System;//Serializable属性を使用

/// <summary>
/// キャラの名前
/// </summary>
public enum CharaName {
    Tamako,//魂子
    Mashiro//真白
}

/// <summary>
/// キャラデータ
/// </summary>
[Serializable]
public class CharaData
{
    public CharaName charaName;//キャラの名前
    public KeyCode[] keys;//キー
}