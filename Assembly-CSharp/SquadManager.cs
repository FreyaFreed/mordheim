using System;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager
{
	public SquadManager(global::WarbandController warbandController)
	{
		this.warCtrlr = warbandController;
		this.squads = new global::System.Collections.Generic.List<global::Squad>();
	}

	public void FormSquads()
	{
		global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>(this.warCtrlr.unitCtrlrs);
		global::UnitController unitController = this.warCtrlr.GetLeader();
		unitController = ((!(unitController == null)) ? unitController : this.warCtrlr.unitCtrlrs[0]);
		this.FormSquad(unitController, list);
		while (list.Count > 0)
		{
			this.FormSquad(this.UnitWithClosestAlly(list), list);
		}
	}

	private void FormSquad(global::UnitController squadLeader, global::System.Collections.Generic.List<global::UnitController> allUnits)
	{
		global::Squad squad = new global::Squad();
		int count = this.squads.Count;
		squad.members.Add(squadLeader);
		squadLeader.AICtrlr.SetSquad(squad, count);
		allUnits.Remove(squadLeader);
		int num = squad.members.Count;
		while (num < 3 && allUnits.Count > 0)
		{
			int num2 = -1;
			float num3 = float.MaxValue;
			for (int i = 0; i < allUnits.Count; i++)
			{
				float num4 = global::UnityEngine.Vector3.SqrMagnitude(squad.members[0].transform.position - allUnits[i].transform.position);
				if (num4 < num3)
				{
					num3 = num4;
					num2 = i;
				}
			}
			if (num2 != -1 && num3 < 900f)
			{
				squad.members.Add(allUnits[num2]);
				allUnits[num2].AICtrlr.SetSquad(squad, count);
				allUnits.RemoveAt(num2);
			}
			num++;
		}
		this.squads.Add(squad);
	}

	public void RefreshSquads()
	{
		for (int i = this.squads.Count - 1; i >= 0; i--)
		{
			this.squads[i].RemoveDeadMembers();
			if (this.squads[i].members.Count == 0)
			{
				this.squads.RemoveAt(i);
			}
		}
		global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>();
		for (int j = 0; j < this.squads.Count; j++)
		{
			if (this.squads[j].LoneLostLastMember())
			{
				list.Add(this.squads[j].members[0]);
				this.squads.RemoveAt(j);
			}
		}
		for (int k = 0; k < this.warCtrlr.unitCtrlrs.Count; k++)
		{
			if (this.warCtrlr.unitCtrlrs[k].AICtrlr.Squad == null)
			{
				list.Add(this.warCtrlr.unitCtrlrs[k]);
			}
		}
		if (this.squads.Count == 0)
		{
			this.squads.Add(new global::Squad());
		}
		for (int l = 0; l < list.Count; l++)
		{
			global::Squad squad = this.squads[0];
			float num = float.MaxValue;
			for (int m = 0; m < this.squads.Count; m++)
			{
				for (int n = 0; n < this.squads[m].members.Count; n++)
				{
					float num2 = global::UnityEngine.Vector3.SqrMagnitude(this.squads[m].members[n].transform.position - list[l].transform.position);
					if (num2 < num)
					{
						num = num2;
						squad = this.squads[m];
					}
				}
			}
			list[l].AICtrlr.SetSquad(squad, this.squads.IndexOf(squad));
			squad.members.Add(list[l]);
		}
	}

	private global::UnitController UnitWithClosestAlly(global::System.Collections.Generic.List<global::UnitController> allUnits)
	{
		global::UnitController result = allUnits[0];
		float num = float.MaxValue;
		for (int i = 0; i < allUnits.Count - 1; i++)
		{
			for (int j = i + 1; j < allUnits.Count; j++)
			{
				float num2 = global::UnityEngine.Vector3.SqrMagnitude(allUnits[i].transform.position - allUnits[j].transform.position);
				if (num2 < num)
				{
					num = num2;
					result = allUnits[i];
				}
			}
		}
		return result;
	}

	public void UnitSpotted(global::UnitController ctrlr)
	{
		for (int i = 0; i < this.spottedUnitsData.Count; i++)
		{
			if (this.spottedUnitsData[i].Key == ctrlr)
			{
				this.spottedUnitsData[i] = new global::System.Collections.Generic.KeyValuePair<global::UnitController, int>(ctrlr, global::PandoraSingleton<global::MissionManager>.Instance.currentTurn);
				return;
			}
		}
		this.spottedUnitsData.Add(new global::System.Collections.Generic.KeyValuePair<global::UnitController, int>(ctrlr, global::PandoraSingleton<global::MissionManager>.Instance.currentTurn));
	}

	public global::System.Collections.Generic.List<global::UnitController> GetSpottedEnemies()
	{
		this.spottedEnemies.Clear();
		for (int i = 0; i < this.spottedUnitsData.Count; i++)
		{
			if (this.spottedUnitsData[i].Key.unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				this.spottedEnemies.Add(this.spottedUnitsData[i].Key);
			}
		}
		return this.spottedEnemies;
	}

	private const int MEMBERS_COUNT = 3;

	private const float MIN_MEMBERS_SQR_DIST = 900f;

	public const int SPOTTED_TURN_LIMIT = 2;

	public global::System.Collections.Generic.List<global::Squad> squads;

	private global::WarbandController warCtrlr;

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::UnitController, int>> spottedUnitsData = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::UnitController, int>>();

	private global::System.Collections.Generic.List<global::UnitController> spottedEnemies = new global::System.Collections.Generic.List<global::UnitController>();
}
