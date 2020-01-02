using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationDescModule : global::UIModule
{
	public void Set(global::UnityEngine.Sprite icon, string titleLoc, string textLoc)
	{
		this.icon.sprite = icon;
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleLoc);
		this.description.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(textLoc);
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text description;
}
