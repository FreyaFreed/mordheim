using System;
using System.Collections.Generic;

public class AchievementEquipItem : global::Achievement
{
	public AchievementEquipItem(global::AchievementData data) : base(data)
	{
		this.achievementEquipItemQualityDatas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AchievementEquipItemQualityData>("fk_achievement_id", data.Id.ToIntString<global::AchievementId>());
	}

	public override bool CheckEquipUnit(global::Unit unit, global::UnitSlotId slotId)
	{
		if (slotId < global::UnitSlotId.ITEM_1)
		{
			for (int i = 0; i < this.achievementEquipItemQualityDatas.Count; i++)
			{
				if (base.IsOfUnitType(unit, this.achievementEquipItemQualityDatas[i].UnitTypeId))
				{
					bool flag = true;
					global::UnitSlotId unitSlotId = global::UnitSlotId.HELMET;
					while (unitSlotId < global::UnitSlotId.ITEM_1 && flag)
					{
						if (unitSlotId != global::UnitSlotId.SET1_OFFHAND && unitSlotId != global::UnitSlotId.SET2_OFFHAND)
						{
							goto IL_6C;
						}
						global::Item item = unit.Items[unitSlotId - global::UnitSlotId.ARMOR];
						if (!item.IsPaired && !item.IsTwoHanded)
						{
							goto IL_6C;
						}
						IL_131:
						unitSlotId++;
						continue;
						IL_6C:
						global::Item item2 = unit.Items[(int)unitSlotId];
						if (this.achievementEquipItemQualityDatas[i].ItemQualityId != global::ItemQualityId.NONE)
						{
							if (item2 == null || item2.Id == global::ItemId.NONE || this.achievementEquipItemQualityDatas[i].ItemQualityId != item2.QualityData.Id)
							{
								flag = false;
							}
							goto IL_131;
						}
						if (this.achievementEquipItemQualityDatas[i].RuneMarkQualityId != global::RuneMarkQualityId.NONE)
						{
							if (item2 == null || item2.Id == global::ItemId.NONE || item2.RuneMark == null || this.achievementEquipItemQualityDatas[i].RuneMarkQualityId != item2.RuneMark.QualityData.Id)
							{
								flag = false;
							}
							goto IL_131;
						}
						flag = false;
						goto IL_131;
					}
					if (flag)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private readonly global::System.Collections.Generic.List<global::AchievementEquipItemQualityData> achievementEquipItemQualityDatas;
}
