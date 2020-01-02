using System;
using Mono.Data.Sqlite;

public class EnchantmentData : global::DataCore
{
	public global::EnchantmentId Id { get; private set; }

	public string Name { get; private set; }

	public global::EffectTypeId EffectTypeId { get; private set; }

	public global::EnchantmentTypeId EnchantmentTypeId { get; private set; }

	public global::EnchantmentQualityId EnchantmentQualityId { get; private set; }

	public global::EnchantmentConsumeId EnchantmentConsumeId { get; private set; }

	public global::EnchantmentTriggerId EnchantmentTriggerIdDestroy { get; private set; }

	public int DamageMin { get; private set; }

	public int DamageMax { get; private set; }

	public global::EnchantmentDmgTriggerId EnchantmentDmgTriggerId { get; private set; }

	public global::AttributeId AttributeIdDmgResistRoll { get; private set; }

	public int Duration { get; private set; }

	public bool ValidNextAction { get; private set; }

	public bool Indestructible { get; private set; }

	public bool RequireUnitState { get; private set; }

	public bool ChangeUnitState { get; private set; }

	public global::UnitStateId UnitStateIdRequired { get; private set; }

	public global::UnitStateId UnitStateIdNext { get; private set; }

	public bool Stackable { get; private set; }

	public bool DestroyOnApply { get; private set; }

	public bool KeepOnDeath { get; private set; }

	public bool NoDisplay { get; private set; }

	public bool MakeUnitVisible { get; private set; }

	public global::EnchantmentId EnchantmentIdOnTurnStart { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::EnchantmentId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.EffectTypeId = (global::EffectTypeId)reader.GetInt32(2);
		this.EnchantmentTypeId = (global::EnchantmentTypeId)reader.GetInt32(3);
		this.EnchantmentQualityId = (global::EnchantmentQualityId)reader.GetInt32(4);
		this.EnchantmentConsumeId = (global::EnchantmentConsumeId)reader.GetInt32(5);
		this.EnchantmentTriggerIdDestroy = (global::EnchantmentTriggerId)reader.GetInt32(6);
		this.DamageMin = reader.GetInt32(7);
		this.DamageMax = reader.GetInt32(8);
		this.EnchantmentDmgTriggerId = (global::EnchantmentDmgTriggerId)reader.GetInt32(9);
		this.AttributeIdDmgResistRoll = (global::AttributeId)reader.GetInt32(10);
		this.Duration = reader.GetInt32(11);
		this.ValidNextAction = (reader.GetInt32(12) != 0);
		this.Indestructible = (reader.GetInt32(13) != 0);
		this.RequireUnitState = (reader.GetInt32(14) != 0);
		this.ChangeUnitState = (reader.GetInt32(15) != 0);
		this.UnitStateIdRequired = (global::UnitStateId)reader.GetInt32(16);
		this.UnitStateIdNext = (global::UnitStateId)reader.GetInt32(17);
		this.Stackable = (reader.GetInt32(18) != 0);
		this.DestroyOnApply = (reader.GetInt32(19) != 0);
		this.KeepOnDeath = (reader.GetInt32(20) != 0);
		this.NoDisplay = (reader.GetInt32(21) != 0);
		this.MakeUnitVisible = (reader.GetInt32(22) != 0);
		this.EnchantmentIdOnTurnStart = (global::EnchantmentId)reader.GetInt32(23);
	}
}
