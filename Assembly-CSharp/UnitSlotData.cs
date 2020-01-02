using System;
using Mono.Data.Sqlite;

public class UnitSlotData : global::DataCore
{
	public global::UnitSlotId Id { get; private set; }

	public string Name { get; private set; }

	public global::UnitSlotTypeId UnitSlotTypeId { get; private set; }

	public global::BoneId BoneId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitSlotId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.UnitSlotTypeId = (global::UnitSlotTypeId)reader.GetInt32(2);
		this.BoneId = (global::BoneId)reader.GetInt32(3);
	}
}
