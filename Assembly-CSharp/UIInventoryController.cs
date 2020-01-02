using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryController : global::CanvasGroupDisabler
{
	private void Awake()
	{
		if (global::UIInventoryController.NONE_ITEM == null)
		{
			global::UIInventoryController.NONE_ITEM = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		}
	}

	private bool CanTakeAll()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.currentSearchPoint.items.Count; i++)
		{
			if (this.currentSearchPoint.items[i].Id != global::ItemId.NONE && this.currentSearchPoint.CanSwitchItem(i, global::UIInventoryController.NONE_ITEM))
			{
				if (this.currentSearchPoint.items[i].IsStackable)
				{
					global::ItemId itemId = this.currentSearchPoint.items[i].Id;
					int num3 = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.Items.FindIndex((global::Item x) => x.Id == itemId);
					if (num3 >= 0)
					{
						num++;
					}
				}
				else
				{
					num2++;
				}
			}
		}
		return num2 <= global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.GetNumEmptyItemSlot();
	}

	private void TakeAll()
	{
		this.OnDisable();
		global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.SendInventoryTakeAll();
	}

	private global::System.Collections.Generic.List<global::Item> GetInventoryItems()
	{
		return global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.Items.GetRange(6, global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.BackpackCapacity);
	}

	public void Show()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, true);
		this.isShow = true;
		this.subtitleText.text = string.Empty;
		this.currentSearchPoint = (global::SearchPoint)global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.interactivePoint;
		bool requiredItem = this.currentSearchPoint.HasRequiredItem();
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.LOOTING);
		base.gameObject.SetActive(true);
		this.leftGroup.Setup(global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.Name, global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.GetIcon(), this.itemPrefab, new global::System.Action<int>(this.OnChooseInventoryItem), global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.BackpackCapacity, this.overlayAlly, new global::System.Action<global::Item>(this.ShowDescriptionInventory), false);
		string locAction = this.currentSearchPoint.GetLocAction();
		this.titleText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(locAction);
		this.titleIcon.sprite = this.currentSearchPoint.GetIconAction();
		if (this.currentSearchPoint.unitController != null)
		{
			this.rightGroup.Setup(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.currentSearchPoint.loc_name, new string[]
			{
				this.currentSearchPoint.unitController.unit.Name
			}), this.currentSearchPoint.unitController.unit.GetIcon(), this.itemPrefab, new global::System.Action<int>(this.OnChooseSearchItem), this.currentSearchPoint.items.Count, (!global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.IsMine()) ? this.overlayEnemy : this.overlayAlly, new global::System.Action<global::Item>(this.ShowDescriptionSearchPoint), requiredItem);
		}
		else
		{
			global::WarbandController myWarbandCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
			if (this.currentSearchPoint.warbandController == null)
			{
				this.rightGroup.Setup(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.currentSearchPoint.loc_name), this.titleIcon.sprite, this.itemPrefab, new global::System.Action<int>(this.OnChooseSearchItem), this.currentSearchPoint.items.Count, this.overlayNeutral, new global::System.Action<global::Item>(this.ShowDescriptionSearchPoint), requiredItem);
			}
			else if (myWarbandCtrlr == this.currentSearchPoint.warbandController)
			{
				this.rightGroup.Setup(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.currentSearchPoint.loc_name, new string[]
				{
					this.currentSearchPoint.warbandController.name
				}), global::Warband.GetIcon(this.currentSearchPoint.warbandController.WarData.Id), this.itemPrefab, new global::System.Action<int>(this.OnChooseSearchItem), this.currentSearchPoint.items.Count, this.overlayAlly, new global::System.Action<global::Item>(this.ShowDescriptionSearchPoint), requiredItem);
			}
			else
			{
				this.rightGroup.Setup(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.currentSearchPoint.loc_name, new string[]
				{
					this.currentSearchPoint.warbandController.name
				}), global::Warband.GetIcon(this.currentSearchPoint.warbandController.WarData.Id), this.itemPrefab, new global::System.Action<int>(this.OnChooseSearchItem), this.currentSearchPoint.items.Count, this.overlayEnemy, new global::System.Action<global::Item>(this.ShowDescriptionSearchPoint), requiredItem);
			}
		}
		this.Reset();
	}

	private void Close()
	{
		this.OnDisable();
		global::Inventory inventory = (global::Inventory)global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.StateMachine.GetActiveState();
		inventory.CloseInventory();
	}

	private void OnChooseInventoryItem(int index)
	{
		if (this.leftGroup.IsSwapping)
		{
			this.leftGroup.swapIndex = index;
		}
		else if (this.rightGroup.IsSwapping)
		{
			this.Swap(index, this.rightGroup.swapIndex);
			this.leftGroup.uiItems[index].SetSelected(true);
		}
		else
		{
			int emptySlot = this.currentSearchPoint.GetEmptySlot();
			global::Item unitItem = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.Items[6 + index];
			int num = this.currentSearchPoint.items.FindIndex((global::Item x) => x.Id == unitItem.Id);
			if (unitItem.IsStackable && num >= 0)
			{
				this.Swap(index, -1);
			}
			else if (emptySlot != -1)
			{
				this.Swap(index, emptySlot);
				this.leftGroup.uiItems[index].SetSelected(true);
			}
			else
			{
				this.leftGroup.swapIndex = index;
				this.leftGroup.Lock();
				this.rightGroup.uiItems[0].GetComponent<global::ToggleEffects>().SetOn();
				this.buttons[0].SetAction("cancel", "loot_cancel_swap", 4, false, null, null);
				this.buttons[0].OnAction(new global::UnityEngine.Events.UnityAction(this.Reset), false, true);
				this.buttons[1].SetAction("action", "loot_swap", 4, false, null, null);
				this.buttons[2].gameObject.SetActive(false);
			}
		}
	}

	private void OnChooseSearchItem(int index)
	{
		global::UnitSlotId unitSlotId;
		if (this.leftGroup.IsSwapping)
		{
			this.Swap(this.leftGroup.swapIndex, index);
			this.Reset();
		}
		else if (this.rightGroup.IsSwapping)
		{
			this.rightGroup.swapIndex = index;
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.unit.GetEmptyItemSlot(out unitSlotId, this.currentSearchPoint.items[index]))
		{
			this.Swap(unitSlotId - global::UnitSlotId.ITEM_1, index);
		}
		else
		{
			this.rightGroup.Lock();
			this.rightGroup.swapIndex = index;
			this.leftGroup.uiItems[0].GetComponent<global::ToggleEffects>().SetOn();
			this.buttons[0].SetAction("cancel", "loot_cancel_swap", 4, false, null, null);
			this.buttons[0].OnAction(new global::UnityEngine.Events.UnityAction(this.Reset), false, true);
			this.buttons[1].SetAction("action", "loot_swap", 4, false, null, null);
			this.buttons[2].gameObject.SetActive(false);
		}
	}

	private void Reset()
	{
		this.leftGroup.Unlock();
		this.rightGroup.Unlock();
		this.leftGroup.swapIndex = -1;
		this.rightGroup.swapIndex = -1;
		this.leftGroup.Refresh(this.GetInventoryItems(), null);
		this.rightGroup.Refresh(this.currentSearchPoint.items, this.currentSearchPoint.GetRestrictedItemIds());
		this.buttons[0].SetAction("cancel", "loot_quit", 4, false, null, null);
		this.buttons[0].OnAction(new global::UnityEngine.Events.UnityAction(this.Close), false, true);
		this.buttons[1].SetAction("action", "loot_take", 4, false, null, null);
		if (!this.currentSearchPoint.IsEmpty() && this.CanTakeAll())
		{
			this.buttons[2].SetAction("take_all", "loot_take_all", 4, false, null, null);
			this.buttons[2].OnAction(new global::UnityEngine.Events.UnityAction(this.TakeAll), false, true);
		}
		else
		{
			this.buttons[2].gameObject.SetActive(false);
		}
		this.buttons[3].gameObject.SetActive(false);
		this.rightGroup.scrollGroup.ResetSelection();
		this.rightGroup.uiItems[0].GetComponent<global::ToggleEffects>().SetOn();
	}

	private void Swap(int inventoryIndex, int searchIndex)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.SendInventoryChange(searchIndex, inventoryIndex);
		base.StartCoroutine(this.WaitToReset());
	}

	public override void OnDisable()
	{
		base.OnDisable();
		if (this.isShow)
		{
			this.isShow = false;
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.LOOTING);
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, false);
		}
		base.gameObject.SetActive(false);
	}

	private void ShowDescriptionInventory(global::Item item)
	{
		this.isLeft = true;
		if (!this.rightGroup.IsSwapping)
		{
			this.rightGroup.toggleGroup.SetAllTogglesOff();
		}
		this.ShowDescription(item);
	}

	private void ShowDescriptionSearchPoint(global::Item item)
	{
		this.isLeft = false;
		if (!this.leftGroup.IsSwapping)
		{
			this.leftGroup.toggleGroup.SetAllTogglesOff();
		}
		this.ShowDescription(item);
	}

	private void ShowDescription(global::Item item)
	{
		if (item.Id == global::ItemId.NONE)
		{
			this.centerItem.gameObject.SetActive(false);
		}
		else
		{
			this.centerItem.gameObject.SetActive(true);
			this.centerItem.Set(item, false, false, global::ItemId.NONE, false);
			this.centerItemDescription.Set(item, null);
		}
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 4))
		{
			if (this.isLeft && !this.rightGroup.IsSwapping)
			{
				this.rightGroup.uiItems[0].SetSelected(true);
			}
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 4) && !this.isLeft && !this.leftGroup.IsSwapping)
		{
			this.leftGroup.uiItems[0].SetSelected(true);
		}
	}

	private global::System.Collections.IEnumerator WaitToReset()
	{
		yield return null;
		yield return new global::UnityEngine.WaitForEndOfFrame();
		this.Reset();
		yield break;
	}

	private const global::PandoraInput.InputLayer uiLayer = global::PandoraInput.InputLayer.LOOTING;

	private static global::Item NONE_ITEM;

	public global::UnityEngine.GameObject itemPrefab;

	public global::UIInventoryItem centerItem;

	public global::UIInventoryItemDescription centerItemDescription;

	public global::UnityEngine.UI.Text titleText;

	public global::UnityEngine.UI.Text subtitleText;

	public global::UnityEngine.UI.Image titleIcon;

	public global::UIInventoryGroup leftGroup;

	public global::UIInventoryGroup rightGroup;

	public global::System.Collections.Generic.List<global::ButtonGroup> buttons;

	private global::SearchPoint currentSearchPoint;

	private bool isShow;

	public global::UnityEngine.Sprite overlayEnemy;

	public global::UnityEngine.Sprite overlayAlly;

	public global::UnityEngine.Sprite overlayNeutral;

	private bool isLeft = true;

	private bool mustReset;
}
