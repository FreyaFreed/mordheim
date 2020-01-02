using System;
using Mono.Data.Sqlite;

public class DeploymentScenarioSlotData : global::DataCore
{
	public global::DeploymentScenarioSlotId Id { get; private set; }

	public string Name { get; private set; }

	public global::DeploymentScenarioId DeploymentScenarioId { get; private set; }

	public global::DeploymentId DeploymentId { get; private set; }

	public string Title { get; private set; }

	public string Setup { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::DeploymentScenarioSlotId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.DeploymentScenarioId = (global::DeploymentScenarioId)reader.GetInt32(2);
		this.DeploymentId = (global::DeploymentId)reader.GetInt32(3);
		this.Title = reader.GetString(4);
		this.Setup = reader.GetString(5);
	}
}
