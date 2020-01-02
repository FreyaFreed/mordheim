using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class BuildingTypeJoinBuildingData : global::DataCore
{
	public global::BuildingTypeId BuildingTypeId { get; private set; }

	public global::BuildingId BuildingId { get; private set; }

	public int Ratio { get; private set; }

	public bool Flippable { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.BuildingTypeId = (global::BuildingTypeId)reader.GetInt32(0);
		this.BuildingId = (global::BuildingId)reader.GetInt32(1);
		this.Ratio = reader.GetInt32(2);
		this.Flippable = (reader.GetInt32(3) != 0);
	}

	public static global::BuildingTypeJoinBuildingData GetRandomRatio(global::System.Collections.Generic.List<global::BuildingTypeJoinBuildingData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<global::BuildingTypeId, int> modifiers = null)
	{
		int num = 0;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < datas.Count; i++)
		{
			int num2 = datas[i].Ratio;
			if (modifiers != null && modifiers.ContainsKey(datas[i].BuildingTypeId))
			{
				num2 = global::UnityEngine.Mathf.Clamp(num2 + modifiers[datas[i].BuildingTypeId], 0, int.MaxValue);
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
