using System;
using Mono.Data.Sqlite;

public class WarbandSkillFreeOutsiderItemData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandSkillFreeOutsiderId WarbandSkillFreeOutsiderId { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandSkillFreeOutsiderId = (global::WarbandSkillFreeOutsiderId)reader.GetInt32(1);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(2);
		this.ItemId = (global::ItemId)reader.GetInt32(3);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(4);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(5);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(6);
	}
}
