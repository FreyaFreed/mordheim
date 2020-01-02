using System;

public class UnitTabsModule : global::TabsModule
{
	public override void Init()
	{
		base.Init();
		int num = 0;
		this.AddTabIcon(global::HideoutManager.State.UNIT_INFO, num++, null, "hideout_description", new global::TabsModule.IsAvailable(this.IsUnitInfoAvailable));
		this.AddTabIcon(global::HideoutManager.State.INVENTORY, num++, null, "hideout_title_inventory", new global::TabsModule.IsAvailable(this.IsInventoryAvailable));
		this.AddTabIcon(global::HideoutManager.State.SKILLS, num++, null, "hideout_menu_unit_skills", new global::TabsModule.IsAvailable(this.IsSkillsAvailable));
		this.AddTabIcon(global::HideoutManager.State.SPELLS, num++, null, "hideout_menu_unit_spells", new global::TabsModule.IsAvailable(this.IsSpellsAvailable));
		this.AddTabIcon(global::HideoutManager.State.CUSTOMIZATION, num++, null, "hideout_submenu_customization", new global::TabsModule.IsAvailable(this.IsCustomizationAvailable));
	}

	public bool IsUnitInfoAvailable(out string reason)
	{
		reason = string.Empty;
		return true;
	}

	public bool IsInventoryAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			reason = "na_hideout_training_outsider";
			return false;
		}
		switch (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.GetActiveStatus())
		{
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			reason = "na_hideout_upkeep_not_paid";
			return false;
		}
		return true;
	}

	public bool IsSkillsAvailable(out string reason)
	{
		reason = string.Empty;
		switch (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.GetActiveStatus())
		{
		case global::UnitActiveStatusId.INJURED:
			reason = "na_hideout_unit_injured";
			return false;
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			reason = "na_hideout_upkeep_not_paid";
			return false;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			reason = "na_hideout_treatment_not_paid";
			return false;
		}
		return true;
	}

	public bool IsSpellsAvailable(out string reason)
	{
		reason = string.Empty;
		switch (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.GetActiveStatus())
		{
		case global::UnitActiveStatusId.INJURED:
			reason = "na_hideout_unit_injured";
			return false;
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			reason = "na_hideout_upkeep_not_paid";
			return false;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			reason = "na_hideout_treatment_not_paid";
			return false;
		}
		if (!this.skillShop.UnitHasSkillLine(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit, global::SkillLineId.SPELL))
		{
			reason = "na_hideout_spell";
			return false;
		}
		return true;
	}

	public bool IsCustomizationAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			reason = "na_hideout_training_outsider";
			return false;
		}
		switch (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.GetActiveStatus())
		{
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			reason = "na_hideout_upkeep_not_paid";
			return false;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			reason = "na_hideout_treatment_not_paid";
			return false;
		}
		return true;
	}

	private readonly global::SkillsShop skillShop = new global::SkillsShop();
}
