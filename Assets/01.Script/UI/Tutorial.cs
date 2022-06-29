using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    Image m_TutorialObj;

    [SerializeField]
    TutorialContent tutorialContent;        //이미지 출력될 스크롤뷰
    [SerializeField]
    TutorialButton[] TRButtonArr;           //버튼들

    public bool isFirstTutorial;        //첫시작 튜토리얼 확인

    private void Awake()
    {
        TRButtonArr = GetComponentsInChildren<TutorialButton>();
    }

    private void Start()
    {
        //버튼들 이벤트 추가
        for (int i = 0; i < TRButtonArr.Length; i++)
        {
            int inx = i;
            TRButtonArr[inx].button.onClick.AddListener(() => SetExplain(TRButtonArr[inx].Img_Explain));
        }
    }

    private void OnEnable()
    {
        SetExplain(TRButtonArr[0].Img_Explain);     //튜토리얼 활성화 시 첫번째 설명 페이지 출력
    }

    private void OnDisable()
    {
        m_TutorialObj.gameObject.SetActive(false);
    }

    //X버튼
    public void _On_XButton()
    {
        if (isFirstTutorial)
        {
            UIManager.Instance.pausePanel.HidePause();      //일시정지 비활성화
        }
    }

    /// <summary>
    /// 스크롤뷰 이미지 세팅
    /// </summary>
    /// <param name="Img_Explain">설명 이미지</param>
    public void SetExplain(Sprite Img_Explain)
    {
        tutorialContent.SetTutorialImg(Img_Explain);
    }

    //첫시작 튜토리얼 
    public void ActiveTutorial(bool _isfirst)
    {
        m_TutorialObj.gameObject.SetActive(true);
        isFirstTutorial = _isfirst;
    }
}
