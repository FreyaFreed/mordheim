using System;
using UnityEngine;

public class DestroyAction : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		if (this.toDestroy == null)
		{
			this.toDestroy = base.gameObject;
		}
	}

	public void Destroy()
	{
		if (this.toDestroy != null)
		{
			global::UnityEngine.Object.Destroy(this.toDestroy);
		}
	}

	public global::UnityEngine.Object toDestroy;
}
