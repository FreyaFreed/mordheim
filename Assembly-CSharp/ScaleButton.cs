using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleButton : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IDeselectHandler
{
	void global::UnityEngine.EventSystems.IDeselectHandler.OnDeselect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.Scale(this.scale.normalScale);
	}

	protected global::UnityEngine.Transform Transform
	{
		get
		{
			if (this.target == null)
			{
				this.target = base.transform;
			}
			return this.target;
		}
	}

	private void OnEnable()
	{
		this.Scale(this.scale.normalScale);
	}

	private void OnDisable()
	{
		this.Scale(this.scale.disabledScale);
	}

	public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.Scale(this.scale.pressedScale);
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.Scale(this.scale.highlightedScale);
	}

	private void Scale(global::UnityEngine.Vector2 newScale)
	{
		global::UnityEngine.Vector3 endValue = new global::UnityEngine.Vector3(newScale.x, newScale.y, this.Transform.localScale.z);
		if (global::UnityEngine.Application.isPlaying)
		{
			this.Transform.DOScale(endValue, this.scale.duration);
		}
	}

	public global::UnityEngine.Transform target;

	public global::UnityEngine.UI.ScaleBlock scale = global::UnityEngine.UI.ScaleBlock.defaultScaleBlock;
}
