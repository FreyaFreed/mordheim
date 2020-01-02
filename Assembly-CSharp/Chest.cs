using System;
using System.Collections.Generic;

public class Chest
{
	public Chest()
	{
		this.items = new global::System.Collections.Generic.List<global::ItemSave>();
	}

	public Chest(global::System.Collections.Generic.List<global::ItemSave> list)
	{
		this.items = list;
		this.ForceRecipeToNormalQuality();
	}

	public void ForceRecipeToNormalQuality()
	{
		global::System.Collections.Generic.List<global::RuneMarkRecipeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkRecipeData>();
		for (int i = 0; i < this.items.Count; i++)
		{
			global::ItemSave itemSave = this.items[i];
			if (itemSave.qualityId > 1)
			{
				global::ItemId itemId = (global::ItemId)itemSave.id;
				if (list.Exists((global::RuneMarkRecipeData x) => x.ItemId == itemId))
				{
					itemSave.qualityId = 1;
				}
			}
		}
	}

	public bool AddItem(global::ItemSave save, bool sold = false)
	{
		return this.AddItem((global::ItemId)save.id, (global::ItemQualityId)save.qualityId, (global::RuneMarkId)save.runeMarkId, (global::RuneMarkQualityId)save.runeMarkQualityId, (global::AllegianceId)save.allegianceId, save.amount, true);
	}

	public virtual bool AddItem(global::ItemId itemId, global::ItemQualityId quality = global::ItemQualityId.NORMAL, global::RuneMarkId runeMark = global::RuneMarkId.NONE, global::RuneMarkQualityId runeMarkQuality = global::RuneMarkQualityId.NONE, global::AllegianceId allegiance = global::AllegianceId.NONE, int count = 1, bool sold = false)
	{
		if (itemId == global::ItemId.NONE)
		{
			return false;
		}
		global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)itemId);
		if (itemData.Backup)
		{
			return false;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			global::ItemSave itemSave = this.items[i];
			if (itemSave.id == (int)itemId && itemSave.qualityId == (int)quality && itemSave.runeMarkId == (int)runeMark && itemSave.runeMarkQualityId == (int)runeMarkQuality)
			{
				itemSave.amount += count;
				if (sold)
				{
					itemSave.soldAmount += count;
				}
				return true;
			}
		}
		global::ItemSave itemSave2 = new global::ItemSave(itemId, quality, runeMark, runeMarkQuality, allegiance, count);
		if (sold)
		{
			itemSave2.soldAmount = count;
		}
		this.items.Add(itemSave2);
		return true;
	}

	public bool AddItems(global::System.Collections.Generic.List<global::Item> from)
	{
		bool flag = false;
		for (int i = 0; i < from.Count; i++)
		{
			flag |= this.AddItem(from[i].Save, false);
		}
		return flag;
	}

	public bool AddItems(global::System.Collections.Generic.List<global::ItemSave> from)
	{
		bool flag = false;
		for (int i = 0; i < from.Count; i++)
		{
			flag |= this.AddItem(from[i], false);
		}
		return flag;
	}

	public global::ItemSave PopItem(global::ItemSave save, int qty = 1)
	{
		if (this.RemoveItem(save, qty))
		{
			return new global::ItemSave((global::ItemId)save.id, (global::ItemQualityId)save.qualityId, (global::RuneMarkId)save.runeMarkId, (global::RuneMarkQualityId)save.runeMarkQualityId, (global::AllegianceId)save.allegianceId, qty);
		}
		return null;
	}

	public void Clear()
	{
		this.items.Clear();
	}

	public bool RemoveItem(global::ItemId itemId, int count)
	{
		return this.RemoveItem(itemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, count);
	}

	public bool RemoveItem(global::ItemSave save, int qty = 1)
	{
		return this.RemoveItem((global::ItemId)save.id, (global::ItemQualityId)save.qualityId, (global::RuneMarkId)save.runeMarkId, (global::RuneMarkQualityId)save.runeMarkQualityId, (global::AllegianceId)save.allegianceId, qty);
	}

	public virtual bool RemoveItem(global::ItemId itemId, global::ItemQualityId quality = global::ItemQualityId.NORMAL, global::RuneMarkId runeMark = global::RuneMarkId.NONE, global::RuneMarkQualityId runeMarkQuality = global::RuneMarkQualityId.NONE, global::AllegianceId allegiance = global::AllegianceId.NONE, int count = 1)
	{
		if (itemId == global::ItemId.NONE)
		{
			return false;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			global::ItemSave itemSave = this.items[i];
			if (itemSave.id == (int)itemId && itemSave.qualityId == (int)quality && itemSave.runeMarkId == (int)runeMark && itemSave.runeMarkQualityId == (int)runeMarkQuality)
			{
				if (count > itemSave.amount)
				{
					count = itemSave.amount;
				}
				itemSave.amount -= count;
				if (itemSave.amount <= 0)
				{
					this.items.RemoveAt(i);
				}
				return true;
			}
		}
		return false;
	}

	public bool HasItem(global::Item item)
	{
		return this.HasItem(item.Id, item.QualityData.Id, (item.RuneMark == null) ? global::RuneMarkId.NONE : item.RuneMark.Data.Id, (item.RuneMark == null) ? global::RuneMarkQualityId.NONE : item.RuneMark.QualityData.Id);
	}

	public bool HasItem(global::ItemSave save)
	{
		return this.HasItem((global::ItemId)save.id, (global::ItemQualityId)save.qualityId, (global::RuneMarkId)save.runeMarkId, (global::RuneMarkQualityId)save.runeMarkQualityId);
	}

	public bool HasItem(global::ItemId itemId, global::ItemQualityId qualityId, global::RuneMarkId runeId = global::RuneMarkId.NONE, global::RuneMarkQualityId runeQualityId = global::RuneMarkQualityId.NONE)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == (int)itemId && this.items[i].qualityId == (int)qualityId && this.items[i].runeMarkId == (int)runeId && this.items[i].runeMarkQualityId == (int)runeQualityId)
			{
				return true;
			}
		}
		return false;
	}

	public global::ItemSave GetItem(global::ItemId id, global::ItemQualityId qualityId = global::ItemQualityId.NORMAL)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == (int)id && this.items[i].qualityId == (int)qualityId)
			{
				return this.items[i];
			}
		}
		return new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
	}

	public virtual global::System.Collections.Generic.List<global::ItemSave> GetItems()
	{
		return this.items;
	}

	public void RemoveSoldItems()
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].soldAmount == this.items[i].amount)
			{
				this.items.RemoveAt(i);
			}
			else
			{
				this.items[i].amount -= this.items[i].soldAmount;
				this.items[i].soldAmount = 0;
			}
		}
	}

	public virtual global::System.Collections.Generic.List<global::ItemSave> GetItems(global::Unit unit, global::UnitSlotId slotId)
	{
		global::System.Collections.Generic.List<global::ItemSave> list = new global::System.Collections.Generic.List<global::ItemSave>();
		slotId = ((slotId <= global::UnitSlotId.ITEM_1) ? slotId : global::UnitSlotId.ITEM_1);
		bool flag = false;
		if (slotId == global::UnitSlotId.SET1_OFFHAND || slotId == global::UnitSlotId.SET2_OFFHAND)
		{
			for (int i = 0; i < unit.Items[slotId - global::UnitSlotId.ARMOR].Enchantments.Count; i++)
			{
				if (unit.Items[slotId - global::UnitSlotId.ARMOR].Enchantments[i].Id == global::EnchantmentId.ITEM_UNWIELDY)
				{
					flag = true;
					break;
				}
			}
		}
		global::MutationId mutationId = unit.GetMutationId(slotId);
		bool flag2 = (slotId == global::UnitSlotId.SET1_MAINHAND || slotId == global::UnitSlotId.SET2_MAINHAND) && unit.GetMutationId(slotId + 1) != global::MutationId.NONE;
		bool flag3 = (slotId == global::UnitSlotId.SET1_MAINHAND || slotId == global::UnitSlotId.SET2_MAINHAND) && unit.GetInjury(slotId + 1) != global::InjuryId.NONE;
		global::System.Collections.Generic.List<global::ItemUnitData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemUnitData>(new string[]
		{
			"fk_unit_id",
			"mutation"
		}, new string[]
		{
			((int)unit.Id).ToConstantString(),
			(mutationId == global::MutationId.NONE) ? "0" : "1"
		});
		global::System.Collections.Generic.List<global::ItemId> list3 = new global::System.Collections.Generic.List<global::ItemId>();
		for (int j = 0; j < list2.Count; j++)
		{
			list3.Add(list2[j].ItemId);
		}
		if (slotId == global::UnitSlotId.SET2_MAINHAND || slotId == global::UnitSlotId.SET2_OFFHAND)
		{
			slotId -= 2;
		}
		for (int k = 0; k < this.items.Count; k++)
		{
			global::ItemSave itemSave = this.items[k];
			global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(itemSave.id);
			global::ItemTypeData itemTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemTypeData>((int)itemData.ItemTypeId);
			global::System.Collections.Generic.List<global::ItemConsumableData> list4 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemConsumableData>(new string[]
			{
				"fk_item_id",
				"fk_item_quality_id"
			}, new string[]
			{
				itemSave.id.ToConstantString(),
				itemSave.qualityId.ToConstantString()
			});
			global::ItemConsumableData itemConsumableData = (list4 == null || list4.Count <= 0) ? null : list4[0];
			global::System.Collections.Generic.List<global::ItemEnchantmentData> list5 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemEnchantmentData>(new string[]
			{
				"fk_item_id",
				"fk_item_quality_id",
				"fk_enchantment_id"
			}, new string[]
			{
				itemSave.id.ToConstantString(),
				itemSave.qualityId.ToConstantString(),
				51.ToConstantString()
			});
			bool flag4 = list5.Count > 0;
			bool flag5 = true;
			if (itemConsumableData != null)
			{
				global::System.Collections.Generic.List<global::ItemConsumableLockConsumableData> list6 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemConsumableLockConsumableData>("fk_skill_id_locked", ((int)itemConsumableData.SkillId).ToConstantString());
				for (int l = 0; l < list6.Count; l++)
				{
					if (unit.UnitSave.consumableSkills.IndexOf(list6[l].SkillId, global::SkillIdComparer.Instance) != -1)
					{
						flag5 = false;
						break;
					}
				}
			}
			global::System.Collections.Generic.List<global::ItemJoinUnitSlotData> list7 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemJoinUnitSlotData>("fk_item_id", itemData.Id.ToIntString<global::ItemId>());
			if (list7.Exists((global::ItemJoinUnitSlotData x) => x.UnitSlotId == slotId) && list3.IndexOf((global::ItemId)itemSave.id, global::ItemIdComparer.Instance) != -1 && !unit.IsItemTypeBlocked(itemTypeData.Id) && (!flag || (flag && itemData.ItemTypeId == global::ItemTypeId.SHIELD)) && (!flag2 || (!itemTypeData.IsTwoHanded && !itemData.Paired && !flag4)) && (!flag3 || (!itemTypeData.IsTwoHanded && !itemData.Paired)) && flag5)
			{
				list.Add(this.items[k]);
			}
		}
		return list;
	}

	protected global::System.Collections.Generic.List<global::ItemSave> items;
}
