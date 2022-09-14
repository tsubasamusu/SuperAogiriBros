using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

namespace yamap 
{
    /// <summary>
    /// �L�����N�^�[�̃f�[�^�̎擾�E�Ǘ����s��
    /// </summary>
    public class CharacterManager : MonoBehaviour 
    {
        /// <summary>
        /// �L�����̏����Ǘ�
        /// </summary>
        [Serializable]
        public class CharaControllerBaseData 
        {
            public CharaData charaData;//�L�����N�^�[�̃f�[�^
            public CharaControllerBase charaControllerBase;//CharaControllerBase
            public CharacterHealth characterHealth;//CharacterHealth
        }

        public List<CharaControllerBaseData> charaList = new();//�L�����̏��̃��X�g

        public CharaDataSO charaDataSO;//CharaDataSO

        /// <summary>
        /// �w�肵���L�����N�^�[�̃f�[�^���擾����
        /// </summary>
        /// <param name="searchCharaName">�L�����N�^�[�̖��O</param>
        /// <returns>�w�肵���L�����N�^�[�̃f�[�^</returns>
        public CharaData GetCharaData(CharaName searchCharaName) 
        {
            //�w�肵���L�����N�^�[�̃f�[�^��Ԃ�
            return charaDataSO.charaDataList.Find(x => x.charaName == searchCharaName);
        }

        /// <summary>
        /// �w�肵���L�����N�^�[��CharacterControllerBase���擾����
        /// </summary>
        /// <param name="searchCharaName">�L�����N�^�[�̖��O</param>
        /// <returns>�w�肵���L�����N�^�[��CharacterControllerBase</returns>
        public CharaControllerBase GetCharaControllerBase(CharaName searchCharaName) 
        {
            //�w�肵���L�����N�^�[��CharacterControllerBase��Ԃ�
            return charaList[(int)searchCharaName].charaControllerBase;
        }

        /// <summary>
        /// �w�肵���L�����N�^�[��CharacterHealth���擾����
        /// </summary>
        /// <param name="charaName">�L�����N�^�[�̖��O</param>
        /// <returns>�w�肵���L�����N�^�[��CharacterHealth</returns>
        public CharacterHealth GetCharacterHealth(CharaName charaName) 
        {
            //�w�肵���L�����N�^�[��CharacterHealth��Ԃ�
            return charaList[(int)charaName].characterHealth;
        }
    }
}