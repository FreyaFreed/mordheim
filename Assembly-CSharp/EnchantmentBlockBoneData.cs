using System;
using Mono.Data.Sqlite;

public class EnchantmentBlockBoneData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::BoneId BoneId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.BoneId = (global::BoneId)reader.GetInt32(2);
	}
}
