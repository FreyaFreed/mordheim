using System;
using System.Collections.Generic;
using Pathfinding;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : global::TriggerPoint
{
	public global::DestructibleData Data { get; private set; }

	public global::UnitController Owner { get; private set; }

	public bool Active
	{
		get
		{
			return this.CurrentWounds > 0;
		}
	}

	public int CurrentWounds { get; set; }

	public global::MapImprint Imprint { get; private set; }

	public int TeamIdx { get; private set; }

	public global::UnityEngine.Collider Collider { get; private set; }

	public string LocalizedName { get; set; }

	public static void Spawn(global::DestructibleId destructibleId, global::UnitController owner, global::UnityEngine.Vector3 pos, int wounds = -1)
	{
		string str = destructibleId.ToLowerString();
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/idols/", global::AssetBundleId.PROPS, str + ".prefab", delegate(global::UnityEngine.Object obj)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)obj);
			gameObject.transform.position = pos;
			gameObject.transform.rotation = ((!(owner != null)) ? global::UnityEngine.Quaternion.identity : owner.transform.rotation);
			global::Destructible component = gameObject.GetComponent<global::Destructible>();
			component.Init(destructibleId, owner);
			component.name += ((!(owner != null)) ? string.Empty : owner.name);
			if (wounds != -1)
			{
				component.CurrentWounds = wounds;
			}
		});
	}

	public void AutoInit()
	{
		if (this.id != global::DestructibleId.NONE && this.autoInit)
		{
			this.Init(this.id, null);
		}
	}

	public void Init(global::DestructibleId destructibleId, global::UnitController owner)
	{
		base.Init();
		this.id = destructibleId;
		this.Owner = owner;
		this.guid = ((!(owner != null)) ? global::PandoraSingleton<global::MissionManager>.Instance.GetNextEnvGUID() : global::PandoraSingleton<global::MissionManager>.Instance.GetNextRTGUID());
		this.TeamIdx = ((!(owner != null)) ? -1 : owner.GetWarband().teamIdx);
		this.LocalizedName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.id.ToLowerString());
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DestructibleData>((int)this.id);
		this.CurrentWounds = this.Data.Wounds;
		if (this.Data.ZoneAoeId != global::ZoneAoeId.NONE)
		{
			global::ZoneAoe.Spawn(this.Data.ZoneAoeId, (float)this.Data.ZoneAoeRadius, base.transform.position, owner, false, delegate(global::ZoneAoe zone)
			{
				this.zoneAoe = zone;
				this.zoneAoe.destructible = this;
			});
		}
		this.dissolver = base.gameObject.AddComponent<global::Dissolver>();
		if (this.imprintIcon != null)
		{
			this.Imprint = base.gameObject.AddComponent<global::MapImprint>();
			this.Imprint.Init(this.imprintIcon, null, false, global::MapImprintType.DESTRUCTIBLE, null, null, null, null, this);
			if (owner != null && owner.IsPlayed())
			{
				this.Imprint.AddViewer(owner);
			}
		}
		global::Pathfinding.NavmeshCut componentInChildren = base.GetComponentInChildren<global::Pathfinding.NavmeshCut>();
		if (componentInChildren != null)
		{
			componentInChildren.ForceUpdate();
		}
		global::UnityEngine.Collider[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].isTrigger)
			{
				this.Collider = componentsInChildren[i];
			}
			else
			{
				this.triggerCol = componentsInChildren[i];
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Add(this);
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.AddDestructible(this);
	}

	public void ApplyDamage(int damage)
	{
		this.CurrentWounds -= damage;
		this.lastReceivedWounds = -damage;
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateDestructible(this);
	}

	public void Hit(global::UnitController damageDealer)
	{
		global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText(global::FlyingTextId.ACTION, delegate(global::FlyingText fl)
		{
			((global::FlyingLabel)fl).Play(base.transform.position + global::UnityEngine.Vector3.up * 1.5f, true, "com_value", new string[]
			{
				this.lastReceivedWounds.ToConstantString()
			});
		});
		if (this.CurrentWounds <= 0)
		{
			this.Deactivate();
			if (damageDealer != null)
			{
				damageDealer.GetWarband().DestroyDestructible(base.name);
			}
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Destructible>(global::Notices.RETROACTION_SHOW_OUTCOME, this);
	}

	public void Deactivate()
	{
		this.CurrentWounds = 0;
		for (int i = 0; i < this.projectiles.Count; i++)
		{
			if (this.projectiles[i] != null && this.projectiles[i].gameObject != null)
			{
				global::UnityEngine.Object.Destroy(this.projectiles[i].gameObject);
			}
		}
		this.projectiles.Clear();
		if (this.zoneAoe != null)
		{
			this.zoneAoe.Deactivate();
		}
		global::PandoraSingleton<global::MissionManager>.Instance.UnregisterDestructible(this);
		if (!string.IsNullOrEmpty(this.fxOnDestroy))
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.fxOnDestroy, base.transform, false, null);
		}
		this.dissolver.Hide(true, false, new global::UnityEngine.Events.UnityAction(this.OnDissolved));
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateDestructible(this);
	}

	private void OnDissolved()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
		global::PandoraSingleton<global::MissionManager>.Instance.RefreshGraph();
	}

	public bool IsInRange(global::UnityEngine.Vector3 src, float maxDistance)
	{
		global::UnityEngine.Vector3 a = base.transform.position + global::UnityEngine.Vector3.up;
		global::UnityEngine.Vector3 direction = a - src;
		direction.Normalize();
		global::UnityEngine.RaycastHit raycastHit;
		bool flag = global::UnityEngine.Physics.Raycast(src, direction, out raycastHit, maxDistance, global::LayerMaskManager.groundMask);
		return raycastHit.collider == this.Collider;
	}

	public global::DestructibleId id;

	public bool autoInit;

	public global::UnityEngine.Sprite imprintIcon;

	public global::ZoneAoe zoneAoe;

	private global::UnityEngine.Collider triggerCol;

	private global::Dissolver dissolver;

	private int lastReceivedWounds;

	public bool destroyOnAoeDestroy = true;

	public global::System.Collections.Generic.List<global::Projectile> projectiles = new global::System.Collections.Generic.List<global::Projectile>();

	public string fxOnDestroy;
}
