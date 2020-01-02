using System;
using System.Collections;

public class Activate : global::ICheapState
{
	public Activate(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		if (this.unitCtrlr.interactivePoint.Highlight != null)
		{
			this.unitCtrlr.FaceTarget(this.unitCtrlr.interactivePoint.Highlight.transform, true);
		}
		else
		{
			this.unitCtrlr.FaceTarget(this.unitCtrlr.interactivePoint.transform, true);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(this.unitCtrlr.interactivePoint.transform.parent);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		this.unitCtrlr.searchVariation = (int)this.unitCtrlr.interactivePoint.anim;
		this.unitCtrlr.GetWarband().Activate(this.unitCtrlr.interactivePoint.name);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("search_start", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	public void ActivatePoint()
	{
		((global::ActivatePoint)this.unitCtrlr.interactivePoint).Activate(this.unitCtrlr, false);
		this.unitCtrlr.LaunchAction(global::UnitActionId.ACTIVATE, true, this.unitCtrlr.unit.Status, 2);
	}

	private void OnSeqDone()
	{
		this.unitCtrlr.StartCoroutine(this.WaitForAnims());
	}

	private global::System.Collections.IEnumerator WaitForAnims()
	{
		while (((global::ActivatePoint)this.unitCtrlr.interactivePoint).IsAnimationPlaying())
		{
			yield return null;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.RefreshGraph();
		global::ActivatePoint point = (global::ActivatePoint)this.unitCtrlr.interactivePoint;
		if (point.consumeRequestedItem)
		{
			this.unitCtrlr.unit.ConsumeItem(point.requestedItemId);
		}
		point.SpawnCampaignUnit();
		point.ActivateZoneAoe();
		if (!global::PandoraSingleton<global::MissionManager>.Instance.CheckEndGame())
		{
			if (point.curseId != global::SkillId.NONE)
			{
				this.unitCtrlr.currentSpellTypeId = global::SpellTypeId.MISSION;
				this.unitCtrlr.currentCurseSkillId = point.curseId;
				this.unitCtrlr.StateMachine.ChangeState(29);
			}
			else
			{
				this.unitCtrlr.StateMachine.ChangeState(10);
			}
		}
		yield break;
	}

	private global::UnitController unitCtrlr;
}
