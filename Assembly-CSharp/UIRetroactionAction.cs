using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRetroactionAction : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.unitName.text = string.Empty;
		this.actionName.text = string.Empty;
	}

	public void Set(global::UnitController unitCtrlr)
	{
		this.unitName.text = unitCtrlr.unit.Name;
		this.actionName.text = unitCtrlr.currentActionData.name;
		this.mastery.enabled = unitCtrlr.currentActionData.mastery;
		this.actionIcon.sprite = unitCtrlr.currentActionData.icon;
		if (this.actionIcon.sprite == null)
		{
			this.actionIcon.sprite = unitCtrlr.unit.GetIcon();
		}
		this.result.gameObject.SetActive(false);
	}

	public global::UnityEngine.UI.Text unitName;

	public global::UnityEngine.UI.Text actionName;

	public global::UnityEngine.UI.Image actionIcon;

	public global::UnityEngine.UI.Image mastery;

	public global::UnityEngine.RectTransform offset;

	public global::UIRetroactionResult result;
}
