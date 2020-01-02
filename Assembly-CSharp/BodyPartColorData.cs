using System;
using Mono.Data.Sqlite;

public class BodyPartColorData : global::DataCore
{
	public int Id { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public global::ColorPresetId ColorPresetId { get; private set; }

	public global::BodyPartId BodyPartId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public string Color { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.UnitId = (global::UnitId)reader.GetInt32(1);
		this.ColorPresetId = (global::ColorPresetId)reader.GetInt32(2);
		this.BodyPartId = (global::BodyPartId)reader.GetInt32(3);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(4);
		this.Color = reader.GetString(5);
	}
}
