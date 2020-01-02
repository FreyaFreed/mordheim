using System;
using Mono.Data.Sqlite;

public class UnitRankData : global::DataCore
{
	public global::UnitRankId Id { get; private set; }

	public string Name { get; private set; }

	public int Rank { get; private set; }

	public int Advancement { get; private set; }

	public int Rating { get; private set; }

	public int Wound { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitRankId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Rank = reader.GetInt32(2);
		this.Advancement = reader.GetInt32(3);
		this.Rating = reader.GetInt32(4);
		this.Wound = reader.GetInt32(5);
	}
}
