using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkirmishLobbyPlayer : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
	}

	public void SetPlayerInfo(int index)
	{
		if (index == -1)
		{
			return;
		}
		global::MissionWarbandSave missionWarbandSave = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[index];
		if (missionWarbandSave == null)
		{
			return;
		}
		this.playerName.text = missionWarbandSave.Name;
		this.flag.sprite = global::Warband.GetFlagIcon(missionWarbandSave.WarbandId, false);
		this.warbandName.text = missionWarbandSave.PlayerName;
		this.ready.isOn = (missionWarbandSave.PlayerTypeId == global::PlayerTypeId.AI || missionWarbandSave.IsReady);
		this.ready.interactable = false;
		this.rating.text = missionWarbandSave.Rating.ToString();
		if (this.ratingIcon != null)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1)
			{
				this.ratingIcon.gameObject.SetActive(true);
				int num = 0;
				global::System.Collections.Generic.List<global::ProcMissionRatingData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>();
				for (int i = 0; i < list.Count; i++)
				{
					num = global::UnityEngine.Mathf.Max(num, list[i].MaxValue);
				}
				global::MissionWarbandSave missionWarbandSave2 = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[(index != 0) ? 0 : 1];
				global::ProcMissionRatingId procMissionRatingId;
				if (missionWarbandSave.PlayerTypeId == global::PlayerTypeId.PLAYER)
				{
					int num2 = (int)(((float)missionWarbandSave.Rating / (float)missionWarbandSave2.Rating - 1f) * 100f);
					num2 = global::UnityEngine.Mathf.Clamp(num2, 0, num);
					procMissionRatingId = global::PandoraSingleton<global::DataFactory>.Instance.InitDataClosest<global::ProcMissionRatingData>("max_value", num2, false).Id;
					procMissionRatingId = ((procMissionRatingId != global::ProcMissionRatingId.NONE) ? procMissionRatingId : global::ProcMissionRatingId.NORMAL);
				}
				else
				{
					procMissionRatingId = (global::ProcMissionRatingId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.ratingId;
				}
				this.ratingIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_mission_difficulty_" + procMissionRatingId.ToLowerString(), true);
				if (this.difficultyText != null)
				{
					this.difficultyText.gameObject.SetActive(true);
					this.difficultyText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_difficulty_" + procMissionRatingId.ToLowerString());
				}
			}
			else
			{
				this.ratingIcon.gameObject.SetActive(false);
				if (this.difficultyText != null)
				{
					this.difficultyText.gameObject.SetActive(false);
				}
			}
		}
		this.rank.text = missionWarbandSave.Rank.ToString();
	}

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Image flag;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Toggle ready;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text playerName;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text warbandName;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text campaign;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text rating;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Image ratingIcon;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text difficultyText;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text rank;
}
