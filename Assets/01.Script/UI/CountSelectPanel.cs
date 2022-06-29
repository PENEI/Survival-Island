using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSelectPanel : MonoBehaviour
{
    public bool isMakingResult; //제작인지 확인

    [SerializeField]
    InputField m_CountInput;

    Slot m_DestSlot;         //드랍 슬롯
    Slot m_OriginSlot;       //이동된 슬롯
    int count;               //갯수

    void Awake()
    {
        m_CountInput = gameObject.GetComponentInChildren<InputField>();
    }

    /// <summary>
    /// 패널 세팅
    /// </summary>
    /// <param name="_destslot">드랍된 슬롯</param>
    /// <param name="_originslot">이동된 슬롯</param>
    /// <param name="_pos">마우스위치</param>
    public void SetCountSelect(Slot _destslot, Slot _originslot, Vector2 _pos)
    {
        this.gameObject.transform.position = _pos;
        m_DestSlot = _destslot;
        m_OriginSlot = _originslot;
        UIManager.Instance.SetActiveUI(gameObject, true);
        m_CountInput.text = m_OriginSlot.ItemInfo.count.ToString();      //아이템 갯수 최대로 세팅
        count = m_OriginSlot.ItemInfo.count;   //갯수 세팅
    }

    //값이 바뀔때
    public void _On_ValChange()
    {
        int tempval;
        if (int.TryParse(m_CountInput.text, out tempval))
        {
            SetTextCount(tempval);
        }
    }

    //증가 버튼
    public void _On_Inc()
    {
        SetTextCount(++count);
    }

    //감소 버튼
    public void _On_Dec()
    {
        SetTextCount(--count);
    }

    //예 버튼
    public void _On_OK()
    {
        //아이템 세팅
        m_DestSlot.SetSlot(new ItemInfo(m_OriginSlot.ItemInfo.itemID, count, m_OriginSlot.ItemInfo.durability));
        //갯수 만큼 아이템 제거
        //제작결과 슬롯일 경우
        if (isMakingResult)
        {
            UIManager.Instance.makingPanelList.SetResultCount(count);   //제작아이템 갯수 세팅
            UIManager.Instance.makingPanelList.GetResultItem();            //제작슬롯에서 아이템 얻기
        }
        else
        {
            m_OriginSlot.SetCount(m_OriginSlot.ItemInfo.count - count);
        }
        OffCountSelectPanle();
    }

    //아니오 버튼
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

    //값이 허용되는 값인지 체크
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
