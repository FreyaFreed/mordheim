using System;
using UnityEngine;
using UnityEngine.UI;

public class FlyingTarget : global::FlyingText
{
	public void Play(global::UnitController unitCtrlr, global::UnitController target)
	{
		global::UnityEngine.Vector3 position = target.BonesTr[global::BoneId.RIG_HEAD].position;
		position.y = target.CapsuleHeight;
		base.Play(position, null);
		if (target.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			this.icon.sprite = target.Imprint.visibleTexture;
			this.icon.color = ((!target.IsPlayed()) ? this.enemyColor : global::UnityEngine.Color.white);
		}
		else if (target.Imprint.State == global::MapImprintStateId.LOST)
		{
			this.icon.sprite = target.Imprint.lostTexture;
		}
		this.hp.gameObject.SetActive(true);
		this.damage.gameObject.SetActive(true);
		global::UnityEngine.UI.Slider slider = this.damage;
		float num = 0f;
		this.hp.minValue = num;
		slider.minValue = num;
		global::UnityEngine.UI.Slider slider2 = this.damage;
		num = (float)target.unit.Wound;
		this.hp.maxValue = num;
		slider2.maxValue = num;
		int minDamage = unitCtrlr.CurrentAction.GetMinDamage(false);
		int maxDamage = unitCtrlr.CurrentAction.GetMaxDamage(false);
		this.hp.value = (float)target.unit.CurrentWound;
		this.damage.value = (float)target.unit.CurrentWound - (float)(maxDamage + minDamage) / 2f;
	}

	public void Play(global::UnitController unitCtrlr, global::Destructible target)
	{
		global::UnityEngine.Vector3 position = target.transform.position + global::UnityEngine.Vector3.up * 1.75f;
		base.Play(position, null);
		if (target.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			this.icon.sprite = target.Imprint.visibleTexture;
			this.icon.color = this.enemyColor;
		}
		this.hp.gameObject.SetActive(true);
		this.damage.gameObject.SetActive(true);
		global::UnityEngine.UI.Slider slider = this.damage;
		float num = 0f;
		this.hp.minValue = num;
		slider.minValue = num;
		global::UnityEngine.UI.Slider slider2 = this.damage;
		num = (float)target.Data.Wounds;
		this.hp.maxValue = num;
		slider2.maxValue = num;
		int minDamage = unitCtrlr.CurrentAction.GetMinDamage(false);
		int maxDamage = unitCtrlr.CurrentAction.GetMaxDamage(false);
		this.hp.value = (float)target.CurrentWounds;
		this.damage.value = (float)target.CurrentWounds - (float)(maxDamage + minDamage) / 2f;
	}

	public global::UnityEngine.Color enemyColor = global::UnityEngine.Color.red;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Slider hp;

	public global::UnityEngine.UI.Slider damage;
}
