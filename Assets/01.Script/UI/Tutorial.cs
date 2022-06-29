using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    Image m_TutorialObj;

    [SerializeField]
    TutorialContent tutorialContent;        //�̹��� ��µ� ��ũ�Ѻ�
    [SerializeField]
    TutorialButton[] TRButtonArr;           //��ư��

    public bool isFirstTutorial;        //ù���� Ʃ�丮�� Ȯ��

    private void Awake()
    {
        TRButtonArr = GetComponentsInChildren<TutorialButton>();
    }

    private void Start()
    {
        //��ư�� �̺�Ʈ �߰�
        for (int i = 0; i < TRButtonArr.Length; i++)
        {
            int inx = i;
            TRButtonArr[inx].button.onClick.AddListener(() => SetExplain(TRButtonArr[inx].Img_Explain));
        }
    }

    private void OnEnable()
    {
        SetExplain(TRButtonArr[0].Img_Explain);     //Ʃ�丮�� Ȱ��ȭ �� ù��° ���� ������ ���
    }

    private void OnDisable()
    {
        m_TutorialObj.gameObject.SetActive(false);
    }

    //X��ư
    public void _On_XButton()
    {
        if (isFirstTutorial)
        {
            UIManager.Instance.pausePanel.HidePause();      //�Ͻ����� ��Ȱ��ȭ
        }
    }

    /// <summary>
    /// ��ũ�Ѻ� �̹��� ����
    /// </summary>
    /// <param name="Img_Explain">���� �̹���</param>
    public void SetExplain(Sprite Img_Explain)
    {
        tutorialContent.SetTutorialImg(Img_Explain);
    }

    //ù���� Ʃ�丮�� 
    public void ActiveTutorial(bool _isfirst)
    {
        m_TutorialObj.gameObject.SetActive(true);
        isFirstTutorial = _isfirst;
    }
}
