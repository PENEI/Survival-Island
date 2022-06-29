using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialContent : MonoBehaviour
{
    Image m_Img;              
    RectTransform m_Rect;
    bool init;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if(!init)
        {
            init = true;
            m_Img = GetComponentInChildren<Image>();
            m_Rect = GetComponent<RectTransform>();
        }

    }


    /// <summary>
    /// 이미지 세팅
    /// </summary>
    /// <param name="_sprite">이미지</param>
    public void SetTutorialImg(Sprite _sprite)
    {
        Init();
        m_Img.sprite = _sprite;
        m_Rect.sizeDelta = new Vector2(m_Rect.sizeDelta.x, m_Img.preferredHeight);
    }
}
