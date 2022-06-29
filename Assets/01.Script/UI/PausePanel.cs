using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public bool isPause;        //�Ͻ����� ������

    [SerializeField]
    Text SaveCountText;     //���� �ؽ�Ʈ
    [SerializeField]    
    Button SaveButton;      //���� ��ư
    [SerializeField]
    Tutorial m_Tutorial;

    void Awake()
    {
        
    }

    void Start()
    {
    }

    /// <summary>
    /// ���� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="_count">���� Ƚ��</param>
    public void SetSaveCountText(int _count)
    {
        SaveCountText.text = "���� ���� Ƚ��:" + _count;
        //0���ϸ� �����ư Ȱ��ȭx
        if(_count <= 0)
        {
            SaveButton.interactable = false;
        }
    }

    //�Ͻ����� �޴� ���̰�
    public void ShowPause()
    {
        isPause = true;         //�Ͻ� ����
        GameManager.Instance.isUpdating = true;
        Time.timeScale = 0;     //�ð� ���帧
        UIManager.Instance.SetActiveUI(gameObject, true);
    }

    //�Ͻ����� �޴� ����
    public void HidePause()
    {
        isPause = false;        //�Ͻ�����X
        Time.timeScale = 1; //�ð� �������
        UIManager.Instance.SetActiveUI(gameObject, false);
        GameManager.Instance.isUpdating = false;
    }

    //����ϱ� ��ư
    public void _On_ContinueButton()
    {
        HidePause();
    }

    //�����ϱ� ��ư
    public void _On_SaveButton()
    {
        if (XmlManager.Instance.Save() <= 0)
        {
            SaveButton.interactable = false;
        }
    }

    //������ ��ư
    public void _On_QuitButton(string _scenename)
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadGameScene(_scenename, true);
    }
    
    //Ʃ�丮�� ��ư
    public void _On_TutorialButton(bool isfirst)
    {
        m_Tutorial.ActiveTutorial(isfirst);

    }
}
