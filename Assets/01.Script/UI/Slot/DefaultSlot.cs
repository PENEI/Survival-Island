using UnityEngine;

public class DefaultSlot : Slot
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
					drag_iteminfo.count - info.Stack,
					dragslot.ItemInfo.durability);

				//�ִ� ���� �Ѵ��� Ȯ��
				if (drag_iteminfo.count > info.Stack)
					drag_iteminfo.count -= this_iteminfo.count;

				SetSlot(drag_iteminfo);      //���� ����
				dragslot.SetSlot(this_iteminfo);            //�巡�� �� ���� 
			}
			else
			{

				if (dragslot.SlotType == E_SlotType.MakingResult)
					return;

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
	protected override void _On_RightClick()
	{
		//������ ���
		ItemData.Info info = CSVManager.Instance.GetItemInfo(ItemInfo.itemID);

		//*****
		//�������ͽ�
		Status status = Player.Instance._Status.status;
		Player.Instance._Status.StatusRecovery(status.hydration, info.Hydration_Variation);
		Player.Instance._Status.StatusRecovery(status.hunger, info.Hunger_Variation);
		Player.Instance._Status.StatusRecovery(status.fatigue, info.Fatigue_Varioation);
		Player.Instance._Status.StatusRecovery(status.hp, info.Hp_Variation);

		//�����
		//����
		if (info.Flu_Removal)
			Player.Instance._Debuff.isDebuff.isCold = !info.Flu_Removal;
		// ���ߵ�
		if (info.Bromatotoxism_Chk)
		{
			// ���ߵ� Ȯ�� üũ
			int rand = Random.Range(0, 100);
			if (rand < 10)
			{
				Player.Instance._Debuff.isDebuff.isFoodpoison = info.Bromatotoxism_Chk;
			}
			Debug.Log($"{rand}, {rand < 10}");
		}
		// ���ߵ� ����
		if (info.Bromatotoxism_Removal)
			Player.Instance._Debuff.isDebuff.isFoodpoison = !info.Bromatotoxism_Removal;
		SetCount(--ItemInfo.count);
	}

	protected override void _On_Init()
	{
		SlotType = E_SlotType.Default;
	}
}
