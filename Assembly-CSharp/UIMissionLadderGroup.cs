using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionLadderGroup : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.overviewSelected.enabled = false;
		this.overviewCurrent.enabled = false;
		this.unitIconTransform = (this.unitIcon.transform as global::UnityEngine.RectTransform);
	}

	public void Set(global::UnitController unitController, bool isCurrent)
	{
		global::Unit unit = unitController.unit;
		string text = unit.Initiative.ToConstantString();
		if (this.initiative.text != text)
		{
			this.initiative.text = text;
		}
		if (isCurrent)
		{
			this.unitIconTransform.sizeDelta = new global::UnityEngine.Vector2(64f, 64f);
			if (!this.backgroundBig.enabled)
			{
				this.backgroundBig.enabled = true;
			}
			if (this.background.enabled)
			{
				this.background.enabled = false;
			}
		}
		else
		{
			this.unitIconTransform.sizeDelta = new global::UnityEngine.Vector2(36f, 36f);
			if (this.backgroundBig.enabled)
			{
				this.backgroundBig.enabled = false;
			}
			if (!this.background.enabled)
			{
				this.background.enabled = true;
			}
		}
		if (!unitController.ladderVisible)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
		}
		else if (unitController.HasBeenSpotted || unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			if (unitController.IsPlayed())
			{
				if (this.backgroundBig.overrideSprite != this.backgroundAllyCurrent)
				{
					this.backgroundBig.overrideSprite = this.backgroundAllyCurrent;
				}
				if (this.background.overrideSprite != this.backgroundAlly)
				{
					this.background.overrideSprite = this.backgroundAlly;
				}
				if (this.unitIcon.color != this.allyColor)
				{
					this.unitIcon.color = this.allyColor;
				}
			}
			else if (unit.IsMonster)
			{
				if (this.backgroundBig.overrideSprite != this.backgroundNeutralCurrent)
				{
					this.backgroundBig.overrideSprite = this.backgroundNeutralCurrent;
				}
				if (this.background.overrideSprite != this.backgroundNeutral)
				{
					this.background.overrideSprite = this.backgroundNeutral;
				}
				if (this.unitIcon.color != this.neutralColor)
				{
					this.unitIcon.color = this.neutralColor;
				}
			}
			else
			{
				if (this.backgroundBig.overrideSprite != this.backgroundEnemyCurrent)
				{
					this.backgroundBig.overrideSprite = this.backgroundEnemyCurrent;
				}
				if (this.background.overrideSprite != this.backgroundEnemy)
				{
					this.background.overrideSprite = this.backgroundEnemy;
				}
				if (this.unitIcon.color != this.enemyColor)
				{
					this.unitIcon.color = this.enemyColor;
				}
			}
			global::UnityEngine.Sprite icon = unit.GetIcon();
			if (this.unitIcon.overrideSprite != icon)
			{
				this.unitIcon.overrideSprite = icon;
			}
			switch (unit.GetUnitTypeId())
			{
			case global::UnitTypeId.LEADER:
			{
				if (!this.unitStarIcon.enabled)
				{
					this.unitStarIcon.enabled = true;
				}
				global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
				if (this.unitStarIcon.sprite != sprite)
				{
					this.unitStarIcon.sprite = sprite;
				}
				goto IL_451;
			}
			case global::UnitTypeId.IMPRESSIVE:
			{
				if (!this.unitStarIcon.enabled)
				{
					this.unitStarIcon.enabled = true;
				}
				global::UnityEngine.Sprite sprite2 = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
				if (this.unitStarIcon.sprite != sprite2)
				{
					this.unitStarIcon.sprite = sprite2;
				}
				goto IL_451;
			}
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
			{
				if (!this.unitStarIcon.enabled)
				{
					this.unitStarIcon.enabled = true;
				}
				global::UnityEngine.Sprite sprite3 = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
				if (this.unitStarIcon.sprite != sprite3)
				{
					this.unitStarIcon.sprite = sprite3;
				}
				goto IL_451;
			}
			}
			if (this.unitStarIcon.enabled)
			{
				this.unitStarIcon.enabled = false;
			}
			IL_451:
			if (this.deadOverlay.enabled != (unit.Status == global::UnitStateId.OUT_OF_ACTION))
			{
				this.deadOverlay.enabled = (unit.Status == global::UnitStateId.OUT_OF_ACTION);
			}
			if (this.outOfSightOverlay.enabled != (!unitController.IsImprintVisible() && unit.Status != global::UnitStateId.OUT_OF_ACTION))
			{
				this.outOfSightOverlay.enabled = (!unitController.IsImprintVisible() && unit.Status != global::UnitStateId.OUT_OF_ACTION);
			}
		}
		else
		{
			this.backgroundBig.overrideSprite = this.backgroundUnknownCurrent;
			this.background.overrideSprite = this.backgroundUnknown;
			this.backgroundBig.overrideSprite = null;
			this.background.overrideSprite = null;
			this.unitIcon.overrideSprite = null;
			this.unitStarIcon.enabled = false;
			this.deadOverlay.enabled = false;
			this.outOfSightOverlay.enabled = false;
			this.unitIcon.color = global::UnityEngine.Color.white;
		}
	}

	public void SetCurrent(bool isCurrent, bool force, bool realCurrent)
	{
		if (isCurrent)
		{
			this.unitIconTransform.sizeDelta = new global::UnityEngine.Vector2(64f, 64f);
			if (!this.backgroundBig.enabled)
			{
				this.backgroundBig.enabled = true;
			}
			if (this.background)
			{
				this.background.enabled = false;
			}
			if (this.overviewCurrent.enabled != (force && realCurrent))
			{
				this.overviewCurrent.enabled = (force && realCurrent);
			}
			if (this.overviewSelected.enabled != force && !realCurrent)
			{
				this.overviewSelected.enabled = (force && !realCurrent);
			}
		}
		else
		{
			this.unitIconTransform.sizeDelta = new global::UnityEngine.Vector2(36f, 36f);
			if (this.backgroundBig.enabled)
			{
				this.backgroundBig.enabled = false;
			}
			if (!this.background.enabled)
			{
				this.background.enabled = true;
			}
			if (this.overviewSelected.enabled)
			{
				this.overviewSelected.enabled = false;
			}
			if (this.overviewCurrent.enabled)
			{
				this.overviewCurrent.enabled = false;
			}
		}
	}

	public global::UnityEngine.UI.Text initiative;

	public global::UnityEngine.UI.Image background;

	public global::UnityEngine.UI.Image backgroundBig;

	public global::UnityEngine.UI.Image unitIcon;

	public global::UnityEngine.RectTransform unitIconTransform;

	public global::UnityEngine.UI.Image unitStarIcon;

	public global::UnityEngine.UI.Image deadOverlay;

	public global::UnityEngine.UI.Image outOfSightOverlay;

	public global::UnityEngine.Sprite backgroundAlly;

	public global::UnityEngine.Sprite backgroundAllyCurrent;

	public global::UnityEngine.Sprite backgroundEnemy;

	public global::UnityEngine.Sprite backgroundEnemyCurrent;

	public global::UnityEngine.Sprite backgroundNeutral;

	public global::UnityEngine.Sprite backgroundNeutralCurrent;

	public global::UnityEngine.Sprite backgroundUnknown;

	public global::UnityEngine.Sprite backgroundUnknownCurrent;

	public global::UnityEngine.UI.Image overviewSelected;

	public global::UnityEngine.UI.Image overviewCurrent;

	public global::UnityEngine.Color allyColor = global::UnityEngine.Color.blue;

	public global::UnityEngine.Color enemyColor = global::UnityEngine.Color.red;

	public global::UnityEngine.Color neutralColor = global::UnityEngine.Color.magenta;
}
