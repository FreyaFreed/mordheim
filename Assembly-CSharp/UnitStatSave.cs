using System;
using System.Collections.Generic;
using System.IO;

public class UnitStatSave : global::IThoth
{
	public UnitStatSave()
	{
		this.name = string.Empty;
		this.overrideName = string.Empty;
		this.id = 0;
		this.stats = new global::System.Collections.Generic.Dictionary<int, int>();
		this.history = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
	}

	int global::IThoth.GetVersion()
	{
		return 11;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		global::Thoth.Write(writer, ((global::IThoth)this).GetVersion());
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, this.name);
		global::Thoth.Write(writer, this.id);
		global::Thoth.Write(writer, this.stats.Count);
		foreach (global::System.Collections.Generic.KeyValuePair<int, int> keyValuePair in this.stats)
		{
			global::Thoth.Write(writer, keyValuePair.Key);
			global::Thoth.Write(writer, keyValuePair.Value);
		}
		global::Thoth.Write(writer, this.history.Count);
		for (int i = 0; i < this.history.Count; i++)
		{
			global::Thoth.Write(writer, this.history[i].Item1);
			global::Thoth.Write(writer, (int)this.history[i].Item2);
			global::Thoth.Write(writer, this.history[i].Item3);
		}
		global::Thoth.Write(writer, this.overrideName);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 > 8)
		{
			global::Thoth.Read(reader, out num);
		}
		if (num2 > 7)
		{
			global::Thoth.Read(reader, out this.name);
			global::Thoth.Read(reader, out this.id);
		}
		if (num2 < 5)
		{
			int num3;
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			global::Thoth.Read(reader, out num3);
			if (num2 >= 3)
			{
				global::Thoth.Read(reader, out num3);
				global::Thoth.Read(reader, out num3);
			}
		}
		else if (num2 < 7)
		{
			int num4;
			global::Thoth.Read(reader, out num4);
			for (int i = 0; i < num4; i++)
			{
				bool flag;
				global::Thoth.Read(reader, out flag);
				if (flag)
				{
					int value;
					global::Thoth.Read(reader, out value);
					this.stats[i] = value;
				}
			}
		}
		else
		{
			int num4;
			global::Thoth.Read(reader, out num4);
			for (int j = 0; j < num4; j++)
			{
				int key;
				global::Thoth.Read(reader, out key);
				int value2;
				global::Thoth.Read(reader, out value2);
				this.stats[key] = value2;
			}
		}
		if (num2 >= 4)
		{
			int num4;
			global::Thoth.Read(reader, out num4);
			for (int k = 0; k < num4; k++)
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
		if (num2 > 10)
		{
			global::Thoth.Read(reader, out this.overrideName);
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
		char[] array = this.name.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			num2 += (int)array[i];
		}
		num2 += this.id;
		foreach (global::System.Collections.Generic.KeyValuePair<int, int> keyValuePair in this.stats)
		{
			num2 += keyValuePair.Value + keyValuePair.Key;
		}
		for (int j = 0; j < this.history.Count; j++)
		{
			num2 = (int)(num2 + (this.history[j].Item1 + this.history[j].Item2 + this.history[j].Item3));
		}
		if (num > 10 && !string.IsNullOrEmpty(this.overrideName))
		{
			array = this.overrideName.ToCharArray();
			for (int k = 0; k < array.Length; k++)
			{
				num2 += (int)array[k];
			}
		}
		return num2;
	}

	private int lastVersion;

	public string name;

	public string overrideName;

	public int id;

	public global::System.Collections.Generic.Dictionary<int, int> stats;

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> history;
}
