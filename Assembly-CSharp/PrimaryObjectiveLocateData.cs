using System;
using Mono.Data.Sqlite;

public class PrimaryObjectiveLocateData : global::DataCore
{
	public int Id { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public int ItemCount { get; private set; }

	public string Zone { get; private set; }

	public int ZoneCount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.PrimaryObjectiveId = (global::PrimaryObjectiveId)reader.GetInt32(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
		this.ItemCount = reader.GetInt32(3);
		this.Zone = reader.GetString(4);
		this.ZoneCount = reader.GetInt32(5);
	}
}
