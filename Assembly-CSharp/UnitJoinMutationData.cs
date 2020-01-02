using System;
using Mono.Data.Sqlite;

public class UnitJoinMutationData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.MutationId = (global::MutationId)reader.GetInt32(1);
	}
}
