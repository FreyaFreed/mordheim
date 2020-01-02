using System;
using Mono.Data.Sqlite;

public class EnchantmentJoinAttributeData : global::DataCore
{
	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(0);
		this.AttributeId = (global::AttributeId)reader.GetInt32(1);
		this.Modifier = reader.GetInt32(2);
	}
}
