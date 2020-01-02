using System;
using Mono.Data.Sqlite;

public class EnchantmentQualityData : global::DataCore
{
	public global::EnchantmentQualityId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::EnchantmentQualityId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
