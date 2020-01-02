using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveLocate : global::Objective
{
	public ObjectiveLocate(global::PrimaryObjectiveId id) : base(id)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_primary_objective_id";
		int num = (int)id;
		this.locateData = instance.InitData<global::PrimaryObjectiveLocateData>(field, num.ToString())[0];
		this.locatedItems = new global::System.Collections.Generic.List<global::Item>();
		if (this.locateData.ItemId != global::ItemId.NONE)
		{
			this.counter = new global::UnityEngine.Vector2(0f, (float)this.locateData.ItemCount);
			this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
			{
				global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.locateData.ItemId.ToLowerString())
			});
		}
		else
		{
			this.locateZones = global::PandoraSingleton<global::MissionManager>.Instance.GetLocateZones(this.locateData.Zone);
			this.counter = new global::UnityEngine.Vector2(0f, (float)this.locateData.ZoneCount);
			this.desc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(base.DescKey, new string[]
			{
				global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.locateData.Zone)
			});
		}
	}

	public override void SetLocked(bool loc)
	{
		bool locked = base.Locked;
		base.SetLocked(loc);
		if (locked != loc && !loc && this.locateZones != null)
		{
			global::System.Collections.Generic.List<global::UnitController> myAliveUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetMyAliveUnits();
			for (int i = 0; i < this.locateZones.Count; i++)
			{
				for (int j = 0; j < myAliveUnits.Count; j++)
				{
					if (this.locateZones[i].ColliderBounds.Contains(myAliveUnits[j].transform.position))
					{
						global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().LocateZone(this.locateZones[i]);
						break;
					}
				}
			}
		}
	}

	protected override void Track(ref bool objectivesChanged)
	{
		if (this.locateData.ItemId != global::ItemId.NONE)
		{
			this.counter.x = (float)this.locatedItems.Count;
		}
	}

	public void UpdateLocatedItems(global::System.Collections.Generic.List<global::Item> items)
	{
		if (this.counter.x == this.counter.y)
		{
			return;
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Id == this.locateData.ItemId && this.locatedItems.IndexOf(items[i]) == -1)
			{
				this.locatedItems.Add(items[i]);
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateObjective(this.guid, 0U);
			}
		}
	}

	public void UpdateLocatedZone(global::LocateZone zone, bool checkEndGame = true)
	{
		if (zone.name == this.locateData.Zone)
		{
			int num = this.locateZones.IndexOf(zone);
			if (num != -1)
			{
				this.counter.x = this.counter.x + 1f;
				this.locateZones.RemoveAt(num);
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateObjective(this.guid, zone.guid);
				if (checkEndGame)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.CheckEndGame();
				}
			}
		}
	}

	public override void Reload(uint trackedUid)
	{
		if (this.locateData.ItemId != global::ItemId.NONE)
		{
			this.locatedItems.Add(new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL));
		}
		else
		{
			global::System.Collections.Generic.List<global::LocateZone> list = global::PandoraSingleton<global::MissionManager>.Instance.GetLocateZones();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].guid == trackedUid)
				{
					this.UpdateLocatedZone(list[i], false);
				}
			}
		}
	}

	private global::System.Collections.Generic.List<global::Item> locatedItems;

	private global::System.Collections.Generic.List<global::LocateZone> locateZones;

	private global::PrimaryObjectiveLocateData locateData;
}
