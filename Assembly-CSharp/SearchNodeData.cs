using System;
using Mono.Data.Sqlite;

public class SearchNodeData : global::DataCore
{
	public global::SearchNodeId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SearchNodeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
