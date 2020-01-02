using System;
using UnityEngine;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Image))]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.LayoutElement))]
[global::UnityEngine.ExecuteInEditMode]
public class ImageLayout : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this._image = base.GetComponent<global::UnityEngine.UI.Image>();
		this._layout = base.GetComponent<global::UnityEngine.UI.LayoutElement>();
	}

	private global::UnityEngine.UI.Image _image;

	private global::UnityEngine.UI.LayoutElement _layout;

	public global::UnityEngine.Rect rect;
}
