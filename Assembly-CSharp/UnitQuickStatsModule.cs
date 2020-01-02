using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitQuickStatsModule : global::UIModule
{
	public void RefreshStats(global::Unit unit, global::Unit comparedUnit = null)
	{
		this.strStat.text = unit.Strength.ToString();
		this.toughnessStat.text = unit.Toughness.ToString();
		this.agilityStat.text = unit.Agility.ToString();
		this.leadershipStat.text = unit.Leadership.ToString();
		this.intelligenceStat.text = unit.Intelligence.ToString();
		this.alertnessStat.text = unit.Alertness.ToString();
		this.weaponSkillStat.text = unit.WeaponSkill.ToString();
		this.ballisticSkillStat.text = unit.BallisticSkill.ToString();
		this.accuracyStat.text = unit.Accuracy.ToString();
		this.movementStat.text = unit.Movement.ToString();
		this.moraleImpactStat.text = unit.MoralImpact.ToString();
		this.moraleStat.text = unit.Moral.ToString();
		this.initiativeStat.text = unit.Initiative.ToString();
		this.poisonResistStat.text = unit.PoisonResistRoll.ToString();
		this.magicResistStat.text = unit.MagicResistance.ToString();
		this.rangeResistStat.text = unit.RangeResistance.ToString();
		this.meleeResistStat.text = unit.MeleeResistance.ToString();
		this.critResistStat.text = unit.CritResistance.ToString();
		this.stunResistStat.text = unit.StunResistRoll.ToString();
		this.trapResistStat.text = unit.TrapResistRoll.ToString();
		this.wyrdstoneResistStat.text = unit.WyrdstoneResistRoll.ToString();
		this.strStatDown.gameObject.SetActive(false);
		this.strStatUp.gameObject.SetActive(false);
		this.toughnessStatDown.gameObject.SetActive(false);
		this.toughnessStatUp.gameObject.SetActive(false);
		this.agilityStatDown.gameObject.SetActive(false);
		this.agilityStatUp.gameObject.SetActive(false);
		this.leadershipStatDown.gameObject.SetActive(false);
		this.leadershipStatUp.gameObject.SetActive(false);
		this.intelligenceStatDown.gameObject.SetActive(false);
		this.intelligenceStatUp.gameObject.SetActive(false);
		this.alertnessStatDown.gameObject.SetActive(false);
		this.alertnessStatUp.gameObject.SetActive(false);
		this.weaponSkillStatDown.gameObject.SetActive(false);
		this.weaponSkillStatUp.gameObject.SetActive(false);
		this.ballisticSkillStatDown.gameObject.SetActive(false);
		this.ballisticSkillStatUp.gameObject.SetActive(false);
		this.accuracyStatDown.gameObject.SetActive(false);
		this.accuracyStatUp.gameObject.SetActive(false);
		this.movementStatDown.gameObject.SetActive(false);
		this.movementStatUp.gameObject.SetActive(false);
		this.moraleImpactStatDown.gameObject.SetActive(false);
		this.moraleImpactStatUp.gameObject.SetActive(false);
		this.moraleStatDown.gameObject.SetActive(false);
		this.moraleStatUp.gameObject.SetActive(false);
		this.initiativeStatDown.gameObject.SetActive(false);
		this.initiativeStatUp.gameObject.SetActive(false);
		this.poisonResistStatDown.gameObject.SetActive(false);
		this.poisonResistStatUp.gameObject.SetActive(false);
		this.magicResistStatDown.gameObject.SetActive(false);
		this.magicResistStatUp.gameObject.SetActive(false);
		this.rangeResistStatDown.gameObject.SetActive(false);
		this.rangeResistStatUp.gameObject.SetActive(false);
		this.meleeResistStatDown.gameObject.SetActive(false);
		this.meleeResistStatUp.gameObject.SetActive(false);
		this.critResistStatDown.gameObject.SetActive(false);
		this.critResistStatUp.gameObject.SetActive(false);
		this.stunResistStatDown.gameObject.SetActive(false);
		this.stunResistStatUp.gameObject.SetActive(false);
		this.trapResistStatDown.gameObject.SetActive(false);
		this.trapResistStatUp.gameObject.SetActive(false);
		this.wyrdstoneResistStatDown.gameObject.SetActive(false);
		this.wyrdstoneResistStatUp.gameObject.SetActive(false);
		if (comparedUnit != null)
		{
			this.SetCompareIcon(unit.Strength, comparedUnit.Strength, this.strStatUp.gameObject, this.strStatDown.gameObject);
			this.SetCompareIcon(unit.Toughness, comparedUnit.Toughness, this.toughnessStatUp.gameObject, this.toughnessStatDown.gameObject);
			this.SetCompareIcon(unit.Agility, comparedUnit.Agility, this.agilityStatUp.gameObject, this.agilityStatDown.gameObject);
			this.SetCompareIcon(unit.Leadership, comparedUnit.Leadership, this.leadershipStatUp.gameObject, this.leadershipStatDown.gameObject);
			this.SetCompareIcon(unit.Intelligence, comparedUnit.Intelligence, this.intelligenceStatUp.gameObject, this.intelligenceStatDown.gameObject);
			this.SetCompareIcon(unit.Alertness, comparedUnit.Alertness, this.alertnessStatUp.gameObject, this.alertnessStatDown.gameObject);
			this.SetCompareIcon(unit.WeaponSkill, comparedUnit.WeaponSkill, this.weaponSkillStatUp.gameObject, this.weaponSkillStatDown.gameObject);
			this.SetCompareIcon(unit.BallisticSkill, comparedUnit.BallisticSkill, this.ballisticSkillStatUp.gameObject, this.ballisticSkillStatDown.gameObject);
			this.SetCompareIcon(unit.Accuracy, comparedUnit.Accuracy, this.accuracyStatUp.gameObject, this.accuracyStatDown.gameObject);
			this.SetCompareIcon(unit.Movement, comparedUnit.Movement, this.movementStatUp.gameObject, this.movementStatDown.gameObject);
			this.SetCompareIcon(unit.MoralImpact, comparedUnit.MoralImpact, this.moraleImpactStatDown.gameObject, this.moraleImpactStatUp.gameObject);
			this.SetCompareIcon(unit.Moral, comparedUnit.Moral, this.moraleStatUp.gameObject, this.moraleStatDown.gameObject);
			this.SetCompareIcon(unit.Initiative, comparedUnit.Initiative, this.initiativeStatUp.gameObject, this.initiativeStatDown.gameObject);
			this.SetCompareIcon(unit.PoisonResistRoll, comparedUnit.PoisonResistRoll, this.poisonResistStatUp.gameObject, this.poisonResistStatDown.gameObject);
			this.SetCompareIcon(unit.MagicResistance, comparedUnit.MagicResistance, this.magicResistStatUp.gameObject, this.magicResistStatDown.gameObject);
			this.SetCompareIcon(unit.RangeResistance, comparedUnit.RangeResistance, this.rangeResistStatUp.gameObject, this.rangeResistStatDown.gameObject);
			this.SetCompareIcon(unit.MeleeResistance, comparedUnit.MeleeResistance, this.meleeResistStatUp.gameObject, this.meleeResistStatDown.gameObject);
			this.SetCompareIcon(unit.CritResistance, comparedUnit.CritResistance, this.critResistStatUp.gameObject, this.critResistStatDown.gameObject);
			this.SetCompareIcon(unit.StunResistRoll, comparedUnit.StunResistRoll, this.stunResistStatUp.gameObject, this.stunResistStatDown.gameObject);
			this.SetCompareIcon(unit.TrapResistRoll, comparedUnit.TrapResistRoll, this.trapResistStatUp.gameObject, this.trapResistStatDown.gameObject);
			this.SetCompareIcon(unit.WyrdstoneResistRoll, comparedUnit.WyrdstoneResistRoll, this.wyrdstoneResistStatUp.gameObject, this.wyrdstoneResistStatDown.gameObject);
		}
		int activeWeaponSlot = (int)unit.ActiveWeaponSlot;
		this.primaryMainWeapon.Set(unit.Items[activeWeaponSlot], false, false, global::ItemId.NONE, false);
		this.primaryOffHandWeapon.Set(unit.Items[activeWeaponSlot + 1], false, false, global::ItemId.NONE, false);
		if (unit.CanSwitchWeapon())
		{
			this.secondaryWeaponGroup.gameObject.SetActive(true);
			int inactiveWeaponSlot = (int)unit.InactiveWeaponSlot;
			this.secondaryMainWeapon.Set(unit.Items[inactiveWeaponSlot], false, false, global::ItemId.NONE, false);
			this.secondaryOffHandWeapon.Set(unit.Items[inactiveWeaponSlot + 1], false, false, global::ItemId.NONE, false);
		}
		else
		{
			this.secondaryWeaponGroup.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.activeSkills.Count; i++)
		{
			if (i < unit.ActiveSkills.Count)
			{
				this.activeSkills[i].gameObject.SetActive(true);
				this.activeSkills[i].Set(unit.ActiveSkills[i], false);
			}
			else
			{
				this.activeSkills[i].gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < this.passiveSkills.Count; j++)
		{
			if (j < unit.PassiveSkills.Count)
			{
				this.passiveSkills[j].gameObject.SetActive(true);
				this.passiveSkills[j].Set(unit.PassiveSkills[j], false);
			}
			else
			{
				this.passiveSkills[j].gameObject.SetActive(false);
			}
		}
		if (unit.Mutations.Count > 0)
		{
			this.mutationGroup.gameObject.SetActive(true);
			for (int k = 0; k < this.mutations.Count; k++)
			{
				if (k < unit.Mutations.Count)
				{
					this.mutations[k].gameObject.SetActive(true);
					this.mutations[k].Set(unit.Mutations[k]);
				}
				else
				{
					this.mutations[k].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			this.mutationGroup.gameObject.SetActive(false);
		}
	}

	private void SetCompareIcon(int statValue, int comparedValue, global::UnityEngine.GameObject compareBetter, global::UnityEngine.GameObject compareWorse)
	{
		if (statValue > comparedValue)
		{
			compareBetter.SetActive(true);
		}
		else if (statValue < comparedValue)
		{
			compareWorse.SetActive(true);
		}
	}

	public global::UnityEngine.UI.Text strStat;

	public global::UnityEngine.UI.Image strStatDown;

	public global::UnityEngine.UI.Image strStatUp;

	public global::UnityEngine.UI.Text toughnessStat;

	public global::UnityEngine.UI.Image toughnessStatDown;

	public global::UnityEngine.UI.Image toughnessStatUp;

	public global::UnityEngine.UI.Text agilityStat;

	public global::UnityEngine.UI.Image agilityStatDown;

	public global::UnityEngine.UI.Image agilityStatUp;

	public global::UnityEngine.UI.Text leadershipStat;

	public global::UnityEngine.UI.Image leadershipStatDown;

	public global::UnityEngine.UI.Image leadershipStatUp;

	public global::UnityEngine.UI.Text intelligenceStat;

	public global::UnityEngine.UI.Image intelligenceStatDown;

	public global::UnityEngine.UI.Image intelligenceStatUp;

	public global::UnityEngine.UI.Text alertnessStat;

	public global::UnityEngine.UI.Image alertnessStatDown;

	public global::UnityEngine.UI.Image alertnessStatUp;

	public global::UnityEngine.UI.Text weaponSkillStat;

	public global::UnityEngine.UI.Image weaponSkillStatDown;

	public global::UnityEngine.UI.Image weaponSkillStatUp;

	public global::UnityEngine.UI.Text ballisticSkillStat;

	public global::UnityEngine.UI.Image ballisticSkillStatDown;

	public global::UnityEngine.UI.Image ballisticSkillStatUp;

	public global::UnityEngine.UI.Text accuracyStat;

	public global::UnityEngine.UI.Image accuracyStatDown;

	public global::UnityEngine.UI.Image accuracyStatUp;

	public global::UnityEngine.UI.Text movementStat;

	public global::UnityEngine.UI.Image movementStatDown;

	public global::UnityEngine.UI.Image movementStatUp;

	public global::UnityEngine.UI.Text moraleImpactStat;

	public global::UnityEngine.UI.Image moraleImpactStatDown;

	public global::UnityEngine.UI.Image moraleImpactStatUp;

	public global::UnityEngine.UI.Text moraleStat;

	public global::UnityEngine.UI.Image moraleStatDown;

	public global::UnityEngine.UI.Image moraleStatUp;

	public global::UnityEngine.UI.Text initiativeStat;

	public global::UnityEngine.UI.Image initiativeStatDown;

	public global::UnityEngine.UI.Image initiativeStatUp;

	public global::UnityEngine.UI.Text poisonResistStat;

	public global::UnityEngine.UI.Image poisonResistStatDown;

	public global::UnityEngine.UI.Image poisonResistStatUp;

	public global::UnityEngine.UI.Text magicResistStat;

	public global::UnityEngine.UI.Image magicResistStatDown;

	public global::UnityEngine.UI.Image magicResistStatUp;

	public global::UnityEngine.UI.Text rangeResistStat;

	public global::UnityEngine.UI.Image rangeResistStatDown;

	public global::UnityEngine.UI.Image rangeResistStatUp;

	public global::UnityEngine.UI.Text meleeResistStat;

	public global::UnityEngine.UI.Image meleeResistStatDown;

	public global::UnityEngine.UI.Image meleeResistStatUp;

	public global::UnityEngine.UI.Text critResistStat;

	public global::UnityEngine.UI.Image critResistStatDown;

	public global::UnityEngine.UI.Image critResistStatUp;

	public global::UnityEngine.UI.Text stunResistStat;

	public global::UnityEngine.UI.Image stunResistStatDown;

	public global::UnityEngine.UI.Image stunResistStatUp;

	public global::UnityEngine.UI.Text trapResistStat;

	public global::UnityEngine.UI.Image trapResistStatDown;

	public global::UnityEngine.UI.Image trapResistStatUp;

	public global::UnityEngine.UI.Text wyrdstoneResistStat;

	public global::UnityEngine.UI.Image wyrdstoneResistStatDown;

	public global::UnityEngine.UI.Image wyrdstoneResistStatUp;

	public global::UIInventoryItem primaryMainWeapon;

	public global::UIInventoryItem primaryOffHandWeapon;

	public global::UnityEngine.GameObject secondaryWeaponGroup;

	public global::UIInventoryItem secondaryMainWeapon;

	public global::UIInventoryItem secondaryOffHandWeapon;

	public global::UnityEngine.GameObject mutationGroup;

	public global::System.Collections.Generic.List<global::UIInventoryMutation> mutations;

	public global::System.Collections.Generic.List<global::UISkillItem> activeSkills;

	public global::System.Collections.Generic.List<global::UISkillItem> passiveSkills;
}
