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

    int count;               //���� ��
    int max_count;          //�ִ�

    bool isInit;        //�ʱ�ȭ�ߴ���Ȯ��


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
        
        m_Slider.value = count; //�����̴� �� ����
        m_Input.text = count.ToString();    //��ǲ�ʵ尪 ����

        isInit = true;
    }

    public void SettingSound()
    {
        SoundManager.Instance.SetSound(mixserName, count);
    }

    //��ǲ�ʵ� �� �����
    public void _On_InputFied_ValChange()
    {
        int tempval;
        if (int.TryParse(m_Input.text, out tempval))
        {
            SetTextCount(tempval);  //�� ���� Ȯ���� ��
        }

        m_Slider.value = count; //�����̴� �� ����
        SaveOption();
        SoundManager.Instance.SetSound(mixserName, count);
    }

    //�����̴� �� ���� ��
    public void _On_Slider_Value_Change()
    {
        count = (int)m_Slider.value;            //���� ��
        m_Input.text = count.ToString();    //��ǲ�ʵ尪 ����
        SaveOption();
        SoundManager.Instance.SetSound(mixserName, count);
    }

    void SetValue()
    {
        m_Slider.value = count; //�����̴� �� ����
        //m_Input.text = count.ToString();    //��ǲ�ʵ尪 ����
    }

    //���� ���Ǵ� ������ üũ
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
