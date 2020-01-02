using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryItemRuneList : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.scrollGroup = base.GetComponent<global::ScrollGroup>();
		if (this.naRuneText != null)
		{
			this.naRuneText.gameObject.SetActive(false);
		}
	}

	public void Clear()
	{
		this.items.Clear();
		this.runes.Clear();
		this.scrollGroup.ClearList();
	}

	public void SetList(global::System.Collections.Generic.List<global::Item> items, global::UnityEngine.Events.UnityAction<global::Item> slotConfirmed, global::UnityEngine.Events.UnityAction<global::Item> slotSelected, bool addEmpty, bool displayPrice, bool buy, bool flagSold = false, bool allowHighlight = true, string reason = null)
	{
		if (this.naRuneText != null)
		{
			if (!string.IsNullOrEmpty(reason))
			{
				this.naRuneText.gameObject.SetActive(true);
				this.naRuneText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(reason);
			}
			else
			{
				this.naRuneText.gameObject.SetActive(false);
			}
		}
		this.Clear();
		this.scrollGroup.Setup(this.itemPrefab, true);
		this.itemSlotConfirmed = slotConfirmed;
		this.itemSlotSelected = slotSelected;
		if (addEmpty)
		{
			this.AddItemToList(new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL), displayPrice, buy, flagSold, allowHighlight);
		}
		for (int i = 0; i < items.Count; i++)
		{
			this.AddItemToList(items[i], displayPrice, buy, flagSold, allowHighlight);
		}
		this.scrollGroup.RepositionScrollListOnNextFrame();
	}

	private void AddItemToList(global::Item item, bool displayPrice, bool buy, bool flagSold = false, bool allowHighlight = true)
	{
		global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
		global::ToggleEffects component = gameObject.GetComponent<global::ToggleEffects>();
		int idx = this.items.Count;
		component.toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			if (isOn && this.itemSlotSelected != null)
			{
				this.itemSlotSelected(item);
			}
		});
		component.onAction.AddListener(delegate()
		{
			this.ItemConfirmed(idx);
		});
		if (!allowHighlight)
		{
			component.toggleOnSelect = false;
			component.toggleOnOver = false;
			component.highlightOnSelect = false;
			component.highlightOnOver = false;
		}
		global::UIInventoryItem component2 = gameObject.GetComponent<global::UIInventoryItem>();
		component2.Set(item, displayPrice, buy, global::ItemId.NONE, flagSold);
		this.items.Add(item);
	}

	public void RemoveItem(global::Item item)
	{
		for (int i = 0; i < this.scrollGroup.items.Count; i++)
		{
			global::UIInventoryItem component = this.scrollGroup.items[i].GetComponent<global::UIInventoryItem>();
			if (component.item == item)
			{
				if (!component.UpdateQuantity())
				{
					this.scrollGroup.RemoveItemAt(i);
				}
				break;
			}
		}
	}

	private void ItemConfirmed(int idx)
	{
		if (this.itemSlotConfirmed != null)
		{
			this.itemSlotConfirmed(this.items[idx]);
		}
	}

	public void SetList(global::System.Collections.Generic.List<global::RuneMark> availableRuneList, global::System.Collections.Generic.List<global::RuneMark> notAvailableRuneList, global::UnityEngine.Events.UnityAction<global::RuneMark> slotConfirmed, global::UnityEngine.Events.UnityAction<global::RuneMark> slotSelected, string reason = null)
	{
		if (this.naRuneText != null)
		{
			if (!string.IsNullOrEmpty(reason))
			{
				this.naRuneText.gameObject.SetActive(true);
				this.naRuneText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(reason);
			}
			else
			{
				this.naRuneText.gameObject.SetActive(false);
			}
		}
		this.Clear();
		this.scrollGroup.Setup(this.runePrefab, true);
		this.runeSlotConfirmed = slotConfirmed;
		this.runeSlotSelected = slotSelected;
		for (int i = 0; i < availableRuneList.Count; i++)
		{
			this.AddRuneToList(availableRuneList[i], true);
		}
		for (int j = 0; j < notAvailableRuneList.Count; j++)
		{
			this.AddRuneToList(notAvailableRuneList[j], false);
		}
		this.scrollGroup.RepositionScrollListOnNextFrame();
	}

	private void AddRuneToList(global::RuneMark rune, bool hasRecipe)
	{
		global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
		global::ToggleEffects component = gameObject.GetComponent<global::ToggleEffects>();
		int idx = this.runes.Count;
		component.toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			if (isOn)
			{
				this.runeSlotSelected(rune);
			}
		});
		if (hasRecipe)
		{
			component.onAction.AddListener(delegate()
			{
				this.RuneConfirmed(idx);
			});
		}
		global::UIInventoryRune component2 = gameObject.GetComponent<global::UIInventoryRune>();
		component2.Set(rune);
		this.runes.Add(rune);
	}

	private void RuneConfirmed(int idx)
	{
		this.runeSlotConfirmed(this.runes[idx]);
	}

	public void SetFocus()
	{
		if (this.scrollGroup != null && this.scrollGroup.items.Count > 0)
		{
			this.scrollGroup.items[0].SetSelected(true);
		}
	}

	public global::UnityEngine.GameObject itemPrefab;

	private global::System.Collections.Generic.List<global::Item> items = new global::System.Collections.Generic.List<global::Item>();

	private global::UnityEngine.Events.UnityAction<global::Item> itemSlotConfirmed;

	private global::UnityEngine.Events.UnityAction<global::Item> itemSlotSelected;

	public global::UnityEngine.GameObject runePrefab;

	private global::System.Collections.Generic.List<global::RuneMark> runes = new global::System.Collections.Generic.List<global::RuneMark>();

	private global::UnityEngine.Events.UnityAction<global::RuneMark> runeSlotConfirmed;

	private global::UnityEngine.Events.UnityAction<global::RuneMark> runeSlotSelected;

	public global::UnityEngine.UI.Text naRuneText;

	public global::ScrollGroup scrollGroup;
}
