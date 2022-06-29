using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

//�κ��丮 �г�
public class InvenPanel : MonoBehaviour
{
    public List<Slot> InvenSlotList { get; set; }        //�κ��丮 ����
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

        //������ ����
        UIManager.Instance.CountSelect.OffCountSelectPanle();
        UIManager.Instance.itemTooltip.gameObject.SetActive(false);
        UIManager.Instance.dragSlotObj.EndDrag();
    }

    public void Init()
    {
        if (!isinit)
        {
            isinit = true;

            //�κ��丮 ���� ��������
            InvenSlotList = new List<Slot>();
            InvenSlotList.AddRange(this.gameObject.GetComponentsInChildren<Slot>());

            //���� ������ ����
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
    /// ����ִ� ���� ����
    /// </summary>
    /// <returns>����ִ� ���� ����</returns>
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
    /// ���Ը���Ʈ ������ �߰�
    /// </summary>
    /// <param name="item"> ������ ���� </param>
    /// <returns>������ �߰� Ȯ��</returns>
    public bool AddItem(ItemInfo item)
    {
        bool isinven = false;           //������ �߰� Ȯ��
        int nullSlotIndex = -1;        //����ִ� ���� �ε���

        for (int i = 0; i < InvenSlotList.Count; i++)
        {
            //�����̸�ã��
            if (InvenSlotList[i].ItemInfo.itemID == item.itemID)
            {
                int curcount = InvenSlotList[i].ItemInfo.count + item.count;  //�κ��丮 ����+�߰��� ������ ����
                //������ �ִ�ġ�� �Ѵ��� Ȯ��
                ItemData.Info info = CSVManager.Instance.GetItemInfo(InvenSlotList[i].ItemInfo.itemID);
                int tempstack = info.Stack;

                //�Ѿ����� Ȯ��
                if (curcount > tempstack)
                {
                    ItemInfo tempinfo = InvenSlotList[i].ItemInfo; //���� ������ ����
                    tempinfo.count = tempstack; //������ ���� = ������ �ִ�ġ

                    InvenSlotList[i].SetSlot(tempinfo);

                    curcount -= tempstack;   //������� - �ִ�ġ
                    //0���ϸ� ����
                    if (curcount <= 0)
                    {
                        isinven = true;     //�κ��丮 �߰� �Ϸ�
                        break;
                    }
                    else
                    {
                        //���� ����
                        item.count = curcount;
                    }
                }
                else
                {
                    //�ȳ�����
                    ItemInfo tempinfo = InvenSlotList[i].ItemInfo; //���� ������ ����
                    tempinfo.count = curcount; //�κ��丮���� = ������ �ִ�ġ
                    InvenSlotList[i].SetSlot(tempinfo);
                    isinven = true;                 //�κ��丮 �߰� �Ϸ�
                    break;
                }
            }

            //����ִ� ����ã��
            if (nullSlotIndex < 0 && InvenSlotList[i].ItemInfo.count <= 0)
            {
                nullSlotIndex = i;
            }
        }

        //�������� ���� �Ҵ�����ʾ����� ����ִ� ���Կ� �Ҵ�
        if (nullSlotIndex >= 0 && !isinven)
        {
            InvenSlotList[nullSlotIndex].SetSlot(item);
            isinven = true;             //�κ��丮 �߰� �Ϸ�
        }

        //�Ҵ�ȵ�
        if(!isinven)
        {
            //�ӽ� �κ��丮�� �Ҵ�
            UIManager.Instance.TempInven.AddItem(item);
            UIManager.Instance.SetActiveUI(UIManager.Instance.InfoPanel.gameObject, true);        //�κ��丮 �г� Ȱ��ȭ
            UIManager.Instance.SetActiveUI(UIManager.Instance.TempInven.gameObject, true);     //�ӽ� �κ��丮 �г� Ȱ��ȭ
            isinven = true;
        }

        return isinven;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="_item">������ ������ ����</param>
    /// <returns>���� Ȯ��</returns>
    public bool SubItem(ItemInfo _item)
    {
        if (_item.itemID == 0)
            return true;

        List<Slot> temp_ItemList =  InvenSlotList.FindAll(x => x.ItemInfo.itemID == _item.itemID);  //�������� ���� ���� ã��

        int count = _item.count;      //�� ����

        //���� ����
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


    //�κ��丮 ���� �巡�� �����ϱ�
    public void SetIsSlotDrag(bool drag)
    {
        foreach(Slot slot in InvenSlotList)
        {
            slot.isDrag = drag;
        }
    }

    /// <summary>
    /// ������ ���� ������ Ȯ��
    /// </summary>
    /// <param name="_item">������ ����</param>
    /// <returns>������ ���� Ȯ�� false: ����</returns>
    public bool GetHaveItem(SourceInfo _item)
    {
        if (_item.ItemID == 0)
            return true;

        List<Slot> temp_ItemList =  InvenSlotList.FindAll(x => x.ItemInfo.itemID == _item.ItemID);  //�������� ���� ���� ã��

        int count = 0;      //�� ����

        foreach (Slot slot in temp_ItemList)
        {
            count += slot.ItemInfo.count;
        }

        //���� Ȯ��
        if (count >= _item.count)
            return true;

        return false;
    }
}
