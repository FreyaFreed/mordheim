using System;
using System.Collections.Generic;
using HighlightingSystem;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class InteractivePoint : global::UnityEngine.MonoBehaviour
{
	public global::HighlightingSystem.Highlighter Highlight { get; private set; }

	public global::MapImprint Imprint { get; private set; }

	public virtual void Init(uint id)
	{
		this.SetActionIds();
		if (this.linkedPoint != null)
		{
			this.linkedPoints.Add(this.linkedPoint);
		}
		if (this.imprintIcon != null && this.linkedPoints.Count == 0)
		{
			this.Imprint = base.gameObject.AddComponent<global::MapImprint>();
			this.Imprint.Init(this.imprintIcon, this.imprintIcon, true, global::MapImprintType.INTERACTIVE_POINT, new global::UnityEngine.Events.UnityAction<bool, bool, global::UnityEngine.Events.UnityAction>(this.Hide), null, this as global::SearchPoint, null, null);
		}
		this.Highlight = base.gameObject.GetComponent<global::HighlightingSystem.Highlighter>();
		if (this.Highlight == null)
		{
			this.Highlight = base.gameObject.AddComponent<global::HighlightingSystem.Highlighter>();
		}
		this.Highlight.seeThrough = false;
		if (this.linkedPoints.Count > 0)
		{
			for (int i = 0; i < this.linkedPoints.Count; i++)
			{
				this.linkedPoints[i].links.Add(this);
				this.sameTriggersAsLinked = true;
				for (int j = 0; j < this.triggers.Count; j++)
				{
					if (j < this.linkedPoints[i].triggers.Count && this.linkedPoints[i].triggers[j] != this.triggers[j])
					{
						this.sameTriggersAsLinked = false;
					}
				}
			}
		}
		if (this.visual != null)
		{
			this.visualDissolver = this.visual.AddComponent<global::Dissolver>();
			this.visualDissolver.dissolveSpeed = this.apparitionDelay;
		}
		this.SetTriggerVisual();
		this.guid = id;
		global::PandoraSingleton<global::MissionManager>.Instance.RegisterInteractivePoint(this);
	}

	protected virtual void SetActionIds()
	{
		this.unitActionIds = new global::System.Collections.Generic.List<global::UnitActionId>();
		this.unitActionIds.Add(this.unitActionId);
	}

	protected virtual global::System.Collections.Generic.List<global::UnitActionId> GetActions(global::UnitController unitCtrlr)
	{
		return this.unitActionIds;
	}

	protected virtual bool CanInteract(global::UnitController unitCtrlr)
	{
		return this.LinksValid(unitCtrlr, this.reverseLinkedCondition) && this.HasRequiredItem(unitCtrlr) && this.CompliesWithRestrictions(unitCtrlr);
	}

	public bool HasRequiredItem(global::UnitController unitCtrlr)
	{
		return this.requestedItemId == global::ItemId.NONE || unitCtrlr.unit.HasItem(this.requestedItemId, global::ItemQualityId.NONE);
	}

	private bool CompliesWithRestrictions(global::UnitController unitCtrlr)
	{
		for (int i = 0; i < this.restrictions.Count; i++)
		{
			global::InteractiveRestriction interactiveRestriction = this.restrictions[i];
			if ((interactiveRestriction.teamIdx != -1 && unitCtrlr.GetWarband().teamIdx != interactiveRestriction.teamIdx) || (interactiveRestriction.warbandId != global::WarbandId.NONE && unitCtrlr.GetWarband().WarData.Id != interactiveRestriction.warbandId) || (interactiveRestriction.allegianceId != global::AllegianceId.NONE && unitCtrlr.GetWarband().WarData.AllegianceId != interactiveRestriction.allegianceId))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual bool LinkValid(global::UnitController unitCtrlr, bool reverseCondition)
	{
		return true;
	}

	public global::System.Collections.Generic.List<global::UnitActionId> GetUnitActionIds(global::UnitController unitCtrlr)
	{
		if (this.CanInteract(unitCtrlr))
		{
			return this.GetActions(unitCtrlr);
		}
		return this.emptyList;
	}

	public void DestroyVisual(bool triggersOnly = false, bool force = false)
	{
		if (this.destroyed)
		{
			return;
		}
		this.destroyed = true;
		for (int i = 0; i < this.links.Count; i++)
		{
			this.links[i].SetTriggerVisual();
		}
		global::PandoraSingleton<global::MissionManager>.Instance.UnregisterInteractivePoint(this);
		this.DestroyTriggers();
		this.needVisualRemoved = !triggersOnly;
		if (this.Imprint == null || force)
		{
			this.DoDestroyVisual();
		}
		else
		{
			this.Imprint.alwaysVisible = false;
			this.Imprint.needsRefresh = true;
		}
	}

	private void RemoveVisual()
	{
		global::UnityEngine.Object.Destroy(this.visual);
		this.visual = null;
	}

	public virtual void SetTriggerVisual()
	{
		this.SetTriggerVisual(true);
	}

	private bool LinksValid(global::UnitController ctrlr, bool reverse)
	{
		bool flag = true;
		for (int i = 0; i < this.linkedPoints.Count; i++)
		{
			flag &= this.linkedPoints[i].LinkValid(ctrlr, reverse);
		}
		return flag;
	}

	protected void SetTriggerVisual(bool baseVisual)
	{
		if (this.sameTriggersAsLinked)
		{
			return;
		}
		bool flag = this.LinksValid(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit(), this.reverseLinkedCondition);
		bool flag2 = this.altTriggers.Count > 0 && this.altTriggers.Count == this.triggers.Count;
		for (int i = 0; i < this.triggers.Count; i++)
		{
			this.triggers[i].SetActive((!flag2 || baseVisual) && flag);
			if (flag2)
			{
				this.altTriggers[i].SetActive(!baseVisual && flag);
			}
		}
		for (int j = 0; j < this.links.Count; j++)
		{
			this.links[j].SetTriggerVisual();
		}
	}

	private void DestroyTriggers()
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			if (this.triggers[i] != null)
			{
				this.triggers[i].SetActive(false);
				global::UnityEngine.Object.Destroy(this.triggers[i]);
				this.triggers[i] = null;
			}
		}
		this.triggers.Clear();
	}

	public void Hide(bool hide, bool force = false, global::UnityEngine.Events.UnityAction onDissolved = null)
	{
		if (this.destroyed)
		{
			if (!hide)
			{
				this.DoDestroyVisual();
			}
		}
		else
		{
			global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = !hide;
			}
		}
	}

	private void DoDestroyVisual()
	{
		global::UnityEngine.Object.Destroy(this.Imprint);
		if (this.needVisualRemoved)
		{
			if (this.visualDissolver != null && this.visualDissolver.gameObject.activeInHierarchy)
			{
				this.visualDissolver.Hide(true, false, new global::UnityEngine.Events.UnityAction(this.RemoveVisual));
			}
			else
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public virtual string GetLocAction()
	{
		return this.loc_action;
	}

	private string GetDefaultIconSpritePath()
	{
		return "action/" + this.unitActionId.ToLowerString();
	}

	public global::UnityEngine.Sprite GetIconAction()
	{
		if (this.imprintIcon != null)
		{
			return this.imprintIcon;
		}
		string defaultIconSpritePath = this.GetDefaultIconSpritePath();
		if (!string.IsNullOrEmpty(defaultIconSpritePath))
		{
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>(defaultIconSpritePath, true);
		}
		return null;
	}

	protected void SpawnFxs(bool activated)
	{
		global::System.Collections.Generic.List<global::Prometheus.OlympusFireStarter> list = this.deactivationFx;
		if (activated || this.useSameFxForDeactivation)
		{
			list = this.activationFx;
		}
		for (int i = 0; i < list.Count; i++)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(list[i], null);
		}
	}

	public virtual void ActivateZoneAoe()
	{
		if (this.zoneAoe != null)
		{
			if (this.zoneAoe.Initialized)
			{
				this.zoneAoe.Deactivate();
			}
			else
			{
				this.zoneAoe.Activate();
			}
		}
	}

	public virtual global::UnitController SpawnCampaignUnit()
	{
		if (this.campaignUnitId != global::CampaignUnitId.NONE)
		{
			return global::PandoraSingleton<global::MissionManager>.Instance.ActivateHiddenUnit(this.campaignUnitId, this.spawnVisible, "mission_unit_spawn");
		}
		return null;
	}

	private global::System.Collections.Generic.List<global::UnitActionId> emptyList = new global::System.Collections.Generic.List<global::UnitActionId>();

	public global::UnitActionId unitActionId;

	public string loc_action;

	public string loc_action_enemy;

	public global::SearchAnim anim;

	public global::UnityEngine.GameObject visual;

	protected global::Dissolver visualDissolver;

	public global::ItemId requestedItemId;

	public global::InteractivePoint linkedPoint;

	public global::System.Collections.Generic.List<global::InteractivePoint> linkedPoints;

	public bool reverseLinkedCondition;

	public global::System.Collections.Generic.List<global::InteractiveRestriction> restrictions;

	protected global::System.Collections.Generic.List<global::InteractivePoint> links = new global::System.Collections.Generic.List<global::InteractivePoint>();

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> triggers = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> altTriggers = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	public global::UnityEngine.Sprite imprintIcon;

	public global::UnityEngine.Transform cameraAnchor;

	public global::System.Collections.Generic.List<global::Prometheus.OlympusFireStarter> activationFx;

	public global::System.Collections.Generic.List<global::Prometheus.OlympusFireStarter> deactivationFx;

	public bool useSameFxForDeactivation;

	public float apparitionDelay = 0.5f;

	public global::ZoneAoe zoneAoe;

	public global::SkillId curseId;

	public global::CampaignUnitId campaignUnitId;

	public bool spawnVisible;

	protected global::System.Collections.Generic.List<global::UnitActionId> unitActionIds = new global::System.Collections.Generic.List<global::UnitActionId>();

	private bool destroyed;

	private bool sameTriggersAsLinked;

	private bool needVisualRemoved;

	public uint guid;
}
