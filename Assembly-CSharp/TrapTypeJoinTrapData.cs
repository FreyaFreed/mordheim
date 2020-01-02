using System;
using Mono.Data.Sqlite;

public class TrapTypeJoinTrapData : global::DataCore
{
	public global::TrapTypeId TrapTypeId { get; private set; }

	public global::TrapId TrapId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.TrapTypeId = (global::TrapTypeId)reader.GetInt32(0);
		this.TrapId = (global::TrapId)reader.GetInt32(1);
	}
}
