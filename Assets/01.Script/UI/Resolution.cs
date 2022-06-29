using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    List<Dropdown.OptionData> m_optionList;     //�ɼ� ����Ʈ

    Toggle m_Toggle_FullScreen;         //��üȭ��
    Dropdown m_Dropdown_Screen;     //�ػ� ����

    bool isInit;        //�ʱ�ȭ�ߴ���Ȯ��

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (isInit)
            return;

        m_Toggle_FullScreen = GetComponentInChildren<Toggle>();
        m_Dropdown_Screen = GetComponentInChildren<Dropdown>();

        //�̺�Ʈ �߰�
        m_Toggle_FullScreen.onValueChanged.AddListener(delegate {
            _On_ValueChanged_FullScreenToggle(m_Toggle_FullScreen.isOn);
        });

        m_Dropdown_Screen.onValueChanged.AddListener(delegate {
            _On_ValueChanged_Screen(m_Dropdown_Screen.value);
        });

        m_optionList = m_Dropdown_Screen.options;

        isInit = true;
    }

    /// <summary>
    /// ȭ�� ����
    /// </summary>
    /// <param name="_h">����</param>
    /// <param name="_w">����</param>
    /// <param name="_full">��üȭ�� ����</param>
    public void ScreenResolution(int _h, int _w, bool _full)
    {
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(_w, _h, _full);
        SaveScreen(_full, m_Dropdown_Screen.value);
    }

    /// <summary>
    /// ȭ�� ����
    /// </summary>
    /// <param name="_inx">�ε���</param>
    /// <param name="_full">��üȭ�� ����</param>
    public void ScreenResolutionInx(int _inx, bool _full)
    {
        m_Toggle_FullScreen.isOn = _full;
        m_Dropdown_Screen.value = Mathf.Clamp(_inx,0,m_optionList.Count-1);
        //StringToHW(m_optionList[_inx].text, out int h, out int w);
        //ScreenResolution(h, w, Screen.fullScreen);
    }

    //����
    void SaveScreen(bool _full, int _inx)
    {
        PlayerPrefs.SetInt("FullScreen", System.Convert.ToInt32(_full));    //bool�� ����
        PlayerPrefs.SetInt("Screen", _inx); //�ε��� ����
    }

    //Ǯ��ũ��
    void _On_ValueChanged_FullScreenToggle(bool _full)
    {
        ScreenResolution(Screen.height, Screen.width, _full);
    }

    //ȭ�麯��
    void _On_ValueChanged_Screen(int _index)
    {
        StringToHW(m_optionList[_index].text, out int h, out int w);
        ScreenResolution(h,w, Screen.fullScreen);
    }

    //String�� int,int�� ��ȯ
    static void StringToHW(string str, out int _h, out int _w)
    {
        // split the items
        string[] sArray = str.Split('x');
        _w = int.Parse(sArray[0]);
        _h = int.Parse(sArray[1]);

    }
}
