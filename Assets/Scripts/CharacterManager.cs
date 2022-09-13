using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

public class CharacterManager : MonoBehaviour
{
    /// <summary>
    /// �L�����N�^�[�̖��O
    /// </summary>
    public enum CharaName
    {
        Tamako,//���q
        Mashiro//�^��
    }

    /// <summary>
    /// �L�[�̎��
    /// </summary>
    public enum KeyType
    {
        Up,Down,Right,Left//�񋓎q
    }

    /// <summary>
    /// �L�����N�^�[�̃f�[�^�Ǘ��p
    /// </summary>
    [Serializable]
    public class CharaData
    {
        public CharaName charaName;//�L�����N�^�[�̖��O
        public KeyType keyType;//�L�[�̎��
        public KeyCode key;//�L�[
    }

    public List<CharaData> charaDataList = new();//�L�����N�^�[�̃f�[�^�̃��X�g

    /// <summary>
    /// �w�肵�������ɍ����L�[���擾����
    /// </summary>
    /// <param name="charaName">�L�����N�^�[�̖��O</param>
    /// <param name="keyType">�L�[�̎��</param>
    /// <returns>�w�肵�������ɍ����L�[</returns>
    public KeyCode GetCharacterControllerKey(CharaName charaName,KeyType keyType)
    {
        //�w�肵�������ɍ����L�[��Ԃ�
        return charaDataList.Find(x => x.charaName == charaName&& x.keyType == keyType).key;
    }

    /// <summary>
    /// �L�����N�^�[�ɃA�^�b�`����Ă���N���X�̃f�[�^�̊Ǘ��p
    /// </summary>
    [Serializable]
    public class CharacterClassData
    {
        public CharaName charaName;//�L�����N�^�[�̖��O
        public NPCController npcController;//NPCController
        public Tsubasa.CharacterController characterController;//CharacterController
        public CharacterHealth characterHealth;//CharacterHealth
    }

    public List<CharacterClassData> characterClassDataList = new();//Controller�̃f�[�^�̃��X�g

    /// <summary>
    /// �w�肵���L�����N�^�[��NPCController���擾����
    /// </summary>
    /// <param name="charaName">�L�����N�^�[�̖��O</param>
    /// <returns>�w�肵���L�����N�^�[��NPCController</returns>
    public NPCController GetNpcController(CharaName charaName)
    {
        //�w�肵���L�����N�^�[��NPCController��Ԃ�
        return characterClassDataList.Find(x=>x.charaName==charaName).npcController;
    }

    /// <summary>
    /// �w�肵���L�����N�^�[��CharacterController���擾����
    /// </summary>
    /// <param name="charaName">�L�����N�^�[�̖��O</param>
    /// <returns>�w�肵���L�����N�^�[��CharacterController</returns>
    public Tsubasa.CharacterController GetCharacterController(CharaName charaName)
    {
        //�w�肵���L�����N�^�[��CharacterController��Ԃ�
        return characterClassDataList.Find(x=>x.charaName == charaName).characterController;
    }

    /// <summary>
    /// �w�肵���L�����N�^�[��CharacterHealth���擾����
    /// </summary>
    /// <param name="charaName">�L�����N�^�[�̖��O</param>
    /// <returns>�w�肵���L�����N�^�[��CharacterHealth</returns>
    public CharacterHealth GetCharacterHealth(CharaName charaName)
    {
        //�w�肵���L�����N�^�[��CharacterHealth��Ԃ�
        return characterClassDataList.Find(x=>x.charaName==charaName).characterHealth;
    }
}
