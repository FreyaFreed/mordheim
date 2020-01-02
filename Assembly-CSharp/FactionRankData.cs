using System;
using Mono.Data.Sqlite;

public class FactionRankData : global::DataCore
{
	public global::FactionRankId Id { get; private set; }

	public string Name { get; private set; }

	public int Reputation { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::FactionRankId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Reputation = reader.GetInt32(2);
	}
}
