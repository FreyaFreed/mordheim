using System;

public class Reload : global::ICheapState
{
	public Reload(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("reload", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	public void ReloadWeapon(int slot)
	{
		if (this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + slot)] != null && this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + slot)].Item.TypeData.IsRange)
		{
			this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + slot)].Reload();
		}
	}

	private void OnSeqDone()
	{
		this.unitCtrlr.nextState = global::UnitController.State.START_MOVE;
	}

	private global::UnitController unitCtrlr;
}
