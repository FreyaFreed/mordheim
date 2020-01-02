using System;
using Mono.Data.Sqlite;

public class BodyPartUpdateData : global::DataCore
{
	public int Id { get; private set; }

	public global::BodyPartId BodyPartId { get; private set; }

	public global::BodyPartId BodyPartIdUpdated { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.BodyPartId = (global::BodyPartId)reader.GetInt32(1);
		this.BodyPartIdUpdated = (global::BodyPartId)reader.GetInt32(2);
	}
}
