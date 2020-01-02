using System;
using UnityEngine.UI;

public class MissionReportLoadingView : global::LoadingView
{
	public override void Show()
	{
		base.Show();
		this.missionEndData = global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission;
		this.mission = new global::Mission(this.missionEndData.missionSave);
		this.missionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(this.mission.missionSave.campaignId);
		this.missionHeaderView.Init(this.mission, this.missionData);
		this.descriptionTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_victory_" + this.missionEndData.VictoryType.ToLowerString());
		if (this.mission.missionSave.isCampaign)
		{
			this.OnCampaign();
		}
		else
		{
			this.OnMission();
		}
	}

	public void OnCampaign()
	{
		this.missionHeaderView.OnCampaign();
		if (this.missionEndData.primaryObjectiveCompleted)
		{
			this.descriptionDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_epilogue_" + this.missionData.Name);
			base.LoadDialog("mission_camp_epilogue_" + this.missionData.Name);
		}
		else
		{
			this.descriptionDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_defeated_" + ((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString());
			base.LoadDialog("mission_defeated_" + ((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString());
		}
		this.SetFlag();
		base.LoadBackground(string.Concat(new object[]
		{
			"bg_end_",
			this.missionData.Name,
			"_",
			global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.missionData.LoadingImageCount)
		}));
	}

	public void OnMission()
	{
		this.missionHeaderView.OnMission();
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.mission.missionSave.deployScenarioMapLayoutId);
		global::MissionMapData missionMapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)deploymentScenarioMapLayoutData.MissionMapId);
		if (this.missionEndData.VictoryType != global::VictoryTypeId.LOSS)
		{
			string text = string.Concat(new object[]
			{
				"mission_victory_",
				((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString(),
				"_",
				global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 4)
			});
			this.descriptionDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(text);
			base.LoadDialog(text);
			int exclusiveBound = 4;
			if (missionMapData.Name.Contains("01_proc"))
			{
				base.LoadBackground(string.Concat(new object[]
				{
					"bg_win_",
					((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString(),
					"_dis_01_",
					global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, exclusiveBound)
				}));
			}
			else if (missionMapData.Name.Contains("02_proc"))
			{
				base.LoadBackground(string.Concat(new object[]
				{
					"bg_win_",
					((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString(),
					"_dis_02_",
					global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, exclusiveBound)
				}));
			}
			else
			{
				base.LoadBackground(string.Concat(new object[]
				{
					"bg_win_",
					((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString(),
					"_dis_01_",
					global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, exclusiveBound)
				}));
			}
		}
		else
		{
			string text2 = string.Concat(new object[]
			{
				"mission_defeated_",
				((global::WarbandId)global::PandoraSingleton<global::GameManager>.Instance.currentSave.id).ToLowerString(),
				"_",
				global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 4)
			});
			this.descriptionDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(text2);
			base.LoadDialog(text2);
			int exclusiveBound2 = 8;
			base.LoadBackground("bg_lose_" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, exclusiveBound2));
		}
		this.SetFlag();
	}

	private void SetFlag()
	{
		this.flag.gameObject.SetActive(false);
	}

	public global::MissionHeaderView missionHeaderView;

	public global::UnityEngine.UI.Text descriptionTitle;

	public global::UnityEngine.UI.Text descriptionDesc;

	public global::UnityEngine.UI.Image flag;

	private global::Mission mission;

	private global::CampaignMissionData missionData;

	private global::MissionEndDataSave missionEndData;
}
