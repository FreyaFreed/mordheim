using System;
using Mono.Data.Sqlite;

public class RuneMarkEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(1);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(2);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(3);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(4);
	}
}
