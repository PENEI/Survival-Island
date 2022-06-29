using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPanel : MonoBehaviour
{
    public E_UseTool PlayerTool = E_UseTool.Default;        //유저 사용 툴

    public ArmorSlot m_ArmorSlot { get; set; }      //방어구 슬롯
    public HandSlot m_HandSlot { get; set; }         //도구 슬롯

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
        //자식 슬롯 가져옴
        m_ArmorSlot = this.gameObject.GetComponentInChildren<ArmorSlot>();
        m_HandSlot = this.gameObject.GetComponentInChildren<HandSlot>();

        //슬롯 데이터 세팅
        m_ArmorSlot.ItemInfo = XmlManager.Instance.dataInfo.Armor;
        m_ArmorSlot.SetSlot(m_ArmorSlot.ItemInfo);
        m_HandSlot.ItemInfo = XmlManager.Instance.dataInfo.Tool;
        m_HandSlot.SetSlot(m_HandSlot.ItemInfo);
    }

    //방어구 내구도 감소
    public void ReductionArmor()
    {
        ReductionSlot(m_ArmorSlot);
    }

    //도구 내구도 감소
    public void ReductionHand()
    {
        ReductionSlot(m_HandSlot);
    }

    //공격력 얻기
    public float GetAtk()
    {
        ItemData.Info info = CSVManager.Instance.GetItemInfo(m_HandSlot.ItemInfo.itemID);
        if (info.Atk_Variation > 0)
            return info.Atk_Variation;

        return 0;
    }

    //방어력 얻기
    public float GetDef()
    {
        ItemData.Info info = CSVManager.Instance.GetItemInfo(m_ArmorSlot.ItemInfo.itemID);
        if (info.Def_Variation > 0)
            return info.Def_Variation;

        return 0;
    }

    // 도구 해제
    public void OffHandTool()
	{
        if(m_HandSlot.isItem)
		{
            //인벤토리에 아이템 추가
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
