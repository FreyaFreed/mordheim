using System;
using Mono.Data.Sqlite;

public class PrimaryObjectiveActivateData : global::DataCore
{
	public int Id { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveId { get; private set; }

	public string PropsName { get; private set; }

	public int PropsCount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.PrimaryObjectiveId = (global::PrimaryObjectiveId)reader.GetInt32(1);
		this.PropsName = reader.GetString(2);
		this.PropsCount = reader.GetInt32(3);
	}
}
