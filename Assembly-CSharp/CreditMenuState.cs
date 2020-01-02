using System;
using DG.Tweening;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class CreditMenuState : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override void Awake()
	{
		base.Awake();
		this.canvas = base.GetComponentInParent<global::UnityEngine.Canvas>();
		this.canvasRect = (global::UnityEngine.RectTransform)this.canvas.transform;
	}

	public override void StateEnter()
	{
		this.butExit.SetAction("cancel", "menu_back", 0, false, this.icnBack, null);
		this.butExit.OnAction(new global::UnityEngine.Events.UnityAction(this.OnInputCancel), false, true);
		this.thanksForPlaying.alpha = 0f;
		base.Show(true);
		base.StateMachine.camManager.dummyCam.transform.position = this.camPos.transform.position;
		base.StateMachine.camManager.dummyCam.transform.rotation = this.camPos.transform.rotation;
		base.StateMachine.camManager.Transition(2f, true);
		if (this.firstTime)
		{
			this.firstTime = false;
			this.translatingScript = this.creditText.gameObject.GetComponentsInChildren<global::Prometheus.Translate>(true)[0];
			this.translatingScript.smooth = false;
			this.oPos = this.creditText.localPosition;
		}
		else
		{
			this.translatingScript.enabled = true;
			this.creditText.localPosition = this.oPos;
		}
		this.canvasRect.GetWorldCorners(this.rectCorners);
		this.canvasTop = this.rectCorners[1].y;
	}

	public override void OnInputCancel()
	{
		base.StateMachine.GoToPrev();
	}

	public override void StateExit()
	{
		this.butExit.gameObject.SetActive(false);
		base.Show(false);
	}

	public override int StateId
	{
		get
		{
			return 4;
		}
	}

	public override void StateUpdate()
	{
		base.StateUpdate();
		((global::UnityEngine.RectTransform)this.creditText).GetWorldCorners(this.rectCorners);
		float y = this.rectCorners[0].y;
		if (this.translatingScript.enabled && y > this.canvasTop)
		{
			this.translatingScript.enabled = false;
			this.thanksForPlaying.DOFade(1f, 1f);
		}
	}

	public global::ButtonGroup butExit;

	public global::UnityEngine.Sprite icnBack;

	public global::UnityEngine.Transform creditText;

	private global::UnityEngine.RectTransform canvasRect;

	public global::UnityEngine.CanvasGroup thanksForPlaying;

	private global::Prometheus.Translate translatingScript;

	public global::UnityEngine.Vector3 oPos;

	private bool firstTime = true;

	public global::UnityEngine.GameObject camPos;

	private global::UnityEngine.Canvas canvas;

	private float canvasTop;

	private global::UnityEngine.Vector3[] rectCorners = new global::UnityEngine.Vector3[4];
}
