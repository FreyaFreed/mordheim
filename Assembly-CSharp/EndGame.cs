using System;
using System.Collections.Generic;

public class EndGame : global::ICheapState
{
	public EndGame(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.missionMngr.gameFinished = true;
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.END_GAME);
		global::PandoraSingleton<global::MissionManager>.Instance.RestoreUnitWeapons();
		global::WarbandController myWarbandCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		global::UnitController unitController = myWarbandCtrlr.GetAliveLeader();
		if (unitController == null)
		{
			myWarbandCtrlr.GetAliveHighestLeaderShip();
		}
		if (unitController == null)
		{
			unitController = myWarbandCtrlr.GetLeader();
		}
		if (unitController == null)
		{
			unitController = myWarbandCtrlr.unitCtrlrs[0];
		}
		unitController.Imprint.alwaysVisible = true;
		unitController.Hide(false, true, null);
		global::MissionEndDataSave missionEndData = this.missionMngr.MissionEndData;
		missionEndData.won = (myWarbandCtrlr.teamIdx == global::PandoraSingleton<global::MissionManager>.Instance.VictoriousTeamIdx);
		missionEndData.primaryObjectiveCompleted = myWarbandCtrlr.AllObjectivesCompleted;
		this.UpdateMVU();
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			global::System.Collections.Generic.List<global::UnitController> allMyUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllMyUnits();
			if (missionEndData.wagonItems == null)
			{
				missionEndData.wagonItems = new global::Chest();
			}
			missionEndData.wagonItems.Clear();
			if (myWarbandCtrlr.wagon.chest)
			{
				missionEndData.wagonItems.AddItems(myWarbandCtrlr.wagon.chest.GetItems());
			}
			this.UpdateSpoilsOfWars();
			missionEndData.AddUnits(allMyUnits.Count);
			for (int i = allMyUnits.Count - 1; i >= 0; i--)
			{
				if (allMyUnits[i].unit.CampaignData != null)
				{
					for (int j = 6; j < allMyUnits[i].unit.Items.Count; j++)
					{
						if (allMyUnits[i].unit.Items[j].Id != global::ItemId.NONE && allMyUnits[i].unit.Items[j].TypeData.Id != global::ItemTypeId.QUEST_ITEM && myWarbandCtrlr.wagon.chest)
						{
							missionEndData.wagonItems.AddItem(allMyUnits[i].unit.Items[j].Save, false);
						}
					}
				}
				else
				{
					missionEndData.UpdateUnit(allMyUnits[i]);
				}
			}
			missionEndData.playerMVUIdx = this.missionMngr.GetMyWarbandCtrlr().GetMVU(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, false);
			missionEndData.enemyMVUIdx = this.missionMngr.GetEnemyWarbandCtrlrs()[0].GetMVU(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, true);
		}
		this.missionMngr.MissionEndData.missionFinished = true;
		if (this.missionMngr.MissionEndData.missionSave.isTuto)
		{
			global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
		}
		else if (global::PandoraSingleton<global::GameManager>.Instance.currentSave != null)
		{
			global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(global::PandoraSingleton<global::GameManager>.Instance.currentSave, global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
		if (missionEndData.VictoryType != global::VictoryTypeId.LOSS)
		{
			if (this.missionMngr.GetMyWarbandCtrlr().WarData.AllegianceId == global::AllegianceId.DESTRUCTION)
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("destruction_win", false);
			}
			else
			{
				global::PandoraSingleton<global::Pan>.Instance.PlayMusic("order_win", false);
			}
		}
		else if (this.missionMngr.GetMyWarbandCtrlr().WarData.AllegianceId == global::AllegianceId.DESTRUCTION)
		{
			global::PandoraSingleton<global::Pan>.Instance.PlayMusic("destruction_lose", false);
		}
		else
		{
			global::PandoraSingleton<global::Pan>.Instance.PlayMusic("order_lose", false);
		}
		if (!global::PandoraSingleton<global::MissionManager>.Instance.isDeploying)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, unitController.transform, true, false, true, false);
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			global::CampaignMissionData campaignMissionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.campaignId);
			string sequence = "defeat";
			if (global::PandoraSingleton<global::GameManager>.Instance.Profile.TutorialCompletion[campaignMissionData.Idx - 1])
			{
				sequence = "victory";
			}
			global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(sequence, unitController, new global::DelSequenceDone(this.OnSeqDone));
		}
		else if (this.missionMngr.MissionEndData.VictoryType != global::VictoryTypeId.OBJECTIVE)
		{
			if (!global::PandoraSingleton<global::MissionManager>.Instance.isDeploying)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence((!myWarbandCtrlr.defeated) ? "victory" : "defeat", unitController, new global::DelSequenceDone(this.OnSeqDone));
			}
			else
			{
				this.OnSeqDone();
			}
		}
		else
		{
			this.OnSeqDone();
		}
		if (this.missionMngr.MissionEndData.VictoryType != global::VictoryTypeId.OBJECTIVE)
		{
			for (int k = 0; k < this.missionMngr.WarbandCtrlrs.Count; k++)
			{
				for (int l = 0; l < this.missionMngr.WarbandCtrlrs[k].unitCtrlrs.Count; l++)
				{
					if (this.missionMngr.WarbandCtrlrs[k].unitCtrlrs[l].unit.Status == global::UnitStateId.NONE)
					{
						this.missionMngr.WarbandCtrlrs[k].unitCtrlrs[l].animator.SetInteger(global::AnimatorIds.action, 50);
						this.missionMngr.WarbandCtrlrs[k].unitCtrlrs[l].animator.SetInteger(global::AnimatorIds.variation, (!this.missionMngr.WarbandCtrlrs[k].defeated) ? 1 : 4);
					}
				}
			}
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.END_GAME);
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	public void OnSeqDone()
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.OPTIONS_QUIT_GAME, false, false);
		}
		else
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.MISSION_END);
		}
	}

	public void UpdateMVU()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i];
			if (warbandController.wagon != null && warbandController.wagon.chest != null)
			{
				for (int j = 0; j < warbandController.wagon.chest.items.Count; j++)
				{
					global::Item item = warbandController.wagon.chest.items[j];
					for (int k = 0; k < warbandController.unitCtrlrs.Count; k++)
					{
						global::UnitController unitController = warbandController.unitCtrlrs[k];
						if (unitController.unit == item.owner)
						{
							global::EndGame.AddItemMVU(unitController, item);
							break;
						}
					}
				}
			}
		}
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int l = 0; l < allUnits.Count; l++)
		{
			global::UnitController unitController2 = allUnits[l];
			for (int m = 6; m < unitController2.unit.Items.Count; m++)
			{
				global::EndGame.AddItemMVU(unitController2, unitController2.unit.Items[m]);
			}
		}
	}

	private static void AddItemMVU(global::UnitController unitController, global::Item item)
	{
		global::ConstantId constantId = global::ConstantId.NONE;
		global::ItemId id = item.Id;
		if (id != global::ItemId.WYRDSTONE_SHARD)
		{
			if (id != global::ItemId.WYRDSTONE_CLUSTER)
			{
				if (id == global::ItemId.WYRDSTONE_FRAGMENT)
				{
					constantId = global::ConstantId.MVU_GATHER_FRAGMENT;
				}
			}
			else
			{
				constantId = global::ConstantId.MVU_GATHER_CLUSTER;
			}
		}
		else
		{
			constantId = global::ConstantId.MVU_GATHER_SHARD;
		}
		if (constantId != global::ConstantId.NONE)
		{
			unitController.AddMvuPoint(constantId, global::MvuCategory.WYRDSTONE, 1);
		}
	}

	public void UpdateSpoilsOfWars()
	{
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		global::System.Collections.Generic.List<global::WarbandController> warbandCtrlrs = this.missionMngr.WarbandCtrlrs;
		for (int i = 0; i < warbandCtrlrs.Count; i++)
		{
			global::WarbandController warbandController = warbandCtrlrs[i];
			if (warbandController.wagon != null)
			{
				for (int j = warbandController.wagon.chest.items.Count - 1; j >= 0; j--)
				{
					if (warbandController.wagon.chest.items[j].TypeData.Id == global::ItemTypeId.QUEST_ITEM)
					{
						warbandController.wagon.chest.items.RemoveAt(j);
					}
				}
			}
			if (warbandController.defeated && warbandController.wagon != null && warbandController.wagon.chest != null && (this.missionMngr.MissionEndData.crushed || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.VictoryTypeId == 2))
			{
				list.AddRange(warbandController.wagon.chest.GetItemsAndClear());
			}
		}
		global::System.Collections.Generic.List<global::WarbandController> list2 = new global::System.Collections.Generic.List<global::WarbandController>();
		int num = 0;
		for (int k = 0; k < warbandCtrlrs.Count; k++)
		{
			if (!warbandCtrlrs[k].defeated && warbandCtrlrs[k].playerTypeId == global::PlayerTypeId.PLAYER)
			{
				list2.Add(warbandCtrlrs[k]);
				for (int l = 0; l < warbandCtrlrs[k].unitCtrlrs.Count; l++)
				{
					if (warbandCtrlrs[k].unitCtrlrs[l].unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						num++;
					}
				}
			}
		}
		int num2 = list2.Count;
		global::PandoraDebug.LogInfo("Winners Count : " + num2, "END GAME", null);
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.VictoryTypeId != 2)
		{
			global::System.Collections.Generic.List<global::Item> list3 = new global::System.Collections.Generic.List<global::Item>();
			global::System.Collections.Generic.List<global::Item> list4 = new global::System.Collections.Generic.List<global::Item>();
			global::PandoraSingleton<global::MissionManager>.Instance.GetUnclaimedLootableItems(ref list3, ref list4);
			global::PandoraDebug.LogInfo("Unclaimed items count : " + list4.Count, "END GAME", null);
			global::ProcMissionRatingData procMissionRatingData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.ratingId);
			float perc = (float)(procMissionRatingData.RewardWyrdstonePerc * num) / 100f;
			global::System.Collections.Generic.List<global::Item> percList = global::PandoraUtils.GetPercList<global::Item>(ref list3, perc);
			list.AddRange(percList);
			float perc2 = (float)(procMissionRatingData.RewardSearchPerc * num) / 100f;
			global::System.Collections.Generic.List<global::Item> percList2 = global::PandoraUtils.GetPercList<global::Item>(ref list4, perc2);
			global::PandoraDebug.LogInfo("Gained Items : " + percList2.Count, "END GAME", null);
			list.AddRange(percList2);
		}
		global::WarbandController myWarbandCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		for (int m = 0; m < list2.Count; m++)
		{
			global::WarbandController warbandController2 = list2[m];
			warbandController2.wyrdstones.Clear();
			warbandController2.spoilsFound = 0;
			bool flag = warbandController2 == myWarbandCtrlr;
			for (int n = list.Count - 1; n >= 0; n -= num2)
			{
				if (flag)
				{
					this.missionMngr.MissionEndData.wagonItems.AddItem(list[n].Save, false);
					global::PandoraDebug.LogInfo("Add " + (global::ItemId)list[n].Save.id + "to stuff transfered to hideout", "END GAME", null);
				}
				if (list[n].IsWyrdStone)
				{
					warbandController2.wyrdstones.Add(list[n]);
					global::PandoraDebug.LogInfo("Add " + (global::ItemId)list[n].Save.id + " to end game display", "END GAME", null);
				}
				else
				{
					warbandController2.spoilsFound++;
					global::PandoraDebug.LogInfo("Add " + (global::ItemId)list[n].Save.id + " to end game display", "END GAME", null);
				}
				list.RemoveAt(n);
			}
			if (flag && this.missionMngr.MissionEndData.VictoryType == global::VictoryTypeId.DECISIVE)
			{
				warbandController2.rewardItems = this.GetRewardItems(warbandController2.Rank);
				for (int num3 = 0; num3 < warbandController2.rewardItems.Count; num3++)
				{
					this.missionMngr.MissionEndData.wagonItems.AddItem(warbandController2.rewardItems[num3].Save, false);
				}
			}
			num2--;
		}
	}

	private global::System.Collections.Generic.List<global::Item> GetRewardItems(int rank)
	{
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		global::System.Collections.Generic.List<global::SearchRewardItemData> rewardItems = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchRewardItemData>("warband_rank", rank.ToString());
		for (int i = 0; i < global::Constant.GetInt(global::ConstantId.END_GAME_DECISIVE_REWARD); i++)
		{
			list.Add(global::Item.GetItemReward(rewardItems, global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche));
		}
		return list;
	}

	private global::MissionManager missionMngr;

	private enum GameResult
	{
		DEFEAT = 1,
		VICTORY
	}
}
