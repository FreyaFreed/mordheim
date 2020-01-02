using System;
using UnityEngine;

public class LoopCredits : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.canvasRect = (global::UnityEngine.RectTransform)base.GetComponentInParent<global::UnityEngine.Canvas>().transform;
		this.creditsRect = (global::UnityEngine.RectTransform)base.transform;
	}

	private void Update()
	{
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[4];
		this.canvasRect.GetWorldCorners(array);
		float y = array[1].y;
		this.creditsRect.GetWorldCorners(array);
		float y2 = array[0].y;
		if (y2 > y)
		{
			base.transform.position = new global::UnityEngine.Vector3(base.transform.position.x, 0f, base.transform.position.z);
		}
	}

	private global::UnityEngine.RectTransform canvasRect;

	private global::UnityEngine.RectTransform creditsRect;
}
