using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [Header("보여지는 시간")]
    public float ShowDelay = 0.5f;      //보여지는 시간
    [Header("사라지는 시간")]         
    public float HideDelay = 0.5f;       //사라지는 시간
    
    Text MSText;     //텍스트
    Image m_img;

    void Awake()
    {
        MSText = GetComponentInChildren<Text>();
        m_img = GetComponent<Image>();
        SetAlpha(0);
    }

    //메세지 활성화
    public void SetShow(string text)
    {
        MSText.text = text;
        
        //나타남
        m_img.DOFade(1, ShowDelay);
        MSText.DOFade(1, ShowDelay);
        //사라짐
        m_img.DOFade(0, ShowDelay).SetDelay(HideDelay + ShowDelay);
        MSText.DOFade(0, ShowDelay).SetDelay(HideDelay + ShowDelay);

    }

    //이미지,텍스트 알파값 세팅
    void SetAlpha(float a)
    {
        Color tempColor = m_img.color;
        tempColor.a = a;
        m_img.color = tempColor;

        tempColor = MSText.color;
        tempColor.a = a;
        MSText.color = tempColor;

    }
}
