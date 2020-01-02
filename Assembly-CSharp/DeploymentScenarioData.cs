using System;
using Mono.Data.Sqlite;

public class DeploymentScenarioData : global::DataCore
{
	public global::DeploymentScenarioId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::DeploymentScenarioId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
