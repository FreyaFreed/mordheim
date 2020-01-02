using System;
using Mono.Data.Sqlite;

public class SkillItemData : global::DataCore
{
	public int Id { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
		this.MutationId = (global::MutationId)reader.GetInt32(3);
	}
}
