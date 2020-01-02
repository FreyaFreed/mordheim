using System;
using Mono.Data.Sqlite;

public class ZoneAoeEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::ZoneAoeId ZoneAoeId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::ZoneTriggerId ZoneTriggerId { get; private set; }

	public bool TargetSelf { get; private set; }

	public bool TargetAlly { get; private set; }

	public bool TargetEnemy { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ZoneAoeId = (global::ZoneAoeId)reader.GetInt32(1);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(2);
		this.ZoneTriggerId = (global::ZoneTriggerId)reader.GetInt32(3);
		this.TargetSelf = (reader.GetInt32(4) != 0);
		this.TargetAlly = (reader.GetInt32(5) != 0);
		this.TargetEnemy = (reader.GetInt32(6) != 0);
	}
}
