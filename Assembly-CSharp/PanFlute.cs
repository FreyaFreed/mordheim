using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.AudioSource))]
public class PanFlute : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::PandoraSingleton<global::Pan>.Instance.AddSource(this);
		global::UnityEngine.Object.Destroy(this);
	}

	public global::Pan.Type fluteType;
}
