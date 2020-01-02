using System;
using UnityEngine;
using UnityEngine.UI;

public class UIChatLogItem : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.RectTransform RctTransform { get; private set; }

	public void Init(string content)
	{
		this.layoutElem = base.GetComponent<global::UnityEngine.UI.LayoutElement>();
		this.text = base.GetComponent<global::UnityEngine.UI.Text>();
		this.RctTransform = base.GetComponent<global::UnityEngine.RectTransform>();
		this.text.text = content;
	}

	public void OnInsideMask()
	{
		if (!this.text.enabled)
		{
			this.text.enabled = true;
		}
	}

	public void OnOutsideMask()
	{
		if (this.text.enabled && this.RctTransform.sizeDelta.y != 0f)
		{
			this.layoutElem.preferredWidth = this.RctTransform.sizeDelta.x;
			this.layoutElem.preferredHeight = this.RctTransform.sizeDelta.y;
			this.text.enabled = false;
		}
	}

	private global::UnityEngine.UI.LayoutElement layoutElem;

	private global::UnityEngine.UI.Text text;
}
