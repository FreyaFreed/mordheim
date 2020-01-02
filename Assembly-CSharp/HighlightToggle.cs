using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Toggle))]
public class HighlightToggle : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.UI.Toggle target
	{
		get
		{
			if (this._target == null)
			{
				this._target = base.GetComponent<global::UnityEngine.UI.Toggle>();
			}
			return this._target;
		}
	}

	private global::UnityEngine.RectTransform targetTransform
	{
		get
		{
			if (this._targetTransform == null)
			{
				this._targetTransform = (global::UnityEngine.RectTransform)base.transform;
			}
			return this._targetTransform;
		}
	}

	public global::HightlightAnimate hightlight
	{
		get
		{
			if (this._hightlight == null && this.findInParent && base.transform.parent != null)
			{
				this._hightlight = base.transform.parent.GetComponentsInChildren<global::HightlightAnimate>(true)[0];
			}
			return this._hightlight;
		}
	}

	private void Awake()
	{
		this.target.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<bool>(this.OnValueChanged));
	}

	public void OnValueChanged(bool isOn)
	{
		if (isOn)
		{
			if (this.hideOnSelect)
			{
				this.hightlight.Deactivate();
			}
			else
			{
				this.hightlight.Highlight(this.targetTransform);
			}
		}
		else if (this.hideOnExit)
		{
			this.hightlight.Deactivate();
		}
	}

	[global::UnityEngine.Serialization.FormerlySerializedAs("target")]
	public global::UnityEngine.UI.Toggle _target;

	public global::UnityEngine.RectTransform _targetTransform;

	[global::UnityEngine.Serialization.FormerlySerializedAs("hightlight")]
	public global::HightlightAnimate _hightlight;

	public bool hideOnExit;

	public bool hideOnSelect;

	public bool findInParent;
}
