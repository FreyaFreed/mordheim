using System;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
public class UnitMenuController : global::UnityEngine.MonoBehaviour
{
	public global::System.Collections.Generic.List<global::ItemController> Equipments { get; protected set; }

	public global::HighlightingSystem.Highlighter Highlight { get; protected set; }

	public global::System.Collections.Generic.Dictionary<global::BoneId, global::UnityEngine.Transform> BonesTr { get; private set; }

	public static global::System.Collections.IEnumerator LoadUnitPrefabAsync(global::Unit unit, global::System.Action<global::UnityEngine.GameObject> callback, global::System.Action finishedLoading)
	{
		string unitBase = unit.Data.UnitBaseId.ToString().ToLower();
		string prefab = "prefabs/characters/" + unitBase + "_menu";
		float startTime = global::UnityEngine.Time.realtimeSinceStartup;
		global::UnityEngine.ResourceRequest res = global::UnityEngine.Resources.LoadAsync<global::UnityEngine.GameObject>(prefab);
		yield return res;
		global::UnityEngine.GameObject instance = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(res.asset);
		instance.name = string.Concat(new object[]
		{
			unitBase,
			"_",
			unit.Id,
			"_",
			unit.Name
		});
		instance.AddComponent<global::AnimatorPlayerEvents>();
		global::UnitMenuController ctrlr = instance.GetComponent<global::UnitMenuController>();
		ctrlr.asyncQueued = 0;
		ctrlr.totalLoaded = 0;
		ctrlr.finishedLoad = finishedLoading;
		ctrlr.SetCharacter(unit);
		yield return null;
		ctrlr.LaunchBodyPartsLoading(new global::System.Action(ctrlr.FinishLoadingBodyParts), true);
		callback(instance);
		yield break;
	}

	protected virtual void Awake()
	{
		this.skinnedMeshCombiner = base.GetComponent<global::SkinnedMeshCombiner>();
		this.shaderSetter = base.GetComponent<global::ShaderSetter>();
		this.animator = base.GetComponent<global::UnityEngine.Animator>();
		this.animator.stabilizeFeet = true;
		this.dissolver = base.GetComponent<global::Dissolver>();
		if (this.dissolver == null)
		{
			this.dissolver = base.gameObject.AddComponent<global::Dissolver>();
		}
		this.Highlight = base.GetComponent<global::HighlightingSystem.Highlighter>();
		if (this.Highlight == null)
		{
			this.Highlight = base.gameObject.AddComponent<global::HighlightingSystem.Highlighter>();
		}
		this.Highlight.seeThrough = false;
		if (base.GetComponent<global::UnityEngine.AudioSource>() == null)
		{
			base.StartCoroutine(this.LoadSoundBaseAsync());
		}
	}

	private global::System.Collections.IEnumerator LoadSoundBaseAsync()
	{
		yield return null;
		global::UnityEngine.ResourceRequest req = global::UnityEngine.Resources.LoadAsync("prefabs/sound_base");
		yield return req;
		global::UnityEngine.GameObject go = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(req.asset);
		go.transform.SetParent(base.transform);
		go.transform.localPosition = global::UnityEngine.Vector3.zero;
		go.transform.localRotation = global::UnityEngine.Quaternion.identity;
		this.audioSource = go.GetComponent<global::UnityEngine.AudioSource>();
		this.audioSource.loop = false;
		this.audioSource.playOnAwake = false;
		yield break;
	}

	protected virtual void LateUpdate()
	{
		if (this.IsAnimating())
		{
			this.animator.SetInteger(global::AnimatorIds.action, 0);
			this.animator.SetInteger(global::AnimatorIds.atkResult, 0);
			this.animator.SetFloat(global::AnimatorIds.emoteVariation, 0f);
		}
	}

	public void MergeNoAtlas()
	{
		if (this.skinnedMeshCombiner != null)
		{
			this.skinnedMeshCombiner.MergeNoAtlas();
		}
	}

	public void SetCharacter(global::Unit unitInfo)
	{
		this.unit = unitInfo;
		this.InitializeBones();
		this.InitBodyTrails();
		this.InstantiateAllEquipment();
	}

	public void InitializeBones()
	{
		this.BonesTr = new global::System.Collections.Generic.Dictionary<global::BoneId, global::UnityEngine.Transform>();
		global::System.Collections.Generic.List<global::BoneData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BoneData>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			global::UnityEngine.Transform child = base.transform.GetChild(i);
			for (int j = 0; j < list.Count; j++)
			{
				if (child.name == list[j].Name)
				{
					this.BonesTr[list[j].Id] = child;
					child.gameObject.SetLayerRecursively(global::UnityEngine.LayerMask.NameToLayer("cloth"));
					break;
				}
			}
		}
	}

	public void InitCloth()
	{
		this.cloths.Clear();
		base.gameObject.GetComponentsInChildren<global::UnityEngine.Cloth>(true, this.cloths);
	}

	public void InitBodyTrails()
	{
		this.bodyPartTrails.Clear();
		foreach (global::BodyPart bodyPart in this.unit.bodyParts.Values)
		{
			for (int i = 0; i < bodyPart.relatedGO.Count; i++)
			{
				bodyPart.relatedGO[i].SetActive(true);
				this.bodyPartTrails.AddRange(bodyPart.relatedGO[i].GetComponentsInChildren<global::WeaponTrail>());
			}
		}
		for (int j = 0; j < this.bodyPartTrails.Count; j++)
		{
			this.bodyPartTrails[j].Emit(false);
		}
	}

	public void SetModelVariation(global::BodyPartId bodyPart, int index, bool reload = true)
	{
		global::System.Collections.Generic.KeyValuePair<int, int> value = new global::System.Collections.Generic.KeyValuePair<int, int>(index, this.unit.UnitSave.customParts[bodyPart].Value);
		this.unit.UnitSave.customParts[bodyPart] = value;
		this.unit.bodyParts[bodyPart].SetVariation(index);
		if (bodyPart == global::BodyPartId.ARML || bodyPart == global::BodyPartId.ARMR)
		{
			this.SetModelVariation((bodyPart != global::BodyPartId.ARML) ? global::BodyPartId.HANDR : global::BodyPartId.HANDL, index, false);
		}
		if (reload)
		{
			this.LoadBodyParts();
		}
	}

	public void SetBodyPartColor(global::BodyPartId bodyPart, int color)
	{
		global::System.Collections.Generic.KeyValuePair<int, int> value = new global::System.Collections.Generic.KeyValuePair<int, int>(this.unit.UnitSave.customParts[bodyPart].Key, color);
		this.unit.UnitSave.customParts[bodyPart] = value;
		this.unit.bodyParts[bodyPart].SetColorOverride(color);
		this.LoadBodyParts();
		if (bodyPart == global::BodyPartId.BODY && this.unit.bodyParts.ContainsKey(global::BodyPartId.GEAR_BODY))
		{
			this.SetBodyPartColor(global::BodyPartId.GEAR_BODY, color);
		}
		if (bodyPart == global::BodyPartId.ARMR || bodyPart == global::BodyPartId.ARML)
		{
			this.SetBodyPartColor((bodyPart != global::BodyPartId.ARMR) ? global::BodyPartId.HANDL : global::BodyPartId.HANDR, color);
		}
	}

	public void SetColorPreset(global::ColorPresetId presetId)
	{
		int num = (int)((int)presetId << 8);
		foreach (global::BodyPart bodyPart in this.unit.bodyParts.Values)
		{
			global::System.Collections.Generic.KeyValuePair<int, int> value = new global::System.Collections.Generic.KeyValuePair<int, int>(this.unit.UnitSave.customParts[bodyPart.Id].Key, num);
			this.unit.UnitSave.customParts[bodyPart.Id] = value;
			bodyPart.SetColorPreset(num);
		}
		this.LoadBodyParts();
	}

	public void SetSkinColor(string color)
	{
		this.unit.UnitSave.skinColor = color;
		foreach (global::BodyPart bodyPart in this.unit.bodyParts.Values)
		{
			bodyPart.SetSkinColor(color);
		}
		this.LoadBodyParts();
	}

	public void LoadBodyParts()
	{
		this.LaunchBodyPartsLoading(new global::System.Action(this.OnBodyPartCustomization), true);
	}

	private void OnBodyPartCustomization()
	{
		for (int i = 0; i < this.Equipments.Count; i++)
		{
			if (this.Equipments[i] != null && this.Equipments[i].gameObject != null)
			{
				this.Equipments[i].gameObject.SetActive(true);
			}
		}
		this.animator.Rebind();
		foreach (global::BodyPart bodyPart in this.unit.bodyParts.Values)
		{
			for (int j = 0; j < bodyPart.relatedGO.Count; j++)
			{
				bodyPart.relatedGO[j].SetActive(true);
			}
		}
		if (this.shaderSetter != null)
		{
			this.shaderSetter.ApplyShaderParams();
		}
		if (this.skinnedMeshCombiner != null)
		{
			this.skinnedMeshCombiner.AttachAttachers();
		}
		this.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		this.Hide(false, false, null);
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadScenes();
	}

	public void LaunchBodyPartsLoading(global::System.Action callback, bool noLOD = true)
	{
		global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(this.LoadBodyPartsAsync(callback, noLOD));
	}

	public global::System.Collections.IEnumerator LoadBodyPartsAsync(global::System.Action callback, bool noLOD = false)
	{
		this.bodyPartsLoadCB = callback;
		for (int i = 0; i < this.Equipments.Count; i++)
		{
			if (this.Equipments[i] != null && this.Equipments[i].gameObject != null)
			{
				this.Equipments[i].gameObject.SetActive(false);
			}
		}
		bool loading = false;
		global::ItemTypeId preferredItemTypeId = global::ItemTypeId.NONE;
		global::Item bodyItem = null;
		if (this.unit.bodyParts.ContainsKey(global::BodyPartId.BODY))
		{
			bodyItem = this.unit.bodyParts[global::BodyPartId.BODY].GetRelatedItem();
		}
		if (bodyItem != null)
		{
			preferredItemTypeId = bodyItem.TypeData.Id;
		}
		foreach (global::System.Collections.Generic.KeyValuePair<global::BodyPartId, global::BodyPart> p in this.unit.bodyParts)
		{
			global::BodyPart part = p.Value;
			if (part.AssetNeedReload)
			{
				part.DestroyRelatedGO();
				string partName = part.GetAsset(preferredItemTypeId);
				if (string.IsNullOrEmpty(partName))
				{
					part.SetColorOverride(0);
					partName = part.GetAsset(preferredItemTypeId);
				}
				part.AssetNeedReload = false;
				if (!part.Empty)
				{
					int bpId = (int)p.Key;
					this.LoadBodyPart(part.AssetBundle, partName, bpId, noLOD);
					loading = true;
				}
			}
		}
		if (!loading && this.asyncQueued == 0 && this.bodyPartsLoadCB != null)
		{
			this.bodyPartsLoadCB();
			this.bodyPartsLoadCB = null;
		}
		yield return null;
		yield break;
	}

	public float GetBodypartPercentLoaded()
	{
		if (this.totalQueued == 0)
		{
			return 0f;
		}
		return (float)this.totalLoaded / (float)this.totalQueued;
	}

	public void LoadBodyPart(string assetBundle, string partName, int bpId, bool noLOD)
	{
		this.asyncQueued++;
		this.totalQueued++;
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadSceneAssetAsync(partName, assetBundle, delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject go2 = global::UnityEngine.SceneManagement.SceneManager.GetSceneByName(partName).GetRootGameObjects()[0];
			this.asyncQueued--;
			this.totalLoaded++;
			this.OnBodyPartLoaded(go2, bpId, noLOD);
		});
	}

	public void OnBodyPartLoaded(global::UnityEngine.GameObject go, int bpId, bool noLOD)
	{
		global::BodyPart LambdaPart = this.unit.bodyParts[(global::BodyPartId)bpId];
		if (this.skinnedMeshCombiner == null || base.gameObject == null)
		{
			return;
		}
		global::UnityEngine.GameObject gameObject;
		if (go == null)
		{
			global::PandoraDebug.LogWarning(string.Concat(new object[]
			{
				"Couldn't find asset in asset bundle ",
				LambdaPart.AssetBundle,
				" ,for body part ",
				LambdaPart.Id
			}), "uncategorised", null);
			gameObject = new global::UnityEngine.GameObject();
			gameObject.transform.position = this.skinnedMeshCombiner.transform.position;
			gameObject.transform.rotation = this.skinnedMeshCombiner.transform.rotation;
		}
		else
		{
			gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go, this.skinnedMeshCombiner.transform.position, this.skinnedMeshCombiner.transform.rotation);
		}
		gameObject.SetLayerRecursively(global::UnityEngine.LayerMask.NameToLayer("characters"));
		gameObject.transform.SetParent(this.skinnedMeshCombiner.transform, true);
		LambdaPart.relatedGO = this.skinnedMeshCombiner.AttachGameObject(gameObject, noLOD);
		if (LambdaPart.MutationId != global::MutationId.NONE)
		{
			global::System.Collections.Generic.List<global::MutationFxData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationFxData>(new string[]
			{
				"fk_mutation_id",
				"fk_unit_id"
			}, new string[]
			{
				((int)LambdaPart.MutationId).ToString(),
				((int)this.unit.Id).ToString()
			});
			if (list != null && list.Count > 0)
			{
				global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(list[0].Asset, this, null, delegate(global::UnityEngine.GameObject fx)
				{
					if (fx != null)
					{
						LambdaPart.relatedGO.Add(fx);
					}
				}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
			}
		}
		for (int i = 0; i < LambdaPart.relatedGO.Count; i++)
		{
			LambdaPart.relatedGO[i].SetActive(false);
		}
		if (this.asyncQueued == 0 && this.bodyPartsLoadCB != null)
		{
			this.bodyPartsLoadCB();
			this.bodyPartsLoadCB = null;
		}
		LambdaPart.AssetNeedReload = false;
	}

	public void FinishLoadingBodyParts()
	{
		foreach (global::BodyPart bodyPart in this.unit.bodyParts.Values)
		{
			for (int i = 0; i < bodyPart.relatedGO.Count; i++)
			{
				bodyPart.relatedGO[i].SetActive(true);
			}
		}
		for (int j = 0; j < this.Equipments.Count; j++)
		{
			if (this.Equipments[j] != null && this.Equipments[j].gameObject != null)
			{
				this.Equipments[j].gameObject.SetActive(true);
			}
		}
		this.Hide(!this.visible, !this.visible, null);
		this.animator.Rebind();
		if (this.shaderSetter != null)
		{
			this.shaderSetter.ApplyShaderParams();
		}
		if (this.skinnedMeshCombiner != null)
		{
			this.skinnedMeshCombiner.AttachAttachers();
		}
		this.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		this.InitCloth();
		if (this.Highlight != null)
		{
			this.Highlight.ReinitMaterials();
		}
		if (this.finishedLoad != null)
		{
			this.finishedLoad();
		}
	}

	public void RefreshBodyParts()
	{
		this.unit.ResetBodyPart();
		this.LoadBodyParts();
	}

	public bool IsAnimating()
	{
		int fullPathHash = this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
		return fullPathHash != global::AnimatorIds.idle && fullPathHash != global::AnimatorIds.kneeling_stunned && fullPathHash != global::AnimatorIds.climb_sheathe && fullPathHash != global::AnimatorIds.jump_sheathe && fullPathHash != global::AnimatorIds.leap_sheathe && fullPathHash != global::AnimatorIds.search_idle;
	}

	public void SetAnimStyle()
	{
		this.animator.SetFloat(global::AnimatorIds.type, (float)this.unit.currentAnimStyleId);
		this.animator.SetInteger(global::AnimatorIds.style, (int)this.unit.currentAnimStyleId);
	}

	public void LaunchAction(global::UnitActionId id, bool success, global::UnitStateId stateId, int variation = 0)
	{
		this.currentActionId = id;
		if (this.seqData.action > 0)
		{
			this.animator.SetInteger(global::AnimatorIds.action, this.seqData.action);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, this.seqData.actionSuccess);
			this.animator.SetInteger(global::AnimatorIds.unit_state, this.seqData.unitState);
			this.animator.SetFloat(global::AnimatorIds.emoteVariation, (float)this.seqData.emoteVariation);
			this.animator.SetInteger(global::AnimatorIds.variation, this.seqData.attackVariation);
			this.seqData.action = 0;
			this.seqData.actionSuccess = false;
			this.seqData.unitState = 0;
			this.seqData.emoteVariation = 0;
			this.seqData.attackVariation = 0;
		}
		else
		{
			this.animator.SetInteger(global::AnimatorIds.action, (int)id);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, success);
			this.animator.SetInteger(global::AnimatorIds.unit_state, (int)stateId);
			this.animator.SetInteger(global::AnimatorIds.variation, variation);
			this.animator.SetFloat(global::AnimatorIds.emoteVariation, 0f);
		}
	}

	public void PlayDefState(global::AttackResultId resultId, int variation, global::UnitStateId stateId)
	{
		this.animator.SetInteger(global::AnimatorIds.atkResult, (int)resultId);
		this.animator.SetInteger(global::AnimatorIds.variation, variation);
		this.animator.SetInteger(global::AnimatorIds.unit_state, (int)stateId);
		this.SetStatusFX();
	}

	public void PlayBuffDebuff(global::EffectTypeId effectId)
	{
		if (effectId == global::EffectTypeId.BUFF || effectId == global::EffectTypeId.DEBUFF)
		{
			this.animator.SetInteger(global::AnimatorIds.action, 40);
			this.animator.SetInteger(global::AnimatorIds.variation, (effectId != global::EffectTypeId.BUFF) ? 1 : 0);
		}
	}

	public virtual void SetAnimSpeed(float speed)
	{
		this.animator.SetFloat(global::AnimatorIds.speed, speed);
	}

	public void SetStatusFX()
	{
		if (this.statusFx != null)
		{
			global::UnityEngine.Object.Destroy(this.statusFx);
		}
		global::UnitStateData unitStateData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitStateData>((int)this.unit.Status);
		if (!string.IsNullOrEmpty(unitStateData.Fx))
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(unitStateData.Fx, this.unit, delegate(global::UnityEngine.GameObject fx)
			{
				this.statusFx = fx;
			});
		}
	}

	public bool HasClose()
	{
		return this.Equipments[(int)this.unit.ActiveWeaponSlot] != null && this.Equipments[(int)this.unit.ActiveWeaponSlot].Item.TypeData != null && !this.Equipments[(int)this.unit.ActiveWeaponSlot].Item.TypeData.IsRange;
	}

	public bool HasRange()
	{
		return this.Equipments[(int)this.unit.ActiveWeaponSlot] != null && this.Equipments[(int)this.unit.ActiveWeaponSlot].Item.TypeData != null && this.Equipments[(int)this.unit.ActiveWeaponSlot].Item.TypeData.IsRange;
	}

	public bool IsAltClose()
	{
		return this.Equipments[(int)this.unit.InactiveWeaponSlot] != null && this.Equipments[(int)this.unit.InactiveWeaponSlot].Item.TypeData != null && !this.Equipments[(int)this.unit.InactiveWeaponSlot].Item.TypeData.IsRange;
	}

	public bool IsAltRange()
	{
		return this.Equipments[(int)this.unit.InactiveWeaponSlot] != null && this.Equipments[(int)this.unit.InactiveWeaponSlot].Item.TypeData != null && this.Equipments[(int)this.unit.InactiveWeaponSlot].Item.TypeData.IsRange;
	}

	public bool CanSwitchWeapon()
	{
		return this.unit.CanSwitchWeapon();
	}

	public void SwitchWeapons(global::UnitSlotId nextWeaponSlot)
	{
		for (int i = 2; i <= 5; i++)
		{
			if ((i != (int)nextWeaponSlot || i != (int)(nextWeaponSlot + 1)) && this.Equipments[i] != null)
			{
				this.Equipments[i].gameObject.SetActive(false);
			}
		}
		this.unit.ActiveWeaponSlot = nextWeaponSlot;
		global::AnimStyleId currentAnimStyleId = global::AnimStyleId.NONE;
		if (this.Equipments[(int)nextWeaponSlot] != null)
		{
			this.Equipments[(int)nextWeaponSlot].gameObject.SetActive(true);
			this.Equipments[(int)nextWeaponSlot].Unsheathe(this.BonesTr, false);
			currentAnimStyleId = this.Equipments[(int)nextWeaponSlot].Item.StyleData.Id;
			if (this.Equipments[(int)(nextWeaponSlot + 1)] != null)
			{
				this.Equipments[(int)(nextWeaponSlot + 1)].gameObject.SetActive(true);
				this.Equipments[(int)(nextWeaponSlot + 1)].Unsheathe(this.BonesTr, true);
				if (this.Equipments[(int)(nextWeaponSlot + 1)].Item.TypeData.Id == global::ItemTypeId.MELEE_1H && this.Equipments[(int)(nextWeaponSlot + 1)].Item.StyleData.Id != global::AnimStyleId.NONE)
				{
					currentAnimStyleId = global::AnimStyleId.DUAL_WIELD;
				}
				else if (this.Equipments[(int)(nextWeaponSlot + 1)].Item.TypeData.Id == global::ItemTypeId.SHIELD)
				{
					currentAnimStyleId = this.Equipments[(int)nextWeaponSlot].Item.StyleData.Id + 1;
				}
			}
		}
		this.unit.currentAnimStyleId = currentAnimStyleId;
		this.SetAnimStyle();
	}

	protected void InstantiateAllEquipment()
	{
		this.Equipments = new global::System.Collections.Generic.List<global::ItemController>();
		for (int i = 0; i < this.unit.Items.Count; i++)
		{
			this.Equipments.Add(null);
		}
		this.RefreshEquipments(null);
		this.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
	}

	public global::System.Collections.Generic.List<global::ItemSave> EquipItem(global::UnitSlotId slot, global::ItemId itemId)
	{
		return this.EquipItem(slot, new global::ItemSave(itemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1));
	}

	private void ShowWeapons()
	{
		this.Hide(false, true, null);
	}

	public global::System.Collections.Generic.List<global::ItemSave> EquipItem(global::UnitSlotId slot, global::ItemSave itemSave)
	{
		global::System.Collections.Generic.List<global::Item> list = this.unit.EquipItem(slot, itemSave);
		global::UnitSlotData unitSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitSlotData>((int)slot);
		switch (unitSlotData.UnitSlotTypeId)
		{
		case global::UnitSlotTypeId.BODY_PART:
			this.HideAndRefreshBodyParts();
			break;
		case global::UnitSlotTypeId.EQUIPMENT:
			this.RefreshEquipments(new global::System.Action(this.ShowWeapons));
			break;
		}
		global::System.Collections.Generic.List<global::ItemSave> list2 = new global::System.Collections.Generic.List<global::ItemSave>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].Save);
		}
		if (this.Highlight)
		{
			this.Highlight.ReinitMaterials();
		}
		return list2;
	}

	private void HideAndRefreshBodyParts()
	{
		this.Hide(true, false, null);
		this.RefreshBodyParts();
	}

	public void RefreshEquipments(global::System.Action callback = null)
	{
		if (callback != null)
		{
			this.bodyPartsLoadCB = callback;
		}
		for (int i = 0; i < 6; i++)
		{
			global::UnitSlotData unitSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitSlotData>(i);
			if (unitSlotData.UnitSlotTypeId == global::UnitSlotTypeId.EQUIPMENT)
			{
				int index2 = i;
				if (this.unit.Items[i - 1].IsPaired)
				{
					index2 = i - 1;
				}
				if ((this.Equipments[i] == null && this.unit.Items[index2].Id != global::ItemId.NONE) || (this.Equipments[i] != null && this.Equipments[i].Item != this.unit.Items[index2]))
				{
					if (this.Equipments[i] != null)
					{
						global::UnityEngine.Object.Destroy(this.Equipments[i].gameObject);
					}
					this.Equipments[i] = null;
					if (this.unit.Items[index2].Id != global::ItemId.NONE)
					{
						int index = i;
						this.asyncQueued++;
						global::ItemController.Instantiate(this.unit.Items[index2], this.unit.RaceId, this.unit.WarbandId, this.unit.Id, (global::UnitSlotId)index, delegate(global::ItemController ic)
						{
							this.asyncQueued--;
							this.Equipments[index] = ic;
							this.tempItemsRenderers.Clear();
							ic.GetComponentsInChildren<global::UnityEngine.Renderer>(true, this.tempItemsRenderers);
							for (int j = 0; j < this.tempItemsRenderers.Count; j++)
							{
								this.tempItemsRenderers[j].enabled = false;
							}
							if (this.Equipments[index] != null)
							{
								this.Equipments[index].Unsheathe(this.BonesTr, index == 3 || index == 5);
							}
							if (this.asyncQueued == 0)
							{
								if (this.bodyPartsLoadCB != null)
								{
									this.bodyPartsLoadCB();
									this.bodyPartsLoadCB = null;
								}
								this.SwitchWeapons(this.unit.ActiveWeaponSlot);
							}
						});
					}
				}
			}
		}
	}

	public virtual void EventDissolve()
	{
		this.Hide(true, false, null);
	}

	public virtual void Hide(bool hide, bool force = false, global::UnityEngine.Events.UnityAction onDissolved = null)
	{
		this.visible = !hide;
		this.dissolver.Hide(hide, force, onDissolved);
	}

	public void InstantMove(global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		for (int i = 0; i < this.cloths.Count; i++)
		{
			if (this.cloths[i] != null)
			{
				this.cloths[i].enabled = false;
			}
		}
		base.transform.position = pos;
		base.transform.rotation = rot;
		for (int j = 0; j < this.cloths.Count; j++)
		{
			if (this.cloths[j] != null)
			{
				this.cloths[j].enabled = true;
			}
		}
	}

	private const string BODY_PARTS_FOLDER = "Assets/prefabs/characters/";

	private const string MATERIAL_FOLDER = "Assets/3d_assets/characters/";

	private const int MAX_DEFAULT_SKIN_COLOR_ITERATION = 3;

	[global::UnityEngine.HideInInspector]
	public global::Unit unit;

	private global::SkinnedMeshCombiner skinnedMeshCombiner;

	protected global::ShaderSetter shaderSetter;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Animator animator;

	protected global::UnitActionId currentActionId;

	public global::SequenceInfo seqData;

	[global::UnityEngine.HideInInspector]
	protected global::System.Collections.Generic.List<global::UnityEngine.Cloth> cloths = new global::System.Collections.Generic.List<global::UnityEngine.Cloth>();

	protected global::System.Collections.Generic.List<global::WeaponTrail> bodyPartTrails = new global::System.Collections.Generic.List<global::WeaponTrail>();

	public global::Dissolver dissolver;

	private bool visible = true;

	public global::UnityEngine.AudioSource audioSource;

	protected string lastFoot;

	protected string lastShout;

	private int asyncQueued;

	private int totalQueued;

	private int totalLoaded;

	private global::System.Action bodyPartsLoadCB;

	protected global::System.Action finishedLoad;

	private global::UnityEngine.GameObject statusFx;

	private readonly global::System.Collections.Generic.List<global::UnityEngine.Renderer> tempItemsRenderers = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();
}
