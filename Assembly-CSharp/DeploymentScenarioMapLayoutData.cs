using System;
using Mono.Data.Sqlite;

public class DeploymentScenarioMapLayoutData : global::DataCore
{
	public global::DeploymentScenarioMapLayoutId Id { get; private set; }

	public string Name { get; private set; }

	public bool Skirmish { get; private set; }

	public bool Procedural { get; private set; }

	public bool Ambush { get; private set; }

	public global::DeploymentScenarioId DeploymentScenarioId { get; private set; }

	public global::MissionMapId MissionMapId { get; private set; }

	public global::MissionMapLayoutId MissionMapLayoutId { get; private set; }

	public global::PropRestrictionId PropRestrictionIdProps { get; private set; }

	public global::PropRestrictionId PropRestrictionIdMadstuff { get; private set; }

	public global::PropRestrictionId PropRestrictionIdBarricade { get; private set; }

	public string PropsLayer { get; private set; }

	public string DeploymentLayer { get; private set; }

	public string TrapsLayer { get; private set; }

	public string SearchLayer { get; private set; }

	public string ExtraLightsFxLayer { get; private set; }

	public int TrapCount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::DeploymentScenarioMapLayoutId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Skirmish = (reader.GetInt32(2) != 0);
		this.Procedural = (reader.GetInt32(3) != 0);
		this.Ambush = (reader.GetInt32(4) != 0);
		this.DeploymentScenarioId = (global::DeploymentScenarioId)reader.GetInt32(5);
		this.MissionMapId = (global::MissionMapId)reader.GetInt32(6);
		this.MissionMapLayoutId = (global::MissionMapLayoutId)reader.GetInt32(7);
		this.PropRestrictionIdProps = (global::PropRestrictionId)reader.GetInt32(8);
		this.PropRestrictionIdMadstuff = (global::PropRestrictionId)reader.GetInt32(9);
		this.PropRestrictionIdBarricade = (global::PropRestrictionId)reader.GetInt32(10);
		this.PropsLayer = reader.GetString(11);
		this.DeploymentLayer = reader.GetString(12);
		this.TrapsLayer = reader.GetString(13);
		this.SearchLayer = reader.GetString(14);
		this.ExtraLightsFxLayer = reader.GetString(15);
		this.TrapCount = reader.GetInt32(16);
	}
}
