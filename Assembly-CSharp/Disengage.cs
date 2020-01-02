using System;

public class Disengage : global::ICheapState
{
	public Disengage(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.SetFleeTarget();
		this.unitCtrlr.FaceTarget(this.unitCtrlr.FleeTarget, true);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("disengage", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	public void OnSeqDone()
	{
		this.unitCtrlr.Ground();
		this.unitCtrlr.CheckEngaged(true);
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			this.unitCtrlr.SendStartMove(this.unitCtrlr.transform.position, this.unitCtrlr.transform.rotation);
		}
	}

	private global::UnitController unitCtrlr;
}
