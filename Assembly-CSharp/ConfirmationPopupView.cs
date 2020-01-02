using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmationPopupView : global::UIPopupModule
{
	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected virtual void Start()
	{
		if (!this.isShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	public override void Init()
	{
		base.Init();
		global::PandoraDebug.LogDebug("ConfirmationPopupView Initialize.", "uncategorised", null);
		if (this.confirmButton != null)
		{
			this.confirmButton.SetAction(null, "menu_confirm", 1, false, null, null);
			this.confirmButton.OnAction(new global::UnityEngine.Events.UnityAction(this.Confirm), false, true);
		}
		if (this.cancelButton != null)
		{
			this.cancelButton.SetAction(null, "menu_cancel", 1, false, null, null);
			this.cancelButton.OnAction(new global::UnityEngine.Events.UnityAction(this.Cancel), false, true);
		}
	}

	public virtual void ShowLocalized(string newTitle, string newText, global::System.Action<bool> callback, bool hideButtons = false, bool hideCancel = false)
	{
		if (!string.IsNullOrEmpty(newTitle))
		{
			this.title.text = newTitle;
		}
		if (!string.IsNullOrEmpty(newText))
		{
			this.text.text = newText;
		}
		this.Show(callback, hideButtons, hideCancel);
	}

	public virtual void Show(string titleId, string textId, global::System.Action<bool> callback, bool hideButtons = false, bool hideCancel = false)
	{
		if (!string.IsNullOrEmpty(titleId))
		{
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleId);
		}
		if (!string.IsNullOrEmpty(textId))
		{
			this.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(textId);
		}
		this.Show(callback, hideButtons, hideCancel);
	}

	public virtual void Show(global::System.Action<bool> callback, bool hideButtons = false, bool hideCancel = false)
	{
		this.Hide();
		if (!this.isShow)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.POP_UP);
		}
		this.isShow = true;
		this._callback = callback;
		if (!base.initialized)
		{
			this.Init();
		}
		this.previousSelection = ((!(global::UnityEngine.EventSystems.EventSystem.current != null)) ? null : global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
		if (this.cancelButton != null && !hideCancel)
		{
			this.cancelButton.effects.toggle.isOn = true;
			if (this.confirmButton != null)
			{
				this.confirmButton.effects.toggle.isOn = false;
			}
			this.cancelButton.SetSelected(true);
		}
		else if (this.confirmButton != null)
		{
			this.confirmButton.effects.toggle.isOn = true;
			if (this.cancelButton != null)
			{
				this.cancelButton.effects.toggle.isOn = false;
			}
			this.confirmButton.SetSelected(true);
		}
		else
		{
			base.gameObject.SetSelected(true);
		}
		base.gameObject.SetActive(true);
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
			if (this.cancelButton != null)
			{
				if (hideCancel)
				{
					this.cancelButton.gameObject.SetActive(false);
				}
				else
				{
					this.cancelButton.SetAction(null, "menu_cancel", 1, false, null, null);
					this.cancelButton.gameObject.SetActive(true);
					this.cancelButton.RefreshImage();
				}
			}
			if (this.confirmButton != null)
			{
				this.confirmButton.gameObject.SetActive(true);
				if (this.cancelButton != null)
				{
					this.confirmButton.SetAction(null, "menu_confirm", 1, false, null, null);
				}
				else
				{
					this.confirmButton.SetAction(null, "menu_continue", 1, false, null, null);
					this.confirmButton.SetSelected(true);
				}
				this.confirmButton.RefreshImage();
			}
		}
	}

	public virtual void Hide()
	{
		if (this.isShow)
		{
			this.isShow = false;
			base.gameObject.SetActive(false);
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.POP_UP);
			if (this.previousSelection != null)
			{
				this.previousSelection.SetSelected(false);
			}
		}
	}

	public void HideCancelButton()
	{
		if (this.cancelButton != null)
		{
			this.cancelButton.gameObject.SetActive(false);
		}
		this.confirmButton.SetAction(null, "menu_continue", 1, false, null, null);
		this.confirmButton.SetSelected(true);
	}

	public virtual void Confirm()
	{
		global::PandoraDebug.LogDebug("ConfirmationPopupView Confirm!", "uncategorised", null);
		this.Hide();
		if (this._callback != null)
		{
			this._callback(true);
		}
	}

	public virtual void Cancel()
	{
		this.Hide();
		if (this._callback != null)
		{
			this._callback(false);
		}
	}

	public void ResetPreviousSelection()
	{
		this.previousSelection = null;
	}

	private void Update()
	{
		if (this.isShow && (this.isSystem || !global::PandoraSingleton<global::GameManager>.Instance.Popup.isShow) && global::UnityEngine.EventSystems.EventSystem.current != null && (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null || global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.root != base.transform.root))
		{
			if (this.cancelButton != null && this.cancelButton.isActiveAndEnabled)
			{
				this.cancelButton.SetSelected(true);
			}
			else if (this.confirmButton != null)
			{
				this.confirmButton.SetSelected(true);
			}
		}
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text text;

	protected global::System.Action<bool> _callback;

	private global::UnityEngine.Vector3 _startPosition;

	private global::UnityEngine.GameObject previousSelection;

	public global::ButtonGroup confirmButton;

	public global::ButtonGroup cancelButton;

	public bool isShow;

	protected bool isSystem;
}
