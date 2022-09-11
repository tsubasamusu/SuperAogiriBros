using System.Collections;
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
    public KeyCode GetKey(CharaName charaName,KeyType keyType)
    {
        //�w�肵�������ɍ����L�[��Ԃ�
        return charaDataList.Find(x => x.charaName == charaName&& x.keyType == keyType).key;
    }
}
