using System;
using UnityEngine;
using UnityEngine.UI;

public class HireUnitDescription : global::UnityEngine.MonoBehaviour
{
	public virtual void Set(global::Unit unit)
	{
		if (this.unitName != null)
		{
			this.unitName.text = unit.Name;
		}
		if (this.unitType != null)
		{
			this.unitType.text = unit.LocalizedType;
		}
		this.icon.sprite = unit.GetIcon();
		if (this.unitTypeIcon != null)
		{
			switch (unit.GetUnitTypeId())
			{
			case global::UnitTypeId.LEADER:
				this.unitTypeIcon.enabled = true;
				this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
				goto IL_12B;
			case global::UnitTypeId.IMPRESSIVE:
				this.unitTypeIcon.enabled = true;
				this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
				goto IL_12B;
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				this.unitTypeIcon.enabled = true;
				this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
				goto IL_12B;
			}
			this.unitTypeIcon.enabled = false;
		}
		IL_12B:
		this.rank.text = unit.Rank.ToString();
		this.cost.text = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitHireCost(unit).ToString();
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitHireCost(unit))
		{
			this.cost.color = global::UnityEngine.Color.red;
		}
	}

	public void Set(global::UnitId unitId, int unitRank)
	{
		if (this.unitName != null)
		{
			this.unitName.text = string.Empty;
		}
		global::UnitData unitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>((int)unitId);
		if (this.unitType != null)
		{
			this.unitType.text = string.Format("{0} / {1}", global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + unitData.UnitTypeId.ToLowerString()), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_" + unitId.ToLowerString()));
		}
		this.icon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("unit/" + unitId.ToLowerString(), true);
		if (this.unitTypeIcon != null)
		{
			switch (unitData.UnitTypeId)
			{
			case global::UnitTypeId.LEADER:
				this.unitTypeIcon.enabled = true;
				this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", false);
				goto IL_197;
			case global::UnitTypeId.IMPRESSIVE:
				this.unitTypeIcon.enabled = true;
				this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
				goto IL_197;
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				this.unitTypeIcon.enabled = true;
				this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
				goto IL_197;
			}
			this.unitTypeIcon.enabled = false;
		}
		IL_197:
		this.rank.text = unitRank.ToString();
		this.cost.transform.parent.gameObject.SetActive(false);
	}

	public global::UnityEngine.UI.Text unitName;

	public global::UnityEngine.UI.Text unitType;

	public global::UnityEngine.UI.Text cost;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text rank;

	public global::UnityEngine.UI.Image unitTypeIcon;

	public global::ToggleEffects btnBuy;
}
