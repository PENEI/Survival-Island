using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSlot : Slot
{
    protected override void _On_DropSlot()
    {
        Slot dragslot = UIManager.Instance.dragSlotObj.dragSlot;

        //�������� ���� ���
        if (isItem)
        {
            //���� ������ �� ���
            if (ItemInfo.itemID == dragslot.ItemInfo.itemID)
            {
                //���� ���ϱ�
                //�巡�� ���� ������ ����
                ItemInfo drag_iteminfo = new ItemInfo(ItemInfo.itemID,
                    ItemInfo.count + dragslot.ItemInfo.count, ItemInfo.durability);
                //�� ���� ������ ����
                ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);
                ItemInfo this_iteminfo = new ItemInfo(ItemInfo.itemID,
                    drag_iteminfo.count - info.Stack, dragslot.ItemInfo.durability);

                //�ִ� ���� �Ѵ��� Ȯ��
                if (drag_iteminfo.count > info.Stack) 
                    drag_iteminfo.count -= this_iteminfo.count;

                SetSlot(drag_iteminfo);      //���� ����
                dragslot.SetSlot(this_iteminfo);            //�巡�� �� ���� 
            }
            else
            {
                //�ƴҰ��
                ChangeSlot(dragslot);       //���� ������ ����
            }

            return;
        }

        //���� ���

        //������ ������
        if (UIManager.Instance.isSplit)
        {
            UIManager.Instance.CountSelect.SetCountSelect(this, dragslot, mousepos);
        }
        else
        {
            SetSlot(dragslot.ItemInfo);     //���� ������ ����
            dragslot.OffSlot();                 //�巡�� �� ���� ����
        }
    }

    protected override void _On_Init()
    {
        SlotType = E_SlotType.Fuel;
    }
}
