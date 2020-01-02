using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionLoadingView : global::LoadingView
{
	public override void Show()
	{
		base.Show();
		this.mission = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission;
		this.missionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(this.mission.missionSave.campaignId);
		this.missionHeaderView.Init(this.mission, this.missionData);
		int num = 0;
		for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count; i++)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[i].PlayerTypeId == global::PlayerTypeId.PLAYER)
			{
				num++;
			}
		}
		if (this.mission.missionSave.isTuto)
		{
			this.OnTutorial();
		}
		else if (this.mission.missionSave.isCampaign)
		{
			this.OnCampaign();
		}
		else if (this.mission.missionSave.isSkirmish || num > 1)
		{
			this.OnSkirmish();
		}
		else
		{
			this.OnMission();
		}
	}

	public void OnCampaign()
	{
		this.missionHeaderView.OnCampaign();
		global::System.Collections.Generic.List<global::Objective> objectives = new global::System.Collections.Generic.List<global::Objective>();
		global::UIObjectivesController component = this.objectivesView.GetComponent<global::UIObjectivesController>();
		component.SetObjectives(objectives, true);
		this.objectivesView.gameObject.SetActive(false);
		this.deploymentTitle.gameObject.SetActive(false);
		this.deploymentDesc.gameObject.SetActive(false);
		this.ambush.SetActive(false);
		this.ambusher.gameObject.SetActive(false);
		this.ambushed.gameObject.SetActive(false);
		this.skirmishView.SetActive(false);
		this.descriptionTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_title_" + this.missionData.Name);
		this.descriptionDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_camp_prologue_" + this.missionData.Name);
		base.LoadBackground(string.Concat(new object[]
		{
			"bg_",
			this.missionData.Name,
			"_",
			global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.missionData.LoadingImageCount)
		}));
		base.LoadDialog("mission_camp_prologue_" + this.missionData.Name);
	}

	public void OnTutorial()
	{
		this.missionHeaderView.OnTutorial();
		global::System.Collections.Generic.List<global::Objective> objectives = new global::System.Collections.Generic.List<global::Objective>();
		global::UIObjectivesController component = this.objectivesView.GetComponent<global::UIObjectivesController>();
		component.SetObjectives(objectives, true);
		this.objectivesView.gameObject.SetActive(false);
		this.deploymentTitle.gameObject.SetActive(false);
		this.deploymentDesc.gameObject.SetActive(false);
		this.ambush.SetActive(false);
		this.ambusher.gameObject.SetActive(false);
		this.ambushed.gameObject.SetActive(false);
		this.skirmishView.SetActive(false);
		this.descriptionTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.missionData.Name + "_title");
		this.descriptionDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.missionData.Name + "_desc");
		base.LoadBackground(string.Concat(new object[]
		{
			"bg_",
			this.missionData.Name,
			"_",
			global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.missionData.LoadingImageCount)
		}));
		base.LoadDialog(this.missionData.Name + "_desc");
	}

	public void OnMission()
	{
		this.missionHeaderView.OnMission();
		this.descriptionTitle.gameObject.SetActive(false);
		this.descriptionDesc.gameObject.SetActive(false);
		this.ambush.SetActive(false);
		this.ambusher.gameObject.SetActive(false);
		this.ambushed.gameObject.SetActive(false);
		this.skirmishView.SetActive(false);
		int index = 0;
		if (!global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			index = 1;
		}
		global::DeploymentScenarioSlotData deploymentScenarioSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>(this.mission.missionSave.deployScenarioSlotIds[index]);
		this.deploymentTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(deploymentScenarioSlotData.Title);
		this.deploymentDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(deploymentScenarioSlotData.Setup);
		if (!this.mission.missionSave.isSkirmish)
		{
			if (deploymentScenarioSlotData.Setup.Contains("ambush_1") || deploymentScenarioSlotData.Setup.Contains("scouting_wagon_1"))
			{
				this.ambush.SetActive(true);
				this.ambusher.gameObject.SetActive(true);
				this.ambush.GetComponentInChildren<global::UnityEngine.UI.Text>().text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_ambusher_desc");
				base.LoadDialog("mission_ambusher_desc");
			}
			if (deploymentScenarioSlotData.Setup.Contains("ambush_2") || deploymentScenarioSlotData.Setup.Contains("scouting_wagon_2"))
			{
				this.ambush.SetActive(true);
				this.ambushed.gameObject.SetActive(true);
				this.ambush.GetComponentInChildren<global::UnityEngine.UI.Text>().text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_ambushed_desc");
				base.LoadDialog("mission_ambushed_desc");
			}
		}
		if (this.audioSrc.clip == null)
		{
			base.LoadDialog(deploymentScenarioSlotData.Setup);
		}
		this.objectivesView.SetActive(true);
		global::UIObjectivesController component = this.objectivesView.GetComponent<global::UIObjectivesController>();
		global::System.Collections.Generic.List<global::Objective> objectives = new global::System.Collections.Generic.List<global::Objective>();
		global::MissionStartData instance = global::PandoraSingleton<global::MissionStartData>.Instance;
		int warbandIndex = global::PandoraSingleton<global::MissionStartData>.Instance.GetWarbandIndex(global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex);
		global::Objective.CreateLoadingObjective(ref objectives, (global::PrimaryObjectiveTypeId)instance.CurrentMission.missionSave.objectiveTypeIds[warbandIndex], instance.FightingWarbands[instance.CurrentMission.missionSave.objectiveTargets[warbandIndex]], instance.CurrentMission.missionSave.objectiveSeeds[warbandIndex]);
		component.SetObjectives(objectives, true);
		global::UnityEngine.UI.Image[] componentsInChildren = this.objectivesView.GetComponentsInChildren<global::UnityEngine.UI.Image>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.name != "tab_cat_line")
			{
				componentsInChildren[i].sprite = this.bulletPoint;
				global::UnityEngine.Vector3 localScale = componentsInChildren[i].transform.localScale;
				localScale.x = 0.8f;
				localScale.y = 0.8f;
				componentsInChildren[i].transform.localScale = localScale;
			}
		}
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.mission.missionSave.deployScenarioMapLayoutId);
		global::MissionMapData missionMapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)deploymentScenarioMapLayoutData.MissionMapId);
		if (missionMapData.Name.Contains("01_proc"))
		{
			base.LoadBackground("bg_grnd_dis_01_" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, missionMapData.LoadingImageCount));
		}
		else if (missionMapData.Name.Contains("02_proc"))
		{
			base.LoadBackground("bg_grnd_dis_02_" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, missionMapData.LoadingImageCount));
		}
		else
		{
			base.LoadBackground(string.Concat(new object[]
			{
				"bg_",
				missionMapData.Name,
				"_",
				global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, missionMapData.LoadingImageCount)
			}));
		}
	}

	public void OnSkirmish()
	{
		this.missionHeaderView.OnSkirmish();
		this.descriptionTitle.gameObject.SetActive(false);
		this.descriptionDesc.gameObject.SetActive(false);
		this.ambush.SetActive(false);
		this.ambusher.gameObject.SetActive(false);
		this.ambushed.gameObject.SetActive(false);
		this.deploymentTitle.gameObject.SetActive(false);
		this.deploymentDesc.gameObject.SetActive(false);
		global::UIObjectivesController component = this.objectivesView.GetComponent<global::UIObjectivesController>();
		component.SetObjectives(null, true);
		this.objectivesView.SetActive(false);
		this.skirmishView.SetActive(true);
		this.campaignDescription.gameObject.SetActive(false);
		int num = 0;
		int num2 = 1;
		if (!global::PandoraSingleton<global::Hermes>.Instance.IsHost())
		{
			num2 = 0;
			num = 1;
		}
		if (num < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count)
		{
			global::MissionWarbandSave missionWarbandSave = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[num];
			this.warbandName.text = missionWarbandSave.Name;
			this.warbandRank.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_rank_colon_value", new string[]
			{
				missionWarbandSave.Rank.ToString()
			});
			this.warbandRating.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_rating_value", new string[]
			{
				missionWarbandSave.Rating.ToString()
			});
			this.warbandFlag.sprite = global::Warband.GetIcon(missionWarbandSave.WarbandId);
		}
		if (num2 < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count)
		{
			global::MissionWarbandSave missionWarbandSave2 = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[num2];
			this.opponentWarbandName.text = missionWarbandSave2.Name;
			this.opponentWarbandRank.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_rank_colon_value", new string[]
			{
				missionWarbandSave2.Rank.ToString()
			});
			this.opponentWarbandRating.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_rating_value", new string[]
			{
				missionWarbandSave2.Rating.ToString()
			});
			this.opponentWarbandFlag.sprite = global::Warband.GetIcon(missionWarbandSave2.WarbandId);
		}
		else
		{
			this.opponentWarbandName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_ai");
			this.opponentWarbandRank.text = "0";
			this.opponentWarbandRating.text = "0";
			this.opponentWarbandFlag.sprite = null;
		}
		this.gameplayType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + ((global::MissionMapGameplayId)this.mission.missionSave.mapGameplayId).ToLowerString());
		this.gameplayTypeDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_desc_" + ((global::MissionMapGameplayId)this.mission.missionSave.mapGameplayId).ToLowerString());
		global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.mission.missionSave.deployScenarioMapLayoutId);
		this.deploymentType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + deploymentScenarioMapLayoutData.DeploymentScenarioId.ToLowerString());
		this.deploymentTypeDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_desc_" + deploymentScenarioMapLayoutData.DeploymentScenarioId.ToLowerString());
		this.optionalObjectivePlayerName.text = global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName();
		this.optionalObjective.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + ((global::PrimaryObjectiveTypeId)this.mission.missionSave.objectiveTypeIds[num]).ToLowerString());
		this.optionalObjectiveOpponentName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_opponent");
		this.optionalObjectiveOpponent.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + ((global::PrimaryObjectiveTypeId)this.mission.missionSave.objectiveTypeIds[num2]).ToLowerString());
		global::MissionMapData missionMapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)deploymentScenarioMapLayoutData.MissionMapId);
		if (missionMapData.Name.Contains("01_proc"))
		{
			base.LoadBackground("bg_grnd_dis_01_" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, missionMapData.LoadingImageCount));
		}
		else if (missionMapData.Name.Contains("02_proc"))
		{
			base.LoadBackground("bg_grnd_dis_02_" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, missionMapData.LoadingImageCount));
		}
		else
		{
			base.LoadBackground(string.Concat(new object[]
			{
				"bg_",
				missionMapData.Name,
				"_",
				global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, missionMapData.LoadingImageCount)
			}));
		}
	}

	public global::MissionHeaderView missionHeaderView;

	public global::UnityEngine.GameObject campaignDescription;

	public global::UnityEngine.GameObject objectivesView;

	public global::UnityEngine.UI.Text deploymentTitle;

	public global::UnityEngine.UI.Text deploymentDesc;

	public global::UnityEngine.UI.Text descriptionTitle;

	public global::UnityEngine.UI.Text descriptionDesc;

	public global::UnityEngine.GameObject ambush;

	public global::UnityEngine.UI.Image ambusher;

	public global::UnityEngine.UI.Image ambushed;

	public global::UnityEngine.Sprite bulletPoint;

	public global::UnityEngine.GameObject skirmishView;

	public global::UnityEngine.UI.Text warbandName;

	public global::UnityEngine.UI.Text warbandRank;

	public global::UnityEngine.UI.Text warbandRating;

	public global::UnityEngine.UI.Image warbandFlag;

	public global::UnityEngine.UI.Text opponentWarbandName;

	public global::UnityEngine.UI.Text opponentWarbandRank;

	public global::UnityEngine.UI.Text opponentWarbandRating;

	public global::UnityEngine.UI.Image opponentWarbandFlag;

	public global::UnityEngine.UI.Text gameplayType;

	public global::UnityEngine.UI.Text gameplayTypeDesc;

	public global::UnityEngine.UI.Text deploymentType;

	public global::UnityEngine.UI.Text deploymentTypeDesc;

	public global::UnityEngine.UI.Text optionalObjectivePlayerName;

	public global::UnityEngine.UI.Text optionalObjective;

	public global::UnityEngine.UI.Text optionalObjectiveOpponentName;

	public global::UnityEngine.UI.Text optionalObjectiveOpponent;

	private global::Mission mission;

	private global::CampaignMissionData missionData;
}
