using System;
using Mono.Data.Sqlite;

public class AchievementEquipItemQualityData : global::DataCore
{
	public int Id { get; private set; }

	public global::AchievementId AchievementId { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.AchievementId = (global::AchievementId)reader.GetInt32(1);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(2);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(3);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(4);
	}
}
