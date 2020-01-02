using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class FlyingText : global::UnityEngine.MonoBehaviour
{
	public bool Done { get; protected set; }

	private void Awake()
	{
		this.Done = false;
	}

	private void OnDestroy()
	{
		this.Destroy();
	}

	public virtual void Destroy()
	{
		global::DG.Tweening.DOTween.Kill(this, false);
	}

	public void Play(global::UnityEngine.Vector3 position, global::UnityEngine.Transform anchorTr = null)
	{
		this.moveOffset = global::UnityEngine.Vector3.zero;
		this.startPosition = position;
		this.anchor = anchorTr;
		this.Done = false;
		if (this.duration > 0f)
		{
			global::DG.Tweening.TweenParams tweenParams = new global::DG.Tweening.TweenParams();
			tweenParams.OnComplete(new global::DG.Tweening.TweenCallback(this.Deactivate));
			global::DG.Tweening.DOTween.To(() => this.moveOffset, delegate(global::UnityEngine.Vector3 pos)
			{
				this.moveOffset = pos;
			}, this.movement, this.duration).SetAs(tweenParams);
		}
	}

	protected virtual void LateUpdate()
	{
		if (!this.Done)
		{
			global::UnityEngine.Vector3 localPosition = global::UnityEngine.Camera.main.WorldToScreenPoint(((!(this.anchor != null)) ? global::UnityEngine.Vector3.zero : this.anchor.position) + this.startPosition + this.startOffset + this.moveOffset);
			localPosition.z = 0f;
			if (this.clamped)
			{
				float num = (base.transform as global::UnityEngine.RectTransform).rect.width / 2f;
				float num2 = (base.transform as global::UnityEngine.RectTransform).rect.height / 2f;
				localPosition.x = global::UnityEngine.Mathf.Clamp(localPosition.x, global::PandoraSingleton<global::FlyingTextManager>.Instance.canvasCorners[0].x + num, global::PandoraSingleton<global::FlyingTextManager>.Instance.canvasCorners[2].x - num);
				localPosition.y = global::UnityEngine.Mathf.Clamp(localPosition.y, global::PandoraSingleton<global::FlyingTextManager>.Instance.canvasCorners[0].y + num2, global::PandoraSingleton<global::FlyingTextManager>.Instance.canvasCorners[2].y - num2);
			}
			base.transform.localPosition = localPosition;
		}
	}

	public virtual void Deactivate()
	{
		this.Done = true;
		base.gameObject.SetActive(false);
	}

	public global::UnityEngine.Color startColor;

	public global::UnityEngine.Color endColor;

	public global::UnityEngine.Vector3 startOffset;

	public global::UnityEngine.Vector3 movement;

	public float duration;

	public float clampBorderSize = 0.05f;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Vector3 moveOffset;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Vector3 startPosition;

	public bool clamped;

	private global::UnityEngine.Transform anchor;
}
