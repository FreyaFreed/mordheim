using System;
using Mono.Data.Sqlite;

public class TableFilterJoinTableListData : global::DataCore
{
	public global::TableFilterId TableFilterId { get; private set; }

	public global::TableListId TableListId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.TableFilterId = (global::TableFilterId)reader.GetInt32(0);
		this.TableListId = (global::TableListId)reader.GetInt32(1);
	}
}
