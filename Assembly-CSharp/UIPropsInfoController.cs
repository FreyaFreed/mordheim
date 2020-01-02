using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPropsInfoController : global::CanvasGroupDisabler
{
	public void Set(global::UnityEngine.Sprite icon, string label)
	{
		if (this.icon.sprite != icon)
		{
			this.icon.sprite = icon;
		}
		if (this.label.text != label)
		{
			this.label.text = label;
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text label;
}
