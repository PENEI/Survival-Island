using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    List<Dropdown.OptionData> m_optionList;     //옵션 리스트

    Toggle m_Toggle_FullScreen;         //전체화면
    Dropdown m_Dropdown_Screen;     //해상도 설정

    bool isInit;        //초기화했는지확인

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

        //이벤트 추가
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
    /// 화면 변경
    /// </summary>
    /// <param name="_h">세로</param>
    /// <param name="_w">가로</param>
    /// <param name="_full">전체화면 여부</param>
    public void ScreenResolution(int _h, int _w, bool _full)
    {
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(_w, _h, _full);
        SaveScreen(_full, m_Dropdown_Screen.value);
    }

    /// <summary>
    /// 화면 변경
    /// </summary>
    /// <param name="_inx">인덱스</param>
    /// <param name="_full">전체화면 여부</param>
    public void ScreenResolutionInx(int _inx, bool _full)
    {
        m_Toggle_FullScreen.isOn = _full;
        m_Dropdown_Screen.value = Mathf.Clamp(_inx,0,m_optionList.Count-1);
        //StringToHW(m_optionList[_inx].text, out int h, out int w);
        //ScreenResolution(h, w, Screen.fullScreen);
    }

    //저장
    void SaveScreen(bool _full, int _inx)
    {
        PlayerPrefs.SetInt("FullScreen", System.Convert.ToInt32(_full));    //bool값 저장
        PlayerPrefs.SetInt("Screen", _inx); //인덱스 저장
    }

    //풀스크린
    void _On_ValueChanged_FullScreenToggle(bool _full)
    {
        ScreenResolution(Screen.height, Screen.width, _full);
    }

    //화면변경
    void _On_ValueChanged_Screen(int _index)
    {
        StringToHW(m_optionList[_index].text, out int h, out int w);
        ScreenResolution(h,w, Screen.fullScreen);
    }

    //String을 int,int로 변환
    static void StringToHW(string str, out int _h, out int _w)
    {
        // split the items
        string[] sArray = str.Split('x');
        _w = int.Parse(sArray[0]);
        _h = int.Parse(sArray[1]);

    }
}
