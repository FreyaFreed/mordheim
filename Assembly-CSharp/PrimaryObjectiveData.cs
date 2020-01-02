using System;
using Mono.Data.Sqlite;

public class PrimaryObjectiveData : global::DataCore
{
	public global::PrimaryObjectiveId Id { get; private set; }

	public string Name { get; private set; }

	public global::PrimaryObjectiveTypeId PrimaryObjectiveTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::PrimaryObjectiveId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.PrimaryObjectiveTypeId = (global::PrimaryObjectiveTypeId)reader.GetInt32(2);
	}
}
