using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

public class Teleporter : global::TriggerPoint
{
	public global::System.Collections.Generic.List<global::PointsChecker> PointsCheckers { get; private set; }

	private global::UnityEngine.GameObject CurrentExit
	{
		get
		{
			return (this.currentExitIdx != 0) ? this.extraExits[this.currentExitIdx - 1] : this.exit;
		}
	}

	private void Awake()
	{
		this.PointsCheckers = new global::System.Collections.Generic.List<global::PointsChecker>();
		this.PointsCheckers.Add(new global::PointsChecker(this.exit.transform, false));
		for (int i = 0; i < this.extraExits.Count; i++)
		{
			this.PointsCheckers.Add(new global::PointsChecker(this.extraExits[i].transform, false));
		}
	}

	private void Start()
	{
		global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Add(this);
		base.Init();
	}

	public override void Trigger(global::UnitController currentUnit)
	{
		for (int i = 0; i < this.PointsCheckers.Count; i++)
		{
			if (this.PointsCheckers[i].IsAvailable())
			{
				this.currentExitIdx = i;
				break;
			}
		}
		if (this.prefabFx != null)
		{
			this.SpawnFx(base.transform);
		}
		base.Trigger(currentUnit);
	}

	private void SpawnFx(global::UnityEngine.Transform parent)
	{
		if (this.prefabFx != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.prefabFx.name, parent, true, null);
		}
	}

	public override bool IsActive()
	{
		for (int i = 0; i < this.PointsCheckers.Count; i++)
		{
			if (this.PointsCheckers[i].IsAvailable())
			{
				return true;
			}
		}
		return false;
	}

	public override void ActionOnUnit(global::UnitController currentUnit)
	{
		this.SpawnFx(this.CurrentExit.transform);
		currentUnit.InstantMove(this.CurrentExit.transform.position, this.CurrentExit.transform.rotation);
		global::PandoraSingleton<global::MissionManager>.Instance.MoveUnitsOnActionZone(currentUnit, this.PointsCheckers[this.currentExitIdx], this.PointsCheckers[this.currentExitIdx].alliesOnZone, false);
		global::PandoraSingleton<global::MissionManager>.Instance.MoveUnitsOnActionZone(currentUnit, this.PointsCheckers[this.currentExitIdx], this.PointsCheckers[this.currentExitIdx].enemiesOnZone, true);
		currentUnit.SetFixed(true);
	}

	private void OnDrawGizmos()
	{
		if (this.PointsCheckers == null)
		{
			return;
		}
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.yellow;
		for (int i = 0; i < this.PointsCheckers.Count; i++)
		{
			for (int j = 0; j < this.PointsCheckers[i].validPoints.Count; j++)
			{
				global::UnityEngine.Gizmos.DrawSphere(this.PointsCheckers[i].validPoints[j], 0.2f);
			}
		}
	}

	public global::UnityEngine.GameObject exit;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> extraExits = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	public global::UnityEngine.GameObject prefabFx;

	private int currentExitIdx;
}
