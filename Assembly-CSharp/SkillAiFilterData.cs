using System;
using Mono.Data.Sqlite;

public class SkillAiFilterData : global::DataCore
{
	public global::SkillAiFilterId Id { get; private set; }

	public string Name { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public global::AiFilterResultId AiFilterResultId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public global::AiFilterCheckId AiFilterCheckId { get; private set; }

	public int CheckValue { get; private set; }

	public global::AttributeId AttributeIdCheck { get; private set; }

	public global::AiFilterCheckId AiFilterCheckIdEngaged { get; private set; }

	public int EngagedValue { get; private set; }

	public bool HasAltSet { get; private set; }

	public bool NeverUsedOnTarget { get; private set; }

	public bool NeverUsedTurn { get; private set; }

	public bool HasRangeWeapon { get; private set; }

	public bool IsAllAlone { get; private set; }

	public bool IsSister { get; private set; }

	public bool IsStunned { get; private set; }

	public bool CannotParry { get; private set; }

	public bool HasSpell { get; private set; }

	public int HealthUnderRatio { get; private set; }

	public int MinRoll { get; private set; }

	public bool HasBeenShot { get; private set; }

	public bool NoEnemyInSight { get; private set; }

	public bool IsPreFight { get; private set; }

	public global::EnchantmentTypeId EnchantmentTypeIdApplied { get; private set; }

	public global::SkillAiFilterId SkillAiFilterIdAnd { get; private set; }

	public bool CheckTargetInstead { get; private set; }

	public bool Reverse { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SkillAiFilterId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.SkillId = (global::SkillId)reader.GetInt32(2);
		this.AiFilterResultId = (global::AiFilterResultId)reader.GetInt32(3);
		this.AttributeId = (global::AttributeId)reader.GetInt32(4);
		this.AiFilterCheckId = (global::AiFilterCheckId)reader.GetInt32(5);
		this.CheckValue = reader.GetInt32(6);
		this.AttributeIdCheck = (global::AttributeId)reader.GetInt32(7);
		this.AiFilterCheckIdEngaged = (global::AiFilterCheckId)reader.GetInt32(8);
		this.EngagedValue = reader.GetInt32(9);
		this.HasAltSet = (reader.GetInt32(10) != 0);
		this.NeverUsedOnTarget = (reader.GetInt32(11) != 0);
		this.NeverUsedTurn = (reader.GetInt32(12) != 0);
		this.HasRangeWeapon = (reader.GetInt32(13) != 0);
		this.IsAllAlone = (reader.GetInt32(14) != 0);
		this.IsSister = (reader.GetInt32(15) != 0);
		this.IsStunned = (reader.GetInt32(16) != 0);
		this.CannotParry = (reader.GetInt32(17) != 0);
		this.HasSpell = (reader.GetInt32(18) != 0);
		this.HealthUnderRatio = reader.GetInt32(19);
		this.MinRoll = reader.GetInt32(20);
		this.HasBeenShot = (reader.GetInt32(21) != 0);
		this.NoEnemyInSight = (reader.GetInt32(22) != 0);
		this.IsPreFight = (reader.GetInt32(23) != 0);
		this.EnchantmentTypeIdApplied = (global::EnchantmentTypeId)reader.GetInt32(24);
		this.SkillAiFilterIdAnd = (global::SkillAiFilterId)reader.GetInt32(25);
		this.CheckTargetInstead = (reader.GetInt32(26) != 0);
		this.Reverse = (reader.GetInt32(27) != 0);
	}
}
