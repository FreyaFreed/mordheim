using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.RectTransform))]
public class ResetToZero : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		(base.transform as global::UnityEngine.RectTransform).anchoredPosition = global::UnityEngine.Vector3.zero;
	}
}
