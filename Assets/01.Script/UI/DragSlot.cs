using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//슬롯 이동 임시 슬롯
public class DragSlot : MonoBehaviour
{
    public Slot dragSlot { get; set; }     //이동하려는 슬롯
    public bool isDraging;                  //드래그 중인지 확인

    Image m_img;                            //이미지
    void Start()
    {
        m_img = GetComponent<Image>();
        SetAlpha(0);            //알파값 0
    }

    //드래그 시작
    public void StartDrag(Image _itemImage, Slot _slot)
    {
        isDraging = true;                           //드래그 중
        dragSlot = _slot;                           //슬롯
        m_img.sprite = _itemImage.sprite;     //이미지
        SetColor(_itemImage.color);                              //알파값
        dragSlot.SetAlpha(0.5f);
    }

    //드래그 종료
    public void EndDrag()
    {
        if(isDraging)
        {
            dragSlot.SetDurColor();

            isDraging = false;                          //드래그 종료
            dragSlot = null;
            SetAlpha(0);                                //알파값
        }
    }

    //알파값 설정
    void SetColor(Color _color)
    {
        m_img.color = new Color(_color.r, _color.g, _color.b, _color.a);
    }

    void SetAlpha(float _alpha)
    {
        m_img.color = new Color(m_img.color.r, m_img.color.g, m_img.color.b, _alpha);
    }
}
