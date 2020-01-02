using System;
using System.Collections.Generic;
using System.IO;

public class WarbandStatSave : global::IThoth
{
	public WarbandStatSave()
	{
		this.stats = new int[63];
		this.history = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
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
		global::Thoth.Write(writer, this.stats.Length);
		for (int i = 0; i < this.stats.Length; i++)
		{
			if (this.stats[i] != 0)
			{
				global::Thoth.Write(writer, true);
				global::Thoth.Write(writer, this.stats[i]);
			}
			else
			{
				global::Thoth.Write(writer, false);
			}
		}
		global::Thoth.Write(writer, this.history.Count);
		for (int j = 0; j < this.history.Count; j++)
		{
			global::Thoth.Write(writer, this.history[j].Item1);
			global::Thoth.Write(writer, (int)this.history[j].Item2);
			global::Thoth.Write(writer, this.history[j].Item3);
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
		if (num2 < 3)
		{
			int num3;
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
		}
		else
		{
			int num4;
			global::Thoth.Read(reader, out num4);
			for (int i = 0; i < num4; i++)
			{
				bool flag;
				global::Thoth.Read(reader, out flag);
				if (flag)
				{
					int num5;
					global::Thoth.Read(reader, out num5);
					this.stats[i] = num5;
				}
			}
		}
		if (num2 >= 2)
		{
			int num6 = 0;
			global::Thoth.Read(reader, out num6);
			for (int j = 0; j < num6; j++)
			{
				int item = 0;
				int item2 = 0;
				int item3 = 0;
				global::Thoth.Read(reader, out item);
				global::Thoth.Read(reader, out item2);
				global::Thoth.Read(reader, out item3);
				this.history.Add(new global::Tuple<int, global::EventLogger.LogEvent, int>(item, (global::EventLogger.LogEvent)item2, item3));
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
		for (int i = 0; i < this.stats.Length; i++)
		{
			num2 += this.stats[i];
		}
		for (int j = 0; j < this.history.Count; j++)
		{
			num2 = (int)(num2 + (this.history[j].Item1 + this.history[j].Item2 + this.history[j].Item3));
		}
		return num2;
	}

	private int lastVersion;

	public int[] stats;

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> history;
}
