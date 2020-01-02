using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRetroactionResult : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.resultName.text = string.Empty;
	}

	public void Set(global::UnitController unitCtrlr)
	{
		if (!string.IsNullOrEmpty(unitCtrlr.currentActionData.actionOutcome))
		{
			base.gameObject.SetActive(true);
			this.actionIcon.enabled = false;
			this.resultName.text = unitCtrlr.currentActionData.actionOutcome;
		}
		else
		{
			this.resultName.text = string.Empty;
		}
	}

	public void Set(string effect, bool isBuff)
	{
		if (this.actionIcon != null)
		{
			this.actionIcon.enabled = true;
			this.actionIcon.rectTransform.localScale = new global::UnityEngine.Vector3(1f, (!isBuff) ? -1f : 1f, 1f);
			this.actionIcon.color = ((!isBuff) ? global::Constant.GetColor(global::ConstantId.COLOR_RED) : global::Constant.GetColor(global::ConstantId.COLOR_GREEN));
		}
		this.resultName.text = effect;
	}

	public void Set(string effect)
	{
		this.resultName.text = effect;
	}

	public void Show()
	{
		if (!string.IsNullOrEmpty(this.resultName.text))
		{
			base.gameObject.SetActive(true);
		}
		if (this.actionIcon != null)
		{
			this.actionIcon.enabled = true;
		}
	}

	public void Hide()
	{
		this.resultName.text = string.Empty;
		if (this.actionIcon != null)
		{
			this.actionIcon.enabled = false;
		}
		base.gameObject.SetActive(false);
	}

	public global::UnityEngine.UI.Image actionIcon;

	public global::UnityEngine.UI.Text resultName;

	public global::UnityEngine.RectTransform offset;
}
