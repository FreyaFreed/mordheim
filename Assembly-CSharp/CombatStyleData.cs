using System;
using Mono.Data.Sqlite;

public class CombatStyleData : global::DataCore
{
	public global::CombatStyleId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemTypeId ItemTypeIdMain { get; private set; }

	public global::UnitSlotId UnitSlotIdMain { get; private set; }

	public global::ItemTypeId ItemTypeIdOff { get; private set; }

	public global::UnitSlotId UnitSlotIdOff { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CombatStyleId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemTypeIdMain = (global::ItemTypeId)reader.GetInt32(2);
		this.UnitSlotIdMain = (global::UnitSlotId)reader.GetInt32(3);
		this.ItemTypeIdOff = (global::ItemTypeId)reader.GetInt32(4);
		this.UnitSlotIdOff = (global::UnitSlotId)reader.GetInt32(5);
	}
}
