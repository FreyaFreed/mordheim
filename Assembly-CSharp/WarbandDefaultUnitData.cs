using System;
using Mono.Data.Sqlite;

public class WarbandDefaultUnitData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public global::WarbandSlotTypeId WarbandSlotTypeId { get; private set; }

	public int Amount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandId = (global::WarbandId)reader.GetInt32(1);
		this.UnitId = (global::UnitId)reader.GetInt32(2);
		this.WarbandSlotTypeId = (global::WarbandSlotTypeId)reader.GetInt32(3);
		this.Amount = reader.GetInt32(4);
	}
}
