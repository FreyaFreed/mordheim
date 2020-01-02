using System;
using Mono.Data.Sqlite;

public class PropRestrictionJoinPropTypeData : global::DataCore
{
	public global::PropRestrictionId PropRestrictionId { get; private set; }

	public global::PropTypeId PropTypeId { get; private set; }

	public int MaxProp { get; private set; }

	public int MaxPercentage { get; private set; }

	public int MinDistance { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.PropRestrictionId = (global::PropRestrictionId)reader.GetInt32(0);
		this.PropTypeId = (global::PropTypeId)reader.GetInt32(1);
		this.MaxProp = reader.GetInt32(2);
		this.MaxPercentage = reader.GetInt32(3);
		this.MinDistance = reader.GetInt32(4);
	}
}
