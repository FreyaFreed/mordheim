using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.BoxCollider))]
public class MovingPlatform : global::UnityEngine.MonoBehaviour
{
	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		global::UnitController component = other.GetComponent<global::UnitController>();
		if (component != null)
		{
			component.transform.parent = base.transform;
		}
	}

	private void OnTriggerExit(global::UnityEngine.Collider other)
	{
		global::UnitController component = other.GetComponent<global::UnitController>();
		if (component != null)
		{
			component.transform.parent = null;
		}
	}
}
