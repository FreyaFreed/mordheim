using System;
using System.Collections.Generic;

public class UnitRating
{
	public void UpdateBaseAttributes()
	{
		this.baseAttributes[0] = this.unit.GetBaseAttribute(global::AttributeId.STRENGTH);
		this.baseAttributes[1] = this.unit.GetBaseAttribute(global::AttributeId.TOUGHNESS);
		this.baseAttributes[2] = this.unit.GetBaseAttribute(global::AttributeId.AGILITY);
		this.baseAttributes[3] = this.unit.GetBaseAttribute(global::AttributeId.LEADERSHIP);
		this.baseAttributes[4] = this.unit.GetBaseAttribute(global::AttributeId.INTELLIGENCE);
		this.baseAttributes[5] = this.unit.GetBaseAttribute(global::AttributeId.ALERTNESS);
		this.baseAttributes[6] = this.unit.GetBaseAttribute(global::AttributeId.WEAPON_SKILL);
		this.baseAttributes[7] = this.unit.GetBaseAttribute(global::AttributeId.BALLISTIC_SKILL);
		this.baseAttributes[8] = this.unit.GetBaseAttribute(global::AttributeId.ACCURACY);
	}

	public void UpdateMaxAttributes()
	{
		this.maxAttributes[0] = this.unit.GetBaseAttribute(global::AttributeId.STRENGTH_MAX);
		this.maxAttributes[1] = this.unit.GetBaseAttribute(global::AttributeId.TOUGHNESS_MAX);
		this.maxAttributes[2] = this.unit.GetBaseAttribute(global::AttributeId.AGILITY_MAX);
		this.maxAttributes[3] = this.unit.GetBaseAttribute(global::AttributeId.LEADERSHIP_MAX);
		this.maxAttributes[4] = this.unit.GetBaseAttribute(global::AttributeId.INTELLIGENCE_MAX);
		this.maxAttributes[5] = this.unit.GetBaseAttribute(global::AttributeId.ALERTNESS_MAX);
		this.maxAttributes[6] = this.unit.GetBaseAttribute(global::AttributeId.WEAPON_SKILL_MAX);
		this.maxAttributes[7] = this.unit.GetBaseAttribute(global::AttributeId.BALLISTIC_SKILL_MAX);
		this.maxAttributes[8] = this.unit.GetBaseAttribute(global::AttributeId.ACCURACY_MAX);
	}

	public global::Unit unit;

	public int rating;

	public global::System.Collections.Generic.List<global::SkillData> skillsData;

	public int[] baseAttributes = new int[9];

	public int[] maxAttributes = new int[9];
}
