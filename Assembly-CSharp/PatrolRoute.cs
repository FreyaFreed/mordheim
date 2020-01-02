using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (this.patrolPoints.Count < 2)
		{
			return;
		}
		for (int i = 0; i < this.patrolPoints.Count - 1; i++)
		{
			global::UnityEngine.Debug.DrawLine(this.patrolPoints[i].transform.position + global::UnityEngine.Vector3.up / 2f, this.patrolPoints[i + 1].transform.position + global::UnityEngine.Vector3.up / 2f, global::UnityEngine.Color.cyan);
		}
		if (this.loop)
		{
			global::UnityEngine.Debug.DrawLine(this.patrolPoints[0].transform.position + global::UnityEngine.Vector3.up / 2f, this.patrolPoints[this.patrolPoints.Count - 1].transform.position + global::UnityEngine.Vector3.up / 2f, global::UnityEngine.Color.cyan);
		}
	}

	public void CheckValidity()
	{
		for (int i = this.patrolPoints.Count - 1; i >= 0; i--)
		{
			if (this.patrolPoints[i] == null)
			{
				this.patrolPoints.RemoveAt(i);
			}
		}
		if (this.patrolPoints.Count < 2)
		{
			global::UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RegisterPatrolRoute(this);
		}
	}

	public bool loop;

	public global::System.Collections.Generic.List<global::DecisionPoint> patrolPoints = new global::System.Collections.Generic.List<global::DecisionPoint>();
}
