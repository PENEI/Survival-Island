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
        //�������� ���� ���
        if (isItem)
        {
            ChangeSlot(dragslot);       //���� ������ ����
            isset = true;
        }

        //���� ���
        if (!isset)
        {
            SetSlot(dragslot.ItemInfo);     //���� ������ ����
            dragslot.OffSlot();                 //�巡�� �� ���� ����
        }


    }

    protected override void _On_Init()
    {
        SlotType = E_SlotType.Tool;
    }

    private void Update()
    {
        //���� �ִϸ��̼�
        //Player.Instance._Animation.ani.SetBool("Weapon", UIManager.Instance.equipPanel.PlayerTool == E_UseTool.Knife);
    }

    protected override void _On_OffSlot()
    {
        Player.Instance.Control.R_HandTool.OffHandTool();   //�𵨸� ����
        Player.Instance.Control.L_HandTool.OffHandTool();   //�𵨸� ����
        UIManager.Instance.equipPanel.PlayerTool = E_UseTool.Default;  //��� �� �ٲٱ�

        //���ݷ� ����
        Player.Instance._Status.status.attackPower = Player.Instance._Status.status.normalAttackPower;
    }

    protected override void _On_SetSlot(ItemInfo _iteminfo)
    {
        ItemData.Info info = CSVManager.Instance.GetItemInfo(_iteminfo.itemID);
        Player.Instance.Control.R_HandTool.OffHandTool();   //�𵨸� ����
        Player.Instance.Control.L_HandTool.OffHandTool();   //�𵨸� ����

        switch (info.Use_Tool)
        {
            case E_UseTool.Bottle:
            case E_UseTool.Default:
            case E_UseTool.Axe:
            case E_UseTool.Knife:
            case E_UseTool.Pickaxe:
            case E_UseTool.Hammer:
                Player.Instance.Control.R_HandTool.SetHandTool(WeaponManager.Instance.GetToolModel(info.ID));   //�𵨸� ����
                break;
            case E_UseTool.Shovel:
                Player.Instance.Control.L_HandTool.SetHandTool(WeaponManager.Instance.GetToolModel(info.ID));   //�𵨸� ����
                break;
        }
        if(info.Use_Tool == E_UseTool.None)
            UIManager.Instance.equipPanel.PlayerTool = E_UseTool.Default;  //��� �� �ٲٱ�
        else
            UIManager.Instance.equipPanel.PlayerTool = info.Use_Tool;  //��� �� �ٲٱ�

        //���ݷ� ����
        Player.Instance._Status.status.attackPower = UIManager.Instance.equipPanel.GetAtk();
        //Player.Instance._Animation.ani.SetBool("Weapon", UIManager.Instance.equipPanel.PlayerTool == E_UseTool.Knife);
    }
}
