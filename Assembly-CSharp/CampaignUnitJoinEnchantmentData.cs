using System;
using Mono.Data.Sqlite;

public class CampaignUnitJoinEnchantmentData : global::DataCore
{
	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
	}
}
