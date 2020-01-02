using System;
using Pathfinding;
using RAIN.Action;
using RAIN.Core;
using UnityEngine;

public class AIMoveOnPath : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "MoveOnPath";
		this.path = this.unitCtrlr.AICtrlr.currentPath;
		this.path.Claim(this.unitCtrlr.gameObject);
		this.animator = this.unitCtrlr.animator;
		this.waypointIdx = 0;
		this.previousNodePos = this.unitCtrlr.transform.position;
		this.nextNodePos = this.path.vectorPath[this.waypointIdx];
		this.moveEnding = false;
		float num = (float)(this.unitCtrlr.unit.Movement * this.unitCtrlr.unit.Movement);
		this.unitCtrlr.SetFixed(false);
		if (!this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.ClampToNavMesh();
		}
		this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = false;
		this.unitCtrlr.startPosition = this.unitCtrlr.transform.position;
		this.lastValidPosition = this.unitCtrlr.transform.position;
		this.lastPosition = this.unitCtrlr.transform.position;
		this.blockedTimer = 0f;
		this.success = (this.unitCtrlr.unit.CurrentStrategyPoints > 0);
		this.tempStratPoints = 0;
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.moveEnding)
		{
			return global::RAIN.Action.RAINAction.ActionResult.RUNNING;
		}
		if (!this.success)
		{
			this.unitCtrlr.AICtrlr.failedMove++;
			return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
		this.unitCtrlr.UpdateActionStatus(false, global::UnitActionRefreshId.NONE);
		this.unitCtrlr.AICtrlr.UpdateVisibility();
		this.unitCtrlr.CheckEngaged(true);
		if (this.unitCtrlr.Engaged)
		{
			this.EndMove();
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		if (this.unitCtrlr.AICtrlr.movingToSearchPoint)
		{
			for (int i = 0; i < this.unitCtrlr.interactivePoints.Count; i++)
			{
				if (this.unitCtrlr.interactivePoints[i] is global::SearchPoint && (global::SearchPoint)this.unitCtrlr.interactivePoints[i] == this.unitCtrlr.AICtrlr.targetSearchPoint)
				{
					this.EndMove();
					return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
				}
			}
		}
		if (this.unitCtrlr.AICtrlr.targetDestructible != null)
		{
			float num = global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - this.unitCtrlr.AICtrlr.targetDestructible.transform.position);
			if (num <= 2.25f)
			{
				this.EndMove();
				return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
			}
			global::SkillId skillId = (!this.unitCtrlr.HasClose()) ? global::SkillId.BASE_SHOOT : global::SkillId.BASE_ATTACK;
			global::ActionStatus action = this.unitCtrlr.GetAction(skillId);
			for (int j = 0; j < action.Destructibles.Count; j++)
			{
				if (action.Destructibles[j] == this.unitCtrlr.AICtrlr.targetDestructible)
				{
					this.EndMove();
					return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
				}
			}
		}
		float num2 = (float)(this.unitCtrlr.unit.Movement * this.unitCtrlr.unit.Movement);
		float num3 = global::PandoraUtils.FlatSqrDistance(this.unitCtrlr.startPosition, this.unitCtrlr.transform.position);
		if (num3 > num2)
		{
			if (this.unitCtrlr.unit.tempStrategyPoints >= this.unitCtrlr.unit.CurrentStrategyPoints)
			{
				this.unitCtrlr.transform.position = this.lastValidPosition;
				this.EndMove();
				return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
			}
			this.tempStratPoints++;
			this.unitCtrlr.startPosition = this.unitCtrlr.transform.position;
			num3 = global::PandoraUtils.FlatSqrDistance(this.unitCtrlr.startPosition, this.unitCtrlr.transform.position);
		}
		this.unitCtrlr.unit.tempStrategyPoints = this.tempStratPoints + ((num3 <= 1f) ? 0 : 1);
		if (global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - this.lastPosition) < 0.0064f)
		{
			this.blockedTimer += global::UnityEngine.Time.deltaTime;
			if (this.blockedTimer >= 1f)
			{
				this.unitCtrlr.AICtrlr.failedMove++;
				this.EndMove();
				global::PandoraDebug.LogWarning("AI movement end because it was blocked", "AI", null);
				return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
			}
		}
		else
		{
			this.blockedTimer = 0f;
		}
		this.lastValidPosition = this.unitCtrlr.transform.position;
		this.lastPosition = this.unitCtrlr.transform.position;
		this.flatPrevPos.x = this.previousNodePos.x;
		this.flatPrevPos.y = this.previousNodePos.z;
		this.flatNextPos.x = this.nextNodePos.x;
		this.flatNextPos.y = this.nextNodePos.z;
		this.flatCurPos.x = this.unitCtrlr.transform.position.x;
		this.flatCurPos.y = this.unitCtrlr.transform.position.z;
		if ((this.unitCtrlr.interactivePoints.Count == 0 && global::UnityEngine.Vector2.SqrMagnitude(this.flatPrevPos - this.flatCurPos) > global::UnityEngine.Vector2.SqrMagnitude(this.flatPrevPos - this.flatNextPos)) || (this.unitCtrlr.interactivePoints.Count != 0 && global::UnityEngine.Vector2.SqrMagnitude(this.flatNextPos - this.flatCurPos) < 0.3f))
		{
			if (this.waypointIdx >= this.path.vectorPath.Count - 1)
			{
				this.EndMove();
				return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
			}
			this.waypointIdx++;
			this.previousNodePos = this.nextNodePos;
			this.nextNodePos = this.path.vectorPath[this.waypointIdx];
			int num4 = -1;
			for (int k = 0; k < this.path.path.Count; k++)
			{
				if (this.path.path[k].position == new global::Pathfinding.Int3(this.nextNodePos))
				{
					num4 = k;
					break;
				}
			}
			if (num4 != -1 && this.unitCtrlr.interactivePoints.Count > 0 && num4 > 0 && this.path.path[num4 - 1].GraphIndex == 1U && this.path.path[num4].GraphIndex == 1U)
			{
				for (int l = 0; l < this.unitCtrlr.interactivePoints.Count; l++)
				{
					if (this.unitCtrlr.interactivePoints[l] is global::ActionZone)
					{
						global::ActionZone actionZone = (global::ActionZone)this.unitCtrlr.interactivePoints[l];
						int m = 0;
						while (m < actionZone.destinations.Count)
						{
							global::ActionDestination actionDestination = actionZone.destinations[m];
							if (new global::Pathfinding.Int3(actionDestination.destination.transform.position) == this.path.path[num4].position)
							{
								this.unitCtrlr.interactivePoint = actionZone;
								this.unitCtrlr.activeActionDest = actionDestination;
								this.unitCtrlr.ValidMove();
								this.CleanUp();
								global::SkillId skillId2 = global::SkillId.NONE;
								global::UnitActionId actionId = actionDestination.actionId;
								switch (actionId)
								{
								case global::UnitActionId.CLIMB_3M:
								case global::UnitActionId.CLIMB_6M:
								case global::UnitActionId.CLIMB_9M:
									break;
								case global::UnitActionId.LEAP:
									skillId2 = global::SkillId.BASE_LEAP;
									goto IL_700;
								case global::UnitActionId.JUMP_3M:
								case global::UnitActionId.JUMP_6M:
								case global::UnitActionId.JUMP_9M:
									goto IL_6E8;
								default:
									if (actionId != global::UnitActionId.CLIMB)
									{
										if (actionId != global::UnitActionId.JUMP)
										{
											goto IL_700;
										}
										goto IL_6E8;
									}
									break;
								}
								skillId2 = global::SkillId.BASE_CLIMB;
								goto IL_700;
								IL_6E8:
								skillId2 = global::SkillId.BASE_JUMPDOWN;
								IL_700:
								global::ActionStatus action2 = this.unitCtrlr.GetAction(skillId2);
								if (action2.Available)
								{
									this.unitCtrlr.SendInteractiveAction(skillId2, actionZone);
									return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
								}
								return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
							}
							else
							{
								m++;
							}
						}
					}
				}
			}
		}
		this.unitCtrlr.SetAnimSpeed(1f);
		global::UnityEngine.Quaternion quaternion = default(global::UnityEngine.Quaternion);
		quaternion.SetLookRotation(this.nextNodePos - this.unitCtrlr.transform.position, global::UnityEngine.Vector3.up);
		global::UnityEngine.Vector3 eulerAngles = quaternion.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		this.unitCtrlr.transform.rotation = global::UnityEngine.Quaternion.Euler(eulerAngles);
		return global::RAIN.Action.RAINAction.ActionResult.RUNNING;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
		this.CleanUp();
	}

	public virtual void GotoNextState()
	{
		this.unitCtrlr.StateMachine.ChangeState(42);
	}

	protected void EndMove()
	{
		this.moveEnding = true;
		this.CleanUp();
		this.unitCtrlr.AICtrlr.atDestination = true;
		if (!this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.ClampToNavMesh();
		}
		this.unitCtrlr.ValidMove();
		this.unitCtrlr.SetFixed(true);
		if (!this.unitCtrlr.wasEngaged && this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.SendEngaged(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation, false);
		}
		else
		{
			this.GotoNextState();
		}
	}

	private void CleanUp()
	{
		this.unitCtrlr.AICtrlr.movingToSearchPoint = false;
		this.unitCtrlr.RestoreAlliesNavCutterSize();
		if (this.path != null)
		{
			this.unitCtrlr.AICtrlr.currentPath = null;
			this.path.Release(this.unitCtrlr.gameObject, false);
			this.path = null;
		}
		if (!this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic)
		{
			this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().velocity = global::UnityEngine.Vector3.zero;
		}
		this.unitCtrlr.SetAnimSpeed(0f);
	}

	private const float BLOCK_TIME = 1f;

	private const float WAYPOINT_DIST = 0.3f;

	private global::Pathfinding.Path path;

	private int waypointIdx;

	private global::UnityEngine.Vector3 previousNodePos;

	private global::UnityEngine.Vector3 nextNodePos;

	private global::UnityEngine.Vector3 lastValidPosition;

	private global::UnityEngine.Vector2 flatPrevPos;

	private global::UnityEngine.Vector2 flatNextPos;

	private global::UnityEngine.Vector2 flatCurPos;

	private global::UnityEngine.Animator animator;

	private global::UnityEngine.Vector3 lastPosition;

	private float blockedTimer;

	private int tempStratPoints;

	private bool moveEnding;
}
