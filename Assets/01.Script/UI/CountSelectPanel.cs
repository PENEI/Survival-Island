using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSelectPanel : MonoBehaviour
{
    public bool isMakingResult; //�������� Ȯ��

    [SerializeField]
    InputField m_CountInput;

    Slot m_DestSlot;         //��� ����
    Slot m_OriginSlot;       //�̵��� ����
    int count;               //����

    void Awake()
    {
        m_CountInput = gameObject.GetComponentInChildren<InputField>();
    }

    /// <summary>
    /// �г� ����
    /// </summary>
    /// <param name="_destslot">����� ����</param>
    /// <param name="_originslot">�̵��� ����</param>
    /// <param name="_pos">���콺��ġ</param>
    public void SetCountSelect(Slot _destslot, Slot _originslot, Vector2 _pos)
    {
        this.gameObject.transform.position = _pos;
        m_DestSlot = _destslot;
        m_OriginSlot = _originslot;
        UIManager.Instance.SetActiveUI(gameObject, true);
        m_CountInput.text = m_OriginSlot.ItemInfo.count.ToString();      //������ ���� �ִ�� ����
        count = m_OriginSlot.ItemInfo.count;   //���� ����
    }

    //���� �ٲ�
    public void _On_ValChange()
    {
        int tempval;
        if (int.TryParse(m_CountInput.text, out tempval))
        {
            SetTextCount(tempval);
        }
    }

    //���� ��ư
    public void _On_Inc()
    {
        SetTextCount(++count);
    }

    //���� ��ư
    public void _On_Dec()
    {
        SetTextCount(--count);
    }

    //�� ��ư
    public void _On_OK()
    {
        //������ ����
        m_DestSlot.SetSlot(new ItemInfo(m_OriginSlot.ItemInfo.itemID, count, m_OriginSlot.ItemInfo.durability));
        //���� ��ŭ ������ ����
        //���۰�� ������ ���
        if (isMakingResult)
        {
            UIManager.Instance.makingPanelList.SetResultCount(count);   //���۾����� ���� ����
            UIManager.Instance.makingPanelList.GetResultItem();            //���۽��Կ��� ������ ���
        }
        else
        {
            m_OriginSlot.SetCount(m_OriginSlot.ItemInfo.count - count);
        }
        OffCountSelectPanle();
    }

    //�ƴϿ� ��ư
    public void _On_Cancle()
    {
        OffCountSelectPanle();
    }

    public void OffCountSelectPanle()
    {
        m_DestSlot = null;
        m_OriginSlot = null;
        isMakingResult = false;
        UIManager.Instance.SetActiveUI(gameObject, false);
    }

    //���� ���Ǵ� ������ üũ
    void SetTextCount(int _count)
    {
        if (_count > m_OriginSlot.ItemInfo.count)
        {
            m_CountInput.text = m_OriginSlot.ItemInfo.count.ToString();
            count = m_OriginSlot.ItemInfo.count;
        }
        else if (_count < 0)
        {
            m_CountInput.text = 0.ToString();
            count = 0;
        }
        else
        {
            m_CountInput.text = _count.ToString();
            count = _count;
        }
    }
}
