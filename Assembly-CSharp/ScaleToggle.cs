using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScaleToggle : global::UnityEngine.MonoBehaviour
{
	protected global::UnityEngine.Transform Transform
	{
		get
		{
			if (this.targetTransform == null)
			{
				this.targetTransform = this.target.transform;
			}
			return this.targetTransform;
		}
	}

	private void Awake()
	{
		if (this.target == null)
		{
			this.target = base.GetComponent<global::UnityEngine.UI.Toggle>();
			this.target.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<bool>(this.OnValueChanged));
		}
	}

	private void OnEnable()
	{
		this.OnValueChanged(false);
	}

	private void OnValueChanged(bool isOn)
	{
		this.Scale((!isOn) ? this.scale.normalScale : this.scale.highlightedScale);
	}

	private void Scale(global::UnityEngine.Vector2 newScale)
	{
		global::UnityEngine.Vector3 endValue = new global::UnityEngine.Vector3(newScale.x, newScale.y, this.Transform.localScale.z);
		if (global::UnityEngine.Application.isPlaying)
		{
			this.Transform.DOScale(endValue, this.scale.duration);
		}
	}

	public global::UnityEngine.UI.Toggle target;

	public global::UnityEngine.Transform targetTransform;

	public global::UnityEngine.UI.ScaleBlock scale = global::UnityEngine.UI.ScaleBlock.defaultScaleBlock;
}
