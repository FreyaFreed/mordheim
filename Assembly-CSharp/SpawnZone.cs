using System;
using UnityEngine;

public class SpawnZone : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.bounds = default(global::UnityEngine.Bounds);
		foreach (object obj in base.transform)
		{
			global::UnityEngine.Transform transform = (global::UnityEngine.Transform)obj;
			if (transform.name == "bounds")
			{
				this.bounds = transform.GetComponent<global::UnityEngine.Renderer>().bounds;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (this.type == global::SpawnZoneId.SCATTER)
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.white;
			global::UnityEngine.Gizmos.DrawWireSphere(base.transform.position, this.range);
		}
	}

	public void Claim(global::DeploymentId deployId)
	{
		this.claimMask |= 1 << (int)deployId;
	}

	public bool IsClaimed(global::DeploymentId deployId)
	{
		global::SpawnZoneId spawnZoneId = this.type;
		if (spawnZoneId != global::SpawnZoneId.AMBUSH)
		{
			return this.claimMask != 0;
		}
		return (this.claimMask & 1 << (int)deployId) != 0;
	}

	public global::SpawnZoneId type;

	public bool indoor;

	public float range;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Bounds bounds;

	private int claimMask;
}
