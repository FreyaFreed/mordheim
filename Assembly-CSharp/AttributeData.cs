using System;
using Mono.Data.Sqlite;

public class AttributeData : global::DataCore
{
	public global::AttributeId Id { get; private set; }

	public string Name { get; private set; }

	public int BaseRoll { get; private set; }

	public global::AttributeTypeId AttributeTypeId { get; private set; }

	public int Rating { get; private set; }

	public bool Persistent { get; private set; }

	public bool IsPercent { get; private set; }

	public global::AttributeId AttributeIdMax { get; private set; }

	public global::AttributeId AttributeIdModifier { get; private set; }

	public bool IsBaseRoll { get; private set; }

	public bool CheckAchievement { get; private set; }

	public bool Save { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AttributeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.BaseRoll = reader.GetInt32(2);
		this.AttributeTypeId = (global::AttributeTypeId)reader.GetInt32(3);
		this.Rating = reader.GetInt32(4);
		this.Persistent = (reader.GetInt32(5) != 0);
		this.IsPercent = (reader.GetInt32(6) != 0);
		this.AttributeIdMax = (global::AttributeId)reader.GetInt32(7);
		this.AttributeIdModifier = (global::AttributeId)reader.GetInt32(8);
		this.IsBaseRoll = (reader.GetInt32(9) != 0);
		this.CheckAchievement = (reader.GetInt32(10) != 0);
		this.Save = (reader.GetInt32(11) != 0);
	}
}
