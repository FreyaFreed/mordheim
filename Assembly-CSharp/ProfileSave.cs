using System;
using System.Collections.Generic;
using System.IO;

public class ProfileSave : global::IThoth
{
	public ProfileSave()
	{
		this.unspentPoint = 0;
		this.xp = 0;
		this.rankId = 0;
		this.lastPlayedCampaign = -1;
		this.completedTutorials = new bool[global::Constant.GetInt(global::ConstantId.TUTORIALS_COUNT)];
		for (int i = 0; i < this.completedTutorials.Length; i++)
		{
			this.completedTutorials[i] = false;
		}
		for (int j = 0; j < this.warProgress.Length; j++)
		{
			this.warProgress[j] = 0;
		}
		this.warbandSaves = new global::System.Collections.Generic.Dictionary<int, int>();
		this.xpChecked = false;
	}

	int global::IThoth.GetVersion()
	{
		return 7;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		int num = (int)global::PandoraSingleton<global::Hephaestus>.Instance.GetUserId();
		global::Thoth.Write(writer, crc + num);
		global::Thoth.Write(writer, this.unspentPoint);
		global::Thoth.Write(writer, this.xp);
		global::Thoth.Write(writer, this.rankId);
		global::Thoth.Write(writer, this.lastPlayedCampaign);
		global::Thoth.Write(writer, this.stats.Length);
		for (int i = 0; i < this.stats.Length; i++)
		{
			global::Thoth.Write(writer, this.stats[i]);
		}
		global::Thoth.Write(writer, this.unlockedAchievements.Length);
		for (int j = 0; j < this.unlockedAchievements.Length; j++)
		{
			global::Thoth.Write(writer, this.unlockedAchievements[j]);
		}
		global::Thoth.Write(writer, this.completedTutorials.Length);
		for (int k = 0; k < this.completedTutorials.Length; k++)
		{
			global::Thoth.Write(writer, this.completedTutorials[k]);
		}
		global::Thoth.Write(writer, this.warbandSaves.Count);
		foreach (global::System.Collections.Generic.KeyValuePair<int, int> keyValuePair in this.warbandSaves)
		{
			global::Thoth.Write(writer, keyValuePair.Key);
			global::Thoth.Write(writer, keyValuePair.Value);
		}
		global::Thoth.Write(writer, this.warProgress.Length);
		for (int l = 0; l < this.warProgress.Length; l++)
		{
			global::Thoth.Write(writer, this.warProgress[l]);
		}
		global::Thoth.Write(writer, this.xpChecked);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 > 3)
		{
			global::Thoth.Read(reader, out num);
		}
		global::Thoth.Read(reader, out this.unspentPoint);
		global::Thoth.Read(reader, out this.xp);
		global::Thoth.Read(reader, out this.rankId);
		if (num2 > 2)
		{
			global::Thoth.Read(reader, out this.lastPlayedCampaign);
		}
		int num3;
		global::Thoth.Read(reader, out num3);
		for (int i = 0; i < num3; i++)
		{
			int num4;
			global::Thoth.Read(reader, out num4);
			this.stats[i] = num4;
		}
		global::Thoth.Read(reader, out num3);
		for (int j = 0; j < num3; j++)
		{
			bool flag;
			global::Thoth.Read(reader, out flag);
			this.unlockedAchievements[j] = flag;
		}
		if (num2 > 1)
		{
			global::Thoth.Read(reader, out num3);
			for (int k = 0; k < num3; k++)
			{
				bool flag2;
				global::Thoth.Read(reader, out flag2);
				this.completedTutorials[k] = flag2;
			}
		}
		if (num2 > 4)
		{
			global::Thoth.Read(reader, out num3);
			for (int l = 0; l < num3; l++)
			{
				int key;
				global::Thoth.Read(reader, out key);
				int value;
				global::Thoth.Read(reader, out value);
				this.warbandSaves[key] = value;
			}
		}
		if (num2 > 5)
		{
			global::Thoth.Read(reader, out num3);
			for (int m = 0; m < num3; m++)
			{
				int num5;
				global::Thoth.Read(reader, out num5);
				this.warProgress[m] = num5;
			}
		}
		if (num2 > 6)
		{
			global::Thoth.Read(reader, out this.xpChecked);
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
		num2 += this.unspentPoint;
		num2 += this.xp;
		num2 += this.rankId;
		num2 += this.lastPlayedCampaign;
		for (int i = 0; i < this.stats.Length; i++)
		{
			num2 += this.stats[i];
		}
		for (int j = 0; j < this.unlockedAchievements.Length; j++)
		{
			num2 += ((!this.unlockedAchievements[j]) ? 0 : 1);
		}
		for (int k = 0; k < this.completedTutorials.Length; k++)
		{
			num2 += ((!this.completedTutorials[k]) ? 0 : 1);
		}
		foreach (global::System.Collections.Generic.KeyValuePair<int, int> keyValuePair in this.warbandSaves)
		{
			num2 += keyValuePair.Value + keyValuePair.Key;
		}
		if (num > 6)
		{
			num2 += ((!this.xpChecked) ? 0 : 1);
		}
		return num2;
	}

	private int lastVersion;

	public int unspentPoint;

	public int xp;

	public int rankId;

	public int lastPlayedCampaign;

	public int[] stats = new int[63];

	public bool[] unlockedAchievements = new bool[227];

	public bool[] completedTutorials;

	public int[] warProgress = new int[6];

	public global::System.Collections.Generic.Dictionary<int, int> warbandSaves;

	public bool xpChecked;
}
