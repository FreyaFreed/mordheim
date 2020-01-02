using System;
using Mono.Data.Sqlite;

public class CampaignMissionData : global::DataCore
{
	public global::CampaignMissionId Id { get; private set; }

	public string Name { get; private set; }

	public bool IsTuto { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public int Rating { get; private set; }

	public int Idx { get; private set; }

	public global::DeploymentScenarioId DeploymentScenarioId { get; private set; }

	public int MapPos { get; private set; }

	public int Days { get; private set; }

	public global::WarbandSkillId WarbandSkillIdReward { get; private set; }

	public global::SearchDensityId SearchDensityId { get; private set; }

	public global::WyrdstonePlacementId WyrdstonePlacementId { get; private set; }

	public global::WyrdstoneDensityId WyrdstoneDensityId { get; private set; }

	public int LoadingImageCount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CampaignMissionId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.IsTuto = (reader.GetInt32(2) != 0);
		this.WarbandId = (global::WarbandId)reader.GetInt32(3);
		this.Rating = reader.GetInt32(4);
		this.Idx = reader.GetInt32(5);
		this.DeploymentScenarioId = (global::DeploymentScenarioId)reader.GetInt32(6);
		this.MapPos = reader.GetInt32(7);
		this.Days = reader.GetInt32(8);
		this.WarbandSkillIdReward = (global::WarbandSkillId)reader.GetInt32(9);
		this.SearchDensityId = (global::SearchDensityId)reader.GetInt32(10);
		this.WyrdstonePlacementId = (global::WyrdstonePlacementId)reader.GetInt32(11);
		this.WyrdstoneDensityId = (global::WyrdstoneDensityId)reader.GetInt32(12);
		this.LoadingImageCount = reader.GetInt32(13);
	}
}
