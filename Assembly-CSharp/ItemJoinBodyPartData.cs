using System;
using Mono.Data.Sqlite;

public class ItemJoinBodyPartData : global::DataCore
{
	public global::ItemId ItemId { get; private set; }

	public global::BodyPartId BodyPartId { get; private set; }

	public bool Lock { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.ItemId = (global::ItemId)reader.GetInt32(0);
		this.BodyPartId = (global::BodyPartId)reader.GetInt32(1);
		this.Lock = (reader.GetInt32(2) != 0);
	}
}
