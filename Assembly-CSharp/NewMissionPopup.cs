using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMissionPopup : global::ConfirmationPopupView
{
	public void Setup(global::System.Action<bool> callback, bool hideButtons = false)
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
		string titleId = "mission_camp_title_" + list[0].Name;
		string textId = "mission_camp_announcement_" + list[0].Name;
		base.Show(titleId, textId, callback, hideButtons, false);
		global::Mission mission = global::PandoraSingleton<global::HideoutManager>.Instance.missions[0];
		mission.RefreshDifficulty(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetRating(), false);
		this.icon.sprite = global::Warband.GetIcon(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id);
		this.rating.text = mission.missionSave.rating.ToString();
		this.difficulty.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_difficulty_" + ((global::ProcMissionRatingId)mission.missionSave.ratingId).ToLowerString());
		this.diffIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_mission_difficulty_" + ((global::ProcMissionRatingId)mission.missionSave.ratingId).ToLowerString(), true);
		this.act.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("act_" + global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx);
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.UI.Text act;

	public global::UnityEngine.UI.Text difficulty;

	public global::UnityEngine.UI.Image diffIcon;
}
