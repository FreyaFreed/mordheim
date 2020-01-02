using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WheelModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.itemSlots.Count; i++)
		{
			int idx = i;
			this.itemSlots[i].slot.onAction.AddListener(delegate()
			{
				this.ConfirmItemSlot(idx);
			});
			this.itemSlots[i].slot.onSelect.AddListener(delegate()
			{
				this.ShowItemSlotDescription(idx);
			});
			this.itemSlots[i].slot.onPointerEnter.AddListener(delegate()
			{
				this.ShowItemSlotDescription(idx);
			});
		}
		for (int j = 0; j < this.mutationSlots.Count; j++)
		{
			int idx = j;
			this.mutationSlots[j].slot.onAction.AddListener(delegate()
			{
				this.ConfirmMutationSlot(idx);
			});
			this.mutationSlots[j].slot.onSelect.AddListener(delegate()
			{
				this.ShowMutationSlotDescription(idx);
			});
			this.mutationSlots[j].slot.onPointerEnter.AddListener(delegate()
			{
				this.ShowMutationSlotDescription(idx);
			});
		}
	}

	public void Activate(global::ToggleEffects left, global::UnityEngine.Events.UnityAction<global::UnitSlotId> showSlotDescription, global::UnityEngine.Events.UnityAction<int> showMutationDescription, global::UnityEngine.Events.UnityAction<global::UnitSlotId> slotConfirmed, global::UnityEngine.Events.UnityAction<int> mutationConfirmed)
	{
		base.SetInteractable(true);
		this.onItemSlotConfirmed = slotConfirmed;
		this.onShowSlotDescription = showSlotDescription;
		this.onMutationSlotConfirmed = mutationConfirmed;
		this.onShowMutationDescription = showMutationDescription;
		for (int i = 0; i <= 5; i++)
		{
			if (i != 3 && i != 5)
			{
				this.itemSlots[i].SetLeftSelectable(left);
			}
		}
		for (int j = 0; j < this.mutationSlots.Count; j++)
		{
			this.mutationSlots[j].SetLeftSelectable(left);
		}
	}

	public void Deactivate()
	{
		base.SetInteractable(false);
		this.onItemSlotConfirmed = null;
	}

	public void ConfirmItemSlot(int idx)
	{
		if (this.onItemSlotConfirmed != null)
		{
			this.onItemSlotConfirmed((global::UnitSlotId)idx);
		}
	}

	public void ConfirmMutationSlot(int idx)
	{
		if (this.onMutationSlotConfirmed != null)
		{
			this.onMutationSlotConfirmed(this.mutationSlots[idx].unitMutationIdx);
		}
	}

	public void ShowItemSlotDescription(int idx)
	{
		if (this.onShowSlotDescription != null && !this.isLocked)
		{
			this.onShowSlotDescription((global::UnitSlotId)idx);
		}
	}

	public void ShowMutationSlotDescription(int idx)
	{
		if (this.onShowMutationDescription != null && !this.isLocked)
		{
			this.onShowMutationDescription(this.mutationSlots[idx].unitMutationIdx);
		}
	}

	public void SelectSlot(int idx)
	{
		this.itemSlots[idx].slot.SetOn();
		this.itemSlots[idx].slot.SetSelected(true);
	}

	public void RefreshSlots(global::UnitMenuController unitController)
	{
		global::Unit unit = unitController.unit;
		this.secondaryWeaponLinkGroup.SetActive(unitController.CanSwitchWeapon());
		for (int i = 0; i < this.itemSlots.Count; i++)
		{
			bool flag = i >= unit.Items.Count;
			if (!flag)
			{
				flag = ((i == 3 || i == 5) && (unit.Items[i - 1].IsPaired || unit.Items[i - 1].IsTwoHanded));
			}
			if (!flag)
			{
				flag = (unit.GetInjury((global::UnitSlotId)i) != global::InjuryId.NONE);
			}
			if (!flag)
			{
				flag = ((i == 4 || i == 5) && unit.GetMutationId((global::UnitSlotId)i) != global::MutationId.NONE);
			}
			if (!flag)
			{
				flag = ((i == 4 || i == 5) && unit.Items[i - 2].IsLockSlot);
			}
			if (!flag)
			{
				flag = ((i == 3 || i == 5) && unit.Items[i - 1].Id == global::ItemId.NONE);
			}
			if (!flag)
			{
				flag = (i >= 7 && (unit.BothArmsMutated() || unit.Data.UnitSizeId == global::UnitSizeId.LARGE || unit.IsUnitActionBlocked(global::UnitActionId.CONSUMABLE)));
			}
			this.itemSlots[i].slot.gameObject.SetActive(true);
			if (flag)
			{
				this.itemSlots[i].slot.toggle.interactable = false;
				this.itemSlots[i].icon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item/none", true);
				this.itemSlots[i].icon.color = global::UnityEngine.Color.white;
				if (this.itemSlots[i].subIcon != null)
				{
					this.itemSlots[i].subIcon.gameObject.SetActive(false);
				}
			}
			else
			{
				this.itemSlots[i].slot.toggle.interactable = true;
				if (unit.Items[i].Id == global::ItemId.NONE)
				{
					this.itemSlots[i].icon.sprite = this.freeSlotIcon;
					this.itemSlots[i].icon.color = global::UnityEngine.Color.white;
				}
				else
				{
					this.itemSlots[i].icon.sprite = unit.Items[i].GetIcon();
					this.itemSlots[i].icon.color = global::PandoraUtils.StringToColor(unit.Items[i].QualityData.Color);
				}
				global::UnityEngine.Sprite runeIcon = unit.Items[i].GetRuneIcon();
				if (runeIcon != null)
				{
					this.itemSlots[i].subIcon.gameObject.SetActive(true);
					this.itemSlots[i].subIcon.sprite = runeIcon;
					this.itemSlots[i].icon.color = global::PandoraUtils.StringToColor(unit.Items[i].QualityData.Color);
				}
				else if (this.itemSlots[i].subIcon != null)
				{
					this.itemSlots[i].subIcon.gameObject.SetActive(false);
				}
			}
		}
		for (int j = 0; j < this.mutationSlots.Count; j++)
		{
			bool flag2 = false;
			for (int k = 0; k < unit.Mutations.Count; k++)
			{
				if (unit.Mutations[k].HasBodyPart(this.mutationSlots[j].partId))
				{
					flag2 = true;
					this.mutationSlots[j].slot.gameObject.SetActive(true);
					this.mutationSlots[j].slot.toggle.interactable = true;
					this.mutationSlots[j].icon.sprite = unit.Mutations[k].GetIcon();
					this.mutationSlots[j].icon.color = global::UnityEngine.Color.white;
					this.mutationSlots[j].unitMutationIdx = k;
					if (this.mutationSlots[j].subIcon != null)
					{
						this.mutationSlots[j].subIcon.gameObject.SetActive(false);
					}
					break;
				}
			}
			if (!flag2)
			{
				if (!this.mutationSlots[j].hiddingSlot)
				{
					this.mutationSlots[j].slot.gameObject.SetActive(false);
				}
				this.mutationSlots[j].unitMutationIdx = -1;
			}
		}
		for (int l = 0; l < this.itemSlots.Count; l++)
		{
			this.itemSlots[l].RefreshNavigation();
		}
		for (int m = 0; m < this.mutationSlots.Count; m++)
		{
			this.mutationSlots[m].RefreshNavigation();
		}
	}

	private void OnEnable()
	{
		if (this.toggleGroup != null)
		{
			this.toggleGroup.SetAllTogglesOff();
		}
	}

	public void Lock()
	{
		this.isLocked = true;
	}

	public void Unlock()
	{
		this.isLocked = false;
	}

	public global::System.Collections.Generic.List<global::UIWheelSlot> itemSlots;

	public global::System.Collections.Generic.List<global::UIWheelMutationSlot> mutationSlots;

	private global::UnityEngine.Events.UnityAction<global::UnitSlotId> onItemSlotConfirmed;

	private global::UnityEngine.Events.UnityAction<int> onMutationSlotConfirmed;

	private global::UnityEngine.Events.UnityAction<global::UnitSlotId> onShowSlotDescription;

	private global::UnityEngine.Events.UnityAction<int> onShowMutationDescription;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;

	private bool isLocked;

	public global::UnityEngine.GameObject mainWeaponLinkGroup;

	public global::UnityEngine.GameObject secondaryWeaponLinkGroup;

	public global::UnityEngine.Sprite freeSlotIcon;
}
