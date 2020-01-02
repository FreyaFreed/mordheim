using System;
using System.Collections.Generic;
using System.IO;

public class FactionSave : global::IThoth
{
	public FactionSave()
	{
		this.factionId = global::FactionId.NONE;
		this.reputation = 0;
		this.rank = 0;
		this.factionIndex = 0;
		this.shipments = new global::System.Collections.Generic.List<global::ShipmentSave>();
	}

	public FactionSave(global::FactionId id, int index)
	{
		this.factionId = id;
		this.reputation = 0;
		this.rank = 0;
		this.factionIndex = index;
		this.shipments = new global::System.Collections.Generic.List<global::ShipmentSave>();
	}

	int global::IThoth.GetVersion()
	{
		return 5;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		int num = (int)global::PandoraSingleton<global::Hephaestus>.Instance.GetUserId();
		global::Thoth.Write(writer, crc + num);
		global::Thoth.Write(writer, (int)this.factionId);
		global::Thoth.Write(writer, this.reputation);
		global::Thoth.Write(writer, this.rank);
		global::Thoth.Write(writer, this.factionIndex);
		global::Thoth.Write(writer, this.shipments.Count);
		for (int i = 0; i < this.shipments.Count; i++)
		{
			global::Thoth.Write(writer, this.shipments[i].weight);
			global::Thoth.Write(writer, this.shipments[i].gold);
			global::Thoth.Write(writer, this.shipments[i].rank);
			global::Thoth.Write(writer, this.shipments[i].sendDate);
			global::Thoth.Write(writer, this.shipments[i].guid);
		}
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 > 4)
		{
			global::Thoth.Read(reader, out num);
		}
		int num3 = 0;
		global::Thoth.Read(reader, out num3);
		this.factionId = (global::FactionId)num3;
		global::Thoth.Read(reader, out this.reputation);
		global::Thoth.Read(reader, out this.rank);
		global::Thoth.Read(reader, out this.factionIndex);
		if (num2 > 2)
		{
			int num4;
			global::Thoth.Read(reader, out num4);
			for (int i = 0; i < num4; i++)
			{
				global::ShipmentSave item;
				global::Thoth.Read(reader, out item.weight);
				global::Thoth.Read(reader, out item.gold);
				global::Thoth.Read(reader, out item.rank);
				item.sendDate = 0;
				item.guid = 0;
				if (num2 > 3)
				{
					global::Thoth.Read(reader, out item.sendDate);
					global::Thoth.Read(reader, out item.guid);
				}
				this.shipments.Add(item);
			}
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
		num2 = (int)(num2 + this.factionId);
		num2 += this.reputation;
		num2 += this.rank;
		num2 += this.factionIndex;
		for (int i = 0; i < this.shipments.Count; i++)
		{
			num2 += this.shipments[i].weight;
			num2 += this.shipments[i].gold;
			num2 += this.shipments[i].rank;
			num2 += this.shipments[i].sendDate;
			num2 += this.shipments[i].guid;
		}
		return num2;
	}

	private int lastVersion;

	public global::FactionId factionId;

	public int reputation;

	public int rank;

	public int factionIndex;

	public global::System.Collections.Generic.List<global::ShipmentSave> shipments;
}
