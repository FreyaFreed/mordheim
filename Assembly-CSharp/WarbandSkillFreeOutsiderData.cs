using System;
using Mono.Data.Sqlite;

public class WarbandSkillFreeOutsiderData : global::DataCore
{
	public global::WarbandSkillFreeOutsiderId Id { get; private set; }

	public string Name { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public int Rank { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandSkillFreeOutsiderId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(2);
		this.UnitId = (global::UnitId)reader.GetInt32(3);
		this.Rank = reader.GetInt32(4);
	}
}
