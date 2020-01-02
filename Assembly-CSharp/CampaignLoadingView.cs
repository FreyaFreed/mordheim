using System;
using UnityEngine;
using UnityEngine.UI;

public class CampaignLoadingView : global::LoadingView
{
	public override void Show()
	{
		base.Show();
		global::WarbandSave currentSave = global::PandoraSingleton<global::GameManager>.Instance.currentSave;
		string str = ((global::WarbandId)currentSave.id).ToString();
		this.warbandName.text = currentSave.Name;
		this.flag.sprite = global::Warband.GetIcon((global::WarbandId)currentSave.id);
		this.rankText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_rank_colon_value", new string[]
		{
			currentSave.rank.ToString()
		});
		base.LoadBackground("bg_warband_load_" + str);
		global::Date date = new global::Date(global::Constant.GetInt(global::ConstantId.CAL_DAY_START));
		this.warbandCreateDate.text = date.ToLocalizedAbbrString();
		this.warbandDaysActive.text = (currentSave.currentDate - global::Constant.GetInt(global::ConstantId.CAL_DAY_START)).ToString();
		int num = currentSave.stats.stats[39];
		this.missionsAttempted.text = num.ToString();
		int num2 = currentSave.stats.stats[24];
		this.missionSuccessRate.text = ((float)num2 / (float)global::UnityEngine.Mathf.Max(1, num)).ToString("00%");
		this.missionCrushingVictories.text = currentSave.stats.stats[45].ToString();
		this.missionTotalVictories.text = currentSave.stats.stats[44].ToString();
		num = currentSave.stats.stats[40];
		this.skirmishesAttempted.text = num.ToString();
		num2 = num - currentSave.stats.stats[28];
		this.skirmishSuccessRate.text = ((float)num2 / (float)global::UnityEngine.Mathf.Max(1, num)).ToString("00%");
		this.skirmishDecisiveVictories.text = currentSave.stats.stats[43].ToString();
		this.skirmishObjectiveVictories.text = currentSave.stats.stats[26].ToString();
		this.skirmishBattlegroundVictories.text = currentSave.stats.stats[27].ToString();
		int b = currentSave.stats.stats[32];
		this.ooaAllies.text = b.ToString();
		int num3 = currentSave.stats.stats[31];
		this.ooaEnemies.text = num3.ToString();
		this.outOfActionRatio.text = ((float)num3 / (float)global::UnityEngine.Mathf.Max(1, b)).ToString("00%");
		this.damageDealt.text = currentSave.stats.stats[42].ToString();
		this.allTimeGold.text = currentSave.stats.stats[59].ToString();
		this.allTimeWyrdFragments.text = currentSave.stats.stats[35].ToString();
		this.allTimeWyrdShards.text = currentSave.stats.stats[36].ToString();
		this.allTimeWyrdClusters.text = currentSave.stats.stats[37].ToString();
	}

	public global::UnityEngine.UI.Text warbandName;

	public global::UnityEngine.UI.Text rankText;

	public global::UnityEngine.UI.Text ratingText;

	public global::UnityEngine.UI.Image flag;

	[global::UnityEngine.Header("Stats")]
	public global::UnityEngine.UI.Text warbandCreateDate;

	public global::UnityEngine.UI.Text warbandDaysActive;

	public global::UnityEngine.UI.Text missionsAttempted;

	public global::UnityEngine.UI.Text missionSuccessRate;

	public global::UnityEngine.UI.Text missionCrushingVictories;

	public global::UnityEngine.UI.Text missionTotalVictories;

	public global::UnityEngine.UI.Text skirmishesAttempted;

	public global::UnityEngine.UI.Text skirmishSuccessRate;

	public global::UnityEngine.UI.Text skirmishDecisiveVictories;

	public global::UnityEngine.UI.Text skirmishObjectiveVictories;

	public global::UnityEngine.UI.Text skirmishBattlegroundVictories;

	public global::UnityEngine.UI.Text ooaAllies;

	public global::UnityEngine.UI.Text ooaEnemies;

	public global::UnityEngine.UI.Text outOfActionRatio;

	public global::UnityEngine.UI.Text damageDealt;

	public global::UnityEngine.UI.Text allTimeGold;

	public global::UnityEngine.UI.Text allTimeWyrdFragments;

	public global::UnityEngine.UI.Text allTimeWyrdShards;

	public global::UnityEngine.UI.Text allTimeWyrdClusters;
}
