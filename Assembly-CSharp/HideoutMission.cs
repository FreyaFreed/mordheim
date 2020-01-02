using System;
using System.Collections.Generic;
using UnityEngine;

public class HideoutMission : global::ICheapState
{
	public HideoutMission(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
		this.pawns = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject>();
		for (int i = 0; i <= 3; i++)
		{
			string text = "map_pawn_" + ((global::PrimaryObjectiveTypeId)i).ToString();
			int index = i;
			global::PandoraDebug.LogDebug("Map Pawn = " + text, "uncategorised", null);
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/maps/", global::AssetBundleId.PROPS, text + ".prefab", delegate(global::UnityEngine.Object go)
			{
				this.pawns[index] = (global::UnityEngine.GameObject)go;
			});
		}
	}

	void global::ICheapState.Update()
	{
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		this.priceDatas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ScoutPriceData>("warband_rank", this.warband.Rank.ToString());
		this.missionConfirm = false;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.ClearLookAtFocus();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET,
			global::ModuleId.MISSION
		});
		global::WarbandSheetModule moduleLeft = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandSheetModule>(global::ModuleId.WARBAND_SHEET);
		moduleLeft.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.TREASURY
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION
		});
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.MISSION);
		this.warbandTabs.Refresh();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::MissionModule>(global::ModuleId.MISSION).Setup(new global::MissionModule.OnScoutButton(this.OnScoutButton));
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		this.RefreshMissionsDifficulty();
		global::MenuNode mapNode = global::PandoraSingleton<global::HideoutManager>.Instance.mapNode;
		mapNode.SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.gameObject);
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.transform.localPosition = global::UnityEngine.Vector3.zero;
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.transform.localScale = global::UnityEngine.Vector3.one;
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.Clear();
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.nodes.Count; i++)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.nodes[i].gameObject.SetActive(false);
		}
		this.AddPawns(0);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count == 0)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("missions_unavailable_title", "missions_unavailable_desc", delegate(bool c)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
			}, false, false);
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
		}
		else
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.Activate(new global::MenuNodeDelegateNode(this.CampNodeSelecteded), new global::MenuNodeDelegateNode(this.Dis1NodeSelecteded), new global::MenuNodeDelegateNode(this.Dis2NodeSelecteded), new global::MenuNodeDelegateNode(this.NodeUnselect), new global::MenuNodeDelegateNode(this.NodeConfirmed), global::PandoraInput.InputLayer.NORMAL, true);
		}
		this.once = true;
	}

	public void Exit(int iTo)
	{
		global::WarbandSwapModule moduleCenter = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSwapModule>(global::ModuleId.SWAP);
		if (moduleCenter != null && moduleCenter.isActiveAndEnabled)
		{
			moduleCenter.ForceClose();
		}
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.Deactivate();
	}

	private void RefreshMissionsDifficulty()
	{
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count; i++)
		{
			if (global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].missionSave.isCampaign)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].RefreshDifficulty(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetRating(), false);
			}
			else
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].RefreshDifficulty(global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].missionSave.rating, true);
			}
		}
	}

	private void AddPawns(int startIndex)
	{
		global::PandoraDebug.LogDebug("Mission Counts = " + global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count, "uncategorised", null);
		for (int i = startIndex; i < global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count; i++)
		{
			global::Mission mission = global::PandoraSingleton<global::HideoutManager>.Instance.missions[i];
			if (mission.missionSave.isCampaign)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.campaignNodes[mission.missionSave.mapPosition].gameObject.SetActive(true);
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.campaignNodes[mission.missionSave.mapPosition].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.mapPawn);
			}
			else if (this.pawns.ContainsKey(mission.missionSave.objectiveTypeIds[0]))
			{
				if (mission.GetDistrictId() == global::DistrictId.MERCHANT)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.procNodesDis01[mission.missionSave.mapPosition].gameObject.SetActive(true);
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.procNodesDis01[mission.missionSave.mapPosition].SetContent(global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.pawns[mission.missionSave.objectiveTypeIds[0]]));
				}
				else if (mission.GetDistrictId() == global::DistrictId.NOBLE)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.procNodesDis02[mission.missionSave.mapPosition].gameObject.SetActive(true);
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.procNodesDis02[mission.missionSave.mapPosition].SetContent(global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.pawns[mission.missionSave.objectiveTypeIds[0]]));
				}
			}
			else
			{
				global::PandoraDebug.LogError("Pawns sholud already be loaded! HideoutMission::AddPawns", "uncategorised", null);
			}
		}
	}

	private void OnScoutButton()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.SMALL, new global::PopupModuleId[]
		{
			global::PopupModuleId.POPUP_GENERIC_ANCHOR,
			global::PopupModuleId.POPUP_SCOUT
		});
		global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
		{
			global::PopupModuleId.POPUP_GENERIC_ANCHOR,
			global::PopupModuleId.POPUP_SCOUT
		});
		modulesPopup[0].GetComponent<global::ConfirmationPopupView>().Show("hideout_mission_scout_title", "hideout_mission_scout_desc", new global::System.Action<bool>(this.OnScoutConfirm), false, false);
		global::WarbandSave warbandSave = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave();
		int scoutCost = this.warband.GetScoutCost(this.priceDatas[warbandSave.scoutsSent]);
		modulesPopup[1].gameObject.GetComponent<global::UIDescription>().SetLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_cost_title"), scoutCost.ToString());
	}

	private void OnScoutConfirm(bool isConfirm)
	{
		if (isConfirm)
		{
			global::WarbandSave warbandSave = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave();
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(this.warband.GetScoutCost(this.priceDatas[warbandSave.scoutsSent]));
			warbandSave.scoutsSent++;
			global::Mission miss = global::PandoraSingleton<global::HideoutManager>.Instance.AddProcMission(true);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.AddPawns(global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count - 1);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::MissionModule>(global::ModuleId.MISSION).RefreshScoutButton();
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(warbandSave);
			this.ShowScoutedMission(miss);
		}
	}

	private void ShowScoutedMission(global::Mission miss)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.SMALL, new global::PopupModuleId[]
		{
			global::PopupModuleId.POPUP_GENERIC_ANCHOR,
			global::PopupModuleId.POPUP_SCOUTED_MISSION
		});
		global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
		{
			global::PopupModuleId.POPUP_GENERIC_ANCHOR,
			global::PopupModuleId.POPUP_SCOUTED_MISSION
		});
		global::PandoraDebug.LogDebug("module length = " + modulesPopup.Count, "uncategorised", null);
		global::ConfirmationPopupView component = modulesPopup[0].GetComponent<global::ConfirmationPopupView>();
		component.Show("hideout_mission_found_title", "hideout_mission_found_desc", null, false, false);
		component.HideCancelButton();
		global::WarbandSave warbandSave = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave();
		modulesPopup[1].gameObject.GetComponent<global::MissionDescModule>().SetupShort(miss);
	}

	private void CampNodeSelecteded(global::MenuNode node, int idx)
	{
		global::PandoraDebug.LogDebug("Node Selected", "uncategorised", null);
		this.selectedMission = 0;
		this.SelectMission(global::PandoraSingleton<global::HideoutManager>.Instance.missions[0]);
	}

	private void Dis1NodeSelecteded(global::MenuNode node, int idx)
	{
		global::PandoraDebug.LogDebug("Node Selected", "uncategorised", null);
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count; i++)
		{
			if (!global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].missionSave.isCampaign && global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].GetDistrictId() == global::DistrictId.MERCHANT && global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].missionSave.mapPosition == idx)
			{
				this.selectedMission = i;
				this.SelectMission(global::PandoraSingleton<global::HideoutManager>.Instance.missions[i]);
				break;
			}
		}
	}

	private void Dis2NodeSelecteded(global::MenuNode node, int idx)
	{
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count; i++)
		{
			if (!global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].missionSave.isCampaign && global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].GetDistrictId() == global::DistrictId.NOBLE && global::PandoraSingleton<global::HideoutManager>.Instance.missions[i].missionSave.mapPosition == idx)
			{
				this.selectedMission = i;
				this.SelectMission(global::PandoraSingleton<global::HideoutManager>.Instance.missions[i]);
				break;
			}
		}
	}

	private void NodeConfirmed(global::MenuNode node, int idx)
	{
		this.ConfirmMission(this.selectedMission);
	}

	private void NodeUnselect(global::MenuNode node, int idx)
	{
		global::PandoraDebug.LogDebug("Node Unselected", "uncategorised", null);
		if (!this.missionConfirm)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModule(false, new global::ModuleId[]
			{
				global::ModuleId.MISSION_DESC
			});
		}
	}

	private void SelectMission(global::Mission miss)
	{
		if (!this.missionConfirm)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModule(true, new global::ModuleId[]
			{
				global::ModuleId.MISSION_DESC
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::MissionDescModule>(global::ModuleId.MISSION_DESC).Setup(miss);
		}
	}

	private void ConfirmMission(int selectedMiss)
	{
		this.missionConfirm = true;
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.SWAP
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(false, new global::ModuleId[0]);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[0]);
		global::WarbandSwapModule moduleCenter = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSwapModule>(global::ModuleId.SWAP);
		moduleCenter.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband, new global::System.Action<bool>(this.OnMissionConfirm), true, global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission].missionSave.isCampaign, false, null, true, 0, 9999);
	}

	private void OnMissionConfirm(bool isConfirm)
	{
		if (isConfirm)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Selected Mission = ",
				this.selectedMission,
				" Missions Count = ",
				global::PandoraSingleton<global::HideoutManager>.Instance.missions.Count
			}), "uncategorised", null);
			global::MissionSave missionSave = global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission].missionSave;
			if (!missionSave.isCampaign && (missionSave.ratingId == 3 || missionSave.ratingId == 4))
			{
				bool flag = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lastMissionAmbushed;
				if (!flag)
				{
					int num = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 100);
					if (num < global::Constant.GetInt(global::ConstantId.AMBUSH_MISSION_PERC))
					{
						global::Mission value = global::Mission.GenerateAmbushMission(global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission]);
						global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission] = value;
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lastMissionAmbushed = flag;
				global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission].missionSave.autoDeploy = !global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSwapModule>(global::ModuleId.SWAP).DeployRequested;
			global::PandoraSingleton<global::MissionStartData>.Instance.Clear();
			global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(false);
			global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::PandoraSingleton<global::MissionStartData>.Instance.SetMissionFull(global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission], global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr, delegate
			{
				if (global::PandoraSingleton<global::HideoutManager>.Instance.missions[this.selectedMission].missionSave.isCampaign)
				{
					global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION_CAMPAIGN, false, false);
				}
				else
				{
					global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION, false, false);
				}
			}));
		}
		else
		{
			this.missionConfirm = false;
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.WARBAND_TABS,
				global::ModuleId.TITLE,
				global::ModuleId.NOTIFICATION
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.WARBAND_SHEET,
				global::ModuleId.TITLE,
				global::ModuleId.MISSION
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
			{
				global::ModuleId.TREASURY
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::MissionModule>(global::ModuleId.MISSION).Setup(new global::MissionModule.OnScoutButton(this.OnScoutButton));
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
			}, false, true);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		}
	}

	public void FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::System.Collections.Generic.List<global::MenuNode> nodes = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.nodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].IsSelectable && nodes[i].gameObject.activeSelf)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.map.SelectNode(nodes[i]);
					break;
				}
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.CAMPAIGN);
		}
	}

	private global::HideoutCamAnchor camAnchor;

	private global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject> pawns;

	private global::System.Collections.Generic.List<global::ScoutPriceData> priceDatas;

	private int selectedMission = -1;

	private global::Warband warband;

	private bool missionConfirm;

	private global::WarbandTabsModule warbandTabs;

	private bool once = true;
}
