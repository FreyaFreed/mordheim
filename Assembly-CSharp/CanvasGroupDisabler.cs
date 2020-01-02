using System;
using UnityEngine;

public class CanvasGroupDisabler : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.CanvasGroup CanvasGroup
	{
		get
		{
			if (this.canvasGroup == null && !this.isInit)
			{
				this.isInit = true;
				this.canvasGroup = base.GetComponent<global::UnityEngine.CanvasGroup>();
			}
			return this.canvasGroup;
		}
		set
		{
			this.canvasGroup = value;
		}
	}

	public bool IsVisible
	{
		get
		{
			return this.CanvasGroup != null && global::UnityEngine.Mathf.Approximately(this.CanvasGroup.alpha, 1f) && this.CanvasGroup.interactable;
		}
	}

	public virtual void OnEnable()
	{
		if (this.CanvasGroup != null)
		{
			this.CanvasGroup.alpha = 1f;
			this.CanvasGroup.interactable = true;
			this.CanvasGroup.blocksRaycasts = true;
		}
	}

	public virtual void OnDisable()
	{
		if (this.CanvasGroup != null)
		{
			this.CanvasGroup.alpha = 0f;
			this.CanvasGroup.interactable = false;
			this.CanvasGroup.blocksRaycasts = false;
		}
	}

	private bool isInit;

	private global::UnityEngine.CanvasGroup canvasGroup;
}
