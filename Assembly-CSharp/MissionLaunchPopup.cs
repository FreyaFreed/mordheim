using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionLaunchPopup : global::UIPopupModule
{
	public void Setup(global::Mission miss)
	{
		if (miss.missionSave.isCampaign)
		{
			global::System.Collections.Generic.List<global::CampaignMissionData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(new string[]
			{
				"fk_warband_id",
				"idx"
			}, new string[]
			{
				((int)global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id).ToString(),
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx.ToString()
			});
			this.missionName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_title_" + list[0].Name);
			this.description.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_announcement_short_" + list[0].Name);
			this.actText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("act_" + global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx);
			this.resources.SetActive(false);
		}
		else
		{
			this.missionName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + miss.GetDeploymentScenarioId().ToLowerString());
			this.actText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + ((global::PrimaryObjectiveTypeId)miss.missionSave.objectiveTypeIds[0]).ToString());
			this.resources.SetActive(true);
			this.description.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_desc_" + miss.GetDeploymentScenarioId().ToLowerString());
		}
		this.difficulty.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_difficulty_" + ((global::PrimaryObjectiveId)miss.missionSave.ratingId).ToString());
		this.SetupShort(miss);
	}

	public void SetupShort(global::Mission miss)
	{
		this.diffIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_mission_difficulty_" + ((global::PrimaryObjectiveId)miss.missionSave.ratingId).ToString(), true);
		this.wyrdstones.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("yield_" + ((global::WyrdstoneDensityId)miss.missionSave.wyrdDensityId).ToString());
		this.treasures.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("yield_" + ((global::SearchDensityId)miss.missionSave.searchDensityId).ToString());
		this.rating.text = miss.missionSave.rating.ToString();
	}

	public global::UnityEngine.UI.Text missionName;

	public global::UnityEngine.UI.Text wyrdstones;

	public global::UnityEngine.UI.Text treasures;

	public global::UnityEngine.UI.Text description;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.UI.Text actText;

	public global::UnityEngine.UI.Text difficulty;

	public global::UnityEngine.UI.Image diffIcon;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.GameObject resources;
}
