using System;
using System.IO;

[global::System.Serializable]
public class ItemSave : global::IThoth
{
	public ItemSave(global::ItemId itemId, global::ItemQualityId quality = global::ItemQualityId.NORMAL, global::RuneMarkId runeMark = global::RuneMarkId.NONE, global::RuneMarkQualityId runeMarkQuality = global::RuneMarkQualityId.NONE, global::AllegianceId allegiance = global::AllegianceId.NONE, int count = 1)
	{
		this.id = (int)itemId;
		this.qualityId = (int)quality;
		this.runeMarkId = (int)runeMark;
		this.runeMarkQualityId = (int)runeMarkQuality;
		this.allegianceId = (int)allegiance;
		this.amount = ((itemId != global::ItemId.NONE) ? count : 0);
		this.soldAmount = 0;
		this.ownerMyrtilus = 0U;
		this.shots = 0;
		this.lastVersion = ((global::IThoth)this).GetVersion();
	}

	int global::IThoth.GetVersion()
	{
		return 8;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, this.id);
		global::Thoth.Write(writer, this.qualityId);
		global::Thoth.Write(writer, this.runeMarkId);
		global::Thoth.Write(writer, this.runeMarkQualityId);
		global::Thoth.Write(writer, this.allegianceId);
		global::Thoth.Write(writer, this.amount);
		global::Thoth.Write(writer, this.oldSlot);
		global::Thoth.Write(writer, this.ownerMyrtilus);
		global::Thoth.Write(writer, this.shots);
		global::Thoth.Write(writer, this.soldAmount);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 >= 4)
		{
			global::Thoth.Read(reader, out num);
		}
		global::Thoth.Read(reader, out this.id);
		global::Thoth.Read(reader, out this.qualityId);
		global::Thoth.Read(reader, out this.runeMarkId);
		if (num2 >= 2)
		{
			global::Thoth.Read(reader, out this.runeMarkQualityId);
		}
		global::Thoth.Read(reader, out this.allegianceId);
		if (num2 >= 3)
		{
			global::Thoth.Read(reader, out this.amount);
		}
		if (num2 >= 5)
		{
			global::Thoth.Read(reader, out this.oldSlot);
		}
		if (num2 >= 6)
		{
			global::Thoth.Read(reader, out this.ownerMyrtilus);
		}
		if (num2 >= 7)
		{
			global::Thoth.Read(reader, out this.shots);
		}
		if (num2 >= 8)
		{
			global::Thoth.Read(reader, out this.soldAmount);
		}
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		num2 += this.id;
		num2 += this.qualityId;
		num2 += this.runeMarkId;
		num2 += this.runeMarkQualityId;
		num2 += this.allegianceId;
		num2 += this.amount;
		if (num >= 5)
		{
			num2 += this.oldSlot;
		}
		if (num >= 6)
		{
			num2 += (int)this.ownerMyrtilus;
		}
		if (num >= 7)
		{
			num2 += this.shots;
		}
		if (num >= 8)
		{
			num2 += this.soldAmount;
		}
		return num2;
	}

	private int lastVersion;

	public int id;

	public int qualityId;

	public int runeMarkId;

	public int runeMarkQualityId;

	public int allegianceId;

	public int amount;

	public int oldSlot;

	public uint ownerMyrtilus;

	public int shots;

	public int soldAmount;
}
