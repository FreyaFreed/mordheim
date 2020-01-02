using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

public class ZoneAoe : global::UnityEngine.MonoBehaviour
{
	public bool Initialized { get; private set; }

	public global::UnitController Owner { get; private set; }

	public string Name
	{
		get
		{
			return this.data.Name;
		}
	}

	public static global::ZoneAoe Spawn(global::UnitController ctrlr, global::ActionStatus action, global::System.Action<global::ZoneAoe> cb = null)
	{
		if (action != null && action.ZoneAoeId != global::ZoneAoeId.NONE)
		{
			global::ZoneAoe.Spawn(action.ZoneAoeId, (float)action.Radius, ctrlr.currentSpellTargetPosition, ctrlr, true, cb);
		}
		return null;
	}

	public static void Spawn(global::ZoneAoeId zoneId, float radius, global::UnityEngine.Vector3 position, global::UnitController caster, bool register = true, global::System.Action<global::ZoneAoe> cb = null)
	{
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/zone_aoe/", global::AssetBundleId.FX, zoneId.ToLowerString() + ".prefab", delegate(global::UnityEngine.Object prefab)
		{
			if (prefab == null)
			{
				return;
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)prefab);
			gameObject.transform.position = position;
			gameObject.transform.rotation = global::UnityEngine.Quaternion.identity;
			global::ZoneAoe component = gameObject.GetComponent<global::ZoneAoe>();
			component.Init(zoneId, caster, radius, 1f);
			int minimum = 0;
			int num = 0;
			component.GetDamages(out minimum, out num);
			if (num > 0)
			{
				for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Count; i++)
				{
					if (global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[i] is global::Destructible)
					{
						global::Destructible destructible = (global::Destructible)global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[i];
						if (destructible.IsInRange(position, component.radius))
						{
							int damage = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(minimum, num);
							destructible.ApplyDamage(damage);
							destructible.Hit(caster);
						}
					}
				}
			}
			if (register)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateAoe(component.guid, caster.uid, component.zoneAoeId, radius, component.durationLeft, position);
			}
			if (cb != null)
			{
				cb(component);
			}
		});
	}

	public void AutoInit()
	{
		if (this.autoInit)
		{
			this.autoInit = false;
			this.Activate();
			this.usedOnce = false;
		}
		else
		{
			this.Deactivate();
		}
	}

	private void Update()
	{
		if (!this.Initialized)
		{
			return;
		}
		if (this.autoDestroyOnFxDone && this.fxPresent && this.fx == null)
		{
			this.autoDestroyOnFxDone = false;
			this.fxPresent = false;
			this.Deactivate();
		}
		for (int i = this.unitsToCheck.Count - 1; i >= 0; i--)
		{
			if (this.unitsToCheck[i].ctrlr.transform.position != this.unitsToCheck[i].lastPos)
			{
				if (this.CheckUnit(this.unitsToCheck[i].ctrlr, true))
				{
					this.unitsToCheck.RemoveAt(i);
				}
				else
				{
					this.unitsToCheck[i].lastPos = this.unitsToCheck[i].ctrlr.transform.position;
				}
			}
		}
	}

	public void Init(global::ZoneAoeId zoneId, global::UnitController caster, float radius, float height = 1f)
	{
		this.zoneAoeId = zoneId;
		global::PandoraSingleton<global::MissionManager>.Instance.RegisterZoneAoe(this);
		this.data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ZoneAoeData>((int)zoneId);
		this.durationLeft = this.data.Duration;
		this.enchantmentsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ZoneAoeEnchantmentData>("fk_zone_aoe_id", ((int)zoneId).ToConstantString());
		this.enchantmentsData.Sort(new global::ZoneEnchantComparer());
		this.Owner = caster;
		if (this.Owner != null)
		{
			this.Owner.GetAlliesEnemies(out this.allies, out this.enemies);
		}
		this.radius = radius;
		this.height = height;
		global::UnityEngine.BoxCollider[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.BoxCollider>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			global::UnityEngine.Bounds bounds = componentsInChildren[i].bounds;
			this.boxBounds.Add(bounds);
		}
		this.Initialized = true;
		global::System.Collections.Generic.List<global::UnitController> allAliveUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllAliveUnits();
		if (allAliveUnits != null)
		{
			for (int j = 0; j < allAliveUnits.Count; j++)
			{
				this.CheckUnit(allAliveUnits[j], false);
			}
		}
		if (!string.IsNullOrEmpty(this.fxName))
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.fxName, base.transform, true, null);
		}
		if (this.autoDestroyOnFxDone)
		{
			this.fx = base.GetComponentInChildren<global::Prometheus.OlympusFire>();
			this.fxPresent = (this.fx != null);
			global::UnityEngine.Collider componentInChildren = base.GetComponentInChildren<global::UnityEngine.Collider>();
			componentInChildren.enabled = false;
		}
		this.guid = global::PandoraSingleton<global::MissionManager>.Instance.GetNextRTGUID();
	}

	public void Activate()
	{
		if (this.once && this.usedOnce)
		{
			return;
		}
		this.usedOnce = true;
		base.gameObject.SetActive(true);
		float num = 0f;
		float num2 = 1f;
		if (this.boxBounds.Count == 0)
		{
			global::UnityEngine.Collider componentInChildren = base.GetComponentInChildren<global::UnityEngine.Collider>();
			if (componentInChildren != null)
			{
				global::UnityEngine.Bounds bounds = componentInChildren.bounds;
				num = bounds.extents.x;
				num2 = global::UnityEngine.Mathf.Max(bounds.size.y, 0.5f);
				this.offset = new global::UnityEngine.Vector3(bounds.center.x, bounds.min.y, bounds.center.z) - base.transform.position;
			}
		}
		this.Init(this.zoneAoeId, null, num, num2);
	}

	public void Deactivate()
	{
		if (this.once && this.usedOnce)
		{
			return;
		}
		this.usedOnce = true;
		global::PandoraDebug.LogInfo("Destroying Aoe Zone : " + this.zoneAoeId, "uncategorised", null);
		this.unitsToCheck.Clear();
		for (int i = this.affectedUnits.Count - 1; i >= 0; i--)
		{
			this.Trigger(this.affectedUnits[i], false, false);
		}
		this.affectedUnits.Clear();
		this.Initialized = false;
		base.gameObject.SetActive(false);
	}

	public void EnterZone(global::UnitController ctrlr)
	{
		if (!this.Initialized || !base.gameObject.activeSelf || ctrlr == null || (!global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && global::PandoraSingleton<global::MissionManager>.Instance.transitionDone))
		{
			return;
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Entering Aoe Zone : ",
			this.zoneAoeId,
			" for unit ",
			ctrlr.name
		}), "uncategorised", null);
		bool flag = false;
		for (int i = 0; i < this.unitsToCheck.Count; i++)
		{
			if (this.unitsToCheck[i].ctrlr == ctrlr)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.unitsToCheck.Add(new global::AoeUnitChecker(ctrlr, global::UnityEngine.Vector3.zero));
		}
	}

	public void ExitZone(global::UnitController ctrlr)
	{
		if (!this.Initialized || !base.gameObject.activeSelf || ctrlr == null || (!global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && global::PandoraSingleton<global::MissionManager>.Instance.transitionDone))
		{
			return;
		}
		if (this.IsUnitInside(ctrlr))
		{
			return;
		}
		this.RemoveUnit(ctrlr, true);
	}

	public void RemoveUnit(global::UnitController ctrlr, bool sendTrigger)
	{
		for (int i = 0; i < this.unitsToCheck.Count; i++)
		{
			if (this.unitsToCheck[i].ctrlr == ctrlr)
			{
				this.unitsToCheck.RemoveAt(i);
				global::PandoraDebug.LogInfo(string.Concat(new object[]
				{
					"Exiting Aoe Zone: ",
					this.zoneAoeId,
					" Removed from target to check : ",
					ctrlr.name
				}), "uncategorised", null);
				return;
			}
		}
		if (sendTrigger)
		{
			for (int j = this.affectedUnits.Count - 1; j >= 0; j--)
			{
				if (this.affectedUnits[j] == ctrlr)
				{
					this.Trigger(ctrlr, false, true);
					global::PandoraDebug.LogInfo(string.Concat(new object[]
					{
						"Exiting Aoe Zone: ",
						this.zoneAoeId,
						" Removed affected units : ",
						ctrlr.name
					}), "uncategorised", null);
					break;
				}
			}
		}
		for (int k = this.affectedUnits.Count - 1; k >= 0; k--)
		{
			if (this.affectedUnits[k] == ctrlr)
			{
				this.affectedUnits.RemoveAt(k);
				break;
			}
		}
	}

	public void AddToAffected(global::UnitController unitCtrlr)
	{
		if (this.affectedUnits.IndexOf(unitCtrlr) == -1)
		{
			this.affectedUnits.Add(unitCtrlr);
		}
	}

	public bool CheckUnit(global::UnitController ctrlr, bool network)
	{
		if (!ctrlr.IsInFriendlyZone && this.IsUnitInside(ctrlr))
		{
			this.Trigger(ctrlr, true, network);
			return true;
		}
		return false;
	}

	public void CheckEnterOrExitUnit(global::UnitController ctrlr, bool network)
	{
		if (this.IsUnitInside(ctrlr))
		{
			this.Trigger(ctrlr, true, network);
		}
		else if (this.IsUnitAffected(ctrlr))
		{
			this.Trigger(ctrlr, false, network);
		}
	}

	private bool IsUnitInside(global::UnitController ctrlr)
	{
		if (this.boxBounds.Count > 0)
		{
			for (int i = 0; i < this.boxBounds.Count; i++)
			{
				if (this.boxBounds[i].Contains(ctrlr.transform.position))
				{
					return true;
				}
			}
		}
		else if (ctrlr.isInsideCylinder(base.transform.position + this.offset, this.radius, this.height, base.transform.up))
		{
			return true;
		}
		return false;
	}

	private void Trigger(global::UnitController ctrlr, bool entry, bool network)
	{
		if (entry)
		{
			if (this.IsUnitAffected(ctrlr))
			{
				return;
			}
			this.affectedUnits.Add(ctrlr);
		}
		ctrlr.SendZoneAoeCross(this, entry, network);
	}

	private bool IsUnitAffected(global::UnitController ctrlr)
	{
		return this.affectedUnits.IndexOf(ctrlr) != -1;
	}

	public global::EffectTypeId GetEnterEffectType()
	{
		for (int i = 0; i < this.enchantmentsData.Count; i++)
		{
			if (this.enchantmentsData[i].ZoneTriggerId == global::ZoneTriggerId.ENTER)
			{
				global::EnchantmentData enchantmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)this.enchantmentsData[i].EnchantmentId);
				return enchantmentData.EffectTypeId;
			}
		}
		return global::EffectTypeId.NONE;
	}

	public void ApplyEnchantments(global::UnitController ctrlr, bool entry)
	{
		bool flag = this.Owner == null || this.Owner == ctrlr;
		bool flag2 = this.Owner == null || this.allies == null || this.allies.IndexOf(ctrlr) != -1;
		bool flag3 = this.Owner == null || this.enemies == null || this.enemies.IndexOf(ctrlr) != -1;
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Unit is affected by AoeZone :  ",
			this.zoneAoeId,
			" for unit ",
			ctrlr.name,
			" self=",
			flag,
			" ally=",
			flag2,
			" enemy=",
			flag3
		}), "uncategorised", null);
		for (int i = 0; i < this.enchantmentsData.Count; i++)
		{
			if (!entry && this.enchantmentsData[i].ZoneTriggerId == global::ZoneTriggerId.INSIDE && ((this.enchantmentsData[i].TargetSelf && flag) || (this.enchantmentsData[i].TargetAlly && flag2) || (this.enchantmentsData[i].TargetEnemy && flag3)))
			{
				ctrlr.unit.RemoveEnchantment(this.enchantmentsData[i].EnchantmentId, (!(this.Owner != null)) ? ctrlr.unit : this.Owner.unit);
			}
			if (base.gameObject.activeSelf && (((this.enchantmentsData[i].ZoneTriggerId == global::ZoneTriggerId.ENTER || this.enchantmentsData[i].ZoneTriggerId == global::ZoneTriggerId.INSIDE) && entry) || (this.enchantmentsData[i].ZoneTriggerId == global::ZoneTriggerId.EXIT && !entry)) && ((this.enchantmentsData[i].TargetSelf && flag) || (this.enchantmentsData[i].TargetAlly && flag2) || (this.enchantmentsData[i].TargetEnemy && flag3)))
			{
				ctrlr.unit.AddEnchantment(this.enchantmentsData[i].EnchantmentId, (!(this.Owner != null)) ? ctrlr.unit : this.Owner.unit, false, false, global::AllegianceId.NONE);
			}
		}
	}

	public void UpdateDuration()
	{
		if (this.durationLeft > 0)
		{
			this.durationLeft--;
			if (this.durationLeft == 0)
			{
				this.Deactivate();
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.ClearAoe(this.guid);
				if (this.destructible != null && this.destructible.destroyOnAoeDestroy)
				{
					this.destructible.Deactivate();
				}
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateAoe(this.guid, this.Owner.uid, this.zoneAoeId, this.radius, this.durationLeft, base.transform.position);
			}
		}
	}

	public void GetDamages(out int damageMin, out int damageMax)
	{
		damageMin = 0;
		damageMax = 0;
		for (int i = 0; i < this.enchantmentsData.Count; i++)
		{
			if (this.enchantmentsData[i].ZoneTriggerId == global::ZoneTriggerId.ENTER)
			{
				global::EnchantmentData enchantmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)this.enchantmentsData[i].EnchantmentId);
				damageMin += enchantmentData.DamageMin;
				damageMax += enchantmentData.DamageMax;
			}
		}
	}

	private const float BASE_HEIGHT = 1f;

	public bool autoInit;

	public bool indestructible;

	public bool once;

	public bool autoDestroyOnFxDone;

	public global::ZoneAoeId zoneAoeId;

	public string fxName;

	private global::ZoneAoeData data;

	private global::System.Collections.Generic.List<global::ZoneAoeEnchantmentData> enchantmentsData;

	private global::System.Collections.Generic.List<global::AoeUnitChecker> unitsToCheck = new global::System.Collections.Generic.List<global::AoeUnitChecker>();

	private float radius;

	private float height;

	private global::UnityEngine.Vector3 offset = global::UnityEngine.Vector3.zero;

	private readonly global::System.Collections.Generic.List<global::UnitController> affectedUnits = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::UnitController> allies;

	private global::System.Collections.Generic.List<global::UnitController> enemies;

	public int durationLeft;

	private bool usedOnce;

	private readonly global::System.Collections.Generic.List<global::UnityEngine.Bounds> boxBounds = new global::System.Collections.Generic.List<global::UnityEngine.Bounds>();

	private global::Prometheus.OlympusFire fx;

	private bool fxPresent;

	[global::UnityEngine.HideInInspector]
	public uint guid;

	[global::UnityEngine.HideInInspector]
	public global::Destructible destructible;
}
