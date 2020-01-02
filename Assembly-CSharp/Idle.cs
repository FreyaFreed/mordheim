using System;
using UnityEngine;

public class Idle : global::ICheapState
{
	public Idle(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("Idle Enter ", "UNIT_FLOW", this.unitCtrlr);
		this.unitCtrlr.SetFixed(true);
		if (this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			this.unitCtrlr.GetComponent<global::UnityEngine.Collider>().enabled = true;
		}
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
