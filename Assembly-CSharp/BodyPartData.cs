using System;
using Mono.Data.Sqlite;

public class BodyPartData : global::DataCore
{
	public global::BodyPartId Id { get; private set; }

	public string Name { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public bool Customizable { get; private set; }

	public bool Empty { get; private set; }

	public bool Skinnable { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::BodyPartId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(2);
		this.Customizable = (reader.GetInt32(3) != 0);
		this.Empty = (reader.GetInt32(4) != 0);
		this.Skinnable = (reader.GetInt32(5) != 0);
	}
}
