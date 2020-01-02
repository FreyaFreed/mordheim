using System;
using UnityEngine;

public class ActionZoneChecker : global::UnityEngine.MonoBehaviour
{
	public void Check()
	{
		global::UnityEngine.LayerMask mask = default(global::UnityEngine.LayerMask);
		mask = (1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground"));
		global::UnityEngine.RaycastHit raycastHit = default(global::UnityEngine.RaycastHit);
		if (global::UnityEngine.Physics.SphereCast(base.transform.position + new global::UnityEngine.Vector3(0f, 0.7f, 0f), 0.45f, global::UnityEngine.Vector3.down, out raycastHit, (float)mask) && (double)raycastHit.distance > 0.27)
		{
			this.toDestroy = true;
		}
	}

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (other.name != "climb_collider" && other.name != "action_zone" && other.name != "large_collision" && other.name != "action_collision" && other.name != "bounds" && other.name != "trigger" && other.name != "fx_ghost_wall_flame_01(clone)" && !other.name.Contains("collision_walk"))
		{
			this.toDestroy = true;
		}
	}

	public bool toDestroy;
}
