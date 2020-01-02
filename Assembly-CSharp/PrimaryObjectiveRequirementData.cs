using System;
using Mono.Data.Sqlite;

public class PrimaryObjectiveRequirementData : global::DataCore
{
	public int Id { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveId { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveIdRequired { get; private set; }

	public bool RequiredCompleted { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.PrimaryObjectiveId = (global::PrimaryObjectiveId)reader.GetInt32(1);
		this.PrimaryObjectiveIdRequired = (global::PrimaryObjectiveId)reader.GetInt32(2);
		this.RequiredCompleted = (reader.GetInt32(3) != 0);
	}
}
