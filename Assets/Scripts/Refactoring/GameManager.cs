using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p
//using DG.Tweening;//DOTween���g�p
using Tsubasa;//CameraController���g�p

namespace yamap {

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private UIManager uIManager;//UIManager

        [SerializeField]
        private CharacterManager characterManager;//CharacterManager

        [SerializeField]
        private CameraController cameraController;//CameraController

        [SerializeField]
        private Transform effectTran;

        private bool isSolo;//�\�����ǂ���

        private bool useTamako;//�\���v���[���[�����q���g�p���邩�ǂ���

        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator Start() {
            //�S�ẴR���g���[���[��񊈐�������i�����O�ɃL�����N�^�[�������Ȃ��悤�ɂ��邽�߁j
            SetControllersFalse();

            ////�L�����N�^�[�̐������J��Ԃ�
            //for (int i = 0; i < characterManager.charaList.Count; i++) {
            //    //CharacterHealth�̏����ݒ���s��
            //    characterManager.GetCharacterHealth((CharaName)i).SetUpCharacterHealth(this, cameraController, effectTran);
            //}

            //�}�E�X�J�[�\�����\���ɂ���
            uIManager.HideCursor();

            //BGM���Đ�
            SoundManager.instance.PlaySound(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Main).clip, true);

            //�Q�[���X�^�[�g���o���I���܂ő҂�
            yield return uIManager.PlayGameStart();

            //���[�h�I����ʂւ̈ڍs���I���܂ő҂�
            yield return uIManager.SetModeSelect();

            //���[�h�I�����I���܂ő҂�
            yield return CheckModeSelect();

            //�f���I�Ȃ�
            if (!isSolo) {
                //BGM��؂�ւ���
                SoundManager.instance.ChangeBgmMainToGame();

                //�������n�܂�܂ő҂�
                yield return StartGame();

                //�L�����N�^�[�̐������J��Ԃ�
                for (int i = 0; i < 2; i++) {
                    //CharacterController������������
                    //characterManager.GetCharacterController((CharacterManager.CharaName)i).enabled = true;

                    // �s�v�ȕ����폜����
                    Destroy(characterManager.charaList[i].charaControllerBase.GetComponent<NPCController>());

                    //CharacterController�̏����ݒ���s��
                    //characterManager.GetCharacterController((CharacterManager.CharaName)i).SetUpCharacterController(characterManager);
                    characterManager.charaList[i].charaControllerBase.GetComponent<CharacterController>().SetUpCharacterController(characterManager.GetCharaData((CharaName)i), OwnerType.Player);
                }

                //�ȍ~�̏������s��Ȃ�
                yield break;
            }

            //�L�����N�^�[�I����ʂւ̈ڍs���I���܂ő҂�
            yield return uIManager.SetCharaSelect();

            //�L�����N�^�I�����I���܂ő҂�
            yield return CheckCharaSelect();

            //BGM��؂�ւ���
            SoundManager.instance.ChangeBgmMainToGame();

            //�������n�܂�܂ő҂�
            yield return StartGame();

            // �ǂ���̃L������I�����Ă��邩
            (CharaName myChara, CharaName npcChara) useChara = useTamako ? (CharaName.Tamako, CharaName.Mashiro) : (CharaName.Mashiro, CharaName.Tamako);

            // �v���C���[���̐ݒ�
            {
                // myChara �� NPCController ���폜
                Destroy(characterManager.GetCharaControllerBase(useChara.myChara).GetComponent<NPCController>());

                // myChara �� CharacterController ��ݒ�
                characterManager.GetCharaControllerBase(useChara.myChara).GetComponent<CharacterController>().SetUpCharacterController(characterManager.GetCharaData(useChara.myChara), OwnerType.Player);

                // myChara �� Health ��ݒ�
                characterManager.GetCharacterHealth(useChara.myChara).SetUpCharacterHealth(this, cameraController, effectTran, characterManager.GetCharaControllerBase(useChara.npcChara));
            }

            // NPC ���̐ݒ�
            {
                // npcChara �� CharacterController ���폜
                Destroy(characterManager.GetCharaControllerBase(useChara.npcChara).GetComponent<CharacterController>());

                // npcChara �� CharacterController ��ݒ�
                characterManager.GetCharaControllerBase(useChara.npcChara).GetComponent<NPCController>().SetUpCharacterController(characterManager.GetCharaData(useChara.npcChara), OwnerType.NPC, characterManager.GetCharaControllerBase(useChara.myChara));

                // npcChara �� Health ��ݒ�
                characterManager.GetCharacterHealth(useChara.npcChara).SetUpCharacterHealth(this, cameraController, effectTran, characterManager.GetCharaControllerBase(useChara.myChara));
            }

            ////�\���v���[���[�����q���g�p����Ȃ�
            //if (useTamako) {
            //    //���q��CharacterController������������
            //    characterManager.GetCharacterController(CharacterManager.CharaName.Tamako).enabled = true;

            //    //CharacterController�̏����ݒ���s��
            //    characterManager.GetCharacterController(CharacterManager.CharaName.Tamako).SetUpCharacterController(characterManager);

            //    //�^����NPCController������������
            //    characterManager.GetNpcController(CharacterManager.CharaName.Mashiro).enabled = true;
            //}
            ////�\���v���[���[���^�����g�p����Ȃ�
            //else {
            //    //�^����CharacterController������������
            //    characterManager.GetCharacterController(CharacterManager.CharaName.Mashiro).enabled = true;

            //    //CharacterController�̏����ݒ���s��
            //    characterManager.GetCharacterController(CharacterManager.CharaName.Mashiro).SetUpCharacterController(characterManager);

            //    //���q��NPCController������������
            //    characterManager.GetNpcController(CharacterManager.CharaName.Tamako).enabled = true;
            //}
        }

        /// <summary>
        /// �S�ẴR���g���[���[��񊈐�������
        /// </summary>
        private void SetControllersFalse() {

            //�L�����N�^�[�̐������J��Ԃ�
            for (int i = 0; i < characterManager.charaList.Count; i++) {
                characterManager.charaList[i].charaControllerBase.enabled = false;
            }

            ////�L�����N�^�[�̐������J��Ԃ�
            //for (int i = 0; i < characterManager.characterClassDataList.Count; i++) {
            //    //CharacterController��񊈐�������
            //    characterManager.GetCharacterController((CharacterManager.CharaName)i).enabled = false;

            //    //NPCController��񊈐�������
            //    characterManager.GetNpcController((CharacterManager.CharaName)i).enabled = false;
            //}
        }

        /// <summary>
        /// �ǂ���̃��[�h��I�����ꂽ���m�F����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator CheckModeSelect() {
            //�����ɌJ��Ԃ�
            while (true) {
                //�u1�v�������ꂽ��
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    //�I�������Đ�����
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                    //�I�����ꂽ���[�h���L������
                    isSolo = true;

                    //�J��Ԃ��������I������
                    break;
                }
                //�u2�v�������ꂽ��
                else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    //�I�������Đ�����
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                    //�I�����ꂽ���[�h���L������
                    isSolo = false;

                    //�J��Ԃ��������I������
                    break;
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }

        /// <summary>
        /// �ǂ���̃L�����N�^�[��I�����ꂽ���m�F����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator CheckCharaSelect() {
            //�����ɌJ��Ԃ�
            while (true) {
                //�u1�v�������ꂽ��
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    //�������Đ�
                    SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.MashiroName).clip);

                    //�I�������Đ�����
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                    //�I�����ꂽ�L�����N�^�[���L������
                    useTamako = false;

                    //�J��Ԃ��������I������
                    break;
                }
                //�u2�v�������ꂽ��
                else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    //�������Đ�
                    SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.TamakoName).clip);

                    //�I�������Đ�����
                    SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                    //�I�����ꂽ�L�����N�^�[���L������
                    useTamako = true;

                    //�J��Ԃ��������I������
                    break;
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }

        /// <summary>
        /// �������J�n����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator StartGame() {
            //������ʂւ̈ڍs���I���܂ő҂�
            yield return uIManager.GoToGame();

            //�������Đ�
            SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.CountDown).clip);

            //�����O�̃J�E���g�_�E�����I���܂ő҂�
            yield return uIManager.CountDown();
        }

        /// <summary>
        /// �Q�[���I���̏������s��
        /// </summary>
        public void SetUpEndGame() {
            //�Q�[�����I������
            StartCoroutine(EndGame());
        }

        /// <summary>
        /// �Q�[�����I������
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator EndGame() {
            //�������Đ�
            SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.GameSet).clip);

            //BGM���t�F�[�h�A�E�g������
            SoundManager.instance.StopSound(0.5f);

            //�Q�[���I�����o���I���܂ő҂�
            yield return uIManager.EndGame();

            //Main�V�[����ǂݍ���
            SceneManager.LoadScene("Main");
        }
    }
}