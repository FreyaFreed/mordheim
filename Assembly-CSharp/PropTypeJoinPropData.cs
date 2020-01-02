using System;
using Mono.Data.Sqlite;

public class PropTypeJoinPropData : global::DataCore
{
	public global::PropTypeId PropTypeId { get; private set; }

	public global::PropId PropId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.PropTypeId = (global::PropTypeId)reader.GetInt32(0);
		this.PropId = (global::PropId)reader.GetInt32(1);
	}
}
