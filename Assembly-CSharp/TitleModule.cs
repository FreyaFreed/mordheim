using System;
using UnityEngine.UI;

public class TitleModule : global::UIModule
{
	public void Set(string titleKey, bool showBg = true)
	{
		this.SetLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleKey), showBg);
	}

	public void SetLocalized(string titleText, bool showBg = true)
	{
		if (this.bg.enabled != showBg)
		{
			this.bg.enabled = showBg;
		}
		if (this.title.text != titleText)
		{
			this.title.text = titleText;
		}
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Image bg;
}
