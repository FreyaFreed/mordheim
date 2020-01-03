using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbandMenuController
{
	public WarbandMenuController(global::WarbandSave save)
	{
		this.Warband = new global::Warband(save);
		this.unitCtrlrs = new global::System.Collections.Generic.List<global::UnitMenuController>();
		this.InitFactions();
		this.hireableUnits = new global::System.Collections.Generic.List<global::UnitMenuController>();
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.PROFILE_RANK_UP, new global::DelReceiveNotice(this.OnProfileRankUp));
		this.asyncQueue = 0;
	}

	public global::Warband Warband { get; private set; }

	public global::FactionMenuController PrimaryFactionController { get; private set; }

	public global::System.Collections.Generic.List<global::Unit> Units
	{
		get
		{
			return this.Warband.Units;
		}
	}

	private void OnProfileRankUp()
	{
		this.Warband.RefreshPlayerSkills(true);
	}

	private void InitFactions()
	{
		this.factionCtrlrs = new global::System.Collections.Generic.List<global::FactionMenuController>();
		for (int i = 0; i < this.Warband.Factions.Count; i++)
		{
			global::FactionMenuController factionMenuController = new global::FactionMenuController(this.Warband.Factions[i], this.Warband.GetWarbandSave());
			this.factionCtrlrs.Add(factionMenuController);
			if (this.Warband.Factions[i].Primary)
			{
				this.PrimaryFactionController = factionMenuController;
			}
		}
	}

	public void RefreshFactions()
	{
		for (int i = 0; i < this.factionCtrlrs.Count; i++)
		{
			this.factionCtrlrs[i].Refresh();
		}
	}

	private int UnitHireOrderCompare(global::UnitMenuController a, global::UnitMenuController b)
	{
		int num = b.unit.GetHireCost() - a.unit.GetHireCost();
		if (num == 0)
		{
			num = b.unit.Id - a.unit.Id;
		}
		return num;
	}

	public global::System.Collections.Generic.List<global::UnitMenuController> GetHireableUnits(int slotIndex, bool isImpressive)
	{
		global::System.Collections.Generic.List<global::UnitMenuController> list = new global::System.Collections.Generic.List<global::UnitMenuController>();
		for (int i = 0; i < this.hireableUnits.Count; i++)
		{
			if (!this.Warband.IsActiveWarbandSlot(slotIndex) || !this.Warband.IsUnitCountExceeded(this.hireableUnits[i].unit, -1))
			{
				if (isImpressive)
				{
					if (this.hireableUnits[i].unit.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE)
					{
						list.Add(this.hireableUnits[i]);
					}
				}
				else if (this.hireableUnits[i].unit.GetUnitTypeId() != global::UnitTypeId.IMPRESSIVE && this.Warband.CanPlaceUnitAt(this.hireableUnits[i].unit, slotIndex) && (!this.Warband.IsActiveWarbandSlot(slotIndex) || !this.Warband.IsUnitCountExceeded(this.hireableUnits[i].unit, slotIndex)))
				{
					list.Add(this.hireableUnits[i]);
				}
			}
		}
		list.Sort(new global::System.Comparison<global::UnitMenuController>(this.UnitHireOrderCompare));
		return list;
	}

	public global::System.Collections.Generic.List<global::UnitMenuController> GetHiredSwordUnits()
	{
		global::System.Collections.Generic.List<global::UnitMenuController> list = new global::System.Collections.Generic.List<global::UnitMenuController>();
		for (int i = 0; i < this.hireableUnits.Count; i++)
		{
			if (this.hireableUnits[i].unit.Rank > 0)
			{
				list.Add(this.hireableUnits[i]);
			}
		}
		list.Sort(new global::System.Comparison<global::UnitMenuController>(this.UnitHireOrderCompare));
		return list;
	}

	public void HireUnit(global::UnitMenuController currentUnit)
	{
		this.hireableUnits.Remove(currentUnit);
		this.unitCtrlrs.Add(currentUnit);
		if (currentUnit.unit.UnitSave.isOutsider)
		{
			this.Warband.HireOutsider(currentUnit.unit);
		}
		else
		{
			this.Warband.HireUnit(currentUnit.unit);
		}
		this.GenerateHireableUnits();
	}

	public void GenerateBanner()
	{
		this.asyncQueue++;
		string bannerName = this.Warband.GetBannerName();
		this.GenerateBanner_(bannerName, delegate(global::UnityEngine.GameObject go)
		{
			this.banner = go;
		});
	}

	public void GenerateBanner_(string bannerName, global::System.Action<global::UnityEngine.GameObject> cb)
	{
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/banners/", global::AssetBundleId.PROPS, bannerName + ".prefab", delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject obj = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)go);
			cb(obj);
			this.FinishedLoadingItem();
		});
	}

	public static void GenerateBanner(string bannerName, global::System.Action<global::UnityEngine.GameObject> cb)
	{
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/banners/", global::AssetBundleId.PROPS, bannerName + ".prefab", delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject obj = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)go);
			cb(obj);
		});
	}

	public void GenerateIdol()
	{
		this.asyncQueue++;
		string text = this.Warband.GetIdolName() + "_menu";
		global::PandoraDebug.LogDebug("Instantiating Idol = " + text, "uncategorised", null);
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/idols/", global::AssetBundleId.PROPS, text + ".prefab", delegate(global::UnityEngine.Object go)
		{
			this.idol = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)go);
			this.FinishedLoadingItem();
		});
	}

	public void GenerateMap()
	{
		this.asyncQueue++;
		string text = this.Warband.GetMapName() + "_menu";
		global::PandoraDebug.LogDebug("Instantiating Map = " + text, "uncategorised", null);
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/maps/", global::AssetBundleId.PROPS, text + ".prefab", delegate(global::UnityEngine.Object mapPrefab)
		{
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(mapPrefab);
			this.map = gameObject.GetComponent<global::HideoutMissionMap>();
			this.FinishedLoadingItem();
		});
		string str = this.Warband.GetMapPawnName() + "_menu";
		global::PandoraDebug.LogDebug("Instantiating Pawn = " + text, "uncategorised", null);
		this.asyncQueue++;
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/maps/", global::AssetBundleId.PROPS, str + ".prefab", delegate(global::UnityEngine.Object go)
		{
			this.mapPawn = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			this.FinishedLoadingItem();
		});
	}

	public void Destroy()
	{
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.unitCtrlrs[i].gameObject);
		}
		this.unitCtrlrs.Clear();
	}

	public bool HasLeader(bool needToBeActive)
	{
		return this.Warband.HasLeader(needToBeActive);
	}

	public global::UnitMenuController GetLeader()
	{
		global::UnitMenuController result = null;
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (this.unitCtrlrs[i].unit.UnitSave.warbandSlotIndex == 2)
			{
				if (this.unitCtrlrs[i].unit.GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE)
				{
					return this.unitCtrlrs[i];
				}
				result = this.unitCtrlrs[i];
			}
			else if (this.unitCtrlrs[i].unit.IsLeader)
			{
				result = this.unitCtrlrs[i];
			}
		}
		return result;
	}

	public global::UnitMenuController GetDramatis()
	{
		return this.dramatis;
	}

	public void SetBannerWagon()
	{
		this.GenerateBanner();
		this.GenerateIdol();
		this.asyncQueue++;
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/", global::AssetBundleId.PROPS, this.Warband.WarbandData.Wagon + "_menu.prefab", delegate(global::UnityEngine.Object go)
		{
			this.wagon = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)go);
			global::InteractivePoint[] componentsInChildren = this.wagon.GetComponentsInChildren<global::InteractivePoint>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				global::UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
			this.FinishedLoadingItem();
		});
	}

	public global::System.Collections.Generic.List<global::Item> Disband(global::Unit unit, global::EventLogger.LogEvent reason, int data)
	{
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (this.unitCtrlrs[i].unit == unit)
			{
				this.unitCtrlrs.RemoveAt(i);
				break;
			}
		}
		global::UnityEngine.Debug.LogWarning("Disband: unitctrl does not exist for this unit");
		global::System.Collections.Generic.List<global::Item> list = this.Warband.Disband(unit);
		switch (reason)
		{
		case global::EventLogger.LogEvent.FIRE:
		case global::EventLogger.LogEvent.DEATH:
		case global::EventLogger.LogEvent.RETIREMENT:
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(list);
			break;
		}
		unit.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, reason, data);
		return list;
	}

	public void DisbandWarband()
	{
		while (this.unitCtrlrs.Count > 0)
		{
			this.Disband(this.unitCtrlrs[0].unit, global::EventLogger.LogEvent.FIRE, 0);
		}
	}

	public global::System.Collections.IEnumerator GenerateUnits()
	{
		this.Destroy();
		for (int i = 0; i < this.Units.Count; i++)
		{
			this.asyncQueue++;
			global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::UnitMenuController.LoadUnitPrefabAsync(this.Units[i], delegate(global::UnityEngine.GameObject go)
			{
				global::UnitMenuController component = go.GetComponent<global::UnitMenuController>();
				this.unitCtrlrs.Add(component);
				component.transform.localPosition = new global::UnityEngine.Vector3((float)this.unitCtrlrs.Count * 5f, 0f, 0f);
			}, new global::System.Action(this.FinishedLoadingItem)));
			yield return null;
		}
		this.unitCtrlrs.Sort(new global::CompareUnitMenuController());
		this.asyncQueue++;
		global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::UnitMenuController.LoadUnitPrefabAsync(new global::Unit(new global::UnitSave(this.Warband.WarbandData.UnitIdDramatis)), delegate(global::UnityEngine.GameObject go)
		{
			this.dramatis = go.GetComponent<global::UnitMenuController>();
		}, new global::System.Action(this.FinishedLoadingItem)));
		yield return null;
		this.GenerateHireableUnits();
		yield break;
	}

	private void FinishedLoadingItem()
	{
		this.asyncQueue--;
		if (this.asyncQueue == 0 && !global::PandoraSingleton<global::HideoutManager>.Instance.startedLoading)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
			global::PandoraSingleton<global::HideoutManager>.Instance.finishedLoading = true;
		}
	}

	public void GenerateHireableUnits()
	{
		if (this.generatingHireable)
		{
			return;
		}
		this.generatingHireable = true;
		for (int i = this.hireableUnits.Count - 1; i >= 0; i--)
		{
			if (this.hireableUnits[i].unit.UnitSave.isOutsider && !this.Warband.Outsiders.Contains(this.hireableUnits[i].unit))
			{
				global::UnityEngine.Object.Destroy(this.hireableUnits[i].gameObject);
				this.hireableUnits.RemoveAt(i);
			}
		}
		for (int j = 0; j < this.Warband.HireableUnitIds.Count; j++)
		{
			global::UnitId unitId = this.Warband.HireableUnitIds[j];
			if (!this.IsUnitGenerated(unitId))
			{
				if ((unitId != global::UnitId.SMUGGLER || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.SMUGGLER)) && (unitId != global::UnitId.GLOBADIER || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.GLOBADIER)) && (unitId != global::UnitId.PRIEST_OF_ULRIC || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.PRIEST_OF_ULRIC)) && (unitId != global::UnitId.DOOMWEAVER || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.DOOMWEAVER)))
				{
					int rank = this.Warband.Rank;
					Unit unit = Unit.GenerateHireUnit(unitId, 0, rank);				
					
					this.asyncQueue++;
					int index = j;
					global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::UnitMenuController.LoadUnitPrefabAsync(unit, delegate(global::UnityEngine.GameObject go)
					{
						global::UnitMenuController component = go.GetComponent<global::UnitMenuController>();
						this.hireableUnits.Add(component);
						component.transform.localPosition = new global::UnityEngine.Vector3((float)index * 5f, 0f, 0f);
					}, new global::System.Action(this.FinishedLoadingItem)));
				}
			}
		}
		for (int k = 0; k < this.Warband.Outsiders.Count; k++)
		{
			if (!this.IsOutsiderGenerated(this.Warband.Outsiders[k]))
			{
				if ((this.Warband.Outsiders[k].Id != global::UnitId.SMUGGLER || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.SMUGGLER)) && (this.Warband.Outsiders[k].Id != global::UnitId.GLOBADIER || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.GLOBADIER)) && (this.Warband.Outsiders[k].Id != global::UnitId.PRIEST_OF_ULRIC || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.PRIEST_OF_ULRIC)) && (this.Warband.Outsiders[k].Id != global::UnitId.DOOMWEAVER || global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.DOOMWEAVER)))
				{
					this.asyncQueue++;
					int index = k;
					global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::UnitMenuController.LoadUnitPrefabAsync(this.Warband.Outsiders[index], delegate(global::UnityEngine.GameObject go)
					{
						global::UnitMenuController component = go.GetComponent<global::UnitMenuController>();
						this.hireableUnits.Add(component);
						component.transform.localPosition = new global::UnityEngine.Vector3((float)index * 5f, 0f, 10f);
					}, new global::System.Action(this.FinishedLoadingItem)));
				}
			}
		}
	}

	public bool IsUnitGenerated(global::UnitId unitId)
	{
		for (int i = 0; i < this.hireableUnits.Count; i++)
		{
			if (!this.hireableUnits[i].unit.UnitSave.isOutsider && this.hireableUnits[i].unit.Id == unitId)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsOutsiderGenerated(global::Unit unit)
	{
		for (int i = 0; i < this.hireableUnits.Count; i++)
		{
			if (this.hireableUnits[i].unit.UnitSave.isOutsider && this.hireableUnits[i].unit == unit)
			{
				return true;
			}
		}
		return false;
	}

	public global::Unit GetLeaderUnit()
	{
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].IsActiveLeader)
			{
				return this.Units[i];
			}
		}
		return null;
	}

	public global::System.Collections.Generic.List<global::Unit> GetUnitsStatus(global::UnitActiveStatusId status)
	{
		global::System.Collections.Generic.List<global::Unit> list = new global::System.Collections.Generic.List<global::Unit>();
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].GetActiveStatus() == status)
			{
				list.Add(this.Units[i]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::Unit> GetActiveUnits()
	{
		global::System.Collections.Generic.List<global::Unit> list = new global::System.Collections.Generic.List<global::Unit>();
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].Active)
			{
				list.Add(this.Units[i]);
			}
		}
		return list;
	}

	public int GetActiveUnitsCount()
	{
		return this.Warband.GetUnitsCount(true);
	}

	public global::System.Collections.Generic.List<global::UnitSave> GetActiveUnitsSave()
	{
		global::System.Collections.Generic.List<global::UnitSave> list = new global::System.Collections.Generic.List<global::UnitSave>();
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].Active)
			{
				list.Add(this.Units[i].UnitSave);
			}
		}
		return list;
	}

	public string[] GetActiveUnitsSerialized()
	{
		global::System.Collections.Generic.List<global::UnitSave> activeUnitsSave = this.GetActiveUnitsSave();
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		for (int i = 0; i < activeUnitsSave.Count; i++)
		{
			list.Add(global::Thoth.WriteToString(activeUnitsSave[i]));
		}
		return list.ToArray();
	}

	public int GetSkirmishRating(global::System.Collections.Generic.List<int> unitsPosition)
	{
		int num = 0;
		if (unitsPosition != null)
		{
			for (int i = 0; i < unitsPosition.Count; i++)
			{
				if (this.Warband.IsActiveWarbandSlot(unitsPosition[i]))
				{
					num += this.Units[i].GetRating();
				}
			}
		}
		else
		{
			num = this.Warband.GetRating();
		}
		return num;
	}

	public void GetSkirmishInfo(global::System.Collections.Generic.List<int> unitsPosition, out int rating, out string[] serializedUnits)
	{
		if (unitsPosition != null)
		{
			rating = 0;
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			for (int i = 0; i < unitsPosition.Count; i++)
			{
				if (this.Warband.IsActiveWarbandSlot(unitsPosition[i]))
				{
					rating += this.Units[i].GetRating();
					list.Add(global::Thoth.WriteToString(this.Units[i].UnitSave));
				}
			}
			serializedUnits = list.ToArray();
		}
		else
		{
			rating = this.Warband.GetRating();
			serializedUnits = this.GetActiveUnitsSerialized();
		}
	}

	public global::System.Collections.Generic.List<global::RuneMark> GetAvailableRuneMarks(global::UnitSlotId unitSlotId, global::Item item, out string reason, ref global::System.Collections.Generic.List<global::RuneMark> availableRuneMarks, ref global::System.Collections.Generic.List<global::RuneMark> notAvailableRuneMarks)
	{
		global::WarbandMenuController.<GetAvailableRuneMarks>c__AnonStorey14B <GetAvailableRuneMarks>c__AnonStorey14B = new global::WarbandMenuController.<GetAvailableRuneMarks>c__AnonStorey14B();
		availableRuneMarks.Clear();
		notAvailableRuneMarks.Clear();
		global::System.Collections.Generic.List<global::RuneMark> result = new global::System.Collections.Generic.List<global::RuneMark>();
		reason = string.Empty;
		global::System.Collections.Generic.List<global::RuneMarkJoinItemTypeData> list = new global::System.Collections.Generic.List<global::RuneMarkJoinItemTypeData>();
		if (item.Id == global::ItemId.NONE)
		{
			if (unitSlotId == global::UnitSlotId.SET2_MAINHAND || unitSlotId == global::UnitSlotId.SET2_OFFHAND)
			{
				unitSlotId -= 2;
			}
			global::System.Collections.Generic.List<global::ItemJoinUnitSlotData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemJoinUnitSlotData>("fk_unit_slot_id", unitSlotId.ToIntString<global::UnitSlotId>());
			for (int l = 0; l < list2.Count; l++)
			{
				global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)list2[l].ItemId);
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].ItemTypeId == itemData.ItemTypeId)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.AddRange(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkJoinItemTypeData>("fk_item_type_id", itemData.ItemTypeId.ToIntString<global::ItemTypeId>()));
				}
			}
		}
		else
		{
			list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkJoinItemTypeData>("fk_item_type_id", item.TypeData.Id.ToIntString<global::ItemTypeId>());
		}
		global::System.Collections.Generic.List<global::RuneMarkQualityData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkQualityData>();
		if (item.QualityData.RuneMarkQualityIdMax == global::RuneMarkQualityId.NONE)
		{
			reason = "na_enchant_no_slot";
		}
		else if (item.RuneMark != null && item.RuneMark.Data.Id != global::RuneMarkId.NONE)
		{
			reason = "na_enchant_used_slot";
		}
		<GetAvailableRuneMarks>c__AnonStorey14B.runeMarksData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkData>("released", "1");
		int i;
		for (i = 0; i < <GetAvailableRuneMarks>c__AnonStorey14B.runeMarksData.Count; i++)
		{
			if (<GetAvailableRuneMarks>c__AnonStorey14B.runeMarksData[i].Id != global::RuneMarkId.NONE && list.Exists((global::RuneMarkJoinItemTypeData x) => x.RuneMarkId == <GetAvailableRuneMarks>c__AnonStorey14B.runeMarksData[i].Id))
			{
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].Id != global::RuneMarkQualityId.NONE)
					{
						string text = reason;
						global::System.Collections.Generic.List<global::RuneMarkRecipeData> list4 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkRecipeData>("fk_rune_mark_id", ((int)<GetAvailableRuneMarks>c__AnonStorey14B.runeMarksData[i].Id).ToConstantString(), "fk_rune_mark_quality_id", ((int)list3[k].Id).ToConstantString());
						if (list4 != null && list4.Count > 0)
						{
							if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.HasItem(list4[0].ItemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE))
							{
								text = "na_enchant_not_found";
							}
							else if (list3[k].Id > item.QualityData.RuneMarkQualityIdMax)
							{
								text = "na_enchant_quality";
							}
							global::RuneMark item2 = new global::RuneMark(<GetAvailableRuneMarks>c__AnonStorey14B.runeMarksData[i].Id, list3[k].Id, this.Warband.WarbandData.AllegianceId, item.TypeData.Id, text);
							if (string.IsNullOrEmpty(text))
							{
								availableRuneMarks.Add(item2);
							}
							else
							{
								notAvailableRuneMarks.Add(item2);
							}
						}
					}
				}
			}
		}
		return result;
	}

	public static int Compare(global::UnitMenuController x, global::UnitMenuController y)
	{
		if (x.unit.IsLeader && !y.unit.IsLeader)
		{
			return -1;
		}
		if (!x.unit.IsLeader && y.unit.IsLeader)
		{
			return 1;
		}
		if (x.unit.IsHero() && !y.unit.IsHero())
		{
			return -1;
		}
		if (!x.unit.IsHero() && y.unit.IsHero())
		{
			return 1;
		}
		if (x.unit.GetUnitTypeId() == global::UnitTypeId.HENCHMEN && y.unit.GetUnitTypeId() != global::UnitTypeId.HENCHMEN)
		{
			return -1;
		}
		if (x.unit.GetUnitTypeId() != global::UnitTypeId.HENCHMEN && y.unit.GetUnitTypeId() == global::UnitTypeId.HENCHMEN)
		{
			return 1;
		}
		if (x.unit.GetRating() > y.unit.GetRating())
		{
			return -1;
		}
		if (x.unit.GetRating() < y.unit.GetRating())
		{
			return 1;
		}
		if (x.unit.Xp > y.unit.Xp)
		{
			return -1;
		}
		if (x.unit.Xp < y.unit.Xp)
		{
			return 1;
		}
		return 0;
	}

	public global::UnitMenuController GetUnitByName(string unitName)
	{
		for (int i = 0; i < this.unitCtrlrs.Count; i++)
		{
			if (string.CompareOrdinal(this.unitCtrlrs[i].unit.Name, unitName) == 0)
			{
				return this.unitCtrlrs[i];
			}
		}
		return null;
	}

	public global::Unit GetLowestRankUnit(global::UnitTypeId unitTypeId, global::InjuryData consequenceDataInjuryId)
	{
		global::System.Collections.Generic.List<global::InjuryId> toExcludes = new global::System.Collections.Generic.List<global::InjuryId>();
		global::Unit unit = null;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].GetUnitTypeId() == unitTypeId)
			{
				if (this.Units[i].CanRollInjury(consequenceDataInjuryId, toExcludes, this.Units[i].GetInjuryModifiers()))
				{
					if (unit == null || this.Units[i].Rank < unit.Rank || (this.Units[i].Rank == unit.Rank && this.Units[i].GetRating() < unit.GetRating()))
					{
						unit = this.Units[i];
					}
				}
			}
		}
		return unit;
	}

	public global::Unit GetHighestRankUnit(global::UnitTypeId unitTypeId, global::InjuryData consequenceDataInjuryId)
	{
		global::Unit unit = null;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].GetUnitTypeId() == unitTypeId && (unit == null || this.Units[i].Rank > unit.Rank || (this.Units[i].Rank == unit.Rank && this.Units[i].GetRating() > unit.GetRating())))
			{
				unit = this.Units[i];
			}
		}
		return unit;
	}

	public void CheckUnitStatus()
	{
		int num = 0;
		for (int i = 0; i < this.Warband.Units.Count; i++)
		{
			global::UnitActiveStatusId activeStatus = this.Warband.Units[i].GetActiveStatus();
			if (activeStatus == global::UnitActiveStatusId.TREATMENT_NOT_PAID || activeStatus == global::UnitActiveStatusId.INJURED || activeStatus == global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID)
			{
				num++;
			}
		}
		if (num >= 6)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.MULTIPLE_INJURED);
		}
	}

	public global::UnityEngine.GameObject banner;

	public global::UnityEngine.GameObject wagon;

	public global::UnityEngine.GameObject idol;

	public global::HideoutMissionMap map;

	public global::UnityEngine.GameObject mapPawn;

	public global::UnitMenuController dramatis;

	public global::System.Collections.Generic.List<global::UnitMenuController> unitCtrlrs;

	public global::System.Collections.Generic.List<global::UnitMenuController> hireableUnits;

	public global::System.Collections.Generic.List<global::FactionMenuController> factionCtrlrs;

	public int teamIndex;

	public bool generatingHireable;

	private int asyncQueue;
}
