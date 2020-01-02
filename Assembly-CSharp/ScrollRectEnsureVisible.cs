using System;
using UnityEngine;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.ScrollRect))]
public class ScrollRectEnsureVisible : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this._mScrollRect = base.GetComponent<global::UnityEngine.UI.ScrollRect>();
		this._mScrollTransform = (this._mScrollRect.transform as global::UnityEngine.RectTransform);
		this._mContent = this._mScrollRect.content;
	}

	public void CenterOnItem(global::UnityEngine.RectTransform target)
	{
		global::UnityEngine.Vector3 worldPointInWidget = this.GetWorldPointInWidget(this._mScrollTransform, this.GetWidgetWorldPoint(target));
		global::UnityEngine.Vector3 worldPointInWidget2 = this.GetWorldPointInWidget(this._mScrollTransform, this.GetWidgetWorldPoint(this.MaskTransform));
		global::UnityEngine.Vector3 vector = worldPointInWidget2 - worldPointInWidget;
		vector.z = 0f;
		if (!this._mScrollRect.horizontal)
		{
			vector.x = 0f;
		}
		if (!this._mScrollRect.vertical)
		{
			vector.y = 0f;
		}
		global::UnityEngine.Vector2 vector2 = this._mContent.rect.size - this._mScrollTransform.rect.size;
		global::UnityEngine.Vector2 b = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Approximately(vector2.x, 0f) ? 0f : (vector.x / vector2.x), global::UnityEngine.Mathf.Approximately(vector2.y, 0f) ? 0f : (vector.y / vector2.y));
		global::UnityEngine.Vector2 normalizedPosition = this._mScrollRect.normalizedPosition - b;
		if (this._mScrollRect.movementType != global::UnityEngine.UI.ScrollRect.MovementType.Unrestricted)
		{
			normalizedPosition.x = global::UnityEngine.Mathf.Clamp01(normalizedPosition.x);
			normalizedPosition.y = global::UnityEngine.Mathf.Clamp01(normalizedPosition.y);
		}
		this._mScrollRect.normalizedPosition = normalizedPosition;
	}

	private global::UnityEngine.Vector3 GetWidgetWorldPoint(global::UnityEngine.RectTransform target)
	{
		global::UnityEngine.Vector3 b = new global::UnityEngine.Vector3((0.5f - target.pivot.x) * target.rect.size.x, (0.5f - target.pivot.y) * target.rect.size.y, 0f);
		global::UnityEngine.Vector3 position = target.localPosition + b;
		return target.parent.TransformPoint(position);
	}

	private global::UnityEngine.Vector3 GetWorldPointInWidget(global::UnityEngine.RectTransform target, global::UnityEngine.Vector3 worldPoint)
	{
		return target.InverseTransformPoint(worldPoint);
	}

	public float AnimTime = 0.15f;

	public bool Snap;

	public global::UnityEngine.RectTransform MaskTransform;

	private global::UnityEngine.UI.ScrollRect _mScrollRect;

	private global::UnityEngine.RectTransform _mScrollTransform;

	private global::UnityEngine.RectTransform _mContent;
}
