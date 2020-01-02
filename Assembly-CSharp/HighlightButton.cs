using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightButton : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IDeselectHandler
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
		if (this.target == null)
		{
			this.target = base.GetComponent<global::UnityEngine.UI.Graphic>();
		}
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.hideOnSelect)
		{
			this.hightlight.Deactivate();
		}
		else
		{
			this.hightlight.Highlight(this.cachedTransform);
		}
	}

	public void OnDeselect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.hideOnExit)
		{
			this.hightlight.Deactivate();
		}
	}

	public global::UnityEngine.UI.Graphic target;

	private global::UnityEngine.RectTransform _cachedTransform;

	public global::HightlightAnimate hightlight;

	public bool hideOnExit;

	public bool hideOnSelect;
}
