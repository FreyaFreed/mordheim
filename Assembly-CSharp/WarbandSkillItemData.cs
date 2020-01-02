using System;
using Mono.Data.Sqlite;

public class WarbandSkillItemData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public int Quantity { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(3);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(4);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(5);
		this.Quantity = reader.GetInt32(6);
	}
}
