using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorTextButton : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IDeselectHandler
{
	void global::UnityEngine.EventSystems.IDeselectHandler.OnDeselect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.Color(this.color.normalColor);
	}

	private void OnEnable()
	{
		this.Color(this.color.normalColor);
	}

	private void OnDisable()
	{
		this.Color(this.color.disabledColor);
	}

	public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.Color(this.color.pressedColor);
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.Color(this.color.highlightedColor);
	}

	private void Color(global::UnityEngine.Color newColor)
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			this.targetGraphic.CrossFadeColor(newColor, this.color.fadeDuration, true, true);
		}
	}

	public global::UnityEngine.UI.Graphic targetGraphic;

	public global::UnityEngine.UI.ColorBlock color = global::UnityEngine.UI.MordheimColorBlock.defaultColorBlock;
}
