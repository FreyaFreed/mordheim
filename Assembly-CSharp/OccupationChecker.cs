using System;
using System.Collections.Generic;
using UnityEngine;

public class OccupationChecker : global::UnityEngine.MonoBehaviour
{
	public int Occupation
	{
		get
		{
			return this.occupiers.Count;
		}
	}

	private void Awake()
	{
		base.gameObject.SetLayerRecursively(global::UnityEngine.LayerMask.NameToLayer("Ignore Raycast"));
	}

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (other.name != "climb_collider" && other.name != "action_zone" && other.name != "action_collision" && other.name != "large_collision" && !other.name.Contains("collision_walk") && this.occupiers.IndexOf(other) == -1)
		{
			this.occupiers.Add(other);
		}
	}

	private void OnTriggerExit(global::UnityEngine.Collider other)
	{
		if (other.name != "climb_collider" && other.name != "action_zone" && other.name != "action_collision" && other.name != "large_collision" && !other.name.Contains("collision_walk"))
		{
			int num = this.occupiers.IndexOf(other);
			if (num != -1)
			{
				this.occupiers.RemoveAt(num);
			}
		}
	}

	private global::System.Collections.Generic.List<global::UnityEngine.Collider> occupiers = new global::System.Collections.Generic.List<global::UnityEngine.Collider>();
}
