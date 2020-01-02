using System;
using UnityEngine;

public class WarbandSkillWheelSlot : global::BaseWheelSlot<global::WarbandSkill>
{
	protected override global::UnityEngine.Sprite GetIcon()
	{
		return global::WarbandSkill.GetIcon(this.skillData);
	}

	protected override bool IsMastery()
	{
		return this.skillData != null && this.skillData.IsMastery;
	}
}
