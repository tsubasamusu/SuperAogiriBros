using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharaDataSO", menuName = "Create CharaDataSO")]
public class CharaDataSO : ScriptableObject
{
    public List<CharaData> charaDataList = new();


    /// <summary>
    /// キャラデータの取得
    /// </summary>
    /// <param name="searchCharaName"></param>
    /// <returns></returns>
    public CharaData GetCharaData(CharaName searchCharaName) {
        return charaDataList.Find(x => x.charaName == searchCharaName);
    }
}