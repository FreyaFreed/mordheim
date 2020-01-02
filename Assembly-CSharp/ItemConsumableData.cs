using System;
using Mono.Data.Sqlite;

public class ItemConsumableData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public bool ConsumeItem { get; private set; }

	public bool OutOfCombat { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(2);
		this.SkillId = (global::SkillId)reader.GetInt32(3);
		this.ConsumeItem = (reader.GetInt32(4) != 0);
		this.OutOfCombat = (reader.GetInt32(5) != 0);
	}
}
