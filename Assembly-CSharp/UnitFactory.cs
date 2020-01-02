using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : global::PandoraSingleton<global::UnitFactory>
{
	public float LoadingPercent
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < this.creationQueue.Count; i++)
			{
				global::UnitFactory.LoadState state = this.creationQueue[i].state;
				if (state != global::UnitFactory.LoadState.INITIALIZING)
				{
					if (state == global::UnitFactory.LoadState.DONE)
					{
						num += 11f;
					}
				}
				else if (this.creationQueue[i].ctrlr != null)
				{
					num += 1f + this.creationQueue[i].ctrlr.GetBodypartPercentLoaded() * 10f;
				}
				else
				{
					num += 1f;
				}
			}
			return num / (float)(this.creationQueue.Count * 11);
		}
	}

	public bool IsCreating()
	{
		for (int i = 0; i < this.creationQueue.Count; i++)
		{
			if (this.creationQueue[i].state != global::UnitFactory.LoadState.DONE)
			{
				return true;
			}
		}
		return false;
	}

	private void Awake()
	{
		this.creationQueue = new global::System.Collections.Generic.List<global::UnitFactory.UnitCreationData>();
		this.prefabs = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		this.unitCreatedCount = 0;
	}

	private void Update()
	{
		int i = 0;
		while (i < this.creationQueue.Count)
		{
			switch (this.creationQueue[i].state)
			{
			case global::UnitFactory.LoadState.NONE:
				this.CreateUnit(this.creationQueue[i]);
				break;
			case global::UnitFactory.LoadState.PREFABING:
			{
				global::UnityEngine.GameObject gameObject = this.FindPrefab(this.creationQueue[i].prefab);
				if (gameObject != null)
				{
					this.InitUnit(this.creationQueue[i], gameObject);
				}
				break;
			}
			case global::UnitFactory.LoadState.INITIALIZING:
				if (this.creationQueue[i].ctrlr.Initialized)
				{
					this.UnitCreationDone(this.creationQueue[i]);
				}
				break;
			}
			IL_BE:
			i++;
			continue;
			goto IL_BE;
		}
	}

	public void GenerateUnit(global::WarbandController warCtrlr, int warbandPos, global::UnitSave unit, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, bool merge = true)
	{
		global::UnitFactory.UnitCreationData unitCreationData = new global::UnitFactory.UnitCreationData();
		unitCreationData.warCtrlr = warCtrlr;
		unitCreationData.warbandPos = warbandPos;
		unitCreationData.unitSave = unit;
		unitCreationData.position = position;
		unitCreationData.rotation = rotation;
		unitCreationData.merge = merge;
		unitCreationData.guid = global::PandoraSingleton<global::Hermes>.Instance.GetNextGUID();
		this.creationQueue.Add(unitCreationData);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"[UnitFactory] Adding Unit : ",
			unit.stats.Name,
			" to queue. Position (",
			position,
			")"
		}), "LOADING", null);
	}

	private void CreateUnit(global::UnitFactory.UnitCreationData creationData)
	{
		global::PandoraDebug.LogInfo("[UnitFactory] Creating unit : " + (global::UnitId)creationData.unitSave.stats.id, "LOADING", null);
		global::UnitData unitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(creationData.unitSave.stats.id);
		string prefab = "prefabs/characters/" + unitData.UnitBaseId.ToString().ToLower();
		creationData.prefab = unitData.UnitBaseId.ToString().ToLower();
		creationData.state = global::UnitFactory.LoadState.PREFABING;
		global::UnityEngine.GameObject x = this.FindPrefab(creationData.prefab);
		if (x == null)
		{
			base.StartCoroutine(this.LoadPrefab(prefab));
		}
	}

	private global::System.Collections.IEnumerator LoadPrefab(string prefab)
	{
		global::PandoraDebug.LogInfo("[UnitFactory] Creating unit name = " + prefab, "LOADING", null);
		global::UnityEngine.ResourceRequest req = global::UnityEngine.Resources.LoadAsync(prefab);
		yield return req;
		this.prefabs.Add((global::UnityEngine.GameObject)req.asset);
		yield break;
	}

	private global::UnityEngine.GameObject FindPrefab(string prefab)
	{
		for (int i = 0; i < this.prefabs.Count; i++)
		{
			if (this.prefabs[i].name == prefab)
			{
				return this.prefabs[i];
			}
		}
		return null;
	}

	public void InitUnit(global::UnitFactory.UnitCreationData creationData, global::UnityEngine.GameObject prefab)
	{
		creationData.state = global::UnitFactory.LoadState.INITIALIZING;
		global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(prefab, creationData.position, creationData.rotation);
		global::UnitController component = gameObject.GetComponent<global::UnitController>();
		creationData.warCtrlr.unitCtrlrs[creationData.warbandPos] = component;
		creationData.ctrlr = component;
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"[UnitFactory] Unit BEFORE INIT",
			this.unitCreatedCount,
			" created at position ",
			component.transform.position
		}), "LOADING", null);
		component.FirstSyncInit(creationData.unitSave, creationData.guid, creationData.warCtrlr.idx, creationData.warCtrlr.playerIdx, creationData.warCtrlr.playerTypeId, creationData.warbandPos, creationData.merge, true);
		component.InitMissionUnit(creationData.unitSave, creationData.guid, creationData.warCtrlr.idx, creationData.warCtrlr.playerIdx, creationData.warCtrlr.playerTypeId, creationData.warbandPos, creationData.merge);
	}

	public void UnitCreationDone(global::UnitFactory.UnitCreationData creationData)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"[UnitFactory] Unit ",
			this.unitCreatedCount,
			" created at position ",
			creationData.warbandPos
		}), "LOADING", null);
		this.unitCreatedCount++;
		creationData.state = global::UnitFactory.LoadState.DONE;
		creationData.warCtrlr.OnUnitCreated(creationData.ctrlr);
	}

	public global::System.Collections.IEnumerator CloneUnitCtrlr(global::UnitController srcCtrlr, int rank, int rating, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		global::UnityEngine.GameObject clone = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(srcCtrlr.gameObject, pos, rot);
		for (int i = 0; i < clone.transform.childCount; i++)
		{
			global::UnityEngine.Transform childTr = clone.transform.GetChild(i);
			if (childTr.name.ToLowerString() == global::BoneId.RIG_WEAPONR.ToLowerString() || childTr.name.ToLowerString() == global::BoneId.RIG_WEAPONL.ToLowerString())
			{
				for (int j = childTr.childCount - 1; j >= 0; j--)
				{
					global::UnityEngine.Object.Destroy(childTr.GetChild(j).gameObject);
				}
			}
		}
		global::Projectile[] projs = clone.GetComponentsInChildren<global::Projectile>();
		for (int k = 0; k < projs.Length; k++)
		{
			global::UnityEngine.Object.Destroy(projs[k].gameObject);
		}
		global::UnitController clonedCtrlr = clone.GetComponent<global::UnitController>();
		global::System.Collections.Generic.List<global::AttributeData> attributesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>();
		global::Unit clonedUnit = global::Unit.GenerateUnit(srcCtrlr.unit.Id, rank);
		int ratingPool = clonedUnit.GetRating();
		global::CombatStyleId set1StyleId = global::UnitFactory.AddCombatStyleSet(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, ref ratingPool, clonedUnit, global::UnitSlotId.SET1_MAINHAND, global::CombatStyleId.NONE, global::ItemQualityId.NORMAL, false, null);
		global::UnitFactory.AddCombatStyleSet(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, ref ratingPool, clonedUnit, global::UnitSlotId.SET2_MAINHAND, set1StyleId, global::ItemQualityId.NORMAL, false, null);
		global::UnitFactory.RaiseAttributes(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, attributesData, clonedUnit, ref ratingPool, rating);
		global::UnitFactory.AddSkillSpells(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, clonedUnit, ref ratingPool, rating);
		int counter = 9999;
		global::UnitFactory.BoostItemsQuality(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, clonedUnit, global::ItemQualityId.GOOD, ref ratingPool, ref counter, rating);
		global::UnitFactory.BoostItemsQuality(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, clonedUnit, global::ItemQualityId.BEST, ref ratingPool, ref counter, rating);
		uint nextGuid = global::PandoraSingleton<global::Hermes>.Instance.GetNextGUID();
		clonedCtrlr.FirstSyncInit(clonedUnit.UnitSave, nextGuid, srcCtrlr.unit.warbandIdx, srcCtrlr.GetWarband().playerIdx, srcCtrlr.GetWarband().playerTypeId, srcCtrlr.GetWarband().unitCtrlrs.Count, false, false);
		clonedCtrlr.InitMissionUnit(clonedUnit.UnitSave, nextGuid, srcCtrlr.unit.warbandIdx, srcCtrlr.GetWarband().playerIdx, srcCtrlr.GetWarband().playerTypeId, srcCtrlr.GetWarband().unitCtrlrs.Count, false);
		yield return null;
		this.OverrideItem(global::UnitSlotId.HELMET, srcCtrlr, clonedCtrlr);
		this.OverrideItem(global::UnitSlotId.ARMOR, srcCtrlr, clonedCtrlr);
		global::System.Collections.Generic.List<global::Item> returnedItems = new global::System.Collections.Generic.List<global::Item>();
		clonedCtrlr.unit.ClearMutations();
		for (int l = 0; l < srcCtrlr.unit.Mutations.Count; l++)
		{
			clonedCtrlr.unit.AddMutation(srcCtrlr.unit.Mutations[l].Data.Id, returnedItems);
		}
		clonedCtrlr.unit.ClearInjuries();
		for (int m = 0; m < srcCtrlr.unit.Injuries.Count; m++)
		{
			clonedCtrlr.unit.AddInjury(srcCtrlr.unit.Injuries[m].Data, 0, returnedItems, false, -1);
		}
		clonedCtrlr.AICtrlr.SetAIProfile(global::AiProfileId.BASE_SCOUT);
		if (returnedItems.Count > 0)
		{
			clonedCtrlr.RefreshEquipments(null);
			clonedCtrlr.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		}
		clonedCtrlr.InitBodyTrails();
		clonedUnit.UnitSave.isReinforcement = true;
		global::PandoraSingleton<global::MissionManager>.Instance.IncludeUnit(clonedCtrlr, srcCtrlr.GetWarband(), pos, rot);
		global::UnityEngine.Debug.Log("Spawned Unit rating = " + clonedCtrlr.unit.GetRating());
		yield break;
	}

	private void OverrideItem(global::UnitSlotId slotId, global::UnitController srcCtrlr, global::UnitController clonedCtlr)
	{
		global::ItemSave itemSave = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		global::Thoth.Copy(srcCtrlr.unit.Items[(int)slotId].Save, itemSave);
		clonedCtlr.unit.Items[(int)slotId] = new global::Item(itemSave);
	}

	public static global::CombatStyleId AddCombatStyleSet(global::Tyche tyche, ref int ratingPool, global::Unit unit, global::UnitSlotId slotId, global::CombatStyleId excludedCombatStyleId = global::CombatStyleId.NONE, global::ItemQualityId qualityId = global::ItemQualityId.NORMAL, bool generateRuneMark = false, global::System.Collections.Generic.List<global::Item> newItems = null)
	{
		global::PandoraDebug.LogInfo("Add combat style set for " + unit.Data.Id, "uncategorised", null);
		global::System.Collections.Generic.List<global::UnitJoinCombatStyleData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinCombatStyleData>("fk_unit_id", ((int)unit.Data.Id).ToConstantString());
		global::System.Collections.Generic.List<global::UnitJoinCombatStyleData> list2 = new global::System.Collections.Generic.List<global::UnitJoinCombatStyleData>();
		bool flag = unit.GetMutationId(global::UnitSlotId.SET1_MAINHAND) != global::MutationId.NONE;
		bool flag2 = unit.GetMutationId(global::UnitSlotId.SET1_OFFHAND) != global::MutationId.NONE || unit.GetInjury(global::UnitSlotId.SET1_OFFHAND) != global::InjuryId.NONE;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].CombatStyleId != excludedCombatStyleId)
			{
				global::CombatStyleData combatStyleData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CombatStyleData>((int)list[i].CombatStyleId);
				if (!flag || combatStyleData.ItemTypeIdMain == global::ItemTypeId.MELEE_1H)
				{
					if (flag2)
					{
						if (combatStyleData.ItemTypeIdOff != global::ItemTypeId.MELEE_1H)
						{
							goto IL_11B;
						}
						if (combatStyleData.ItemTypeIdMain != global::ItemTypeId.MELEE_1H)
						{
							goto IL_11B;
						}
					}
					if (excludedCombatStyleId < global::CombatStyleId.RANGE || list[i].CombatStyleId < global::CombatStyleId.RANGE)
					{
						list2.Add(list[i]);
					}
				}
			}
			IL_11B:;
		}
		if (list2.Count == 0)
		{
			return global::CombatStyleId.NONE;
		}
		global::UnitJoinCombatStyleData randomRatio = global::UnitJoinCombatStyleData.GetRandomRatio(list2, tyche, null);
		if (excludedCombatStyleId != global::CombatStyleId.NONE && excludedCombatStyleId < global::CombatStyleId.RANGE && randomRatio.CombatStyleId < global::CombatStyleId.RANGE)
		{
			return global::CombatStyleId.NONE;
		}
		global::CombatStyleData combatStyleData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CombatStyleData>((int)randomRatio.CombatStyleId);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Using combat style ",
			combatStyleData2.Id,
			" for unit ",
			unit.Data.Id
		}), "uncategorised", null);
		global::Item procItem = global::UnitFactory.GetProcItem(tyche, ref ratingPool, unit, combatStyleData2.UnitSlotIdMain, combatStyleData2.ItemTypeIdMain, qualityId, generateRuneMark, flag);
		unit.EquipItem(slotId, procItem, true);
		if (newItems != null)
		{
			newItems.Add(procItem);
		}
		if (combatStyleData2.ItemTypeIdOff != global::ItemTypeId.NONE)
		{
			procItem = global::UnitFactory.GetProcItem(tyche, ref ratingPool, unit, combatStyleData2.UnitSlotIdOff, combatStyleData2.ItemTypeIdOff, qualityId, generateRuneMark, flag2);
			unit.EquipItem(slotId + 1, procItem, true);
			if (newItems != null)
			{
				newItems.Add(procItem);
			}
		}
		return combatStyleData2.Id;
	}

	public static global::ArmorStyleId AddArmorStyleSet(global::Tyche tyche, ref int ratingPool, global::Unit unit, global::ItemQualityId qualityId = global::ItemQualityId.NORMAL, bool generateArmor = true, bool generateHelmet = true, bool generateArmorRuneMark = false, bool generateHelmetRuneMark = false, global::System.Collections.Generic.List<global::Item> newItems = null)
	{
		global::System.Collections.Generic.List<global::UnitJoinArmorStyleData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinArmorStyleData>("fk_unit_id", ((int)unit.Id).ToConstantString());
		global::System.Collections.Generic.List<global::UnitJoinArmorStyleData> list2 = new global::System.Collections.Generic.List<global::UnitJoinArmorStyleData>();
		for (int i = 0; i < list.Count; i++)
		{
			global::ArmorStyleData armorStyleData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ArmorStyleData>((int)list[i].ArmorStyleId);
			if (unit.GetMutationId(global::UnitSlotId.ARMOR) == global::MutationId.NONE || armorStyleData.ItemTypeIdArmor == global::ItemTypeId.CLOTH_ARMOR)
			{
				list2.Add(list[i]);
			}
		}
		global::UnitJoinArmorStyleData randomRatio = global::UnitJoinArmorStyleData.GetRandomRatio(list2, tyche, null);
		global::ArmorStyleData armorStyleData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ArmorStyleData>((int)randomRatio.ArmorStyleId);
		if (generateArmor)
		{
			global::Item procItem = global::UnitFactory.GetProcItem(tyche, ref ratingPool, unit, global::UnitSlotId.ARMOR, armorStyleData2.ItemTypeIdArmor, qualityId, generateArmorRuneMark, unit.GetMutationId(global::UnitSlotId.ARMOR) != global::MutationId.NONE);
			unit.EquipItem(global::UnitSlotId.ARMOR, procItem, true);
			if (newItems != null)
			{
				newItems.Add(procItem);
			}
		}
		if (generateHelmet && armorStyleData2.ItemTypeIdHelmet != global::ItemTypeId.NONE)
		{
			global::Item procItem2 = global::UnitFactory.GetProcItem(tyche, ref ratingPool, unit, global::UnitSlotId.HELMET, armorStyleData2.ItemTypeIdHelmet, qualityId, generateHelmetRuneMark, unit.GetMutationId(global::UnitSlotId.HELMET) != global::MutationId.NONE);
			unit.EquipItem(global::UnitSlotId.HELMET, procItem2, true);
			if (newItems != null)
			{
				newItems.Add(procItem2);
			}
		}
		return armorStyleData2.Id;
	}

	public static global::Item GetProcItem(global::Tyche tyche, ref int ratingPool, global::Unit unit, global::UnitSlotId unitSlotId, global::ItemTypeId itemTypeId, global::ItemQualityId qualityId = global::ItemQualityId.NORMAL, bool generateRunemark = false, bool hasMutation = false)
	{
		string text = ((int)unit.Id).ToConstantString();
		global::System.Collections.Generic.List<global::ItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>("fk_item_type_id", ((int)itemTypeId).ToConstantString()).ToDynList<global::ItemData>();
		global::System.Collections.Generic.List<global::ItemUnitData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemUnitData>(new string[]
		{
			"fk_unit_id",
			"mutation"
		}, new string[]
		{
			text,
			(!hasMutation) ? "0" : "1"
		});
		for (int i = list.Count - 1; i >= 0; i--)
		{
			bool flag = false;
			global::System.Collections.Generic.List<global::ItemJoinUnitSlotData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemJoinUnitSlotData>("fk_item_id", list[i].Id.ToIntString<global::ItemId>());
			if (list3.Exists((global::ItemJoinUnitSlotData x) => x.UnitSlotId == unitSlotId))
			{
				for (int j = 0; j < list2.Count; j++)
				{
					if (list[i].Id == list2[j].ItemId)
					{
						flag = true;
						break;
					}
				}
			}
			if (!(flag & (list[i].Id != global::ItemId.MAIDEN_FLAIL && list[i].Id != global::ItemId.MAIDEN_TWO_HANDED_FLAIL)))
			{
				list.RemoveAt(i);
			}
		}
		if (list.Count == 0)
		{
			global::PandoraDebug.LogWarning(string.Concat(new object[]
			{
				"No items of type ",
				itemTypeId,
				" are availables for unit ",
				unit.Id,
				" in slot ",
				unitSlotId,
				" please check combat/armor style values for this unit"
			}), "uncategorised", null);
			return new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		}
		global::ItemData itemData = list[tyche.Rand(0, list.Count)];
		ratingPool += itemData.Rating;
		global::Item item = new global::Item(new global::ItemSave(itemData.Id, qualityId, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1));
		if (qualityId > global::ItemQualityId.NORMAL)
		{
			global::ItemQualityJoinItemTypeData itemQualityJoinItemTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityJoinItemTypeData>(new string[]
			{
				"fk_item_quality_id",
				"fk_item_type_id"
			}, new string[]
			{
				((int)qualityId).ToConstantString(),
				((int)itemData.ItemTypeId).ToConstantString()
			})[0];
			ratingPool += itemQualityJoinItemTypeData.RatingModifier;
		}
		if (generateRunemark)
		{
			ratingPool = global::UnitFactory.TryAddRune(tyche, ref ratingPool, unit, item);
		}
		return item;
	}

	private static int TryAddRune(global::Tyche tyche, ref int ratingPool, global::Unit unit, global::Item item)
	{
		if (item.CanAddRuneMark())
		{
			global::RuneMarkId randomRuneMark = global::Item.GetRandomRuneMark(tyche, item, unit.AllegianceId);
			if (randomRuneMark != global::RuneMarkId.NONE)
			{
				bool flag = item.AddRuneMark(randomRuneMark, item.QualityData.RuneMarkQualityIdMax, unit.AllegianceId);
				if (flag)
				{
					global::RuneMarkQualityJoinItemTypeData runeMarkQualityJoinItemTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkQualityJoinItemTypeData>(new string[]
					{
						"fk_rune_mark_quality_id",
						"fk_item_type_id"
					}, new string[]
					{
						((int)item.RuneMark.QualityData.Id).ToConstantString(),
						((int)item.TypeData.Id).ToConstantString()
					})[0];
					ratingPool += runeMarkQualityJoinItemTypeData.Rating;
				}
			}
		}
		return ratingPool;
	}

	public static void BoostItemsQuality(global::Tyche tyche, global::Unit unit, global::ItemQualityId newQualityId, ref int ratingPool, ref int counter, int rating)
	{
		global::UnitFactory.BoostItemQuality(tyche, unit, newQualityId, global::UnitSlotId.ARMOR, ref ratingPool, ref counter, rating);
		global::UnitFactory.BoostItemQuality(tyche, unit, newQualityId, global::UnitSlotId.HELMET, ref ratingPool, ref counter, rating);
		global::UnitFactory.BoostItemQuality(tyche, unit, newQualityId, global::UnitSlotId.SET1_MAINHAND, ref ratingPool, ref counter, rating);
		global::UnitFactory.BoostItemQuality(tyche, unit, newQualityId, global::UnitSlotId.SET1_OFFHAND, ref ratingPool, ref counter, rating);
		global::UnitFactory.BoostItemQuality(tyche, unit, newQualityId, global::UnitSlotId.SET2_MAINHAND, ref ratingPool, ref counter, rating);
		global::UnitFactory.BoostItemQuality(tyche, unit, newQualityId, global::UnitSlotId.SET2_OFFHAND, ref ratingPool, ref counter, rating);
	}

	public static void BoostItemQuality(global::Tyche tyche, global::Unit unit, global::ItemQualityId newQualityId, global::UnitSlotId unitSlotId, ref int ratingPool, ref int counter, int rating)
	{
		if (counter <= 0 || ratingPool > rating)
		{
			return;
		}
		global::ItemSave itemSave = unit.UnitSave.items[(int)unitSlotId];
		if (itemSave == null || itemSave.id == 0)
		{
			return;
		}
		global::ItemTypeId itemTypeId = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(itemSave.id).ItemTypeId;
		if (itemSave.qualityId < (int)newQualityId)
		{
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"Boosting quality of item ",
				(global::ItemId)itemSave.id,
				" to ",
				newQualityId
			}), "uncategorised", null);
			int ratingModifier = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityJoinItemTypeData>(new string[]
			{
				"fk_item_quality_id",
				"fk_item_type_id"
			}, new string[]
			{
				((int)newQualityId).ToConstantString(),
				((int)itemTypeId).ToConstantString()
			})[0].RatingModifier;
			if (ratingPool + ratingModifier <= rating)
			{
				itemSave.qualityId = (int)newQualityId;
				global::PandoraDebug.LogInfo(string.Concat(new object[]
				{
					"Quality of item ",
					(global::ItemId)itemSave.id,
					" boosted to ",
					newQualityId
				}), "uncategorised", null);
				ratingPool += ratingModifier;
				counter--;
				itemSave.runeMarkId = 0;
				itemSave.runeMarkQualityId = 0;
				global::Item item = new global::Item(itemSave);
				unit.Items[(int)unitSlotId] = item;
				global::UnitFactory.TryAddRune(tyche, ref ratingPool, unit, item);
			}
		}
	}

	public static void RaiseAttributes(global::Tyche tyche, global::System.Collections.Generic.List<global::AttributeData> attributesData, global::Unit unit, ref int ratingPool, int ratingMax)
	{
		int[] baseAttributes = new int[]
		{
			unit.GetBaseAttribute(global::AttributeId.STRENGTH),
			unit.GetBaseAttribute(global::AttributeId.TOUGHNESS),
			unit.GetBaseAttribute(global::AttributeId.AGILITY),
			unit.GetBaseAttribute(global::AttributeId.LEADERSHIP),
			unit.GetBaseAttribute(global::AttributeId.INTELLIGENCE),
			unit.GetBaseAttribute(global::AttributeId.ALERTNESS),
			unit.GetBaseAttribute(global::AttributeId.WEAPON_SKILL),
			unit.GetBaseAttribute(global::AttributeId.BALLISTIC_SKILL),
			unit.GetBaseAttribute(global::AttributeId.ACCURACY)
		};
		int[] maxAttributes = new int[]
		{
			unit.GetBaseAttribute(global::AttributeId.STRENGTH_MAX),
			unit.GetBaseAttribute(global::AttributeId.TOUGHNESS_MAX),
			unit.GetBaseAttribute(global::AttributeId.AGILITY_MAX),
			unit.GetBaseAttribute(global::AttributeId.LEADERSHIP_MAX),
			unit.GetBaseAttribute(global::AttributeId.INTELLIGENCE_MAX),
			unit.GetBaseAttribute(global::AttributeId.ALERTNESS_MAX),
			unit.GetBaseAttribute(global::AttributeId.WEAPON_SKILL_MAX),
			unit.GetBaseAttribute(global::AttributeId.BALLISTIC_SKILL_MAX),
			unit.GetBaseAttribute(global::AttributeId.ACCURACY_MAX)
		};
		global::UnitFactory.RaiseAttributes(tyche, attributesData, unit, ref ratingPool, ratingMax, baseAttributes, maxAttributes);
	}

	public static void RaiseAttributes(global::Tyche tyche, global::System.Collections.Generic.List<global::AttributeData> attributesData, global::Unit unit, ref int ratingPool, int ratingMax, int[] baseAttributes, int[] maxAttributes)
	{
		global::UnitFactory.RaiseAttributeType(tyche, attributesData, global::AttributeTypeId.PHYSICAL, unit, ref ratingPool, ratingMax, baseAttributes, maxAttributes);
		global::UnitFactory.RaiseAttributeType(tyche, attributesData, global::AttributeTypeId.MARTIAL, unit, ref ratingPool, ratingMax, baseAttributes, maxAttributes);
		global::UnitFactory.RaiseAttributeType(tyche, attributesData, global::AttributeTypeId.MENTAL, unit, ref ratingPool, ratingMax, baseAttributes, maxAttributes);
		unit.ApplyChanges(true);
	}

	private static void RaiseAttributeType(global::Tyche tyche, global::System.Collections.Generic.List<global::AttributeData> attributesData, global::AttributeTypeId attrTypeId, global::Unit unit, ref int ratingPool, int ratingMax, int[] baseAttributes, int[] maxAttributes)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Unit has ",
			unit.UnspentPhysical,
			" unspent physicals ",
			unit.UnspentMartial,
			" unspent martials ",
			unit.UnspentMental,
			" unspent mentals"
		}), "MISSION", null);
		int num = 0;
		global::System.Collections.Generic.List<global::AttributeData> list = new global::System.Collections.Generic.List<global::AttributeData>();
		for (int i = 0; i < attributesData.Count; i++)
		{
			if (attributesData[i].AttributeTypeId == attrTypeId)
			{
				list.Add(attributesData[i]);
			}
		}
		int num2 = 0;
		switch (attrTypeId)
		{
		case global::AttributeTypeId.PHYSICAL:
			num2 = unit.UnspentPhysical;
			break;
		case global::AttributeTypeId.MENTAL:
			num2 = unit.UnspentMental;
			break;
		case global::AttributeTypeId.MARTIAL:
			num2 = unit.UnspentMartial;
			break;
		}
		bool flag = true;
		while (flag && ratingPool < ratingMax)
		{
			flag = false;
			int num3 = tyche.Rand(0, list.Count);
			int num4 = 0;
			while (!flag && num4 < list.Count)
			{
				global::AttributeData attributeData = list[num3];
				if (unit.CanRaiseAttributeFast(attributeData.Id, baseAttributes, maxAttributes, num2))
				{
					unit.RaiseAttribute(attributeData.Id, false);
					ratingPool += attributeData.Rating;
					flag = true;
					num++;
					num2--;
				}
				num3 = (num3 + 1) % list.Count;
				num4++;
			}
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Added ",
			attrTypeId,
			" ",
			num,
			" added!"
		}), "MISSION", null);
	}

	public static void AddSkillSpells(global::Tyche tyche, global::Unit unit, ref int ratingPool, int ratingMax)
	{
		int[] baseAttributes = new int[]
		{
			unit.GetBaseAttribute(global::AttributeId.STRENGTH),
			unit.GetBaseAttribute(global::AttributeId.TOUGHNESS),
			unit.GetBaseAttribute(global::AttributeId.AGILITY),
			unit.GetBaseAttribute(global::AttributeId.LEADERSHIP),
			unit.GetBaseAttribute(global::AttributeId.INTELLIGENCE),
			unit.GetBaseAttribute(global::AttributeId.ALERTNESS),
			unit.GetBaseAttribute(global::AttributeId.WEAPON_SKILL),
			unit.GetBaseAttribute(global::AttributeId.BALLISTIC_SKILL),
			unit.GetBaseAttribute(global::AttributeId.ACCURACY)
		};
		global::UnitFactory.AddSkillSpells(tyche, unit, ref ratingPool, ratingMax, global::UnitFactory.GetLearnableSkills(unit), baseAttributes);
	}

	public static global::System.Collections.Generic.List<global::SkillData> GetLearnableSkills(global::Unit unit)
	{
		global::System.Collections.Generic.List<global::UnitJoinSkillLineData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkillLineData>("fk_unit_id", ((int)unit.Id).ToConstantString());
		global::System.Collections.Generic.List<global::SkillData> list2 = new global::System.Collections.Generic.List<global::SkillData>();
		for (int i = 0; i < list.Count; i++)
		{
			global::System.Collections.Generic.List<global::SkillLineJoinSkillData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineJoinSkillData>("fk_skill_line_id", list[i].SkillLineId.ToIntString<global::SkillLineId>());
			for (int j = 0; j < list3.Count; j++)
			{
				list2.Add(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)list3[j].SkillId));
			}
		}
		return list2;
	}

	public static void AddSkillSpells(global::Tyche tyche, global::Unit unit, ref int ratingPool, int ratingMax, global::System.Collections.Generic.List<global::SkillData> skillsData, int[] baseAttributes)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Unit ",
			unit.Id,
			unit.Name,
			" has ",
			unit.UnspentSkill,
			" unspent skill points",
			unit.UnspentSpell,
			" unspent spell points"
		}), "MISSION", null);
		for (int i = skillsData.Count - 1; i >= 0; i--)
		{
			if (skillsData[i].AttributeIdStat != global::AttributeId.NONE && (skillsData[i].AiProof || !unit.CanLearnSkillFast(skillsData[i], baseAttributes)))
			{
				skillsData.RemoveAt(i);
			}
		}
		while ((unit.UnspentSkill > 0 || unit.UnspentSpell > 0) && skillsData.Count > 0)
		{
			int num = tyche.Rand(0, skillsData.Count);
			global::SkillData skillData = skillsData[num];
			global::SkillTypeId skillTypeId = skillData.SkillTypeId;
			int rating = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillQualityData>((int)skillData.SkillQualityId).Rating;
			if (ratingPool + rating <= ratingMax && ((skillTypeId == global::SkillTypeId.SKILL_ACTION && unit.UnspentSkill > 0) || (skillTypeId == global::SkillTypeId.SPELL_ACTION && unit.UnspentSpell > 0)))
			{
				ratingPool += rating;
				unit.StartLearningSkill(skillData, 0, true);
				unit.EndLearnSkill(false);
			}
			skillsData.RemoveAt(num);
		}
		unit.UpdateAttributes();
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Unit ",
			unit.Id,
			unit.Name,
			" has now ",
			unit.UnitSave.activeSkills.Count,
			" active skills ",
			unit.UnitSave.passiveSkills.Count,
			" passive skills ",
			unit.UnitSave.spells.Count,
			" spells",
			unit.UnspentSkill,
			" unspent skill points",
			unit.UnspentSpell,
			" unspent spell points"
		}), "MISSION", null);
	}

	private global::System.Collections.Generic.List<global::UnitFactory.UnitCreationData> creationQueue;

	private int unitCreatedCount;

	private int index;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> prefabs;

	public enum LoadState
	{
		NONE,
		PREFABING,
		INITIALIZING,
		DONE,
		COUNT
	}

	public class UnitCreationData
	{
		public global::WarbandController warCtrlr;

		public global::UnitController ctrlr;

		public uint guid;

		public int warbandPos;

		public global::UnitSave unitSave;

		public global::UnityEngine.Vector3 position;

		public global::UnityEngine.Quaternion rotation;

		public bool merge;

		public global::UnitFactory.LoadState state;

		public string prefab;
	}
}
