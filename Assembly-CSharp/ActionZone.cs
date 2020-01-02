using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionZone : global::InteractivePoint
{
	public global::PointsChecker PointsChecker { get; private set; }

	private void Awake()
	{
		base.enabled = false;
		this.setuped = false;
		this.PointsChecker = new global::PointsChecker(base.transform, true);
	}

	public override void Init(uint id)
	{
		base.enabled = true;
		this.imprintIcon = null;
		base.Init(id);
	}

	public void SetupFX()
	{
		if (this.setuped)
		{
			return;
		}
		for (int i = 0; i < this.destinations.Count; i++)
		{
			this.destinations[i].particles = this.destinations[i].fx.GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
		}
		this.setuped = true;
	}

	protected override void SetActionIds()
	{
		this.unitActionIds = this.destinations.ConvertAll<global::UnitActionId>((global::ActionDestination x) => x.actionId);
	}

	private void OnDestroy()
	{
		if (this.largeOccupation != null)
		{
			global::UnityEngine.Object.Destroy(this.largeOccupation.gameObject);
		}
	}

	protected override global::System.Collections.Generic.List<global::UnitActionId> GetActions(global::UnitController unitCtrlr)
	{
		bool flag = unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE;
		this.unitActionIds.Clear();
		for (int i = 0; i < this.destinations.Count; i++)
		{
			global::ActionDestination actionDestination = this.destinations[i];
			if ((!flag || actionDestination.destination.supportLargeUnit) && this.PointsChecker.CanDoAthletic() && actionDestination.destination.PointsChecker.IsAvailable())
			{
				global::UnitActionId item = actionDestination.actionId;
				switch (actionDestination.actionId)
				{
				case global::UnitActionId.CLIMB_3M:
				case global::UnitActionId.CLIMB_6M:
				case global::UnitActionId.CLIMB_9M:
					item = global::UnitActionId.CLIMB;
					break;
				case global::UnitActionId.JUMP_3M:
				case global::UnitActionId.JUMP_6M:
				case global::UnitActionId.JUMP_9M:
					item = global::UnitActionId.JUMP;
					break;
				}
				this.unitActionIds.Add(item);
			}
		}
		int num = this.unitActionIds.IndexOf(global::UnitActionId.LEAP, global::UnitActionIdComparer.Instance);
		if (num != -1 && this.unitActionIds.IndexOf(global::UnitActionId.JUMP, global::UnitActionIdComparer.Instance) == -1)
		{
			this.unitActionIds.RemoveAt(num);
		}
		return this.unitActionIds;
	}

	public global::ActionDestination GetClimb()
	{
		for (int i = 0; i < this.destinations.Count; i++)
		{
			if (this.destinations[i].actionId == global::UnitActionId.CLIMB_3M || this.destinations[i].actionId == global::UnitActionId.CLIMB_6M || this.destinations[i].actionId == global::UnitActionId.CLIMB_9M)
			{
				return this.destinations[i];
			}
		}
		return null;
	}

	public global::ActionDestination GetJump()
	{
		for (int i = 0; i < this.destinations.Count; i++)
		{
			if (this.destinations[i].actionId == global::UnitActionId.JUMP_3M || this.destinations[i].actionId == global::UnitActionId.JUMP_6M || this.destinations[i].actionId == global::UnitActionId.JUMP_9M)
			{
				return this.destinations[i];
			}
		}
		return null;
	}

	public global::ActionDestination GetLeap()
	{
		for (int i = 0; i < this.destinations.Count; i++)
		{
			if (this.destinations[i].actionId == global::UnitActionId.LEAP)
			{
				return this.destinations[i];
			}
		}
		return null;
	}

	public void EnableFx(bool active)
	{
		for (int i = 0; i < this.destinations.Count; i++)
		{
			if (this.destinations[i].fx.activeSelf != active)
			{
				this.destinations[i].fx.SetActive(active);
			}
		}
	}

	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.yellow;
		if (this.PointsChecker != null)
		{
			for (int i = 0; i < this.PointsChecker.validPoints.Count; i++)
			{
				global::UnityEngine.Gizmos.DrawSphere(this.PointsChecker.validPoints[i], 0.1f);
			}
		}
	}

	public bool supportLargeUnit;

	public global::System.Collections.Generic.List<global::ActionDestination> destinations = new global::System.Collections.Generic.List<global::ActionDestination>();

	public global::OccupationChecker occupation;

	public global::OccupationChecker largeOccupation;

	private global::UnitController unitCtrlr;

	private bool setuped;
}
