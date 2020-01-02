using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WarbandRankPopupView : global::UnityEngine.MonoBehaviour
{
	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		if (!this.isShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	protected virtual void Init()
	{
		this.init = true;
		if (this.confirmButton != null)
		{
			this.confirmButton.SetAction("action", "menu_confirm", 1, false, null, null);
			this.confirmButton.OnAction(new global::UnityEngine.Events.UnityAction(this.Confirm), false, true);
		}
		if (this.cancelButton != null)
		{
			this.cancelButton.SetAction("cancel", "menu_cancel", 1, false, null, null);
			this.cancelButton.OnAction(new global::UnityEngine.Events.UnityAction(this.Cancel), false, true);
		}
	}

	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	public virtual void Show(string titleId, string textId, global::System.Action<bool, int> callback, bool hideButtons = false)
	{
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleId);
		this.Show(callback, hideButtons);
	}

	public virtual void Show(global::System.Action<bool, int> callback, bool hideButtons = false)
	{
		this.isShow = true;
		this._callback = callback;
		if (!this.init)
		{
			this.Init();
		}
		if (hideButtons)
		{
			if (this.confirmButton != null)
			{
				this.confirmButton.gameObject.SetActive(false);
			}
			if (this.cancelButton != null)
			{
				this.cancelButton.gameObject.SetActive(false);
			}
		}
		else
		{
			if (this.confirmButton != null)
			{
				this.confirmButton.gameObject.SetActive(true);
			}
			if (this.cancelButton != null)
			{
				this.cancelButton.gameObject.SetActive(true);
			}
		}
		base.gameObject.SetActive(true);
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.POP_UP);
	}

	public virtual void Hide()
	{
		if (this.isShow)
		{
			this.isShow = false;
			base.gameObject.SetActive(false);
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.POP_UP);
		}
	}

	public virtual void Confirm()
	{
		this.Hide();
		if (this._callback != null)
		{
			this._callback(true, (this.rank.CurSel != 0) ? 5 : 0);
		}
	}

	public virtual void Cancel()
	{
		this.Hide();
		if (this._callback != null)
		{
			this._callback(false, (this.rank.CurSel != 0) ? 5 : 0);
		}
	}

	public global::UnityEngine.UI.Text title;

	public global::SelectorGroup rank;

	protected global::System.Action<bool, int> _callback;

	private global::UnityEngine.Vector3 _startPosition;

	public global::ButtonGroup confirmButton;

	public global::ButtonGroup cancelButton;

	protected bool isShow;

	protected bool init;
}
