using System;
using System.Collections.Generic;
using UnityEngine;

public class Profile
{
	public Profile(global::ProfileSave save)
	{
		this.ProfileSave = save;
		this.RankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>(this.ProfileSave.rankId);
		this.InitAchievements();
	}

	public global::ProfileSave ProfileSave { get; private set; }

	public global::PlayerRankData RankData
	{
		get
		{
			return this.rankData;
		}
		set
		{
			this.rankData = value;
			this.HasNextRank = (global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>((int)(this.RankData.Id + 1)) != null);
		}
	}

	public bool HasNextRank { get; set; }

	public int Rank
	{
		get
		{
			return this.ProfileSave.rankId;
		}
	}

	public int CurrentXp
	{
		get
		{
			return this.ProfileSave.xp;
		}
	}

	public int NewGameBonusGold
	{
		get
		{
			return this.RankData.NewGameGold;
		}
	}

	public global::Achievement[] Achievements { get; private set; }

	public bool[] TutorialCompletion
	{
		get
		{
			return this.ProfileSave.completedTutorials;
		}
	}

	public global::AchievementCategory[] AchievementsCategories { get; private set; }

	public int LastPlayedCampaign
	{
		get
		{
			return this.ProfileSave.lastPlayedCampaign;
		}
		set
		{
			this.ProfileSave.lastPlayedCampaign = value;
		}
	}

	public void UpdateHash(int campaign, int hash)
	{
		this.ProfileSave.warbandSaves[campaign] = hash;
	}

	public void ClearHash(int campaign)
	{
		this.ProfileSave.warbandSaves.Remove(campaign);
	}

	public void CheckXp()
	{
		if (this.ProfileSave.xpChecked && this.ProfileSave.xp >= this.RankData.XpNeeded && this.HasNextRank)
		{
			this.AddXp(0);
			global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
			return;
		}
		if (this.ProfileSave.xpChecked)
		{
			return;
		}
		this.ProfileSave.xpChecked = true;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>
		{
			0,
			10,
			25,
			45,
			70,
			100,
			135,
			175,
			220,
			270,
			320,
			370,
			420,
			470,
			520
		};
		int num = 0;
		global::System.Collections.Generic.List<global::PlayerRankData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>();
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i].Id < (global::PlayerRankId)this.Rank)
			{
				num += list2[i].XpNeeded;
			}
		}
		int xp = global::UnityEngine.Mathf.Max(list[this.Rank] - num, 0);
		this.AddXp(xp);
		global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
	}

	public global::System.Collections.Generic.List<global::PlayerRankData> AddXp(int xp)
	{
		global::System.Collections.Generic.List<global::PlayerRankData> list = new global::System.Collections.Generic.List<global::PlayerRankData>();
		this.ProfileSave.xp += xp;
		global::PlayerRankData playerRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>(this.ProfileSave.rankId);
		bool flag = false;
		while (this.ProfileSave.xp >= playerRankData.XpNeeded && this.HasNextRank)
		{
			this.ProfileSave.xp -= playerRankData.XpNeeded;
			global::PlayerRankData playerRankData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>(this.ProfileSave.rankId + 1);
			if (playerRankData2 != null)
			{
				list.Add(playerRankData2);
				playerRankData = playerRankData2;
				this.ProfileSave.rankId++;
				this.RankData = playerRankData2;
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int>(global::Notices.PROFILE_RANK_UP, this.ProfileSave.rankId);
				flag = true;
			}
		}
		if (flag)
		{
			global::PandoraSingleton<global::Pan>.Instance.Narrate("new_veteran_rank");
		}
		else
		{
			global::PandoraSingleton<global::Pan>.Instance.Narrate("veteran_task_completed");
		}
		return list;
	}

	public void AddToStat(global::WarbandAttributeId statId, int increment)
	{
		if (this.ProfileSave != null)
		{
			this.ProfileSave.stats[(int)statId] += increment;
			this.CheckAchievement(statId, this.ProfileSave.stats[(int)statId]);
		}
	}

	public void SetStat(global::WarbandAttributeId statId, int value)
	{
		if (this.ProfileSave != null)
		{
			this.ProfileSave.stats[(int)statId] = value;
			this.CheckAchievement(statId, this.ProfileSave.stats[(int)statId]);
		}
	}

	private void InitAchievements()
	{
		this.Achievements = new global::Achievement[227];
		this.AchievementsCategories = new global::AchievementCategory[36];
		global::System.Collections.Generic.List<global::AchievementData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AchievementData>();
		for (int i = 0; i < list.Count; i++)
		{
			global::Achievement achievement = global::Achievement.Create(list[i]);
			if (achievement != null)
			{
				this.Achievements[(int)list[i].Id] = achievement;
				achievement.Completed = this.ProfileSave.unlockedAchievements[(int)list[i].Id];
				if (achievement.Data.PlayerProgression)
				{
					int achievementCategoryId = (int)achievement.Data.AchievementCategoryId;
					if (this.AchievementsCategories[achievementCategoryId] == null)
					{
						this.AchievementsCategories[achievementCategoryId] = new global::AchievementCategory(achievement.Data.AchievementCategoryId);
					}
					this.AchievementsCategories[achievementCategoryId].achievements.Add(achievement);
				}
			}
		}
	}

	public bool IsAchievementUnlocked(global::AchievementId achievementId)
	{
		return this.Achievements[(int)achievementId] != null && this.Achievements[(int)achievementId].Completed;
	}

	public void CheckAchievement(global::Warband warband, global::WarbandAttributeId statId, int value)
	{
		for (int i = 0; i < this.Achievements.Length; i++)
		{
			if (this.Achievements[i] != null && this.Achievements[i].CanCheck() && this.Achievements[i].Target == global::AchievementTargetId.WARBAND && this.Achievements[i].CheckWarband(warband, statId, value))
			{
				this.Unlock(this.Achievements[i].Id);
			}
		}
	}

	public void CheckAchievement(global::WarbandAttributeId statId, int value)
	{
		for (int i = 0; i < this.Achievements.Length; i++)
		{
			if (this.Achievements[i] != null && this.Achievements[i].CanCheck() && this.Achievements[i].Target == global::AchievementTargetId.PROFILE && this.Achievements[i].CheckProfile(statId, value))
			{
				this.Unlock(this.Achievements[i].Id);
			}
		}
	}

	public void CheckAchievement(global::Unit unit, global::AttributeId statId = global::AttributeId.NONE, int value = 0)
	{
		for (int i = 0; i < this.Achievements.Length; i++)
		{
			if (this.Achievements[i] != null && this.Achievements[i].CanCheck() && this.Achievements[i].Target == global::AchievementTargetId.UNIT && this.Achievements[i].CheckUnit(unit, statId, value))
			{
				this.Unlock(this.Achievements[i].Id);
			}
		}
	}

	public void CheckAchievement(global::CampaignMissionId missionId)
	{
		for (int i = 0; i < this.Achievements.Length; i++)
		{
			if (this.Achievements[i] != null && this.Achievements[i].CanCheck() && this.Achievements[i].Target == global::AchievementTargetId.PROFILE && this.Achievements[i].CheckFinishMission(missionId))
			{
				this.Unlock(this.Achievements[i].Id);
			}
		}
	}

	public void CheckEquipAchievement(global::Unit unit, global::UnitSlotId slotId)
	{
		for (int i = 0; i < this.Achievements.Length; i++)
		{
			if (this.Achievements[i] != null && this.Achievements[i].CanCheck() && this.Achievements[i].Target == global::AchievementTargetId.UNIT && this.Achievements[i].CheckEquipUnit(unit, slotId))
			{
				this.Unlock(this.Achievements[i].Id);
			}
		}
	}

	private void Unlock(global::AchievementId achievementId)
	{
		if (!this.ProfileSave.unlockedAchievements[(int)achievementId])
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::AchievementId>(global::Notices.TASK_COMPLETED, achievementId);
			this.ProfileSave.unlockedAchievements[(int)achievementId] = true;
			this.AddXp(this.Achievements[(int)achievementId].Xp);
			this.Achievements[(int)achievementId].Completed = true;
			global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
		}
	}

	public void CompleteTutorial(int idx)
	{
		if (!this.ProfileSave.completedTutorials[idx])
		{
			this.ProfileSave.completedTutorials[idx] = true;
		}
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (this.ProfileSave.completedTutorials[i])
			{
				num++;
			}
		}
		if (num >= 2)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.TUTO_1);
		}
		if (num >= 4)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.TUTO_2);
		}
	}

	public bool HasCompletedTutorials()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.ProfileSave.completedTutorials[i])
			{
				return true;
			}
		}
		return false;
	}

	public string GetNextRankDescription()
	{
		if (!this.HasNextRank)
		{
			return string.Empty;
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("player_" + (this.Rank + global::PlayerRankId.RANK_1).ToLowerString() + "_perks_desc");
	}

	public string GetCurrentRankDescription()
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("player_" + ((global::PlayerRankId)this.Rank).ToLowerString() + "_desc");
	}

	public void UpdateGameProgress(global::WarbandId warId, int progress)
	{
		int num = 0;
		switch (warId)
		{
		case global::WarbandId.HUMAN_MERCENARIES:
			break;
		case global::WarbandId.SKAVENS:
			num = 1;
			break;
		case global::WarbandId.SISTERS_OF_SIGMAR:
			num = 2;
			break;
		default:
			if (warId != global::WarbandId.WITCH_HUNTERS)
			{
				if (warId != global::WarbandId.UNDEAD)
				{
					return;
				}
				num = 5;
			}
			else
			{
				num = 4;
			}
			break;
		case global::WarbandId.POSSESSED:
			num = 3;
			break;
		}
		this.ProfileSave.warProgress[num] = global::UnityEngine.Mathf.Max(this.ProfileSave.warProgress[num], progress);
	}

	public int GetGameProgress()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num += this.ProfileSave.warProgress[i];
		}
		return (int)((float)num * 100f / 4f / (float)global::Constant.GetInt(global::ConstantId.CAMPAIGN_LAST_MISSION));
	}

	public const int TUTO_MISSION_MAX_IDX = 4;

	private const int MAX_WAR_IDX_PROGRESS = 4;

	private global::PlayerRankData rankData;
}
