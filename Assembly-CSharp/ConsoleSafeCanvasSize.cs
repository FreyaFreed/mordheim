using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Canvas))]
public class ConsoleSafeCanvasSize : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.canvas = base.GetComponent<global::UnityEngine.Canvas>();
		global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("safe_zone", new global::System.Type[]
		{
			typeof(global::UnityEngine.RectTransform)
		});
		this.rt = (global::UnityEngine.RectTransform)gameObject.transform;
		this.rt.SetParent(this.canvas.transform);
		this.rt.anchorMin = global::UnityEngine.Vector2.zero;
		this.rt.anchorMax = global::UnityEngine.Vector2.one;
		this.rt.offsetMin = global::UnityEngine.Vector2.zero;
		this.rt.offsetMax = global::UnityEngine.Vector2.zero;
		this.rt.pivot = new global::UnityEngine.Vector2(0.5f, 0.5f);
		this.rt.localScale = global::UnityEngine.Vector3.one;
		for (int i = this.canvas.transform.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Transform child = this.canvas.transform.GetChild(i);
			if (child != this.rt)
			{
				child.SetParent(this.rt, true);
				child.SetAsFirstSibling();
			}
		}
		this.Update();
	}

	private void Update()
	{
		float num = this.lastScaleFactor;
		num = global::UnityEngine.Mathf.Lerp(0.9f, 1f, global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsGuiScale);
		if (num != this.lastScaleFactor)
		{
			this.lastScaleFactor = num;
			this.rt.localScale = new global::UnityEngine.Vector3(this.lastScaleFactor, this.lastScaleFactor, 1f);
		}
	}

	private global::UnityEngine.Canvas canvas;

	private float lastScaleFactor;

	private float lastSafeAreaRatio;

	private global::UnityEngine.RectTransform rt;
}
