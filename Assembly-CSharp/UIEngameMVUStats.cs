using System;
using UnityEngine;
using UnityEngine.UI;

public class UIEngameMVUStats : global::UnityEngine.MonoBehaviour
{
	public void Set(global::UnitController unitController)
	{
		if (unitController == null)
		{
			this.unitIcon.enabled = false;
			this.unitName.enabled = false;
			this.points.enabled = false;
			for (int i = 0; i < this.categoryPointsGo.Length; i++)
			{
				this.categoryPointsGo[i].SetActive(false);
			}
			return;
		}
		this.unitIcon.sprite = unitController.unit.GetIcon();
		switch (unitController.unit.GetUnitTypeId())
		{
		case global::UnitTypeId.LEADER:
			this.unitTypeIcon.enabled = true;
			this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
			goto IL_138;
		case global::UnitTypeId.IMPRESSIVE:
			this.unitTypeIcon.enabled = true;
			this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
			goto IL_138;
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			this.unitTypeIcon.enabled = true;
			this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
			goto IL_138;
		}
		this.unitTypeIcon.enabled = false;
		IL_138:
		this.unitName.text = unitController.unit.Name;
		this.points.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_mvu_pts", new string[]
		{
			unitController.unit.GetAttribute(global::AttributeId.CURRENT_MVU).ToConstantString()
		});
		for (int j = 0; j < unitController.MVUptsPerCategory.Length; j++)
		{
			if (unitController.MVUptsPerCategory[j] != 0)
			{
				this.categoryPointsGo[j].SetActive(true);
				this.categoryPoints[j].text = unitController.MVUptsPerCategory[j].ToString("+#;-#");
			}
			else
			{
				this.categoryPointsGo[j].SetActive(false);
			}
		}
	}

	public global::UnityEngine.UI.Image unitIcon;

	public global::UnityEngine.UI.Image unitTypeIcon;

	public global::UnityEngine.UI.Text unitName;

	public global::UnityEngine.UI.Text points;

	public global::UnityEngine.GameObject[] categoryPointsGo;

	public global::UnityEngine.UI.Text[] categoryPoints;
}
