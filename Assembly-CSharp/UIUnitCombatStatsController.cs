using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitCombatStatsController : global::UIUnitControllerChanged
{
	public bool UpdateAction { get; set; }

	protected virtual void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_ATTRIBUTES_CHANGED, new global::DelReceiveNotice(this.OnAttributesChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_ACTION_CHANGED, new global::DelReceiveNotice(this.OnActionChanged));
	}

	private void OnActionChanged()
	{
		global::UnitController y = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController;
		if (base.CurrentUnitController != null && base.CurrentUnitController == y && base.CurrentUnitController.IsPlayed())
		{
			global::ActionStatus actionStatus = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1] as global::ActionStatus;
			if (actionStatus != null)
			{
				this.damageBar.value = (float)(base.CurrentUnitController.unit.CurrentWound - actionStatus.skillData.WoundsCostMin);
			}
			else
			{
				this.damageBar.value = (float)base.CurrentUnitController.unit.CurrentWound;
			}
		}
	}

	private void OnAttributesChanged()
	{
		global::Unit unit = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::Unit;
		if (base.CurrentUnitController != null && base.CurrentUnitController.unit == unit)
		{
			base.UpdateUnit = true;
		}
		else
		{
			global::UnitController y = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController;
			if (base.CurrentUnitController != null && base.CurrentUnitController == y)
			{
				base.UpdateUnit = true;
			}
		}
	}

	public float GetAverageDamage()
	{
		if (base.TargetUnitController != null && base.TargetUnitController.CurrentAction != null)
		{
			return (float)(base.TargetUnitController.CurrentAction.GetMinDamage(false) + base.TargetUnitController.CurrentAction.GetMinDamage(false)) / 2f;
		}
		return 0f;
	}

	public void AttributeChangedDestructible()
	{
		global::Destructible targetDestructible = base.TargetDestructible;
		this.nameText.text = targetDestructible.LocalizedName;
		this.icon.sprite = targetDestructible.imprintIcon;
		this.iconStar.enabled = false;
		this.armor.text = "0";
		this.damage.text = "0-0";
		this.critChance.text = "0%";
		this.nbBuffs.text = "0";
		this.nbDebuffs.text = "0";
		this.wounds.text = string.Format("{0}/{1}", targetDestructible.CurrentWounds, targetDestructible.Data.Wounds);
		this.hpBar.maxValue = (float)targetDestructible.Data.Wounds;
		this.hpBar.minValue = 0f;
		this.hpBar.value = global::UnityEngine.Mathf.Max(0f, (float)targetDestructible.CurrentWounds - this.GetAverageDamage());
		this.damageBar.maxValue = (float)targetDestructible.Data.Wounds;
		this.damageBar.minValue = 0f;
		this.damageBar.value = (float)global::UnityEngine.Mathf.Max(0, targetDestructible.CurrentWounds);
		for (int i = 0; i < this.offensePoints.Count; i++)
		{
			this.offensePoints[i].enabled = false;
		}
		for (int j = 0; j < this.strategyPoints.Count; j++)
		{
			this.strategyPoints[j].enabled = false;
		}
	}

	public void AttributesChanged()
	{
		global::Unit unit = base.CurrentUnitController.unit;
		this.nameText.text = unit.Name;
		this.icon.sprite = unit.GetIcon();
		switch (unit.GetUnitTypeId())
		{
		case global::UnitTypeId.LEADER:
			this.iconStar.enabled = true;
			this.iconStar.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
			goto IL_F3;
		case global::UnitTypeId.IMPRESSIVE:
			this.iconStar.enabled = true;
			this.iconStar.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
			goto IL_F3;
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			this.iconStar.enabled = true;
			this.iconStar.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
			goto IL_F3;
		}
		this.iconStar.enabled = false;
		IL_F3:
		this.armor.text = global::UnityEngine.Mathf.Clamp(unit.ArmorAbsorptionPerc, 0, global::Constant.GetInt(global::ConstantId.MAX_ROLL)).ToConstantPercString();
		this.damage.text = global::PandoraUtils.StringBuilder.Append(unit.GetWeaponDamageMin(null, false, false, false).ToConstantString()).Append('-').Append(unit.GetWeaponDamageMax(null, false, false, false).ToConstantString()).ToString();
		this.critChance.text = global::UnityEngine.Mathf.Clamp((!unit.HasRange()) ? unit.CriticalMeleeAttemptRoll : unit.CriticalRangeAttemptRoll, 0, global::Constant.GetInt(global::ConstantId.MAX_ROLL)).ToConstantPercString();
		this.nbBuffs.text = base.CurrentUnitController.GetEffectTypeCount(global::EffectTypeId.BUFF).ToConstantString();
		this.nbDebuffs.text = base.CurrentUnitController.GetEffectTypeCount(global::EffectTypeId.DEBUFF).ToConstantString();
		this.wounds.text = global::PandoraUtils.StringBuilder.Append(unit.CurrentWound.ToConstantString()).Append('/').Append(unit.Wound.ToConstantString()).ToString();
		this.hpBar.maxValue = (float)unit.Wound;
		this.hpBar.minValue = 0f;
		this.hpBar.value = global::UnityEngine.Mathf.Max(0f, (float)unit.CurrentWound - this.GetAverageDamage());
		this.damageBar.maxValue = (float)unit.Wound;
		this.damageBar.minValue = 0f;
		this.damageBar.value = (float)global::UnityEngine.Mathf.Max(0, unit.CurrentWound);
		for (int i = 0; i < this.offensePoints.Count; i++)
		{
			if (i < unit.OffensePoints)
			{
				this.offensePoints[i].enabled = true;
				if (i >= unit.CurrentOffensePoints - unit.tempOffensePoints)
				{
					this.offensePoints[i].overrideSprite = this.offensePointConsumed;
				}
				else if (base.CurrentUnitController.CurrentAction != null && i >= unit.CurrentOffensePoints - unit.tempOffensePoints - base.CurrentUnitController.CurrentAction.OffensePoints)
				{
					this.offensePoints[i].overrideSprite = this.offensePointPreview;
				}
				else
				{
					this.offensePoints[i].overrideSprite = null;
				}
			}
			else
			{
				this.offensePoints[i].enabled = false;
			}
		}
		for (int j = 0; j < this.strategyPoints.Count; j++)
		{
			if (j < unit.StrategyPoints)
			{
				this.strategyPoints[j].enabled = true;
				if (j >= unit.CurrentStrategyPoints - unit.tempStrategyPoints)
				{
					this.strategyPoints[j].overrideSprite = this.strategyPointConsumed;
				}
				else if (base.CurrentUnitController.CurrentAction != null && j >= unit.CurrentStrategyPoints - unit.tempStrategyPoints - base.CurrentUnitController.CurrentAction.StrategyPoints)
				{
					this.strategyPoints[j].overrideSprite = this.strategyPointPreview;
				}
				else
				{
					this.strategyPoints[j].overrideSprite = null;
				}
			}
			else
			{
				this.strategyPoints[j].enabled = false;
			}
		}
	}

	protected override void OnUnitChanged()
	{
		if (base.TargetDestructible != null)
		{
			this.AttributeChangedDestructible();
		}
		else if (base.CurrentUnitController != null)
		{
			this.AttributesChanged();
		}
	}

	public global::UnityEngine.UI.Text nameText;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image iconStar;

	public global::UnityEngine.UI.Text armor;

	public global::UnityEngine.UI.Text damage;

	public global::UnityEngine.UI.Text critChance;

	public global::UnityEngine.UI.Text nbBuffs;

	public global::UnityEngine.UI.Text nbDebuffs;

	public global::UnityEngine.UI.Text wounds;

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Image> offensePoints;

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Image> strategyPoints;

	public global::UnityEngine.Sprite offensePointPreview;

	public global::UnityEngine.Sprite offensePointConsumed;

	public global::UnityEngine.Sprite strategyPointPreview;

	public global::UnityEngine.Sprite strategyPointConsumed;

	public global::UnityEngine.UI.Slider hpBar;

	public global::UnityEngine.UI.Slider damageBar;

	private bool update;
}
