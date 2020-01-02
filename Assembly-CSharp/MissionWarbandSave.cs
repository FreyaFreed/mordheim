using System;
using System.Collections.Generic;
using System.IO;

public class MissionWarbandSave : global::IThoth
{
	public MissionWarbandSave()
	{
	}

	public MissionWarbandSave(global::WarbandId type, global::CampaignWarbandId campaignId, string name, string overrideName, string playerName, int rank, int rating, int playerIndex, global::PlayerTypeId playerTypeId, string[] units)
	{
		this.WarbandId = type;
		this.CampaignWarId = campaignId;
		this.Name = ((string.IsNullOrEmpty(overrideName) || (global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex != playerIndex && global::PandoraSingleton<global::Hephaestus>.Instance.IsPrivilegeRestricted(global::Hephaestus.RestrictionId.UGC))) ? name : overrideName);
		this.OverrideName = overrideName;
		this.PlayerName = playerName;
		this.Rank = rank;
		this.Rating = rating;
		this.PlayerIndex = playerIndex;
		this.PlayerTypeId = playerTypeId;
		this.SerializedUnits = units;
		this.Units = new global::System.Collections.Generic.List<global::UnitSave>();
		if (units != null)
		{
			for (int i = 0; i < units.Length; i++)
			{
				global::UnitSave unitSave = new global::UnitSave();
				global::Thoth.ReadFromString(units[i], unitSave);
				this.Units.Add(unitSave);
			}
		}
		this.ResetReady();
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
		this.WarbandId = (global::WarbandId)num3;
		global::Thoth.Read(reader, out num3);
		this.CampaignWarId = (global::CampaignWarbandId)num3;
		string text = null;
		global::Thoth.Read(reader, out text);
		this.Name = text;
		global::Thoth.Read(reader, out text);
		this.PlayerName = text;
		global::Thoth.Read(reader, out num3);
		this.Rank = num3;
		global::Thoth.Read(reader, out num3);
		this.Rating = num3;
		global::Thoth.Read(reader, out num3);
		this.PlayerIndex = num3;
		global::Thoth.Read(reader, out num3);
		this.PlayerTypeId = (global::PlayerTypeId)num3;
		int num4 = 0;
		global::Thoth.Read(reader, out num4);
		this.Units = new global::System.Collections.Generic.List<global::UnitSave>(num4);
		for (int i = 0; i < num4; i++)
		{
			global::UnitSave unitSave = new global::UnitSave();
			((global::IThoth)unitSave).Read(reader);
			this.Units.Add(unitSave);
		}
		if (num2 > 0)
		{
			num4 = 0;
			global::Thoth.Read(reader, out num4);
			for (int j = 0; j < num4; j++)
			{
				uint item = 0U;
				global::Thoth.Read(reader, out item);
				this.openedSearches.Add(item);
			}
		}
	}

	public int GetVersion()
	{
		return 1;
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	public global::WarbandId WarbandId { get; private set; }

	public global::CampaignWarbandId CampaignWarId { get; private set; }

	public string Name { get; private set; }

	public string OverrideName { get; private set; }

	public string PlayerName { get; private set; }

	public int Rank { get; private set; }

	public int Rating { get; private set; }

	public int PlayerIndex { get; private set; }

	public global::PlayerTypeId PlayerTypeId { get; private set; }

	public bool IsReady { get; set; }

	public string[] SerializedUnits { get; private set; }

	public global::System.Collections.Generic.List<global::UnitSave> Units { get; set; }

	public global::WarbandSave ToWarbandSave()
	{
		return new global::WarbandSave(this.WarbandId)
		{
			name = this.Name,
			units = this.Units
		};
	}

	public void ResetReady()
	{
		this.IsReady = (this.PlayerTypeId == global::PlayerTypeId.AI);
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		num2 = (int)(num2 + this.WarbandId);
		num2 = (int)(num2 + this.CampaignWarId);
		char[] array = this.Name.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			num2 += (int)array[i];
		}
		array = this.PlayerName.ToCharArray();
		for (int j = 0; j < array.Length; j++)
		{
			num2 += (int)array[j];
		}
		num2 += this.Rank;
		num2 += this.Rating;
		num2 += this.PlayerIndex;
		num2 = (int)(num2 + this.PlayerTypeId);
		for (int k = 0; k < this.Units.Count; k++)
		{
			num2 += this.Units[k].GetCRC(read);
		}
		if (num > 0)
		{
			for (int l = 0; l < this.openedSearches.Count; l++)
			{
				num2 += (int)this.openedSearches[l];
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
		global::Thoth.Write(writer, (int)this.WarbandId);
		global::Thoth.Write(writer, (int)this.CampaignWarId);
		global::Thoth.Write(writer, this.Name);
		global::Thoth.Write(writer, this.PlayerName);
		global::Thoth.Write(writer, this.Rank);
		global::Thoth.Write(writer, this.Rating);
		global::Thoth.Write(writer, this.PlayerIndex);
		global::Thoth.Write(writer, (int)this.PlayerTypeId);
		global::Thoth.Write(writer, this.Units.Count);
		for (int i = 0; i < this.Units.Count; i++)
		{
			((global::IThoth)this.Units[i]).Write(writer);
		}
		global::Thoth.Write(writer, this.openedSearches.Count);
		for (int j = 0; j < this.openedSearches.Count; j++)
		{
			global::Thoth.Write(writer, this.openedSearches[j]);
		}
	}

	private int lastVersion;

	public global::System.Collections.Generic.List<uint> openedSearches = new global::System.Collections.Generic.List<uint>();
}
