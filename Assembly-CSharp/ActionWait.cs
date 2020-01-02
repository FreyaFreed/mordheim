using System;

public class ActionWait : global::ICheapState
{
	public ActionWait(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("ActionWait Enter ", "UNIT_FLOW", this.unitCtrlr);
		this.unitCtrlr.SetFixed(true);
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

	private global::UnitController unitCtrlr;
}
