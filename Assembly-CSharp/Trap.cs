using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : global::TriggerPoint
{
	public global::TrapEffectData EffectData { get; private set; }

	public int TeamIdx { get; set; }

	public global::MapImprint Imprint { get; private set; }

	public static void SpawnTrap(global::TrapTypeId trapTypeId, int teamIdx, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, global::System.Action cb = null, bool unload = true)
	{
		global::TrapTypeData typeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeData>((int)trapTypeId);
		global::System.Collections.Generic.List<global::TrapTypeJoinTrapData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeJoinTrapData>("fk_trap_type_id", typeData.Id.ToIntString<global::TrapTypeId>());
		string visual = list[global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list.Count)].TrapId.ToLowerString();
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, visual + ".prefab", delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)go);
			if (unload)
			{
				global::UnityEngine.SceneManagement.SceneManager.UnloadScene(visual);
			}
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			global::Trap component = gameObject.GetComponent<global::Trap>();
			uint nextRTGUID = global::PandoraSingleton<global::MissionManager>.Instance.GetNextRTGUID();
			component.Init(typeData, nextRTGUID);
			component.name += nextRTGUID;
			component.TeamIdx = teamIdx;
			component.SetImprint();
			global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Add(component);
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.AddDynamicTrap(component);
			if (cb != null)
			{
				cb();
			}
		});
	}

	public void Init(global::TrapTypeData data, uint id)
	{
		this.defaultType = data.Id;
		global::System.Collections.Generic.List<global::TrapTypeJoinTrapEffectData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeJoinTrapEffectData>("fk_trap_type_id", data.Id.ToIntString<global::TrapTypeId>());
		int index = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list.Count);
		global::TrapEffectId trapEffectId = list[index].TrapEffectId;
		this.EffectData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapEffectData>((int)trapEffectId);
		this.soundName = "trap";
		this.guid = id;
		this.TeamIdx = -1;
		base.Init();
	}

	public void SetImprint()
	{
		bool flag = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().teamIdx == this.TeamIdx;
		if ((flag && this.allyImprintIcon != null) || (!flag && this.enemyImprintIcon != null))
		{
			this.Imprint = base.gameObject.AddComponent<global::MapImprint>();
			this.Imprint.Init((!flag) ? this.enemyImprintIcon : this.allyImprintIcon, null, true, global::MapImprintType.TRAP, null, null, null, this, null);
		}
	}

	private void OnDestroy()
	{
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			int num = global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.IndexOf(this);
			if (num != -1 && global::PandoraSingleton<global::GameManager>.Instance.currentSave != null && !global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.missionSave.isTuto)
			{
				if (this.TeamIdx != -1)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateDynamicTrap(this);
				}
				else
				{
					global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.destroyedTraps.Add(this.guid);
				}
			}
		}
		if (this.Imprint != null)
		{
			global::UnityEngine.Object.Destroy(this.Imprint);
		}
		if (this.trigger != null)
		{
			global::UnityEngine.Object.Destroy(this.trigger.gameObject);
			this.trigger = null;
		}
		if (this.removeObjOnDestroy)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void Trigger(global::UnitController currentUnit)
	{
		if (!string.IsNullOrEmpty(this.EffectData.Fx))
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.EffectData.Fx, this.projectileStart.transform, true, null);
		}
		this.ActionOnUnit(currentUnit);
		if (this.EffectData.ZoneAoeId != global::ZoneAoeId.NONE)
		{
			global::ZoneAoe.Spawn(this.EffectData.ZoneAoeId, (float)this.EffectData.Radius, this.projectileStart.transform.position, currentUnit, true, null);
		}
		base.Trigger(currentUnit);
	}

	public override void ActionOnUnit(global::UnitController currentUnit)
	{
		currentUnit.Hit();
	}

	public global::TrapTypeId defaultType;

	public global::UnityEngine.GameObject projectileStart;

	public bool forceInactive;

	public bool removeObjOnDestroy;

	public global::UnityEngine.Sprite allyImprintIcon;

	public global::UnityEngine.Sprite enemyImprintIcon;

	public global::MapImprintType enemyImprintType;

	private bool debugDis;
}
