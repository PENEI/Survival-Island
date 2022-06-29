using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingResultSlot : Slot
{
    public override void _On_Drag_End_Drop()
    {
        //������ ������ Ȯ��
        if (UIManager.Instance.isSplit)
        {
            UIManager.Instance.CountSelect.isMakingResult = true;   //���� ���� ���Կ� ���� true
        }
        else
        {
            UIManager.Instance.makingPanelList.GetResultItem(); //���� ����
        }
    }

    protected override void _On_DropSlot()
    {
    }

    protected override void _On_EndDrag(Vector2 _mouse_pos)
    {

    }

    protected override void _On_Init()
    {
        SlotType = E_SlotType.MakingResult;
    }
}
