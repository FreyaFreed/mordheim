using System;
using Mono.Data.Sqlite;

public class PrimaryObjectiveKeepAliveData : global::DataCore
{
	public int Id { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveId { get; private set; }

	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.PrimaryObjectiveId = (global::PrimaryObjectiveId)reader.GetInt32(1);
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(2);
	}
}
