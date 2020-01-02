using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryMutation : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Mutation mut)
	{
		this.icon.sprite = mut.GetIcon();
		if (this.title != null)
		{
			this.title.text = mut.LocName;
		}
		if (this.desc != null)
		{
			this.desc.text = mut.LocDesc;
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text desc;
}
