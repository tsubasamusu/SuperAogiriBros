using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

public class CharacterManager : MonoBehaviour
{
    /// <summary>
    /// キャラクターの名前
    /// </summary>
    public enum CharaName
    {
        Tamako,//魂子
        Mashiro//真白
    }

    /// <summary>
    /// キーの種類
    /// </summary>
    public enum KeyType
    {
        Up,Down,Right,Left//列挙子
    }

    /// <summary>
    /// キャラクターのデータ管理用
    /// </summary>
    [Serializable]
    public class CharaData
    {
        public CharaName charaName;//キャラクターの名前
        public KeyType keyType;//キーの種類
        public KeyCode key;//キー
    }

    public List<CharaData> charaDataList = new();//キャラクターのデータのリスト

    /// <summary>
    /// 指定した条件に合うキーを取得する
    /// </summary>
    /// <param name="charaName">キャラクターの名前</param>
    /// <param name="keyType">キーの種類</param>
    /// <returns>指定した条件に合うキー</returns>
    public KeyCode GetCharacterControllerKey(CharaName charaName,KeyType keyType)
    {
        //指定した条件に合うキーを返す
        return charaDataList.Find(x => x.charaName == charaName&& x.keyType == keyType).key;
    }

    /// <summary>
    /// キャラクターにアタッチされているクラスのデータの管理用
    /// </summary>
    [Serializable]
    public class CharacterClassData
    {
        public CharaName charaName;//キャラクターの名前
        public NPCController npcController;//NPCController
        public Tsubasa.CharacterController characterController;//CharacterController
        public CharacterHealth characterHealth;//CharacterHealth
    }

    public List<CharacterClassData> characterClassDataList = new();//Controllerのデータのリスト

    /// <summary>
    /// 指定したキャラクターのNPCControllerを取得する
    /// </summary>
    /// <param name="charaName">キャラクターの名前</param>
    /// <returns>指定したキャラクターのNPCController</returns>
    public NPCController GetNpcController(CharaName charaName)
    {
        //指定したキャラクターのNPCControllerを返す
        return characterClassDataList.Find(x=>x.charaName==charaName).npcController;
    }

    /// <summary>
    /// 指定したキャラクターのCharacterControllerを取得する
    /// </summary>
    /// <param name="charaName">キャラクターの名前</param>
    /// <returns>指定したキャラクターのCharacterController</returns>
    public Tsubasa.CharacterController GetCharacterController(CharaName charaName)
    {
        //指定したキャラクターのCharacterControllerを返す
        return characterClassDataList.Find(x=>x.charaName == charaName).characterController;
    }

    /// <summary>
    /// 指定したキャラクターのCharacterHealthを取得する
    /// </summary>
    /// <param name="charaName">キャラクターの名前</param>
    /// <returns>指定したキャラクターのCharacterHealth</returns>
    public CharacterHealth GetCharacterHealth(CharaName charaName)
    {
        //指定したキャラクターのCharacterHealthを返す
        return characterClassDataList.Find(x=>x.charaName==charaName).characterHealth;
    }
}
