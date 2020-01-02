using System;
using Mono.Data.Sqlite;

public class TrapTypeJoinTrapEffectData : global::DataCore
{
	public global::TrapTypeId TrapTypeId { get; private set; }

	public global::TrapEffectId TrapEffectId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.TrapTypeId = (global::TrapTypeId)reader.GetInt32(0);
		this.TrapEffectId = (global::TrapEffectId)reader.GetInt32(1);
	}
}
