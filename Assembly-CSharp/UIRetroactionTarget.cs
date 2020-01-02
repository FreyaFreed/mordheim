using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIRetroactionTarget : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.unitName.text = string.Empty;
		this.damageOrResult.text = string.Empty;
		this.newStatus.text = string.Empty;
	}

	public void AddEnchant(string name, global::EffectTypeId effectTypeId)
	{
		for (int i = 0; i < this.enchants.Count; i++)
		{
			if (string.IsNullOrEmpty(this.enchants[i].resultName.text))
			{
				if (effectTypeId != global::EffectTypeId.BUFF && effectTypeId != global::EffectTypeId.DEBUFF)
				{
					this.enchants[i].Set(name);
				}
				else
				{
					this.enchants[i].Set(name, effectTypeId == global::EffectTypeId.BUFF);
				}
				break;
			}
		}
	}

	public void Hide()
	{
		this.unitName.text = string.Empty;
		this.damageOrResult.text = string.Empty;
		this.newStatus.text = string.Empty;
		base.gameObject.SetActive(false);
		this.life.gameObject.SetActive(false);
		this.unitIcon.enabled = true;
		this.check.Hide();
		for (int i = 0; i < this.enchants.Count; i++)
		{
			this.enchants[i].Hide();
		}
		global::DG.Tweening.DOTween.Kill(this, false);
	}

	public void SetDamage(global::Destructible dest, int startingWound, string damageText, bool critical)
	{
		this.damageOrResult.enabled = false;
		if (critical)
		{
			this.damageOrResult.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_critical", new string[]
			{
				damageText
			});
		}
		else
		{
			this.damageOrResult.text = damageText;
		}
		this.life.gameObject.SetActive(true);
		this.life.minValue = 0f;
		this.life.maxValue = (float)dest.Data.Wounds;
		this.life.value = (float)startingWound;
		this.damage.gameObject.SetActive(true);
		this.damage.minValue = 0f;
		this.damage.maxValue = (float)dest.Data.Wounds;
		this.damage.value = (float)startingWound;
		global::DG.Tweening.Sequence t = global::DG.Tweening.DOTween.Sequence().SetTarget(this).Append(this.damage.DOValue((float)dest.CurrentWounds, 0.35f, true)).Append(this.life.DOValue((float)dest.CurrentWounds, 0.25f, true));
		if (dest.CurrentWounds <= 0)
		{
			t.OnComplete(new global::DG.Tweening.TweenCallback(this.HideDamageBar));
		}
	}

	public void SetDamage(global::UnitController unit, int startingWound, string damageText, bool critical)
	{
		this.damageOrResult.enabled = false;
		if (critical)
		{
			this.damageOrResult.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_critical", new string[]
			{
				damageText
			});
		}
		else
		{
			this.damageOrResult.text = damageText;
		}
		this.life.gameObject.SetActive(true);
		this.life.minValue = 0f;
		this.life.maxValue = (float)unit.unit.Wound;
		this.life.value = (float)startingWound;
		this.damage.gameObject.SetActive(true);
		this.damage.minValue = 0f;
		this.damage.maxValue = (float)unit.unit.Wound;
		this.damage.value = (float)startingWound;
		global::DG.Tweening.Sequence t = global::DG.Tweening.DOTween.Sequence().SetTarget(this).Append(this.damage.DOValue((float)unit.unit.CurrentWound, 0.35f, true)).Append(this.life.DOValue((float)unit.unit.CurrentWound, 0.25f, true));
		if (unit.unit.CurrentWound <= 0)
		{
			t.OnComplete(new global::DG.Tweening.TweenCallback(this.HideDamageBar));
		}
	}

	private void HideDamageBar()
	{
		this.damage.gameObject.SetActive(false);
	}

	public void SetOutcome(string effect, string damageEffect)
	{
		if (!string.IsNullOrEmpty(effect))
		{
			this.check.Set(effect);
		}
		if (!string.IsNullOrEmpty(damageEffect))
		{
			this.damageOrResult.enabled = false;
			this.damageOrResult.text = damageEffect;
		}
	}

	public void ShowOutcome()
	{
		if (!string.IsNullOrEmpty(this.damageOrResult.text))
		{
			this.damageOrResult.enabled = true;
		}
	}

	public void SetTarget(global::Destructible dest)
	{
		base.gameObject.SetActive(true);
		this.unitName.text = dest.LocalizedName;
		if (dest.Owner == null)
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_ENEMY);
		}
		else if (dest.Owner.IsPlayed())
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_ALLY);
		}
		else if (dest.Owner.unit.IsMonster)
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_NEUTRAL);
		}
		else
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_ENEMY);
		}
		this.unitIcon.sprite = dest.imprintIcon;
		this.unitSubIcon.enabled = false;
		this.check.Hide();
		this.damageOrResult.text = string.Empty;
	}

	public void SetTarget(global::UnitController unit)
	{
		base.gameObject.SetActive(true);
		if (global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit == unit)
		{
			this.unitName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_target_self");
			this.unitIcon.color = global::UnityEngine.Color.white;
		}
		else
		{
			this.unitName.text = unit.unit.Name;
		}
		if (unit.IsPlayed())
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_ALLY);
		}
		else if (unit.unit.IsMonster)
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_NEUTRAL);
		}
		else
		{
			this.unitIcon.color = global::Constant.GetColor(global::ConstantId.COLOR_ENEMY);
		}
		this.unitIcon.sprite = unit.unit.GetIcon();
		this.unitSubIcon.sprite = unit.unit.GetUnitTypeIcon();
		this.unitSubIcon.enabled = (this.unitSubIcon.sprite != null);
		this.check.Hide();
		this.damageOrResult.text = string.Empty;
	}

	public void SetEnchantDamage(string enchant, int damage)
	{
		base.gameObject.SetActive(true);
		this.unitName.text = enchant;
		this.unitIcon.enabled = false;
		this.life.gameObject.SetActive(false);
		this.check.Hide();
		this.damageOrResult.enabled = false;
		this.damageOrResult.text = damage.ToConstantString();
	}

	public void SetStatus(string status)
	{
		this.newStatus.text = status;
	}

	public global::UnityEngine.UI.Text unitName;

	public global::UnityEngine.UI.Image unitIcon;

	public global::UnityEngine.UI.Image unitSubIcon;

	public global::UnityEngine.UI.Slider life;

	public global::UnityEngine.UI.Slider damage;

	public global::UnityEngine.UI.Text damageOrResult;

	public global::UnityEngine.UI.Text newStatus;

	public global::UIRetroactionResult check;

	public global::System.Collections.Generic.List<global::UIRetroactionResult> enchants;

	public global::UnityEngine.RectTransform offset;
}
