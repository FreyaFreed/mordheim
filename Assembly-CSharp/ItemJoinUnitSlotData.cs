using System;
using Mono.Data.Sqlite;

public class ItemJoinUnitSlotData : global::DataCore
{
	public global::ItemId ItemId { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.ItemId = (global::ItemId)reader.GetInt32(0);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(1);
	}
}
