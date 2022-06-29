using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInvenPanel : MonoBehaviour
{
    public SlotPanel TempInvenSlotList; //���Ը���Ʈ


    void Awake()
    {
        TempInvenSlotList = gameObject.GetComponentInChildren<SlotPanel>();
    }


    //���Կ� �������߰�
    public void AddItem(ItemInfo item)
    {
        int nullSlotIndex = -1;        //����ִ� ���� �ε���

        for (int i = 0; i < TempInvenSlotList.SlotList.Count; i++)
        {
            //����ִ� ����ã��
            if (nullSlotIndex < 0 && TempInvenSlotList.SlotList[i].ItemInfo.count <= 0)
            {
                nullSlotIndex = i;
                break;
            }
        }

        //����ִ� ���Կ� �Ҵ�
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
