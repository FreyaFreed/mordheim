using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomizationWheelModule : global::UIModule
{
	public bool IsFocused { get; set; }

	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.bodyPartSlot.Count; i++)
		{
			int idx = i;
			this.bodyPartSlot[i].slot.onAction.AddListener(delegate()
			{
				this.ConfirmBodyPartSlot(idx);
			});
			this.bodyPartSlot[i].slot.onSelect.AddListener(delegate()
			{
				this.ShowBodyPartSlotDescription(idx, false);
			});
			this.bodyPartSlot[i].slot.onPointerEnter.AddListener(delegate()
			{
				this.ShowBodyPartSlotDescription(idx, false);
			});
		}
		this.presetSlot.slot.onAction.AddListener(delegate()
		{
			this.ConfirmPresetsSlot();
		});
		this.presetSlot.slot.onSelect.AddListener(delegate()
		{
			this.ShowPresetsSlotDescription(false);
		});
		this.presetSlot.slot.onPointerEnter.AddListener(delegate()
		{
			this.ShowPresetsSlotDescription(false);
		});
		this.skinSlot.slot.onAction.AddListener(delegate()
		{
			this.ConfirmSkinSlot();
		});
		this.skinSlot.slot.onSelect.AddListener(delegate()
		{
			this.ShowSkinSlotDescription(false);
		});
		this.skinSlot.slot.onPointerEnter.AddListener(delegate()
		{
			this.ShowSkinSlotDescription(false);
		});
	}

	public void Activate(global::UnityEngine.UI.Selectable leftNavItem, global::UnityEngine.Events.UnityAction<global::BodyPartId, global::UnityEngine.Sprite> showSlotDescription, global::UnityEngine.Events.UnityAction<global::BodyPartId> slotConfirmed, global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite> showPresetsDescription, global::UnityEngine.Events.UnityAction presetsConfirmed, global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite> showSkinDescription, global::UnityEngine.Events.UnityAction skinConfirmed)
	{
		base.SetInteractable(true);
		this.leftNavItem = leftNavItem;
		this.onShowSlotDescription = showSlotDescription;
		this.onBodyPartSlotConfirmed = slotConfirmed;
		this.onShowPresetsSlotDescription = showPresetsDescription;
		this.onPresetsSlotConfirmed = presetsConfirmed;
		this.onShowSkinSlotDescription = showSkinDescription;
		this.onSkinSlotConfirmed = skinConfirmed;
		this.lastSelected = null;
		this.SelectLastSelected();
	}

	public void Deactivate()
	{
		base.SetInteractable(false);
		this.onBodyPartSlotConfirmed = null;
	}

	private void OnDisable()
	{
		if (this.lastSelected != null)
		{
			this.lastSelected.isOn = false;
		}
	}

	public void ShowBodyPartSlotDescription(int bodyPartIdx, bool force = false)
	{
		if (this.onShowSlotDescription != null && (this.IsFocused || force))
		{
			this.onShowSlotDescription(this.bodyPartSlot[bodyPartIdx].bodyPart, this.bodyPartSlot[bodyPartIdx].icon.sprite);
		}
	}

	public void ConfirmBodyPartSlot(int bodyPartIdx)
	{
		this.ShowBodyPartSlotDescription(bodyPartIdx, true);
		this.lastSelected = this.bodyPartSlot[bodyPartIdx].slot.toggle;
		if (this.onBodyPartSlotConfirmed != null)
		{
			this.IsFocused = false;
			this.onBodyPartSlotConfirmed(this.bodyPartSlot[bodyPartIdx].bodyPart);
		}
	}

	public void ShowPresetsSlotDescription(bool force = false)
	{
		if (this.onShowPresetsSlotDescription != null && (this.IsFocused || force))
		{
			this.onShowPresetsSlotDescription(this.presetSlot.icon.sprite);
		}
	}

	public void ConfirmPresetsSlot()
	{
		this.ShowPresetsSlotDescription(true);
		this.lastSelected = this.presetSlot.slot.toggle;
		if (this.onPresetsSlotConfirmed != null)
		{
			this.IsFocused = false;
			this.onPresetsSlotConfirmed();
		}
	}

	public void ShowSkinSlotDescription(bool force = false)
	{
		if (this.onShowSkinSlotDescription != null && (this.IsFocused || force))
		{
			this.onShowSkinSlotDescription(this.skinSlot.icon.sprite);
		}
	}

	public void ConfirmSkinSlot()
	{
		this.ShowSkinSlotDescription(true);
		this.lastSelected = this.skinSlot.slot.toggle;
		if (this.onSkinSlotConfirmed != null)
		{
			this.IsFocused = false;
			this.onSkinSlotConfirmed();
		}
	}

	public void SelectBodyPartSlot(global::BodyPartId bodyPart)
	{
		for (int i = 0; i < this.bodyPartSlot.Count; i++)
		{
			if (this.bodyPartSlot[i].bodyPart == bodyPart)
			{
				this.bodyPartSlot[i].SetSelected(true);
				break;
			}
		}
	}

	private bool IsBodyPartCustomizable(global::UnitMenuController unitController, global::BodyPartId bodyPart)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = unitController.unit.bodyParts.ContainsKey(bodyPart);
		if (flag3)
		{
			global::BodyPart bodyPart2 = unitController.unit.bodyParts[bodyPart];
			flag = bodyPart2.Customizable;
			flag2 = (bodyPart2.GetAvailableModels().Count > 1 || bodyPart2.GetAvailableMaterials(false).Count > 1);
		}
		return flag3 && flag && flag2;
	}

	public void RefreshSlots(global::UnitMenuController unitController, bool hasPresets)
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < this.bodyPartSlot.Count; i++)
		{
			this.bodyPartSlot[i].SetLocked(!this.IsBodyPartCustomizable(unitController, this.bodyPartSlot[i].bodyPart));
			if (this.bodyPartSlot[i].bodyPart == global::BodyPartId.GEAR_HEAD || this.bodyPartSlot[i].bodyPart == global::BodyPartId.HELMET)
			{
				if (!flag && this.bodyPartSlot[i].IsLocked())
				{
					this.bodyPartSlot[i].gameObject.SetActive(false);
				}
				else if (flag && !flag2)
				{
					this.bodyPartSlot[i].gameObject.SetActive(false);
				}
				else
				{
					this.bodyPartSlot[i].gameObject.SetActive(true);
				}
				flag2 = this.bodyPartSlot[i].IsLocked();
				flag = true;
			}
		}
		bool flag3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkinColorData>("fk_unit_id", unitController.unit.Data.Id.ToIntString<global::UnitId>()).Count > 1;
		this.skinSlot.SetLocked(!flag3);
		this.presetSlot.SetLocked(!hasPresets);
		for (int j = 0; j < this.bodyPartSlot.Count; j++)
		{
			if (this.bodyPartSlot[j].isActiveAndEnabled)
			{
				this.AutoLinkNavNode(this.bodyPartSlot[j]);
			}
		}
		if (flag3)
		{
			this.AutoLinkNavNode(this.skinSlot);
		}
		if (hasPresets)
		{
			this.AutoLinkNavNode(this.presetSlot);
		}
	}

	private void AutoLinkNavNode(global::UIWheelSlot node)
	{
		global::UnityEngine.UI.Navigation navigation = node.slot.toggle.navigation;
		global::System.Collections.Generic.List<global::UIWheelSlot> list = new global::System.Collections.Generic.List<global::UIWheelSlot>();
		global::UIWheelSlot uiwheelSlot = node;
		list.Clear();
		navigation.selectOnUp = null;
		while (uiwheelSlot != null && !list.Contains(uiwheelSlot))
		{
			list.Add(uiwheelSlot);
			if (uiwheelSlot.defaultUpItem != null && uiwheelSlot.defaultUpItem.isActiveAndEnabled && uiwheelSlot.defaultUpItem.toggle.interactable)
			{
				navigation.selectOnUp = uiwheelSlot.defaultUpItem.toggle;
				break;
			}
			if (uiwheelSlot.altUpItem != null && uiwheelSlot.altUpItem.isActiveAndEnabled && uiwheelSlot.altUpItem.toggle.interactable)
			{
				navigation.selectOnUp = uiwheelSlot.altUpItem.toggle;
				break;
			}
			if (!(uiwheelSlot.defaultUpItem != null))
			{
				break;
			}
			uiwheelSlot = uiwheelSlot.defaultUpItem.GetComponent<global::UIWheelSlot>();
		}
		uiwheelSlot = node;
		list.Clear();
		navigation.selectOnDown = null;
		while (uiwheelSlot != null && !list.Contains(uiwheelSlot))
		{
			list.Add(uiwheelSlot);
			if (uiwheelSlot.defaultDownItem != null && uiwheelSlot.defaultDownItem.isActiveAndEnabled && uiwheelSlot.defaultDownItem.toggle.interactable)
			{
				navigation.selectOnDown = uiwheelSlot.defaultDownItem.toggle;
				break;
			}
			if (uiwheelSlot.altDownItem != null && uiwheelSlot.altDownItem.isActiveAndEnabled && uiwheelSlot.altDownItem.toggle.interactable)
			{
				navigation.selectOnDown = uiwheelSlot.altDownItem.toggle;
				break;
			}
			if (!(uiwheelSlot.defaultDownItem != null))
			{
				break;
			}
			uiwheelSlot = uiwheelSlot.defaultDownItem.GetComponent<global::UIWheelSlot>();
		}
		uiwheelSlot = node;
		list.Clear();
		navigation.selectOnLeft = null;
		while (uiwheelSlot != null && !list.Contains(uiwheelSlot))
		{
			list.Add(uiwheelSlot);
			if (uiwheelSlot.defaultLeftItem != null && uiwheelSlot.defaultLeftItem.isActiveAndEnabled && uiwheelSlot.defaultLeftItem.toggle.interactable)
			{
				navigation.selectOnLeft = uiwheelSlot.defaultLeftItem.toggle;
				break;
			}
			if (uiwheelSlot.altLeftItem != null && uiwheelSlot.altLeftItem.isActiveAndEnabled && uiwheelSlot.altLeftItem.toggle.interactable)
			{
				navigation.selectOnLeft = uiwheelSlot.altLeftItem.toggle;
				break;
			}
			if (!(uiwheelSlot.defaultLeftItem != null))
			{
				break;
			}
			uiwheelSlot = uiwheelSlot.defaultLeftItem.GetComponent<global::UIWheelSlot>();
		}
		if (uiwheelSlot != null && uiwheelSlot.defaultLeftItem == null)
		{
			navigation.selectOnLeft = this.leftNavItem;
		}
		uiwheelSlot = node;
		list.Clear();
		navigation.selectOnRight = null;
		while (uiwheelSlot != null && !list.Contains(uiwheelSlot))
		{
			list.Add(uiwheelSlot);
			if (uiwheelSlot.defaultRightItem != null && uiwheelSlot.defaultRightItem.isActiveAndEnabled && uiwheelSlot.defaultRightItem.toggle.interactable)
			{
				navigation.selectOnRight = uiwheelSlot.defaultRightItem.toggle;
				break;
			}
			if (uiwheelSlot.altRightItem != null && uiwheelSlot.altRightItem.isActiveAndEnabled && uiwheelSlot.altRightItem.toggle.interactable)
			{
				navigation.selectOnRight = uiwheelSlot.altRightItem.toggle;
				break;
			}
			if (!(uiwheelSlot.defaultRightItem != null))
			{
				break;
			}
			uiwheelSlot = uiwheelSlot.defaultRightItem.GetComponent<global::UIWheelSlot>();
		}
		node.slot.toggle.navigation = navigation;
	}

	private void OnEnable()
	{
		if (this.toggleGroup != null)
		{
			this.toggleGroup.SetAllTogglesOff();
		}
	}

	public void SelectLastSelected()
	{
		if (this.lastSelected != null && this.lastSelected.isActiveAndEnabled && this.lastSelected.IsInteractable())
		{
			this.lastSelected.SetSelected(true);
		}
		else if (this.presetSlot.isActiveAndEnabled && this.presetSlot.slot.toggle.IsInteractable())
		{
			this.presetSlot.SetSelected(true);
			this.lastSelected = this.presetSlot.slot.toggle;
		}
		else if (this.skinSlot.isActiveAndEnabled && this.skinSlot.slot.toggle.IsInteractable())
		{
			this.skinSlot.SetSelected(true);
			this.lastSelected = this.skinSlot.slot.toggle;
		}
		else
		{
			for (int i = 0; i < this.bodyPartSlot.Count; i++)
			{
				if (this.bodyPartSlot[i].isActiveAndEnabled && this.bodyPartSlot[i].slot.toggle.IsInteractable())
				{
					this.bodyPartSlot[i].SetSelected(true);
					this.lastSelected = this.bodyPartSlot[i].slot.toggle;
					break;
				}
			}
		}
	}

	public global::System.Collections.Generic.List<global::UIWheelBodyPartSlot> bodyPartSlot;

	public global::UIWheelBodyPartSlot presetSlot;

	public global::UIWheelBodyPartSlot skinSlot;

	private global::UnityEngine.Events.UnityAction<global::BodyPartId> onBodyPartSlotConfirmed;

	private global::UnityEngine.Events.UnityAction<global::BodyPartId, global::UnityEngine.Sprite> onShowSlotDescription;

	private global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite> onShowPresetsSlotDescription;

	private global::UnityEngine.Events.UnityAction onPresetsSlotConfirmed;

	private global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite> onShowSkinSlotDescription;

	private global::UnityEngine.Events.UnityAction onSkinSlotConfirmed;

	private global::UnityEngine.UI.Selectable leftNavItem;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;

	private bool focused;

	private global::UnityEngine.UI.Toggle lastSelected;
}
