using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionHeaderView : global::UnityEngine.MonoBehaviour
{
	public void Init(global::Mission mission, global::CampaignMissionData missionData)
	{
		this._mission = mission;
		this._missionData = missionData;
	}

	public void OnCampaign()
	{
		this.wyrdstoneGroup.gameObject.SetActive(false);
		this.searchGroup.gameObject.SetActive(false);
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_title_" + this._missionData.Name);
		this.txtAct.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_act_" + this._missionData.Name.Substring(this._missionData.Name.LastIndexOf('_') - 1));
		this.SetDifficulty();
		this.SetMission();
	}

	public void OnTutorial()
	{
		this.difficultyGroup.gameObject.SetActive(false);
		this.txtAct.enabled = false;
		this.missionGroup.gameObject.SetActive(false);
		this.wyrdstoneGroup.gameObject.SetActive(false);
		this.searchGroup.gameObject.SetActive(false);
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this._missionData.Name + "_title");
	}

	public void OnMission()
	{
		this.txtAct.enabled = false;
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("district_" + this._mission.GetDistrictId().ToLowerString());
		this.SetDifficulty();
		this.SetMission();
		this.searchGroup.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("yield_" + ((global::SearchDensityId)this._mission.missionSave.searchDensityId).ToLowerString());
		this.wyrdstoneGroup.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("yield_" + ((global::WyrdstoneDensityId)this._mission.missionSave.wyrdDensityId).ToLowerString());
	}

	public void OnSkirmish()
	{
		this.txtAct.enabled = false;
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("district_" + this._mission.GetDistrictId().ToLowerString());
		this.searchGroup.gameObject.SetActive(false);
		this.missionGroup.gameObject.SetActive(false);
		this.wyrdstoneGroup.gameObject.SetActive(false);
	}

	private void SetMission()
	{
		this.missionGroup.gameObject.SetActive(true);
		if (this._mission.missionSave.ratingId != 0)
		{
			this.missionGroup.text.text = ((global::ProcMissionRatingId)this._mission.missionSave.rating).ToString();
		}
		else
		{
			this.missionGroup.text.text = string.Empty;
		}
	}

	private void SetDifficulty()
	{
		this.difficultyGroup.gameObject.SetActive(false);
		if (this._mission.missionSave.ratingId != 0)
		{
			this.difficultyGroup.buttonImage.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_mission_difficulty_" + ((global::ProcMissionRatingId)this._mission.missionSave.ratingId).ToLowerString(), true);
		}
	}

	public global::ButtonMapView difficultyGroup;

	public global::UnityEngine.UI.Text txtAct;

	public global::UnityEngine.UI.Text title;

	public global::ButtonMapView missionGroup;

	public global::ButtonMapView wyrdstoneGroup;

	public global::ButtonMapView searchGroup;

	private global::Mission _mission;

	private global::CampaignMissionData _missionData;
}
