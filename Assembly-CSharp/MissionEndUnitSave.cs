using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MissionEndUnitSave : global::IThoth
{
	public MissionEndUnitSave()
	{
		this.items = new global::System.Collections.Generic.List<global::Item>();
		this.injuries = new global::System.Collections.Generic.List<global::InjuryData>();
		this.XPs = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, string>>();
		this.advancements = new global::System.Collections.Generic.List<global::UnitJoinUnitRankData>();
		this.mutations = new global::System.Collections.Generic.List<global::Mutation>();
		this.enchantments = new global::System.Collections.Generic.List<global::EndUnitEnchantment>();
		this.unitSave = new global::UnitSave();
		this.lostItems = new global::System.Collections.Generic.List<global::Item>();
		this.mvuPerCategories = new int[5];
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		global::Thoth.Read(reader, out num);
		int num3 = 0;
		global::Thoth.Read(reader, out num3);
		this.status = (global::UnitStateId)num3;
		global::Thoth.Read(reader, out this.costOfLosingId);
		int num4 = 0;
		global::Thoth.Read(reader, out num4);
		for (int i = 0; i < num4; i++)
		{
			global::EndUnitEnchantment item = default(global::EndUnitEnchantment);
			if (num2 == 0)
			{
				int enchantId = 0;
				global::Thoth.Read(reader, out enchantId);
				item.enchantId = (global::EnchantmentId)enchantId;
			}
			else
			{
				global::Thoth.Read(reader, out item.guid);
				int enchantId2 = 0;
				global::Thoth.Read(reader, out enchantId2);
				item.enchantId = (global::EnchantmentId)enchantId2;
				global::Thoth.Read(reader, out item.durationLeft);
				global::Thoth.Read(reader, out item.ownerMyrtilusId);
				if (num2 > 2)
				{
					global::Thoth.Read(reader, out item.runeAllegianceId);
				}
			}
			this.enchantments.Add(item);
		}
		((global::IThoth)this.unitSave).Read(reader);
		if (num2 > 0)
		{
			global::Thoth.Read(reader, out this.position.x);
			global::Thoth.Read(reader, out this.position.y);
			global::Thoth.Read(reader, out this.position.z);
			global::Thoth.Read(reader, out this.rotation.x);
			global::Thoth.Read(reader, out this.rotation.y);
			global::Thoth.Read(reader, out this.rotation.z);
			global::Thoth.Read(reader, out this.myrtilusId);
			global::Thoth.Read(reader, out this.currentWounds);
			global::Thoth.Read(reader, out this.currentSP);
			global::Thoth.Read(reader, out this.currentOP);
			int num5 = 0;
			global::Thoth.Read(reader, out num5);
			this.weaponSet = (global::UnitSlotId)num5;
			global::Thoth.Read(reader, out this.turnStarted);
		}
		if (num2 > 1)
		{
			global::Thoth.Read(reader, out this.isPlayed);
		}
		if (num2 > 3)
		{
			global::Thoth.Read(reader, out this.isLadderVisible);
		}
		else
		{
			this.isLadderVisible = (this.status != global::UnitStateId.OUT_OF_ACTION);
		}
		if (num2 > 4)
		{
			global::Thoth.Read(reader, out this.currentMvu);
			global::Thoth.Read(reader, out num4);
			if (num4 != 5)
			{
				this.mvuPerCategories = new int[5];
			}
			for (int j = 0; j < num4; j++)
			{
				int num6;
				global::Thoth.Read(reader, out num6);
				this.mvuPerCategories[j] = num6;
			}
		}
		for (int k = 0; k < this.unitSave.items.Count; k++)
		{
			if (this.unitSave.items[k] == null)
			{
				this.items.Add(new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL));
			}
			else
			{
				this.items.Add(new global::Item(this.unitSave.items[k]));
			}
		}
	}

	public int GetVersion()
	{
		return 5;
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	public void UpdateUnit(global::UnitController unit)
	{
		this.myrtilusId = unit.uid;
		this.position = unit.transform.position;
		this.rotation = unit.transform.rotation.eulerAngles;
		this.currentWounds = unit.unit.CurrentWound;
		this.currentSP = unit.unit.CurrentStrategyPoints;
		this.currentOP = unit.unit.CurrentOffensePoints;
		this.currentMvu = unit.unit.GetAttribute(global::AttributeId.CURRENT_MVU);
		this.mvuPerCategories = unit.MVUptsPerCategory;
		this.unitSave = unit.unit.UnitSave;
		this.status = unit.unit.Status;
		this.items.Clear();
		this.items.AddRange(unit.unit.Items);
		this.isPlayed = (unit.IsPlayed() && unit.unit.CampaignData == null);
		this.isLadderVisible = unit.ladderVisible;
		this.weaponSet = unit.unit.ActiveWeaponSlot;
		this.turnStarted = unit.TurnStarted;
		this.enchantments.Clear();
		for (int i = 0; i < unit.unit.Enchantments.Count; i++)
		{
			if (!unit.unit.Enchantments[i].original)
			{
				global::EndUnitEnchantment item = default(global::EndUnitEnchantment);
				item.guid = unit.unit.Enchantments[i].guid;
				item.enchantId = unit.unit.Enchantments[i].Id;
				item.durationLeft = unit.unit.Enchantments[i].Duration;
				item.runeAllegianceId = (int)unit.unit.Enchantments[i].AllegianceId;
				global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(unit.unit.Enchantments[i].Provider, true);
				if (unitController != null)
				{
					item.ownerMyrtilusId = unitController.uid;
				}
				else
				{
					item.ownerMyrtilusId = unit.uid;
				}
				this.enchantments.Add(item);
			}
		}
	}

	public int GetAttribute(global::AttributeId attributeId)
	{
		return this.unitSave.stats.stats[(int)attributeId];
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		num2 = (int)(num2 + this.status);
		num2 += this.costOfLosingId;
		for (int i = 0; i < this.enchantments.Count; i++)
		{
			if (num > 0)
			{
				num2 += (int)this.enchantments[i].guid;
			}
			num2 = (int)(num2 + this.enchantments[i].enchantId);
			if (num > 0)
			{
				num2 += this.enchantments[i].durationLeft;
				num2 += (int)this.enchantments[i].ownerMyrtilusId;
			}
			if (num > 2)
			{
				num2 += this.enchantments[i].runeAllegianceId;
			}
		}
		num2 += this.unitSave.GetCRC(read);
		if (num > 0)
		{
			num2 += (int)this.position.x;
			num2 += (int)this.position.y;
			num2 += (int)this.position.z;
			num2 += (int)this.rotation.x;
			num2 += (int)this.rotation.y;
			num2 += (int)this.rotation.z;
			num2 += (int)this.myrtilusId;
			num2 += this.currentWounds;
			num2 += this.currentSP;
			num2 += this.currentOP;
			num2 = (int)(num2 + this.weaponSet);
			num2 += ((!this.turnStarted) ? 0 : 1);
		}
		if (num > 1)
		{
			num2 += ((!this.isPlayed) ? 0 : 1);
		}
		if (num > 3)
		{
			num2 += ((!this.isLadderVisible) ? 0 : 1);
		}
		return num2;
	}

	public void Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, (int)this.status);
		global::Thoth.Write(writer, this.costOfLosingId);
		global::Thoth.Write(writer, this.enchantments.Count);
		for (int i = 0; i < this.enchantments.Count; i++)
		{
			global::Thoth.Write(writer, this.enchantments[i].guid);
			global::Thoth.Write(writer, (int)this.enchantments[i].enchantId);
			global::Thoth.Write(writer, this.enchantments[i].durationLeft);
			global::Thoth.Write(writer, this.enchantments[i].ownerMyrtilusId);
			global::Thoth.Write(writer, this.enchantments[i].runeAllegianceId);
		}
		((global::IThoth)this.unitSave).Write(writer);
		if (version > 0)
		{
			global::Thoth.Write(writer, this.position.x);
			global::Thoth.Write(writer, this.position.y);
			global::Thoth.Write(writer, this.position.z);
			global::Thoth.Write(writer, this.rotation.x);
			global::Thoth.Write(writer, this.rotation.y);
			global::Thoth.Write(writer, this.rotation.z);
			global::Thoth.Write(writer, this.myrtilusId);
			global::Thoth.Write(writer, this.currentWounds);
			global::Thoth.Write(writer, this.currentSP);
			global::Thoth.Write(writer, this.currentOP);
			global::Thoth.Write(writer, (int)this.weaponSet);
			global::Thoth.Write(writer, this.turnStarted);
		}
		if (version > 1)
		{
			global::Thoth.Write(writer, this.isPlayed);
		}
		if (version > 3)
		{
			global::Thoth.Write(writer, this.isLadderVisible);
		}
		if (version > 4)
		{
			global::Thoth.Write(writer, this.currentMvu);
			global::Thoth.Write(writer, this.mvuPerCategories.Length);
			for (int j = 0; j < this.mvuPerCategories.Length; j++)
			{
				global::Thoth.Write(writer, this.mvuPerCategories[j]);
			}
		}
	}

	private int lastVersion;

	public global::UnitStateId status;

	public global::System.Collections.Generic.List<global::Item> items;

	public global::System.Collections.Generic.List<global::EndUnitEnchantment> enchantments;

	public global::UnitSave unitSave;

	public global::UnityEngine.Vector3 position;

	public global::UnityEngine.Vector3 rotation;

	public uint myrtilusId;

	public int currentMvu;

	public int[] mvuPerCategories;

	public int currentWounds;

	public int currentSP;

	public int currentOP;

	public global::UnitSlotId weaponSet;

	public bool turnStarted;

	public bool isPlayed;

	public int costOfLosingId;

	public global::System.Collections.Generic.List<global::InjuryData> injuries;

	public bool dead;

	public bool isLadderVisible;

	public bool isMaxRank;

	public global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, string>> XPs;

	public global::System.Collections.Generic.List<global::UnitJoinUnitRankData> advancements;

	public global::System.Collections.Generic.List<global::Mutation> mutations;

	public global::System.Collections.Generic.List<global::Item> lostItems;
}
