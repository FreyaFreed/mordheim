using System;
using UnityEngine;
using UnityEngine.UI;

public class WarbandManagementModule : global::WarbandSlotPlacementModule
{
	public override void Init()
	{
		base.Init();
		if (this.swap != null)
		{
			this.swap.onAction.AddListener(delegate()
			{
				this.onSwap();
			});
		}
		this.hiredSwords.onSelect.AddListener(delegate()
		{
			this.onHiredSwordsSelected();
		});
		this.hiredSwords.onAction.AddListener(delegate()
		{
			this.onHiredSwordsConfirmed();
		});
	}

	public void Set(global::Warband warband, global::System.Action<int, global::Unit, bool> unitSelected, global::System.Action<int, global::Unit, bool> unitConfirmed, global::System.Action swapUnits = null, global::System.Action hiredSwordsSelected = null, global::System.Action hiredSwordsConfirmed = null)
	{
		base.Set(warband, null, 0, 9999);
		if (this.idol != null)
		{
			this.idol.Set(0, new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotOver), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotSelected), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotConfirmed));
			this.idol.Activate();
			this.idol.icon.sprite = global::Warband.GetIcon(warband.Id);
			if (warband.GetTotalTreatmentOwned() > 0)
			{
				this.warbandFees.SetActive(true);
				this.warbandFees.GetComponentInChildren<global::UnityEngine.UI.Text>().text = warband.GetTotalTreatmentOwned().ToString();
			}
			else if (warband.GetTotalUpkeepOwned() > 0)
			{
				this.warbandFees.SetActive(true);
				this.warbandFees.GetComponentInChildren<global::UnityEngine.UI.Text>().text = warband.GetTotalUpkeepOwned().ToString();
			}
			else
			{
				this.warbandFees.SetActive(false);
			}
		}
		if (this.swap != null)
		{
			this.onSwap = swapUnits;
		}
		this.onHiredSwordsSelected = hiredSwordsSelected;
		this.onHiredSwordsConfirmed = hiredSwordsConfirmed;
		this.onUnitConfirmed = unitConfirmed;
		this.onUnitSelected = unitSelected;
		this.currentWarband = warband;
		this.Refresh();
	}

	public void Refresh()
	{
		this.canHireUnit = this.currentWarband.CanHireMoreUnit(false);
		this.canHireImpressive = this.currentWarband.CanHireMoreUnit(true);
		base.SetupAvailableSlots();
	}

	protected override void SetupWarbandSlot(global::UIUnitSlot slot, global::Unit unit, int slotIndex, bool isLocked, bool hide = false)
	{
		if (isLocked || hide)
		{
			if (hide)
			{
				slot.gameObject.SetActive(false);
			}
			else
			{
				slot.Set(unit, slotIndex, new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotOver), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotSelected), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotConfirmed), true);
				slot.Activate();
				slot.Lock(this.lockIcon);
			}
		}
		else
		{
			slot.Set(unit, slotIndex, new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotOver), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotSelected), new global::System.Action<int, global::Unit, bool>(this.OnUnitSlotConfirmed), true);
			if (unit == null && ((slot.isImpressive && !this.canHireImpressive) || (!slot.isImpressive && !this.canHireUnit)))
			{
				slot.icon.color = global::UnityEngine.Color.white;
				slot.icon.overrideSprite = this.noneIcon;
			}
			slot.Activate();
		}
	}

	protected override void OnUnitSlotOver(int slotIndex, global::Unit unit, bool isImpressive)
	{
	}

	protected override void OnUnitSlotSelected(int slotIndex, global::Unit unit, bool isImpressive)
	{
		this.onUnitSelected(slotIndex, unit, isImpressive);
	}

	protected override void OnUnitSlotConfirmed(int slotIndex, global::Unit unit, bool isImpressive)
	{
		this.onUnitConfirmed(slotIndex, unit, isImpressive);
	}

	public global::ToggleEffects swap;

	public global::ToggleEffects hiredSwords;

	public global::UIUnitSlot idol;

	public global::UnityEngine.GameObject warbandFees;

	private global::System.Action<int, global::Unit, bool> onUnitSelected;

	private global::System.Action<int, global::Unit, bool> onUnitConfirmed;

	private global::System.Action onSwap;

	private global::System.Action onHiredSwordsSelected;

	private global::System.Action onHiredSwordsConfirmed;

	private bool canHireUnit;

	private bool canHireImpressive;
}
