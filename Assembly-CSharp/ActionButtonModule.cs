using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButtonModule : global::UIModule
{
	public void Refresh(string title, string desc, string buttonTextTag, global::UnityEngine.Events.UnityAction onAction)
	{
		this.title.gameObject.SetActive(!string.IsNullOrEmpty(title));
		this.title.text = title;
		this.desc.gameObject.SetActive(!string.IsNullOrEmpty(desc));
		this.desc.text = desc;
		this.btnAction.SetAction(string.Empty, buttonTextTag, 0, false, null, null);
		this.btnAction.OnAction(onAction, false, true);
	}

	public void SetFocus()
	{
		this.btnAction.SetSelected(true);
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text desc;

	public global::ButtonGroup btnAction;
}
