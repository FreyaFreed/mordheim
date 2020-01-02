using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDescription : global::UnityEngine.MonoBehaviour
{
	public void Set(string titleKey, string descKey)
	{
		this.Set(titleKey, descKey, string.Empty);
	}

	public void Set(string titleKey, string descKey, string keySubtitle)
	{
		this.SetLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleKey), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(descKey));
	}

	public void SetLocalized(string title, string desc)
	{
		this.SetLocalized(title, desc, string.Empty);
	}

	public void SetLocalized(string title, string desc, string subtitle)
	{
		base.gameObject.SetActive(true);
		this.title.text = title;
		this.desc.text = desc;
		if (this.subtitle != null)
		{
			this.subtitle.text = subtitle;
		}
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text subtitle;

	public global::UnityEngine.UI.Text desc;
}
