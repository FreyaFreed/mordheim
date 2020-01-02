using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayTogetherPopupView : global::UIPopupModule
{
	public override void Init()
	{
		base.Init();
		if (this.exhibitionButton != null)
		{
			this.exhibitionButton.SetAction(null, "menu_skirmish_exhibition", 1, false, null, null);
			this.exhibitionButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnExhibition), false, true);
		}
		if (this.contestButton != null)
		{
			this.contestButton.SetAction(null, "menu_skirmish_contest", 1, false, null, null);
			this.contestButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnContest), false, true);
		}
		if (this.abandonButton != null)
		{
			this.abandonButton.SetAction(null, "menu_cancel", 1, false, null, null);
			this.abandonButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnCancel), false, true);
		}
	}

	protected virtual void Start()
	{
		if (!this.isShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnExhibition()
	{
		this.Hide();
		if (this.onExhibitionCallback != null)
		{
			this.onExhibitionCallback();
		}
	}

	private void OnContest()
	{
		this.Hide();
		if (this.onContestCallback != null)
		{
			this.onContestCallback();
		}
	}

	private void OnCancel()
	{
		this.Hide();
		if (this.onCancelCallback != null)
		{
			this.onCancelCallback();
		}
	}

	public void Show(string titleId, string textId, global::System.Action exhibitionCallback, global::System.Action contestCallback, global::System.Action cancelCallback)
	{
		this.onCancelCallback = cancelCallback;
		this.onContestCallback = contestCallback;
		this.onExhibitionCallback = exhibitionCallback;
		if (!this.isShow)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.POP_UP);
		}
		this.isShow = true;
		if (!base.initialized)
		{
			this.Init();
		}
		this.previousSelection = ((!(global::UnityEngine.EventSystems.EventSystem.current != null)) ? null : global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
		if (!string.IsNullOrEmpty(titleId))
		{
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleId);
		}
		if (!string.IsNullOrEmpty(textId))
		{
			this.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(textId);
		}
		this.contestButton.effects.toggle.isOn = false;
		this.exhibitionButton.effects.toggle.isOn = false;
		this.abandonButton.effects.toggle.isOn = true;
		this.abandonButton.SetSelected(true);
		base.gameObject.SetActive(true);
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

	private void Update()
	{
		if (this.isShow && !global::PandoraSingleton<global::GameManager>.Instance.Popup.isShow && global::UnityEngine.EventSystems.EventSystem.current != null && (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null || global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.root != base.transform.root))
		{
			this.abandonButton.SetSelected(true);
		}
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text text;

	public global::ButtonGroup exhibitionButton;

	public global::ButtonGroup contestButton;

	public global::ButtonGroup abandonButton;

	protected bool isShow;

	private global::UnityEngine.GameObject previousSelection;

	private global::System.Action onExhibitionCallback;

	private global::System.Action onContestCallback;

	private global::System.Action onCancelCallback;
}
