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
        //�������� ���� ���
        if (isItem)
        {
            ChangeSlot(dragslot);       //���� ������ ����
            isset = true;
        }

        //���� ���
        if(!isset)
        {
            SetSlot(dragslot.ItemInfo);     //���� ������ ����
            dragslot.OffSlot();                 //�巡�� �� ���� ����
        }
    }

    protected override void _On_Init()
    {
        SlotType = E_SlotType.Armor;
    }

    protected override void _On_OffSlot()
    {
        //*****
        //���� ����
        Player.Instance._Status.status.defensePower = 0;
    }

    protected override void _On_SetSlot(ItemInfo _iteminfo)
    {
        //*****
        //���� ����
        Player.Instance._Status.status.defensePower = UIManager.Instance.equipPanel.GetDef();
    }
}
