using System;
using System.Collections.Generic;
using RAIN.Core;
using UnityEngine;

public class AIUseBuff : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "AIUseBuff";
		this.unitCtrlr.UpdateActionStatus(false, global::UnitActionRefreshId.NONE);
		this.buffs.Clear();
		for (int i = 0; i < this.unitCtrlr.actionStatus.Count; i++)
		{
			if (this.unitCtrlr.actionStatus[i].Available && this.unitCtrlr.actionStatus[i].skillData.EffectTypeId == global::EffectTypeId.BUFF && (this.unitCtrlr.actionStatus[i].ActionId == global::UnitActionId.SKILL || this.unitCtrlr.actionStatus[i].ActionId == global::UnitActionId.SPELL))
			{
				this.buffs.Add(this.unitCtrlr.actionStatus[i]);
			}
		}
		global::System.Collections.Generic.List<global::UnitController> aliveAllies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveAllies(this.unitCtrlr.GetWarband().idx);
		for (int j = this.buffs.Count - 1; j >= 0; j--)
		{
			if (this.buffs[j].TargetingId != global::TargetingId.SINGLE_TARGET)
			{
				bool flag = false;
				int num = 0;
				while (!flag && num < aliveAllies.Count)
				{
					float num2 = (float)(this.buffs[j].RangeMax + this.buffs[j].Radius);
					num2 *= num2;
					if (global::UnityEngine.Vector3.SqrMagnitude(aliveAllies[num].transform.position - this.unitCtrlr.transform.position) < num2)
					{
						flag = true;
					}
					num++;
				}
				if (!flag)
				{
					this.buffs.RemoveAt(j);
				}
			}
		}
	}

	private global::System.Collections.Generic.List<global::ActionStatus> buffs = new global::System.Collections.Generic.List<global::ActionStatus>();
}
