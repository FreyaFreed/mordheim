using System;
using System.Collections.Generic;
using UnityEngine;

public class ColliderActivator : global::UnityEngine.MonoBehaviour
{
	public static void ActivateAll()
	{
		for (int i = 0; i < global::ColliderActivator.activators.Count; i++)
		{
			if (global::ColliderActivator.activators[i] != null)
			{
				if (global::ColliderActivator.activators[i].col != null)
				{
					global::ColliderActivator.activators[i].col.enabled = true;
				}
				global::UnityEngine.Object.Destroy(global::ColliderActivator.activators[i]);
			}
		}
		global::ColliderActivator.activators.Clear();
	}

	private void Awake()
	{
		this.col = base.GetComponent<global::UnityEngine.Collider>();
		if (this.col != null)
		{
			this.col.enabled = false;
			global::ColliderActivator.activators.Add(this);
		}
		else
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	public static global::System.Collections.Generic.List<global::ColliderActivator> activators = new global::System.Collections.Generic.List<global::ColliderActivator>();

	private global::UnityEngine.Collider col;
}
