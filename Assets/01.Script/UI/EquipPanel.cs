using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPanel : MonoBehaviour
{
    public E_UseTool PlayerTool = E_UseTool.Default;        //���� ��� ��

    public ArmorSlot m_ArmorSlot { get; set; }      //�� ����
    public HandSlot m_HandSlot { get; set; }         //���� ����

    bool isinit;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (isinit)
            return;
        isinit = true;
        //�ڽ� ���� ������
        m_ArmorSlot = this.gameObject.GetComponentInChildren<ArmorSlot>();
        m_HandSlot = this.gameObject.GetComponentInChildren<HandSlot>();

        //���� ������ ����
        m_ArmorSlot.ItemInfo = XmlManager.Instance.dataInfo.Armor;
        m_ArmorSlot.SetSlot(m_ArmorSlot.ItemInfo);
        m_HandSlot.ItemInfo = XmlManager.Instance.dataInfo.Tool;
        m_HandSlot.SetSlot(m_HandSlot.ItemInfo);
    }

    //�� ������ ����
    public void ReductionArmor()
    {
        ReductionSlot(m_ArmorSlot);
    }

    //���� ������ ����
    public void ReductionHand()
    {
        ReductionSlot(m_HandSlot);
    }

    //���ݷ� ���
    public float GetAtk()
    {
        ItemData.Info info = CSVManager.Instance.GetItemInfo(m_HandSlot.ItemInfo.itemID);
        if (info.Atk_Variation > 0)
            return info.Atk_Variation;

        return 0;
    }

    //���� ���
    public float GetDef()
    {
        ItemData.Info info = CSVManager.Instance.GetItemInfo(m_ArmorSlot.ItemInfo.itemID);
        if (info.Def_Variation > 0)
            return info.Def_Variation;

        return 0;
    }

    // ���� ����
    public void OffHandTool()
	{
        if(m_HandSlot.isItem)
		{
            //�κ��丮�� ������ �߰�
            if(UIManager.Instance.invenPanel.AddItem(m_HandSlot.ItemInfo))
			{
                m_HandSlot.OffSlot();
			}
		}
	}

    void ReductionSlot(Slot _slot)
    {
        if (_slot.isItem && _slot.ItemInfo.itemID > 0)
        {
            ItemData.Info info = CSVManager.Instance.GetItemInfo(_slot.ItemInfo.itemID);
            _slot.ItemInfo.durability -= info.Dur_Reduction;
            if (_slot.ItemInfo.durability <= 0)
            {
                _slot.OffSlot();
            }
        }
    }
}
