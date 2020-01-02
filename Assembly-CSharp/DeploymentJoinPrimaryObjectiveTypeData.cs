using System;
using Mono.Data.Sqlite;

public class DeploymentJoinPrimaryObjectiveTypeData : global::DataCore
{
	public global::DeploymentId DeploymentId { get; private set; }

	public global::PrimaryObjectiveTypeId PrimaryObjectiveTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.DeploymentId = (global::DeploymentId)reader.GetInt32(0);
		this.PrimaryObjectiveTypeId = (global::PrimaryObjectiveTypeId)reader.GetInt32(1);
	}
}
