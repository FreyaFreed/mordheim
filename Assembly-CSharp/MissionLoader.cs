using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionLoader : global::PandoraSingleton<global::MissionLoader>
{
	private int CurrentParts
	{
		get
		{
			return this.currentParts;
		}
		set
		{
			this.currentParts = value;
			this.UpdateLoadingProgress();
		}
	}

	private float CurrentPartsPercent
	{
		get
		{
			return this.currentPartsPercent;
		}
		set
		{
			this.currentPartsPercent = value;
			this.UpdateLoadingProgress();
		}
	}

	private void Awake()
	{
		global::PandoraSingleton<global::MissionLoader>.instance = this;
	}

	private void Start()
	{
		global::UnityEngine.Application.backgroundLoadingPriority = global::UnityEngine.ThreadPriority.High;
		this.totPropNodes = new global::System.Collections.Generic.List<global::PropNode>();
		this.totBuildingNodes = new global::System.Collections.Generic.List<global::BuildingNode>();
		this.jobs = new global::System.Collections.Generic.List<string>();
		this.parents = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::UnityEngine.Transform, global::UnityEngine.Transform>>();
		this.rotations = new global::System.Collections.Generic.List<int>();
		this.tempPropNodes = new global::System.Collections.Generic.List<global::PropNode>();
		this.tempBuildingNodes = new global::System.Collections.Generic.List<global::BuildingNode>();
		this.assetLibrary = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject>();
		this.groundLayers = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>(11);
		this.groundLayersOrder = new global::System.Collections.Generic.List<string>(11);
		this.startTime = global::UnityEngine.Time.realtimeSinceStartup;
		this.loadingPlanks = 0;
		this.loadingZoneAoe = 0;
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.GetComponent<global::UnityEngine.Camera>().enabled = false;
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.FIXED, null, false, true, true, false);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.dummyCam.transform.position = new global::UnityEngine.Vector3(1000f, 1000f, 1000f);
		global::PandoraSingleton<global::Hermes>.Instance.DoNotDisconnectMode = true;
		this.LoadMissionData();
		base.StartCoroutine(this.GenerateAllAsync());
	}

	private void UpdateLoadingProgress()
	{
		if (this.loadingPartsIndex >= this.LOADING_PARTS.Length)
		{
			this.percent = 100;
		}
		else
		{
			this.percent = global::UnityEngine.Mathf.FloorToInt(((float)this.CurrentParts + this.CurrentPartsPercent * (float)this.LOADING_PARTS[this.loadingPartsIndex]) / 353f * 100f);
		}
	}

	public global::System.Collections.IEnumerator GenerateAllAsync()
	{
		this.percent = 0;
		this.CurrentParts = 0;
		this.loadingPartsIndex = 0;
		this.GenerateLayersAsync();
		yield return base.StartCoroutine(this.WaitLayers());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.currentAssetbundleName = global::AssetBundleId.BUILDINGS_SCENE.ToLowerString();
		this.GenerateBuildingsAsync(this.ground);
		yield return base.StartCoroutine(this.CheckSceneJobs(new global::System.Action<global::UnityEngine.GameObject, int>(this.OnBuildingLoaded), null));
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.Unload(this.currentAssetbundleName);
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.currentAssetbundleName = global::AssetBundleId.PROPS_SCENE.ToLowerString();
		this.GeneratePropsAsync(this.ground);
		yield return base.StartCoroutine(this.CheckSceneJobs(new global::System.Action<global::UnityEngine.GameObject, int>(this.OnPropLoaded), null));
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.InitActionNodes();
		this.GenerateHangingNodes();
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		while (this.loadingPlanks > 0)
		{
			yield return null;
		}
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
		this.GenerateRestrictedPropsAsync();
		yield return base.StartCoroutine(this.CheckSceneJobs(new global::System.Action<global::UnityEngine.GameObject, int>(this.OnPropLoaded), null));
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		base.StartCoroutine(this.GenerateUnits());
		this.DestroyBuildingPropNodes();
		yield return base.StartCoroutine(this.BatchEnvironment());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.InitActionNodes();
		yield return base.StartCoroutine(this.CreateActionWeb());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		while (global::PandoraSingleton<global::UnitFactory>.instance.IsCreating())
		{
			this.CurrentPartsPercent = global::UnityEngine.Mathf.Max(this.CurrentPartsPercent, global::PandoraSingleton<global::UnitFactory>.Instance.LoadingPercent);
			yield return null;
		}
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
		this.GenerateTrapsAsync();
		yield return base.StartCoroutine(this.CheckSceneJobs(new global::System.Action<global::UnityEngine.GameObject, int>(this.OnTrapLoaded), new global::System.Action(this.OnTrapCleared)));
		this.TrapsPostProcess();
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.GenerateWyrdStonesAsync();
		yield return base.StartCoroutine(this.CheckSceneJobs(new global::System.Action<global::UnityEngine.GameObject, int>(this.OnWyrdstoneLoaded), new global::System.Action(this.OnWyrdstoneCleared)));
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.GenerateSearchAsync();
		yield return base.StartCoroutine(this.CheckSceneJobs(new global::System.Action<global::UnityEngine.GameObject, int>(this.OnSearchLoaded), null));
		this.SearchPostProcess();
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.GenerateReinforcements();
		while (global::PandoraSingleton<global::UnitFactory>.instance.IsCreating())
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.DeployUnits());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		yield return base.StartCoroutine(this.GenerateNavMesh());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.SetCameraSetter();
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.MissionReloadPostProcess();
		}
		while (this.loadingZoneAoe > 0)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.ReloadTraps());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		yield return base.StartCoroutine(this.ClearLoadingElements());
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		this.SetupObjectives();
		this.ReloadDestructibles();
		this.CurrentPartsPercent = 0f;
		this.CurrentParts += this.LOADING_PARTS[this.loadingPartsIndex++];
		yield return null;
		global::PandoraSingleton<global::MissionManager>.Instance.SendLoadingDone();
		global::UnityEngine.Debug.LogError("Loading Done! Total Time :" + (global::UnityEngine.Time.realtimeSinceStartup - this.startTime) + "s");
		global::UnityEngine.Application.backgroundLoadingPriority = global::UnityEngine.ThreadPriority.Normal;
		global::UnityEngine.Object.Destroy(this);
		yield break;
	}

	private void InitActionNodes()
	{
		this.aNodes.Clear();
		this.ground.GetComponentsInChildren<global::ActionNode>(this.aNodes);
		for (int i = 0; i < this.aNodes.Count; i++)
		{
			this.aNodes[i].Init();
		}
	}

	private void MissionReloadPostProcess()
	{
		global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, global::SearchSave>> searches = global::PandoraSingleton<global::MissionStartData>.Instance.searches;
		global::System.Collections.Generic.List<global::InteractivePoint> interactivePoints = global::PandoraSingleton<global::MissionManager>.Instance.interactivePoints;
		global::PandoraDebug.LogWarning("Interactive Points counts " + interactivePoints.Count, "uncategorised", null);
		for (int i = 0; i < searches.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < interactivePoints.Count; j++)
			{
				if (searches[i].Key < 200000000U && interactivePoints[j].guid == searches[i].Key)
				{
					global::SearchPoint searchPoint = (global::SearchPoint)interactivePoints[j];
					global::PandoraDebug.LogDebug("Search Point Name = " + searchPoint.name, "uncategorised", null);
					searchPoint.GetItemsAndClear();
					if (searches[i].Value != null)
					{
						for (int k = 0; k < searches[i].Value.items.Count; k++)
						{
							if (k >= searchPoint.items.Count)
							{
								searchPoint.AddItem(searches[i].Value.items[k]);
							}
							else
							{
								searchPoint.SetItem(searches[i].Value.items[k], k);
							}
							global::PandoraSingleton<global::MissionManager>.Instance.ResetItemOwnership(searchPoint.items[k], null);
						}
						searchPoint.wasSearched = searches[i].Value.wasSearched;
					}
					searchPoint.Refresh();
					searchPoint.Close(true);
					flag = true;
					break;
				}
			}
			if (!flag && searches[i].Value.items != null)
			{
				global::SearchPoint searchPoint2 = global::PandoraSingleton<global::MissionManager>.Instance.SpawnLootBag(global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(searches[i].Value.unitCtrlrUid), searches[i].Value.pos, searches[i].Value.items, true, searches[i].Value.wasSearched);
			}
		}
		global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, int>> converters = global::PandoraSingleton<global::MissionStartData>.Instance.converters;
		for (int l = 0; l < converters.Count; l++)
		{
			for (int m = 0; m < interactivePoints.Count; m++)
			{
				if (interactivePoints[m].guid == converters[l].Key)
				{
					((global::ConvertPoint)interactivePoints[m]).SetCapacity(converters[l].Value);
					((global::ConvertPoint)interactivePoints[m]).Close(false);
				}
			}
		}
		global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, bool>> activaters = global::PandoraSingleton<global::MissionStartData>.Instance.activaters;
		for (int n = 0; n < activaters.Count; n++)
		{
			for (int num = 0; num < interactivePoints.Count; num++)
			{
				if (interactivePoints[num].guid == activaters[n].Key)
				{
					((global::ActivatePoint)interactivePoints[num]).activated = !activaters[n].Value;
					((global::ActivatePoint)interactivePoints[num]).ActivateZoneAoe();
					((global::ActivatePoint)interactivePoints[num]).Activate(null, true);
				}
			}
		}
		global::System.Collections.Generic.List<global::EndZoneAoe> zones = global::PandoraSingleton<global::MissionStartData>.Instance.aoeZones;
		for (int num2 = 0; num2 < zones.Count; num2++)
		{
			int zoneIndex = num2;
			global::EndZoneAoe endZoneAoe = zones[zoneIndex];
			global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
			global::UnitController unit = null;
			for (int num3 = 0; num3 < allUnits.Count; num3++)
			{
				if (allUnits[num3].uid == endZoneAoe.myrtilusId)
				{
					unit = allUnits[num3];
				}
			}
			this.loadingZoneAoe++;
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/zone_aoe/", global::AssetBundleId.FX, endZoneAoe.aoeId.ToLowerString() + ".prefab", delegate(global::UnityEngine.Object prefab)
			{
				global::EndZoneAoe value = zones[zoneIndex];
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)prefab);
				gameObject.transform.position = value.position;
				gameObject.transform.rotation = global::UnityEngine.Quaternion.identity;
				global::ZoneAoe component = gameObject.GetComponent<global::ZoneAoe>();
				component.Init(value.aoeId, unit, value.radius, 1f);
				component.durationLeft = value.durationLeft;
				value.guid = component.guid;
				zones[zoneIndex] = value;
				this.loadingZoneAoe--;
			});
		}
		global::System.Collections.Generic.List<global::UnitController> allUnits2 = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int num4 = 0; num4 < allUnits2.Count; num4++)
		{
			global::System.Collections.Generic.List<global::Item> items = allUnits2[num4].unit.Items;
			for (int num5 = 0; num5 < items.Count; num5++)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.ResetItemOwnership(items[num5], allUnits2[num4]);
			}
		}
	}

	private global::System.Collections.IEnumerator BatchEnvironment()
	{
		global::MeshBatcher[] batchers = this.ground.GetComponentsInChildren<global::MeshBatcher>();
		global::UnityEngine.LODGroup.crossFadeAnimationDuration = 1.5f;
		float st = global::UnityEngine.Time.realtimeSinceStartup;
		for (int i = 0; i < batchers.Length; i++)
		{
			batchers[i].Batch();
			if ((double)(global::UnityEngine.Time.realtimeSinceStartup - st) > 0.01)
			{
				yield return null;
			}
			st = global::UnityEngine.Time.realtimeSinceStartup;
			this.CurrentPartsPercent = (float)i / (float)batchers.Length;
		}
		yield break;
	}

	private global::System.Collections.IEnumerator CheckSceneJobs(global::System.Action<global::UnityEngine.GameObject, int> loaded, global::System.Action clear)
	{
		int i = 0;
		while (i < this.jobs.Count)
		{
			global::UnityEngine.GameObject go = this.assetLibrary[this.jobs[i]];
			if (go != null)
			{
				global::UnityEngine.GameObject obj = this.InstantiateAsset(this.parents[i].Key, this.parents[i].Value, this.rotations[i], go, true);
				if (loaded != null)
				{
					loaded(obj, i);
				}
				i++;
			}
			else
			{
				yield return null;
			}
			this.CurrentPartsPercent = global::UnityEngine.Mathf.Max(this.CurrentPartsPercent, (float)(this.jobLoaded + 1) / (float)this.assetLibrary.Count);
		}
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
		this.jobs.Clear();
		this.parents.Clear();
		this.rotations.Clear();
		this.assetLibrary.Clear();
		if (clear != null)
		{
			clear();
		}
		this.jobLoaded = 0;
		yield break;
	}

	private void OnBuildingLoaded(global::UnityEngine.GameObject go, int index)
	{
		this.GenerateBuildingsAsync(go);
	}

	private void OnPropLoaded(global::UnityEngine.GameObject go, int index)
	{
		this.GeneratePropsAsync(go);
	}

	private void OnTrapLoaded(global::UnityEngine.GameObject go, int index)
	{
		global::Trap component = go.GetComponent<global::Trap>();
		component.Init(this.trapData[index].Key, global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		component.forceInactive = this.trapData[index].Value;
		this.traps.Add(component);
	}

	private void OnTrapCleared()
	{
		this.trapData.Clear();
	}

	private void OnWyrdstoneLoaded(global::UnityEngine.GameObject go, int index)
	{
		global::SearchPoint component = go.GetComponent<global::SearchPoint>();
		component.Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID(), 0, true);
		component.AddItem(this.wyrdTypeData[index].ItemId, global::ItemQualityId.NORMAL);
		if (go.name.Contains("outdoor"))
		{
			component.slots[0].restrictedItemId = this.wyrdTypeData[index].ItemId;
		}
	}

	private void OnWyrdstoneCleared()
	{
		this.wyrdTypeData.Clear();
	}

	private void OnSearchLoaded(global::UnityEngine.GameObject go, int index)
	{
		global::SearchPoint componentInChildren = go.GetComponentInChildren<global::SearchPoint>();
		global::SearchDensityLootData randomRatio = global::SearchDensityLootData.GetRandomRatio(this.rewards, this.networkTyche, null);
		int num = this.networkTyche.Rand(randomRatio.ItemMin, randomRatio.ItemMax + 1);
		componentInChildren.Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID(), num, false);
		this.FillSearchPoint(componentInChildren, randomRatio, this.lowestWarbandRank, num);
	}

	public void GenerateBuildingsAsync(global::UnityEngine.GameObject go)
	{
		go.GetComponentsInChildren<global::BuildingNode>(this.tempBuildingNodes);
		if (this.tempBuildingNodes != null)
		{
			this.totBuildingNodes.AddRange(this.tempBuildingNodes);
			for (int i = 0; i < this.tempBuildingNodes.Count; i++)
			{
				global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
				string field = "fk_building_type_id";
				int buildingType = (int)this.tempBuildingNodes[i].buildingType;
				global::System.Collections.Generic.List<global::BuildingTypeJoinBuildingData> datas = instance.InitData<global::BuildingTypeJoinBuildingData>(field, buildingType.ToString());
				global::BuildingTypeJoinBuildingData randomRatio = global::BuildingTypeJoinBuildingData.GetRandomRatio(datas, this.networkTyche, null);
				string assetName = randomRatio.BuildingId.ToLowerString();
				this.AddSceneJob(assetName, this.tempBuildingNodes[i].transform, (!randomRatio.Flippable) ? 0 : 180);
			}
			this.tempBuildingNodes.Clear();
		}
	}

	private global::UnityEngine.GameObject InstantiateAsset(global::UnityEngine.Transform parent, global::UnityEngine.Transform pos, int rot, global::UnityEngine.GameObject asset, bool instantiate = true)
	{
		global::UnityEngine.GameObject gameObject = null;
		global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
		global::UnityEngine.Quaternion quaternion = global::UnityEngine.Quaternion.identity;
		global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.one;
		if (asset != null)
		{
			if (pos != null)
			{
				global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.Euler(0f, (float)this.GetRandomRotation(rot), 0f);
				vector = pos.localPosition;
				quaternion = pos.localRotation * rhs;
				vector2 = pos.localScale;
			}
			if (instantiate)
			{
				gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(asset, vector, quaternion);
			}
			else
			{
				gameObject = asset;
				gameObject.transform.localPosition = vector;
				gameObject.transform.localRotation = quaternion;
			}
			if (parent != null)
			{
				gameObject.transform.SetParent(parent, false);
			}
			if (!global::PandoraUtils.Approximately(vector2, global::UnityEngine.Vector3.one))
			{
				gameObject.transform.localScale = vector2;
			}
			gameObject.isStatic = true;
			gameObject.SetActive(true);
		}
		return gameObject;
	}

	private int GetRandomRotation(int rot)
	{
		int num = 0;
		if (rot != 0)
		{
			num = 360 / rot;
		}
		return this.networkTyche.Rand(0, num + 1) * rot;
	}

	private void GenerateLayersAsync()
	{
		global::MissionMapData missionMapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)this.deployMapData.MissionMapId);
		string name = missionMapData.Name;
		this.currentAssetbundleName = name + "_scene";
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsync(this.currentAssetbundleName);
		this.LoadLayerAsync(name, this.currentAssetbundleName);
		this.LoadLayerAsync(this.mapLayout.LightsName, this.currentAssetbundleName);
		this.LoadLayerAsync(this.mapLayout.FxName, this.currentAssetbundleName);
		if (missionMapData.HasRecastHelper)
		{
			string layerName = name + "_recast_helper";
			this.LoadLayerAsync(layerName, this.currentAssetbundleName);
		}
		this.LoadLayerAsync(this.deployMapData.PropsLayer, this.currentAssetbundleName);
		this.LoadLayerAsync(this.deployMapData.DeploymentLayer, this.currentAssetbundleName);
		this.LoadLayerAsync(this.deployMapData.TrapsLayer, this.currentAssetbundleName);
		this.LoadLayerAsync(this.deployMapData.SearchLayer, this.currentAssetbundleName);
		if (!string.IsNullOrEmpty(this.deployMapData.ExtraLightsFxLayer))
		{
			this.LoadLayerAsync(this.deployMapData.ExtraLightsFxLayer, this.currentAssetbundleName);
		}
		if (this.mapGameplay.Id != global::MissionMapGameplayId.NONE)
		{
			this.LoadLayerAsync(this.mapGameplay.Name, this.currentAssetbundleName);
		}
		if (this.mapLayout.CloudsName.Contains("dis_00"))
		{
			this.LoadLayerAsync(this.mapLayout.CloudsName, "grnd_dis_00_proc_00_scene");
		}
		else
		{
			this.LoadLayerAsync(this.mapLayout.CloudsName, this.currentAssetbundleName);
		}
	}

	private void LoadLayerAsync(string layerName, string assetBundle)
	{
		this.groundLayersOrder.Add(layerName);
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadSceneAssetAsync(layerName, assetBundle, delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject item = (global::UnityEngine.GameObject)go;
			item = global::UnityEngine.SceneManagement.SceneManager.GetSceneByName(layerName).GetRootGameObjects()[0];
			this.groundLayers.Add(item);
		});
	}

	private global::System.Collections.IEnumerator WaitLayers()
	{
		while (this.groundLayers.Count != this.groundLayersOrder.Count)
		{
			this.CurrentPartsPercent = (float)this.groundLayers.Count / (float)this.groundLayersOrder.Count;
			yield return null;
		}
		for (int i = this.groundLayers.Count - 1; i >= 0; i--)
		{
			if (this.groundLayers[i].name == this.groundLayersOrder[0])
			{
				this.ground = this.groundLayers[i];
				global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.ground, global::UnityEngine.SceneManagement.SceneManager.GetActiveScene());
				this.groundLayers.RemoveAt(i);
				break;
			}
		}
		for (int j = 1; j < this.groundLayersOrder.Count; j++)
		{
			for (int k = 0; k < this.groundLayers.Count; k++)
			{
				if (this.groundLayersOrder[j] == this.groundLayers[k].name)
				{
					this.groundLayers[k].transform.SetParent(this.ground.transform);
					this.groundLayers.RemoveAt(k);
					break;
				}
			}
		}
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
		this.groundLayersOrder.Clear();
		this.groundLayers.Clear();
		global::PandoraSingleton<global::MissionManager>.Instance.mapContour = this.ground.GetComponentInChildren<global::MapContour>();
		global::PandoraSingleton<global::MissionManager>.Instance.mapOrigin = this.ground.GetComponentInChildren<global::MapOrigin>();
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.Unload("grnd_dis_00_proc_00_scene");
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsync(this.currentAssetbundleName);
		yield break;
	}

	private global::PropData GetRandomProp(global::PropTypeId propTypeId)
	{
		global::System.Collections.Generic.List<global::PropTypeJoinPropData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PropTypeJoinPropData>("fk_prop_type_id", propTypeId.ToIntString<global::PropTypeId>());
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PropData>((int)list[this.networkTyche.Rand(0, list.Count)].PropId);
	}

	private void GeneratePropsAsync(global::UnityEngine.GameObject parent)
	{
		parent.GetComponentsInChildren<global::PropNode>(true, this.tempPropNodes);
		this.totPropNodes.AddRange(this.tempPropNodes);
		for (int i = 0; i < this.tempPropNodes.Count; i++)
		{
			if (!this.AddIfRestricted(this.tempPropNodes[i]))
			{
				global::PropTypeData propTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PropTypeData>((int)this.tempPropNodes[i].propType);
				global::PropData randomProp = this.GetRandomProp(propTypeData.Id);
				this.AddSceneJob(randomProp.Name, this.tempPropNodes[i].transform, 0);
			}
		}
		this.tempPropNodes.Clear();
	}

	private void AddSceneJob(string assetName, global::UnityEngine.Transform nodeTransform, int rotation = 0)
	{
		this.AddSceneJob(assetName, nodeTransform.parent, nodeTransform, rotation);
	}

	private void AddSceneJob(string assetName, global::UnityEngine.Transform parentTransform, global::UnityEngine.Transform nodeTransform, int rotation = 0)
	{
		if (!this.assetLibrary.ContainsKey(assetName))
		{
			this.assetLibrary[assetName] = null;
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadSceneAssetAsync(assetName, this.currentAssetbundleName, delegate(global::UnityEngine.Object prefab)
			{
				global::UnityEngine.GameObject value = (global::UnityEngine.GameObject)prefab;
				global::UnityEngine.SceneManagement.Scene sceneByName = global::UnityEngine.SceneManagement.SceneManager.GetSceneByName(assetName);
				if (false || !sceneByName.IsValid())
				{
					return;
				}
				value = sceneByName.GetRootGameObjects()[0];
				this.assetLibrary[assetName] = value;
				this.jobLoaded++;
			});
		}
		this.jobs.Add(assetName);
		this.parents.Add(new global::System.Collections.Generic.KeyValuePair<global::UnityEngine.Transform, global::UnityEngine.Transform>(parentTransform, nodeTransform));
		this.rotations.Add(rotation);
	}

	private void GenerateRestrictedPropsAsync()
	{
		if (this.restrictions != null)
		{
			for (int i = 0; i < this.restrictions.Count; i++)
			{
				global::PropRestrictions propRestrictions = this.restrictions[i];
				if (propRestrictions.props.Count > 0)
				{
					float num = (float)(propRestrictions.restrictionData.MinDistance * propRestrictions.restrictionData.MinDistance);
					int count = propRestrictions.props.Count;
					global::System.Collections.Generic.List<global::UnityEngine.Transform> list = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();
					float num2 = (float)(list.Count * 100 / count);
					int maxProp = propRestrictions.restrictionData.MaxProp;
					int maxPercentage = propRestrictions.restrictionData.MaxPercentage;
					while (propRestrictions.props.Count > 0 && list.Count < maxProp && num2 < (float)maxPercentage)
					{
						int index = this.networkTyche.Rand(0, propRestrictions.props.Count);
						if (propRestrictions.props[index] == null)
						{
							propRestrictions.props.RemoveAt(index);
						}
						else
						{
							global::UnityEngine.Transform transform = propRestrictions.props[index].transform;
							bool flag = false;
							int num3 = 0;
							while (num3 < list.Count && !flag && num > 0.1f)
							{
								if (num > global::UnityEngine.Vector3.SqrMagnitude(transform.position - list[num3].position))
								{
									flag = true;
									propRestrictions.props.RemoveAt(index);
								}
								num3++;
							}
							if (!flag)
							{
								global::PropTypeData propTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PropTypeData>((int)propRestrictions.restrictionData.PropTypeId);
								global::PropData randomProp = this.GetRandomProp(propTypeData.Id);
								this.AddSceneJob(randomProp.Name, transform, 0);
								propRestrictions.props.RemoveAt(index);
								list.Add(transform);
								num2 = (float)(list.Count * 100 / count);
							}
						}
					}
				}
			}
		}
	}

	private global::System.Collections.IEnumerator ClearLoadingElements()
	{
		while (global::PandoraSingleton<global::AssetBundleLoader>.Instance.IsLoading)
		{
			yield return null;
		}
		global::PandoraDebug.LogDebug("ClearLoadingElements 1", "LOADING", this);
		global::UnityEngine.GameObject[] gos = global::UnityEngine.GameObject.FindGameObjectsWithTag("loading");
		for (int goIndex = 0; goIndex < gos.Length; goIndex++)
		{
			global::UnityEngine.Object.DestroyImmediate(gos[goIndex]);
		}
		global::PandoraDebug.LogDebug("ClearLoadingElements 2", "LOADING", this);
		yield return base.StartCoroutine(global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadAll(false));
		global::PandoraDebug.LogDebug("ClearLoadingElements Finished!", "LOADING", this);
		yield break;
	}

	private void LoadMissionData()
	{
		this.missionSave = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave;
		global::PandoraSingleton<global::MissionManager>.Instance.campaignId = (global::CampaignMissionId)this.missionSave.campaignId;
		this.networkTyche = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche;
		this.lowestWarbandRank = -1;
		for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count; i++)
		{
			global::MissionWarbandSave missionWarbandSave = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[i];
			if (missionWarbandSave.Rank > 0 && (this.lowestWarbandRank == -1 || missionWarbandSave.Rank < this.lowestWarbandRank))
			{
				this.lowestWarbandRank = missionWarbandSave.Rank;
			}
		}
		this.lowestWarbandRank = global::UnityEngine.Mathf.Max(0, this.lowestWarbandRank);
		if (this.ground == null)
		{
			this.deployMapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(this.missionSave.deployScenarioMapLayoutId);
			this.restrictions = new global::System.Collections.Generic.List<global::PropRestrictions>();
			this.AddPropsRestriction(this.deployMapData.PropRestrictionIdBarricade);
			this.AddPropsRestriction(this.deployMapData.PropRestrictionIdMadstuff);
			this.AddPropsRestriction(this.deployMapData.PropRestrictionIdProps);
			this.mapLayout = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapLayoutData>(this.missionSave.mapLayoutId);
			this.mapGameplay = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapGameplayData>(this.missionSave.mapGameplayId);
			this.trapCount = this.deployMapData.TrapCount;
			global::PandoraSingleton<global::MissionManager>.Instance.mapData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MissionMapData>((int)this.deployMapData.MissionMapId);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.SetTurnTimer((float)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.turnTimer, null);
		global::PandoraSingleton<global::MissionManager>.Instance.SetBeaconLimit(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.beaconLimit);
		if (this.missionSave.isCampaign)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.SetRichPresence((!this.missionSave.isTuto) ? global::Hephaestus.RichPresenceId.CAMPAIGN_MISSION : global::Hephaestus.RichPresenceId.TUTORIAL_MISSION, true);
		}
		else if (this.missionSave.isSkirmish)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.AI)
			{
				global::PandoraSingleton<global::Hephaestus>.instance.SetRichPresence(global::Hephaestus.RichPresenceId.EXHIBITION_AI, true);
			}
			else
			{
				global::PandoraSingleton<global::Hephaestus>.instance.SetRichPresence(global::Hephaestus.RichPresenceId.EXHIBITION_PLAYER, true);
			}
		}
		else if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].PlayerTypeId == global::PlayerTypeId.AI)
		{
			global::PandoraSingleton<global::Hephaestus>.instance.SetRichPresence(global::Hephaestus.RichPresenceId.PROC_MISSION, true);
		}
		else
		{
			global::PandoraSingleton<global::Hephaestus>.instance.SetRichPresence(global::Hephaestus.RichPresenceId.CONTEST, true);
		}
	}

	private void AddPropsRestriction(global::PropRestrictionId propsRestrictId)
	{
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_prop_restriction_id";
		int num = (int)propsRestrictId;
		global::System.Collections.Generic.List<global::PropRestrictionJoinPropTypeData> list = instance.InitData<global::PropRestrictionJoinPropTypeData>(field, num.ToString());
		for (int i = 0; i < list.Count; i++)
		{
			global::PropRestrictionJoinPropTypeData data = list[i];
			this.restrictions.Add(new global::PropRestrictions(data));
		}
	}

	private global::System.Collections.Generic.List<global::SpawnNode> GetSpawnNodes()
	{
		global::System.Collections.Generic.List<global::SpawnNode> list = new global::System.Collections.Generic.List<global::SpawnNode>();
		list.AddRange(this.ground.GetComponentsInChildren<global::SpawnNode>());
		return list;
	}

	private bool AddIfRestricted(global::PropNode node)
	{
		if (this.restrictions != null)
		{
			for (int i = 0; i < this.restrictions.Count; i++)
			{
				global::PropRestrictions propRestrictions = this.restrictions[i];
				if (node.propType == propRestrictions.restrictionData.PropTypeId)
				{
					propRestrictions.props.Add(node.gameObject);
					return true;
				}
			}
		}
		return false;
	}

	private void GenerateHangingNodes()
	{
		global::HangingNode[] componentsInChildren = this.ground.GetComponentsInChildren<global::HangingNode>();
		global::PandoraDebug.LogInfo("Hangings Nodes = " + componentsInChildren.Length, "PROCEDURAL", null);
		global::System.Collections.Generic.List<int>[] array = new global::System.Collections.Generic.List<int>[componentsInChildren.Length];
		bool[] array2 = new bool[componentsInChildren.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new global::System.Collections.Generic.List<int>();
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (!array2[j])
			{
				global::HangingNode hangingNode = componentsInChildren[j];
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					if (!array2[k])
					{
						global::HangingNode hangingNode2 = componentsInChildren[k];
						float num = global::UnityEngine.Vector3.SqrMagnitude(hangingNode2.transform.position - hangingNode.transform.position);
						if (num < 4f && global::UnityEngine.Vector3.Dot(hangingNode2.transform.forward, hangingNode.transform.forward) < -0.98f)
						{
							global::PandoraDebug.LogInfo(string.Concat(new object[]
							{
								"Skip Hanging Nodes! ",
								j,
								" pos = ",
								hangingNode.transform.position,
								" and ",
								k,
								" pos ",
								hangingNode2.transform.position,
								"Distance ",
								num
							}), "PROCEDURAL", null);
							array2[k] = true;
							array2[j] = true;
						}
					}
				}
			}
		}
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			this.CurrentPartsPercent = (float)l / (float)componentsInChildren.Length;
			if (!array2[l])
			{
				global::HangingNode hangingNode3 = componentsInChildren[l];
				for (int m = 0; m < componentsInChildren.Length; m++)
				{
					if (l != m && !array2[m])
					{
						global::HangingNode hangingNode4 = componentsInChildren[m];
						if (hangingNode4.isPlank == hangingNode3.isPlank)
						{
							if (!hangingNode3.isPlank)
							{
								bool flag = true;
								int num2 = 0;
								while (num2 < array[m].Count && flag)
								{
									if (array[m][num2] == l)
									{
										flag = false;
									}
									num2++;
								}
								int num3 = 0;
								while (num3 < array[l].Count && flag)
								{
									if (array[l][num3] == m)
									{
										flag = false;
									}
									num3++;
								}
								if (!flag)
								{
									goto IL_68F;
								}
							}
							global::UnityEngine.Vector3 vector = hangingNode4.transform.position - hangingNode3.transform.position;
							float num4 = global::UnityEngine.Vector3.SqrMagnitude(vector);
							if (global::UnityEngine.Vector3.SqrMagnitude(hangingNode4.transform.position - (hangingNode3.transform.position + hangingNode3.transform.forward)) <= num4)
							{
								if ((double)num4 > 20.25 && (((double)num4 < 182.25 && hangingNode3.isPlank) || (double)num4 < 90.25) && global::UnityEngine.Mathf.Abs(vector.y) < 0.5f && hangingNode4.transform.forward == -hangingNode3.transform.forward)
								{
									if (hangingNode3.isPlank)
									{
										if (array[m].Count > 0 || array[l].Count > 0)
										{
											goto IL_68F;
										}
										global::UnityEngine.RaycastHit raycastHit = default(global::UnityEngine.RaycastHit);
										if (!global::PandoraUtils.RectCast(hangingNode3.transform.position + new global::UnityEngine.Vector3(0f, 0.75f, 0f), vector, vector.magnitude + 0.5f, 1.25f, 1.25f, global::LayerMaskManager.groundMask, null, out raycastHit, true))
										{
											goto IL_68F;
										}
										float num5 = global::UnityEngine.Vector3.Angle(hangingNode4.transform.forward, vector);
										if (num5 < 145f)
										{
											goto IL_68F;
										}
										global::UnityEngine.RaycastHit[] array3 = global::UnityEngine.Physics.RaycastAll(hangingNode3.transform.position + global::UnityEngine.Vector3.up * 0.5f, vector, vector.magnitude, 1 << global::UnityEngine.LayerMask.NameToLayer("ignore_raycast") | 1 << global::UnityEngine.LayerMask.NameToLayer("collision_wall"));
										if (array3.Length != 2)
										{
											goto IL_68F;
										}
									}
									else if (global::UnityEngine.Physics.SphereCast(new global::UnityEngine.Ray(hangingNode3.transform.position, vector), 0.15f, vector.magnitude, global::LayerMaskManager.groundMask))
									{
										goto IL_68F;
									}
									float scale = 1f;
									if (hangingNode3.isPlank)
									{
										string text;
										if ((double)num4 < 56.25)
										{
											text = "woodplank_dis_01_6m_01";
											if ((double)num4 > 42.25)
											{
												scale = 1.1f;
											}
										}
										else if ((double)num4 < 110.25)
										{
											text = "woodplank_dis_01_9m_01";
											if ((double)num4 > 90.25)
											{
												scale = 1.1f;
											}
										}
										else
										{
											text = "woodplank_dis_01_12m_01";
											if ((double)num4 > 156.25)
											{
												scale = 1.1f;
											}
										}
										global::PandoraDebug.LogInfo("Plank Loaded = " + text, "PROCEDURAL", null);
										this.LoadPlank(text, hangingNode3.transform.position, scale, vector);
									}
									else
									{
										global::PropTypeId id;
										if ((double)num4 < 30.25)
										{
											id = global::PropTypeId.GARLAND_DIS_00_5M;
										}
										else if ((double)num4 < 42.25)
										{
											id = global::PropTypeId.GARLAND_DIS_00_6M;
										}
										else if ((double)num4 < 56.25)
										{
											id = global::PropTypeId.GARLAND_DIS_00_7M;
										}
										else if ((double)num4 < 72.25)
										{
											id = global::PropTypeId.GARLAND_DIS_00_8M;
										}
										else
										{
											id = global::PropTypeId.GARLAND_DIS_00_9M;
										}
										global::PropTypeData propTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PropTypeData>((int)id);
										global::PropData randomProp = this.GetRandomProp(propTypeData.Id);
										string text = randomProp.Name;
										global::PandoraDebug.LogInfo("Garland Loaded = " + text, "PROCEDURAL", null);
										this.LoadPlank(text, hangingNode3.transform.position, 1f, vector);
									}
									array[l].Add(m);
									array[m].Add(l);
									break;
								}
							}
						}
					}
					IL_68F:;
				}
			}
		}
		for (int n = 0; n < componentsInChildren.Length; n++)
		{
			if (componentsInChildren[n].isPlank || array[n].Count == 0)
			{
				global::UnityEngine.Object.Destroy(componentsInChildren[n].gameObject);
			}
		}
	}

	private void LoadPlank(string goName, global::UnityEngine.Vector3 position, float scale, global::UnityEngine.Vector3 distV)
	{
		this.loadingPlanks++;
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadSceneAssetAsync(goName, global::AssetBundleId.PROPS_SCENE.ToLowerString(), delegate(global::UnityEngine.Object go)
		{
			this.loadingPlanks--;
			go = global::UnityEngine.SceneManagement.SceneManager.GetSceneByName(goName).GetRootGameObjects()[0];
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)go);
			gameObject.transform.position = position + distV / 2f;
			gameObject.transform.SetParent(this.ground.transform);
			gameObject.transform.rotation = global::UnityEngine.Quaternion.FromToRotation(gameObject.transform.right, distV);
			global::UnityEngine.Vector3 one = global::UnityEngine.Vector3.one;
			one.x = scale;
			gameObject.transform.localScale = one;
		});
	}

	private void DestroyBuildingPropNodes()
	{
		if (this.totPropNodes != null)
		{
			for (int i = this.totPropNodes.Count - 1; i >= 0; i--)
			{
				global::UnityEngine.Object.DestroyImmediate(this.totPropNodes[i].gameObject);
			}
		}
		if (this.totBuildingNodes != null)
		{
			for (int j = this.totBuildingNodes.Count - 1; j >= 0; j--)
			{
				global::UnityEngine.Object.DestroyImmediate(this.totBuildingNodes[j].gameObject);
			}
		}
	}

	private global::System.Collections.IEnumerator CreateActionWeb()
	{
		global::ActionZone[] aZones = this.ground.GetComponentsInChildren<global::ActionZone>();
		global::ActionZoneChecker[] aZonesCheck = this.ground.GetComponentsInChildren<global::ActionZoneChecker>();
		for (int i = 0; i < aZones.Length; i++)
		{
			aZones[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < aZonesCheck.Length; j++)
		{
			aZonesCheck[j].toDestroy = false;
			aZonesCheck[j].gameObject.SetActive(false);
		}
		yield return new global::UnityEngine.WaitForFixedUpdate();
		yield return new global::UnityEngine.WaitForFixedUpdate();
		for (int k = 0; k < aZones.Length; k++)
		{
			aZones[k].gameObject.SetActive(true);
		}
		for (int l = 0; l < aZonesCheck.Length; l++)
		{
			aZonesCheck[l].gameObject.SetActive(true);
		}
		yield return new global::UnityEngine.WaitForFixedUpdate();
		yield return new global::UnityEngine.WaitForFixedUpdate();
		if (aZones.Length > this.actionZones.Count)
		{
			this.actionZones.Clear();
			this.actionZones.AddRange(aZones);
		}
		for (int m = 0; m < aZonesCheck.Length; m++)
		{
			aZonesCheck[m].Check();
		}
		global::PandoraDebug.LogInfo("CreateActionWeb Zones = " + this.actionZones.Count, "PROCEDURAL", null);
		this.linkCount = new int[this.actionZones.Count];
		this.linkedCount = new int[this.actionZones.Count];
		for (int n = 0; n < this.actionZones.Count; n++)
		{
			this.linkCount[n] = 0;
			this.linkedCount[n] = 0;
		}
		for (int i2 = 0; i2 < this.actionZones.Count; i2++)
		{
			global::ActionZone myZone = this.actionZones[i2];
			global::ActionZoneChecker check = myZone.GetComponentInChildren<global::ActionZoneChecker>();
			if (!check.toDestroy)
			{
				if (myZone.largeOccupation && myZone.largeOccupation.Occupation > 0)
				{
					myZone.supportLargeUnit = false;
					global::UnityEngine.Object.Destroy(myZone.largeOccupation.gameObject);
					myZone.largeOccupation = null;
				}
				for (int j2 = 0; j2 < this.actionZones.Count; j2++)
				{
					if (i2 != j2)
					{
						global::ActionZone checkZone = this.actionZones[j2];
						check = checkZone.GetComponentInChildren<global::ActionZoneChecker>();
						if (checkZone.largeOccupation && checkZone.largeOccupation.Occupation > 0)
						{
							checkZone.supportLargeUnit = false;
							global::UnityEngine.Object.Destroy(checkZone.largeOccupation.gameObject);
							checkZone.largeOccupation = null;
						}
						bool pass = true;
						for (int destIndex = 0; destIndex < myZone.destinations.Count; destIndex++)
						{
							global::ActionDestination destination = myZone.destinations[destIndex];
							float dist = global::UnityEngine.Vector3.SqrMagnitude(destination.destination.transform.position - checkZone.transform.position);
							if ((double)dist < 0.0625)
							{
								pass = false;
							}
						}
						if (pass)
						{
							float yDist = checkZone.transform.position.y - myZone.transform.position.y;
							float xDist = global::UnityEngine.Vector3.SqrMagnitude(new global::UnityEngine.Vector3(myZone.transform.position.x, 0f, myZone.transform.position.z) - new global::UnityEngine.Vector3(checkZone.transform.position.x, 0f, checkZone.transform.position.z));
							if (xDist < 3.23999977f && yDist < 1E-05f && global::UnityEngine.Vector3.Dot(checkZone.transform.forward, myZone.transform.forward) < -0.98f && global::UnityEngine.Vector3.SqrMagnitude(checkZone.transform.position - (myZone.transform.position + myZone.transform.forward)) < xDist)
							{
								check.toDestroy = true;
								check = myZone.GetComponentInChildren<global::ActionZoneChecker>();
								check.toDestroy = true;
							}
							if (!check.toDestroy)
							{
								if (xDist <= 19.65f)
								{
									if (yDist < -1f && xDist < 3f && checkZone.transform.forward == -myZone.transform.forward)
									{
										global::UnityEngine.Vector3 rayDir = checkZone.transform.position - myZone.transform.position;
										global::UnityEngine.Vector3 rayDirH = rayDir;
										rayDirH.y = 0f;
										global::UnityEngine.Vector3 rayDirV = rayDir;
										rayDirV.z = 0f;
										rayDirV.x = 0f;
										global::UnityEngine.LayerMask rayLayers = default(global::UnityEngine.LayerMask);
										rayLayers = (1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground"));
										global::UnityEngine.RaycastHit hit = default(global::UnityEngine.RaycastHit);
										bool horizontal = global::UnityEngine.Physics.CapsuleCast(myZone.transform.position + new global::UnityEngine.Vector3(0f, 1f, 0f), myZone.transform.position + new global::UnityEngine.Vector3(0f, 1.25f, 0f), 0.5f, rayDirH, out hit, rayDirH.magnitude + 0.5f, rayLayers);
										bool vertical = global::UnityEngine.Physics.CapsuleCast(new global::UnityEngine.Vector3(checkZone.transform.position.x, myZone.transform.position.y + 1f, checkZone.transform.position.z), new global::UnityEngine.Vector3(checkZone.transform.position.x, myZone.transform.position.y + 1.75f, checkZone.transform.position.z), 0.5f, rayDirV, out hit, rayDirV.magnitude + 0.5f, rayLayers);
										if (!horizontal && !vertical)
										{
											global::ActionDestination destination2 = new global::ActionDestination();
											int magnitude = global::UnityEngine.Mathf.Abs((int)yDist);
											destination2.actionId = ((magnitude > 3) ? ((magnitude > 6) ? global::UnitActionId.JUMP_9M : global::UnitActionId.JUMP_6M) : global::UnitActionId.JUMP_3M);
											destination2.destination = checkZone;
											this.LoadAthleticFx(destination2, "fx_jump_01", i2);
											myZone.destinations.Add(destination2);
											this.linkedCount[j2]++;
											global::UnityEngine.Vector3 rayPos = checkZone.transform.position;
											rayPos += global::UnityEngine.Vector3.up;
											global::UnityEngine.LayerMask climbLayer = default(global::UnityEngine.LayerMask);
											climbLayer = 1 << global::UnityEngine.LayerMask.NameToLayer("loading");
											bool canClimb = true;
											int climbIndex = 0;
											while (climbIndex < magnitude / 3 && canClimb)
											{
												global::UnityEngine.Vector3 checkRay = rayPos + new global::UnityEngine.Vector3(0f, (float)(climbIndex * 3), 0f);
												canClimb = global::UnityEngine.Physics.Raycast(checkRay, -rayDirH, 2f, climbLayer);
												climbIndex++;
											}
											if (canClimb)
											{
												destination2 = new global::ActionDestination();
												destination2.destination = myZone;
												destination2.actionId = ((magnitude > 3) ? ((magnitude > 6) ? global::UnitActionId.CLIMB_9M : global::UnitActionId.CLIMB_6M) : global::UnitActionId.CLIMB_3M);
												this.LoadAthleticFx(destination2, "fx_climb_01", j2);
												checkZone.destinations.Add(destination2);
												this.linkedCount[i2]++;
											}
										}
									}
								}
							}
						}
					}
				}
				this.linkCount[i2] = myZone.destinations.Count;
			}
		}
		for (int i3 = 0; i3 < this.actionZones.Count; i3++)
		{
			if (this.linkCount[i3] == 0 && this.linkedCount[i3] == 0)
			{
				global::ActionZoneChecker check2 = this.actionZones[i3].GetComponentInChildren<global::ActionZoneChecker>();
				if (check2.toDestroy)
				{
					global::UnityEngine.Object.Destroy(this.actionZones[i3].transform.parent.gameObject);
				}
				else
				{
					global::UnityEngine.Object.Destroy(this.actionZones[i3].gameObject);
				}
			}
			else
			{
				global::ActionZone myZone2 = this.actionZones[i3];
				foreach (global::UnityEngine.Rigidbody body in myZone2.gameObject.GetComponentsInChildren<global::UnityEngine.Rigidbody>())
				{
					global::UnityEngine.Object.Destroy(body);
				}
				foreach (global::ActionZoneChecker check3 in myZone2.gameObject.GetComponentsInChildren<global::ActionZoneChecker>())
				{
					global::UnityEngine.Object.Destroy(check3);
				}
				if (myZone2.largeOccupation != null)
				{
					global::UnityEngine.Object.Destroy(myZone2.largeOccupation.GetComponent<global::UnityEngine.Rigidbody>());
				}
				bool pass2 = false;
				for (int k2 = 0; k2 < myZone2.destinations.Count; k2++)
				{
					if (myZone2.destinations[k2].actionId == global::UnitActionId.JUMP_3M || myZone2.destinations[k2].actionId == global::UnitActionId.JUMP_6M || myZone2.destinations[k2].actionId == global::UnitActionId.JUMP_9M)
					{
						pass2 = true;
						break;
					}
				}
				int j3 = 0;
				while (pass2 && j3 < this.actionZones.Count)
				{
					if (i3 != j3)
					{
						if (this.linkCount[j3] != 0 || this.linkedCount[j3] != 0)
						{
							global::ActionZone checkZone2 = this.actionZones[j3];
							float yDist2 = checkZone2.transform.position.y - myZone2.transform.position.y;
							float xDist2 = global::UnityEngine.Vector3.SqrMagnitude(new global::UnityEngine.Vector3(myZone2.transform.position.x, 0f, myZone2.transform.position.z) - new global::UnityEngine.Vector3(checkZone2.transform.position.x, 0f, checkZone2.transform.position.z));
							if (xDist2 <= 19.65f)
							{
								if (global::UnityEngine.Vector3.SqrMagnitude(checkZone2.transform.position - (myZone2.transform.position + myZone2.transform.forward)) <= xDist2)
								{
									if (yDist2 == 0f && xDist2 < 19.65f && xDist2 > 15.65f && checkZone2.transform.forward == -myZone2.transform.forward)
									{
										global::UnityEngine.Vector3 rayDir2 = checkZone2.transform.position - myZone2.transform.position;
										global::UnityEngine.Vector3 rayDirH2 = rayDir2;
										rayDirH2.y = 0f;
										global::UnityEngine.LayerMask rayLayers2 = default(global::UnityEngine.LayerMask);
										rayLayers2 = (1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground"));
										global::UnityEngine.RaycastHit hit2 = default(global::UnityEngine.RaycastHit);
										if (!global::UnityEngine.Physics.CapsuleCast(myZone2.transform.position + new global::UnityEngine.Vector3(0f, 1f, 0f), myZone2.transform.position + new global::UnityEngine.Vector3(0f, 2f, 0f), 0.5f, rayDirH2, out hit2, rayDirH2.magnitude, rayLayers2))
										{
											global::ActionDestination destination3 = new global::ActionDestination();
											destination3.actionId = global::UnitActionId.LEAP;
											destination3.destination = checkZone2;
											this.LoadAthleticFx(destination3, "fx_leap_01", i3);
											myZone2.destinations.Add(destination3);
											this.linkedCount[j3]++;
										}
									}
								}
							}
						}
					}
					j3++;
				}
				this.linkCount[i3] = myZone2.destinations.Count;
				if (this.linkCount[i3] == 0)
				{
					myZone2.transform.parent.gameObject.SetActive(false);
				}
				else
				{
					myZone2.Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
				}
			}
		}
		global::UnityEngine.GameObject linksAnchor = new global::UnityEngine.GameObject("linksAnchor");
		global::PandoraSingleton<global::MissionManager>.instance.actionZones = this.actionZones;
		global::PandoraSingleton<global::MissionManager>.Instance.nodeLinks = new global::System.Collections.Generic.List<global::Pathfinding.NodeLink2>();
		if (this.actionZones.Count > 0)
		{
			for (int i4 = 0; i4 < this.actionZones.Count; i4++)
			{
				if (this.linkCount[i4] > 0)
				{
					global::ActionZone zone = this.actionZones[i4];
					for (int destIndex2 = 0; destIndex2 < zone.destinations.Count; destIndex2++)
					{
						global::ActionDestination dest = zone.destinations[destIndex2];
						this.AddNodeLink(linksAnchor, zone.transform, dest.destination.transform, dest);
					}
				}
			}
		}
		foreach (global::Teleporter tel in this.ground.GetComponentsInChildren<global::Teleporter>())
		{
			this.AddNodeLink(linksAnchor, tel.transform, tel.exit.transform, null);
		}
		global::DecisionPoint[] decisions = this.ground.GetComponentsInChildren<global::DecisionPoint>();
		for (int i6 = 0; i6 < decisions.Length; i6++)
		{
			decisions[i6].Register();
		}
		global::PatrolRoute[] patrolRoutes = this.ground.GetComponentsInChildren<global::PatrolRoute>();
		for (int i7 = 0; i7 < patrolRoutes.Length; i7++)
		{
			patrolRoutes[i7].CheckValidity();
		}
		yield break;
	}

	private void LoadAthleticFx(global::ActionDestination destination, string name, int index)
	{
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, name + ".prefab", delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			gameObject.SetActive(true);
			gameObject.transform.SetParent(this.actionZones[index].transform.parent);
			gameObject.transform.localPosition = new global::UnityEngine.Vector3(0f, 0f, 0f);
			gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
			destination.fx = gameObject;
		});
	}

	private void AddNodeLink(global::UnityEngine.GameObject anchor, global::UnityEngine.Transform enterPoint, global::UnityEngine.Transform exitPoint, global::ActionDestination dest)
	{
		global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("node_link_teleport");
		gameObject.transform.SetParent(anchor.transform);
		global::Pathfinding.NodeLink2 nodeLink = gameObject.AddComponent<global::Pathfinding.NodeLink2>();
		nodeLink.oneWay = true;
		nodeLink.transform.position = enterPoint.position;
		nodeLink.transform.rotation = enterPoint.rotation;
		nodeLink.end = exitPoint;
		if (dest != null)
		{
			dest.navLink = nodeLink;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.nodeLinks.Add(nodeLink);
	}

	private global::System.Collections.IEnumerator GenerateUnits()
	{
		global::UnityEngine.Vector3 defpos = new global::UnityEngine.Vector3(500f, 150f, 500f);
		for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count; i++)
		{
			global::DeploymentScenarioSlotData deployScenarData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioSlotData>(this.missionSave.deployScenarioSlotIds[i]);
			global::DeploymentId deployId = deployScenarData.DeploymentId;
			global::MissionWarbandSave missionWar = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[i];
			global::WarbandController warCtrlr = new global::WarbandController(missionWar, deployId, i, this.missionSave.teams[i], (global::PrimaryObjectiveTypeId)this.missionSave.objectiveTypeIds[i], this.missionSave.objectiveTargets[i], this.missionSave.objectiveSeeds[i]);
			global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Add(warCtrlr);
			for (int j = 0; j < missionWar.Units.Count; j++)
			{
				warCtrlr.GenerateUnit(missionWar.Units[j], defpos + new global::UnityEngine.Vector3((float)i * 5f, 0f, (float)j * 5f), global::UnityEngine.Quaternion.identity, true);
			}
			yield return null;
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.roamingUnitId != 0)
		{
			global::System.Collections.Generic.List<global::SpawnNode> nodes = this.GetSpawnNodes();
			bool valid = false;
			for (int k = 0; k < nodes.Count; k++)
			{
				if (nodes[k].IsOfType(global::SpawnNodeId.ROAMING))
				{
					valid = true;
					break;
				}
			}
			if (valid)
			{
				global::UnitId unitId = (global::UnitId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.roamingUnitId;
				global::UnitData unitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>((int)unitId);
				int maxRank0 = 0;
				for (int u = 0; u < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[0].Units.Count; u++)
				{
					global::UnitSave save = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[0].Units[u];
					maxRank0 = ((save.rankId <= (global::UnitRankId)maxRank0) ? maxRank0 : ((int)save.rankId));
				}
				global::UnitRankData rankData0 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>(maxRank0);
				int maxRank = 0;
				for (int u2 = 0; u2 < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].Units.Count; u2++)
				{
					global::UnitSave save2 = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[1].Units[u2];
					maxRank = ((save2.rankId <= (global::UnitRankId)maxRank) ? maxRank : ((int)save2.rankId));
				}
				global::UnitRankData rankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>(maxRank);
				global::Unit unit = global::Unit.GenerateUnit(unitId, (int)((float)(rankData0.Rank + rankData.Rank) / 2f));
				if (!string.IsNullOrEmpty(unit.Data.FirstName))
				{
					unit.UnitSave.stats.name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(unit.Data.FirstName);
				}
				int rating = 0;
				global::UnitFactory.RaiseAttributes(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>(), unit, ref rating, 999999);
				int teamIdx = 0;
				for (int t = 0; t < this.missionSave.teams.Count; t++)
				{
					teamIdx = ((this.missionSave.teams[t] <= teamIdx) ? teamIdx : this.missionSave.teams[t]);
				}
				teamIdx++;
				global::MissionWarbandSave missionWar2 = new global::MissionWarbandSave(unitData.WarbandId, global::CampaignWarbandId.NONE, "roamingWarbandName", string.Empty, "roamingPlayerName", 0, 0, global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[0].PlayerIndex, global::PlayerTypeId.AI, null);
				global::WarbandController warCtrlr2 = new global::WarbandController(missionWar2, global::DeploymentId.ROAMING, global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count, teamIdx, global::PrimaryObjectiveTypeId.NONE, 0, 0);
				global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Add(warCtrlr2);
				warCtrlr2.GenerateUnit(unit.UnitSave, defpos, global::UnityEngine.Quaternion.identity, true);
			}
			else
			{
				global::PandoraDebug.LogWarning("No roaming point in ground", "uncategorised", null);
			}
		}
		yield return null;
		yield break;
	}

	private void GenerateReinforcements()
	{
		global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(500f, 150f, 500f);
		for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.reinforcementsIdx.Count; i++)
		{
			int key = global::PandoraSingleton<global::MissionStartData>.Instance.reinforcementsIdx[i].Key;
			int value = global::PandoraSingleton<global::MissionStartData>.Instance.reinforcementsIdx[i].Value;
			global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[value].GenerateUnit(global::PandoraSingleton<global::MissionStartData>.Instance.units[key].unitSave, position, global::UnityEngine.Quaternion.identity, true);
		}
	}

	private global::System.Collections.IEnumerator DeployUnits()
	{
		global::PandoraSingleton<global::MissionStartData>.instance.spawnZones = new global::System.Collections.Generic.List<global::SpawnZone>();
		global::PandoraSingleton<global::MissionStartData>.instance.spawnZones.AddRange(this.ground.GetComponentsInChildren<global::SpawnZone>());
		global::PandoraSingleton<global::MissionStartData>.instance.spawnNodes = new global::System.Collections.Generic.List<global::SpawnNode>[global::PandoraSingleton<global::MissionManager>.instance.WarbandCtrlrs.Count];
		this.spawnNodes = this.GetSpawnNodes();
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.instance.WarbandCtrlrs.Count; i++)
		{
			global::PandoraSingleton<global::MissionStartData>.instance.spawnNodes[i] = new global::System.Collections.Generic.List<global::SpawnNode>();
			this.Deploy(global::PandoraSingleton<global::MissionManager>.instance.WarbandCtrlrs[i], i);
			yield return null;
		}
		while (this.wagonQueued > 0)
		{
			yield return null;
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign)
		{
			int playerWarbandIdx = -1;
			for (int j = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count - 1; j >= 0; j--)
			{
				if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].CampaignWarbandId == global::CampaignWarbandId.NONE)
				{
					playerWarbandIdx = j;
					break;
				}
			}
			if (playerWarbandIdx != -1)
			{
				global::WarbandController playerWarband = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[playerWarbandIdx];
				for (int k = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count - 1; k >= 0; k--)
				{
					if (k != playerWarbandIdx)
					{
						global::WarbandController otherWarband = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[k];
						if (otherWarband.playerTypeId == global::PlayerTypeId.PLAYER && otherWarband.teamIdx == playerWarband.teamIdx)
						{
							for (int u = 0; u < otherWarband.unitCtrlrs.Count; u++)
							{
								otherWarband.unitCtrlrs[u].unit.warbandIdx = playerWarband.idx;
								playerWarband.unitCtrlrs.Add(otherWarband.unitCtrlrs[u]);
								playerWarband.OnUnitCreated(otherWarband.unitCtrlrs[u]);
							}
							global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.RemoveAt(k);
						}
					}
				}
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.excludedUnits = new global::System.Collections.Generic.List<global::UnitController>();
		for (int w = 0; w < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; w++)
		{
			global::WarbandController warCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[w];
			warCtrlr.idx = w;
			for (int l = warCtrlr.unitCtrlrs.Count - 1; l >= 0; l--)
			{
				global::UnitController ctrlr = warCtrlr.unitCtrlrs[l];
				ctrlr.unit.warbandIdx = w;
			}
			warCtrlr.InitBlackLists();
			if (warCtrlr.SquadManager != null)
			{
				warCtrlr.SquadManager.FormSquads();
			}
			global::PandoraSingleton<global::MissionManager>.Instance.allUnitsList.AddRange(warCtrlr.unitCtrlrs);
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			for (int nodeIndex = 0; nodeIndex < this.spawnNodes.Count; nodeIndex++)
			{
				global::SpawnNode node = this.spawnNodes[nodeIndex];
				if (node != null)
				{
					global::UnityEngine.Object.DestroyImmediate(node.gameObject);
				}
			}
			for (int zoneIndex = 0; zoneIndex < global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones.Count; zoneIndex++)
			{
				global::SpawnZone zone = global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones[zoneIndex];
				if (zone != null)
				{
					global::UnityEngine.Object.DestroyImmediate(zone.gameObject);
				}
			}
			global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes = null;
			global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones = null;
		}
		else
		{
			for (int zoneIndex2 = 0; zoneIndex2 < global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones.Count; zoneIndex2++)
			{
				global::SpawnZone zone2 = global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones[zoneIndex2];
				if (zone2 != null)
				{
					global::UnityEngine.Renderer[] r = zone2.GetComponentsInChildren<global::UnityEngine.Renderer>();
					for (int m = 0; m < r.Length; m++)
					{
						r[m].enabled = false;
					}
				}
			}
		}
		yield break;
	}

	public void Deploy(global::WarbandController warCtrlr, int idx)
	{
		this.deployedZones = new global::System.Collections.Generic.List<global::SpawnZone>();
		switch (warCtrlr.deploymentId)
		{
		case global::DeploymentId.WAGON:
			this.DeployInZone(warCtrlr, global::SpawnZoneId.START, global::SpawnNodeId.START, warCtrlr.unitCtrlrs, idx);
			break;
		case global::DeploymentId.STRIKE_TEAM:
		{
			global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>();
			global::System.Collections.Generic.List<global::UnitController> list2 = new global::System.Collections.Generic.List<global::UnitController>();
			global::System.Collections.Generic.List<global::UnitController> list3 = new global::System.Collections.Generic.List<global::UnitController>();
			int num = warCtrlr.unitCtrlrs.Count / 3;
			global::System.Collections.Generic.List<int> list4 = new global::System.Collections.Generic.List<int>(warCtrlr.unitCtrlrs.Count);
			for (int i = 0; i < warCtrlr.unitCtrlrs.Count; i++)
			{
				if (warCtrlr.unitCtrlrs[i].unit.IsImpressive)
				{
					list3.Add(warCtrlr.unitCtrlrs[i]);
				}
				else
				{
					list4.Add(i);
				}
			}
			for (int j = 0; j < num; j++)
			{
				int index = this.networkTyche.Rand(0, list4.Count);
				list.Add(warCtrlr.unitCtrlrs[list4[index]]);
				list4.RemoveAt(index);
			}
			for (int k = 0; k < num; k++)
			{
				int index2 = this.networkTyche.Rand(0, list4.Count);
				list2.Add(warCtrlr.unitCtrlrs[list4[index2]]);
				list4.RemoveAt(index2);
			}
			for (int l = 0; l < list4.Count; l++)
			{
				list3.Add(warCtrlr.unitCtrlrs[list4[l]]);
			}
			this.DeployInZone(warCtrlr, global::SpawnZoneId.STRIKE_START, global::SpawnNodeId.STRIKE, list3, idx);
			this.DeployInZone(warCtrlr, global::SpawnZoneId.STRIKE, global::SpawnNodeId.STRIKE, list, idx);
			this.DeployInZone(warCtrlr, global::SpawnZoneId.STRIKE, global::SpawnNodeId.STRIKE, list2, idx);
			break;
		}
		case global::DeploymentId.SCATTERED:
			this.DeployInZone(warCtrlr, global::SpawnZoneId.SCATTER, 12, warCtrlr.unitCtrlrs, idx);
			break;
		case global::DeploymentId.EXPLORING:
			this.DeployInZone(warCtrlr, global::SpawnZoneId.SCATTER, global::SpawnNodeId.INDOOR, warCtrlr.unitCtrlrs, idx);
			break;
		case global::DeploymentId.SCOUTING:
			this.DeployInZone(warCtrlr, global::SpawnZoneId.SCATTER, global::SpawnNodeId.OUTDOOR, warCtrlr.unitCtrlrs, idx);
			break;
		case global::DeploymentId.AMBUSHED:
			this.DeployInZone(warCtrlr, global::SpawnZoneId.AMBUSH, global::SpawnNodeId.INDOOR, warCtrlr.unitCtrlrs, idx);
			break;
		case global::DeploymentId.AMBUSHER:
			this.DeployInZone(warCtrlr, global::SpawnZoneId.AMBUSH, global::SpawnNodeId.OUTDOOR, warCtrlr.unitCtrlrs, idx);
			break;
		case global::DeploymentId.CAMPAIGN_PLAYER:
		case global::DeploymentId.CAMPAIGN_AI:
			this.DeployCampaign(warCtrlr, warCtrlr.unitCtrlrs);
			break;
		case global::DeploymentId.ROAMING:
			this.DeployRoaming(warCtrlr, idx);
			break;
		}
		this.deployedZones.Clear();
		this.deployedZones = null;
	}

	private void DeployRoaming(global::WarbandController warCtrlr, int idx)
	{
		global::System.Collections.Generic.List<global::SpawnNode> list = new global::System.Collections.Generic.List<global::SpawnNode>();
		for (int i = 0; i < this.spawnNodes.Count; i++)
		{
			if (!this.spawnNodes[i].claimed && this.spawnNodes[i].IsOfType(global::SpawnNodeId.ROAMING))
			{
				list.Add(this.spawnNodes[i]);
				global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[idx].Add(this.spawnNodes[i]);
			}
		}
		global::SpawnNode spawnNode = list[global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list.Count)];
		spawnNode.claimed = true;
		global::UnityEngine.Debug.Log("DEPLOYING UNIT ON NODE AT POSITION : " + spawnNode.transform.position);
		warCtrlr.unitCtrlrs[0].transform.rotation = spawnNode.transform.rotation;
		warCtrlr.unitCtrlrs[0].SetFixed(spawnNode.transform.position, true);
	}

	private void DeployCampaign(global::WarbandController warCtrlr, global::System.Collections.Generic.List<global::UnitController> units)
	{
		global::CampaignMissionJoinCampaignWarbandData missionWarData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionJoinCampaignWarbandData>(new string[]
		{
			"fk_campaign_mission_id",
			"fk_campaign_warband_id"
		}, new string[]
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.campaignId.ToString(),
			((int)warCtrlr.CampaignWarbandId).ToString()
		})[0];
		warCtrlr.canRout = missionWarData.CanRout;
		global::PandoraDebug.LogInfo("Looking for zone : " + missionWarData.DeployZone, "uncategorised", null);
		global::SpawnZone spawnZone = global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones.Find((global::SpawnZone x) => x.name == missionWarData.DeployZone);
		spawnZone.Claim(global::DeploymentId.CAMPAIGN_PLAYER);
		global::System.Collections.Generic.List<global::SpawnNode> list = new global::System.Collections.Generic.List<global::SpawnNode>();
		list.AddRange(spawnZone.GetComponentsInChildren<global::SpawnNode>());
		global::System.Collections.Generic.List<global::SpawnNode> list2 = new global::System.Collections.Generic.List<global::SpawnNode>();
		global::System.Collections.Generic.List<global::SpawnNode> list3 = new global::System.Collections.Generic.List<global::SpawnNode>();
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].IsOfType(global::SpawnNodeId.WAGON) && !list[i].claimed)
			{
				list2.Add(list[i]);
				if (list[i].IsOfType(global::SpawnNodeId.IMPRESSIVE))
				{
					list3.Add(list[i]);
				}
			}
		}
		if (warCtrlr.NeedWagon)
		{
			for (int j = 0; j < list.Count; j++)
			{
				global::SpawnNode spawnNode = list[j];
				if (spawnNode.IsOfType(global::SpawnNodeId.WAGON) && !spawnNode.claimed)
				{
					spawnNode.claimed = true;
					this.SpawnWagon(spawnNode, warCtrlr);
					break;
				}
			}
		}
		for (int k = units.Count - 1; k >= 0; k--)
		{
			if (units[k].unit.IsImpressive)
			{
				global::SpawnNode spawnNode2;
				if (missionWarData.CampaignWarbandId == global::CampaignWarbandId.NONE)
				{
					int index = this.networkTyche.Rand(0, list3.Count);
					spawnNode2 = list3[index];
					spawnNode2.claimed = true;
					list3.RemoveAt(index);
					list2.Remove(spawnNode2);
				}
				else
				{
					global::CampaignWarbandJoinCampaignUnitData campUnitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandJoinCampaignUnitData>(new string[]
					{
						"fk_campaign_warband_id",
						"fk_campaign_unit_id"
					}, new string[]
					{
						((int)warCtrlr.CampaignWarbandId).ToString(),
						units[k].unit.UnitSave.campaignId.ToString()
					})[0];
					spawnNode2 = list.Find((global::SpawnNode x) => x.name == campUnitData.DeployNode);
					spawnNode2.claimed = true;
				}
				units[k].transform.rotation = spawnNode2.transform.rotation;
				units[k].SetFixed(spawnNode2.transform.position, true);
			}
		}
		for (int l = 0; l < units.Count; l++)
		{
			if (!units[l].unit.IsImpressive && !units[l].unit.UnitSave.isReinforcement)
			{
				global::SpawnNode spawnNode3;
				if (missionWarData.CampaignWarbandId == global::CampaignWarbandId.NONE)
				{
					int index2 = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list2.Count);
					spawnNode3 = list2[index2];
					spawnNode3.claimed = true;
					list2.RemoveAt(index2);
				}
				else
				{
					if (units[l].unit.Id == global::UnitId.BLUE_HORROR)
					{
						goto IL_45E;
					}
					global::CampaignWarbandJoinCampaignUnitData campUnitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandJoinCampaignUnitData>(new string[]
					{
						"fk_campaign_warband_id",
						"fk_campaign_unit_id"
					}, new string[]
					{
						((int)warCtrlr.CampaignWarbandId).ToString(),
						units[l].unit.UnitSave.campaignId.ToString()
					})[0];
					spawnNode3 = list.Find((global::SpawnNode x) => x.name == campUnitData.DeployNode);
					spawnNode3.claimed = true;
				}
				units[l].transform.rotation = spawnNode3.transform.rotation;
				units[l].SetFixed(spawnNode3.transform.position, true);
			}
			IL_45E:;
		}
	}

	private void DeployInZone(global::WarbandController warCtrlr, global::SpawnZoneId id, global::SpawnNodeId nodeId, global::System.Collections.Generic.List<global::UnitController> units, int idx)
	{
		this.DeployInZone(warCtrlr, id, 1 << nodeId - global::SpawnNodeId.START, units, idx);
	}

	private void DeployInZone(global::WarbandController warCtrlr, global::SpawnZoneId zoneId, int nodeTypes, global::System.Collections.Generic.List<global::UnitController> units, int idx)
	{
		global::System.Collections.Generic.List<global::SpawnZone> list = new global::System.Collections.Generic.List<global::SpawnZone>();
		if (zoneId != global::SpawnZoneId.STRIKE)
		{
			for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.instance.spawnZones.Count; i++)
			{
				global::SpawnZone spawnZone = global::PandoraSingleton<global::MissionStartData>.instance.spawnZones[i];
				if (spawnZone.type == zoneId && !spawnZone.IsClaimed(warCtrlr.deploymentId))
				{
					list.Add(spawnZone);
				}
			}
		}
		else
		{
			list.AddRange(this.deployedZones[0].GetComponentsInChildren<global::SpawnZone>());
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (list[j].IsClaimed(warCtrlr.deploymentId))
				{
					list.RemoveAt(j);
				}
			}
		}
		int index = this.networkTyche.Rand(0, list.Count);
		global::SpawnZone spawnZone2 = list[index];
		spawnZone2.Claim(warCtrlr.deploymentId);
		this.deployedZones.Add(spawnZone2);
		global::System.Collections.Generic.List<global::SpawnNode> list2 = new global::System.Collections.Generic.List<global::SpawnNode>();
		global::System.Collections.Generic.List<global::SpawnNode> list3 = new global::System.Collections.Generic.List<global::SpawnNode>();
		global::System.Collections.Generic.List<global::SpawnNode> list4 = new global::System.Collections.Generic.List<global::SpawnNode>();
		bool flag = !string.IsNullOrEmpty(warCtrlr.WarData.Wagon);
		for (int k = 0; k < this.spawnNodes.Count; k++)
		{
			global::SpawnNode spawnNode = this.spawnNodes[k];
			if (!spawnNode.claimed && (spawnNode.types & nodeTypes) != 0 && !spawnNode.IsOfType(global::SpawnNodeId.WAGON) && ((zoneId == global::SpawnZoneId.SCATTER && global::UnityEngine.Vector3.SqrMagnitude(spawnZone2.transform.position - spawnNode.transform.position) < spawnZone2.range * spawnZone2.range && !this.InsideZone(list, spawnNode.transform.position)) || (zoneId != global::SpawnZoneId.SCATTER && spawnZone2.bounds.Contains(spawnNode.transform.position))))
			{
				list3.Add(spawnNode);
				global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[idx].Add(spawnNode);
				if (spawnNode.IsOfType(global::SpawnNodeId.IMPRESSIVE))
				{
					list4.Add(spawnNode);
				}
			}
			else if (flag && warCtrlr.deploymentId == global::DeploymentId.AMBUSHED && !spawnNode.claimed && spawnNode.IsOfType(global::SpawnNodeId.WAGON) && spawnNode.IsOfType(global::SpawnNodeId.INDOOR) && spawnZone2.bounds.Contains(spawnNode.transform.position))
			{
				list2.Add(spawnNode);
			}
		}
		if (flag && spawnZone2.type != global::SpawnZoneId.STRIKE)
		{
			if (warCtrlr.deploymentId != global::DeploymentId.AMBUSHED)
			{
				foreach (global::SpawnNode spawnNode2 in spawnZone2.GetComponentsInChildren<global::SpawnNode>())
				{
					if (spawnNode2.IsOfType(global::SpawnNodeId.WAGON))
					{
						list2.Add(spawnNode2);
					}
				}
			}
			int index2 = this.networkTyche.Rand(0, list2.Count);
			global::SpawnNode spawnNode3 = list2[index2];
			spawnNode3.claimed = true;
			this.SpawnWagon(spawnNode3, warCtrlr);
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			for (int m = units.Count - 1; m >= 0; m--)
			{
				if (units[m].unit.IsImpressive)
				{
					int index3 = this.networkTyche.Rand(0, list4.Count);
					global::SpawnNode spawnNode4 = list4[index3];
					spawnNode4.claimed = true;
					list4.RemoveAt(index3);
					list3.Remove(spawnNode4);
					global::UnityEngine.Debug.Log("DEPLOYING UNIT ON NODE AT POSITION : " + spawnNode4.transform.position);
					units[m].transform.rotation = spawnNode4.transform.rotation;
					units[m].SetFixed(spawnNode4.transform.position, true);
				}
			}
			for (int n = 0; n < units.Count; n++)
			{
				if (!units[n].unit.IsImpressive)
				{
					int index4 = this.networkTyche.Rand(0, list3.Count);
					global::SpawnNode spawnNode5 = list3[index4];
					spawnNode5.claimed = true;
					global::UnityEngine.Debug.Log("DEPLOYING UNIT ON NODE AT POSITION : " + spawnNode5.transform.position);
					units[n].transform.rotation = spawnNode5.transform.rotation;
					units[n].SetFixed(spawnNode5.transform.position, true);
					list3.RemoveAt(index4);
				}
			}
		}
	}

	private bool InsideZone(global::System.Collections.Generic.List<global::SpawnZone> zones, global::UnityEngine.Vector3 pos)
	{
		for (int i = 0; i < zones.Count; i++)
		{
			global::SpawnZone spawnZone = zones[i];
			if (spawnZone.bounds.Contains(pos))
			{
				return true;
			}
		}
		return false;
	}

	private void SpawnWagon(global::SpawnNode wagonNode, global::WarbandController warCtrlr)
	{
		bool flag;
		if (wagonNode.overrideStyle == global::SpawnNodeId.INDOOR || wagonNode.overrideStyle == global::SpawnNodeId.OUTDOOR)
		{
			flag = (wagonNode.overrideStyle == global::SpawnNodeId.INDOOR);
		}
		else
		{
			flag = wagonNode.IsOfType(global::SpawnNodeId.INDOOR);
		}
		global::UnityEngine.Vector3 position = wagonNode.transform.position;
		global::UnityEngine.Quaternion rotation = wagonNode.transform.rotation;
		this.wagonQueued++;
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/" + ((!flag) ? "wagons/" : "chests/"), global::AssetBundleId.PROPS, ((!flag) ? warCtrlr.WarData.Wagon : warCtrlr.WarData.Chest) + ".prefab", delegate(global::UnityEngine.Object wagPrefab)
		{
			this.wagonQueued--;
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(wagPrefab);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			warCtrlr.SetWagon(gameObject);
			if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign && warCtrlr.playerTypeId == global::PlayerTypeId.AI)
			{
				global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
				string[] fields = new string[]
				{
					"warband_rank",
					"fk_search_density_id"
				};
				string[] array = new string[2];
				array[0] = this.lowestWarbandRank.ToString();
				int num = 1;
				int searchDensityId = this.missionSave.searchDensityId;
				array[num] = searchDensityId.ToString();
				global::System.Collections.Generic.List<global::SearchDensityLootData> datas = instance.InitData<global::SearchDensityLootData>(fields, array);
				global::SearchDensityLootData randomRatio = global::SearchDensityLootData.GetRandomRatio(datas, this.networkTyche, null);
				int itemCount = this.networkTyche.Rand(randomRatio.ItemMin, randomRatio.ItemMax + 1);
				this.FillSearchPoint(warCtrlr.wagon.chest, randomRatio, this.lowestWarbandRank, itemCount);
			}
		});
	}

	private global::TrapId GetRandomTrapId(global::TrapTypeId trapTypeId)
	{
		global::System.Collections.Generic.List<global::TrapTypeJoinTrapData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeJoinTrapData>("fk_trap_type_id", trapTypeId.ToIntString<global::TrapTypeId>());
		return list[this.networkTyche.Rand(0, list.Count)].TrapId;
	}

	private void GenerateTrapsAsync()
	{
		this.traps = new global::System.Collections.Generic.List<global::Trap>();
		this.trapData = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::TrapTypeData, bool>>();
		this.trapNodes = new global::System.Collections.Generic.List<global::TrapNode>();
		this.traps.AddRange(this.ground.GetComponentsInChildren<global::Trap>());
		for (int i = 0; i < this.traps.Count; i++)
		{
			global::Trap trap = this.traps[i];
			global::TrapTypeData data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeData>((int)trap.defaultType);
			trap.Init(data, global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
			global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Add(trap);
		}
		if (this.trapCount == 0)
		{
			return;
		}
		this.trapNodes.AddRange(this.ground.GetComponentsInChildren<global::TrapNode>());
		if (this.trapNodes.Count == 0)
		{
			return;
		}
		global::System.Collections.Generic.Dictionary<global::TrapTypeId, global::System.Collections.Generic.List<global::TrapNode>> dictionary = new global::System.Collections.Generic.Dictionary<global::TrapTypeId, global::System.Collections.Generic.List<global::TrapNode>>();
		for (int j = 0; j < this.trapNodes.Count; j++)
		{
			global::TrapNode trapNode = this.trapNodes[j];
			if (!dictionary.ContainsKey(trapNode.typeId))
			{
				dictionary[trapNode.typeId] = new global::System.Collections.Generic.List<global::TrapNode>();
			}
			dictionary[trapNode.typeId].Add(trapNode);
		}
		foreach (global::TrapTypeId trapTypeId in dictionary.Keys)
		{
			global::TrapTypeData trapTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeData>((int)trapTypeId);
			int num = (int)global::UnityEngine.Mathf.Ceil((float)(dictionary[trapTypeId].Count * trapTypeData.Perc) / 100f);
			for (int k = 0; k < num; k++)
			{
				int index = this.networkTyche.Rand(0, dictionary[trapTypeId].Count);
				global::TrapNode trapNode2 = dictionary[trapTypeId][index];
				global::TrapZone component = trapNode2.transform.parent.gameObject.GetComponent<global::TrapZone>();
				global::TrapId trapId;
				if (component != null && component.currentTrapId != global::TrapId.NONE)
				{
					trapId = component.currentTrapId;
				}
				else
				{
					trapId = this.GetRandomTrapId(trapTypeData.Id);
					if (component != null)
					{
						component.currentTrapId = trapId;
					}
				}
				string name = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapData>((int)trapId).Name;
				this.AddSceneJob(name, trapNode2.transform, 0);
				this.trapData.Add(new global::System.Collections.Generic.KeyValuePair<global::TrapTypeData, bool>(trapTypeData, trapNode2.forceInactive));
				dictionary[trapTypeId].RemoveAt(index);
			}
		}
	}

	private void TrapsPostProcess()
	{
		global::System.Collections.Generic.List<global::Trap> availableTraps = this.GetAvailableTraps();
		int num = global::UnityEngine.Mathf.Min(this.trapCount, availableTraps.Count);
		int num2 = 0;
		while (num2 < num && availableTraps.Count > 0)
		{
			int index = this.networkTyche.Rand(0, availableTraps.Count);
			global::Trap trap = availableTraps[index];
			global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Add(trap);
			this.traps.Remove(trap);
			availableTraps.RemoveAt(index);
			float num3 = global::Constant.GetFloat(global::ConstantId.MIN_TRAP_DISTANCE);
			num3 *= num3;
			for (int i = availableTraps.Count - 1; i >= 0; i--)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(availableTraps[i].transform.position - trap.transform.position) < num3)
				{
					availableTraps.RemoveAt(i);
				}
			}
			num2++;
		}
		for (int j = this.traps.Count - 1; j >= 0; j--)
		{
			global::UnityEngine.Object.Destroy(this.traps[j]);
		}
		for (int k = 0; k < this.trapNodes.Count; k++)
		{
			global::TrapNode trapNode = this.trapNodes[k];
			global::UnityEngine.Object.Destroy(trapNode.gameObject);
		}
		this.trapNodes.Clear();
	}

	private global::System.Collections.Generic.List<global::Trap> GetAvailableTraps()
	{
		global::System.Collections.Generic.List<global::SpawnNode> list = this.GetSpawnNodes();
		global::System.Collections.Generic.List<global::Trap> list2 = new global::System.Collections.Generic.List<global::Trap>();
		for (int i = 0; i < this.traps.Count; i++)
		{
			global::Trap trap = this.traps[i];
			bool flag = !trap.forceInactive;
			if (flag)
			{
				int num = 0;
				while (num < list.Count && flag)
				{
					flag &= (global::UnityEngine.Vector3.SqrMagnitude(trap.trigger.transform.position - list[num].transform.position) > 25f);
					num++;
				}
			}
			if (flag)
			{
				list2.Add(trap);
			}
		}
		return list2;
	}

	private global::System.Collections.IEnumerator ReloadTraps()
	{
		this.loadingTraps = 0;
		global::System.Collections.Generic.List<uint> usedTraps = global::PandoraSingleton<global::MissionStartData>.Instance.usedTraps;
		for (int usedTrapIdx = 0; usedTrapIdx < usedTraps.Count; usedTrapIdx++)
		{
			global::System.Collections.Generic.List<global::TriggerPoint> missionTraps = global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints;
			for (int generatedTrapsIdx = 0; generatedTrapsIdx < missionTraps.Count; generatedTrapsIdx++)
			{
				if (missionTraps[generatedTrapsIdx].guid == usedTraps[usedTrapIdx])
				{
					global::UnityEngine.Object.Destroy(missionTraps[generatedTrapsIdx]);
					break;
				}
			}
		}
		global::System.Collections.Generic.List<global::EndDynamicTrap> dynamicTraps = global::PandoraSingleton<global::MissionStartData>.Instance.dynamicTraps;
		for (int i = 0; i < dynamicTraps.Count; i++)
		{
			if (!dynamicTraps[i].consumed)
			{
				this.loadingTraps++;
				global::Trap.SpawnTrap((global::TrapTypeId)dynamicTraps[i].trapTypeId, dynamicTraps[i].teamIdx, dynamicTraps[i].pos, dynamicTraps[i].rot, delegate
				{
					this.loadingTraps--;
				}, false);
			}
		}
		while (this.loadingTraps > 0)
		{
			yield return null;
		}
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
		yield break;
	}

	private void ReloadDestructibles()
	{
		global::System.Collections.Generic.List<global::EndDestructible> destructibles = global::PandoraSingleton<global::MissionStartData>.Instance.destructibles;
		for (int i = 0; i < destructibles.Count; i++)
		{
			global::EndDestructible endDestructible = destructibles[i];
			if (endDestructible.onwerGuid == 0U)
			{
				for (int j = 0; j < global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Count; j++)
				{
					if (global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[j].guid == endDestructible.guid)
					{
						global::Destructible destructible = (global::Destructible)global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[j];
						destructible.CurrentWounds = endDestructible.wounds;
						if (endDestructible.wounds <= 0)
						{
							destructible.Deactivate();
						}
						endDestructible.guid = destructible.guid;
					}
				}
			}
			else if (endDestructible.wounds > 0)
			{
				global::Destructible.Spawn(endDestructible.destructibleId, global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(endDestructible.onwerGuid), endDestructible.position, endDestructible.wounds);
			}
		}
	}

	private void GenerateWyrdStonesAsync()
	{
		this.wyrdTypeData = new global::System.Collections.Generic.List<global::WyrdstoneTypeData>();
		global::SearchPoint[] componentsInChildren = this.ground.GetComponentsInChildren<global::SearchPoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		}
		global::ActivatePoint[] componentsInChildren2 = this.ground.GetComponentsInChildren<global::ActivatePoint>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		}
		global::System.Collections.Generic.List<global::SearchArea> list = new global::System.Collections.Generic.List<global::SearchArea>();
		list.AddRange(this.ground.GetComponentsInChildren<global::SearchArea>());
		global::System.Collections.Generic.List<global::SearchZone> list2 = new global::System.Collections.Generic.List<global::SearchZone>();
		list2.AddRange(this.ground.GetComponentsInChildren<global::SearchZone>());
		this.searchZones = new global::System.Collections.Generic.List<global::SearchZone>();
		for (int k = 0; k < list2.Count; k++)
		{
			global::SearchZone searchZone = list2[k];
			if (this.InsideArea(list, searchZone.transform.position))
			{
				this.searchZones.Add(searchZone);
			}
		}
		this.allSearchNodes = new global::System.Collections.Generic.List<global::SearchNode>();
		this.allSearchNodes.AddRange(this.ground.GetComponentsInChildren<global::SearchNode>());
		this.searchNodes = new global::System.Collections.Generic.List<global::SearchNode>();
		for (int l = 0; l < this.allSearchNodes.Count; l++)
		{
			global::SearchNode searchNode = this.allSearchNodes[l];
			if (this.InsideArea(list, searchNode.transform.position))
			{
				this.searchNodes.Add(searchNode);
			}
		}
		if (this.missionSave.wyrdPlacementId != 0 && this.missionSave.wyrdDensityId != 0)
		{
			this.GenerateWyrdStones((global::WyrdstonePlacementId)this.missionSave.wyrdPlacementId, (global::WyrdstoneDensityId)this.missionSave.wyrdDensityId);
		}
	}

	private void GenerateSearchAsync()
	{
		if (this.missionSave.searchDensityId != 0)
		{
			this.GenerateSearch((global::SearchDensityId)this.missionSave.searchDensityId);
		}
	}

	private void SearchPostProcess()
	{
		global::ZoneAoe[] componentsInChildren = this.ground.GetComponentsInChildren<global::ZoneAoe>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].AutoInit();
		}
		global::Destructible[] componentsInChildren2 = this.ground.GetComponentsInChildren<global::Destructible>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].AutoInit();
		}
		global::System.Collections.Generic.List<global::SearchArea> list = new global::System.Collections.Generic.List<global::SearchArea>();
		list.AddRange(this.ground.GetComponentsInChildren<global::SearchArea>());
		global::System.Collections.Generic.List<global::SearchZone> list2 = new global::System.Collections.Generic.List<global::SearchZone>();
		list2.AddRange(this.ground.GetComponentsInChildren<global::SearchZone>());
		for (int k = 0; k < list.Count; k++)
		{
			global::SearchArea searchArea = list[k];
			global::UnityEngine.Object.Destroy(searchArea.gameObject);
		}
		for (int l = 0; l < list2.Count; l++)
		{
			global::SearchZone searchZone = list2[l];
			global::UnityEngine.Object.Destroy(searchZone.gameObject);
		}
		for (int m = 0; m < this.allSearchNodes.Count; m++)
		{
			global::SearchNode searchNode = this.allSearchNodes[m];
			global::UnityEngine.Object.Destroy(searchNode.gameObject);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.numWyrdstones = global::PandoraSingleton<global::MissionManager>.Instance.GetInitialWyrdstoneCount();
	}

	private void GenerateSearchPoints()
	{
		global::SearchPoint[] componentsInChildren = this.ground.GetComponentsInChildren<global::SearchPoint>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		}
		global::ActivatePoint[] componentsInChildren2 = this.ground.GetComponentsInChildren<global::ActivatePoint>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].Init(global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID());
		}
		global::System.Collections.Generic.List<global::SearchArea> list = new global::System.Collections.Generic.List<global::SearchArea>();
		list.AddRange(this.ground.GetComponentsInChildren<global::SearchArea>());
		global::System.Collections.Generic.List<global::SearchZone> list2 = new global::System.Collections.Generic.List<global::SearchZone>();
		list2.AddRange(this.ground.GetComponentsInChildren<global::SearchZone>());
		this.searchZones = new global::System.Collections.Generic.List<global::SearchZone>();
		for (int k = 0; k < list2.Count; k++)
		{
			global::SearchZone searchZone = list2[k];
			if (this.InsideArea(list, searchZone.transform.position))
			{
				this.searchZones.Add(searchZone);
			}
		}
		this.allSearchNodes = new global::System.Collections.Generic.List<global::SearchNode>();
		this.allSearchNodes.AddRange(this.ground.GetComponentsInChildren<global::SearchNode>());
		this.searchNodes = new global::System.Collections.Generic.List<global::SearchNode>();
		for (int l = 0; l < this.allSearchNodes.Count; l++)
		{
			global::SearchNode searchNode = this.allSearchNodes[l];
			if (this.InsideArea(list, searchNode.transform.position))
			{
				this.searchNodes.Add(searchNode);
			}
		}
		if (this.missionSave.wyrdPlacementId != 0 && this.missionSave.wyrdDensityId != 0)
		{
			this.GenerateWyrdStones((global::WyrdstonePlacementId)this.missionSave.wyrdPlacementId, (global::WyrdstoneDensityId)this.missionSave.wyrdDensityId);
		}
		if (this.missionSave.searchDensityId != 0)
		{
			this.GenerateSearch((global::SearchDensityId)this.missionSave.searchDensityId);
		}
		global::ZoneAoe[] componentsInChildren3 = this.ground.GetComponentsInChildren<global::ZoneAoe>();
		for (int m = 0; m < componentsInChildren3.Length; m++)
		{
			componentsInChildren3[m].AutoInit();
		}
		global::Destructible[] componentsInChildren4 = this.ground.GetComponentsInChildren<global::Destructible>();
		for (int n = 0; n < componentsInChildren4.Length; n++)
		{
			componentsInChildren4[n].AutoInit();
		}
		for (int num = 0; num < list.Count; num++)
		{
			global::SearchArea searchArea = list[num];
			global::UnityEngine.Object.Destroy(searchArea.gameObject);
		}
		for (int num2 = 0; num2 < list2.Count; num2++)
		{
			global::SearchZone searchZone2 = list2[num2];
			global::UnityEngine.Object.Destroy(searchZone2.gameObject);
		}
		for (int num3 = 0; num3 < this.allSearchNodes.Count; num3++)
		{
			global::SearchNode searchNode2 = this.allSearchNodes[num3];
			global::UnityEngine.Object.Destroy(searchNode2.gameObject);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.numWyrdstones = global::PandoraSingleton<global::MissionManager>.Instance.GetInitialWyrdstoneCount();
	}

	private bool InsideArea(global::System.Collections.Generic.List<global::SearchArea> areas, global::UnityEngine.Vector3 position)
	{
		for (int i = 0; i < areas.Count; i++)
		{
			global::SearchArea searchArea = areas[i];
			if (searchArea.Contains(position))
			{
				return true;
			}
		}
		return false;
	}

	private global::UnityEngine.Transform GetValidParent(global::UnityEngine.GameObject node)
	{
		global::UnityEngine.GameObject gameObject = node.transform.parent.gameObject;
		if (gameObject.GetComponent<global::SearchArea>() != null || gameObject.GetComponent<global::SearchZone>() != null)
		{
			return this.GetValidParent(gameObject);
		}
		return gameObject.transform;
	}

	private void GenerateWyrdStones(global::WyrdstonePlacementId placementId, global::WyrdstoneDensityId densityId)
	{
		global::WyrdstoneDensityData wyrdstoneDensityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneDensityData>((int)densityId);
		int i = this.networkTyche.Rand(wyrdstoneDensityData.SpawnMin, wyrdstoneDensityData.SpawnMax + 1);
		global::WyrdstoneDensityProgressionData wyrdstoneDensityProgressionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneDensityProgressionData>(new string[]
		{
			"fk_wyrdstone_density_id",
			"warband_rank"
		}, new string[]
		{
			((int)wyrdstoneDensityData.Id).ToString(),
			this.lowestWarbandRank.ToString()
		})[0];
		int num = (int)global::UnityEngine.Mathf.Ceil((float)(i * wyrdstoneDensityProgressionData.Cluster) / 100f);
		int num2 = (int)global::UnityEngine.Mathf.Ceil((float)(i * wyrdstoneDensityProgressionData.Shard) / 100f);
		int count = i - num - num2;
		global::System.Collections.Generic.List<global::WyrdstoneTypeId> types = new global::System.Collections.Generic.List<global::WyrdstoneTypeId>();
		this.WyrdstoneAddType(types, num, global::WyrdstoneTypeId.CLUSTER);
		this.WyrdstoneAddType(types, num2, global::WyrdstoneTypeId.SHARD);
		this.WyrdstoneAddType(types, count, global::WyrdstoneTypeId.FRAGMENT);
		switch (placementId)
		{
		case global::WyrdstonePlacementId.CONCENTRATION:
			this.DeployWyrdStoneZone(global::SearchZoneId.WYRDSTONE_CONCENTRATION, i, types);
			break;
		case global::WyrdstonePlacementId.CLUMP_AND_SPREAD:
		{
			int num3 = this.networkTyche.Rand(3, 7);
			this.DeployWyrdStoneZone(global::SearchZoneId.WYRDSTONE_CLUSTER, num3, types);
			i -= num3;
			this.DeployWyrdStoneZone(global::SearchZoneId.NONE, i, types);
			break;
		}
		case global::WyrdstonePlacementId.SPREAD:
			this.DeployWyrdStoneZone(global::SearchZoneId.NONE, i, types);
			break;
		case global::WyrdstonePlacementId.CLUMP:
			while (i > 0)
			{
				int num4 = global::UnityEngine.Mathf.Min(this.networkTyche.Rand(3, 7), i);
				i -= num4;
				this.DeployWyrdStoneZone(global::SearchZoneId.WYRDSTONE_CLUSTER, num4, types);
			}
			break;
		}
	}

	private void WyrdstoneAddType(global::System.Collections.Generic.List<global::WyrdstoneTypeId> types, int count, global::WyrdstoneTypeId id)
	{
		for (int i = 0; i < count; i++)
		{
			types.Add(id);
		}
	}

	private void DeployWyrdStoneZone(global::SearchZoneId zoneId, int count, global::System.Collections.Generic.List<global::WyrdstoneTypeId> types)
	{
		global::System.Collections.Generic.List<global::SearchNode> list = new global::System.Collections.Generic.List<global::SearchNode>();
		switch (zoneId)
		{
		case global::SearchZoneId.NONE:
			for (int i = 0; i < this.searchNodes.Count; i++)
			{
				global::SearchNode searchNode = this.searchNodes[i];
				if (!searchNode.claimed && searchNode.IsOfType(global::SearchNodeId.WYRDSTONE))
				{
					list.Add(searchNode);
				}
			}
			break;
		case global::SearchZoneId.WYRDSTONE_CONCENTRATION:
		case global::SearchZoneId.WYRDSTONE_CLUSTER:
		{
			global::System.Collections.Generic.List<global::SearchZone> list2 = new global::System.Collections.Generic.List<global::SearchZone>();
			for (int j = this.searchZones.Count - 1; j >= 0; j--)
			{
				if (!this.searchZones[j].claimed && this.searchZones[j].type == zoneId)
				{
					list2.Add(this.searchZones[j]);
				}
			}
			if (list2.Count == 0)
			{
				global::PandoraDebug.LogWarning("No WyrdStone Zone of type " + zoneId + " found. Either none on the map or to close to units", "SEARCH_POINTS", this);
				return;
			}
			int index = this.networkTyche.Rand(0, list2.Count);
			global::SearchZone searchZone = list2[index];
			searchZone.claimed = true;
			for (int k = 0; k < this.searchNodes.Count; k++)
			{
				global::SearchNode searchNode2 = this.searchNodes[k];
				if (!searchNode2.claimed && searchNode2.IsOfType(global::SearchNodeId.WYRDSTONE) && searchZone.Contains(searchNode2.transform.position))
				{
					list.Add(searchNode2);
					searchNode2.claimed = true;
				}
			}
			break;
		}
		}
		while (count > 0 && list.Count > 0)
		{
			int index2 = this.networkTyche.Rand(0, list.Count);
			int index3 = this.networkTyche.Rand(0, types.Count);
			this.SpawnWyrdStone(types[index3], list[index2]);
			list.RemoveAt(index2);
			types.RemoveAt(index3);
			count--;
		}
	}

	private void SpawnWyrdStone(global::WyrdstoneTypeId typeId, global::SearchNode node)
	{
		node.claimed = true;
		global::WyrdstoneTypeData wyrdstoneTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneTypeData>((int)typeId);
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		global::System.Collections.Generic.List<global::WyrdstoneTypeJoinWyrdstoneData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneTypeJoinWyrdstoneData>("fk_wyrdstone_type_id", wyrdstoneTypeData.Id.ToIntString<global::WyrdstoneTypeId>());
		for (int i = 0; i < list2.Count; i++)
		{
			int wyrdstoneId = (int)list2[i].WyrdstoneId;
			global::WyrdstoneData wyrdstoneData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneData>(wyrdstoneId);
			if ((wyrdstoneData.Outdoor && node.IsOfType(global::SearchNodeId.OUTDOOR)) || (!wyrdstoneData.Outdoor && node.IsOfType(global::SearchNodeId.INDOOR)))
			{
				list.Add(wyrdstoneData.Name);
			}
		}
		int index = this.networkTyche.Rand(0, list.Count);
		string assetName = list[index];
		this.AddSceneJob(assetName, this.GetValidParent(node.gameObject), node.transform, 0);
		this.wyrdTypeData.Add(wyrdstoneTypeData);
	}

	private void GenerateSearch(global::SearchDensityId densityId)
	{
		global::System.Collections.Generic.List<global::SearchData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchData>();
		global::System.Collections.Generic.List<global::SearchData> list2 = new global::System.Collections.Generic.List<global::SearchData>();
		global::System.Collections.Generic.List<global::SearchData> list3 = new global::System.Collections.Generic.List<global::SearchData>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Outdoor)
			{
				list2.Add(list[i]);
			}
			else
			{
				list3.Add(list[i]);
			}
		}
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string[] fields = new string[]
		{
			"warband_rank",
			"fk_search_density_id"
		};
		string[] array = new string[2];
		array[0] = this.lowestWarbandRank.ToString();
		int num = 1;
		int num2 = (int)densityId;
		array[num] = num2.ToString();
		this.rewards = instance.InitData<global::SearchDensityLootData>(fields, array);
		int num3 = 0;
		for (int j = this.allSearchNodes.Count - 1; j >= 0; j--)
		{
			global::SearchNode searchNode = this.allSearchNodes[j];
			if (!searchNode.claimed && searchNode.IsOfType(global::SearchNodeId.SEARCH) && searchNode.forceInit)
			{
				num3++;
				this.SpawnSearch(searchNode, (!searchNode.IsOfType(global::SearchNodeId.OUTDOOR)) ? list3 : list2, densityId, this.lowestWarbandRank);
				searchNode.claimed = true;
			}
		}
		global::System.Collections.Generic.List<global::SearchNode> list4 = new global::System.Collections.Generic.List<global::SearchNode>();
		for (int k = 0; k < this.searchNodes.Count; k++)
		{
			global::SearchNode searchNode2 = this.searchNodes[k];
			if (!searchNode2.claimed && searchNode2.IsOfType(global::SearchNodeId.SEARCH))
			{
				list4.Add(searchNode2);
				searchNode2.claimed = true;
			}
		}
		global::SearchDensityData searchDensityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchDensityData>((int)densityId);
		int num4 = this.networkTyche.Rand(searchDensityData.SpawnMin, searchDensityData.SpawnMax + 1);
		num4 -= num3;
		num4 = global::UnityEngine.Mathf.Clamp(num4, 0, list4.Count);
		for (int l = 0; l < num4; l++)
		{
			int index = this.networkTyche.Rand(0, list4.Count);
			this.SpawnSearch(list4[index], (!list4[index].IsOfType(global::SearchNodeId.OUTDOOR)) ? list3 : list2, densityId, this.lowestWarbandRank);
			list4.RemoveAt(index);
		}
	}

	private void SpawnSearch(global::SearchNode node, global::System.Collections.Generic.List<global::SearchData> visuals, global::SearchDensityId densityId, int lowestWarbandRank)
	{
		int index = this.networkTyche.Rand(0, visuals.Count);
		string name = visuals[index].Name;
		this.AddSceneJob(name, this.GetValidParent(node.gameObject), node.transform, 0);
	}

	private void FillSearchPoint(global::SearchPoint search, global::SearchDensityLootData densityLootData, int lowestWarbandRank, int itemCount)
	{
		switch (densityLootData.SearchRewardId)
		{
		case global::SearchRewardId.NOTHING:
		case global::SearchRewardId.GOLD_LUMPS:
			this.AddGoldToSearch(search, densityLootData.SearchRewardId, lowestWarbandRank, itemCount);
			break;
		case global::SearchRewardId.NORMAL_ITEMS:
		case global::SearchRewardId.GOOD_ITEMS:
		case global::SearchRewardId.BEST_ITEMS:
		case global::SearchRewardId.GOOD_ENCHANTED:
		case global::SearchRewardId.BEST_ENCHANTED_NORMAL:
		case global::SearchRewardId.BEST_ENCHANTED_MASTER:
		{
			global::SearchRewardData searchRewardData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchRewardData>((int)densityLootData.SearchRewardId);
			global::System.Collections.Generic.List<global::SearchRewardItemData> rewardItems = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchRewardItemData>(new string[]
			{
				"fk_search_reward_id",
				"warband_rank"
			}, new string[]
			{
				((int)searchRewardData.Id).ToString(),
				lowestWarbandRank.ToString()
			});
			for (int i = 0; i < itemCount; i++)
			{
				global::Item itemReward = global::Item.GetItemReward(rewardItems, this.networkTyche);
				search.AddItem(itemReward, false);
			}
			break;
		}
		}
		global::Item.SortEmptyItems(search.items, 0);
	}

	private void AddGoldToSearch(global::SearchPoint search, global::SearchRewardId rewardId, int warbandRank, int itemCount)
	{
		int num = 0;
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string[] fields = new string[]
		{
			"fk_search_reward_id",
			"warband_rank"
		};
		string[] array = new string[2];
		int num2 = 0;
		int num3 = (int)rewardId;
		array[num2] = num3.ToString();
		array[1] = warbandRank.ToString();
		global::SearchRewardGoldData searchRewardGoldData = instance.InitData<global::SearchRewardGoldData>(fields, array)[0];
		for (int i = 0; i < itemCount; i++)
		{
			num += this.networkTyche.Rand(searchRewardGoldData.Min, searchRewardGoldData.Max + 1);
		}
		search.AddItem(new global::Item(global::ItemId.GOLD, global::ItemQualityId.NORMAL)
		{
			Save = 
			{
				amount = num
			}
		}, false);
	}

	private void SetupObjectives()
	{
		global::System.Collections.Generic.List<global::LocateZone> locateZones = global::PandoraSingleton<global::MissionManager>.Instance.GetLocateZones();
		for (int i = 0; i < locateZones.Count; i++)
		{
			locateZones[i].guid = global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID();
		}
		global::CampaignMissionId campaignId = (global::CampaignMissionId)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.campaignId;
		if (campaignId != global::CampaignMissionId.NONE)
		{
			global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
			string field = "fk_campaign_mission_id";
			int num = (int)campaignId;
			global::System.Collections.Generic.List<global::CampaignMissionJoinCampaignWarbandData> list = instance.InitData<global::CampaignMissionJoinCampaignWarbandData>(field, num.ToString());
			for (int j = 0; j < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; j++)
			{
				if (list[j].Objective)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[j].SetupObjectivesCampaign(campaignId);
				}
			}
		}
		else
		{
			for (int k = 0; k < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; k++)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[k].SetupObjectivesProc();
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.mapObjectiveZones = new global::System.Collections.Generic.List<global::MapObjectiveZone>(this.ground.GetComponentsInChildren<global::MapObjectiveZone>());
		global::PandoraSingleton<global::MissionManager>.Instance.ActivateMapObjectiveZones(false);
		global::ICustomMissionSetup[] componentsInChildren = this.ground.GetComponentsInChildren<global::ICustomMissionSetup>();
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			componentsInChildren[l].Execute();
		}
		global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<uint, uint>> objectives = global::PandoraSingleton<global::MissionStartData>.Instance.objectives;
		for (int m = 0; m < objectives.Count; m++)
		{
			for (int n = 0; n < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; n++)
			{
				for (int num2 = 0; num2 < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[n].objectives.Count; num2++)
				{
					if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[n].objectives[num2].guid == objectives[m].Key)
					{
						global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[n].objectives[num2].Reload(objectives[m].Value);
					}
				}
			}
		}
		for (int num3 = 0; num3 < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; num3++)
		{
			global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[num3];
			if (warbandController.IsPlayed())
			{
				for (int num4 = 0; num4 < warbandController.objectives.Count; num4++)
				{
					warbandController.objectives[num4].CheckObjective();
				}
			}
			global::System.Collections.Generic.List<uint> list2;
			if (warbandController.saveIdx < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count)
			{
				list2 = global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[warbandController.saveIdx].openedSearches;
			}
			else
			{
				list2 = new global::System.Collections.Generic.List<uint>();
			}
			for (int num5 = 0; num5 < list2.Count; num5++)
			{
				for (int num6 = 0; num6 < global::PandoraSingleton<global::MissionManager>.Instance.interactivePoints.Count; num6++)
				{
					if (global::PandoraSingleton<global::MissionManager>.Instance.interactivePoints[num6].guid == list2[num5])
					{
						warbandController.openedSearch.Add((global::SearchPoint)global::PandoraSingleton<global::MissionManager>.instance.interactivePoints[num6]);
						break;
					}
				}
			}
		}
		for (int num7 = 0; num7 < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; num7++)
		{
			global::WarbandController warbandController2 = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[num7];
		}
		global::PandoraSingleton<global::MissionManager>.Instance.UpdateObjectivesUI(false);
	}

	private global::System.Collections.IEnumerator GenerateNavMesh()
	{
		foreach (object obj in global::AstarPath.active.astarData.FindGraphsOfType(typeof(global::Pathfinding.RecastGraph)))
		{
			global::Pathfinding.RecastGraph rg = (global::Pathfinding.RecastGraph)obj;
			rg.forcedBoundsSize = this.ground.GetComponent<global::BoundingBox>().size;
			rg.forcedBoundsCenter = this.ground.GetComponent<global::BoundingBox>().center;
		}
		global::AstarPath.active.graphUpdateBatchingInterval = 1.5f;
		foreach (global::Pathfinding.Progress p in global::AstarPath.active.ScanAsync())
		{
			this.CurrentPartsPercent = p.progress;
			yield return null;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.tileHandlerHelper = global::AstarPath.active.gameObject.AddComponent<global::Pathfinding.TileHandlerHelper>();
		global::PandoraSingleton<global::MissionManager>.Instance.tileHandlerHelper.updateInterval = -1f;
		global::PandoraSingleton<global::MissionManager>.Instance.tileHandlerHelper.ForceUpdate();
		global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker = global::AstarPath.active.gameObject.AddComponent<global::Seeker>();
		global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.DeregisterModifier(global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.startEndModifier);
		global::Pathfinding.RayPathModifier pathModifier = global::AstarPath.active.gameObject.AddComponent<global::Pathfinding.RayPathModifier>();
		global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.DeregisterModifier(pathModifier);
		global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.RegisterModifier(pathModifier);
		global::PandoraSingleton<global::MissionManager>.Instance.pathRayModifier = pathModifier;
		for (int z = 0; z < this.actionZones.Count; z++)
		{
			global::ActionZone zone = this.actionZones[z];
			for (int d = 0; d < zone.destinations.Count; d++)
			{
				global::ActionDestination dest = zone.destinations[d];
				uint zoneTag = 0U;
				switch (zone.destinations[d].actionId)
				{
				case global::UnitActionId.CLIMB_3M:
				case global::UnitActionId.CLIMB_6M:
				case global::UnitActionId.CLIMB_9M:
					zoneTag = ((!dest.destination.supportLargeUnit) ? 1U : 4U);
					break;
				case global::UnitActionId.LEAP:
					zoneTag = ((!dest.destination.supportLargeUnit) ? 3U : 6U);
					break;
				case global::UnitActionId.JUMP_3M:
				case global::UnitActionId.JUMP_6M:
				case global::UnitActionId.JUMP_9M:
					zoneTag = ((!dest.destination.supportLargeUnit) ? 2U : 5U);
					break;
				}
				dest.navLink.startNode.Tag = zoneTag;
				dest.navLink.endNode.Tag = zoneTag;
			}
		}
		global::ColliderActivator.ActivateAll();
		yield break;
	}

	private void SetCameraSetter()
	{
		global::CameraSetter cameraSetter = global::UnityEngine.Object.FindObjectOfType<global::CameraSetter>();
		if (cameraSetter)
		{
			cameraSetter.SetCameraInfo(global::PandoraSingleton<global::MissionManager>.Instance.CamManager.GetComponent<global::UnityEngine.Camera>());
		}
	}

	private const int totalParts = 353;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.GameObject ground;

	public int trapCount;

	private global::MissionSave missionSave;

	private global::MissionMapLayoutData mapLayout;

	private global::MissionMapGameplayData mapGameplay;

	private global::DeploymentScenarioMapLayoutData deployMapData;

	private global::System.Collections.Generic.List<global::PropRestrictions> restrictions;

	private global::System.Collections.Generic.List<global::ActionZone> actionZones = new global::System.Collections.Generic.List<global::ActionZone>();

	private global::System.Collections.Generic.List<global::SpawnZone> deployedZones;

	private global::System.Collections.Generic.List<global::SpawnNode> spawnNodes;

	private global::System.Collections.Generic.List<global::SearchZone> searchZones;

	private global::System.Collections.Generic.List<global::SearchNode> allSearchNodes;

	private global::System.Collections.Generic.List<global::SearchNode> searchNodes;

	private global::System.Collections.Generic.List<global::PropNode> totPropNodes;

	private global::System.Collections.Generic.List<global::BuildingNode> totBuildingNodes;

	private global::System.Collections.Generic.List<global::Trap> traps;

	private global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::TrapTypeData, bool>> trapData;

	private global::System.Collections.Generic.List<global::TrapNode> trapNodes;

	private global::System.Collections.Generic.List<global::WyrdstoneTypeData> wyrdTypeData;

	private global::System.Collections.Generic.List<global::SearchDensityLootData> rewards;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> groundLayers;

	private global::System.Collections.Generic.List<string> groundLayersOrder;

	private global::Tyche networkTyche;

	private int lowestWarbandRank;

	private int[] linkCount;

	private int[] linkedCount;

	private global::System.Collections.Generic.List<string> jobs;

	private global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<global::UnityEngine.Transform, global::UnityEngine.Transform>> parents;

	private global::System.Collections.Generic.List<int> rotations;

	private global::System.Collections.Generic.List<global::PropNode> tempPropNodes;

	private global::System.Collections.Generic.List<global::BuildingNode> tempBuildingNodes;

	private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject> assetLibrary;

	private string currentAssetbundleName;

	private int loadingPlanks;

	private float startTime;

	private readonly int[] LOADING_PARTS = new int[]
	{
		10,
		50,
		50,
		5,
		50,
		5,
		5,
		50,
		10,
		5,
		5,
		1,
		100,
		5,
		1,
		1
	};

	private int currentParts;

	private float currentPartsPercent;

	private int loadingPartsIndex;

	private int loadingZoneAoe;

	public int percent;

	private int jobLoaded;

	private readonly global::System.Collections.Generic.List<global::ActionNode> aNodes = new global::System.Collections.Generic.List<global::ActionNode>();

	private int wagonQueued;

	private int loadingTraps;
}
