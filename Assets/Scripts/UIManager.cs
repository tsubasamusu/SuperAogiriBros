using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image imgBackground;//�w�i

    [SerializeField]
    private Image imgLogo;//���S

    [SerializeField]
    private Image imgPicture;//PNG�\���p

    [SerializeField]
    private Text txtMessage;//���b�Z�[�W

    [SerializeField]
    private Sprite sprTitle;//�^�C�g��

    [SerializeField]
    private Sprite sprModeSelect;//���[�h�I�����

    [SerializeField]
    private Sprite sprCharaSelect;//�L�����N�^�[�I�����

    private float time = 3.5f;//�J�E���g�_�E���p

    private IEnumerator Start()
    {
        yield return StartCoroutine(PlayGameStart());

        yield return StartCoroutine(SetModeSelect());

        yield return StartCoroutine(SetCharaSelect());

        yield return StartCoroutine(GoToGame());

        yield return StartCoroutine(CountDown());

        Debug.Log("GameStart");

        yield return StartCoroutine(EndGame());

        Debug.Log("End");
    }

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameStart()
    {
        //�Q�[���X�^�[�g���o���I��������ǂ���
        bool endGameStart = false;

        //���S���^�C�g���ɐݒ�
        imgLogo.sprite = sprTitle;

        //���S���\���ɂ���
        imgLogo.DOFade(0f, 0f);

        //�w�i�𔒐F�ɐݒ�
        imgBackground.color = Color.white;

        //�w�i��\��
        imgBackground.DOFade(1f, 0f);

        //���S��\�����āA����
        imgLogo.DOFade(1f, 1f).SetLoops(2,LoopType.Yoyo).OnComplete(() => { endGameStart = true; });

        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return new WaitUntil(()=>endGameStart);
    }

    /// <summary>
    /// ���[�h�I����ʂ�\������
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator SetModeSelect()
    {
        //���o���I��������ǂ���
        bool end=false;

        //���[�h�I����ʂ̉摜��ݒ�
        imgPicture.sprite = sprModeSelect;

        //�ݒ肵���摜��\��
        imgPicture.DOFade(1f,1f).OnComplete(() => end = true);

        //���o���I���܂ő҂�
        yield return new WaitUntil(() => end);
    }

    /// <summary>
    /// �L�����N�^�[�I����ʂ�\������
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator SetCharaSelect()
    {
        //���o���I��������ǂ���
        bool end = false;

        //���[�h�I����ʂ��\���ɂ���
        imgPicture.DOFade(0f, 0.5f).

            //��ʂ��L�����N�^�[�I����ʂɐݒ肵�A�\��
            OnComplete(() =>{ { imgPicture.sprite = sprCharaSelect; }{ imgPicture.DOFade(1f, 0.5f).OnComplete(()=>end=true); } });

        //���o���I���܂ő҂�
        yield return new WaitUntil(()=>end);
    }

    /// <summary>
    /// ������ʂֈȍ~����
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator GoToGame()
    {
        //���o���I��������ǂ���
        bool end = false;

        //�I����ʂ��\���ɂ���
        imgPicture.DOFade(0f, 1f);

        //�w�i�𓧖��ɂ���
        imgBackground.DOFade(0f, 1f).OnComplete(()=>end=true);

        //���o���I���܂ő҂�
        yield return new WaitUntil(()=>end);
    }

    /// <summary>
    /// �����J�n�O�̃J�E���g�_�E�����s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator CountDown()
    {
        //���b�Z�[�W�̐F��ΐF�ɐݒ�
        txtMessage.color = Color.green;

        //���b�Z�[�W��\������
        txtMessage.DOFade(1f, 0f);

        //�����ɌJ��Ԃ�
        while(true)
        {
            //�J�E���g�_�E������
            time -= Time.deltaTime;

            //�J�E���g�_�E����0.5�ȉ��ɂȂ�����
            if(time<=0.5f)
            {
                //GO�ƕ\������
                txtMessage.text = "GO";

                //���b�Z�[�W���\���ɂ���
                txtMessage.DOFade(0f, 1f).OnComplete(() => txtMessage.text = "");

                //�J��Ԃ��������I������
                break;
            }

            //�J�E���g�_�E����\������
            txtMessage.text = time.ToString("F0");

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// �uGame Set�v�ƕ\������
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator EndGame()
    {
        //���o���I��������ǂ���
        bool end = false;

        //���b�Z�[�W�̐F��F�ɐݒ�
        txtMessage.color = Color.blue;

        //�uGame Set�v�Ƃ���������ݒ肷��
        txtMessage.text = "Game Set";

        //������\�����ď���
        txtMessage.DOFade(1f, 0.5f).OnComplete(()=>txtMessage.DOFade(0f,1f).OnComplete(() => end = true));

        //���o���I���܂ő҂�
        yield return new WaitUntil(() => end);
    }
}
