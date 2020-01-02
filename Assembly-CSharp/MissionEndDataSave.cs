using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MissionEndDataSave : global::IThoth
{
	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		global::Thoth.Read(reader, out num);
		if (num2 < 2)
		{
			int num3;
			global::Thoth.Read(reader, out num3);
		}
		int num4 = 0;
		global::Thoth.Read(reader, out num4);
		this.ratingId = (global::ProcMissionRatingId)num4;
		global::Thoth.Read(reader, out this.won);
		global::Thoth.Read(reader, out this.primaryObjectiveCompleted);
		global::Thoth.Read(reader, out this.crushed);
		global::Thoth.Read(reader, out this.isCampaign);
		global::Thoth.Read(reader, out this.isSkirmish);
		global::Thoth.Read(reader, out this.isVsAI);
		int num5 = 0;
		global::Thoth.Read(reader, out num5);
		this.units = new global::System.Collections.Generic.List<global::MissionEndUnitSave>(num5);
		for (int i = 0; i < num5; i++)
		{
			global::MissionEndUnitSave missionEndUnitSave = new global::MissionEndUnitSave();
			((global::IThoth)missionEndUnitSave).Read(reader);
			this.units.Add(missionEndUnitSave);
		}
		global::Thoth.Read(reader, out num5);
		for (int j = 0; j < num5; j++)
		{
			global::ItemSave itemSave = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
			((global::IThoth)itemSave).Read(reader);
			this.wagonItems.AddItem(itemSave, false);
		}
		global::Thoth.Read(reader, out this.playerMVUIdx);
		global::Thoth.Read(reader, out this.enemyMVUIdx);
		if (num2 == 0)
		{
			int num6;
			global::Thoth.Read(reader, out num6);
		}
		else
		{
			global::Thoth.Read(reader, out this.routable);
		}
		((global::IThoth)this.missionSave).Read(reader);
		if (num2 > 2)
		{
			global::Thoth.Read(reader, out this.seed);
			num5 = 0;
			global::Thoth.Read(reader, out num5);
			for (int k = 0; k < num5; k++)
			{
				global::MissionWarbandSave missionWarbandSave = new global::MissionWarbandSave();
				((global::IThoth)missionWarbandSave).Read(reader);
				this.missionWarbands.Add(missionWarbandSave);
			}
			num5 = 0;
			global::Thoth.Read(reader, out num5);
			for (int l = 0; l < num5; l++)
			{
				uint item = 0U;
				global::Thoth.Read(reader, out item);
				this.destroyedTraps.Add(item);
			}
			num5 = 0;
			global::Thoth.Read(reader, out num5);
			for (int m = 0; m < num5; m++)
			{
				uint key = 0U;
				global::Thoth.Read(reader, out key);
				uint unitCtrlrUid = 0U;
				global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
				if (num2 > 3)
				{
					global::Thoth.Read(reader, out unitCtrlrUid);
					global::Thoth.Read(reader, out zero.x);
					global::Thoth.Read(reader, out zero.y);
					global::Thoth.Read(reader, out zero.z);
				}
				int num7 = 0;
				global::Thoth.Read(reader, out num7);
				global::System.Collections.Generic.List<global::ItemSave> list = new global::System.Collections.Generic.List<global::ItemSave>(num7);
				for (int n = 0; n < num7; n++)
				{
					global::ItemSave itemSave2 = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
					((global::IThoth)itemSave2).Read(reader);
					list.Add(itemSave2);
				}
				bool wasSearched = false;
				if (this.lastVersion > 11)
				{
					global::Thoth.Read(reader, out wasSearched);
				}
				global::SearchSave searchSave = new global::SearchSave();
				searchSave.unitCtrlrUid = unitCtrlrUid;
				searchSave.pos = zero;
				searchSave.items = ((list.Count <= 0) ? null : list);
				searchSave.wasSearched = wasSearched;
				this.searches.Add(new global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>(key, searchSave));
			}
			num5 = 0;
			global::Thoth.Read(reader, out num5);
			for (int num8 = 0; num8 < num5; num8++)
			{
				global::EndZoneAoe item2 = default(global::EndZoneAoe);
				global::Thoth.Read(reader, out item2.guid);
				global::Thoth.Read(reader, out item2.myrtilusId);
				int aoeId = 0;
				global::Thoth.Read(reader, out aoeId);
				item2.aoeId = (global::ZoneAoeId)aoeId;
				global::Thoth.Read(reader, out item2.radius);
				global::Thoth.Read(reader, out item2.durationLeft);
				global::Thoth.Read(reader, out item2.position.x);
				global::Thoth.Read(reader, out item2.position.y);
				global::Thoth.Read(reader, out item2.position.z);
				this.aoeZones.Add(item2);
			}
			num5 = 0;
			global::Thoth.Read(reader, out num5);
			this.myrtilusLadder = new global::System.Collections.Generic.List<uint>();
			for (int num9 = 0; num9 < num5; num9++)
			{
				uint item3;
				global::Thoth.Read(reader, out item3);
				this.myrtilusLadder.Add(item3);
			}
			global::Thoth.Read(reader, out this.currentLadderIdx);
			global::Thoth.Read(reader, out this.currentTurn);
			num5 = 0;
			global::Thoth.Read(reader, out num5);
			this.warbandMorals = new global::System.Collections.Generic.List<global::Tuple<int, int, bool>>();
			for (int num10 = 0; num10 < num5; num10++)
			{
				int item4 = 0;
				int item5 = 0;
				bool item6 = false;
				global::Thoth.Read(reader, out item4);
				global::Thoth.Read(reader, out item5);
				global::Thoth.Read(reader, out item6);
				global::Tuple<int, int, bool> item7 = new global::Tuple<int, int, bool>(item4, item5, item6);
				this.warbandMorals.Add(item7);
			}
			global::Thoth.Read(reader, out this.missionFinished);
			global::Thoth.Read(reader, out num5);
			for (int num11 = 0; num11 < num5; num11++)
			{
				int key2 = 0;
				global::Thoth.Read(reader, out key2);
				int value = 0;
				global::Thoth.Read(reader, out value);
				global::System.Collections.Generic.KeyValuePair<int, int> item8 = new global::System.Collections.Generic.KeyValuePair<int, int>(key2, value);
				this.reinforcements.Add(item8);
			}
			global::Thoth.Read(reader, out num5);
			for (int num12 = 0; num12 < num5; num12++)
			{
				uint key3;
				global::Thoth.Read(reader, out key3);
				uint value2;
				global::Thoth.Read(reader, out value2);
				this.objectives.Add(new global::System.Collections.Generic.KeyValuePair<uint, uint>(key3, value2));
			}
			global::Thoth.Read(reader, out num5);
			for (int num13 = 0; num13 < num5; num13++)
			{
				uint key4;
				global::Thoth.Read(reader, out key4);
				int value3;
				global::Thoth.Read(reader, out value3);
				this.converters.Add(new global::System.Collections.Generic.KeyValuePair<uint, int>(key4, value3));
			}
			global::Thoth.Read(reader, out num5);
			for (int num14 = 0; num14 < num5; num14++)
			{
				uint key5;
				global::Thoth.Read(reader, out key5);
				bool value4;
				global::Thoth.Read(reader, out value4);
				this.activaters.Add(new global::System.Collections.Generic.KeyValuePair<uint, bool>(key5, value4));
			}
			if (num2 > 6)
			{
				global::Thoth.Read(reader, out num5);
				for (int num15 = 0; num15 < num5; num15++)
				{
					global::EndDynamicTrap item9 = default(global::EndDynamicTrap);
					global::Thoth.Read(reader, out item9.guid);
					global::Thoth.Read(reader, out item9.trapTypeId);
					global::Thoth.Read(reader, out item9.teamIdx);
					global::Thoth.Read(reader, out item9.pos.x);
					global::Thoth.Read(reader, out item9.pos.y);
					global::Thoth.Read(reader, out item9.pos.z);
					global::Thoth.Read(reader, out item9.rot.x);
					global::Thoth.Read(reader, out item9.rot.y);
					global::Thoth.Read(reader, out item9.rot.z);
					global::Thoth.Read(reader, out item9.rot.w);
					this.dynamicTraps.Add(item9);
				}
			}
			if (num2 > 7)
			{
				global::Thoth.Read(reader, out num5);
				for (int num16 = 0; num16 < num5; num16++)
				{
					global::EndDestructible item10 = default(global::EndDestructible);
					global::Thoth.Read(reader, out item10.guid);
					int destructibleId = 0;
					global::Thoth.Read(reader, out destructibleId);
					item10.destructibleId = (global::DestructibleId)destructibleId;
					global::Thoth.Read(reader, out item10.onwerGuid);
					global::Thoth.Read(reader, out item10.wounds);
					global::Thoth.Read(reader, out item10.position.x);
					global::Thoth.Read(reader, out item10.position.y);
					global::Thoth.Read(reader, out item10.position.z);
					global::Thoth.Read(reader, out item10.rot.x);
					global::Thoth.Read(reader, out item10.rot.y);
					global::Thoth.Read(reader, out item10.rot.z);
					global::Thoth.Read(reader, out item10.rot.w);
					this.destructibles.Add(item10);
				}
			}
		}
	}

	public int GetVersion()
	{
		return 12;
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	public global::VictoryTypeId VictoryType
	{
		get
		{
			global::VictoryTypeId result = global::VictoryTypeId.LOSS;
			if (this.isCampaign)
			{
				if (this.primaryObjectiveCompleted)
				{
					result = global::VictoryTypeId.CAMPAIGN;
				}
			}
			else if (this.won && !this.primaryObjectiveCompleted)
			{
				result = global::VictoryTypeId.BATTLEGROUND;
			}
			else if (!this.won && this.primaryObjectiveCompleted)
			{
				result = global::VictoryTypeId.OBJECTIVE;
			}
			else if (this.won && this.primaryObjectiveCompleted)
			{
				result = global::VictoryTypeId.DECISIVE;
			}
			return result;
		}
	}

	public void AddUnits(int count)
	{
		this.units = new global::System.Collections.Generic.List<global::MissionEndUnitSave>();
	}

	public void UpdateMoral(int idx, int moral, int oldMoral, bool idolMoral)
	{
		global::Tuple<int, int, bool> value = new global::Tuple<int, int, bool>(moral, oldMoral, idolMoral);
		this.warbandMorals[idx] = value;
	}

	public void UpdateUnit(global::UnitController unit)
	{
		if (this.units == null || unit == null)
		{
			return;
		}
		for (int i = 0; i < this.units.Count; i++)
		{
			if (this.units[i].myrtilusId == unit.uid)
			{
				this.units[i].UpdateUnit(unit);
				return;
			}
		}
		global::MissionEndUnitSave missionEndUnitSave = new global::MissionEndUnitSave();
		missionEndUnitSave.UpdateUnit(unit);
		this.units.Add(missionEndUnitSave);
		if (unit.unit.UnitSave.isReinforcement)
		{
			global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[unit.unit.warbandIdx];
			global::System.Collections.Generic.KeyValuePair<int, int> item = new global::System.Collections.Generic.KeyValuePair<int, int>(this.units.Count - 1, warbandController.saveIdx);
			this.reinforcements.Add(item);
		}
	}

	public void UpdateSearches(uint guid, uint unitControllerUid, global::UnityEngine.Vector3 pos, global::System.Collections.Generic.List<global::ItemSave> items, bool wasSearched)
	{
		global::SearchSave searchSave;
		for (int i = 0; i < this.searches.Count; i++)
		{
			if (this.searches[i].Key == guid)
			{
				searchSave = this.searches[i].Value;
				searchSave.unitCtrlrUid = unitControllerUid;
				searchSave.pos = pos;
				searchSave.items = items;
				searchSave.wasSearched = wasSearched;
				this.searches[i] = new global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>(guid, searchSave);
				return;
			}
		}
		searchSave = new global::SearchSave();
		searchSave.unitCtrlrUid = unitControllerUid;
		searchSave.pos = pos;
		searchSave.items = items;
		searchSave.wasSearched = wasSearched;
		this.searches.Add(new global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>(guid, searchSave));
	}

	public void UpdateAoe(uint guid, uint myrtilusId, global::ZoneAoeId aoeId, float radius, int durationLeft, global::UnityEngine.Vector3 position)
	{
		global::EndZoneAoe endZoneAoe = default(global::EndZoneAoe);
		endZoneAoe.myrtilusId = myrtilusId;
		endZoneAoe.guid = guid;
		endZoneAoe.aoeId = aoeId;
		endZoneAoe.radius = radius;
		endZoneAoe.durationLeft = durationLeft;
		endZoneAoe.position = position;
		for (int i = 0; i < this.aoeZones.Count; i++)
		{
			if (this.aoeZones[i].guid == guid)
			{
				this.aoeZones[i] = endZoneAoe;
				return;
			}
		}
		this.aoeZones.Add(endZoneAoe);
	}

	public void ClearAoe(uint guid)
	{
		for (int i = 0; i < this.aoeZones.Count; i++)
		{
			if (this.aoeZones[i].guid == guid)
			{
				this.aoeZones.RemoveAt(i);
				break;
			}
		}
	}

	public void UpdateOpenedSearches(int warbandSaveIdx, uint guid)
	{
		if (this.missionWarbands[warbandSaveIdx].openedSearches.IndexOf(guid) == -1)
		{
			this.missionWarbands[warbandSaveIdx].openedSearches.Add(guid);
		}
	}

	public void UpdateObjective(uint uidObj, uint trackedUid = 0U)
	{
		this.objectives.Add(new global::System.Collections.Generic.KeyValuePair<uint, uint>(uidObj, trackedUid));
	}

	public void UpdateConverters(uint guid, int capacity)
	{
		global::System.Collections.Generic.KeyValuePair<uint, int> keyValuePair = new global::System.Collections.Generic.KeyValuePair<uint, int>(guid, capacity);
		for (int i = 0; i < this.converters.Count; i++)
		{
			if (this.converters[i].Key == guid)
			{
				this.converters[i] = keyValuePair;
				return;
			}
		}
		this.converters.Add(keyValuePair);
	}

	public void UpdateActivated(uint guid, bool status)
	{
		global::System.Collections.Generic.KeyValuePair<uint, bool> keyValuePair = new global::System.Collections.Generic.KeyValuePair<uint, bool>(guid, status);
		for (int i = 0; i < this.activaters.Count; i++)
		{
			if (this.activaters[i].Key == guid)
			{
				this.activaters[i] = keyValuePair;
				return;
			}
		}
		this.activaters.Add(keyValuePair);
	}

	public void AddDynamicTrap(global::Trap trap)
	{
		global::EndDynamicTrap item = default(global::EndDynamicTrap);
		item.guid = trap.guid;
		item.teamIdx = trap.TeamIdx;
		item.trapTypeId = (int)trap.defaultType;
		item.pos = trap.transform.position;
		item.rot = trap.transform.rotation;
		item.consumed = false;
		this.dynamicTraps.Add(item);
	}

	public void UpdateDynamicTrap(global::Trap trap)
	{
		for (int i = 0; i < this.dynamicTraps.Count; i++)
		{
			if (this.dynamicTraps[i].guid == trap.guid)
			{
				global::EndDynamicTrap value = this.dynamicTraps[i];
				value.consumed = true;
				this.dynamicTraps[i] = value;
			}
		}
	}

	public void AddDestructible(global::Destructible dest)
	{
		global::EndDestructible item = default(global::EndDestructible);
		item.guid = dest.guid;
		item.destructibleId = dest.id;
		item.onwerGuid = ((!(dest.Owner != null)) ? 0U : dest.Owner.uid);
		item.wounds = dest.CurrentWounds;
		item.position = dest.transform.position;
		item.rot = dest.transform.rotation;
		this.destructibles.Add(item);
	}

	public void UpdateDestructible(global::Destructible dest)
	{
		for (int i = 0; i < this.destructibles.Count; i++)
		{
			if (this.destructibles[i].guid == dest.guid)
			{
				global::EndDestructible value = this.destructibles[i];
				value.wounds = dest.CurrentWounds;
				this.destructibles[i] = value;
			}
		}
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		if (num > 1)
		{
			num2 = (int)(num2 + this.ratingId);
			num2 = (int)(num2 + this.ratingId);
			num2 += ((!this.won) ? 0 : 1);
			num2 += ((!this.primaryObjectiveCompleted) ? 0 : 1);
			num2 += ((!this.crushed) ? 0 : 1);
			num2 += ((!this.isCampaign) ? 0 : 1);
			num2 += ((!this.isSkirmish) ? 0 : 1);
			num2 += ((!this.isVsAI) ? 0 : 1);
			global::System.Collections.Generic.List<global::ItemSave> items = this.wagonItems.GetItems();
			for (int i = 0; i < items.Count; i++)
			{
				num2 += items[i].GetCRC(read);
			}
			num2 += this.playerMVUIdx;
			num2 += this.playerMVUIdx;
			num2 += this.enemyMVUIdx;
			num2 += this.missionSave.GetCRC(read);
		}
		if (num > 0)
		{
			num2 += ((!this.routable) ? 0 : 1);
		}
		if (num > 2)
		{
			num2 += this.seed;
			for (int j = 0; j < this.missionWarbands.Count; j++)
			{
				num2 += this.missionWarbands[j].GetCRC(read);
			}
			for (int k = 0; k < this.destroyedTraps.Count; k++)
			{
				num2 += (int)this.destroyedTraps[k];
			}
			for (int l = 0; l < this.searches.Count; l++)
			{
				num2 += (int)this.searches[l].Key;
				num2 += (int)this.searches[l].Value.unitCtrlrUid;
				num2 += (int)this.searches[l].Value.pos.x;
				num2 += (int)this.searches[l].Value.pos.y;
				num2 += (int)this.searches[l].Value.pos.z;
				if (this.searches[l].Value != null)
				{
					num2 += this.searches[l].Value.items.Count;
				}
			}
			for (int m = 0; m < this.aoeZones.Count; m++)
			{
				num2 += (int)this.aoeZones[m].guid;
				num2 += (int)this.aoeZones[m].myrtilusId;
				num2 = (int)(num2 + this.aoeZones[m].aoeId);
				num2 += (int)this.aoeZones[m].radius;
				num2 += this.aoeZones[m].durationLeft;
				num2 += (int)this.aoeZones[m].position.x;
				num2 += (int)this.aoeZones[m].position.y;
				num2 += (int)this.aoeZones[m].position.z;
			}
			for (int n = 0; n < this.myrtilusLadder.Count; n++)
			{
				num2 += (int)this.myrtilusLadder[n];
			}
			num2 += this.currentLadderIdx;
			num2 += this.currentTurn;
			for (int num3 = 0; num3 < this.warbandMorals.Count; num3++)
			{
				num2 += this.warbandMorals[num3].Item1;
				num2 += this.warbandMorals[num3].Item2;
				num2 += ((!this.warbandMorals[num3].Item3) ? 0 : 1);
			}
			num2 += ((!this.missionFinished) ? 0 : 1);
			for (int num4 = 0; num4 < this.reinforcements.Count; num4++)
			{
				num2 += this.reinforcements[num4].Key;
				num2 += this.reinforcements[num4].Value;
			}
			for (int num5 = 0; num5 < this.objectives.Count; num5++)
			{
				num2 += (int)this.objectives[num5].Key;
				num2 += (int)this.objectives[num5].Value;
			}
			for (int num6 = 0; num6 < this.converters.Count; num6++)
			{
				num2 += (int)this.converters[num6].Key;
				num2 += this.converters[num6].Value;
			}
			for (int num7 = 0; num7 < this.activaters.Count; num7++)
			{
				num2 += (int)this.activaters[num7].Key;
				num2 += ((!this.activaters[num7].Value) ? 0 : 1);
			}
			if (num > 6)
			{
				for (int num8 = 0; num8 < this.dynamicTraps.Count; num8++)
				{
					num2 += this.dynamicTraps[num8].trapTypeId;
					num2 += this.dynamicTraps[num8].teamIdx;
					num2 += (int)this.dynamicTraps[num8].pos.x;
					num2 += (int)this.dynamicTraps[num8].pos.y;
					num2 += (int)this.dynamicTraps[num8].pos.z;
					num2 += (int)this.dynamicTraps[num8].rot.x;
					num2 += (int)this.dynamicTraps[num8].rot.y;
					num2 += (int)this.dynamicTraps[num8].rot.z;
					num2 += (int)this.dynamicTraps[num8].rot.w;
				}
			}
			if (num > 7)
			{
				for (int num9 = 0; num9 < this.destructibles.Count; num9++)
				{
					num2 = (int)(num2 + this.destructibles[num9].destructibleId);
					num2 += this.destructibles[num9].wounds;
					num2 += (int)this.destructibles[num9].onwerGuid;
					num2 += (int)this.destructibles[num9].position.x;
					num2 += (int)this.destructibles[num9].position.y;
					num2 += (int)this.destructibles[num9].position.z;
					num2 += (int)this.destructibles[num9].rot.x;
					num2 += (int)this.destructibles[num9].rot.y;
					num2 += (int)this.destructibles[num9].rot.z;
					num2 += (int)this.destructibles[num9].rot.w;
				}
			}
		}
		return num2;
	}

	public void Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, (int)this.ratingId);
		global::Thoth.Write(writer, this.won);
		global::Thoth.Write(writer, this.primaryObjectiveCompleted);
		global::Thoth.Write(writer, this.crushed);
		global::Thoth.Write(writer, this.isCampaign);
		global::Thoth.Write(writer, this.isSkirmish);
		global::Thoth.Write(writer, this.isVsAI);
		global::Thoth.Write(writer, this.units.Count);
		for (int i = 0; i < this.units.Count; i++)
		{
			this.units[i].Write(writer);
		}
		global::Thoth.Write(writer, this.wagonItems.GetItems().Count);
		for (int j = 0; j < this.wagonItems.GetItems().Count; j++)
		{
			((global::IThoth)this.wagonItems.GetItems()[j]).Write(writer);
		}
		global::Thoth.Write(writer, this.playerMVUIdx);
		global::Thoth.Write(writer, this.enemyMVUIdx);
		global::Thoth.Write(writer, this.routable);
		((global::IThoth)this.missionSave).Write(writer);
		global::Thoth.Write(writer, this.seed);
		global::Thoth.Write(writer, this.missionWarbands.Count);
		for (int k = 0; k < this.missionWarbands.Count; k++)
		{
			this.missionWarbands[k].Write(writer);
		}
		global::Thoth.Write(writer, this.destroyedTraps.Count);
		for (int l = 0; l < this.destroyedTraps.Count; l++)
		{
			global::Thoth.Write(writer, this.destroyedTraps[l]);
		}
		global::Thoth.Write(writer, this.searches.Count);
		for (int m = 0; m < this.searches.Count; m++)
		{
			global::Thoth.Write(writer, this.searches[m].Key);
			if (this.searches[m].Value != null)
			{
				global::Thoth.Write(writer, this.searches[m].Value.unitCtrlrUid);
				global::Thoth.Write(writer, this.searches[m].Value.pos.x);
				global::Thoth.Write(writer, this.searches[m].Value.pos.y);
				global::Thoth.Write(writer, this.searches[m].Value.pos.z);
				global::Thoth.Write(writer, this.searches[m].Value.items.Count);
				for (int n = 0; n < this.searches[m].Value.items.Count; n++)
				{
					((global::IThoth)this.searches[m].Value.items[n]).Write(writer);
				}
				global::Thoth.Write(writer, this.searches[m].Value.wasSearched);
			}
			else
			{
				global::Thoth.Write(writer, 0);
			}
		}
		global::Thoth.Write(writer, this.aoeZones.Count);
		for (int num = 0; num < this.aoeZones.Count; num++)
		{
			global::Thoth.Write(writer, this.aoeZones[num].guid);
			global::Thoth.Write(writer, this.aoeZones[num].myrtilusId);
			global::Thoth.Write(writer, (int)this.aoeZones[num].aoeId);
			global::Thoth.Write(writer, this.aoeZones[num].radius);
			global::Thoth.Write(writer, this.aoeZones[num].durationLeft);
			global::Thoth.Write(writer, this.aoeZones[num].position.x);
			global::Thoth.Write(writer, this.aoeZones[num].position.y);
			global::Thoth.Write(writer, this.aoeZones[num].position.z);
		}
		global::Thoth.Write(writer, this.myrtilusLadder.Count);
		for (int num2 = 0; num2 < this.myrtilusLadder.Count; num2++)
		{
			global::Thoth.Write(writer, this.myrtilusLadder[num2]);
		}
		global::Thoth.Write(writer, this.currentLadderIdx);
		global::Thoth.Write(writer, this.currentTurn);
		global::Thoth.Write(writer, this.warbandMorals.Count);
		for (int num3 = 0; num3 < this.warbandMorals.Count; num3++)
		{
			global::Thoth.Write(writer, this.warbandMorals[num3].Item1);
			global::Thoth.Write(writer, this.warbandMorals[num3].Item2);
			global::Thoth.Write(writer, this.warbandMorals[num3].Item3);
		}
		global::Thoth.Write(writer, this.missionFinished);
		global::Thoth.Write(writer, this.reinforcements.Count);
		for (int num4 = 0; num4 < this.reinforcements.Count; num4++)
		{
			global::Thoth.Write(writer, this.reinforcements[num4].Key);
			global::Thoth.Write(writer, this.reinforcements[num4].Value);
		}
		global::Thoth.Write(writer, this.objectives.Count);
		for (int num5 = 0; num5 < this.objectives.Count; num5++)
		{
			global::Thoth.Write(writer, this.objectives[num5].Key);
			global::Thoth.Write(writer, this.objectives[num5].Value);
		}
		global::Thoth.Write(writer, this.converters.Count);
		for (int num6 = 0; num6 < this.converters.Count; num6++)
		{
			global::Thoth.Write(writer, this.converters[num6].Key);
			global::Thoth.Write(writer, this.converters[num6].Value);
		}
		global::Thoth.Write(writer, this.activaters.Count);
		for (int num7 = 0; num7 < this.activaters.Count; num7++)
		{
			global::Thoth.Write(writer, this.activaters[num7].Key);
			global::Thoth.Write(writer, this.activaters[num7].Value);
		}
		global::Thoth.Write(writer, this.dynamicTraps.Count);
		for (int num8 = 0; num8 < this.dynamicTraps.Count; num8++)
		{
			global::Thoth.Write(writer, this.dynamicTraps[num8].guid);
			global::Thoth.Write(writer, this.dynamicTraps[num8].trapTypeId);
			global::Thoth.Write(writer, this.dynamicTraps[num8].teamIdx);
			global::Thoth.Write(writer, this.dynamicTraps[num8].pos.x);
			global::Thoth.Write(writer, this.dynamicTraps[num8].pos.y);
			global::Thoth.Write(writer, this.dynamicTraps[num8].pos.z);
			global::Thoth.Write(writer, this.dynamicTraps[num8].rot.x);
			global::Thoth.Write(writer, this.dynamicTraps[num8].rot.y);
			global::Thoth.Write(writer, this.dynamicTraps[num8].rot.z);
			global::Thoth.Write(writer, this.dynamicTraps[num8].rot.w);
		}
		global::Thoth.Write(writer, this.destructibles.Count);
		for (int num9 = 0; num9 < this.destructibles.Count; num9++)
		{
			global::Thoth.Write(writer, this.destructibles[num9].guid);
			global::Thoth.Write(writer, (int)this.destructibles[num9].destructibleId);
			global::Thoth.Write(writer, this.destructibles[num9].onwerGuid);
			global::Thoth.Write(writer, this.destructibles[num9].wounds);
			global::Thoth.Write(writer, this.destructibles[num9].position.x);
			global::Thoth.Write(writer, this.destructibles[num9].position.y);
			global::Thoth.Write(writer, this.destructibles[num9].position.z);
			global::Thoth.Write(writer, this.destructibles[num9].rot.x);
			global::Thoth.Write(writer, this.destructibles[num9].rot.y);
			global::Thoth.Write(writer, this.destructibles[num9].rot.z);
			global::Thoth.Write(writer, this.destructibles[num9].rot.w);
		}
	}

	public int lastVersion;

	public global::ProcMissionRatingId ratingId;

	public bool won;

	public bool primaryObjectiveCompleted;

	public bool crushed;

	public bool isCampaign;

	public bool isSkirmish;

	public bool isVsAI;

	public global::System.Collections.Generic.List<global::MissionEndUnitSave> units;

	public global::Chest wagonItems = new global::Chest();

	public int playerMVUIdx;

	public int enemyMVUIdx = -1;

	public global::MissionSave missionSave = new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE));

	public bool routable;

	public int seed;

	public global::System.Collections.Generic.List<global::MissionWarbandSave> missionWarbands = new global::System.Collections.Generic.List<global::MissionWarbandSave>();

	public global::System.Collections.Generic.List<global::Tuple<int, int, bool>> warbandMorals = new global::System.Collections.Generic.List<global::Tuple<int, int, bool>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>> searches = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>>();

	public global::System.Collections.Generic.List<uint> destroyedTraps = new global::System.Collections.Generic.List<uint>();

	public global::System.Collections.Generic.List<global::EndZoneAoe> aoeZones = new global::System.Collections.Generic.List<global::EndZoneAoe>();

	public global::System.Collections.Generic.List<uint> myrtilusLadder = new global::System.Collections.Generic.List<uint>();

	public int currentLadderIdx;

	public int currentTurn;

	public bool missionFinished;

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>> reinforcements = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, uint>> objectives = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, uint>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, int>> converters = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, int>>();

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, bool>> activaters = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, bool>>();

	public global::System.Collections.Generic.List<global::EndDynamicTrap> dynamicTraps = new global::System.Collections.Generic.List<global::EndDynamicTrap>();

	public global::System.Collections.Generic.List<global::EndDestructible> destructibles = new global::System.Collections.Generic.List<global::EndDestructible>();
}
