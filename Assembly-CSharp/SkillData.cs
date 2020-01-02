using System;
using Mono.Data.Sqlite;

public class SkillData : global::DataCore
{
	public global::SkillId Id { get; private set; }

	public string Name { get; private set; }

	public bool Released { get; private set; }

	public bool Passive { get; private set; }

	public global::SkillTypeId SkillTypeId { get; private set; }

	public global::SpellTypeId SpellTypeId { get; private set; }

	public global::EffectTypeId EffectTypeId { get; private set; }

	public global::UnitActionId UnitActionId { get; private set; }

	public global::BoneId BoneIdTarget { get; private set; }

	public global::SkillQualityId SkillQualityId { get; private set; }

	public global::SkillLineId SkillLineId { get; private set; }

	public global::SkillId SkillIdPrerequiste { get; private set; }

	public bool TargetSelf { get; private set; }

	public bool TargetAlly { get; private set; }

	public bool TargetEnemy { get; private set; }

	public global::TargetingId TargetingId { get; private set; }

	public bool NeedValidGround { get; private set; }

	public int StrategyPoints { get; private set; }

	public int OffensePoints { get; private set; }

	public int WoundsCostMin { get; private set; }

	public int WoundsCostMax { get; private set; }

	public global::AttributeId AttributeIdStat { get; private set; }

	public int StatValue { get; private set; }

	public int Cost { get; private set; }

	public int Time { get; private set; }

	public int Points { get; private set; }

	public bool NotEngaged { get; private set; }

	public bool Engaged { get; private set; }

	public bool NeedCloseSet { get; private set; }

	public bool NeedRangeSet { get; private set; }

	public bool WeaponLoaded { get; private set; }

	public global::EnchantmentId EnchantmentIdRequired { get; private set; }

	public global::EnchantmentId EnchantmentIdRequiredTarget { get; private set; }

	public int RangeMin { get; private set; }

	public int Range { get; private set; }

	public int Radius { get; private set; }

	public int Angle { get; private set; }

	public int WoundMin { get; private set; }

	public int WoundMax { get; private set; }

	public bool BypassArmor { get; private set; }

	public bool BypassMagicResist { get; private set; }

	public int LadderDiff { get; private set; }

	public global::ZoneAoeId ZoneAoeId { get; private set; }

	public global::TrapTypeId TrapTypeId { get; private set; }

	public global::DestructibleId DestructibleId { get; private set; }

	public bool AiProof { get; private set; }

	public bool AutoSuccess { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SkillId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Released = (reader.GetInt32(2) != 0);
		this.Passive = (reader.GetInt32(3) != 0);
		this.SkillTypeId = (global::SkillTypeId)reader.GetInt32(4);
		this.SpellTypeId = (global::SpellTypeId)reader.GetInt32(5);
		this.EffectTypeId = (global::EffectTypeId)reader.GetInt32(6);
		this.UnitActionId = (global::UnitActionId)reader.GetInt32(7);
		this.BoneIdTarget = (global::BoneId)reader.GetInt32(8);
		this.SkillQualityId = (global::SkillQualityId)reader.GetInt32(9);
		this.SkillLineId = (global::SkillLineId)reader.GetInt32(10);
		this.SkillIdPrerequiste = (global::SkillId)reader.GetInt32(11);
		this.TargetSelf = (reader.GetInt32(12) != 0);
		this.TargetAlly = (reader.GetInt32(13) != 0);
		this.TargetEnemy = (reader.GetInt32(14) != 0);
		this.TargetingId = (global::TargetingId)reader.GetInt32(15);
		this.NeedValidGround = (reader.GetInt32(16) != 0);
		this.StrategyPoints = reader.GetInt32(17);
		this.OffensePoints = reader.GetInt32(18);
		this.WoundsCostMin = reader.GetInt32(19);
		this.WoundsCostMax = reader.GetInt32(20);
		this.AttributeIdStat = (global::AttributeId)reader.GetInt32(21);
		this.StatValue = reader.GetInt32(22);
		this.Cost = reader.GetInt32(23);
		this.Time = reader.GetInt32(24);
		this.Points = reader.GetInt32(25);
		this.NotEngaged = (reader.GetInt32(26) != 0);
		this.Engaged = (reader.GetInt32(27) != 0);
		this.NeedCloseSet = (reader.GetInt32(28) != 0);
		this.NeedRangeSet = (reader.GetInt32(29) != 0);
		this.WeaponLoaded = (reader.GetInt32(30) != 0);
		this.EnchantmentIdRequired = (global::EnchantmentId)reader.GetInt32(31);
		this.EnchantmentIdRequiredTarget = (global::EnchantmentId)reader.GetInt32(32);
		this.RangeMin = reader.GetInt32(33);
		this.Range = reader.GetInt32(34);
		this.Radius = reader.GetInt32(35);
		this.Angle = reader.GetInt32(36);
		this.WoundMin = reader.GetInt32(37);
		this.WoundMax = reader.GetInt32(38);
		this.BypassArmor = (reader.GetInt32(39) != 0);
		this.BypassMagicResist = (reader.GetInt32(40) != 0);
		this.LadderDiff = reader.GetInt32(41);
		this.ZoneAoeId = (global::ZoneAoeId)reader.GetInt32(42);
		this.TrapTypeId = (global::TrapTypeId)reader.GetInt32(43);
		this.DestructibleId = (global::DestructibleId)reader.GetInt32(44);
		this.AiProof = (reader.GetInt32(45) != 0);
		this.AutoSuccess = (reader.GetInt32(46) != 0);
	}
}
