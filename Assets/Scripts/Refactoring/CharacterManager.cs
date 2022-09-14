using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

namespace yamap {

    public class CharacterManager : MonoBehaviour {

        ///// <summary>
        ///// �L�����N�^�[�̖��O
        ///// </summary>
        //public enum CharaName
        //{
        //    Tamako,//���q
        //    Mashiro//�^��
        //}

        ///// <summary>
        ///// �L�[�̎��
        ///// </summary>
        //public enum KeyType
        //{
        //    Up,Down,Right,Left//�񋓎q
        //}

        ///// <summary>
        ///// �L�����N�^�[�̃f�[�^�Ǘ��p
        ///// </summary>
        //[Serializable]
        //public class CharaData
        //{
        //    public CharaName charaName;//�L�����N�^�[�̖��O
        //    public KeyType keyType;//�L�[�̎��
        //    public KeyCode key;//�L�[
        //}

        //public List<CharaData> charaDataList = new();//�L�����N�^�[�̃f�[�^�̃��X�g

        ///// <summary>
        ///// �w�肵�������ɍ����L�[���擾����
        ///// </summary>
        ///// <param name="charaName">�L�����N�^�[�̖��O</param>
        ///// <param name="keyType">�L�[�̎��</param>
        ///// <returns>�w�肵�������ɍ����L�[</returns>
        //public KeyCode GetCharacterControllerKey(CharaName charaName,KeyType keyType)
        ////{
        ////    //�w�肵�������ɍ����L�[��Ԃ�
        ////    return charaDataList.Find(x => x.charaName == charaName&& x.keyType == keyType).key;
        ////}

        ///// <summary>
        ///// NPCController�̃f�[�^�Ǘ��p
        ///// </summary>
        //[Serializable]
        //public class NPCControllerData
        //{
        //    public CharaName charaName;//�L�����N�^�[�̖��O
        //    public NPCController npcController;//NPCController
        //}

        //public List<NPCControllerData> npcControllerDataList = new();//NPCController�̃f�[�^�̃��X�g

        ///// <summary>
        ///// �w�肵���L�����N�^�[��NPCController���擾����
        ///// </summary>
        ///// <param name="charaName">�L�����N�^�[�̖��O</param>
        ///// <returns>�w�肵���L�����N�^�[��NPCController</returns>
        //public NPCController GetNpcController(CharaName charaName)
        //{
        //    //�w�肵���L�����N�^�[��NPCController��Ԃ�
        //    return npcControllerDataList.Find(x=>x.charaName==charaName).npcController;
        //}

        ///// <summary>
        ///// CharacterController�̃f�[�^�Ǘ��p
        ///// </summary>
        //[Serializable]
        //public class CharacterControllerData
        //{
        //    public CharaName charaName;//�L�����N�^�[�̖��O
        //    public Tsubasa.CharacterController characterController;//CharacterController
        //}

        //public List<CharacterControllerData> characterControllerDataList = new();//CharacterController�̃f�[�^�̃��X�g

        /// <summary>
        /// �L�����̏����܂Ƃ߂��N���X
        /// </summary>
        [Serializable]
        public class CharaControllerBaseData {
            public CharaData charaData;
            public CharaControllerBase charaControllerBase;
            public CharacterHealth characterHealth;
        }
        public List<CharaControllerBaseData> charaList = new();

        public CharaDataSO charaDataSO;

        /// <summary>
        /// CharaData �擾
        /// </summary>
        /// <param name="searchCharaName"></param>
        /// <returns></returns>
        public CharaData GetCharaData(CharaName searchCharaName) {
            return charaDataSO.charaDataList.Find(x => x.charaName == searchCharaName);
        }

        /// <summary>
        /// CharacterControllerBase �擾
        /// </summary>
        /// <param name="searchCharaName"></param>
        /// <returns></returns>
        public CharaControllerBase GetCharaControllerBase(CharaName searchCharaName) {
            return charaList[(int)searchCharaName].charaControllerBase;
        }

        /// <summary>
        /// �w�肵���L�����N�^�[��CharacterHealth���擾����
        /// </summary>
        /// <param name="charaName">�L�����N�^�[�̖��O</param>
        /// <returns>�w�肵���L�����N�^�[��CharacterHealth</returns>
        public CharacterHealth GetCharacterHealth(CharaName charaName) {
            //�w�肵���L�����N�^�[��CharacterHealth��Ԃ�
            return charaList[(int)charaName].characterHealth;
        }

        ///// <summary>
        ///// �w�肵���L�����N�^�[��CharacterController���擾����
        ///// </summary>
        ///// <param name="charaName">�L�����N�^�[�̖��O</param>
        ///// <returns>�w�肵���L�����N�^�[��CharacterController</returns>
        //public Tsubasa.CharacterController GetCharacterController(CharaName charaName)
        //{
        //    //�w�肵���L�����N�^�[��CharacterController��Ԃ�
        //    return characterControllerDataList.Find(x=>x.charaName == charaName).characterController;
        //}
    }
}