using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Toggle))]
public class ColorToggle : global::UnityEngine.MonoBehaviour
{
	protected global::UnityEngine.Transform Transform
	{
		get
		{
			if (this.cachedTransform == null)
			{
				this.cachedTransform = base.transform;
			}
			return this.cachedTransform;
		}
	}

	private void Awake()
	{
		if (this.target == null)
		{
			this.target = base.GetComponent<global::UnityEngine.UI.Toggle>();
			this.target.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<bool>(this.OnValueChanged));
		}
		if (this.targetGraphic == null)
		{
			this.targetGraphic = base.GetComponent<global::UnityEngine.UI.Graphic>();
		}
	}

	private void OnEnable()
	{
		this.OnValueChanged(false);
	}

	private void OnValueChanged(bool isOn)
	{
		this.Color((!isOn) ? this.color.normalColor : this.color.highlightedColor);
	}

	private void Color(global::UnityEngine.Color newColor)
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			if (this.targetGraphic == null)
			{
				this.target.targetGraphic.CrossFadeColor(newColor, this.color.fadeDuration, true, true);
			}
			else
			{
				this.targetGraphic.CrossFadeColor(newColor, this.color.fadeDuration, true, true);
			}
		}
	}

	public global::UnityEngine.UI.Toggle target;

	public global::UnityEngine.UI.Graphic targetGraphic;

	private global::UnityEngine.Transform cachedTransform;

	public global::UnityEngine.UI.ColorBlock color = global::UnityEngine.UI.MordheimColorBlock.defaultColorBlock;
}
