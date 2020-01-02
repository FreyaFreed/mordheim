using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FleeMove : global::ICheapState
{
	public FleeMove(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Update()
	{
		if (!global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating && !this.pathSearched)
		{
			this.pathSearched = true;
			global::Pathfinding.FleePath fleePath = global::Pathfinding.FleePath.Construct(this.unitCtrlr.transform.position, this.unitCtrlr.FleeTarget, (int)((float)this.unitCtrlr.unit.Movement * this.unitCtrlr.fleeDistanceMultiplier * 1000f), null);
			fleePath.aimStrength = 10f;
			int traversableTags = 1;
			global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.traversableTags = traversableTags;
			global::PandoraSingleton<global::MissionManager>.Instance.pathRayModifier.SetRadius(this.unitCtrlr.CapsuleRadius);
			global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.StartPath(fleePath, new global::Pathfinding.OnPathDelegate(this.PathDone), -1);
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.unitCtrlr.transform, true, false, true, this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.WATCH, null, true, true, true, false);
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(this.unitCtrlr.transform);
			}
		}
		if (this.pathDone && !this.moveDone)
		{
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWOwnMoving(this.unitCtrlr);
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
			}
			this.flatPrevPos.x = this.previousNodePos.x;
			this.flatPrevPos.y = this.previousNodePos.z;
			this.flatNextPos.x = this.nextNodePos.x;
			this.flatNextPos.y = this.nextNodePos.z;
			this.flatCurPos.x = this.unitCtrlr.transform.position.x;
			this.flatCurPos.y = this.unitCtrlr.transform.position.z;
			if ((this.unitCtrlr.interactivePoints.Count == 0 && global::UnityEngine.Vector2.SqrMagnitude(this.flatPrevPos - this.flatCurPos) > global::UnityEngine.Vector2.SqrMagnitude(this.flatPrevPos - this.flatNextPos)) || (this.unitCtrlr.interactivePoints.Count != 0 && global::UnityEngine.Vector2.SqrMagnitude(this.flatNextPos - this.flatCurPos) < 0.2f))
			{
				if (this.waypointIdx >= this.path.vectorPath.Count - 1)
				{
					this.EndMove();
					return;
				}
				this.waypointIdx++;
				this.previousNodePos = this.nextNodePos;
				this.nextNodePos = this.path.vectorPath[this.waypointIdx];
				int num = -1;
				for (int i = 0; i < this.path.path.Count; i++)
				{
					if (this.path.path[i].position == new global::Pathfinding.Int3(this.nextNodePos))
					{
						num = i;
						break;
					}
				}
				if (num != -1 && this.unitCtrlr.interactivePoints.Count > 0 && num > 0 && this.path.path[num - 1].GraphIndex == 1U && this.path.path[num].GraphIndex == 1U)
				{
					this.EndMove();
					return;
				}
			}
			if (global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - this.lastPosition) < 0.0100000007f)
			{
				this.blockedTimer += global::UnityEngine.Time.deltaTime;
				if (this.blockedTimer >= 1f)
				{
					this.EndMove();
					return;
				}
			}
			else
			{
				this.blockedTimer = 0f;
			}
			this.unitCtrlr.SetAnimSpeed(1f);
			global::UnityEngine.Quaternion quaternion = default(global::UnityEngine.Quaternion);
			quaternion.SetLookRotation(this.nextNodePos - this.unitCtrlr.transform.position, global::UnityEngine.Vector3.up);
			global::UnityEngine.Vector3 eulerAngles = quaternion.eulerAngles;
			eulerAngles.x = 0f;
			eulerAngles.z = 0f;
			this.unitCtrlr.transform.rotation = global::UnityEngine.Quaternion.Euler(eulerAngles);
			this.lastPosition = this.unitCtrlr.transform.position;
		}
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.unitCtrlr.SetFleeTarget();
		global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Pause();
		this.pathDone = false;
		this.previousNodePos = this.unitCtrlr.transform.position;
		this.nextNodePos = this.unitCtrlr.transform.position;
		this.lastPosition = global::UnityEngine.Vector3.zero;
		this.waypointIdx = 0;
		this.blockedTimer = 0f;
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int i = 0; i < allUnits.Count; i++)
		{
			if (allUnits[i].unit.Status != global::UnitStateId.OUT_OF_ACTION && !allUnits[i].isNavCutterActive())
			{
				allUnits[i].SetGraphWalkability(false);
			}
		}
		this.unitCtrlr.SetGraphWalkability(true);
		global::PandoraDebug.LogInfo("Flee move distance " + (int)((float)this.unitCtrlr.unit.Movement * this.unitCtrlr.fleeDistanceMultiplier * 1000f), "uncategorised", null);
		this.pathSearched = false;
		this.moveDone = true;
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() != this.unitCtrlr && global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
		{
			this.unitCtrlr.StartSync();
		}
	}

	public void Exit(int iTo)
	{
		this.unitCtrlr.fleeDistanceMultiplier = global::Constant.GetFloat(global::ConstantId.FLEE_MOVEMENT_MULTIPLIER);
		this.unitCtrlr.SetGraphWalkability(false);
	}

	public void FixedUpdate()
	{
	}

	private void PathDone(global::Pathfinding.Path p)
	{
		this.path = (global::Pathfinding.FleePath)p;
		this.path.Claim(this.unitCtrlr);
		if (p.vectorPath.Count > 0)
		{
			this.pathDone = true;
			this.moveDone = false;
			this.nextNodePos = this.path.vectorPath[this.waypointIdx];
			this.unitCtrlr.SetFixed(false);
			this.unitCtrlr.SetKinemantic(false);
		}
		else
		{
			this.EndMove();
		}
	}

	private void EndMove()
	{
		this.moveDone = true;
		this.path.Release(this.unitCtrlr, false);
		this.unitCtrlr.SetAnimSpeed(0f);
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetKinemantic(true);
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() != this.unitCtrlr && global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
		{
			this.unitCtrlr.StopSync();
		}
		this.unitCtrlr.SendStartMove(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation);
	}

	private const float BLOCK_TIME = 1f;

	private global::UnitController unitCtrlr;

	private bool pathDone;

	private global::UnityEngine.GameObject pathRenderer;

	private global::Pathfinding.FleePath path;

	private global::UnityEngine.Vector2 flatPrevPos;

	private global::UnityEngine.Vector2 flatNextPos;

	private global::UnityEngine.Vector2 flatCurPos;

	private global::UnityEngine.Vector3 previousNodePos;

	private global::UnityEngine.Vector3 nextNodePos;

	private int waypointIdx;

	private global::UnityEngine.Vector3 lastPosition;

	private float blockedTimer;

	private bool pathSearched;

	private bool moveDone;
}
