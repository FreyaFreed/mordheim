using System;
using Mono.Data.Sqlite;

public class AchievementData : global::DataCore
{
	public global::AchievementId Id { get; private set; }

	public string Name { get; private set; }

	public global::AchievementCategoryId AchievementCategoryId { get; private set; }

	public global::AchievementTargetId AchievementTargetId { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public global::AchievementTypeId AchievementTypeId { get; private set; }

	public global::AchievementId AchievementIdRequire { get; private set; }

	public global::WarbandAttributeId WarbandAttributeId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public global::CampaignMissionId CampaignMissionId { get; private set; }

	public int TargetStatValue { get; private set; }

	public bool PlayerProgression { get; private set; }

	public bool EndReportVisible { get; private set; }

	public int Xp { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AchievementId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.AchievementCategoryId = (global::AchievementCategoryId)reader.GetInt32(2);
		this.AchievementTargetId = (global::AchievementTargetId)reader.GetInt32(3);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(4);
		this.WarbandId = (global::WarbandId)reader.GetInt32(5);
		this.AchievementTypeId = (global::AchievementTypeId)reader.GetInt32(6);
		this.AchievementIdRequire = (global::AchievementId)reader.GetInt32(7);
		this.WarbandAttributeId = (global::WarbandAttributeId)reader.GetInt32(8);
		this.AttributeId = (global::AttributeId)reader.GetInt32(9);
		this.CampaignMissionId = (global::CampaignMissionId)reader.GetInt32(10);
		this.TargetStatValue = reader.GetInt32(11);
		this.PlayerProgression = (reader.GetInt32(12) != 0);
		this.EndReportVisible = (reader.GetInt32(13) != 0);
		this.Xp = reader.GetInt32(14);
	}
}
