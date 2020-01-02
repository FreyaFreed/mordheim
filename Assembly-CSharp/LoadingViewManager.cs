using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadingViewManager : global::UnityEngine.MonoBehaviour
{
	protected virtual void Awake()
	{
		this.canvas = base.gameObject.GetComponentsInChildren<global::UnityEngine.Canvas>(true)[0];
		this.audioSrc = base.GetComponent<global::UnityEngine.AudioSource>();
		this.continueButton.SetAction("load_confirm", "menu_continue", 1000, false, null, null);
		this.continueButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnActionDone), false, true);
		this.continueButton.gameObject.SetActive(false);
		this.textWaitingForPlayer.enabled = false;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TRANSITION_WAIT_FOR_ACTION, new global::DelReceiveNotice(this.OnLoadingDone));
		global::LoadingView[] componentsInChildren = base.GetComponentsInChildren<global::LoadingView>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.views.Add(componentsInChildren[i].id, componentsInChildren[i]);
		}
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		global::PandoraSingleton<global::TransitionManager>.Instance.RequestLoadingContent(this);
	}

	public void SetContent(global::SceneLoadingTypeId loadType, bool waitForPlayers)
	{
		this.currentLoadType = loadType;
		this.views[loadType].Show();
		this.showWaitingMessage = waitForPlayers;
	}

	private void OnActionDone()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.TRANSITION_ACTION);
		if (this.showWaitingMessage)
		{
			this.textWaitingForPlayer.enabled = true;
			this.continueButton.gameObject.SetActive(false);
		}
		global::PandoraSingleton<global::Pan>.Instance.SoundsOn();
	}

	private void OnLoadingDone()
	{
		this.canvas.sortingOrder = this.canvas.sortingOrder;
		if (this.continueButton != null)
		{
			this.continueButton.RefreshImage();
			this.continueButton.gameObject.SetActive(true);
			if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1)
			{
				this.continueButton.SetSelected(true);
			}
		}
		if (this.loadingText != null)
		{
			global::UnityEngine.UI.Graphic[] componentsInChildren = this.loadingText.GetComponentsInChildren<global::UnityEngine.UI.Graphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CrossFadeAlpha(0f, 1f, true);
			}
		}
	}

	private void Update()
	{
		if (this.continueButton != null && this.continueButton.gameObject.activeInHierarchy)
		{
			global::UnityEngine.Color color = this.continueButton.label.color;
			color.a = 0.2f + global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.Sin(global::UnityEngine.Time.time));
			this.continueButton.label.color = color;
			if (global::UnityEngine.EventSystems.EventSystem.current != null && this.continueButton != null && global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != this.continueButton && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1)
			{
				this.continueButton.SetSelected(true);
			}
		}
		else if (this.textWaitingForPlayer != null && this.textWaitingForPlayer.enabled)
		{
			global::UnityEngine.Color color2 = this.textWaitingForPlayer.color;
			color2.a = 0.2f + global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.Sin(global::UnityEngine.Time.time));
			this.textWaitingForPlayer.color = color2;
		}
		if (global::PandoraSingleton<global::MissionLoader>.Exists())
		{
			if (!this.loadingProgress.enabled)
			{
				this.loadingProgress.enabled = true;
			}
			this.loadingProgress.text = global::Constant.ToString(global::PandoraSingleton<global::MissionLoader>.Instance.percent) + "%";
		}
		else if (this.loadingProgress.enabled)
		{
			this.loadingProgress.enabled = false;
		}
	}

	public void OnTransitionDone()
	{
		this.views[this.currentLoadType].PlayDialog();
	}

	private void OnDestroy()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.TRANSITION_WAIT_FOR_ACTION, new global::DelReceiveNotice(this.OnLoadingDone));
	}

	public global::UnityEngine.GameObject loadingText;

	public global::ButtonGroup continueButton;

	public global::UnityEngine.UI.Text textWaitingForPlayer;

	private global::System.Collections.Generic.Dictionary<global::SceneLoadingTypeId, global::LoadingView> views = new global::System.Collections.Generic.Dictionary<global::SceneLoadingTypeId, global::LoadingView>(5);

	private global::UnityEngine.Canvas canvas;

	private global::UnityEngine.AudioSource audioSrc;

	private bool showWaitingMessage;

	public global::UnityEngine.UI.Text loadingProgress;

	private global::SceneLoadingTypeId currentLoadType;
}
