using System;
using Mono.Data.Sqlite;

public class ItemMutationData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public int DamageMin { get; private set; }

	public int DamageMax { get; private set; }

	public global::ItemSpeedId ItemSpeedId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.MutationId = (global::MutationId)reader.GetInt32(2);
		this.DamageMin = reader.GetInt32(3);
		this.DamageMax = reader.GetInt32(4);
		this.ItemSpeedId = (global::ItemSpeedId)reader.GetInt32(5);
	}
}
