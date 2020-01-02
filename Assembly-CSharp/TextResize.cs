using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.ExecuteInEditMode]
public class TextResize : global::UnityEngine.EventSystems.UIBehaviour
{
	private new void Start()
	{
		this._transform = (base.transform as global::UnityEngine.RectTransform);
	}

	public void AdjustSize()
	{
		this._transform.sizeDelta = new global::UnityEngine.Vector2(this.element.preferredWidth, this.element.preferredHeight);
	}

	private global::UnityEngine.RectTransform _transform;

	public global::UnityEngine.UI.ILayoutElement element;

	private bool isResize;
}
