using System;
using System.Collections.Generic;
using RAIN.Core;
using UnityEngine;

public class AIHasEnemiesInMinRange : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIHasEnemiesInMinRange";
		int num = 0;
		if (this.unitCtrlr.unit.Items[(int)this.unitCtrlr.unit.ActiveWeaponSlot].RangeMin != 0)
		{
			num = this.unitCtrlr.unit.Items[(int)this.unitCtrlr.unit.ActiveWeaponSlot].RangeMin;
		}
		else if (this.unitCtrlr.unit.Items[(int)this.unitCtrlr.unit.InactiveWeaponSlot].RangeMin != 0)
		{
			num = this.unitCtrlr.unit.Items[(int)this.unitCtrlr.unit.InactiveWeaponSlot].RangeMin;
		}
		this.success = false;
		if (num != 0)
		{
			global::UnityEngine.Vector3 position = this.unitCtrlr.transform.position;
			global::System.Collections.Generic.List<global::UnitController> spottedEnemies = this.unitCtrlr.GetWarband().SquadManager.GetSpottedEnemies();
			for (int i = 0; i < spottedEnemies.Count; i++)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(spottedEnemies[i].transform.position - position) <= (float)(num * num))
				{
					this.success = true;
					return;
				}
			}
		}
	}
}
