using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

namespace yamap 
{
    /// <summary>
    /// キャラクターのデータの取得・管理を行う
    /// </summary>
    public class CharacterManager : MonoBehaviour 
    {
        /// <summary>
        /// キャラの情報を管理
        /// </summary>
        [Serializable]
        public class CharaControllerBaseData 
        {
            public CharaData charaData;//キャラクターのデータ
            public CharaControllerBase charaControllerBase;//CharaControllerBase
            public CharacterHealth characterHealth;//CharacterHealth
        }

        public List<CharaControllerBaseData> charaList = new();//キャラの情報のリスト

        public CharaDataSO charaDataSO;//CharaDataSO

        /// <summary>
        /// 指定したキャラクターのデータを取得する
        /// </summary>
        /// <param name="searchCharaName">キャラクターの名前</param>
        /// <returns>指定したキャラクターのデータ</returns>
        public CharaData GetCharaData(CharaName searchCharaName) 
        {
            //指定したキャラクターのデータを返す
            return charaDataSO.charaDataList.Find(x => x.charaName == searchCharaName);
        }

        /// <summary>
        /// 指定したキャラクターのCharacterControllerBaseを取得する
        /// </summary>
        /// <param name="searchCharaName">キャラクターの名前</param>
        /// <returns>指定したキャラクターのCharacterControllerBase</returns>
        public CharaControllerBase GetCharaControllerBase(CharaName searchCharaName) 
        {
            //指定したキャラクターのCharacterControllerBaseを返す
            return charaList[(int)searchCharaName].charaControllerBase;
        }

        /// <summary>
        /// 指定したキャラクターのCharacterHealthを取得する
        /// </summary>
        /// <param name="charaName">キャラクターの名前</param>
        /// <returns>指定したキャラクターのCharacterHealth</returns>
        public CharacterHealth GetCharacterHealth(CharaName charaName) 
        {
            //指定したキャラクターのCharacterHealthを返す
            return charaList[(int)charaName].characterHealth;
        }
    }
}