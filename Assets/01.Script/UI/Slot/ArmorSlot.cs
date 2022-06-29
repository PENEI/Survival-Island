using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSlot : Slot
{
    protected override void _On_DropSlot()
    {
        Slot dragslot = UIManager.Instance.dragSlotObj.dragSlot;

        if (E_MaterialType.Armor != CSVManager.Instance.GetItemInfo(dragslot.ItemInfo.itemID).Material_Type)
            return;

        bool isset = false;
        //아이템이 있을 경우
        if (isItem)
        {
            ChangeSlot(dragslot);       //슬롯 아이템 세팅
            isset = true;
        }

        //없을 경우
        if(!isset)
        {
            SetSlot(dragslot.ItemInfo);     //슬롯 아이템 세팅
            dragslot.OffSlot();                 //드래그 된 슬롯 제거
        }
    }

    protected override void _On_Init()
    {
        SlotType = E_SlotType.Armor;
    }

    protected override void _On_OffSlot()
    {
        //*****
        //방어력 해제
        Player.Instance._Status.status.defensePower = 0;
    }

    protected override void _On_SetSlot(ItemInfo _iteminfo)
    {
        //*****
        //방어력 적용
        Player.Instance._Status.status.defensePower = UIManager.Instance.equipPanel.GetDef();
    }
}
