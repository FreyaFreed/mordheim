using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIEndGameReport : global::CanvasGroupDisabler
{
	private void Awake()
	{
	}

	public void Show()
	{
		this.isShow = true;
		base.gameObject.SetActive(true);
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.END_GAME);
		this.button.SetAction("action", "menu_continue", 3, false, null, null);
		this.button.OnAction(new global::UnityEngine.Events.UnityAction(this.OnContinue), false, true);
		global::WarbandController myWarbandCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		global::System.Collections.Generic.List<global::WarbandController> enemyWarbandCtrlrs = global::PandoraSingleton<global::MissionManager>.Instance.GetEnemyWarbandCtrlrs();
		this.centerIcon.sprite = global::Warband.GetIcon(myWarbandCtrlr.WarData.Id);
		global::VictoryTypeId victoryType = global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.VictoryType;
		if (victoryType != global::VictoryTypeId.LOSS)
		{
			this.centerOverlay.sprite = this.overlayPlayer;
			this.centerTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((victoryType != global::VictoryTypeId.OBJECTIVE) ? ("mission_victory_" + global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.VictoryType) : "mission_victory_objective");
			this.centerSubtitle.text = string.Empty;
		}
		else
		{
			this.centerOverlay.sprite = this.overlayEnemy;
			this.centerTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_victory_loss");
			this.centerSubtitle.text = string.Empty;
		}
		this.SetObjectivesRewards(myWarbandCtrlr);
		this.SetBattlegroundVictoryRewards(myWarbandCtrlr);
		this.SetTreasury(myWarbandCtrlr);
		this.SetExtraRewards(myWarbandCtrlr);
		int outOfAction = this.GetOutOfAction(myWarbandCtrlr.unitCtrlrs);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < enemyWarbandCtrlrs.Count; i++)
		{
			num += this.GetOutOfAction(enemyWarbandCtrlrs[i].unitCtrlrs);
			num2 += enemyWarbandCtrlrs[i].unitCtrlrs.Count;
		}
		this.playerWarbandName.text = myWarbandCtrlr.name;
		this.playerWarbandIcon.sprite = global::Warband.GetIcon(myWarbandCtrlr.WarData.Id);
		this.playerCasualties.text = string.Format("{0}/{1}", outOfAction, myWarbandCtrlr.unitCtrlrs.Count);
		this.enemyWarbandName.text = enemyWarbandCtrlrs[0].name;
		if (enemyWarbandCtrlrs[0].WarData.Basic)
		{
			this.enemyWarbandIcon.sprite = global::Warband.GetIcon(enemyWarbandCtrlrs[0].WarData.Id);
		}
		else if (enemyWarbandCtrlrs[0].unitCtrlrs.Count == 0)
		{
			this.enemyWarbandIcon.enabled = false;
		}
		else
		{
			this.enemyWarbandIcon.sprite = enemyWarbandCtrlrs[0].unitCtrlrs[0].unit.GetIcon();
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.isCampaign)
		{
			this.enemyCasualties.text = num.ToConstantString();
		}
		else
		{
			this.enemyCasualties.text = string.Format("{0}/{1}", num, enemyWarbandCtrlrs[0].unitCtrlrs.Count);
		}
		for (int j = 0; j < myWarbandCtrlr.unitCtrlrs.Count; j++)
		{
			if (myWarbandCtrlr.unitCtrlrs[j].unit.UnitSave.warbandSlotIndex == global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.playerMVUIdx)
			{
				this.playerMVU.Set(myWarbandCtrlr.unitCtrlrs[j]);
				break;
			}
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.isCampaign)
		{
			int num3 = -1;
			global::UnitController unitController = null;
			for (int k = 0; k < enemyWarbandCtrlrs.Count; k++)
			{
				if (enemyWarbandCtrlrs[k].unitCtrlrs.Count > 0)
				{
					global::UnitController unitController2 = enemyWarbandCtrlrs[k].unitCtrlrs[enemyWarbandCtrlrs[k].GetMVU(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, true)];
					int attribute = unitController2.unit.GetAttribute(global::AttributeId.CURRENT_MVU);
					if (attribute > num3)
					{
						num3 = attribute;
						unitController = unitController2;
					}
				}
			}
			this.enemyMVU.Set(unitController);
		}
		else
		{
			this.enemyMVU.Set(enemyWarbandCtrlrs[0].unitCtrlrs[global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.enemyMVUIdx]);
		}
	}

	public int GetOutOfAction(global::System.Collections.Generic.List<global::UnitController> units)
	{
		int num = 0;
		for (int i = 0; i < units.Count; i++)
		{
			if (units[i].unit.Status == global::UnitStateId.OUT_OF_ACTION)
			{
				num++;
			}
		}
		return num;
	}

	public void SetObjectivesRewards(global::WarbandController playerWarband)
	{
		if (playerWarband.objectives.Count > 0 && playerWarband.AllObjectivesCompleted)
		{
			this.objectivesLock.sprite = this.unlockedSprite;
			this.objectivesGroup.alpha = 1f;
			this.objectivesExperience.text = global::Constant.GetInt(global::ConstantId.UNIT_XP_OBJECTIVE).ToString("+#;-#");
		}
		else
		{
			this.objectivesLock.sprite = this.lockedSprite;
			this.objectivesGroup.alpha = 0.25f;
			this.objectivesExperience.text = string.Empty;
		}
	}

	public void SetBattlegroundVictoryRewards(global::WarbandController playerWarband)
	{
		if (!playerWarband.defeated && !global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.isSkirmish)
		{
			this.battlegroundVictoryGroup.alpha = 1f;
			this.battlegroundVictoryLock.sprite = this.unlockedSprite;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (playerWarband.wyrdstones != null)
			{
				for (int i = 0; i < playerWarband.wyrdstones.Count; i++)
				{
					global::Item item = playerWarband.wyrdstones[i];
					global::ItemId id = item.Id;
					if (id != global::ItemId.WYRDSTONE_SHARD)
					{
						if (id != global::ItemId.WYRDSTONE_CLUSTER)
						{
							if (id == global::ItemId.WYRDSTONE_FRAGMENT)
							{
								num2++;
							}
						}
						else
						{
							num++;
						}
					}
					else
					{
						num3++;
					}
				}
			}
			this.battlegroundVictoryClusters.text = num.ToConstantString();
			this.battlegroundVictoryFragments.text = num2.ToConstantString();
			this.battlegroundVictoryShards.text = num3.ToConstantString();
			this.battlegroundVictorySearch.text = playerWarband.spoilsFound.ToConstantString();
		}
		else
		{
			this.battlegroundVictoryGroup.alpha = 0.25f;
			this.battlegroundVictoryLock.sprite = this.lockedSprite;
			this.battlegroundVictoryClusters.text = "0";
			this.battlegroundVictoryFragments.text = "0";
			this.battlegroundVictoryShards.text = "0";
			this.battlegroundVictorySearch.text = "0";
		}
	}

	public void SetTreasury(global::WarbandController playerWarband)
	{
		int value = 0;
		int value2 = 0;
		int value3 = 0;
		int value4 = 0;
		if (!global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.isSkirmish)
		{
			this.IncrementRewardsCount(playerWarband.wagon.chest.items, ref value, ref value2, ref value3, ref value4);
			for (int i = 0; i < playerWarband.unitCtrlrs.Count; i++)
			{
				if (playerWarband.unitCtrlrs[i].unit.Status != global::UnitStateId.OUT_OF_ACTION || !playerWarband.defeated)
				{
					this.IncrementRewardsCount(playerWarband.unitCtrlrs[i].unit.Items, ref value, ref value2, ref value3, ref value4);
				}
			}
		}
		this.clusters.text = value2.ToConstantString();
		this.fragments.text = value3.ToConstantString();
		this.shards.text = value4.ToConstantString();
		this.gold.text = value.ToConstantString();
	}

	private void IncrementRewardsCount(global::System.Collections.Generic.List<global::Item> items, ref int goldCount, ref int clustersCount, ref int fragmentsCount, ref int shardsCount)
	{
		for (int i = 0; i < items.Count; i++)
		{
			global::ItemId id = items[i].Id;
			switch (id)
			{
			case global::ItemId.WYRDSTONE_FRAGMENT:
				fragmentsCount++;
				break;
			default:
				if (id != global::ItemId.WYRDSTONE_SHARD)
				{
					if (id == global::ItemId.WYRDSTONE_CLUSTER)
					{
						clustersCount++;
					}
				}
				else
				{
					shardsCount++;
				}
				break;
			case global::ItemId.GOLD:
				goldCount += items[i].Amount;
				break;
			}
		}
	}

	public void SetExtraRewards(global::WarbandController playerWarband)
	{
		if (playerWarband.rewardItems != null)
		{
			this.noExtraRewards.gameObject.SetActive(false);
			int @int = global::Constant.GetInt(global::ConstantId.END_GAME_DECISIVE_REWARD);
			int num = 0;
			while (num < playerWarband.rewardItems.Count && num < @int)
			{
				global::UnityEngine.GameObject gameObject = this.extraRewards[num];
				global::UIInventoryItem component = gameObject.GetComponent<global::UIInventoryItem>();
				component.Set(playerWarband.rewardItems[num], false, false, global::ItemId.NONE, false);
				num++;
			}
		}
		else
		{
			this.noExtraRewards.gameObject.SetActive(true);
			for (int i = 0; i < this.extraRewards.Count; i++)
			{
				this.extraRewards[i].gameObject.SetActive(false);
			}
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		if (this.isShow)
		{
			this.isShow = false;
		}
	}

	private void OnContinue()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.END_GAME);
		global::PandoraSingleton<global::MissionManager>.Instance.ForceQuitMission();
		this.button.transform.parent.gameObject.SetActive(false);
	}

	public global::UnityEngine.Sprite lockedSprite;

	public global::UnityEngine.Sprite unlockedSprite;

	public global::UnityEngine.UI.Text fragments;

	public global::UnityEngine.UI.Text shards;

	public global::UnityEngine.UI.Text clusters;

	public global::UnityEngine.UI.Text gold;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> extraRewards;

	public global::UnityEngine.UI.Text noExtraRewards;

	public global::UnityEngine.CanvasGroup objectivesGroup;

	public global::UnityEngine.UI.Image objectivesLock;

	public global::UnityEngine.UI.Text objectivesExperience;

	public global::UnityEngine.CanvasGroup battlegroundVictoryGroup;

	public global::UnityEngine.UI.Image battlegroundVictoryLock;

	public global::UnityEngine.UI.Text battlegroundVictoryFragments;

	public global::UnityEngine.UI.Text battlegroundVictoryShards;

	public global::UnityEngine.UI.Text battlegroundVictoryClusters;

	public global::UnityEngine.UI.Text battlegroundVictorySearch;

	public global::UnityEngine.UI.Image playerWarbandIcon;

	public global::UnityEngine.UI.Text playerWarbandName;

	public global::UnityEngine.UI.Text playerCasualties;

	public global::UIEngameMVUStats playerMVU;

	public global::UnityEngine.UI.Image enemyWarbandIcon;

	public global::UnityEngine.UI.Text enemyWarbandName;

	public global::UnityEngine.UI.Text enemyCasualties;

	public global::UIEngameMVUStats enemyMVU;

	public global::UnityEngine.UI.Text centerTitle;

	public global::UnityEngine.UI.Text centerSubtitle;

	public global::UnityEngine.UI.Image centerIcon;

	public global::UnityEngine.UI.Image centerOverlay;

	public global::UnityEngine.Sprite overlayEnemy;

	public global::UnityEngine.Sprite overlayPlayer;

	public global::ButtonGroup button;

	private bool isShow;
}
