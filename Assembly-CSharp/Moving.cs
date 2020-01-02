using System;
using System.Collections.Generic;
using UnityEngine;

public class Moving : global::ICheapState
{
	public Moving(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
		this.rigBody = this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>();
		this.actionIndex = 0;
		global::SkillData data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>(455);
		this.interactionAction = new global::ActionStatus(data, this.unitCtrlr);
		this.interactionAction.UpdateAvailable();
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.zonesSet = false;
		this.alliesCutterReduced = false;
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.interactivePoint = null;
		this.UpdateInteractivePoints();
		global::PandoraSingleton<global::MissionManager>.Instance.SetAccessibleActionZones(this.unitCtrlr, delegate
		{
			this.zonesSet = true;
		});
		float num = (float)(this.unitCtrlr.unit.Movement * this.unitCtrlr.unit.Movement);
		float num2 = global::PandoraUtils.FlatSqrDistance(this.unitCtrlr.startPosition, this.unitCtrlr.transform.position);
		if (num2 <= num)
		{
			this.lastValidPosition = this.unitCtrlr.transform.position;
		}
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows.Count; i++)
		{
			global::UnityEngine.GameObject gameObject = global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows[i];
			gameObject.transform.SetParent(this.unitCtrlr.transform);
			gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.unitCtrlr.transform, true, false, true, this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		global::PandoraSingleton<global::MissionManager>.Instance.ShowCombatCircles(this.unitCtrlr);
		this.unitCtrlr.defenderCtrlr = null;
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"Moving Enter - wasEngaged = ",
			this.unitCtrlr.wasEngaged,
			" IsEngaged Now = ",
			this.unitCtrlr.Engaged
		}), "FLOW", this.unitCtrlr);
		global::PandoraSingleton<global::MissionManager>.Instance.RefreshActionZones(this.unitCtrlr);
		this.RefreshActions(global::UnitActionRefreshId.ALWAYS);
		if (this.unitCtrlr.CurrentAction.waitForConfirmation)
		{
			this.actionIndex = this.actions.IndexOf(this.unitCtrlr.CurrentAction);
		}
		this.UpdateCurrentAction(0);
		this.oldTempStrats = this.unitCtrlr.unit.tempStrategyPoints;
		if (!this.unitCtrlr.Engaged && global::PandoraSingleton<global::MissionManager>.Instance.ActiveBeacons() == 0 && this.unitCtrlr.unit.CurrentStrategyPoints > 0)
		{
			this.unitCtrlr.SpawnBeacon();
		}
		else
		{
			this.RefreshActions(global::UnitActionRefreshId.ALWAYS);
			if (this.unitCtrlr.unit.CurrentStrategyPoints > 0)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.MoveCircle.Show(this.unitCtrlr.startPosition, (float)this.unitCtrlr.unit.Movement);
			}
		}
		this.unitCtrlr.ReduceAlliesNavCutterSize(delegate
		{
			this.alliesCutterReduced = true;
		});
		global::PandoraDebug.LogInfo("Moving connected : " + global::PandoraSingleton<global::Hermes>.Instance.IsConnected(), "HERMES", null);
		global::PandoraDebug.LogInfo("Moving  isMine : " + this.unitCtrlr.IsMine(), "HERMES", null);
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"start sync for unit ID ",
			this.unitCtrlr.uid,
			" Owner ID =",
			this.unitCtrlr.owner
		}), "uncategorised", null);
		this.uiDisplayed = false;
		global::PandoraSingleton<global::UIMissionManager>.Instance.unitAction.OnDisable();
		this.enchantRemoved = false;
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.unitCtrlr.newRotation = global::UnityEngine.Quaternion.identity;
		this.unitCtrlr.lastActionWounds = 0;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.RETROACTION_ACTION_CLEAR);
		if (iTo != 44)
		{
			this.unitCtrlr.RestoreAlliesNavCutterSize();
			if (!this.unitCtrlr.wasEngaged)
			{
				this.unitCtrlr.ClampToNavMesh();
			}
		}
		this.unitCtrlr.HideDetected();
		this.unitCtrlr.SetAnimSpeed(0f);
		global::PandoraSingleton<global::MissionManager>.Instance.TurnOffActionZones();
		this.HidePointingArrows();
		global::PandoraSingleton<global::MissionManager>.Instance.MoveCircle.Hide();
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int, global::System.Collections.Generic.List<global::UnitController>>(global::Notices.COMBAT_HIGHLIGHT_TARGET, -1, new global::System.Collections.Generic.List<global::UnitController>());
	}

	void global::ICheapState.Update()
	{
		if (!this.zonesSet || !this.alliesCutterReduced)
		{
			return;
		}
		if (!this.uiDisplayed)
		{
			this.uiDisplayed = true;
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UNIT_START_MOVE);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.INTERACTION_POINTS_CHANGED);
		}
		if (this.unitCtrlr.CurrentAction.waitForConfirmation)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
			{
				this.unitCtrlr.CurrentAction.Select();
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
			{
				this.unitCtrlr.CurrentAction.Cancel();
			}
			return;
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", 0))
		{
			this.unitCtrlr.StateMachine.ChangeState(44);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.MoveCircle.AdjustHeightAndRadius(this.unitCtrlr.transform.position.y, (float)this.unitCtrlr.unit.GetAttribute(global::AttributeId.MOVEMENT));
		if (!this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.RefreshDetected();
		}
		if (!this.unitCtrlr.Engaged && this.unitCtrlr.animator.deltaPosition.x == 0f && this.unitCtrlr.animator.deltaPosition.z == 0f && !this.unitCtrlr.IsAnimating() && this.unitCtrlr.animator.GetInteger(global::AnimatorIds.action) == 0)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) && this.unitCtrlr.GetAction(global::SkillId.BASE_END_TURN).Available)
			{
				this.actionIndex = this.actions.IndexOf(this.unitCtrlr.GetAction(global::SkillId.BASE_END_TURN));
				this.unitCtrlr.SetCurrentAction(global::SkillId.BASE_END_TURN);
				this.unitCtrlr.CurrentAction.Select();
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, this.actions);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
			{
				this.unitCtrlr.CurrentAction.Select();
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cycling", 0))
			{
				this.UpdateCurrentAction(1);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("cycling", 0))
			{
				this.UpdateCurrentAction(-1);
			}
		}
	}

	void global::ICheapState.FixedUpdate()
	{
		if (!this.zonesSet || !this.alliesCutterReduced)
		{
			return;
		}
		if (this.unitCtrlr.CurrentAction.waitForConfirmation)
		{
			return;
		}
		this.unitCtrlr.CheckEngaged(true);
		if (this.unitCtrlr.Engaged)
		{
			this.unitCtrlr.SendEngaged(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation, false);
		}
		else
		{
			this.unitCtrlr.ClampToNavMesh();
			this.RefreshActions(global::UnitActionRefreshId.ALWAYS);
			this.ShowPointingArrows();
			float num = global::PandoraUtils.FlatSqrDistance(this.unitCtrlr.startPosition, this.unitCtrlr.transform.position);
			float num2 = global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.startPosition - this.unitCtrlr.transform.position);
			float num3 = (float)(this.unitCtrlr.unit.Movement * this.unitCtrlr.unit.Movement);
			if (num > num3)
			{
				if (this.unitCtrlr.unit.Movement > 0 && this.unitCtrlr.unit.tempStrategyPoints < this.unitCtrlr.unit.CurrentStrategyPoints)
				{
					this.unitCtrlr.SpawnBeacon();
					this.UpdateCurrentAction(0);
					num = global::PandoraUtils.FlatSqrDistance(this.unitCtrlr.startPosition, this.unitCtrlr.transform.position);
				}
				else
				{
					this.unitCtrlr.transform.position = this.lastValidPosition;
					if (!this.rigBody.isKinematic)
					{
						this.rigBody.velocity = global::UnityEngine.Vector3.zero;
					}
				}
			}
			else
			{
				this.lastValidPosition = this.unitCtrlr.transform.position;
			}
			if (this.unitCtrlr.CurrentBeacon != null)
			{
				this.unitCtrlr.CurrentBeacon.SetActive(num2 > 1f);
			}
			if (num2 > 1f && !this.enchantRemoved)
			{
				this.unitCtrlr.unit.RemoveEnchantments(global::EnchantmentId.CLIMB_FAIL_EFFECT);
				this.enchantRemoved = true;
			}
			this.unitCtrlr.unit.tempStrategyPoints = global::UnityEngine.Mathf.Clamp(global::PandoraSingleton<global::MissionManager>.Instance.ActiveBeacons() - 1 + ((num2 <= 1f) ? 0 : 1), 0, this.unitCtrlr.unit.StrategyPoints);
			if (this.oldTempStrats != this.unitCtrlr.unit.tempStrategyPoints)
			{
				this.oldTempStrats = this.unitCtrlr.unit.tempStrategyPoints;
				this.RefreshActions(global::UnitActionRefreshId.NONE);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ATTRIBUTES_CHANGED, this.unitCtrlr.unit);
			}
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("v", 0);
			float axis2 = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("h", 0);
			if (this.unitCtrlr.unit.CurrentStrategyPoints > 0 && (axis2 != 0f || axis != 0f) && this.unitCtrlr.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == global::AnimatorIds.idle)
			{
				global::UnityEngine.Vector3 a = global::PandoraSingleton<global::MissionManager>.Instance.CamManager.transform.forward;
				a.y = 0f;
				a.Normalize();
				a *= axis;
				global::UnityEngine.Vector3 vector = global::PandoraSingleton<global::MissionManager>.Instance.CamManager.transform.right;
				vector.y = 0f;
				vector.Normalize();
				vector *= axis2;
				global::UnityEngine.Vector3 forward = a + vector;
				global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(forward, global::UnityEngine.Vector3.up);
				this.unitCtrlr.newRotation = global::UnityEngine.Quaternion.Lerp(this.unitCtrlr.transform.rotation, b, 7f * global::UnityEngine.Time.fixedDeltaTime);
				this.unitCtrlr.animator.SetFloat(global::AnimatorIds.speed, forward.magnitude, 0.1f, global::UnityEngine.Time.fixedDeltaTime);
				this.unitCtrlr.SetFixed(false);
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWOwnMoving(this.unitCtrlr);
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshActionZones(this.unitCtrlr);
				global::PandoraSingleton<global::MissionManager>.Instance.UpdateCombatCirclesAlpha(this.unitCtrlr);
			}
			else
			{
				if (this.unitCtrlr.animator.GetFloat(global::AnimatorIds.speed) > 0f)
				{
					this.unitCtrlr.SetAnimSpeed(0f);
				}
				this.unitCtrlr.newRotation = global::UnityEngine.Quaternion.identity;
				this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().drag = 100f;
			}
		}
	}

	private void RefreshActions(global::UnitActionRefreshId actionRefreshId)
	{
		if (this.unitCtrlr.UpdateActionStatus(false, actionRefreshId))
		{
			this.UpdateCurrentAction(0);
		}
	}

	private void UpdateCurrentAction(int dir = 0)
	{
		this.actions.Clear();
		for (int i = 0; i < this.unitCtrlr.availableActionStatus.Count; i++)
		{
			if (!this.unitCtrlr.availableActionStatus[i].actionData.Interactive)
			{
				this.actions.Add(this.unitCtrlr.availableActionStatus[i]);
			}
		}
		global::ActionStatus actionStatus = null;
		global::InteractivePoint interactivePoint = null;
		this.possibleActions.Clear();
		for (int j = 0; j < this.unitCtrlr.interactivePoints.Count; j++)
		{
			global::InteractivePoint interactivePoint2 = this.unitCtrlr.interactivePoints[j];
			global::System.Collections.Generic.List<global::UnitActionId> unitActionIds = interactivePoint2.GetUnitActionIds(this.unitCtrlr);
			for (int k = 0; k < unitActionIds.Count; k++)
			{
				global::UnitActionId unitActionId = unitActionIds[k];
				for (int l = 0; l < this.unitCtrlr.actionStatus.Count; l++)
				{
					if (this.unitCtrlr.actionStatus[l].ActionId == unitActionId && this.unitCtrlr.actionStatus[l].Available)
					{
						interactivePoint = interactivePoint2;
						this.possibleActions.Add(this.unitCtrlr.actionStatus[l]);
					}
				}
				if (this.possibleActions.Count > 1)
				{
					break;
				}
			}
		}
		if (this.possibleActions.Count > 0)
		{
			if (actionStatus == null && this.possibleActions.Count == 1)
			{
				actionStatus = this.possibleActions[0];
			}
			else
			{
				actionStatus = this.interactionAction;
			}
		}
		if (actionStatus != null)
		{
			this.actions.Add(actionStatus);
			if (actionStatus != this.interactionAction)
			{
				this.unitCtrlr.interactivePoint = interactivePoint;
				global::UnitActionId actionId = actionStatus.ActionId;
				if (actionId != global::UnitActionId.CLIMB)
				{
					if (actionId != global::UnitActionId.JUMP)
					{
						if (actionId == global::UnitActionId.LEAP)
						{
							this.unitCtrlr.activeActionDest = ((global::ActionZone)interactivePoint).GetLeap();
						}
					}
					else
					{
						this.unitCtrlr.activeActionDest = ((global::ActionZone)interactivePoint).GetJump();
					}
				}
				else
				{
					this.unitCtrlr.activeActionDest = ((global::ActionZone)interactivePoint).GetClimb();
				}
			}
		}
		this.actions.Sort(global::Moving.ActionStatusComparer);
		if (this.actions.Count > 0)
		{
			if (dir == 0)
			{
				this.actionIndex = 0;
			}
			else
			{
				this.actionIndex += dir;
				this.actionIndex = ((this.actionIndex < this.actions.Count) ? ((this.actionIndex >= 0) ? this.actionIndex : (this.actions.Count - 1)) : 0);
			}
			this.unitCtrlr.SetCurrentAction(this.actions[this.actionIndex].SkillId);
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.actions[this.actionIndex], this.actions);
			}
		}
		else if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, null, null);
		}
	}

	private void ShowPointingArrows()
	{
		global::System.Collections.Generic.List<global::UnitController> list;
		if (this.unitCtrlr.HasRange())
		{
			global::ActionStatus action = this.unitCtrlr.GetAction(global::SkillId.BASE_SHOOT);
			list = action.Targets;
		}
		else
		{
			list = this.unitCtrlr.chargeTargets;
		}
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows.Count; i++)
		{
			if (i >= list.Count || list[i] == null)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows[i].SetActive(false);
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows[i].SetActive(true);
				global::UnityEngine.Vector3 toDirection = list[i].transform.position - this.unitCtrlr.transform.position;
				toDirection.y = 0f;
				global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows[i].transform.rotation = global::UnityEngine.Quaternion.FromToRotation(global::UnityEngine.Vector3.forward, toDirection);
			}
		}
	}

	private void HidePointingArrows()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows.Count; i++)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.pointingArrows[i].SetActive(false);
		}
	}

	public void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (this.unitCtrlr.AICtrlr == null)
		{
			this.UpdateCurrentAction(0);
		}
	}

	public void OnTriggerExit(global::UnityEngine.Collider other)
	{
		if (this.unitCtrlr.AICtrlr == null)
		{
			this.UpdateCurrentAction(0);
		}
	}

	private void UpdateInteractivePoints()
	{
		for (int i = this.unitCtrlr.interactivePoints.Count - 1; i >= 0; i--)
		{
			if (!this.unitCtrlr.interactivePoints[i])
			{
				this.unitCtrlr.interactivePoints.RemoveAt(i);
			}
		}
	}

	private const float turnSmoothing = 7f;

	private const float speedDampTime = 0.1f;

	private static readonly global::ActionStatusComparer ActionStatusComparer = new global::ActionStatusComparer();

	private global::UnitController unitCtrlr;

	private global::UnityEngine.Rigidbody rigBody;

	private global::UnityEngine.Vector3 lastValidPosition;

	public global::System.Collections.Generic.List<global::ActionStatus> actions = new global::System.Collections.Generic.List<global::ActionStatus>();

	private global::ActionStatus interactionAction;

	private global::System.Collections.Generic.List<global::ActionStatus> possibleActions = new global::System.Collections.Generic.List<global::ActionStatus>();

	public int actionIndex;

	private int oldTempStrats;

	private bool enchantRemoved;

	private bool zonesSet;

	private bool alliesCutterReduced;

	private bool uiDisplayed;
}
