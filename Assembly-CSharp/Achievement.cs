using System;

public abstract class Achievement
{
	protected Achievement(global::AchievementData data)
	{
		this.Data = data;
	}

	public global::AchievementData Data { get; private set; }

	public global::AchievementId Id
	{
		get
		{
			return this.Data.Id;
		}
	}

	public bool Completed { get; set; }

	public global::AchievementTargetId Target
	{
		get
		{
			return this.Data.AchievementTargetId;
		}
	}

	public int Xp
	{
		get
		{
			return this.Data.Xp;
		}
	}

	public string LocName
	{
		get
		{
			if (this.locName == null)
			{
				this.locName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_title_" + this.Data.Name);
			}
			return this.locName;
		}
	}

	public string LocDesc
	{
		get
		{
			if (this.locDesc == null)
			{
				this.locDesc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_desc_" + this.Data.Name);
			}
			return this.locDesc;
		}
	}

	public static global::Achievement Create(global::AchievementData data)
	{
		switch (data.AchievementTypeId)
		{
		case global::AchievementTypeId.STAT:
			return new global::AchievementStat(data);
		case global::AchievementTypeId.EQUIP_ITEM_QUALITY:
		case global::AchievementTypeId.EQUIP_RUNE_QUALITY:
			return new global::AchievementEquipItem(data);
		case global::AchievementTypeId.CAMPAIGN_MISSION:
			return new global::AchievementCampaignMission(data);
		}
		if (data.Id != global::AchievementId.NONE)
		{
		}
		return null;
	}

	public virtual bool CheckProfile(global::WarbandAttributeId statId, int value)
	{
		return false;
	}

	public virtual bool CheckWarband(global::Warband warband, global::WarbandAttributeId statId, int value)
	{
		return false;
	}

	public virtual bool CheckUnit(global::Unit unit, global::AttributeId statId, int value)
	{
		return false;
	}

	public virtual bool CheckEquipUnit(global::Unit unit, global::UnitSlotId slotId)
	{
		return false;
	}

	public virtual bool CheckFinishMission(global::CampaignMissionId missionId)
	{
		return false;
	}

	public bool CanCheck()
	{
		return !this.Completed && (this.Data.AchievementIdRequire == global::AchievementId.NONE || global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(this.Data.AchievementIdRequire));
	}

	public int Unlock()
	{
		this.Completed = true;
		return this.Data.Xp;
	}

	protected bool IsOfUnitType(global::Unit unit, global::UnitTypeId requiredUnitTypeId)
	{
		global::UnitTypeId unitTypeId = unit.GetUnitTypeId();
		return requiredUnitTypeId == global::UnitTypeId.NONE || requiredUnitTypeId == unitTypeId || (requiredUnitTypeId == global::UnitTypeId.HERO_1 && (unitTypeId == global::UnitTypeId.HERO_2 || unitTypeId == global::UnitTypeId.HERO_3)) || (requiredUnitTypeId == global::UnitTypeId.ACHIEVEMENT_OUTSIDER && unit.UnitSave.isOutsider);
	}

	private string locName;

	private string locDesc;
}
