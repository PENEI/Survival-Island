using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� �̵� �ӽ� ����
public class DragSlot : MonoBehaviour
{
    public Slot dragSlot { get; set; }     //�̵��Ϸ��� ����
    public bool isDraging;                  //�巡�� ������ Ȯ��

    Image m_img;                            //�̹���
    void Start()
    {
        m_img = GetComponent<Image>();
        SetAlpha(0);            //���İ� 0
    }

    //�巡�� ����
    public void StartDrag(Image _itemImage, Slot _slot)
    {
        isDraging = true;                           //�巡�� ��
        dragSlot = _slot;                           //����
        m_img.sprite = _itemImage.sprite;     //�̹���
        SetColor(_itemImage.color);                              //���İ�
        dragSlot.SetAlpha(0.5f);
    }

    //�巡�� ����
    public void EndDrag()
    {
        if(isDraging)
        {
            dragSlot.SetDurColor();

            isDraging = false;                          //�巡�� ����
            dragSlot = null;
            SetAlpha(0);                                //���İ�
        }
    }

    //���İ� ����
    void SetColor(Color _color)
    {
        m_img.color = new Color(_color.r, _color.g, _color.b, _color.a);
    }

    void SetAlpha(float _alpha)
    {
        m_img.color = new Color(m_img.color.r, m_img.color.g, m_img.color.b, _alpha);
    }
}
