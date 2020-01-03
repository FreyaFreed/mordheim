using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WarbandSlotPlacementModule : global::UIModule
{
	protected bool IsImpressiveAvailable { get; set; }

	public override void Init()
	{
		base.Init();
		this.allSlots = new global::System.Collections.Generic.List<global::UIUnitSlot>();
		this.allImpressiveSlots = new global::System.Collections.Generic.List<global::UIUnitSlot>();
		this.allSlots.Add(null);
		this.allSlots.Add(null);
		this.allSlots.AddRange(this.leaderSlots);
		this.allSlots.AddRange(this.heroSlots);
		this.allSlots.AddRange(this.heroImpressiveSlots);
		this.allSlots.AddRange(this.henchmenSlots);
		this.allSlots.AddRange(this.reserveSlots);
		this.allImpressiveSlots.AddRange(this.impressiveSlots);
		this.allImpressiveSlots.AddRange(this.reserveImpressiveSlots);
	}

	public void Set(global::Warband warband, global::System.Collections.Generic.List<int> unitPosition, int ratingMin = 0, int ratingMax = 9999)
	{
		this.warbandRatingMax = ratingMax;
		this.warbandRatingMin = ratingMin;
		this.currentWarband = warband;
		this.warbandRankSlotData = this.currentWarband.GetWarbandSlots();
		this.IsImpressiveAvailable = this.currentWarband.IsUnitTypeUnlocked(global::UnitTypeId.IMPRESSIVE);
		this.unitsPosition = unitPosition;
	}

	public void SetupAvailableSlots()
	{
		this.SetupSlotSection(this.leaderSlots, global::WarbandSlotTypeId.LEADER, this.warbandRankSlotData.Leader, false);
		this.SetupSlotSection(this.heroSlots, global::WarbandSlotTypeId.HERO, this.warbandRankSlotData.Hero, false);
		this.SetupImpressiveSlotSection(this.heroImpressiveSlots, this.impressiveSlots, global::WarbandSlotTypeId.HERO_IMPRESSIVE, this.warbandRankSlotData.Impressive);
		this.SetupSlotSection(this.henchmenSlots, global::WarbandSlotTypeId.HENCHMEN, this.warbandRankSlotData.Henchman, false);
		this.SetupImpressiveSlotSection(this.reserveSlots, this.reserveImpressiveSlots, global::WarbandSlotTypeId.RESERVE, this.warbandRankSlotData.Reserve);
	}

	protected virtual void SetupWarbandSlot(global::UIUnitSlot slot, global::Unit unit, int slotIndex, bool isLocked, bool hide = false)
	{
		if (hide)
		{
			slot.gameObject.SetActive(false);
		}
		else if (isLocked)
		{
			slot.Set(unit, slotIndex, new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotOver), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotSelected), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotConfirmed), this.unitsPosition == null);
			slot.Activate();
			slot.Lock(this.lockIcon);
		}
		else
		{
			slot.Set(unit, slotIndex, new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotOver), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotSelected), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotConfirmed), this.unitsPosition == null);
			slot.Activate();
		}
	}

	private void SetupSlotSection(global::UIUnitSlot[] slots, global::WarbandSlotTypeId slotTypeId, int rankSlotAvailable, bool hideEmpty = false)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			int slotIndex = (int)(slotTypeId + i);
			global::Unit unitAtWarbandSlot = this.GetUnitAtWarbandSlot(slotIndex);
			this.SetupWarbandSlot(slots[i], unitAtWarbandSlot, slotIndex, rankSlotAvailable == 0 || i >= rankSlotAvailable, hideEmpty && i >= rankSlotAvailable);
		}
	}

	private void SetupImpressiveSlotSection(global::UIUnitSlot[] slots, global::UIUnitSlot[] linkedImpressiveSlots, global::WarbandSlotTypeId slotTypeId, int rankSlotAvailable)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			int num = (int)(slotTypeId + i);
			global::Unit unitAtWarbandSlot = this.GetUnitAtWarbandSlot(num);
			int num2 = i / 2;
			bool flag = i % 2 == 0;
			if (flag && unitAtWarbandSlot != null && unitAtWarbandSlot.IsImpressive)
			{
				this.SetupWarbandSlot(linkedImpressiveSlots[num2], unitAtWarbandSlot, num, !this.IsImpressiveAvailable || i >= rankSlotAvailable, false);
				this.SetupWarbandSlot(slots[i], unitAtWarbandSlot, num, true, false);
				slots[i].Deactivate();
				this.SetupWarbandSlot(slots[i + 1], unitAtWarbandSlot, num, true, false);
				slots[i + 1].Deactivate();
				i++;
			}
			else
			{
				this.SetupWarbandSlot(slots[i], unitAtWarbandSlot, num, rankSlotAvailable == 0 || i >= rankSlotAvailable, false);
				if (flag)
				{
					this.SetupWarbandSlot(linkedImpressiveSlots[num2], null, num, !this.IsImpressiveAvailable || i + 1 >= rankSlotAvailable, false);
					if (unitAtWarbandSlot != null || this.GetUnitAtWarbandSlot(num + 1) != null)
					{
						linkedImpressiveSlots[num2].icon.overrideSprite = this.lockIcon;
						linkedImpressiveSlots[num2].icon.color = global::UnityEngine.Color.white;
						linkedImpressiveSlots[num2].Deactivate();
					}
				}
			}
		}
	}

	protected global::Unit GetUnitAtWarbandSlot(int slotIndex)
	{
		if (this.unitsPosition == null)
		{
			return this.currentWarband.GetUnitAtWarbandSlot(slotIndex, true);
		}
		int num = this.unitsPosition.IndexOf(slotIndex);
		if (num == -1)
		{
			return null;
		}
		return this.currentWarband.Units[num];
	}

	protected int GetUnitSlotIndex(global::Unit unit)
	{
		if (this.unitsPosition != null)
		{
			int index = this.currentWarband.Units.IndexOf(unit);
			return this.unitsPosition[index];
		}
		return unit.UnitSave.warbandSlotIndex;
	}

	protected void SetUnitSlotIndex(global::Unit unit, int slotIndex, bool checkIfAvailable = false)
	{
		if (this.unitsPosition != null)
		{
			int index = this.currentWarband.Units.IndexOf(unit);
			this.unitsPosition[index] = slotIndex;
		}
		else
		{
			unit.UnitSave.warbandSlotIndex = slotIndex;
		}
	}

	protected int GetActiveUnitIdCount(global::UnitId unitId, global::System.Collections.Generic.List<int> excludeSlots = null)
	{
		if (this.unitsPosition != null)
		{
			int num = 0;
			for (int i = 0; i < this.unitsPosition.Count; i++)
			{
				if (this.currentWarband.IsActiveWarbandSlot(this.unitsPosition[i]) && this.currentWarband.Units[i].Id == unitId && (excludeSlots == null || !excludeSlots.Contains(this.unitsPosition[i])))
				{
					num++;
				}
			}
			return num;
		}
		return this.currentWarband.GetActiveUnitIdCount(unitId, excludeSlots);
	}

	protected bool IsUnitCountExceeded(global::Unit unit, int excludeSlot1, int excludeSlot2)
	{
		this.tempList[0] = excludeSlot1;
		this.tempList[1] = excludeSlot2;
		return this.GetActiveUnitIdCount(unit.Id, this.tempList) >= unit.Data.MaxCount;
	}

	protected bool CanPlaceUnitAt(global::Unit unit, int toIndex)
	{
		return (this.unitsPosition != null || unit.GetActiveStatus() != global::UnitActiveStatusId.IN_TRAINING || !this.skillsShop.IsSkillChangeType(unit.UnitSave.skillInTrainingId) || toIndex >= 12) && this.currentWarband.CanPlaceUnitAt(unit, toIndex) && (!this.currentWarband.IsActiveWarbandSlot(toIndex) || !this.IsUnitCountExceeded(unit, unit.UnitSave.warbandSlotIndex, toIndex));
	}

	protected int GetActiveUnitsCount()
	{
		if (this.unitsPosition != null)
		{
			int num = 0;
			for (int i = 0; i < this.unitsPosition.Count; i++)
			{
				if (this.currentWarband.IsActiveWarbandSlot(this.unitsPosition[i]))
				{
					num++;
				}
			}
			return num;
		}
		return this.currentWarband.GetNbActiveUnits(false);
	}

	protected int FindEmptyAvailableSlot()
	{
		int num = this.currentWarband.GetNbMaxActiveSlots() + this.currentWarband.GetNbMaxReserveSlot();
		for (int i = 0; i < num; i++)
		{
			if (!this.unitsPosition.Contains(12 + this.reserveSlots.Length + i))
			{
				return 12 + this.reserveSlots.Length + i;
			}
		}
		return -1;
	}

	protected abstract void OnUnitSlotOver(int slotIndex, global::Unit unit, bool isImpressive);

	protected abstract void OnUnitSlotSelected(int slotIndex, global::Unit unit, bool isImpressive);

	protected abstract void OnUnitSlotConfirmed(int slotIndex, global::Unit unit, bool isImpressive);

	protected void CreateAdditionalReserveSlotGroup(global::UnityEngine.Transform transformtoinstantiate, global::UnityEngine.Vector3 newlocalposition)
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(transformtoinstantiate.gameObject);
		global::UnityEngine.Transform transform = gameObject.transform;
		transform.parent = transformtoinstantiate.parent;
		transform.localScale = transformtoinstantiate.localScale;
		transform.localRotation = transformtoinstantiate.localRotation;
		transform.localPosition = newlocalposition;
		global::UIUnitSlot[] componentsInChildren = gameObject.GetComponentsInChildren<global::UIUnitSlot>(true);
		if (componentsInChildren.Length == 3)
		{
			global::System.Array.Resize<global::UIUnitSlot>(ref this.reserveImpressiveSlots, this.reserveImpressiveSlots.Length + 1);
			this.reserveImpressiveSlots[this.reserveImpressiveSlots.Length - 1] = componentsInChildren[0];
			global::System.Array.Resize<global::UIUnitSlot>(ref this.reserveSlots, this.reserveSlots.Length + 2);
			this.reserveSlots[this.reserveSlots.Length - 2] = componentsInChildren[1];
			this.reserveSlots[this.reserveSlots.Length - 1] = componentsInChildren[2];
		}
	}

	private global::SkillsShop skillsShop = new global::SkillsShop();

	public global::UnityEngine.Sprite lockIcon;

	public global::UnityEngine.Sprite noneIcon;

	public global::UnityEngine.Sprite plusIcon;

	public global::UnityEngine.Sprite swapIcon;

	public global::UIUnitSlot[] leaderSlots;

	public global::UIUnitSlot[] impressiveSlots;

	public global::UIUnitSlot[] heroSlots;

	public global::UIUnitSlot[] heroImpressiveSlots;

	public global::UIUnitSlot[] henchmenSlots;

	public global::UIUnitSlot[] reserveSlots;

	public global::UIUnitSlot[] reserveImpressiveSlots;

	protected global::System.Collections.Generic.List<global::UIUnitSlot> allSlots;

	protected global::System.Collections.Generic.List<global::UIUnitSlot> allImpressiveSlots;

	private global::WarbandRankSlotData warbandRankSlotData;

	protected global::Warband currentWarband;

	protected global::System.Collections.Generic.List<int> unitsPosition;

	protected int warbandRatingMin;

	protected int warbandRatingMax;

	private readonly global::System.Collections.Generic.List<int> tempList = new global::System.Collections.Generic.List<int>
	{
		-1,
		-1
	};
}
