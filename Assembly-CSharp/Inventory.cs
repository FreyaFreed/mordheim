using System;
using System.Collections.Generic;

public class Inventory : global::ICheapState
{
	public Inventory(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.itemIndex = 0;
		this.slotIndex = 0;
		this.UpdateInventory();
		global::SearchPoint searchPoint = (global::SearchPoint)this.unitCtrlr.interactivePoint;
		this.unitCtrlr.GetWarband().LocateItem(searchPoint.items);
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.MISSION_INVENTORY);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.unitItems.Clear();
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	public void UpdateInventory()
	{
		this.unitItems.Clear();
		for (int i = 6; i < this.unitCtrlr.unit.Items.Count; i++)
		{
			this.unitItems.Add(this.unitCtrlr.unit.Items[i]);
		}
	}

	private void SwitchItem()
	{
		global::SearchPoint searchPoint = (global::SearchPoint)this.unitCtrlr.interactivePoint;
		global::Item item = this.unitItems[this.slotIndex];
		if (this.itemIndex == -1)
		{
			searchPoint.AddItem(item, false);
			global::Item item2 = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			this.unitCtrlr.unit.EquipItem(global::UnitSlotId.ITEM_1 + this.slotIndex, item2, true);
		}
		else
		{
			global::Item item3 = searchPoint.GetItems()[this.itemIndex];
			global::Item item2 = searchPoint.SwitchItem(this.unitCtrlr, this.itemIndex, item);
			if (item2 != item)
			{
				this.unitCtrlr.unit.EquipItem(global::UnitSlotId.ITEM_1 + this.slotIndex, item2, true);
				global::WarbandController warband = this.unitCtrlr.GetWarband();
				if (item3.IsIdol)
				{
					this.unitCtrlr.unit.AddEnchantment(global::EnchantmentId.IDOL_THEFT, this.unitCtrlr.unit, false, true, global::AllegianceId.NONE);
					this.unitCtrlr.Imprint.idolTexture = item3.GetIcon();
				}
				else if (item.IsIdol)
				{
					this.unitCtrlr.unit.RemoveEnchantment(global::EnchantmentId.IDOL_THEFT, this.unitCtrlr.unit);
					this.unitCtrlr.Imprint.idolTexture = null;
					for (int i = 6; i < this.unitCtrlr.unit.Items.Count; i++)
					{
						if (this.unitCtrlr.unit.Items[i].IsIdol)
						{
							this.unitCtrlr.Imprint.idolTexture = this.unitCtrlr.unit.Items[i].GetIcon();
							break;
						}
					}
				}
				for (int j = 0; j < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; j++)
				{
					if (item3.IsIdol && item3 == global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].ItemIdol)
					{
						if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].teamIdx == warband.teamIdx)
						{
							global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].AddMoralIdol();
						}
						else
						{
							global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].RemoveMoralIdol();
						}
					}
					else if (item.IsIdol && item == global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].ItemIdol)
					{
						if (searchPoint == global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].wagon.idol)
						{
							global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].AddMoralIdol();
						}
						else if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].teamIdx == warband.teamIdx)
						{
							global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].AddMoralIdol();
						}
						else
						{
							global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].RemoveMoralIdol();
						}
					}
				}
			}
		}
		this.UpdateInventory();
	}

	public void PickupItem(int inventoryIndex, int searchZoneSlotIndex)
	{
		this.UpdateInventory();
		this.itemIndex = inventoryIndex;
		this.slotIndex = searchZoneSlotIndex;
		this.SwitchItem();
	}

	public void CloseInventory()
	{
		this.unitCtrlr.SendInventoryDone();
	}

	private global::UnitController unitCtrlr;

	private int itemIndex;

	private int slotIndex;

	private readonly global::System.Collections.Generic.List<global::Item> unitItems = new global::System.Collections.Generic.List<global::Item>();
}
