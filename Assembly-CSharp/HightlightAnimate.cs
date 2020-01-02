using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class HightlightAnimate : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.RectTransform cachedTransform
	{
		get
		{
			if (this._cachedTransform == null)
			{
				this._cachedTransform = base.GetComponent<global::UnityEngine.RectTransform>();
			}
			return this._cachedTransform;
		}
	}

	private void Awake()
	{
		base.GetComponentsInChildren<global::UnityEngine.UI.Graphic>(true, this.graphics);
	}

	private void Start()
	{
		this.Deactivate();
	}

	public void Highlight(global::UnityEngine.RectTransform targetTransform)
	{
		if (this.current != null)
		{
			this.current.Kill(false);
		}
		if (this.currentCoroutine != null)
		{
			base.StopCoroutine(this.currentCoroutine);
		}
		this.currentCoroutine = base.StartCoroutine(this.OnHighlight(targetTransform));
	}

	private global::System.Collections.IEnumerator OnHighlight(global::UnityEngine.RectTransform targetTransform)
	{
		yield return null;
		if (targetTransform != null)
		{
			if (targetTransform != this.currentTarget || !this.isActivated)
			{
				this.currentTarget = targetTransform;
				global::UnityEngine.Vector2 startSize = targetTransform.sizeDelta;
				startSize.x = 0f;
				this.cachedTransform.sizeDelta = startSize;
				this.cachedTransform.position = targetTransform.position;
				this.current = global::DG.Tweening.DOTween.To(() => this.cachedTransform.sizeDelta, delegate(global::UnityEngine.Vector2 value)
				{
					this.cachedTransform.sizeDelta = value;
				}, targetTransform.sizeDelta, this.duration).SetEase(this.ease);
				this.Activate();
			}
		}
		else
		{
			this.Deactivate();
		}
		yield break;
	}

	private void Update()
	{
		if (this.isActivated && (this.currentTarget == null || !this.currentTarget.gameObject.activeInHierarchy))
		{
			this.Deactivate();
		}
	}

	public void Activate()
	{
		for (int i = 0; i < this.graphics.Count; i++)
		{
			this.graphics[i].enabled = true;
		}
		this.isActivated = true;
	}

	public void Deactivate()
	{
		for (int i = 0; i < this.graphics.Count; i++)
		{
			this.graphics[i].enabled = false;
		}
		this.isActivated = false;
	}

	private global::UnityEngine.RectTransform target;

	private bool isActivated;

	public float duration = 1f;

	public global::DG.Tweening.Ease ease = global::DG.Tweening.Ease.InOutQuad;

	private global::UnityEngine.RectTransform _cachedTransform;

	private global::UnityEngine.Vector2 toSize;

	private global::UnityEngine.Vector3 toPosition;

	private global::DG.Tweening.Tweener current;

	private readonly global::System.Collections.Generic.List<global::UnityEngine.UI.Graphic> graphics = new global::System.Collections.Generic.List<global::UnityEngine.UI.Graphic>();

	private global::UnityEngine.Coroutine currentCoroutine;

	private global::UnityEngine.RectTransform currentTarget;
}
