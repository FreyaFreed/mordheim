using System;
using UnityEngine;
using UnityEngine.UI;

public class FlyingOverview : global::FlyingText
{
	private void Awake()
	{
		this.orientation.SetActive(false);
	}

	public void Set(global::MapImprint imprint, bool clamp, bool selected)
	{
		this.imprint = imprint;
		global::UnityEngine.Vector3 lastKnownPos = imprint.lastKnownPos;
		lastKnownPos.y += 2.75f;
		base.Play(lastKnownPos, null);
		switch (imprint.imprintType)
		{
		case global::MapImprintType.UNIT:
			base.transform.SetParent(global::PandoraSingleton<global::FlyingTextManager>.Instance.unitContainer);
			if (imprint.UnitCtrlr != null && imprint.UnitCtrlr != global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
			{
				base.transform.SetAsFirstSibling();
			}
			break;
		case global::MapImprintType.PLAYER_WAGON:
		case global::MapImprintType.ENEMY_WAGON:
		case global::MapImprintType.INTERACTIVE_POINT:
		case global::MapImprintType.WYRDSTONE:
		case global::MapImprintType.TRAP:
		case global::MapImprintType.DESTRUCTIBLE:
			base.transform.SetParent(global::PandoraSingleton<global::FlyingTextManager>.Instance.miscContainer);
			if (imprint.imprintType != global::MapImprintType.ENEMY_WAGON && imprint.imprintType != global::MapImprintType.PLAYER_WAGON)
			{
				base.transform.SetAsFirstSibling();
			}
			break;
		case global::MapImprintType.BEACON:
			base.transform.SetParent(global::PandoraSingleton<global::FlyingTextManager>.Instance.beaconContainer);
			break;
		case global::MapImprintType.PLAYER_DEPLOYMENT:
		case global::MapImprintType.ENEMY_DEPLOYMENT:
			base.transform.SetParent(global::PandoraSingleton<global::FlyingTextManager>.Instance.deploymentContainter);
			break;
		default:
			global::PandoraDebug.LogWarning("Unknown imprint type while setting imprint in FlyingOverview::Set", "UI", imprint);
			break;
		}
		bool flag = imprint.State == global::MapImprintStateId.VISIBLE || imprint.State == global::MapImprintStateId.LOST;
		if (this.icon.enabled != flag)
		{
			this.icon.enabled = flag;
		}
		this.icon.sprite = imprint.visibleTexture;
		if (imprint.UnitCtrlr != null)
		{
			this.icon.color = ((!imprint.UnitCtrlr.IsPlayed()) ? ((!imprint.UnitCtrlr.unit.IsMonster) ? this.enemyColor : this.neutralColor) : this.allyColor);
		}
		else if (imprint.imprintType == global::MapImprintType.PLAYER_DEPLOYMENT)
		{
			this.icon.color = this.allyColor;
		}
		else if (imprint.imprintType == global::MapImprintType.ENEMY_DEPLOYMENT)
		{
			this.icon.color = this.enemyColor;
		}
		else
		{
			this.icon.color = global::UnityEngine.Color.white;
		}
		if (imprint.UnitCtrlr != null)
		{
			switch (imprint.UnitCtrlr.unit.GetUnitTypeId())
			{
			case global::UnitTypeId.LEADER:
				this.iconLeader.enabled = true;
				this.iconLeader.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
				goto IL_319;
			case global::UnitTypeId.IMPRESSIVE:
				this.iconLeader.enabled = true;
				this.iconLeader.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
				goto IL_319;
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				this.iconLeader.enabled = true;
				this.iconLeader.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
				goto IL_319;
			}
			this.iconLeader.enabled = false;
			IL_319:;
		}
		else
		{
			this.iconLeader.enabled = false;
		}
		bool flag2 = imprint.imprintType == global::MapImprintType.TRAP && imprint.Trap != null && global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().teamIdx == imprint.Trap.TeamIdx;
		this.iconBounty.enabled = (imprint.UnitCtrlr != null && !imprint.UnitCtrlr.IsPlayed() && imprint.UnitCtrlr.IsBounty());
		this.iconLost.enabled = (imprint.UnitCtrlr != null && imprint.State == global::MapImprintStateId.LOST);
		this.iconIdol.sprite = imprint.idolTexture;
		this.iconIdol.enabled = (imprint.idolTexture != null);
		this.iconSearched.enabled = (imprint.Search != null && imprint.Search.wasSearched);
		this.iconSearched.color = ((!this.iconSearched.enabled || !imprint.Search.IsEmpty()) ? global::UnityEngine.Color.white : global::UnityEngine.Color.red);
		this.bgNeutral.enabled = (this.icon.enabled && imprint.UnitCtrlr != null && imprint.UnitCtrlr.unit.IsMonster);
		this.bgEnemy.enabled = ((this.icon.enabled && imprint.UnitCtrlr != null && !imprint.UnitCtrlr.IsPlayed() && !this.bgNeutral.enabled) || imprint.imprintType == global::MapImprintType.ENEMY_WAGON || imprint.imprintType == global::MapImprintType.ENEMY_DEPLOYMENT);
		this.bgPlayer.enabled = (this.icon.enabled && ((imprint.UnitCtrlr != null && imprint.UnitCtrlr.IsPlayed()) || imprint.imprintType == global::MapImprintType.PLAYER_WAGON || imprint.imprintType == global::MapImprintType.PLAYER_DEPLOYMENT));
		this.bgCurrentPlayer.enabled = (this.icon.enabled && imprint.UnitCtrlr != null && global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() == imprint.UnitCtrlr);
		this.bgSelected.enabled = selected;
		this.bgWyrdstone.enabled = (this.icon.enabled && (imprint.imprintType == global::MapImprintType.WYRDSTONE || (!flag2 && imprint.imprintType == global::MapImprintType.TRAP && imprint.Trap.enemyImprintType == global::MapImprintType.WYRDSTONE)));
		this.bgSearchPoint.enabled = (this.icon.enabled && (imprint.imprintType == global::MapImprintType.INTERACTIVE_POINT || imprint.imprintType == global::MapImprintType.DESTRUCTIBLE || (!flag2 && imprint.imprintType == global::MapImprintType.TRAP && imprint.Trap.enemyImprintType == global::MapImprintType.INTERACTIVE_POINT)));
		this.bgBeacon.enabled = (this.icon.enabled && imprint.imprintType == global::MapImprintType.BEACON);
		this.bgTrap.enabled = (this.icon.enabled && imprint.imprintType == global::MapImprintType.TRAP && flag2);
		if (this.bgBeacon.enabled)
		{
			this.icon.color = this.bgBeacon.color;
		}
		this.orientation.SetActive(imprint.UnitCtrlr != null && imprint.State == global::MapImprintStateId.VISIBLE && global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() == imprint.UnitCtrlr);
		this.clamped = (this.orientation.activeSelf || clamp);
		if (imprint.State == global::MapImprintStateId.VISIBLE && (imprint.UnitCtrlr != null || imprint.Destructible != null))
		{
			int num = (!(imprint.UnitCtrlr != null)) ? imprint.Destructible.Data.Wounds : imprint.UnitCtrlr.unit.Wound;
			int num2 = (!(imprint.UnitCtrlr != null)) ? imprint.Destructible.CurrentWounds : imprint.UnitCtrlr.unit.CurrentWound;
			this.icon_ooa.enabled = ((!(imprint.UnitCtrlr != null)) ? (imprint.Destructible.CurrentWounds == 0) : (imprint.UnitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION));
			this.hp.gameObject.SetActive(true);
			this.damage.gameObject.SetActive(true);
			if (num2 > 0)
			{
				this.damage.fillRect.gameObject.SetActive(true);
				this.hp.fillRect.gameObject.SetActive(true);
				this.hp.enabled = true;
				global::UnityEngine.UI.Slider slider = this.damage;
				float num3 = 0f;
				this.hp.minValue = num3;
				slider.minValue = num3;
				global::UnityEngine.UI.Slider slider2 = this.damage;
				num3 = (float)num;
				this.hp.maxValue = num3;
				slider2.maxValue = num3;
				this.hp.value = (float)num2;
				this.damage.value = (float)num2;
			}
			else
			{
				this.damage.fillRect.gameObject.SetActive(false);
				this.hp.fillRect.gameObject.SetActive(false);
			}
		}
		else
		{
			this.icon_ooa.enabled = false;
			this.hp.gameObject.SetActive(false);
			this.damage.gameObject.SetActive(false);
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}

	private void Update()
	{
		if (this.orientation.activeSelf)
		{
			this.orientation.transform.rotation = global::UnityEngine.Quaternion.Euler(new global::UnityEngine.Vector3(0f, 0f, global::PandoraSingleton<global::MissionManager>.Instance.CamManager.transform.rotation.eulerAngles.y - this.imprint.UnitCtrlr.transform.rotation.eulerAngles.y));
		}
	}

	public global::UnityEngine.Color enemyColor = global::UnityEngine.Color.red;

	public global::UnityEngine.Color allyColor = global::UnityEngine.Color.blue;

	public global::UnityEngine.Color neutralColor = global::UnityEngine.Color.magenta;

	public global::UnityEngine.UI.Image bgEnemy;

	public global::UnityEngine.UI.Image bgPlayer;

	public global::UnityEngine.UI.Image bgNeutral;

	public global::UnityEngine.UI.Image bgCurrentPlayer;

	public global::UnityEngine.UI.Image bgSelected;

	public global::UnityEngine.UI.Image bgWyrdstone;

	public global::UnityEngine.UI.Image bgSearchPoint;

	public global::UnityEngine.UI.Image bgBeacon;

	public global::UnityEngine.UI.Image bgTrap;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image icon_ooa;

	public global::UnityEngine.UI.Image iconLeader;

	public global::UnityEngine.UI.Image iconBounty;

	public global::UnityEngine.UI.Image iconLost;

	public global::UnityEngine.UI.Image iconIdol;

	public global::UnityEngine.UI.Image iconSearched;

	public global::UnityEngine.GameObject orientation;

	public global::UnityEngine.UI.Slider hp;

	public global::UnityEngine.UI.Slider damage;

	[global::UnityEngine.HideInInspector]
	public global::MapImprint imprint;
}
