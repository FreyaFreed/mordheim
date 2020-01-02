using System;
using Mono.Data.Sqlite;

public class ShrineEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::ShrineId ShrineId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::WarbandRankId WarbandRankId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ShrineId = (global::ShrineId)reader.GetInt32(1);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(2);
		this.WarbandRankId = (global::WarbandRankId)reader.GetInt32(3);
	}
}
