using System;
using Mono.Data.Sqlite;

public class CampaignUnitData : global::DataCore
{
	public global::CampaignUnitId Id { get; private set; }

	public string Name { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public global::AiProfileId AiProfileId { get; private set; }

	public int Rank { get; private set; }

	public string FirstName { get; private set; }

	public bool StartHidden { get; private set; }

	public bool StartInactive { get; private set; }

	public global::CampaignUnitId CampaignUnitIdSpawnOnDeath { get; private set; }

	public bool NoLootBag { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CampaignUnitId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.UnitId = (global::UnitId)reader.GetInt32(2);
		this.AiProfileId = (global::AiProfileId)reader.GetInt32(3);
		this.Rank = reader.GetInt32(4);
		this.FirstName = reader.GetString(5);
		this.StartHidden = (reader.GetInt32(6) != 0);
		this.StartInactive = (reader.GetInt32(7) != 0);
		this.CampaignUnitIdSpawnOnDeath = (global::CampaignUnitId)reader.GetInt32(8);
		this.NoLootBag = (reader.GetInt32(9) != 0);
	}
}
