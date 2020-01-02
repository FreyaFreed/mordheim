using System;
using Mono.Data.Sqlite;

public class FactionConsequenceTargetData : global::DataCore
{
	public global::FactionConsequenceTargetId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::FactionConsequenceTargetId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
