using UnityEngine;

public class DefaultSlot : Slot
{
	protected override void _On_DropSlot()
	{
		Slot dragslot = UIManager.Instance.dragSlotObj.dragSlot;


		//아이템이 있을 경우
		if (isItem)
		{
			//같은 아이템 일 경우
			if (ItemInfo.itemID == dragslot.ItemInfo.itemID)
			{
				//수량 정하기
				//드래그 슬롯 아이템 정보
				ItemInfo drag_iteminfo = new ItemInfo(ItemInfo.itemID,
					ItemInfo.count + dragslot.ItemInfo.count, ItemInfo.durability);
				//이 슬롯 아이템 정보
				ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);
				ItemInfo this_iteminfo = new ItemInfo(ItemInfo.itemID,
					drag_iteminfo.count - info.Stack,
					dragslot.ItemInfo.durability);

				//최대 수량 넘는지 확인
				if (drag_iteminfo.count > info.Stack)
					drag_iteminfo.count -= this_iteminfo.count;

				SetSlot(drag_iteminfo);      //슬롯 세팅
				dragslot.SetSlot(this_iteminfo);            //드래그 된 슬롯 
			}
			else
			{

				if (dragslot.SlotType == E_SlotType.MakingResult)
					return;

				//아닐경우
				ChangeSlot(dragslot);       //슬롯 아이템 세팅
			}

			return;
		}

		//없을 경우

		//아이템 나누기
		if (UIManager.Instance.isSplit)
		{
			UIManager.Instance.CountSelect.SetCountSelect(this, dragslot, mousepos);
		}
		else
		{
			SetSlot(dragslot.ItemInfo);     //슬롯 아이템 세팅
			dragslot.OffSlot();                 //드래그 된 슬롯 제거
		}

	}
	protected override void _On_RightClick()
	{
		//아이템 사용
		ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);

		//*****
		//스테이터스
		Status status = Player.Instance._Status.status;
		Player.Instance._Status.StatusRecovery(status.hydration, info.Hydration_Variation);
		Player.Instance._Status.StatusRecovery(status.hunger, info.Hunger_Variation);
		Player.Instance._Status.StatusRecovery(status.fatigue, info.Fatigue_Varioation);
		Player.Instance._Status.StatusRecovery(status.hp, info.Hp_Variation);

		//디버프
		//감기
		if (info.Flu_Removal)
			Player.Instance._Debuff.isDebuff.isCold = !info.Flu_Removal;
		// 식중독
		if (info.Bromatotoxism_Chk)
		{
			// 식중독 확률 체크
			int rand = Random.Range(0, 100);
			if (rand < 10)
			{
				Player.Instance._Debuff.isDebuff.isFoodpoison = info.Bromatotoxism_Chk;
			}
			Debug.Log($"{rand}, {rand < 10}");
		}
		// 식중독 제거
		if (info.Bromatotoxism_Removal)
			Player.Instance._Debuff.isDebuff.isFoodpoison = !info.Bromatotoxism_Removal;
		SetCount(--ItemInfo.count);
	}

	protected override void _On_Init()
	{
		SlotType = E_SlotType.Default;
	}
}
