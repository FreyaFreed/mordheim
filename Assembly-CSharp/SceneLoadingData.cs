using System;
using Mono.Data.Sqlite;

public class SceneLoadingData : global::DataCore
{
	public global::SceneLoadingId Id { get; private set; }

	public string Name { get; private set; }

	public string LoadScene { get; private set; }

	public global::SceneLoadingTypeId SceneLoadingTypeId { get; private set; }

	public string NextScene { get; private set; }

	public string TransitionName { get; private set; }

	public double TransitionDuration { get; private set; }

	public bool WaitAction { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SceneLoadingId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.LoadScene = reader.GetString(2);
		this.SceneLoadingTypeId = (global::SceneLoadingTypeId)reader.GetInt32(3);
		this.NextScene = reader.GetString(4);
		this.TransitionName = reader.GetString(5);
		this.TransitionDuration = reader.GetDouble(6);
		this.WaitAction = (reader.GetInt32(7) != 0);
	}
}
