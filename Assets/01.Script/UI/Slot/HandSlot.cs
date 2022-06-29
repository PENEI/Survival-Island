using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSlot : Slot
{
    protected override void _On_DropSlot()
    {
        Slot dragslot = UIManager.Instance.dragSlotObj.dragSlot;
        ItemData.Info info = CSVManager.Instance.GetItemInfo(dragslot.ItemInfo.itemID);
        if (E_MaterialType.Tool != info.Material_Type)
            return;

        bool isset = false;
        //아이템이 있을 경우
        if (isItem)
        {
            ChangeSlot(dragslot);       //슬롯 아이템 세팅
            isset = true;
        }

        //없을 경우
        if (!isset)
        {
            SetSlot(dragslot.ItemInfo);     //슬롯 아이템 세팅
            dragslot.OffSlot();                 //드래그 된 슬롯 제거
        }


    }

    protected override void _On_Init()
    {
        SlotType = E_SlotType.Tool;
    }

    private void Update()
    {
        //무기 애니메이션
        //Player.Instance._Animation.ani.SetBool("Weapon", UIManager.Instance.equipPanel.PlayerTool == E_UseTool.Knife);
    }

    protected override void _On_OffSlot()
    {
        Player.Instance.Control.R_HandTool.OffHandTool();   //모델링 세팅
        Player.Instance.Control.L_HandTool.OffHandTool();   //모델링 세팅
        UIManager.Instance.equipPanel.PlayerTool = E_UseTool.Default;  //사용 툴 바꾸기

        //공격력 해제
        Player.Instance._Status.status.attackPower = Player.Instance._Status.status.normalAttackPower;
    }

    protected override void _On_SetSlot(ItemInfo _iteminfo)
    {
        ItemData.Info info = CSVManager.Instance.GetItemInfo(_iteminfo.itemID);
        Player.Instance.Control.R_HandTool.OffHandTool();   //모델링 세팅
        Player.Instance.Control.L_HandTool.OffHandTool();   //모델링 세팅

        switch (info.Use_Tool)
        {
            case E_UseTool.Bottle:
            case E_UseTool.Default:
            case E_UseTool.Axe:
            case E_UseTool.Knife:
            case E_UseTool.Pickaxe:
            case E_UseTool.Hammer:
                Player.Instance.Control.R_HandTool.SetHandTool(WeaponManager.Instance.GetToolModel(info.ID));   //모델링 세팅
                break;
            case E_UseTool.Shovel:
                Player.Instance.Control.L_HandTool.SetHandTool(WeaponManager.Instance.GetToolModel(info.ID));   //모델링 세팅
                break;
        }
        if(info.Use_Tool == E_UseTool.None)
            UIManager.Instance.equipPanel.PlayerTool = E_UseTool.Default;  //사용 툴 바꾸기
        else
            UIManager.Instance.equipPanel.PlayerTool = info.Use_Tool;  //사용 툴 바꾸기

        //공격력 적용
        Player.Instance._Status.status.attackPower = UIManager.Instance.equipPanel.GetAtk();
        //Player.Instance._Animation.ani.SetBool("Weapon", UIManager.Instance.equipPanel.PlayerTool == E_UseTool.Knife);
    }
}
