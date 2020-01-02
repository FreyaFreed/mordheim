using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Toggle))]
public class UIGroupEffects : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IPointerEnterHandler, global::UnityEngine.EventSystems.IPointerExitHandler
{
	public global::UnityEngine.UI.Toggle toggle
	{
		get
		{
			if (this._toggle == null)
			{
				this._toggle = base.GetComponent<global::UnityEngine.UI.Toggle>();
			}
			return this._toggle;
		}
	}

	private void Awake()
	{
		foreach (global::UnityEngine.EventSystems.UIBehaviour mono in this.selectables)
		{
			mono.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.Select, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.OnSelect));
		}
		this.toggle.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<bool>(this.OnValueChanged));
	}

	private void OnEnable()
	{
		this.OnValueChanged(this.toggle.isOn);
	}

	private void OnValueChanged(bool isOn)
	{
		if (base.isActiveAndEnabled)
		{
			this.Color(isOn);
			this.Scale(isOn);
		}
		if (this.toHighlight != null)
		{
			if (isOn)
			{
				if (this.highlight.isActiveAndEnabled)
				{
					this.highlight.Highlight(this.toHighlight);
				}
			}
			else if (this.highlight != null)
			{
				this.highlight.Deactivate();
			}
		}
	}

	private void Color(bool isOn)
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			foreach (global::ColorList colorList in this.toColor)
			{
				foreach (global::UnityEngine.UI.Graphic graphic in colorList.toScale)
				{
					graphic.CrossFadeColor((!isOn) ? colorList.color.normalColor : colorList.color.highlightedColor, colorList.color.fadeDuration, true, true);
				}
			}
		}
	}

	private void Scale(bool isOn)
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			foreach (global::ScaleList scaleList in this.toScale)
			{
				global::UnityEngine.Vector2 vector = (!isOn) ? scaleList.scale.normalScale : scaleList.scale.highlightedScale;
				global::UnityEngine.Vector3 endValue = new global::UnityEngine.Vector3(vector.x, vector.y, 1f);
				foreach (global::UnityEngine.RectTransform target in scaleList.toScale)
				{
					target.DOScale(endValue, scaleList.scale.duration);
				}
			}
		}
	}

	private void OnSelect(global::UnityEngine.EventSystems.BaseEventData arg0)
	{
		this.toggle.isOn = true;
	}

	public void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (this.toggleOnOver && !eventData.eligibleForClick && this.toggle.enabled)
		{
			this.toggle.isOn = true;
		}
	}

	public void OnPointerExit(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (this.untoggleOnExit && !eventData.eligibleForClick && this.toggle.enabled)
		{
			this.toggle.isOn = false;
		}
	}

	public void OnSubmit(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
	}

	private global::UnityEngine.UI.Toggle _toggle;

	public global::System.Collections.Generic.List<global::UnityEngine.EventSystems.UIBehaviour> selectables;

	public global::System.Collections.Generic.List<global::ColorList> toColor;

	public global::System.Collections.Generic.List<global::ScaleList> toScale;

	public bool toggleOnOver = true;

	public bool untoggleOnExit;

	public global::HightlightAnimate highlight;

	public global::UnityEngine.RectTransform toHighlight;
}
