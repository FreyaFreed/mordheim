using System;
using UnityEngine.Events;

public class ContinuePopupView : global::ConfirmationPopupView
{
	public override void Init()
	{
		base.Init();
		if (this.abandonButton != null)
		{
			this.abandonButton.SetAction(string.Empty, "abandon", 1, false, null, null);
			this.abandonButton.OnAction(new global::UnityEngine.Events.UnityAction(this.Abandon), false, true);
		}
	}

	public void Show(string titleId, string textId, global::System.Action<bool> callback)
	{
		base.Show(titleId, textId, null, false, false);
		this.btnCallback = callback;
	}

	public override void Confirm()
	{
		base.Confirm();
		if (this.btnCallback != null)
		{
			this.btnCallback(true);
		}
	}

	public void Abandon()
	{
		base.Confirm();
		if (this.btnCallback != null)
		{
			this.btnCallback(false);
		}
	}

	public global::ButtonGroup abandonButton;

	protected global::System.Action<bool> btnCallback;
}
