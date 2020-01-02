using System;
using UnityEngine;

public class cw_trigger : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (other.tag == "Player" && !base.transform.parent.GetComponent<global::UnityEngine.Rigidbody>().isKinematic)
		{
			global::UnityEngine.GameObject gameObject = base.transform.parent.gameObject;
			global::cw_behavior component = gameObject.GetComponent<global::cw_behavior>();
			component.timetoSelectAgain = 0f;
			component.FoundFoodTarget = false;
			component.FoundFreeTarget = false;
		}
	}
}
