using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class InjuryData : global::DataCore
{
	public global::InjuryId Id { get; private set; }

	public string Name { get; private set; }

	public int Ratio { get; private set; }

	public int Duration { get; private set; }

	public int Rating { get; private set; }

	public int RepeatLimit { get; private set; }

	public global::BodyPartId BodyPartId { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public bool Released { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::InjuryId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Ratio = reader.GetInt32(2);
		this.Duration = reader.GetInt32(3);
		this.Rating = reader.GetInt32(4);
		this.RepeatLimit = reader.GetInt32(5);
		this.BodyPartId = (global::BodyPartId)reader.GetInt32(6);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(7);
		this.Released = (reader.GetInt32(8) != 0);
	}

	public static global::InjuryData GetRandomRatio(global::System.Collections.Generic.List<global::InjuryData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<global::InjuryId, int> modifiers = null)
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
