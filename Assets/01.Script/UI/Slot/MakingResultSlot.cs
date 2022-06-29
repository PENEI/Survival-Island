using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingResultSlot : Slot
{
    public override void _On_Drag_End_Drop()
    {
        //아이템 나누기 확인
        if (UIManager.Instance.isSplit)
        {
            UIManager.Instance.CountSelect.isMakingResult = true;   //갯수 선택 슬롯에 제작 true
        }
        else
        {
            UIManager.Instance.makingPanelList.GetResultItem(); //갯수 감소
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
