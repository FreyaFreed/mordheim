using System;
using UnityEngine;

public class SkillWheelSlot : global::BaseWheelSlot<global::SkillData>
{
	protected override global::UnityEngine.Sprite GetIcon()
	{
		if (this.skillData != null)
		{
			return global::SkillHelper.GetIcon(this.skillData);
		}
		return null;
	}

	protected override bool IsMastery()
	{
		return this.skillData != null && global::SkillHelper.IsMastery(this.skillData);
	}
}
