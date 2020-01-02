using System;
using Mono.Data.Sqlite;

public class ItemQualityJoinItemTypeData : global::DataCore
{
	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public int RatingModifier { get; private set; }

	public int DamageMinModifier { get; private set; }

	public int DamageMaxModifier { get; private set; }

	public int ArmorAbsorptionModifier { get; private set; }

	public int RangeModifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(0);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(1);
		this.RatingModifier = reader.GetInt32(2);
		this.DamageMinModifier = reader.GetInt32(3);
		this.DamageMaxModifier = reader.GetInt32(4);
		this.ArmorAbsorptionModifier = reader.GetInt32(5);
		this.RangeModifier = reader.GetInt32(6);
	}
}
