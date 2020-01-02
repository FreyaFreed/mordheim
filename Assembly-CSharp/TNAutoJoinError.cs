using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.TextMesh))]
public class TNAutoJoinError : global::UnityEngine.MonoBehaviour
{
	private void AutoJoinError(string message)
	{
		global::UnityEngine.TextMesh component = base.GetComponent<global::UnityEngine.TextMesh>();
		component.text = message;
	}
}
