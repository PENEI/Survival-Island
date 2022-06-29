using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public bool isPause;        //일시정지 중인지

    [SerializeField]
    Text SaveCountText;     //저장 텍스트
    [SerializeField]    
    Button SaveButton;      //저장 버튼
    [SerializeField]
    Tutorial m_Tutorial;

    void Awake()
    {
        
    }

    void Start()
    {
    }

    /// <summary>
    /// 저장 텍스트 세팅
    /// </summary>
    /// <param name="_count">저장 횟수</param>
    public void SetSaveCountText(int _count)
    {
        SaveCountText.text = "남은 저장 횟수:" + _count;
        //0이하면 저장버튼 활성화x
        if(_count <= 0)
        {
            SaveButton.interactable = false;
        }
    }

    //일시정지 메뉴 보이게
    public void ShowPause()
    {
        isPause = true;         //일시 정지
        GameManager.Instance.isUpdating = true;
        Time.timeScale = 0;     //시간 안흐름
        UIManager.Instance.SetActiveUI(gameObject, true);
    }

    //일시정지 메뉴 감춤
    public void HidePause()
    {
        isPause = false;        //일시정지X
        Time.timeScale = 1; //시간 원래대로
        UIManager.Instance.SetActiveUI(gameObject, false);
        GameManager.Instance.isUpdating = false;
    }

    //계속하기 버튼
    public void _On_ContinueButton()
    {
        HidePause();
    }

    //저장하기 버튼
    public void _On_SaveButton()
    {
        if (XmlManager.Instance.Save() <= 0)
        {
            SaveButton.interactable = false;
        }
    }

    //나가기 버튼
    public void _On_QuitButton(string _scenename)
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadGameScene(_scenename, true);
    }
    
    //튜토리얼 버튼
    public void _On_TutorialButton(bool isfirst)
    {
        m_Tutorial.ActiveTutorial(isfirst);

    }
}
