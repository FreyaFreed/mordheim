using System;
using Mono.Data.Sqlite;

public class WyrdstoneTypeJoinWyrdstoneData : global::DataCore
{
	public global::WyrdstoneTypeId WyrdstoneTypeId { get; private set; }

	public global::WyrdstoneId WyrdstoneId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.WyrdstoneTypeId = (global::WyrdstoneTypeId)reader.GetInt32(0);
		this.WyrdstoneId = (global::WyrdstoneId)reader.GetInt32(1);
	}
}
