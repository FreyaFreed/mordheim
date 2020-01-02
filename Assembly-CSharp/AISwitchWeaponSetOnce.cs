using System;
using RAIN.Core;

public class AISwitchWeaponSetOnce : global::AISwitchWeaponSet
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "SwitchWeaponSetOnce";
		this.success &= (this.unitCtrlr.AICtrlr.switchCount == 0);
	}
}
