using System;
using UnityEngine;
using UnityEngine.UI;

public class UIIconDesc : global::UnityEngine.MonoBehaviour
{
	public void Set(global::UnityEngine.Sprite image, string descKey)
	{
		this.SetLocalized(image, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(descKey));
	}

	public void SetLocalized(global::UnityEngine.Sprite image, string desc)
	{
		base.gameObject.SetActive(true);
		this.icon.sprite = image;
		this.desc.text = desc;
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text desc;
}
