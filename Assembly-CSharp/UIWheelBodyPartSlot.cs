using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWheelBodyPartSlot : global::UIWheelSlot
{
	protected void Start()
	{
		this.idNum = base.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0];
		this.idNum.gameObject.SetActive(false);
	}

	public void SetLocked(bool locked)
	{
		if (locked)
		{
			this.icon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item/none", true);
			this.slot.toggle.interactable = false;
		}
		else
		{
			this.icon.sprite = this.bodyPartSprite;
			this.slot.toggle.interactable = true;
		}
	}

	public bool IsLocked()
	{
		return !this.slot.toggle.interactable;
	}

	public global::BodyPartId bodyPart;

	public global::UnityEngine.Sprite bodyPartSprite;

	public global::UnityEngine.UI.Text idNum;
}
