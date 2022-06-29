using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

//인벤토리 패널
public class InvenPanel : MonoBehaviour
{
    public List<Slot> InvenSlotList { get; set; }        //인벤토리 슬롯
    bool isinit;


    private void OnEnable()
    {
        if(isinit)
            SoundManager.Instance.PlaySound(E_SoundType.Effect, 50038);
    }

    private void OnDisable()
    {
        if (UIManager.Instance == null)
            return;

        //유아이 끄기
        UIManager.Instance.CountSelect.OffCountSelectPanle();
        UIManager.Instance.itemTooltip.gameObject.SetActive(false);
        UIManager.Instance.dragSlotObj.EndDrag();
    }

    public void Init()
    {
        if (!isinit)
        {
            isinit = true;

            //인벤토리 슬롯 가져오기
            InvenSlotList = new List<Slot>();
            InvenSlotList.AddRange(this.gameObject.GetComponentsInChildren<Slot>());

            //슬롯 데이터 세팅
            for (int i = 0; i < InvenSlotList.Count; i++)
            {
                if (XmlManager.Instance.dataInfo.InvenItemList.Count <= i)
                {
                    //ItemInfo temp = new ItemInfo();
                    //InvenSlotList[i].ItemInfo = temp;
                    XmlManager.Instance.dataInfo.InvenItemList.Insert(i, InvenSlotList[i].ItemInfo);
                }
                else
                {
                    InvenSlotList[i].ItemInfo = XmlManager.Instance.dataInfo.InvenItemList[i];
                }
                InvenSlotList[i].SetSlot(InvenSlotList[i].ItemInfo);
            }


        }
    }


    /// <summary>
    /// 비어있는 슬롯 갯수
    /// </summary>
    /// <returns>비어있는 슬롯 갯수</returns>
    public int GetNotSetCount()
    {
        int count = 0;

        foreach (Slot slot in InvenSlotList)
        {
            if (!slot.isItem)
                count++;
        }

        return count;
    }

    /// <summary>
    /// 슬롯리스트 아이템 추가
    /// </summary>
    /// <param name="item"> 아이템 정보 </param>
    /// <returns>아이템 추가 확인</returns>
    public bool AddItem(ItemInfo item)
    {
        bool isinven = false;           //아이템 추가 확인
        int nullSlotIndex = -1;        //비어있는 슬롯 인덱스

        for (int i = 0; i < InvenSlotList.Count; i++)
        {
            //같은이름찾기
            if (InvenSlotList[i].ItemInfo.itemID == item.itemID)
            {
                int curcount = InvenSlotList[i].ItemInfo.count + item.count;  //인벤토리 갯수+추가할 아이템 갯수
                //아이템 최대치를 넘는지 확인
                ItemData.Info info = CSVManager.Instance.GetItemInfo(InvenSlotList[i].ItemInfo.itemID);
                int tempstack = info.Stack;

                //넘었는지 확인
                if (curcount > tempstack)
                {
                    ItemInfo tempinfo = InvenSlotList[i].ItemInfo; //슬롯 아이템 정보
                    tempinfo.count = tempstack; //아이템 갯수 = 아이템 최대치

                    InvenSlotList[i].SetSlot(tempinfo);

                    curcount -= tempstack;   //현재숫자 - 최대치
                    //0이하면 종료
                    if (curcount <= 0)
                    {
                        isinven = true;     //인벤토리 추가 완료
                        break;
                    }
                    else
                    {
                        //남은 숫자
                        item.count = curcount;
                    }
                }
                else
                {
                    //안넘을때
                    ItemInfo tempinfo = InvenSlotList[i].ItemInfo; //슬롯 아이템 정보
                    tempinfo.count = curcount; //인벤토리숫자 = 아이템 최대치
                    InvenSlotList[i].SetSlot(tempinfo);
                    isinven = true;                 //인벤토리 추가 완료
                    break;
                }
            }

            //비어있는 슬롯찾기
            if (nullSlotIndex < 0 && InvenSlotList[i].ItemInfo.count <= 0)
            {
                nullSlotIndex = i;
            }
        }

        //아이템이 전부 할당되지않았으면 비어있는 슬롯에 할당
        if (nullSlotIndex >= 0 && !isinven)
        {
            InvenSlotList[nullSlotIndex].SetSlot(item);
            isinven = true;             //인벤토리 추가 완료
        }

        //할당안됨
        if(!isinven)
        {
            //임시 인벤토리에 할당
            UIManager.Instance.TempInven.AddItem(item);
            UIManager.Instance.SetActiveUI(UIManager.Instance.InfoPanel.gameObject, true);        //인벤토리 패널 활성화
            UIManager.Instance.SetActiveUI(UIManager.Instance.TempInven.gameObject, true);     //임시 인벤토리 패널 활성화
            isinven = true;
        }

        return isinven;
    }

    /// <summary>
    /// 아이템 차감
    /// </summary>
    /// <param name="_item">차감할 아이템 정보</param>
    /// <returns>차감 확인</returns>
    public bool SubItem(ItemInfo _item)
    {
        if (_item.itemID == 0)
            return true;

        List<Slot> temp_ItemList =  InvenSlotList.FindAll(x => x.ItemInfo.itemID == _item.itemID);  //아이템을 가진 슬롯 찾기

        int count = _item.count;      //총 갯수

        //갯수 차감
        if (count >= _item.count)
        {
            foreach (Slot slot in temp_ItemList)
            {
                int slotcount = slot.ItemInfo.count;

                int tempcount = count - slotcount;
                if(tempcount < 0)
                {
                    slot.SetCount(slotcount - count);
                    count = 0;
                }
                else
                {
                    slot.SetCount(slotcount - (count - tempcount));
                    count = tempcount;
                }

                if (count <= 0)
                    return true;
            }
        }

        return false;
    }


    //인벤토리 슬롯 드래그 설정하기
    public void SetIsSlotDrag(bool drag)
    {
        foreach(Slot slot in InvenSlotList)
        {
            slot.isDrag = drag;
        }
    }

    /// <summary>
    /// 아이템 소유 중인지 확인
    /// </summary>
    /// <param name="_item">아이템 정보</param>
    /// <returns>아이템 소유 확인 false: 없음</returns>
    public bool GetHaveItem(SourceInfo _item)
    {
        if (_item.ItemID == 0)
            return true;

        List<Slot> temp_ItemList =  InvenSlotList.FindAll(x => x.ItemInfo.itemID == _item.ItemID);  //아이템을 가진 슬롯 찾기

        int count = 0;      //총 갯수

        foreach (Slot slot in temp_ItemList)
        {
            count += slot.ItemInfo.count;
        }

        //갯수 확인
        if (count >= _item.count)
            return true;

        return false;
    }
}
