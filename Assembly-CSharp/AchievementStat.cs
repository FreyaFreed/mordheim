using System;

public class AchievementStat : global::Achievement
{
	public AchievementStat(global::AchievementData data) : base(data)
	{
	}

	private bool CheckStat(int value)
	{
		return value >= base.Data.TargetStatValue;
	}

	public override bool CheckProfile(global::WarbandAttributeId statId, int value)
	{
		return base.Data.AchievementTargetId == global::AchievementTargetId.PROFILE && base.Data.WarbandAttributeId != global::WarbandAttributeId.NONE && !base.Completed && base.Data.WarbandAttributeId == statId && this.CheckStat(value);
	}

	public override bool CheckWarband(global::Warband warband, global::WarbandAttributeId statId, int value)
	{
		return base.Data.AchievementTargetId == global::AchievementTargetId.WARBAND && base.Data.WarbandAttributeId != global::WarbandAttributeId.NONE && !base.Completed && base.Data.WarbandAttributeId == statId && this.CheckStat(value);
	}

	public override bool CheckUnit(global::Unit unit, global::AttributeId statId, int value)
	{
		return base.Data.AchievementTargetId == global::AchievementTargetId.UNIT && base.Data.AttributeId != global::AttributeId.NONE && !base.Completed && base.Data.AttributeId == statId && base.IsOfUnitType(unit, base.Data.UnitTypeId) && this.CheckStat(value);
	}
}
