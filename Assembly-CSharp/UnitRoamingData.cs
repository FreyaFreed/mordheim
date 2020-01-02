using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class UnitRoamingData : global::DataCore
{
	public global::UnitRoamingId Id { get; private set; }

	public string Name { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public global::AiProfileId AiProfileId { get; private set; }

	public int Ratio { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitRoamingId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.UnitId = (global::UnitId)reader.GetInt32(2);
		this.AiProfileId = (global::AiProfileId)reader.GetInt32(3);
		this.Ratio = reader.GetInt32(4);
	}

	public static global::UnitRoamingData GetRandomRatio(global::System.Collections.Generic.List<global::UnitRoamingData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<global::UnitRoamingId, int> modifiers = null)
	{
		int num = 0;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < datas.Count; i++)
		{
			int num2 = datas[i].Ratio;
			if (modifiers != null && modifiers.ContainsKey(datas[i].Id))
			{
				num2 = global::UnityEngine.Mathf.Clamp(num2 + modifiers[datas[i].Id], 0, int.MaxValue);
			}
			num += num2;
			list.Add(num);
		}
		int num3 = tyche.Rand(0, num);
		for (int j = 0; j < list.Count; j++)
		{
			if (num3 < list[j])
			{
				return datas[j];
			}
		}
		return null;
	}
}
