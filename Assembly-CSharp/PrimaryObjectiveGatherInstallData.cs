using System;
using Mono.Data.Sqlite;

public class PrimaryObjectiveGatherInstallData : global::DataCore
{
	public int Id { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public int ItemCount { get; private set; }

	public bool CheckUnits { get; private set; }

	public bool CheckWagon { get; private set; }

	public string CheckSearch { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.PrimaryObjectiveId = (global::PrimaryObjectiveId)reader.GetInt32(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
		this.ItemCount = reader.GetInt32(3);
		this.CheckUnits = (reader.GetInt32(4) != 0);
		this.CheckWagon = (reader.GetInt32(5) != 0);
		this.CheckSearch = reader.GetString(6);
	}
}
