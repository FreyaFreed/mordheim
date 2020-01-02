using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using RAIN.BehaviorTrees;
using RAIN.Core;
using RAIN.Minds;
using UnityEngine;
using UnityEngine.Events;

public class AIController
{
	public AIController(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
		this.Brain = new global::RAIN.Core.AI();
		this.Brain.AIInit();
		this.Brain.Body = this.unitCtrlr.gameObject;
		this.Brain.Mind = new global::RAIN.Minds.BasicMind();
		this.Brain.Mind.AIInit(this.Brain);
		if (ctrlr.unit.CampaignData != null)
		{
			this.SetAIProfile(ctrlr.unit.CampaignData.AiProfileId);
		}
		else
		{
			global::System.Collections.Generic.List<global::UnitRoamingData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRoamingData>("fk_unit_id", ((int)ctrlr.unit.Id).ToConstantString());
			if (this.unitCtrlr.unit.Id == global::UnitId.MANTICORE)
			{
				this.SetAIProfile(global::AiProfileId.CAMPAIGN_MANTICORE);
			}
			else if (list != null && list.Count > 0)
			{
				this.SetAIProfile(list[0].AiProfileId);
			}
			else
			{
				this.SetAIProfile(global::AiProfileId.BASE_SKIRMISH);
			}
		}
	}

	public global::RAIN.Core.AI Brain { get; private set; }

	public global::AiProfileData profileData { get; private set; }

	public global::AiUnitData aiUnitData { get; private set; }

	public global::RAIN.BehaviorTrees.BTAsset bt { get; private set; }

	public global::Squad Squad { get; private set; }

	public void SetAIProfile(global::AiProfileId profileId)
	{
		this.aiProfileId = profileId;
		this.profileData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AiProfileData>((int)this.aiProfileId);
		this.SetBT(this.profileData.AiUnitIdBase);
	}

	public void TurnStartCleanUp()
	{
		this.switchCount = 0;
		this.disengageCount = 0;
		this.atDestination = false;
		this.failedMove = 0;
		this.switchCount = 0;
		this.disengageCount = 0;
		this.targetEnemy = null;
		this.targetDecisionPoint = null;
		this.targetDestructible = null;
		this.excludedUnits.Clear();
		this.skillTargets.Clear();
		this.lootedSearchPoints.Clear();
		this.usedSkillTurn.Clear();
		this.hasCastSkill = false;
	}

	public void TurnEndCleanUp()
	{
		this.hasSeenEnemy = false;
	}

	public void AddEngagedToExcluded()
	{
		if (this.unitCtrlr.HasClose())
		{
			this.excludedUnits.AddRange(this.unitCtrlr.EngagedUnits);
		}
	}

	public void RemoveInactive(global::System.Collections.Generic.List<global::UnitController> targets)
	{
		for (int i = targets.Count - 1; i >= 0; i--)
		{
			if (!targets[i].isInLadder || targets[i].unit.Status == global::UnitStateId.OUT_OF_ACTION)
			{
				targets.RemoveAt(i);
			}
		}
	}

	public bool GotoAlternateMode()
	{
		if (this.profileData.AiUnitIdAlternate != global::AiUnitId.NONE && this.aiUnitData.Id != this.profileData.AiUnitIdAlternate)
		{
			this.SetBT(this.profileData.AiUnitIdAlternate);
			return true;
		}
		return false;
	}

	public void GotoBaseMode()
	{
		if (this.aiUnitData.Id != this.profileData.AiUnitIdBase)
		{
			this.SetBT(this.profileData.AiUnitIdBase);
		}
	}

	public void GotoSearchMode()
	{
		if (this.aiUnitData.Id != this.profileData.AiUnitIdSearch)
		{
			this.SetBT(this.profileData.AiUnitIdSearch);
		}
	}

	public void GotoSkillSpellTargetMode()
	{
		if (this.aiUnitData.Id != this.profileData.AiUnitIdSearch)
		{
			this.SetBT(this.profileData.AiUnitIdSkillSpellTarget);
		}
	}

	public void GotoPreviousMode()
	{
		if (this.aiUnitData.Id != this.previousAIId)
		{
			this.SetBT(this.previousAIId);
		}
	}

	private void SetBT(global::AiUnitId aiId)
	{
		this.previousAIId = ((this.aiUnitData == null) ? global::AiUnitId.NONE : this.aiUnitData.Id);
		this.aiUnitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AiUnitData>((int)aiId);
		this.bt = (global::RAIN.BehaviorTrees.BTAsset)global::UnityEngine.Object.Instantiate(global::UnityEngine.Resources.Load("ai/" + this.aiUnitData.Name));
		(this.Brain.Mind as global::RAIN.Minds.BasicMind).SetBehavior(this.bt, global::PandoraSingleton<global::TreeBinder>.Instance.BtBindings);
	}

	public void RestartBT()
	{
		(this.Brain.Mind as global::RAIN.Minds.BasicMind).ResetBehavior();
	}

	public void FixedUpdate()
	{
		this.Brain.UpdateTime();
		this.Brain.Think();
	}

	public bool AlreadyLootSearchPoint(global::SearchPoint search)
	{
		return this.lootedSearchPoints.IndexOf(search) != -1;
	}

	public void UpdateVisibility()
	{
		global::System.Collections.Generic.List<global::UnitController> aliveEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.unitCtrlr.GetWarband().idx);
		float @float = global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC);
		this.unitCtrlr.UpdateTargetsData();
		for (int i = 0; i < aliveEnemies.Count; i++)
		{
			if (this.unitCtrlr.IsInRange(aliveEnemies[i], 0f, (float)this.unitCtrlr.unit.ViewDistance, @float, false, false, global::BoneId.NONE))
			{
				this.unitCtrlr.GetWarband().SquadManager.UnitSpotted(aliveEnemies[i]);
			}
		}
	}

	public void SetSquad(global::Squad squad, int idx)
	{
		this.Squad = squad;
	}

	public void FindPath(global::System.Collections.Generic.List<global::UnitController> unitsToCheck, global::UnityEngine.Events.UnityAction<bool> allChecked, bool onlyReachable)
	{
		if (this.atDestination)
		{
			allChecked(false);
			return;
		}
		this.units = unitsToCheck;
		if (!onlyReachable && !global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			this.Squad.RefineTargetsList(this.units);
		}
		for (int i = this.units.Count - 1; i >= 0; i--)
		{
			if (this.excludedUnits.IndexOf(this.units[i]) != -1)
			{
				this.units.RemoveAt(i);
			}
		}
		this.reachableUnits.Clear();
		this.reachableUnitsPaths.Clear();
		this.AllChecked = allChecked;
		if (this.targetEnemy != null && this.targetEnemy.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			global::PandoraDebug.LogInfo("Clear targets and set previous target instead(" + this.targetEnemy.name + ")", "PATHFINDING", null);
			this.units.Clear();
			this.units.Add(this.targetEnemy);
		}
		this.StartPathsCheck<global::UnitController>(this.units, global::AIController.BestPathType.CLOSEST, !onlyReachable, this.unitCtrlr.HasRange(), onlyReachable, false, new global::UnityEngine.Events.UnityAction(this.PreCheckUnit), new global::UnityEngine.Events.UnityAction<bool>(this.PostCheckUnit), new global::UnityEngine.Events.UnityAction(this.PathFoundUnit), null);
	}

	private void PreCheckUnit()
	{
		global::UnitController unitController = this.units[this.pathIdx];
		unitController.SetGraphWalkability(true);
		global::PandoraSingleton<global::MissionManager>.Instance.pathRayModifier.traversableColliders.Clear();
		global::PandoraSingleton<global::MissionManager>.Instance.pathRayModifier.traversableColliders.Add(unitController.GetComponent<global::UnityEngine.Collider>());
		global::PandoraSingleton<global::MissionManager>.Instance.pathRayModifier.traversableColliders.Add(unitController.combatCircle.Collider);
		this.maxDist = (float)(this.unitCtrlr.unit.CurrentStrategyPoints * this.unitCtrlr.unit.Movement) + global::Constant.GetFloat((unitController.unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? global::ConstantId.MELEE_RANGE_NORMAL : global::ConstantId.MELEE_RANGE_LARGE);
	}

	private void PostCheckUnit(bool reachable)
	{
		if (reachable)
		{
			this.reachableUnits.Add(this.units[this.pathIdx]);
			this.reachableUnitsPaths[this.units[this.pathIdx]] = this.calculatedPath;
		}
		this.units[this.pathIdx].SetGraphWalkability(false);
	}

	private void PathFoundUnit()
	{
		this.targetEnemy = this.units[this.pathIdx];
	}

	public void FindPath(global::System.Collections.Generic.List<global::SearchPoint> searchPointsToCheck, global::UnityEngine.Events.UnityAction<bool> allChecked)
	{
		if (this.atDestination)
		{
			allChecked(false);
			return;
		}
		this.searchPoints = searchPointsToCheck;
		this.AllChecked = allChecked;
		if (this.targetSearchPoint != null)
		{
			this.searchPoints.Clear();
			this.searchPoints.Add(this.targetSearchPoint);
		}
		this.maxDist = (float)(this.unitCtrlr.unit.CurrentStrategyPoints * this.unitCtrlr.unit.Movement);
		this.StartPathsCheck<global::SearchPoint>(this.searchPoints, global::AIController.BestPathType.CLOSEST, true, false, false, false, new global::UnityEngine.Events.UnityAction(this.PreCheckSearchPoint), null, new global::UnityEngine.Events.UnityAction(this.PathFoundSearch), new global::UnityEngine.Events.UnityAction(this.CannotReachSearch));
	}

	private void PreCheckSearchPoint()
	{
		int index = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.searchPoints[this.pathIdx].triggers.Count);
		this.targets[this.pathIdx] = this.searchPoints[this.pathIdx].triggers[index].transform;
	}

	private void PathFoundSearch()
	{
		this.movingToSearchPoint = true;
		this.targetSearchPoint = this.searchPoints[this.pathIdx];
	}

	private void CannotReachSearch()
	{
		this.movingToSearchPoint = false;
		this.targetSearchPoint = null;
	}

	public void FindPath(global::DecisionPointId decisionPointId, float dist, global::UnityEngine.Events.UnityAction<bool> allChecked)
	{
		if (this.atDestination)
		{
			allChecked(false);
			return;
		}
		this.AllChecked = allChecked;
		this.decisionPoints = global::PandoraSingleton<global::MissionManager>.Instance.GetDecisionPoints(this.unitCtrlr, decisionPointId, dist, true);
		this.maxDist = (float)(this.unitCtrlr.unit.CurrentStrategyPoints * this.unitCtrlr.unit.Movement);
		this.StartPathsCheck<global::DecisionPoint>(this.decisionPoints, global::AIController.BestPathType.CLOSEST, false, false, true, false, null, null, new global::UnityEngine.Events.UnityAction(this.PathFoundTactical), null);
	}

	private void FindPathDecision(global::UnityEngine.Vector3 targetPosition, bool fallBackOnOldPath)
	{
		global::DecisionPointId decisionPointId = (!this.unitCtrlr.HasClose()) ? global::DecisionPointId.OVERWATCH : global::DecisionPointId.AMBUSH;
		bool keepReachable = true;
		global::AIController.BestPathType pathType = global::AIController.BestPathType.FAREST;
		if (this.targetDecisionPoint != null)
		{
			this.decisionPoints.Clear();
			this.decisionPoints.Add(this.targetDecisionPoint);
		}
		else if (this.targetEnemy != null && decisionPointId == global::DecisionPointId.OVERWATCH)
		{
			keepReachable = false;
			pathType = global::AIController.BestPathType.CLOSEST;
			this.FindBestOverwatch();
			if (this.decisionPoints.Count == 0)
			{
				keepReachable = true;
				pathType = global::AIController.BestPathType.FAREST;
				this.FindDecisionClosePath(decisionPointId);
			}
		}
		else
		{
			this.FindDecisionClosePath(decisionPointId);
		}
		this.StartPathsCheck<global::DecisionPoint>(this.decisionPoints, pathType, false, false, keepReachable, fallBackOnOldPath, null, null, new global::UnityEngine.Events.UnityAction(this.PathFoundTactical), null);
	}

	private void FindDecisionClosePath(global::DecisionPointId decisionPointId)
	{
		int num = this.unitCtrlr.unit.CurrentStrategyPoints * this.unitCtrlr.unit.Movement;
		num *= num;
		this.decisionPoints = global::PandoraSingleton<global::MissionManager>.Instance.GetDecisionPoints(this.unitCtrlr, decisionPointId, (float)num, true);
		float num2 = (float)(this.unitCtrlr.unit.Movement * 2);
		num2 *= num2;
		for (int i = this.decisionPoints.Count - 1; i >= 0; i--)
		{
			bool flag = false;
			int num3 = 0;
			while (!flag && num3 < this.currentPath.vectorPath.Count - 1)
			{
				float num4 = global::PandoraUtils.SqrDistPointLineDist(this.currentPath.vectorPath[num3], this.currentPath.vectorPath[num3 + 1], this.decisionPoints[i].transform.position, true);
				if (num4 < num2)
				{
					flag = true;
				}
				num3++;
			}
			if (!flag)
			{
				this.decisionPoints.RemoveAt(i);
			}
		}
	}

	private void FindBestOverwatch()
	{
		float minDistance = 0f;
		float num = 0f;
		if (this.unitCtrlr.HasRange())
		{
			minDistance = (float)this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.RangeMin;
			num = (float)this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.RangeMax;
		}
		else if (this.unitCtrlr.IsAltRange())
		{
			minDistance = (float)this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.InactiveWeaponSlot].Item.RangeMin;
			num = (float)this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.InactiveWeaponSlot].Item.RangeMax;
		}
		global::System.Collections.Generic.List<global::DecisionPoint> list = global::PandoraSingleton<global::MissionManager>.Instance.GetDecisionPoints(this.targetEnemy, global::DecisionPointId.OVERWATCH, num, true);
		int num2 = 0;
		this.decisionPoints.Clear();
		this.hitRolls.Clear();
		this.unitCtrlr.defenderCtrlr = this.targetEnemy;
		global::TargetData targetData = new global::TargetData(this.targetEnemy);
		float @float = global::Constant.GetFloat(global::ConstantId.RANGE_SHOOT_REQUIRED_PERC);
		for (int i = 0; i < list.Count; i++)
		{
			global::DecisionPoint decisionPoint = list[i];
			if (this.unitCtrlr.CanTargetFromPoint(targetData, minDistance, num, @float, true, true, global::BoneId.NONE))
			{
				int rangeHitRoll = this.unitCtrlr.GetRangeHitRoll(decisionPoint.transform, false);
				if (rangeHitRoll > num2)
				{
					num2 = rangeHitRoll;
					for (int j = this.decisionPoints.Count - 1; j >= 0; j--)
					{
						if (this.hitRolls[j] < num2 - 10)
						{
							this.decisionPoints.RemoveAt(j);
							this.hitRolls.RemoveAt(j);
						}
					}
					this.decisionPoints.Add(decisionPoint);
					this.hitRolls.Add(rangeHitRoll);
				}
				else if (rangeHitRoll >= num2 - 10)
				{
					this.decisionPoints.Add(decisionPoint);
					this.hitRolls.Add(rangeHitRoll);
				}
			}
		}
		this.unitCtrlr.defenderCtrlr = null;
	}

	private void PathFoundTactical()
	{
		this.targetDecisionPoint = this.decisionPoints[this.pathIdx];
	}

	public void FindPath(global::System.Collections.Generic.List<global::Destructible> destToCheck, global::UnityEngine.Events.UnityAction<bool> allChecked)
	{
		if (this.atDestination)
		{
			allChecked(false);
			return;
		}
		this.destructibles = destToCheck;
		this.AllChecked = allChecked;
		if (this.targetDestructible != null)
		{
			this.destructibles.Clear();
			this.destructibles.Add(this.targetDestructible);
		}
		this.maxDist = (float)(this.unitCtrlr.unit.CurrentStrategyPoints * this.unitCtrlr.unit.Movement);
		this.StartPathsCheck<global::Destructible>(this.destructibles, global::AIController.BestPathType.CLOSEST, true, false, false, false, null, null, new global::UnityEngine.Events.UnityAction(this.PathFoundDestructible), new global::UnityEngine.Events.UnityAction(this.CannotReachDestructible));
	}

	private void PathFoundDestructible()
	{
		this.targetDestructible = this.destructibles[this.pathIdx];
	}

	private void CannotReachDestructible()
	{
		this.targetDestructible = null;
	}

	private void StartPathsCheck<T>(global::System.Collections.Generic.List<T> behaviours, global::AIController.BestPathType pathType, bool checkDecisionAfter, bool forceDecisionAfter, bool keepReachable, bool fallbackToOld, global::UnityEngine.Events.UnityAction precheck, global::UnityEngine.Events.UnityAction<bool> postCheck, global::UnityEngine.Events.UnityAction pathFound, global::UnityEngine.Events.UnityAction cannotReach) where T : global::UnityEngine.MonoBehaviour
	{
		if (this.targetDecisionPoint != null && (forceDecisionAfter || checkDecisionAfter))
		{
			this.FindPathDecision(this.unitCtrlr.transform.position, false);
			return;
		}
		if (behaviours.Count == 0)
		{
			this.AllChecked(fallbackToOld && this.currentPath != null);
			return;
		}
		this.bestPathType = pathType;
		this.checkDecisionOnCannotReach = checkDecisionAfter;
		this.forceDecisionCheck = forceDecisionAfter;
		this.keepOnlyReachable = keepReachable;
		this.fallBackToOldPath = fallbackToOld;
		this.PreCheck = precheck;
		this.PostCheck = postCheck;
		this.PathFound = pathFound;
		this.CannotReach = cannotReach;
		this.targets.Clear();
		for (int i = 0; i < behaviours.Count; i++)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Transform> list = this.targets;
			T t = behaviours[i];
			list.Add(t.transform);
		}
		this.pathIdx = 0;
		this.oldPath = this.currentPath;
		this.currentPath = null;
		this.targetDecisionPoint = null;
		bool flag = this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE;
		int num;
		if (flag)
		{
			num = 113;
		}
		else
		{
			num = 127;
		}
		if (this.unitCtrlr.unit.IsUnitActionBlocked(global::UnitActionId.CLIMB))
		{
			num = (num & -3 & -17);
		}
		if (this.unitCtrlr.unit.IsUnitActionBlocked(global::UnitActionId.JUMP))
		{
			num = (num & -5 & -33);
		}
		if (this.unitCtrlr.unit.IsUnitActionBlocked(global::UnitActionId.LEAP))
		{
			num = (num & -9 & -65);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.traversableTags = num;
		global::PandoraSingleton<global::MissionManager>.Instance.pathRayModifier.SetRadius(this.unitCtrlr.CapsuleRadius);
		this.unitCtrlr.StopCoroutine(this.CheckNext());
		this.unitCtrlr.ReduceAlliesNavCutterSize(delegate
		{
			this.unitCtrlr.StartCoroutine(this.CheckNext());
		});
	}

	private global::System.Collections.IEnumerator CheckNext()
	{
		if (this.PreCheck != null)
		{
			this.PreCheck();
		}
		while (global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating)
		{
			yield return 0;
		}
		global::Pathfinding.ABPath abPath = global::Pathfinding.ABPath.Construct(this.unitCtrlr.transform.position, this.targets[this.pathIdx].position, null);
		abPath.calculatePartial = true;
		global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.StartPath(abPath, new global::Pathfinding.OnPathDelegate(this.OnPathFinish), -1);
		yield break;
	}

	private void OnPathFinish(global::Pathfinding.Path p)
	{
		bool flag = false;
		this.calculatedPath = p;
		if (p.vectorPath.Count > 0 && ((global::Pathfinding.ABPath)p).endNode == global::AstarPath.active.GetNearest(this.targets[this.pathIdx].position).node)
		{
			float totalLength = p.GetTotalLength();
			flag = (totalLength < this.maxDist);
			if ((!this.keepOnlyReachable || (this.keepOnlyReachable && flag)) && (this.currentPath == null || (this.bestPathType == global::AIController.BestPathType.CLOSEST && totalLength < this.currentPathLg) || (this.bestPathType == global::AIController.BestPathType.FAREST && totalLength > this.currentPathLg)))
			{
				this.bestTarget = this.targets[this.pathIdx];
				this.currentPath = p;
				this.currentPathLg = totalLength;
				this.PathFound();
			}
		}
		if (this.PostCheck != null)
		{
			this.PostCheck(flag);
		}
		this.pathIdx++;
		if (this.pathIdx < this.targets.Count)
		{
			this.unitCtrlr.StopCoroutine(this.CheckNext());
			this.unitCtrlr.StartCoroutine(this.CheckNext());
		}
		else
		{
			global::PandoraDebug.LogInfo("All paths checked!", "PATHFINDING", null);
			if (this.currentPath != null)
			{
				bool flag2 = this.currentPathLg <= this.maxDist;
				if (!flag2 && this.CannotReach != null)
				{
					global::PandoraDebug.LogInfo("Cannot Reach", "PATHFINDING", null);
					this.CannotReach();
				}
				if ((!flag2 && this.checkDecisionOnCannotReach) || this.forceDecisionCheck)
				{
					this.FindPathDecision(this.bestTarget.position, !this.forceDecisionCheck);
				}
				else
				{
					global::PandoraDebug.LogInfo("Path found successfully!", "PATHFINDING", null);
					this.AllChecked(true);
				}
			}
			else
			{
				if (this.fallBackToOldPath && !this.checkDecisionOnCannotReach && !this.forceDecisionCheck && this.oldPath != null)
				{
					global::PandoraDebug.LogInfo("No path found, using old Path", "PATHFINDING", null);
					this.currentPath = this.oldPath;
				}
				else
				{
					global::PandoraDebug.LogInfo("Path found failed!", "PATHFINDING", null);
				}
				this.AllChecked(this.currentPath != null);
			}
		}
	}

	public void UsedSkill(global::SkillId skillId, global::UnitController target = null)
	{
		if (!this.skillTargets.ContainsKey(skillId))
		{
			this.skillTargets.Add(skillId, new global::System.Collections.Generic.List<global::UnitController>());
		}
		if (target != null && this.skillTargets[skillId].IndexOf(target) == -1)
		{
			this.skillTargets[skillId].Add(target);
		}
		if (this.usedSkillTurn.IndexOf(skillId, global::SkillIdComparer.Instance) == -1)
		{
			this.usedSkillTurn.Add(skillId);
		}
	}

	public global::ActionStatus GetBestAction(global::System.Collections.Generic.List<global::UnitActionId> actionIds, out global::UnitController target, global::UnityEngine.Events.UnityAction<global::ActionStatus, global::System.Collections.Generic.List<global::UnitController>> RefineTargets)
	{
		this.defendersCopy.Clear();
		this.defendersCopy.AddRange(this.unitCtrlr.defenders);
		global::UnitController unitController = this.targetEnemy;
		global::ActionStatus result = null;
		target = null;
		this.allActions.Clear();
		this.allTargets.Clear();
		this.validActions.Clear();
		this.validTargets.Clear();
		this.unitCtrlr.UpdateActionStatus(false, global::UnitActionRefreshId.ALWAYS);
		for (int i = 0; i < this.unitCtrlr.actionStatus.Count; i++)
		{
			if (actionIds.IndexOf(this.unitCtrlr.actionStatus[i].ActionId, global::UnitActionIdComparer.Instance) != -1 && this.unitCtrlr.actionStatus[i].Available)
			{
				global::System.Collections.Generic.List<global::UnitController> list = this.GetTargets(this.unitCtrlr.actionStatus[i]);
				RefineTargets(this.unitCtrlr.actionStatus[i], list);
				for (int j = 0; j < list.Count; j++)
				{
					this.unitCtrlr.defenderCtrlr = list[j];
					this.unitCtrlr.SetCurrentAction(this.unitCtrlr.actionStatus[i].SkillId);
					this.targetEnemy = list[j];
					global::AiFilterResultId aiFilterResultId = this.CheckFilters(this.unitCtrlr.actionStatus[i]);
					global::PandoraDebug.LogInfo(string.Concat(new object[]
					{
						"Skill ",
						this.unitCtrlr.actionStatus[i].SkillId,
						" filter result = ",
						aiFilterResultId
					}), "AI", null);
					switch (aiFilterResultId)
					{
					case global::AiFilterResultId.NONE:
						this.allActions.Add(this.unitCtrlr.actionStatus[i]);
						this.allTargets.Add(list[j]);
						break;
					case global::AiFilterResultId.VALID:
						this.validActions.Add(this.unitCtrlr.actionStatus[i]);
						this.validTargets.Add(list[j]);
						break;
					}
				}
			}
		}
		if (this.validActions.Count > 0)
		{
			int index = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.validActions.Count);
			result = this.validActions[index];
			target = this.validTargets[index];
		}
		else if (this.allActions.Count > 0)
		{
			int index2 = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.allActions.Count);
			result = this.allActions[index2];
			target = this.allTargets[index2];
		}
		this.unitCtrlr.defenders.Clear();
		this.unitCtrlr.defenders.AddRange(this.defendersCopy);
		this.targetEnemy = unitController;
		return result;
	}

	private global::System.Collections.Generic.List<global::UnitController> GetTargets(global::ActionStatus action)
	{
		this.tmpTargets.Clear();
		this.tmpTargets.AddRange(action.Targets);
		float @float = global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC);
		switch (action.TargetingId)
		{
		case global::TargetingId.LINE:
		case global::TargetingId.CONE:
		case global::TargetingId.AREA:
		case global::TargetingId.AREA_GROUND:
			for (int i = this.tmpTargets.Count - 1; i >= 0; i--)
			{
				if (!this.unitCtrlr.IsInRange(this.tmpTargets[i], (float)action.RangeMin, (float)(action.RangeMax + action.Radius), @float, false, false, global::BoneId.NONE))
				{
					this.tmpTargets.RemoveAt(i);
				}
			}
			break;
		case global::TargetingId.ARC:
			this.tmpTargets.AddRange(this.unitCtrlr.EngagedUnits);
			break;
		}
		return this.tmpTargets;
	}

	private global::AiFilterResultId CheckFilters(global::ActionStatus action)
	{
		global::System.Collections.Generic.List<global::SkillAiFilterData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillAiFilterData>("fk_skill_id", ((int)action.skillData.Id).ToConstantString());
		this.excludeFilters.Clear();
		this.validFilters.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			global::AiFilterResultId aiFilterResultId = list[i].AiFilterResultId;
			if (aiFilterResultId != global::AiFilterResultId.VALID)
			{
				if (aiFilterResultId == global::AiFilterResultId.EXCLUDED)
				{
					this.excludeFilters.Add(list[i]);
				}
			}
			else
			{
				this.validFilters.Add(list[i]);
			}
		}
		int roll = action.GetRoll(false);
		if (this.CheckFilterType(this.excludeFilters, global::AiFilterResultId.EXCLUDED))
		{
			return global::AiFilterResultId.EXCLUDED;
		}
		if (this.validFilters.Count == 0 || this.CheckFilterType(this.validFilters, global::AiFilterResultId.VALID))
		{
			return global::AiFilterResultId.VALID;
		}
		return global::AiFilterResultId.NONE;
	}

	private bool CheckFilterType(global::System.Collections.Generic.List<global::SkillAiFilterData> filtersData, global::AiFilterResultId resultId)
	{
		global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::SkillAiFilterData>> list = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::SkillAiFilterData>>();
		while (filtersData.Count > 0)
		{
			global::SkillAiFilterData skillAiFilterData = this.GetHeadFilter(filtersData[0], filtersData);
			global::System.Collections.Generic.List<global::SkillAiFilterData> list2 = new global::System.Collections.Generic.List<global::SkillAiFilterData>();
			list2.Add(skillAiFilterData);
			filtersData.Remove(skillAiFilterData);
			while (skillAiFilterData.SkillAiFilterIdAnd != global::SkillAiFilterId.NONE)
			{
				skillAiFilterData = this.GetFilter(skillAiFilterData.SkillAiFilterIdAnd, filtersData);
				list2.Add(skillAiFilterData);
				filtersData.Remove(skillAiFilterData);
			}
			list.Add(list2);
		}
		for (int i = 0; i < list.Count; i++)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < list[i].Count)
			{
				flag &= this.IsFilterValid(list[i][num]);
				num++;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	private global::SkillAiFilterData GetHeadFilter(global::SkillAiFilterData data, global::System.Collections.Generic.List<global::SkillAiFilterData> filters)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 0; i < filters.Count; i++)
			{
				if (filters[i].SkillAiFilterIdAnd == data.Id)
				{
					data = filters[i];
					flag = true;
				}
			}
		}
		return data;
	}

	private global::SkillAiFilterData GetFilter(global::SkillAiFilterId filterId, global::System.Collections.Generic.List<global::SkillAiFilterData> filters)
	{
		for (int i = 0; i < filters.Count; i++)
		{
			if (filters[i].Id == filterId)
			{
				return filters[i];
			}
		}
		return null;
	}

	private bool IsFilterValid(global::SkillAiFilterData filterData)
	{
		global::UnitController unitController = this.unitCtrlr;
		if (filterData.CheckTargetInstead)
		{
			unitController = this.unitCtrlr.AICtrlr.targetEnemy;
		}
		if (unitController == null)
		{
			global::PandoraDebug.LogWarning("No target found when evaluating skill filter " + filterData.Name, "AI", null);
			return false;
		}
		bool flag = true;
		bool reverse = filterData.Reverse;
		if (filterData.AttributeId != global::AttributeId.NONE)
		{
			int attributeVal = this.GetAttributeVal(filterData.AttributeId, unitController);
			int checkValue = filterData.CheckValue;
			if (filterData.AttributeIdCheck != global::AttributeId.NONE)
			{
				checkValue = this.GetAttributeVal(filterData.AttributeIdCheck, unitController);
			}
			flag &= global::AIController.CheckValue(filterData.AiFilterCheckId, checkValue, attributeVal);
		}
		if (flag && filterData.AiFilterCheckIdEngaged != global::AiFilterCheckId.NONE)
		{
			flag &= global::AIController.CheckValue(filterData.AiFilterCheckIdEngaged, filterData.EngagedValue, unitController.EngagedUnits.Count);
		}
		if (flag && filterData.HasAltSet)
		{
			flag &= ((!reverse && unitController.CanSwitchWeapon()) || (reverse && !unitController.CanSwitchWeapon()));
		}
		if (flag && filterData.NeverUsedOnTarget)
		{
			global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)filterData.SkillId);
			global::TargetingId targetingId = skillData.TargetingId;
			global::System.Collections.Generic.List<global::UnitController> list;
			if (targetingId != global::TargetingId.SINGLE_TARGET)
			{
				flag &= ((!reverse && !this.skillTargets.ContainsKey(filterData.SkillId)) || (reverse && this.skillTargets.ContainsKey(filterData.SkillId)));
			}
			else if (this.skillTargets.TryGetValue(filterData.SkillId, out list))
			{
				flag &= ((!reverse && list.IndexOf(unitController) == -1) || (reverse && list.IndexOf(unitController) != -1));
			}
			else
			{
				flag &= !reverse;
			}
		}
		if (flag && filterData.NeverUsedTurn)
		{
			flag &= ((!reverse && this.usedSkillTurn.IndexOf(filterData.SkillId, global::SkillIdComparer.Instance) == -1) || (reverse && this.usedSkillTurn.IndexOf(filterData.SkillId, global::SkillIdComparer.Instance) != -1));
		}
		if (flag && filterData.HasRangeWeapon)
		{
			flag &= ((!reverse && unitController.HasRange()) || (reverse && !unitController.HasRange()));
		}
		if (flag && filterData.IsAllAlone)
		{
			flag &= ((!reverse && unitController.IsAllAlone()) || (reverse && !unitController.IsAllAlone()));
		}
		if (flag && filterData.IsSister)
		{
			flag &= ((!reverse && unitController.GetWarband().WarData.Id == global::WarbandId.SISTERS_OF_SIGMAR) || (reverse && unitController.GetWarband().WarData.Id != global::WarbandId.SISTERS_OF_SIGMAR));
		}
		if (flag && filterData.IsStunned)
		{
			flag &= ((!reverse && unitController.unit.Status == global::UnitStateId.STUNNED) || (reverse && unitController.unit.Status != global::UnitStateId.STUNNED));
		}
		if (flag && filterData.CannotParry)
		{
			flag &= ((!reverse && !unitController.unit.HasEnchantment(global::EnchantmentId.ITEM_PARRY)) || (reverse && unitController.unit.HasEnchantment(global::EnchantmentId.ITEM_PARRY)));
		}
		if (flag && filterData.HasSpell)
		{
			flag &= ((!reverse && unitController.HasSpells()) || (reverse && !unitController.HasSpells()));
		}
		if (flag && filterData.HealthUnderRatio != 0)
		{
			int num = (int)((float)unitController.unit.CurrentWound / (float)unitController.unit.Wound * 100f);
			flag &= ((!reverse && num <= filterData.HealthUnderRatio) || (reverse && num > filterData.HealthUnderRatio));
		}
		if (flag && filterData.MinRoll != 0)
		{
			flag &= ((!reverse && this.unitCtrlr.CurrentAction.GetRoll(false) >= filterData.MinRoll) || (reverse && this.unitCtrlr.CurrentAction.GetRoll(false) < filterData.MinRoll));
		}
		if (flag && filterData.HasBeenShot)
		{
			flag &= ((!reverse && unitController.beenShot) || (reverse && !unitController.beenShot));
		}
		if (flag && filterData.NoEnemyInSight)
		{
			flag &= ((!reverse && !unitController.HasEnemyInSight()) || (reverse && unitController.HasEnemyInSight()));
		}
		if (flag && filterData.IsPreFight)
		{
			flag &= ((!reverse && this.preFight) || (reverse && !this.preFight));
		}
		if (flag && filterData.EnchantmentTypeIdApplied != global::EnchantmentTypeId.NONE)
		{
			flag &= ((!reverse && unitController.unit.HasEnchantment(filterData.EnchantmentTypeIdApplied)) || (reverse && !unitController.unit.HasEnchantment(filterData.EnchantmentTypeIdApplied)));
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Checking filter ",
			filterData.Name,
			" using ",
			unitController.name,
			" as target. Result is ",
			flag
		}), "AI", null);
		return flag;
	}

	private int GetAttributeVal(global::AttributeId attrId, global::UnitController target)
	{
		switch (attrId)
		{
		case global::AttributeId.COMBAT_MELEE_HIT_ROLL:
			return target.GetMeleeHitRoll(false);
		case global::AttributeId.COMBAT_RANGE_HIT_ROLL:
			return target.GetRangeHitRoll(false);
		}
		return target.unit.GetAttribute(attrId);
	}

	private static bool CheckValue(global::AiFilterCheckId aiFilterCheckId, int checkValue, int attributeVal)
	{
		switch (aiFilterCheckId)
		{
		case global::AiFilterCheckId.EQUAL:
			return attributeVal == checkValue;
		case global::AiFilterCheckId.GREATER:
			return attributeVal > checkValue;
		case global::AiFilterCheckId.LOWER:
			return attributeVal < checkValue;
		case global::AiFilterCheckId.GREATER_EQUAL:
			return attributeVal >= checkValue;
		case global::AiFilterCheckId.LOWER_EQUAL:
			return attributeVal <= checkValue;
		case global::AiFilterCheckId.NOT_EQUAL:
			return attributeVal != checkValue;
		default:
			return false;
		}
	}

	public const int ACTION_MAX_TARGET = 3;

	public const int MIN_ROLL = 50;

	public static global::System.Collections.Generic.List<global::UnitActionId> attackActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.MELEE_ATTACK
	};

	public static global::System.Collections.Generic.List<global::UnitActionId> spellActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.SPELL
	};

	public static global::System.Collections.Generic.List<global::UnitActionId> consSkillActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.CONSUMABLE,
		global::UnitActionId.SKILL
	};

	public static global::System.Collections.Generic.List<global::UnitActionId> consSkillSpellActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.CONSUMABLE,
		global::UnitActionId.SPELL,
		global::UnitActionId.SKILL
	};

	public static global::System.Collections.Generic.List<global::UnitActionId> chargeActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.CHARGE
	};

	public static global::System.Collections.Generic.List<global::UnitActionId> shootActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.SHOOT
	};

	public static global::System.Collections.Generic.List<global::UnitActionId> stanceActions = new global::System.Collections.Generic.List<global::UnitActionId>
	{
		global::UnitActionId.STANCE
	};

	private global::UnitController unitCtrlr;

	public global::AiProfileId aiProfileId;

	private global::AiUnitId previousAIId;

	public global::Pathfinding.Path currentPath;

	public bool movingToSearchPoint;

	public global::SearchPoint targetSearchPoint;

	public global::UnitController targetEnemy;

	public global::DecisionPoint targetDecisionPoint;

	public global::Destructible targetDestructible;

	public int failedMove;

	public int switchCount;

	public int disengageCount;

	public bool atDestination;

	public global::System.Collections.Generic.List<global::SearchPoint> lootedSearchPoints = new global::System.Collections.Generic.List<global::SearchPoint>();

	public global::System.Collections.Generic.List<global::UnitController> reachableUnits = new global::System.Collections.Generic.List<global::UnitController>();

	public global::System.Collections.Generic.Dictionary<global::UnitController, global::Pathfinding.Path> reachableUnitsPaths = new global::System.Collections.Generic.Dictionary<global::UnitController, global::Pathfinding.Path>();

	private int pathIdx;

	private float currentPathLg;

	private global::Pathfinding.Path calculatedPath;

	private global::AIController.BestPathType bestPathType;

	private bool checkDecisionOnCannotReach;

	private bool forceDecisionCheck;

	private bool keepOnlyReachable;

	private bool fallBackToOldPath;

	private global::System.Collections.Generic.List<global::UnityEngine.Transform> targets = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();

	private global::UnityEngine.Transform bestTarget;

	private global::UnityEngine.Events.UnityAction PreCheck;

	private global::UnityEngine.Events.UnityAction<bool> PostCheck;

	private global::UnityEngine.Events.UnityAction PathFound;

	private global::UnityEngine.Events.UnityAction CannotReach;

	private global::UnityEngine.Events.UnityAction<bool> AllChecked;

	private global::System.Collections.Generic.List<global::UnitController> units;

	private global::System.Collections.Generic.List<global::UnitController> excludedUnits = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::SearchPoint> searchPoints;

	private global::System.Collections.Generic.List<global::DecisionPoint> decisionPoints = new global::System.Collections.Generic.List<global::DecisionPoint>();

	private global::System.Collections.Generic.List<global::Destructible> destructibles = new global::System.Collections.Generic.List<global::Destructible>();

	private global::Pathfinding.Path oldPath;

	private float maxDist;

	private global::System.Collections.Generic.List<int> hitRolls = new global::System.Collections.Generic.List<int>();

	private global::System.Collections.Generic.Dictionary<global::SkillId, global::System.Collections.Generic.List<global::UnitController>> skillTargets = new global::System.Collections.Generic.Dictionary<global::SkillId, global::System.Collections.Generic.List<global::UnitController>>();

	private global::System.Collections.Generic.List<global::SkillId> usedSkillTurn = new global::System.Collections.Generic.List<global::SkillId>();

	public bool hasCastSkill;

	public bool preFight;

	public bool hasSeenEnemy;

	private global::System.Collections.Generic.List<global::ActionStatus> allActions = new global::System.Collections.Generic.List<global::ActionStatus>();

	private global::System.Collections.Generic.List<global::UnitController> allTargets = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::ActionStatus> validActions = new global::System.Collections.Generic.List<global::ActionStatus>();

	private global::System.Collections.Generic.List<global::UnitController> validTargets = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::UnitController> tmpTargets = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::UnitController> defendersCopy = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::SkillAiFilterData> excludeFilters = new global::System.Collections.Generic.List<global::SkillAiFilterData>();

	private global::System.Collections.Generic.List<global::SkillAiFilterData> validFilters = new global::System.Collections.Generic.List<global::SkillAiFilterData>();

	private enum BestPathType
	{
		CLOSEST,
		FAREST
	}
}
