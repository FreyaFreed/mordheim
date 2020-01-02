using System;
using Mono.Data.Sqlite;

public class MutationGroupData : global::DataCore
{
	public global::MutationGroupId Id { get; private set; }

	public string Name { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public bool EmptyLinkedBodyPart { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::MutationGroupId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(2);
		this.EmptyLinkedBodyPart = (reader.GetInt32(3) != 0);
	}
}
