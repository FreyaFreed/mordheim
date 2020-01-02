using System;
using System.Collections.Generic;
using Pathfinding;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class SearchPoint : global::InteractivePoint
{
	public bool IsOpened { get; private set; }

	public void Init(uint id, int capacity, bool wyrdStone = false)
	{
		this.slots = new global::System.Collections.Generic.List<global::SearchSlotData>();
		for (int i = 0; i < capacity; i++)
		{
			this.slots.Add(new global::SearchSlotData());
		}
		this.isWyrdstone = wyrdStone;
		this.Init(id);
	}

	public override void Init(uint id)
	{
		this.IsOpened = false;
		this.wasSearched = false;
		for (int i = 0; i < this.slots.Count; i++)
		{
			global::Item item = this.AddItem(this.slots[i].itemId, this.slots[i].itemQualityId, this.slots[i].runeMarkId, this.slots[i].runeMarkQualityId, this.slots[i].allegianceId, false);
			if (this.slots[i].itemId == global::ItemId.GOLD)
			{
				item.Save.amount = this.slots[i].value;
			}
		}
		this.contentDissolvers = new global::System.Collections.Generic.List<global::Dissolver>();
		for (int j = 0; j < this.contentVisuals.Count; j++)
		{
			if (this.contentVisuals[j] == null)
			{
				global::PandoraDebug.LogError("visual cannot be null", "SEARCH", this);
			}
			this.contentDissolvers.Add(this.contentVisuals[j].AddComponent<global::Dissolver>());
			this.contentDissolvers[j].dissolveSpeed = global::UnityEngine.Mathf.Max(this.apparitionDelay, 0.5f);
			this.contentDissolvers[j].Hide(true, true, null);
		}
		this.animComponent = base.GetComponent<global::UnityEngine.Animation>();
		if (this.visual != null)
		{
			this.visualNavCutter = this.visual.GetComponentInChildren<global::Pathfinding.NavmeshCut>();
			if (this.visualNavCutter != null)
			{
				this.visualNavCutter.ForceUpdate();
			}
		}
		for (int k = 0; k < this.contentVisuals.Count; k++)
		{
			this.contentCutters.Add(this.contentVisuals[k].GetComponent<global::Pathfinding.NavmeshCut>());
			if (this.contentCutters[k] != null)
			{
				this.contentCutters[k].ForceUpdate();
			}
		}
		base.Init(id);
		if (this.isWyrdstone)
		{
			base.Imprint.imprintType = global::MapImprintType.WYRDSTONE;
		}
		this.Refresh();
	}

	public void InitInteraction()
	{
		this.wasEmpty = this.IsEmpty();
		this.wasFull = this.IsFull();
	}

	public void Open()
	{
		if (!this.IsOpened)
		{
			if (this.animComponent != null)
			{
				this.animComponent[this.animComponent.clip.name].speed = 1f;
				this.animComponent.Play(this.animComponent.clip.name);
			}
			base.SpawnFxs(true);
			this.IsOpened = true;
		}
	}

	public virtual void Close(bool force = false)
	{
		base.SpawnFxs(false);
		if (this.animComponent != null)
		{
			int num = this.CountItems();
			if (this.IsOpened && num != 0)
			{
				this.animComponent[this.animComponent.clip.name].speed = -1f;
				this.animComponent.Play(this.animComponent.clip.name);
			}
			else if (!this.IsOpened && this.CountItems() == 0)
			{
				this.animComponent[this.animComponent.clip.name].speed = 1f;
				this.animComponent.Play(this.animComponent.clip.name);
			}
		}
		this.IsOpened = false;
		if ((this.destroyOnEmpty && this.IsEmpty()) || (this.destroyOnFull && this.IsFull()))
		{
			base.DestroyVisual(this.destroyOnlyTriggers, force);
		}
		else if (this.lockOnDrop && this.IsFull() && (this.linkedPoint == null || this.linkedPoint.gameObject != base.gameObject) && (this.links.Count == 0 || this.links[0].gameObject != base.gameObject))
		{
			base.DestroyVisual(true, force);
		}
		else
		{
			this.SetTriggerVisual();
		}
		global::System.Collections.Generic.List<global::ItemSave> list = new global::System.Collections.Generic.List<global::ItemSave>();
		for (int i = 0; i < this.items.Count; i++)
		{
			list.Add(this.items[i].Save);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateSearches(this.guid, (!(this.unitController != null)) ? 0U : this.unitController.uid, base.transform.position, list, this.wasSearched);
	}

	public override void SetTriggerVisual()
	{
		bool flag = (this.useAltTriggerOnEmpty && this.IsEmpty()) || (this.useAltTriggerOnFull && this.IsFull());
		base.SetTriggerVisual(!flag);
	}

	public override void ActivateZoneAoe()
	{
		if ((this.activateZoneAoeOnEmpty && this.IsEmpty()) || (this.activateZoneAoeOnFull && this.IsFull()))
		{
			base.ActivateZoneAoe();
		}
	}

	public bool ShouldTriggerCurse()
	{
		return this.curseId != global::SkillId.NONE && ((this.curseOnEmpty && !this.wasEmpty && this.IsEmpty()) || (this.curseOnFull && !this.wasFull && this.IsFull())) && (!this.curseOnEnemyTeam || (this.curseOnEnemyTeam && this.warbandController != null && this.warbandController.teamIdx != global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().GetWarband().teamIdx));
	}

	public override global::UnitController SpawnCampaignUnit()
	{
		if ((this.spawnOnEmpty && this.IsEmpty()) || (this.spawnOnFull && this.IsFull()))
		{
			return base.SpawnCampaignUnit();
		}
		return null;
	}

	protected override bool LinkValid(global::UnitController unitCtrlr, bool reverseCondition)
	{
		return (!reverseCondition && this.IsFull()) || (reverseCondition && this.IsEmpty());
	}

	protected override bool CanInteract(global::UnitController unitCtrlr)
	{
		return (!this.lockOnDrop || (this.lockOnDrop && !this.IsFull())) && unitCtrlr.unit.Data.UnitSizeId != global::UnitSizeId.LARGE && !unitCtrlr.unit.BothArmsMutated() && this.CompliesToSlotRestrictions(unitCtrlr) && base.CanInteract(unitCtrlr);
	}

	public bool CompliesToSlotRestrictions(global::UnitController unitCtrlr)
	{
		return this.slots.Count == 0 || (this.slots[0].restrictedItemId == global::ItemId.NONE && this.slots[0].restrictedItemTypeId == global::ItemTypeId.NONE) || (this.slots[0].restrictedItemId != global::ItemId.NONE && (!unitCtrlr.unit.IsInventoryFull() || unitCtrlr.unit.HasItem(this.slots[0].restrictedItemId, global::ItemQualityId.NONE))) || (this.slots[0].restrictedItemTypeId != global::ItemTypeId.NONE && (!unitCtrlr.unit.IsInventoryFull() || unitCtrlr.unit.HasItem(this.slots[0].restrictedItemTypeId)));
	}

	public global::Item AddItem(global::ItemSave itemSave)
	{
		return this.AddItem(new global::Item(itemSave), false);
	}

	public global::Item AddItem(global::ItemId itemId, global::ItemQualityId itemQualityId)
	{
		return this.AddItem(new global::Item(itemId, itemQualityId), false);
	}

	public global::Item AddItem(global::ItemId itemId, global::ItemQualityId itemQualityId, global::RuneMarkId runeMarkId, global::RuneMarkQualityId runeMarkQualityId, global::AllegianceId allegianceId, bool first = false)
	{
		global::Item item = new global::Item(itemId, itemQualityId);
		if (runeMarkId != global::RuneMarkId.NONE)
		{
			item.AddRuneMark(runeMarkId, runeMarkQualityId, allegianceId);
		}
		return this.AddItem(item, first);
	}

	public global::Item AddItem(global::Item item, bool first = false)
	{
		if (item.IsStackable)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].IsSame(item))
				{
					this.items[i].Save.amount += item.Save.amount;
					return this.items[i];
				}
			}
		}
		if (first)
		{
			this.items.Insert(0, item);
		}
		else
		{
			this.items.Add(item);
		}
		int num = this.items.Count - this.slots.Count;
		for (int j = 0; j < num; j++)
		{
			if (first)
			{
				this.slots.Insert(0, new global::SearchSlotData());
			}
			else
			{
				this.slots.Add(new global::SearchSlotData());
			}
		}
		return item;
	}

	public global::Item SetItem(global::ItemSave itemSave, int index)
	{
		return this.SetItem(new global::Item(itemSave), index);
	}

	public global::Item SetItem(int index, global::ItemId itemId, global::ItemQualityId itemQualityId = global::ItemQualityId.NORMAL, global::RuneMarkId runeMarkId = global::RuneMarkId.NONE, global::RuneMarkQualityId runeMarkQualityId = global::RuneMarkQualityId.NONE, global::AllegianceId allegianceId = global::AllegianceId.NONE)
	{
		global::Item item = new global::Item(itemId, itemQualityId);
		if (runeMarkId != global::RuneMarkId.NONE)
		{
			item.AddRuneMark(runeMarkId, runeMarkQualityId, allegianceId);
		}
		this.items[index] = item;
		return item;
	}

	private global::Item SetItem(global::Item item, int index)
	{
		this.items[index] = item;
		return item;
	}

	public bool Contains(global::Item item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i] == item)
			{
				return true;
			}
		}
		return false;
	}

	public global::System.Collections.Generic.List<global::Item> GetItems()
	{
		return this.items;
	}

	public virtual global::System.Collections.Generic.List<global::Item> GetObjectiveItems()
	{
		return this.items;
	}

	public virtual global::Item SwitchItem(global::UnitController unitCtrlr, int index, global::Item switchItem = null)
	{
		if (switchItem == null || (switchItem.IsStackable && switchItem.Id == this.items[index].Id))
		{
			switchItem = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		}
		global::Item result;
		if (this.CanSwitchItem(index, switchItem))
		{
			result = this.items[index];
			this.items[index] = switchItem;
			this.OnItemSwitched(unitCtrlr);
			this.Refresh();
		}
		else
		{
			result = switchItem;
		}
		this.SortItems();
		if (this.hasIdol)
		{
			this.hasIdol = false;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].IsIdol)
				{
					this.AddIdolImprint(this.items[i]);
					break;
				}
			}
			if (!this.hasIdol)
			{
				this.RemoveIdolImprint();
			}
		}
		return result;
	}

	public void SortItems()
	{
		global::Item.SortEmptyItems(this.items, 0);
	}

	public void AddIdolImprint(global::Item idol)
	{
		this.hasIdol = true;
		if (base.Imprint == null && this.warbandController != null)
		{
			this.warbandController.wagon.mapImprint.idolTexture = idol.GetIcon();
		}
		else if (base.Imprint != null)
		{
			base.Imprint.idolTexture = idol.GetIcon();
		}
	}

	public void RemoveIdolImprint()
	{
		this.hasIdol = true;
		if (base.Imprint == null && this.warbandController != null)
		{
			this.warbandController.wagon.mapImprint.idolTexture = null;
		}
		else if (base.Imprint != null)
		{
			base.Imprint.idolTexture = null;
		}
	}

	public bool CanSwitchItem(int index, global::Item switchItem)
	{
		return !switchItem.IsUndroppable && (!this.lockOnDrop || (this.lockOnDrop && this.items[index].Id == global::ItemId.NONE)) && (switchItem.Id == global::ItemId.NONE || ((this.slots[index].restrictedItemId == global::ItemId.NONE || switchItem.Id == this.slots[index].restrictedItemId) && (this.slots[index].restrictedItemTypeId == global::ItemTypeId.NONE || switchItem.TypeData.Id == this.slots[index].restrictedItemTypeId)));
	}

	protected virtual void OnItemSwitched(global::UnitController unitCtrlr)
	{
	}

	public virtual void Refresh()
	{
		int num = this.CountItems();
		for (int i = 0; i < this.contentVisuals.Count; i++)
		{
			if (i < this.contentSpawnFx.Count && !this.contentDissolvers[i].Dissolving && !this.contentDissolvers[i].Ressolved && (this.playAllContentFx || i < num))
			{
				global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.contentSpawnFx[i], null);
			}
			this.contentVisuals[i].SetActive(true);
			this.contentDissolvers[i].Hide(i + 1 > num, false, new global::UnityEngine.Events.UnityAction(this.OnContentDissolved));
		}
		if (this.visualDissolver != null && this.visualDissolver.gameObject.activeInHierarchy)
		{
			this.visualDissolver.Hide(this.hideVisualOnFill && num != 0, false, new global::UnityEngine.Events.UnityAction(this.OnVisualDissolved));
		}
	}

	private void OnContentDissolved()
	{
		for (int i = 0; i < this.contentDissolvers.Count; i++)
		{
			if (!this.contentDissolvers[i].Dissolving)
			{
				if (this.contentCutters[i] != null)
				{
					this.contentCutters[i].enabled = !this.contentDissolvers[i].Dissolved;
				}
				this.contentVisuals[i].SetActive(!this.contentDissolvers[i].Dissolved);
			}
		}
	}

	private void OnVisualDissolved()
	{
		if (this.visualNavCutter != null)
		{
			this.visualNavCutter.enabled = !this.visualDissolver.Dissolved;
		}
		this.visual.SetActive(!this.visualDissolver.Dissolved);
	}

	public virtual bool IsEmpty()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].Id != global::ItemId.NONE)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsFull()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].Id == global::ItemId.NONE)
			{
				return false;
			}
		}
		return true;
	}

	public int CountItems()
	{
		int num = 0;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].Id != global::ItemId.NONE)
			{
				num++;
			}
		}
		return num;
	}

	public global::System.Collections.Generic.List<global::Item> GetItemsAndClear()
	{
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].Id != global::ItemId.NONE)
			{
				list.Add(this.items[i]);
				this.items[i] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
		}
		return list;
	}

	public int GetEmptySlot()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].Id == global::ItemId.NONE)
			{
				return i;
			}
		}
		return -1;
	}

	public override string GetLocAction()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (!string.IsNullOrEmpty(this.loc_action_enemy) && currentUnit != null && ((this.unitController != null && this.unitController.GetWarband().teamIdx != currentUnit.GetWarband().teamIdx) || (this.warbandController != null && this.warbandController.teamIdx != currentUnit.GetWarband().teamIdx)))
		{
			return this.loc_action_enemy;
		}
		string locAction = base.GetLocAction();
		return (!string.IsNullOrEmpty(locAction)) ? locAction : "action_name_search";
	}

	public bool HasRequiredItem()
	{
		for (int i = 0; i < this.slots.Count; i++)
		{
			if (this.slots[i].restrictedItemId != global::ItemId.NONE)
			{
				return true;
			}
		}
		return false;
	}

	public global::System.Collections.Generic.List<global::ItemId> GetRestrictedItemIds()
	{
		if (this.restrictedIds == null)
		{
			this.restrictedIds = new global::System.Collections.Generic.List<global::ItemId>();
			for (int i = 0; i < this.slots.Count; i++)
			{
				this.restrictedIds.Add(this.slots[i].restrictedItemId);
			}
		}
		return this.restrictedIds;
	}

	public bool hideVisualOnFill;

	public bool destroyOnFull;

	public bool destroyOnEmpty;

	public bool destroyOnlyTriggers;

	public bool lockOnDrop;

	public bool isWyrdstone;

	public bool useAltTriggerOnFull;

	public bool useAltTriggerOnEmpty;

	public bool activateZoneAoeOnFull;

	public bool activateZoneAoeOnEmpty;

	public bool curseOnFull;

	public bool curseOnEmpty;

	public bool curseOnEnemyTeam;

	public bool spawnOnFull;

	public bool spawnOnEmpty;

	[global::UnityEngine.HideInInspector]
	public bool hasIdol;

	[global::UnityEngine.HideInInspector]
	public bool wasSearched;

	public global::System.Collections.Generic.List<global::SearchSlotData> slots;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> contentVisuals;

	public global::System.Collections.Generic.List<global::Prometheus.OlympusFireStarter> contentSpawnFx;

	private global::System.Collections.Generic.List<global::Dissolver> contentDissolvers;

	public bool playAllContentFx;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::Item> items = new global::System.Collections.Generic.List<global::Item>();

	[global::UnityEngine.HideInInspector]
	public global::UnitController unitController;

	[global::UnityEngine.HideInInspector]
	public global::WarbandController warbandController;

	public string loc_name;

	private global::System.Collections.Generic.List<global::ItemId> restrictedIds;

	private global::UnityEngine.Animation animComponent;

	private global::Pathfinding.NavmeshCut visualNavCutter;

	private global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> contentCutters = new global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut>();

	private bool wasEmpty;

	private bool wasFull;
}
