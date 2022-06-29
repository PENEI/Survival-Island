using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInvenPanel : MonoBehaviour
{
    public SlotPanel TempInvenSlotList; //슬롯리스트


    void Awake()
    {
        TempInvenSlotList = gameObject.GetComponentInChildren<SlotPanel>();
    }


    //슬롯에 아이템추가
    public void AddItem(ItemInfo item)
    {
        int nullSlotIndex = -1;        //비어있는 슬롯 인덱스

        for (int i = 0; i < TempInvenSlotList.SlotList.Count; i++)
        {
            //비어있는 슬롯찾기
            if (nullSlotIndex < 0 && TempInvenSlotList.SlotList[i].ItemInfo.count <= 0)
            {
                nullSlotIndex = i;
                break;
            }
        }

        //비어있는 슬롯에 할당
        if (nullSlotIndex >= 0)
        {
            TempInvenSlotList.SlotList[nullSlotIndex].SetSlot(item);
        }
    }

    public void _On_ButtonOk()
    {
        UIManager.Instance.SetActiveUI(this.gameObject, false);
    }

    private void OnDisable()
    {
        foreach (Slot slot in TempInvenSlotList.SlotList)
        {
            slot.OffSlot();
        }
    }
}
