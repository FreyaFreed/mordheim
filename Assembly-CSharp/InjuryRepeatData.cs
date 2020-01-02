using System;
using Mono.Data.Sqlite;

public class InjuryRepeatData : global::DataCore
{
	public global::InjuryRepeatId Id { get; private set; }

	public string Name { get; private set; }

	public global::InjuryRepeatId InjuryRepeatIdResult { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::InjuryRepeatId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.InjuryRepeatIdResult = (global::InjuryRepeatId)reader.GetInt32(2);
	}
}
