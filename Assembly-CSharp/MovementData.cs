﻿using System;
using Mono.Data.Sqlite;

public class MovementData : global::DataCore
{
	public global::MovementId Id { get; private set; }

	public string Name { get; private set; }

	public int Distance { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::MovementId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Distance = reader.GetInt32(2);
	}
}
