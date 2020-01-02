using System;
using System.Collections.Generic;
using System.IO;

public class UnitSave : global::IThoth
{
	public UnitSave()
	{
		this.injuries = new global::System.Collections.Generic.List<global::InjuryId>();
		this.spells = new global::System.Collections.Generic.List<global::SkillId>();
		this.activeSkills = new global::System.Collections.Generic.List<global::SkillId>();
		this.passiveSkills = new global::System.Collections.Generic.List<global::SkillId>();
		this.consumableSkills = new global::System.Collections.Generic.List<global::SkillId>();
		this.mutations = new global::System.Collections.Generic.List<global::MutationId>();
		this.attributes = new global::System.Collections.Generic.Dictionary<global::AttributeId, int>(global::AttributeIdComparer.Instance);
		this.customParts = new global::System.Collections.Generic.Dictionary<global::BodyPartId, global::System.Collections.Generic.KeyValuePair<int, int>>(global::BodyPartIdComparer.Instance);
		this.items = new global::System.Collections.Generic.List<global::ItemSave>(13);
		for (int i = 0; i < 13; i++)
		{
			this.items.Add(null);
		}
		this.stats = new global::UnitStatSave();
		this.isReinforcement = false;
		this.mutationChecked = false;
	}

	public UnitSave(global::UnitId unitId) : this()
	{
		this.stats.id = (int)unitId;
		this.xp = 0;
		this.rankId = global::UnitRankId.UNIT_RANK_00;
		this.numLevelupUpdate = 0;
		this.upkeepOwned = 0;
		this.upkeepMissedDays = 0;
		this.injuredTime = 0;
		this.lastInjuryDate = 0;
		this.injuryPaid = true;
		this.skillInTrainingId = global::SkillId.NONE;
		this.skinColor = string.Empty;
		this.bio = string.Empty;
		this.trainingTime = 0;
		this.warbandSlotIndex = -1;
	}

	int global::IThoth.GetVersion()
	{
		return 19;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		global::Thoth.Write(writer, ((global::IThoth)this).GetVersion());
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, this.campaignId);
		global::Thoth.Write(writer, this.bio);
		global::Thoth.Write(writer, this.isOutsider);
		global::Thoth.Write(writer, this.isFreeOutsider);
		global::Thoth.Write(writer, this.attributes.Count);
		foreach (global::System.Collections.Generic.KeyValuePair<global::AttributeId, int> keyValuePair in this.attributes)
		{
			global::Thoth.Write(writer, (int)keyValuePair.Key);
			global::Thoth.Write(writer, keyValuePair.Value);
		}
		global::Thoth.Write(writer, this.xp);
		global::Thoth.Write(writer, (int)this.overrideTypeId);
		global::Thoth.Write(writer, (int)this.rankId);
		global::Thoth.Write(writer, this.numLevelupUpdate);
		global::Thoth.Write(writer, this.upkeepOwned);
		global::Thoth.Write(writer, this.upkeepMissedDays);
		global::Thoth.Write(writer, this.injuredTime);
		global::Thoth.Write(writer, this.lastInjuryDate);
		global::Thoth.Write(writer, this.injuryPaid);
		global::Thoth.Write(writer, (int)this.skillInTrainingId);
		global::Thoth.Write(writer, this.trainingTime);
		global::Thoth.Write(writer, this.warbandSlotIndex);
		global::Thoth.Write(writer, this.injuries.Count);
		for (int i = 0; i < this.injuries.Count; i++)
		{
			global::Thoth.Write(writer, (int)this.injuries[i]);
		}
		global::Thoth.Write(writer, this.items.Count);
		for (int j = 0; j < this.items.Count; j++)
		{
			if (this.items[j] != null)
			{
				global::Thoth.Write(writer, true);
				((global::IThoth)this.items[j]).Write(writer);
			}
			else
			{
				global::Thoth.Write(writer, false);
			}
		}
		global::Thoth.Write(writer, this.skinColor);
		global::Thoth.Write(writer, this.customParts.Count);
		foreach (global::System.Collections.Generic.KeyValuePair<global::BodyPartId, global::System.Collections.Generic.KeyValuePair<int, int>> keyValuePair2 in this.customParts)
		{
			global::Thoth.Write(writer, (int)keyValuePair2.Key);
			global::Thoth.Write(writer, keyValuePair2.Value.Key);
			global::Thoth.Write(writer, keyValuePair2.Value.Value);
		}
		global::Thoth.Write(writer, this.spells.Count);
		for (int k = 0; k < this.spells.Count; k++)
		{
			global::Thoth.Write(writer, (int)this.spells[k]);
		}
		global::Thoth.Write(writer, this.activeSkills.Count);
		for (int l = 0; l < this.activeSkills.Count; l++)
		{
			global::Thoth.Write(writer, (int)this.activeSkills[l]);
		}
		global::Thoth.Write(writer, this.passiveSkills.Count);
		for (int m = 0; m < this.passiveSkills.Count; m++)
		{
			global::Thoth.Write(writer, (int)this.passiveSkills[m]);
		}
		global::Thoth.Write(writer, this.consumableSkills.Count);
		for (int n = 0; n < this.consumableSkills.Count; n++)
		{
			global::Thoth.Write(writer, (int)this.consumableSkills[n]);
		}
		global::Thoth.Write(writer, this.mutations.Count);
		for (int num = 0; num < this.mutations.Count; num++)
		{
			global::Thoth.Write(writer, (int)this.mutations[num]);
		}
		((global::IThoth)this.stats).Write(writer);
		global::Thoth.Write(writer, this.isReinforcement);
		global::Thoth.Write(writer, this.mutationChecked);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 >= 17)
		{
			global::Thoth.Read(reader, out num);
		}
		global::Thoth.Read(reader, out this.campaignId);
		if (num2 >= 12)
		{
			global::Thoth.Read(reader, out this.bio);
		}
		if (num2 >= 13)
		{
			global::Thoth.Read(reader, out this.isOutsider);
		}
		if (num2 >= 14)
		{
			global::Thoth.Read(reader, out this.isFreeOutsider);
		}
		int num3 = 0;
		global::Thoth.Read(reader, out num3);
		for (int i = 0; i < num3; i++)
		{
			int key;
			global::Thoth.Read(reader, out key);
			int value;
			global::Thoth.Read(reader, out value);
			this.attributes.Add((global::AttributeId)key, value);
		}
		global::Thoth.Read(reader, out this.xp);
		if (num2 >= 5)
		{
			int num4 = 0;
			global::Thoth.Read(reader, out num4);
			this.overrideTypeId = (global::UnitTypeId)num4;
		}
		else
		{
			this.overrideTypeId = global::UnitTypeId.NONE;
		}
		if (num2 >= 3)
		{
			int num5 = 0;
			global::Thoth.Read(reader, out num5);
			this.rankId = (global::UnitRankId)num5;
		}
		else
		{
			this.rankId = global::UnitRankId.UNIT_RANK_00;
		}
		global::Thoth.Read(reader, out this.numLevelupUpdate);
		global::Thoth.Read(reader, out this.upkeepOwned);
		global::Thoth.Read(reader, out this.upkeepMissedDays);
		global::Thoth.Read(reader, out this.injuredTime);
		global::Thoth.Read(reader, out this.lastInjuryDate);
		global::Thoth.Read(reader, out this.injuryPaid);
		int num6 = 0;
		global::Thoth.Read(reader, out num6);
		this.skillInTrainingId = (global::SkillId)num6;
		if (num2 <= 3)
		{
			global::Thoth.Read(reader, out num6);
		}
		global::Thoth.Read(reader, out this.trainingTime);
		if (num2 < 15)
		{
			int num7;
			global::Thoth.Read(reader, out num7);
			global::Thoth.Read(reader, out num7);
		}
		if (num2 < 16)
		{
			int num8;
			global::Thoth.Read(reader, out num8);
		}
		if (num2 >= 2)
		{
			global::Thoth.Read(reader, out this.warbandSlotIndex);
		}
		global::Thoth.Read(reader, out num3);
		for (int j = 0; j < num3; j++)
		{
			int item = 0;
			global::Thoth.Read(reader, out item);
			this.injuries.Add((global::InjuryId)item);
		}
		global::Thoth.Read(reader, out num3);
		for (int k = 0; k < num3; k++)
		{
			bool flag = false;
			global::Thoth.Read(reader, out flag);
			if (flag)
			{
				global::ItemSave itemSave = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
				((global::IThoth)itemSave).Read(reader);
				this.items[k] = itemSave;
			}
		}
		global::Thoth.Read(reader, out this.skinColor);
		if (num2 >= 10 && num2 < 14 && num2 < 14)
		{
			int num9;
			global::Thoth.Read(reader, out num9);
		}
		if (num2 >= 11)
		{
			if (num2 >= 13)
			{
				global::Thoth.Read(reader, out num3);
				for (int l = 0; l < num3; l++)
				{
					int key2;
					global::Thoth.Read(reader, out key2);
					int key3;
					global::Thoth.Read(reader, out key3);
					int value2;
					global::Thoth.Read(reader, out value2);
					this.customParts.Add((global::BodyPartId)key2, new global::System.Collections.Generic.KeyValuePair<int, int>(key3, value2));
				}
			}
			else
			{
				global::Thoth.Read(reader, out num3);
				for (int m = 0; m < num3; m++)
				{
					int key4;
					global::Thoth.Read(reader, out key4);
					string text;
					global::Thoth.Read(reader, out text);
					this.customParts.Add((global::BodyPartId)key4, new global::System.Collections.Generic.KeyValuePair<int, int>(1, 1));
				}
			}
		}
		global::Thoth.Read(reader, out num3);
		for (int n = 0; n < num3; n++)
		{
			int item2;
			global::Thoth.Read(reader, out item2);
			this.spells.Add((global::SkillId)item2);
		}
		global::Thoth.Read(reader, out num3);
		for (int num10 = 0; num10 < num3; num10++)
		{
			int item3;
			global::Thoth.Read(reader, out item3);
			this.activeSkills.Add((global::SkillId)item3);
		}
		global::Thoth.Read(reader, out num3);
		for (int num11 = 0; num11 < num3; num11++)
		{
			int item4;
			global::Thoth.Read(reader, out item4);
			this.passiveSkills.Add((global::SkillId)item4);
		}
		if (num2 >= 8)
		{
			global::Thoth.Read(reader, out num3);
			for (int num12 = 0; num12 < num3; num12++)
			{
				int item5;
				global::Thoth.Read(reader, out item5);
				this.consumableSkills.Add((global::SkillId)item5);
			}
		}
		global::Thoth.Read(reader, out num3);
		for (int num13 = 0; num13 < num3; num13++)
		{
			int item6;
			global::Thoth.Read(reader, out item6);
			this.mutations.Add((global::MutationId)item6);
		}
		if (num2 > 1)
		{
			((global::IThoth)this.stats).Read(reader);
		}
		if (num2 > 17)
		{
			global::Thoth.Read(reader, out this.isReinforcement);
		}
		if (num2 > 18)
		{
			global::Thoth.Read(reader, out this.mutationChecked);
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
		char[] array = this.bio.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			num2 += (int)array[i];
		}
		num2 += this.campaignId;
		num2 += ((!this.isOutsider) ? 0 : 1);
		num2 += ((!this.isFreeOutsider) ? 0 : 1);
		foreach (global::System.Collections.Generic.KeyValuePair<global::AttributeId, int> keyValuePair in this.attributes)
		{
			num2 = (int)(num2 + (keyValuePair.Value + keyValuePair.Key));
		}
		num2 += this.xp;
		num2 = (int)(num2 + this.overrideTypeId);
		num2 = (int)(num2 + this.rankId);
		for (int j = 0; j < this.injuries.Count; j++)
		{
			num2 = (int)(num2 + this.injuries[j]);
		}
		for (int k = 0; k < this.items.Count; k++)
		{
			if (this.items[k] != null)
			{
				num2 += this.items[k].GetCRC(read);
			}
		}
		array = this.skinColor.ToCharArray();
		for (int l = 0; l < array.Length; l++)
		{
			num2 += (int)array[l];
		}
		foreach (global::System.Collections.Generic.KeyValuePair<global::BodyPartId, global::System.Collections.Generic.KeyValuePair<int, int>> keyValuePair2 in this.customParts)
		{
			num2 = (int)(num2 + (keyValuePair2.Value.Key + keyValuePair2.Value.Value + keyValuePair2.Key));
		}
		for (int m = 0; m < this.spells.Count; m++)
		{
			num2 = (int)(num2 + this.spells[m]);
		}
		for (int n = 0; n < this.activeSkills.Count; n++)
		{
			num2 = (int)(num2 + this.activeSkills[n]);
		}
		for (int num3 = 0; num3 < this.passiveSkills.Count; num3++)
		{
			num2 = (int)(num2 + this.passiveSkills[num3]);
		}
		for (int num4 = 0; num4 < this.consumableSkills.Count; num4++)
		{
			num2 = (int)(num2 + this.consumableSkills[num4]);
		}
		for (int num5 = 0; num5 < this.mutations.Count; num5++)
		{
			num2 = (int)(num2 + this.mutations[num5]);
		}
		num2 += this.numLevelupUpdate;
		num2 += this.upkeepOwned;
		num2 += this.upkeepMissedDays;
		num2 += this.injuredTime;
		num2 += this.lastInjuryDate;
		num2 += ((!this.injuryPaid) ? 0 : 1);
		num2 = (int)(num2 + this.skillInTrainingId);
		num2 += this.trainingTime;
		num2 += this.warbandSlotIndex;
		num2 += this.stats.GetCRC(read);
		if (num > 17)
		{
			num2 += ((!this.isReinforcement) ? 0 : 1);
		}
		if (num > 18)
		{
			num2 += ((!this.mutationChecked) ? 0 : 1);
		}
		return num2;
	}

	private int lastVersion;

	public string bio;

	public int campaignId;

	public bool isOutsider;

	public bool isFreeOutsider;

	public global::System.Collections.Generic.Dictionary<global::AttributeId, int> attributes;

	public int xp;

	public global::UnitTypeId overrideTypeId;

	public global::UnitRankId rankId;

	public global::System.Collections.Generic.List<global::InjuryId> injuries;

	public global::System.Collections.Generic.List<global::ItemSave> items;

	public string skinColor;

	public global::System.Collections.Generic.Dictionary<global::BodyPartId, global::System.Collections.Generic.KeyValuePair<int, int>> customParts;

	public global::System.Collections.Generic.List<global::SkillId> spells;

	public global::System.Collections.Generic.List<global::SkillId> activeSkills;

	public global::System.Collections.Generic.List<global::SkillId> passiveSkills;

	public global::System.Collections.Generic.List<global::SkillId> consumableSkills;

	public global::System.Collections.Generic.List<global::MutationId> mutations;

	public int numLevelupUpdate;

	public int upkeepOwned;

	public int upkeepMissedDays;

	public int injuredTime;

	public int lastInjuryDate;

	public bool injuryPaid;

	public global::SkillId skillInTrainingId;

	public int trainingTime;

	public int warbandSlotIndex;

	public global::UnitStatSave stats;

	public bool isReinforcement;

	public bool mutationChecked;
}
