using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [Header("여백")]
    public float margine = 10.0f;

    Text m_Text;                        //텍스트
    RectTransform m_ImgRect;        //이미지 렉트트랜스폼

    void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        m_ImgRect = GetComponent<RectTransform>();
    }

    //아이템 툴팁
    public void SetItemTooltip(string _itemname, RectTransform _rect)
    {
        gameObject.SetActive(true);
        m_Text.text = _itemname;        //아이템 이름 텍스트 세팅
        m_ImgRect.sizeDelta = new Vector2(m_Text.preferredWidth + margine,
            m_Text.preferredHeight + margine);      //이미지 사이즈

        //툴팁 이미지 사이즈
        Vector2 rect = new Vector2(m_ImgRect.sizeDelta.x, m_ImgRect.sizeDelta.y);
        rect *= 0.5f;

        //슬롯사이즈
        Vector2 slot = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y);
        slot *= 0.5f;

        //위치
        Vector2 temppos = new Vector2(_rect.gameObject.transform.position.x,
            _rect.gameObject.transform.position.y);

        Vector2 tempsize = new Vector2(rect.x + slot.x, -rect.y + slot.y);

        //화면 넘어가는지 확인
        if (Screen.width < temppos.x + tempsize.x + rect.x)
        {
            tempsize.x *= -1;
        }

        this.transform.position = temppos;
        m_ImgRect.anchoredPosition = m_ImgRect.anchoredPosition + tempsize;
    }

    /// <summary>
    /// 아이템 툴팁 세팅
    /// </summary>
    /// <param name="_itemname">아이템 이름</param>
    /// <param name="_rect">기준 트랜스폼</param>
    /// <param name="_pos">추가 벡터</param>
    public void SetItemTooltip(string _itemname, RectTransform _rect, Vector2 _pos)
    {
        gameObject.SetActive(true);
        m_Text.text = _itemname;        //아이템 이름 텍스트 세팅
        m_ImgRect.sizeDelta = new Vector2(m_Text.preferredWidth + margine,
            m_Text.preferredHeight + margine);      //이미지 사이즈

        //툴팁 이미지 사이즈
        Vector2 rect = new Vector2(m_ImgRect.sizeDelta.x, m_ImgRect.sizeDelta.y);
        rect *= 0.5f;

        //슬롯사이즈
        Vector2 slot = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y);
        slot *= 0.5f;

        //위치
        Vector2 temppos = new Vector2(_rect.gameObject.transform.position.x,
            _rect.gameObject.transform.position.y);

        Vector2 tempsize = new Vector2(rect.x + slot.x, -rect.y + slot.y);

        //화면 넘어가는지 확인
        if (Screen.width < temppos.x + tempsize.x + rect.x)
        {
            tempsize.x *= -1;
        }

        this.transform.position = temppos;
        m_ImgRect.anchoredPosition = m_ImgRect.anchoredPosition + tempsize + _pos;
    }
}
