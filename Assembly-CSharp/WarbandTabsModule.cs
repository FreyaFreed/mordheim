using System;
using System.Collections;

public class WarbandTabsModule : global::TabsModule
{
	public override void Init()
	{
		base.Init();
		int num = 0;
		this.AddTabIcon(global::HideoutManager.State.CAMP, num++, global::HideoutCamp.NodeSlot.CAMP, null, "hideout_camp", null);
		this.AddTabIcon(global::HideoutManager.State.SHIPMENT, num++, global::HideoutCamp.NodeSlot.SHIPMENT, null, "hideout_smuggler", new global::TabsModule.IsAvailable(this.IsSmugglersAvailable));
		this.AddTabIcon(global::HideoutManager.State.SHOP, num++, global::HideoutCamp.NodeSlot.SHOP, null, "hideout_shop", new global::TabsModule.IsAvailable(this.IsShopAvailable));
		this.AddTabIcon(global::HideoutManager.State.WARBAND, num++, global::HideoutCamp.NodeSlot.LEADER, null, "hideout_management", null);
		this.AddTabIcon(global::HideoutManager.State.SKIRMISH, num++, global::HideoutCamp.NodeSlot.BANNER, "icn_mission_type_bounty", "hideout_skirmish", new global::TabsModule.IsAvailable(this.IsSkirmishAvailable));
		this.AddTabIcon(global::HideoutManager.State.PLAYER_PROGRESSION, num++, global::HideoutCamp.NodeSlot.DRAMATIS, "unit/" + global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetDramatis().unit.Id.ToLowerString(), "hideout_progression", new global::TabsModule.IsAvailable(this.IsPlayerProgressionAvailable));
		this.AddTabIcon(global::HideoutManager.State.MISSION, num++, global::HideoutCamp.NodeSlot.WAGON, "icn_mission_type_mission", "hideout_campaign", new global::TabsModule.IsAvailable(this.IsMissionAvailable));
		this.AddTabIcon(global::HideoutManager.State.CAMP, num++, global::HideoutCamp.NodeSlot.NEXT_DAY, "action/end_turn", "hideout_day_skip", new global::TabsModule.IsAvailable(this.IsNextDayAvailable));
	}

	protected override void SetCurrentTabs(int index)
	{
		base.SetCurrentTabs(index);
		if (base.IsTabAvailable(index) && this.icons[index].nodeSlot == global::HideoutCamp.NodeSlot.NEXT_DAY)
		{
			this.TriggerNextDay();
		}
	}

	private void TriggerNextDay()
	{
		base.StartCoroutine(this.DelayNextDay());
	}

	private global::System.Collections.IEnumerator DelayNextDay()
	{
		yield return null;
		global::PandoraSingleton<global::HideoutManager>.Instance.OnNextDay();
		this.Refresh();
		yield break;
	}

	public void AddTabIcon(global::HideoutManager.State state, int index, global::HideoutCamp.NodeSlot nodeSlot, string icon = null, string loc = null, global::TabsModule.IsAvailable isAvailable = null)
	{
		global::TabIcon tabIcon = this.AddTabIcon(state, index, icon, loc, isAvailable);
		tabIcon.nodeSlot = nodeSlot;
	}

	public global::TabIcon GetTabIcon(global::HideoutCamp.NodeSlot nodeSlot)
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			if (this.icons[i].nodeSlot == nodeSlot)
			{
				return this.icons[i];
			}
		}
		return null;
	}

	public bool IsShopAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START) && global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Units.Count == 1 && !global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
		{
			reason = "na_hideout_new_game";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			reason = "na_hideout_late_shipment_count";
			return false;
		}
		return true;
	}

	public bool IsSmugglersAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			reason = "na_hideout_late_shipment_count";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START))
		{
			reason = "na_hideout_next_day";
			return false;
		}
		return true;
	}

	public bool IsMissionAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			reason = "na_hideout_late_shipment_count";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
		{
			reason = "na_hideout_post_mission";
			return false;
		}
		if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.HasLeader(true))
		{
			reason = "na_hideout_active_leader";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetActiveUnitsCount() < global::Constant.GetInt(global::ConstantId.MIN_MISSION_UNITS))
		{
			reason = "na_hideout_min_active_unit";
			return false;
		}
		return true;
	}

	public bool IsSkirmishAvailable(out string reason)
	{
		reason = string.Empty;
		return global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.IsSkirmishAvailable(out reason);
	}

	public bool IsNextDayAvailable(out string reason)
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START) && !global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
		{
			reason = "na_hideout_next_day";
			return false;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			reason = "na_hideout_late_shipment_count";
			return false;
		}
		reason = string.Empty;
		return true;
	}

	public bool IsPlayerProgressionAvailable(out string reason)
	{
		reason = string.Empty;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START) && global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Units.Count == 1 && !global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission() && global::PandoraSingleton<global::GameManager>.Instance.Profile.CurrentXp == 0)
		{
			reason = "na_hideout_new_game";
			return false;
		}
		return true;
	}
}
