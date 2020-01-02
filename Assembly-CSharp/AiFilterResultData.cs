using System;
using Mono.Data.Sqlite;

public class AiFilterResultData : global::DataCore
{
	public global::AiFilterResultId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AiFilterResultId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
