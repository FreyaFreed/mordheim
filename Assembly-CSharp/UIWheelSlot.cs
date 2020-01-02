using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWheelSlot : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.slot = base.GetComponent<global::ToggleEffects>();
	}

	public void SetLeftSelectable(global::ToggleEffects left)
	{
		if (this.defaultLeftItem == null)
		{
			this.defaultLeftItem = left;
		}
		else
		{
			this.altLeftItem = left;
		}
	}

	public void RefreshNavigation()
	{
		global::UnityEngine.UI.Navigation navigation = this.slot.toggle.navigation;
		navigation.selectOnUp = this.SetSelectable(this.defaultUpItem, this.altUpItem);
		navigation.selectOnDown = this.SetSelectable(this.defaultDownItem, this.altDownItem);
		navigation.selectOnRight = this.SetSelectable(this.defaultRightItem, this.altRightItem);
		navigation.selectOnLeft = this.SetSelectable(this.defaultLeftItem, this.altLeftItem);
		this.slot.toggle.navigation = navigation;
	}

	private global::UnityEngine.UI.Selectable SetSelectable(global::ToggleEffects defaultItem, global::ToggleEffects altItem)
	{
		if (defaultItem != null && defaultItem.isActiveAndEnabled && defaultItem.toggle.interactable)
		{
			return defaultItem.toggle;
		}
		if (altItem != null && altItem.isActiveAndEnabled && altItem.toggle.interactable)
		{
			return altItem.toggle;
		}
		return null;
	}

	[global::UnityEngine.HideInInspector]
	public global::ToggleEffects slot;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image subIcon;

	public global::ToggleEffects defaultUpItem;

	public global::ToggleEffects defaultDownItem;

	public global::ToggleEffects defaultRightItem;

	public global::ToggleEffects defaultLeftItem;

	public global::ToggleEffects altUpItem;

	public global::ToggleEffects altDownItem;

	public global::ToggleEffects altRightItem;

	public global::ToggleEffects altLeftItem;
}
