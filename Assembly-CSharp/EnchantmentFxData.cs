using System;
using Mono.Data.Sqlite;

public class EnchantmentFxData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public string Fx { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.Fx = reader.GetString(2);
	}
}
