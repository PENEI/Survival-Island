using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    [SerializeField]
    string mixserName;

    [SerializeField]
    string optionName;

    [SerializeField]
    InputField m_Input;
    [SerializeField]
    Slider m_Slider;

    int count;               //사운드 값
    int max_count;          //최대

    bool isInit;        //초기화했는지확인


    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (isInit)
            return;

        //m_Input = GetComponentInChildren<InputField>();
        //m_Slider = GetComponentInChildren<Slider>();
        max_count = (int)m_Slider.maxValue;
        count = Mathf.Clamp(PlayerPrefs.GetInt(optionName, 100), 0, max_count);
        
        m_Slider.value = count; //슬라이더 값 변경
        m_Input.text = count.ToString();    //인풋필드값 변경

        isInit = true;
    }

    public void SettingSound()
    {
        SoundManager.Instance.SetSound(mixserName, count);
    }

    //인풋필드 값 변경시
    public void _On_InputFied_ValChange()
    {
        int tempval;
        if (int.TryParse(m_Input.text, out tempval))
        {
            SetTextCount(tempval);  //값 변경 확인후 값
        }

        m_Slider.value = count; //슬라이더 값 변경
        SaveOption();
        SoundManager.Instance.SetSound(mixserName, count);
    }

    //슬라이더 값 변경 시
    public void _On_Slider_Value_Change()
    {
        count = (int)m_Slider.value;            //현재 값
        m_Input.text = count.ToString();    //인풋필드값 변경
        SaveOption();
        SoundManager.Instance.SetSound(mixserName, count);
    }

    void SetValue()
    {
        m_Slider.value = count; //슬라이더 값 변경
        //m_Input.text = count.ToString();    //인풋필드값 변경
    }

    //값이 허용되는 값인지 체크
    void SetTextCount(int _count)
    {
        if (_count > max_count)
        {
            m_Input.text = max_count.ToString();
            count = max_count;
        }
        else if (_count < 0)
        {
            m_Input.text = 0.ToString();
            count = 0;
        }
        else
        {
            m_Input.text = _count.ToString();
            count = _count;
        }
    }

    void SaveOption()
    {
        PlayerPrefs.SetInt(optionName, count);  
    }
}
