using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemAttribute : global::UnityEngine.MonoBehaviour
{
	public void Set(string attributeValue)
	{
		base.gameObject.SetActive(true);
		this.value.text = attributeValue;
		if (this.improveIcon != null)
		{
			this.improveIcon.enabled = false;
		}
		if (this.difference != null)
		{
			this.difference.enabled = false;
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text value;

	public global::UnityEngine.UI.Image improveIcon;

	public global::UnityEngine.UI.Text difference;
}
