using System;
using Mono.Data.Sqlite;

public class CampaignMissionSpawnQueueData : global::DataCore
{
	public int Id { get; private set; }

	public global::CampaignMissionSpawnId CampaignMissionSpawnId { get; private set; }

	public int Order { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public int Amount { get; private set; }

	public int Rank { get; private set; }

	public int Rating { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.CampaignMissionSpawnId = (global::CampaignMissionSpawnId)reader.GetInt32(1);
		this.Order = reader.GetInt32(2);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(3);
		this.Amount = reader.GetInt32(4);
		this.Rank = reader.GetInt32(5);
		this.Rating = reader.GetInt32(6);
	}
}
