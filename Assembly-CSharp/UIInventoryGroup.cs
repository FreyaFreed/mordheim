using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryGroup : global::CanvasGroupDisabler
{
	public bool IsSwapping
	{
		get
		{
			return this.swapIndex != -1;
		}
	}

	private void Awake()
	{
		this.hightlight = base.GetComponentInChildren<global::HightlightAnimate>();
		this.toggleGroup = base.GetComponentInParent<global::UnityEngine.UI.ToggleGroup>();
	}

	public void Setup(string name, global::UnityEngine.Sprite icon, global::UnityEngine.GameObject itemPrefab, global::System.Action<int> itemSelected, int nb, global::UnityEngine.Sprite background, global::System.Action<global::Item> showDescription, bool requiredItem = false)
	{
		this.titleBackground.sprite = background;
		this.itemSelectedCallback = itemSelected;
		this.titleText.text = name;
		this.subtitleText.text = ((!requiredItem) ? string.Empty : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("required_item"));
		this.titleIcon.sprite = icon;
		this.scrollGroup.ClearList();
		this.scrollGroup.Setup(itemPrefab, true);
		this.uiItems.Clear();
		for (int i = 0; i < nb; i++)
		{
			global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
			this.uiItems.Add(gameObject.GetComponent<global::UIInventoryItem>());
			int index = i;
			global::ToggleEffects component = gameObject.GetComponent<global::ToggleEffects>();
			component.onSelect.AddListener(delegate()
			{
				this.OnOverItem(index);
			});
			component.onAction.AddListener(delegate()
			{
				this.itemSelectedCallback(index);
			});
		}
		this.showDescriptionCallback = showDescription;
	}

	public void OnOverItem(int index)
	{
		this.selectedIndex = index;
		this.showDescriptionCallback(this.items[index]);
	}

	public void Refresh(global::System.Collections.Generic.List<global::Item> refItems, global::System.Collections.Generic.List<global::ItemId> restrictedItemIds = null)
	{
		this.selectedIndex = -1;
		this.items = refItems;
		for (int i = 0; i < this.items.Count; i++)
		{
			this.uiItems[i].Set(this.items[i], false, false, (restrictedItemIds == null) ? global::ItemId.NONE : restrictedItemIds[i], false);
		}
		this.toggleGroup.SetAllTogglesOff();
		this.hightlight.Deactivate();
	}

	public void Lock()
	{
		global::UnityEngine.CanvasGroup canvasGroup = base.CanvasGroup;
		bool flag = false;
		base.CanvasGroup.interactable = flag;
		canvasGroup.blocksRaycasts = flag;
	}

	public void Unlock()
	{
		global::UnityEngine.CanvasGroup canvasGroup = base.CanvasGroup;
		bool flag = true;
		base.CanvasGroup.interactable = flag;
		canvasGroup.blocksRaycasts = flag;
	}

	public global::UnityEngine.UI.Image titleIcon;

	public global::UnityEngine.UI.Image titleBackground;

	public global::UnityEngine.UI.Text titleText;

	public global::UnityEngine.UI.Text subtitleText;

	public global::ScrollGroup scrollGroup;

	public readonly global::System.Collections.Generic.List<global::UIInventoryItem> uiItems = new global::System.Collections.Generic.List<global::UIInventoryItem>();

	private global::System.Collections.Generic.List<global::Item> items = new global::System.Collections.Generic.List<global::Item>();

	public int selectedIndex = -1;

	public int swapIndex = -1;

	private global::System.Action<int> itemSelectedCallback;

	private global::System.Action<global::Item> showDescriptionCallback;

	private global::HightlightAnimate hightlight;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;
}
