using System;
using System.Collections.Generic;
using System.IO;

public class WarbandSave : global::IThoth
{
	public WarbandSave() : this(global::WarbandId.NONE)
	{
	}

	public WarbandSave(global::WarbandId warbandId)
	{
		this.name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + warbandId.ToString().ToLowerInvariant());
		this.overrideName = string.Empty;
		this.id = (int)warbandId;
		this.currentDate = 0;
		this.units = new global::System.Collections.Generic.List<global::UnitSave>();
		this.outsiders = new global::System.Collections.Generic.List<global::UnitSave>();
		this.skills = new global::System.Collections.Generic.List<global::WarbandSkillId>();
		this.oldUnits = new global::System.Collections.Generic.List<global::UnitStatSave>();
		this.items = new global::System.Collections.Generic.List<global::ItemSave>();
		this.marketItems = new global::System.Collections.Generic.List<global::ItemSave>();
		this.addedMarketItems = new global::System.Collections.Generic.List<global::ItemSave>();
		this.stats = new global::WarbandStatSave();
		this.factions = new global::System.Collections.Generic.List<global::FactionSave>();
		this.missions = new global::System.Collections.Generic.List<global::MissionSave>();
		this.lateShipmentCount = 0;
		this.lastShipmentFailed = false;
		this.nextShipmentExtraDays = 0;
		this.campaignId = 0;
		this.curCampaignIdx = 1;
		this.scoutsSent = 0;
		this.marketEventId = 0;
		this.hideoutTutos = 0U;
		this.smugglersMaxRankShown = false;
		this.unitsSlotsIndex = new global::System.Collections.Generic.List<int>();
		this.exhibitionUnitsSlotsIndex = new global::System.Collections.Generic.List<int>();
		this.winningStreak = 0;
		this.warbandFaced = 0;
		this.inMission = false;
		this.endMission = new global::MissionEndDataSave();
		this.lastMissionAmbushed = false;
		this.availaibleRespec = -1;
	}

	int global::IThoth.GetVersion()
	{
		return 34;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		int num = (int)global::PandoraSingleton<global::Hephaestus>.Instance.GetUserId();
		global::Thoth.Write(writer, crc + num);
		global::Thoth.Write(writer, this.name);
		global::Thoth.Write(writer, this.id);
		global::Thoth.Write(writer, this.currentDate);
		global::Thoth.Write(writer, this.rank);
		global::Thoth.Write(writer, this.xp);
		global::Thoth.Write(writer, this.units.Count);
		for (int i = 0; i < this.units.Count; i++)
		{
			((global::IThoth)this.units[i]).Write(writer);
		}
		if (version > 10)
		{
			global::Thoth.Write(writer, this.oldUnits.Count);
			for (int j = 0; j < this.oldUnits.Count; j++)
			{
				((global::IThoth)this.oldUnits[j]).Write(writer);
			}
		}
		global::Thoth.Write(writer, this.items.Count);
		for (int k = 0; k < this.items.Count; k++)
		{
			((global::IThoth)this.items[k]).Write(writer);
		}
		global::Thoth.Write(writer, this.marketEventId);
		global::Thoth.Write(writer, this.marketItems.Count);
		for (int l = 0; l < this.marketItems.Count; l++)
		{
			((global::IThoth)this.marketItems[l]).Write(writer);
		}
		global::Thoth.Write(writer, this.addedMarketItems.Count);
		for (int m = 0; m < this.addedMarketItems.Count; m++)
		{
			((global::IThoth)this.addedMarketItems[m]).Write(writer);
		}
		((global::IThoth)this.stats).Write(writer);
		global::Thoth.Write(writer, this.factions.Count);
		for (int n = 0; n < this.factions.Count; n++)
		{
			((global::IThoth)this.factions[n]).Write(writer);
		}
		global::Thoth.Write(writer, this.lateShipmentCount);
		global::Thoth.Write(writer, this.lastShipmentFailed);
		global::Thoth.Write(writer, this.nextShipmentExtraDays);
		global::Thoth.Write(writer, this.curCampaignIdx);
		global::Thoth.Write(writer, this.missions.Count);
		for (int num2 = 0; num2 < this.missions.Count; num2++)
		{
			((global::IThoth)this.missions[num2]).Write(writer);
		}
		global::Thoth.Write(writer, this.scoutsSent);
		global::Thoth.Write(writer, this.skills.Count);
		for (int num3 = 0; num3 < this.skills.Count; num3++)
		{
			global::Thoth.Write(writer, (int)this.skills[num3]);
		}
		global::Thoth.Write(writer, this.outsiders.Count);
		for (int num4 = 0; num4 < this.outsiders.Count; num4++)
		{
			((global::IThoth)this.outsiders[num4]).Write(writer);
		}
		global::Thoth.Write(writer, this.hideoutTutos);
		global::Thoth.Write(writer, this.smugglersMaxRankShown);
		global::Thoth.Write(writer, this.winningStreak);
		global::Thoth.Write(writer, this.warbandFaced);
		global::Thoth.Write(writer, this.inMission);
		if (this.inMission)
		{
			((global::IThoth)this.endMission).Write(writer);
		}
		global::Thoth.Write(writer, this.lastMissionAmbushed);
		global::Thoth.Write(writer, this.overrideName);
		global::Thoth.Write(writer, this.availaibleRespec);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 > 26)
		{
			global::Thoth.Read(reader, out num);
		}
		global::Thoth.Read(reader, out this.name);
		global::Thoth.Read(reader, out this.id);
		if (num2 < 10)
		{
			global::Thoth.Read(reader, out this.campaignId);
		}
		global::Thoth.Read(reader, out this.currentDate);
		global::Thoth.Read(reader, out this.rank);
		global::Thoth.Read(reader, out this.xp);
		int num3 = 0;
		global::Thoth.Read(reader, out num3);
		for (int i = 0; i < num3; i++)
		{
			global::UnitSave unitSave = new global::UnitSave(global::UnitId.NONE);
			((global::IThoth)unitSave).Read(reader);
			this.units.Add(unitSave);
		}
		if (num2 > 10)
		{
			global::Thoth.Read(reader, out num3);
			for (int j = 0; j < num3; j++)
			{
				global::UnitStatSave unitStatSave = new global::UnitStatSave();
				((global::IThoth)unitStatSave).Read(reader);
				this.oldUnits.Add(unitStatSave);
			}
		}
		global::Thoth.Read(reader, out num3);
		for (int k = 0; k < num3; k++)
		{
			global::ItemSave itemSave = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
			((global::IThoth)itemSave).Read(reader);
			this.items.Add(itemSave);
		}
		if (num2 >= 14)
		{
			global::Thoth.Read(reader, out this.marketEventId);
			global::Thoth.Read(reader, out num3);
			for (int l = 0; l < num3; l++)
			{
				global::ItemSave itemSave2 = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
				((global::IThoth)itemSave2).Read(reader);
				this.marketItems.Add(itemSave2);
			}
		}
		if (num2 >= 15)
		{
			global::Thoth.Read(reader, out num3);
			for (int m = 0; m < num3; m++)
			{
				global::ItemSave itemSave3 = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
				((global::IThoth)itemSave3).Read(reader);
				this.addedMarketItems.Add(itemSave3);
			}
		}
		((global::IThoth)this.stats).Read(reader);
		if (num2 > 8)
		{
			global::Thoth.Read(reader, out num3);
			for (int n = 0; n < num3; n++)
			{
				global::FactionSave factionSave = new global::FactionSave();
				((global::IThoth)factionSave).Read(reader);
				this.factions.Add(factionSave);
			}
			global::Thoth.Read(reader, out this.lateShipmentCount);
			global::Thoth.Read(reader, out this.lastShipmentFailed);
			global::Thoth.Read(reader, out this.nextShipmentExtraDays);
		}
		if (num2 > 9)
		{
			global::Thoth.Read(reader, out this.curCampaignIdx);
			global::Thoth.Read(reader, out num3);
			for (int num4 = 0; num4 < num3; num4++)
			{
				global::MissionSave missionSave = new global::MissionSave(global::Constant.GetFloat(global::ConstantId.ROUT_RATIO_ALIVE));
				((global::IThoth)missionSave).Read(reader);
				this.missions.Add(missionSave);
			}
		}
		if (num2 > 10)
		{
			global::Thoth.Read(reader, out this.scoutsSent);
		}
		if (num2 > 11)
		{
			global::Thoth.Read(reader, out num3);
			for (int num5 = 0; num5 < num3; num5++)
			{
				int item;
				global::Thoth.Read(reader, out item);
				this.skills.Add((global::WarbandSkillId)item);
			}
		}
		if (num2 > 18)
		{
			global::Thoth.Read(reader, out num3);
			for (int num6 = 0; num6 < num3; num6++)
			{
				global::UnitSave unitSave2 = new global::UnitSave(global::UnitId.NONE);
				((global::IThoth)unitSave2).Read(reader);
				this.outsiders.Add(unitSave2);
			}
		}
		if (num2 > 20)
		{
			global::Thoth.Read(reader, out this.hideoutTutos);
		}
		if (num2 > 22)
		{
			global::Thoth.Read(reader, out this.smugglersMaxRankShown);
		}
		if (num2 > 21)
		{
			global::Thoth.Read(reader, out this.winningStreak);
		}
		if (num2 > 25)
		{
			global::Thoth.Read(reader, out this.warbandFaced);
		}
		if (num2 > 27)
		{
			global::Thoth.Read(reader, out this.inMission);
		}
		if (num2 > 28 && this.inMission)
		{
			((global::IThoth)this.endMission).Read(reader);
		}
		if (num2 > 29)
		{
			global::Thoth.Read(reader, out this.lastMissionAmbushed);
		}
		if (num2 > 32)
		{
			global::Thoth.Read(reader, out this.overrideName);
		}
		if (num2 > 33)
		{
			global::Thoth.Read(reader, out this.availaibleRespec);
		}
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	public string Name
	{
		get
		{
			if (string.IsNullOrEmpty(this.overrideName) || global::PandoraSingleton<global::Hephaestus>.Instance.IsPrivilegeRestricted(global::Hephaestus.RestrictionId.UGC))
			{
				return this.name;
			}
			return this.overrideName;
		}
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		num2 += this.id;
		num2 += this.currentDate;
		for (int i = 0; i < this.units.Count; i++)
		{
			num2 += this.units[i].GetCRC(read);
		}
		for (int j = 0; j < this.outsiders.Count; j++)
		{
			num2 += this.outsiders[j].GetCRC(read);
		}
		for (int k = 0; k < this.skills.Count; k++)
		{
			num2 = (int)(num2 + this.skills[k]);
		}
		for (int l = 0; l < this.oldUnits.Count; l++)
		{
			num2 += this.oldUnits[l].GetCRC(read);
		}
		for (int m = 0; m < this.items.Count; m++)
		{
			num2 += this.items[m].GetCRC(read);
		}
		for (int n = 0; n < this.marketItems.Count; n++)
		{
			num2 += this.marketItems[n].GetCRC(read);
		}
		for (int num3 = 0; num3 < this.addedMarketItems.Count; num3++)
		{
			num2 += this.addedMarketItems[num3].GetCRC(read);
		}
		num2 += this.stats.GetCRC(read);
		for (int num4 = 0; num4 < this.factions.Count; num4++)
		{
			num2 += this.factions[num4].GetCRC(read);
		}
		for (int num5 = 0; num5 < this.missions.Count; num5++)
		{
			num2 += this.missions[num5].GetCRC(read);
		}
		num2 += this.lateShipmentCount;
		num2 += ((!this.lastShipmentFailed) ? 0 : 1);
		num2 += this.nextShipmentExtraDays;
		num2 += this.campaignId;
		num2 += this.curCampaignIdx;
		num2 += this.scoutsSent;
		num2 += this.marketEventId;
		num2 += (int)this.hideoutTutos;
		num2 += ((!this.smugglersMaxRankShown) ? 0 : 1);
		for (int num6 = 0; num6 < this.unitsSlotsIndex.Count; num6++)
		{
			num2 += this.unitsSlotsIndex[num6];
		}
		for (int num7 = 0; num7 < this.exhibitionUnitsSlotsIndex.Count; num7++)
		{
			num2 += this.exhibitionUnitsSlotsIndex[num7];
		}
		num2 += this.winningStreak;
		if (num > 25)
		{
			num2 += this.warbandFaced;
		}
		if (num > 27)
		{
			num2 += ((!this.inMission) ? 0 : 1);
		}
		if (this.inMission && num > 28)
		{
			num2 += this.endMission.GetCRC(read);
		}
		if (num > 29)
		{
			num2 += ((!this.lastMissionAmbushed) ? 0 : 1);
		}
		if (num > 32)
		{
			num2 += ((!this.lastMissionAmbushed) ? 0 : 1);
		}
		if (num > 33)
		{
			num2 += this.availaibleRespec;
		}
		return num2;
	}

	public int lastVersion;

	public string overrideName;

	public string name;

	public int id;

	public int currentDate;

	public int rank;

	public int xp;

	public int scoutsSent;

	public global::WarbandStatSave stats;

	public global::System.Collections.Generic.List<global::UnitSave> units;

	public global::System.Collections.Generic.List<global::UnitSave> outsiders;

	public global::System.Collections.Generic.List<global::WarbandSkillId> skills;

	public global::System.Collections.Generic.List<global::UnitStatSave> oldUnits;

	public global::System.Collections.Generic.List<global::ItemSave> items;

	public int marketEventId;

	public global::System.Collections.Generic.List<global::ItemSave> marketItems;

	public global::System.Collections.Generic.List<global::ItemSave> addedMarketItems;

	public global::System.Collections.Generic.List<global::FactionSave> factions;

	public int lateShipmentCount;

	public bool lastShipmentFailed;

	public int nextShipmentExtraDays;

	public int campaignId;

	public int curCampaignIdx;

	public global::System.Collections.Generic.List<global::MissionSave> missions;

	public uint hideoutTutos;

	public bool smugglersMaxRankShown;

	public global::System.Collections.Generic.List<int> unitsSlotsIndex;

	public global::System.Collections.Generic.List<int> exhibitionUnitsSlotsIndex;

	public int winningStreak;

	public int warbandFaced;

	public bool inMission;

	public global::MissionEndDataSave endMission;

	public bool lastMissionAmbushed;

	public int availaibleRespec;
}
