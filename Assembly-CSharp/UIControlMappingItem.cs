using System;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class UIControlMappingItem : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.mappingButtons.Length; i++)
		{
			int btnId = i;
			this.mappingButtons[i].onAction.AddListener(delegate()
			{
				this.OnMappingButtonClicked(btnId);
			});
		}
		this.actionElement = new global::Rewired.ActionElementMap[this.mappingButtons.Length];
	}

	public void SetMapping(global::Rewired.ActionElementMap mapping, int index, bool remappable = true)
	{
		if (index < this.mappingButtons.Length)
		{
			this.actionElement[index] = mapping;
			if (mapping != null)
			{
				string text = mapping.elementIdentifierName;
				text = "key_" + text.ToLowerInvariant().Replace(" ", "_");
				if (text.Equals("key_mouse_horizontal") || text.Equals("key_mouse_vertical"))
				{
					text = "key_mouse_move";
				}
				else if (index == 1)
				{
					text = text.Replace("_+", string.Empty);
					text = text.Replace("_-", string.Empty);
				}
				string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(text);
				this.mappingButtons[index].GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = stringById;
			}
			else if (!remappable)
			{
				this.mappingButtons[index].GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_action_name_not_available");
			}
			else
			{
				this.mappingButtons[index].GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = string.Empty;
			}
			if (!remappable)
			{
				if (this.mappingButtons[index] != null)
				{
					this.mappingButtons[index].onAction.RemoveAllListeners();
					this.mappingButtons[index].color.highlightedColor = this.mappingButtons[index].color.normalColor;
				}
				this.mappingButtons[index].GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].color = this.disabledColor;
			}
		}
	}

	public void SetNav(global::UIControlMappingItem up, global::UIControlMappingItem down)
	{
		for (int i = 0; i < this.mappingButtons.Length; i++)
		{
			global::UnityEngine.UI.Navigation navigation = this.mappingButtons[i].toggle.navigation;
			navigation.selectOnUp = up.mappingButtons[i].toggle;
			navigation.selectOnDown = down.mappingButtons[i].toggle;
			this.mappingButtons[i].toggle.navigation = navigation;
		}
	}

	public void OnMappingButtonClicked(int buttonIndex)
	{
		this.OnMappingButton(this, buttonIndex, this.actionElement[buttonIndex]);
	}

	public global::ToggleEffects[] mappingButtons;

	public global::UnityEngine.UI.Text actionLabel;

	public int actionId;

	public bool isPositiveInput = true;

	public string inputCategory;

	private global::Rewired.ActionElementMap[] actionElement;

	public global::System.Action<global::UIControlMappingItem, int, global::Rewired.ActionElementMap> OnMappingButton;

	public global::UnityEngine.Color disabledColor;
}
