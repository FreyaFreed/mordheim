using System;
using Mono.Data.Sqlite;

public class CampaignUnitJoinMutationData : global::DataCore
{
	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(0);
		this.MutationId = (global::MutationId)reader.GetInt32(1);
	}
}
