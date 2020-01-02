using System;
using UnityEngine.UI;

public class WarbandOverviewUnitsModule : global::UIModule
{
	public void Set(global::Warband warband)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 0; i < warband.Units.Count; i++)
		{
			switch (warband.Units[i].GetActiveStatus())
			{
			case global::UnitActiveStatusId.AVAILABLE:
				num++;
				if (warband.Units[i].IsLeader)
				{
					num6++;
				}
				break;
			case global::UnitActiveStatusId.INJURED:
				num4++;
				break;
			case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
				num2++;
				break;
			case global::UnitActiveStatusId.IN_TRAINING:
				num3++;
				break;
			case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
				num5++;
				break;
			case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
				num2++;
				num4++;
				break;
			default:
				global::PandoraDebug.LogWarning("Unknown status in WarbandUnitsHistoryModule.Set", "UI", this);
				break;
			}
		}
		if (num < global::Constant.GetInt(global::ConstantId.MIN_MISSION_UNITS))
		{
			this.message.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_msg_mission_warning_not_enough_units");
			this.message.gameObject.SetActive(true);
		}
		else
		{
			this.message.gameObject.SetActive(false);
		}
		this.activeUnits.text = num.ToString();
		this.unpaidUnits.text = num2.ToString();
		this.learningUnits.text = num3.ToString();
		this.inTreatment.text = num4.ToString();
		this.injuredUnits.text = num5.ToString();
		this.leadersAvailable.text = num6.ToString();
	}

	public global::UnityEngine.UI.Text message;

	public global::UnityEngine.UI.Text activeUnits;

	public global::UnityEngine.UI.Text unpaidUnits;

	public global::UnityEngine.UI.Text learningUnits;

	public global::UnityEngine.UI.Text inTreatment;

	public global::UnityEngine.UI.Text injuredUnits;

	public global::UnityEngine.UI.Text leadersAvailable;
}
