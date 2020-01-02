using System;
using Mono.Data.Sqlite;

public class WarbandSkillUnitData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public bool BaseUnit { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(1);
		this.UnitId = (global::UnitId)reader.GetInt32(2);
		this.BaseUnit = (reader.GetInt32(3) != 0);
	}
}
