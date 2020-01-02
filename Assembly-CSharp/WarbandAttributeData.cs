using System;
using Mono.Data.Sqlite;

public class WarbandAttributeData : global::DataCore
{
	public global::WarbandAttributeId Id { get; private set; }

	public string Name { get; private set; }

	public int BaseValue { get; private set; }

	public bool Persistent { get; private set; }

	public bool CheckAchievement { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandAttributeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.BaseValue = reader.GetInt32(2);
		this.Persistent = (reader.GetInt32(3) != 0);
		this.CheckAchievement = (reader.GetInt32(4) != 0);
		this.AttributeId = (global::AttributeId)reader.GetInt32(5);
	}
}
