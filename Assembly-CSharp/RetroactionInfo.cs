using System;
using System.Collections.Generic;

public class RetroactionInfo
{
	public global::UIRetroactionTarget RetroactionTarget
	{
		get
		{
			return (!global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.IsPlayed()) ? this.enemyUnitAction : this.playerUnitAction;
		}
	}

	public void AddOutcome(string actionEffect, bool success, string damageEffect)
	{
		this.damageOrEffect = damageEffect;
		this.effect = actionEffect;
	}

	public void AddDamage(int damage, bool critical)
	{
		this.hasDamage = true;
		this.totalDamage += damage;
		this.damageOrEffect = (-this.totalDamage).ToConstantString();
		this.isCritical = critical;
	}

	public void AddEnchant(string enchantmentId, global::EffectTypeId effectTypeId, string effect)
	{
		bool flag = false;
		for (int i = 0; i < this.enchants.Count; i++)
		{
			if (this.enchants[i].Item1 == enchantmentId && this.enchants[i].Item2 == effectTypeId && this.enchants[i].Item3 == effect)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.enchants.Add(new global::Tuple<string, global::EffectTypeId, string>(enchantmentId, effectTypeId, effect));
		}
	}

	public void Reset()
	{
		this.unitCtrlr = null;
		this.destructible = null;
		this.hasDamage = false;
		this.damageShown = false;
		this.damageOrEffect = null;
		this.isCritical = false;
		this.enchantShown = false;
		this.effect = null;
		this.playerUnitAction.Hide();
		this.enemyUnitAction.Hide();
		this.enchants.Clear();
		this.startingWound = 0;
		this.totalDamage = 0;
	}

	public void SetTarget(global::UnitController unitController)
	{
		this.unitCtrlr = unitController;
		this.startingWound = this.unitCtrlr.unit.CurrentWound;
		this.destructible = null;
	}

	public void SetTarget(global::Destructible dest)
	{
		this.unitCtrlr = null;
		this.destructible = dest;
		this.startingWound = this.destructible.CurrentWounds;
	}

	public void ShowTarget()
	{
		if (this.unitCtrlr != null)
		{
			this.RetroactionTarget.SetTarget(this.unitCtrlr);
		}
		else if (this.destructible != null)
		{
			this.RetroactionTarget.SetTarget(this.destructible);
		}
	}

	public void ShowEnchant()
	{
		if (!this.enchantShown)
		{
			this.enchantShown = true;
			for (int i = 0; i < this.enchants.Count; i++)
			{
				this.RetroactionTarget.AddEnchant(this.enchants[i].Item1, this.enchants[i].Item2);
			}
		}
	}

	public void ShowOutcome()
	{
		this.RetroactionTarget.SetOutcome(this.effect, this.damageOrEffect);
		if (this.hasDamage && !this.damageShown)
		{
			this.damageShown = true;
			if (this.destructible != null)
			{
				this.RetroactionTarget.SetDamage(this.destructible, this.startingWound, this.damageOrEffect, this.isCritical);
			}
			else if (this.unitCtrlr != null)
			{
				this.RetroactionTarget.SetDamage(this.unitCtrlr, this.startingWound, this.damageOrEffect, this.isCritical);
			}
		}
		this.RetroactionTarget.ShowOutcome();
	}

	public void ShowStatus(string status)
	{
		this.RetroactionTarget.SetStatus(status);
	}

	public global::UnitController unitCtrlr;

	public global::Destructible destructible;

	public global::UIRetroactionTarget playerUnitAction;

	public global::UIRetroactionTarget enemyUnitAction;

	private bool hasDamage;

	private bool damageShown;

	private string damageOrEffect;

	private bool isCritical;

	private string effect;

	private int startingWound;

	private int totalDamage;

	public global::System.Collections.Generic.List<global::Tuple<string, global::EffectTypeId, string>> enchants = new global::System.Collections.Generic.List<global::Tuple<string, global::EffectTypeId, string>>(8);

	private bool enchantShown;
}
