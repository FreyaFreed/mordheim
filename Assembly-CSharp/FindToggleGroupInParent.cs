using System;
using UnityEngine;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Toggle))]
public class FindToggleGroupInParent : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::UnityEngine.UI.Toggle component = base.GetComponent<global::UnityEngine.UI.Toggle>();
		global::UnityEngine.UI.ToggleGroup componentInParent = base.GetComponentInParent<global::UnityEngine.UI.ToggleGroup>();
		component.group = componentInParent;
	}
}
