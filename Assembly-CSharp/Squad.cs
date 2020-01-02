using System;
using System.Collections.Generic;
using UnityEngine;

public class Squad
{
	public Squad()
	{
		this.members = new global::System.Collections.Generic.List<global::UnitController>();
	}

	public void RefineTargetsList(global::System.Collections.Generic.List<global::UnitController> targets)
	{
		if (this.targetEnemy == null || this.targetEnemy.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			this.targetEnemy = null;
			return;
		}
		for (int i = targets.Count - 1; i >= 0; i--)
		{
			if (global::UnityEngine.Vector3.SqrMagnitude(targets[i].transform.position - this.targetEnemy.transform.position) > 100f)
			{
				targets.RemoveAt(i);
			}
		}
		if (targets.Count == 0)
		{
			targets.Add(this.targetEnemy);
		}
	}

	public void SetTarget(global::UnitController target, bool spottedReachable)
	{
		if (target == null)
		{
			return;
		}
		if (this.targetEnemy == null || this.targetEnemy.unit.Status == global::UnitStateId.OUT_OF_ACTION || (this.targetEnemy != null && !this.targetSpottedReachable && target != null && spottedReachable))
		{
			this.targetEnemy = target;
			this.targetSpottedReachable = spottedReachable;
		}
	}

	public void RemoveDeadMembers()
	{
		for (int i = this.members.Count - 1; i >= 0; i--)
		{
			if (this.members[i].unit.Status == global::UnitStateId.OUT_OF_ACTION)
			{
				this.members.RemoveAt(i);
			}
		}
	}

	public bool LoneLostLastMember()
	{
		if (this.members.Count == 1 && !this.members[0].Engaged)
		{
			global::System.Collections.Generic.List<global::UnitController> aliveEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.members[0].GetWarband().idx);
			for (int i = 0; i < aliveEnemies.Count; i++)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(this.members[0].transform.position - aliveEnemies[i].transform.position) < 100f)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	private const float MIN_TARGETS_SQR_DIST = 100f;

	public global::System.Collections.Generic.List<global::UnitController> members;

	public global::UnitController targetEnemy;

	public bool targetSpottedReachable;
}
