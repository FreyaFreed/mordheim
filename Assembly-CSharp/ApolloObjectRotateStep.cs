using System;
using UnityEngine;

public class ApolloObjectRotateStep : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		if (this.next)
		{
			this.next = false;
			base.transform.rotation = base.transform.rotation * global::UnityEngine.Quaternion.Euler(this.rotateStep);
		}
	}

	public bool next;

	public global::UnityEngine.Vector3 rotateStep = new global::UnityEngine.Vector3(0f, 180f, 0f);
}
