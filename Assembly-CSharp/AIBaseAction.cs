using System;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using UnityEngine;
using UnityEngine.Events;

public abstract class AIBaseAction : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIBaseAction";
		this.bestAction = this.unitCtrlr.AICtrlr.GetBestAction(this.GetRelatedActions(), out this.refTarget, new global::UnityEngine.Events.UnityAction<global::ActionStatus, global::System.Collections.Generic.List<global::UnitController>>(this.RefineTargets));
		this.success = (this.bestAction != null && this.refTarget != null);
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.success)
		{
			if (global::AIController.consSkillActions.Contains(this.bestAction.skillData.UnitActionId, global::UnitActionIdComparer.Instance))
			{
				this.unitCtrlr.AICtrlr.hasCastSkill = true;
			}
			this.unitCtrlr.FaceTarget(this.refTarget.transform, true);
			switch (this.bestAction.TargetingId)
			{
			case global::TargetingId.SINGLE_TARGET:
				this.unitCtrlr.SendSkillSingleTarget(this.bestAction.SkillId, this.refTarget);
				break;
			case global::TargetingId.LINE:
			{
				global::UnityEngine.Vector3 targetPos;
				global::UnityEngine.Vector3 targetDir;
				global::PandoraSingleton<global::MissionManager>.Instance.InitLineTarget(this.unitCtrlr.transform, (float)this.bestAction.Radius, (float)this.bestAction.RangeMax, out targetPos, out targetDir);
				global::PandoraSingleton<global::MissionManager>.Instance.lineTarget.SetActive(false);
				this.unitCtrlr.SendSkillTargets(this.bestAction.SkillId, targetPos, targetDir);
				break;
			}
			case global::TargetingId.CONE:
			{
				global::UnityEngine.Vector3 targetPos;
				global::UnityEngine.Vector3 targetDir;
				global::PandoraSingleton<global::MissionManager>.Instance.InitConeTarget(this.unitCtrlr.transform, (float)this.bestAction.Radius, (float)this.bestAction.RangeMax, out targetPos, out targetDir);
				global::PandoraSingleton<global::MissionManager>.Instance.coneTarget.SetActive(false);
				this.unitCtrlr.SendSkillTargets(this.bestAction.SkillId, targetPos, targetDir);
				break;
			}
			case global::TargetingId.AREA:
			case global::TargetingId.AREA_GROUND:
			{
				global::UnityEngine.Vector3 targetPos;
				global::UnityEngine.Vector3 targetDir;
				global::PandoraSingleton<global::MissionManager>.Instance.InitSphereTarget(this.unitCtrlr.transform, (float)this.bestAction.Radius, this.bestAction.TargetingId, out targetPos, out targetDir);
				global::PandoraSingleton<global::MissionManager>.Instance.sphereTarget.SetActive(false);
				global::UnityEngine.Vector3 vector = this.refTarget.transform.position;
				if (this.bestAction.skillData.NeedValidGround)
				{
					if (this.refTarget == this.unitCtrlr)
					{
						vector += this.unitCtrlr.transform.forward * (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 2);
					}
					vector = global::PandoraSingleton<global::MissionManager>.Instance.ClampToNavMesh(vector);
				}
				this.unitCtrlr.SendSkillTargets(this.bestAction.SkillId, vector, vector - this.unitCtrlr.transform.position);
				break;
			}
			case global::TargetingId.ARC:
				this.unitCtrlr.SendSkillTargets(this.bestAction.SkillId, this.unitCtrlr.transform.position, this.refTarget.transform.position - this.unitCtrlr.transform.position);
				break;
			}
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}

	private void RefineTargets(global::ActionStatus action, global::System.Collections.Generic.List<global::UnitController> allTargets)
	{
		global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>();
		global::System.Collections.Generic.List<int> list2 = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < allTargets.Count; i++)
		{
			if (this.IsValid(action, allTargets[i]))
			{
				int criteriaValue = this.GetCriteriaValue(allTargets[i]);
				if (this.ByPassLimit(allTargets[i]) || list.Count < 3)
				{
					list.Add(allTargets[i]);
					list2.Add(criteriaValue);
				}
				else
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (this.IsBetter(criteriaValue, list2[j]))
						{
							list.RemoveAt(j);
							list2.RemoveAt(j);
							list.Add(allTargets[i]);
							list2.Add(criteriaValue);
							break;
						}
					}
				}
			}
		}
		allTargets.Clear();
		allTargets.AddRange(list);
	}

	protected void RemoveRandomTargets<U>(global::System.Collections.Generic.List<U> units)
	{
		while (units.Count > 3)
		{
			int index = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, units.Count);
			units.RemoveAt(index);
		}
	}

	protected abstract bool IsValid(global::ActionStatus action, global::UnitController target);

	protected abstract bool ByPassLimit(global::UnitController target);

	protected abstract bool IsBetter(int currentVal, int val);

	protected abstract int GetCriteriaValue(global::UnitController target);

	protected abstract global::System.Collections.Generic.List<global::UnitActionId> GetRelatedActions();

	private global::ActionStatus bestAction;

	private global::UnitController refTarget;
}
