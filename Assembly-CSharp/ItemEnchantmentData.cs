using System;
using Mono.Data.Sqlite;

public class ItemEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(2);
		this.MutationId = (global::MutationId)reader.GetInt32(3);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(4);
	}
}
