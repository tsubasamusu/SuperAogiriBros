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
    private Sprite sprTitle;//�^�C�g��

    [SerializeField]
    private Sprite sprModeSelect;//���[�h�I�����

    [SerializeField]
    private Sprite sprCharaSelect;//�L�����N�^�[�I�����

    private IEnumerator Start()
    {
        yield return StartCoroutine(PlayGameStart());

        yield return StartCoroutine(SetModeSelect());

        yield return StartCoroutine(SetCharaSelect());

        yield return StartCoroutine(GoToGame());

        Debug.Log("end");
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
}
