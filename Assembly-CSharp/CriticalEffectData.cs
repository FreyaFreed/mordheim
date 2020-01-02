using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class CriticalEffectData : global::DataCore
{
	public global::CriticalEffectId Id { get; private set; }

	public string Name { get; private set; }

	public int Ratio { get; private set; }

	public bool ApplyDebuf { get; private set; }

	public global::UnitStateId UnitStateId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CriticalEffectId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Ratio = reader.GetInt32(2);
		this.ApplyDebuf = (reader.GetInt32(3) != 0);
		this.UnitStateId = (global::UnitStateId)reader.GetInt32(4);
	}

	public static global::CriticalEffectData GetRandomRatio(global::System.Collections.Generic.List<global::CriticalEffectData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<global::CriticalEffectId, int> modifiers = null)
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
