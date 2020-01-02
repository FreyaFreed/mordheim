using System;
using Mono.Data.Sqlite;

public class SearchRewardData : global::DataCore
{
	public global::SearchRewardId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SearchRewardId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(2);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(3);
	}
}
