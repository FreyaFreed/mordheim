using System;
using DG.Tweening;
using UnityEngine;

public class UISlideInElement : global::CanvasGroupDisabler
{
	private void Awake()
	{
		this.isInited = false;
		this.canvas = base.GetComponentInParent<global::UnityEngine.Canvas>();
	}

	private void Init()
	{
		this.dockedPos = base.transform.position;
		this.slideOutPos = this.slideOutTarget.position;
		base.transform.position = this.slideOutPos;
		this.slideVector = (this.dockedPos - this.slideOutPos).normalized;
		this.isInited = true;
	}

	private void OnSlidingComplete()
	{
		this.slindingInTween = null;
		this.slindingOutTween = null;
	}

	public void Show()
	{
		if (!this.isInited)
		{
			this.Init();
		}
		if (!base.IsVisible)
		{
			base.transform.position = this.slideOutPos;
			this.OnEnable();
		}
		if (this.slindingOutTween != null)
		{
			global::DG.Tweening.DOTween.Kill(this.slindingOutTween.id, false);
		}
		if (this.slindingInTween == null && !global::PandoraUtils.Approximately(base.transform.position, this.dockedPos))
		{
			this.slindingInTween = base.transform.DOMove(this.dockedPos, this.slideSpeed, false).SetSpeedBased(true).OnComplete(new global::DG.Tweening.TweenCallback(this.OnSlidingComplete));
		}
	}

	public void Hide()
	{
		if (!this.isInited)
		{
			this.Init();
		}
		if (this.slindingInTween != null)
		{
			global::DG.Tweening.DOTween.Kill(this.slindingInTween.id, false);
		}
		if (this.slindingOutTween == null && !global::PandoraUtils.Approximately(base.transform.position, this.slideOutPos))
		{
			this.slindingOutTween = base.transform.DOMove(this.slideOutPos, this.slideSpeed, false).SetSpeedBased(true).OnComplete(new global::DG.Tweening.TweenCallback(this.OnSlidingComplete));
		}
	}

	private global::UnityEngine.Vector3 dockedPos;

	private global::UnityEngine.Vector3 slideOutPos;

	public global::UnityEngine.Transform slideOutTarget;

	public float slideSpeed = 500f;

	private global::UnityEngine.Vector3 slideVector;

	private bool isInited;

	private global::DG.Tweening.Tweener slindingInTween;

	private global::DG.Tweening.Tweener slindingOutTween;

	private global::DG.Tweening.Tweener slindingTween;

	private global::UnityEngine.Canvas canvas;
}
