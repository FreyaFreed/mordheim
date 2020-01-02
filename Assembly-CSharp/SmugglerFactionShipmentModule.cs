using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmugglerFactionShipmentModule : global::UIModule
{
	public void Setup(global::FactionMenuController faction, global::System.Action onShipmentSentCb)
	{
		this.onShipmentSent = onShipmentSentCb;
		this.description.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(faction.Faction.Data.Desc + "_desc");
		this.factionType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(faction.Faction.Data.Desc + "_name");
		this.reputation.text = faction.Faction.Save.rank.ToString();
		if (faction.NextRankReputation == -1)
		{
			this.progression.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_max_rank");
			this.progressBar.normalizedValue = 1f;
		}
		else
		{
			this.progression.text = string.Format("{0}/{1}", faction.Faction.Reputation, faction.NextRankReputation);
			this.progressBar.normalizedValue = (float)faction.Faction.Reputation / (float)faction.NextRankReputation;
		}
		if (faction.Faction.Primary && faction.HasShipment)
		{
			this.shipmentType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_fill_request");
			this.delivery.gameObject.SetActive(false);
			this.shipment.gameObject.SetActive(true);
			this.shipment.SetFaction(faction, new global::System.Action<global::FactionShipment>(this.OnSendShipment));
		}
		else
		{
			this.shipmentType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_send");
			this.delivery.gameObject.SetActive(true);
			this.shipment.gameObject.SetActive(false);
			this.delivery.SetFaction(faction, new global::System.Action<global::FactionShipment>(this.OnSendShipment));
		}
	}

	public void SetFocus()
	{
		if (this.shipment.gameObject.activeInHierarchy)
		{
			this.shipment.fragGroup.SetSelected(false);
		}
		else
		{
			this.delivery.fragGroup.SetSelected(false);
		}
	}

	public void OnLostFocus()
	{
		this.shipment.toggleGroup.SetAllTogglesOff();
		this.delivery.toggleGroup.SetAllTogglesOff();
	}

	private void OnSendShipment(global::FactionShipment factionShipment)
	{
		if (factionShipment.TotalWeight <= 0)
		{
			return;
		}
		this.currentFactionShipment = factionShipment;
		int num;
		if (this.currentFactionShipment.FactionCtrlr.Faction.Primary && this.currentFactionShipment.FactionCtrlr.HasShipment)
		{
			num = this.currentFactionShipment.TotalWeight - this.currentFactionShipment.FactionCtrlr.ShipmentWeight;
		}
		else
		{
			num = this.currentFactionShipment.TotalWeight;
		}
		int num2 = num - this.currentFactionShipment.FactionCtrlr.MaxReputationGain;
		if (num2 > 0)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.MEDIUM, new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR
			});
			global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR
			});
			global::ConfirmationPopupView confirmationPopupView = (global::ConfirmationPopupView)modulesPopup[0];
			if (this.currentFactionShipment.FactionCtrlr.MaxReputationGain > 0)
			{
				confirmationPopupView.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_reputation_warning_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_reputation_warning_desc", new string[]
				{
					this.currentFactionShipment.FactionCtrlr.MaxReputationGain.ToString()
				}), delegate(bool confirm)
				{
					if (confirm)
					{
						this.DoSendShipment();
					}
				}, false, false);
			}
			else if (!this.currentFactionShipment.FactionCtrlr.WarSave.smugglersMaxRankShown)
			{
				this.currentFactionShipment.FactionCtrlr.WarSave.smugglersMaxRankShown = true;
				confirmationPopupView.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_reputation_max_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_reputation_max_desc"), delegate(bool confirm)
				{
					if (confirm)
					{
						this.DoSendShipment();
					}
				}, false, false);
				confirmationPopupView.HideCancelButton();
			}
			else
			{
				this.DoSendShipment();
			}
		}
		else
		{
			this.DoSendShipment();
		}
	}

	private void DoSendShipment()
	{
		if (this.currentFactionShipment.FactionCtrlr.Faction.Primary && this.currentFactionShipment.FactionCtrlr.HasShipment)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.MEDIUM, new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR,
				global::PopupModuleId.POPUP_SEND_SHIPMENT_REQUEST
			});
			global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR,
				global::PopupModuleId.POPUP_SEND_SHIPMENT_REQUEST
			});
			global::SendShipmentRequestPopup sendShipmentRequestPopup = (global::SendShipmentRequestPopup)modulesPopup[1];
			sendShipmentRequestPopup.Set((global::FactionRequest)this.currentFactionShipment);
			global::ConfirmationPopupView confirmationPopupView = (global::ConfirmationPopupView)modulesPopup[0];
			confirmationPopupView.Show("popup_request_title", "popup_request_desc", new global::System.Action<bool>(this.OnRequestConfirm), false, false);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.MEDIUM, new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR,
				global::PopupModuleId.POPUP_SEND_SHIPMENT
			});
			global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup2 = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR,
				global::PopupModuleId.POPUP_SEND_SHIPMENT
			});
			global::SendShipmentPopup sendShipmentPopup = (global::SendShipmentPopup)modulesPopup2[1];
			sendShipmentPopup.Set((global::FactionDelivery)this.currentFactionShipment);
			global::ConfirmationPopupView confirmationPopupView2 = (global::ConfirmationPopupView)modulesPopup2[0];
			confirmationPopupView2.Show("popup_shipment_title", "popup_shipment_desc", new global::System.Action<bool>(this.OnShipmentConfirm), false, false);
		}
	}

	private void OnShipmentConfirm(bool confirm)
	{
		if (confirm)
		{
			this.RemoveWyrdstone();
			int deliveryDate = this.GetDeliveryDate(this.currentFactionShipment.FactionCtrlr.Faction.GetFactionDeliveryEvent());
			this.SendShipment(this.currentFactionShipment.FactionCtrlr.Faction.GetFactionDeliveryEvent(), deliveryDate, this.currentFactionShipment.TotalWeight, this.currentFactionShipment.FactionCtrlr.GetDeliveryPrice(this.currentFactionShipment.TotalWeight), this.currentFactionShipment.FactionCtrlr.GetDeliveryReputation(this.currentFactionShipment.TotalWeight));
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.onShipmentSent();
		}
	}

	private void OnRequestConfirm(bool confirm)
	{
		global::Warband warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		global::WarbandSave warbandSave = warband.GetWarbandSave();
		global::EventLogger logger = warband.Logger;
		if (confirm)
		{
			this.RemoveWyrdstone();
			int currentDate = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
			int deliveryDate = this.GetDeliveryDate(this.currentFactionShipment.FactionCtrlr.Faction.GetFactionDeliveryEvent());
			int num = this.currentFactionShipment.TotalWeight - this.currentFactionShipment.FactionCtrlr.ShipmentWeight;
			this.SendShipment(global::EventLogger.LogEvent.SHIPMENT_DELIVERY, deliveryDate, this.currentFactionShipment.FactionCtrlr.ShipmentWeight, this.currentFactionShipment.FactionCtrlr.ShipmentGoldReward, 0);
			warband.AddToAttribute(global::WarbandAttributeId.DELIVERY_WITHOUT_DELAY, 1);
			this.SendShipment(this.currentFactionShipment.FactionCtrlr.Faction.GetFactionDeliveryEvent(), deliveryDate, num, this.currentFactionShipment.FactionCtrlr.GetDeliveryPrice(num), this.currentFactionShipment.FactionCtrlr.GetDeliveryReputation(this.currentFactionShipment.TotalWeight));
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = logger.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_LATE);
			warbandSave.nextShipmentExtraDays = tuple.Item1 - currentDate;
			warbandSave.lastShipmentFailed = false;
			logger.RemoveHistory(tuple);
			warband.UpdateAttributes();
			this.currentFactionShipment.FactionCtrlr.CreateNewShipmentRequest(tuple.Item1, deliveryDate);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.onShipmentSent();
		}
	}

	private int GetDeliveryDate(global::EventLogger.LogEvent log)
	{
		int currentDate = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.FindLastEvent(log);
		int num = currentDate + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(this.currentFactionShipment.FactionCtrlr.Faction.Data.MinDeliveryDays, this.currentFactionShipment.FactionCtrlr.Faction.Data.MaxDeliveryDays + 1);
		if (tuple != null && tuple.Item1 > currentDate)
		{
			num = global::UnityEngine.Mathf.Max(num, tuple.Item1 + 1);
		}
		return num;
	}

	private void SendShipment(global::EventLogger.LogEvent logEvent, int date, int weight, int gold, int reputation)
	{
		int rank = this.currentFactionShipment.FactionCtrlr.AddReputation(reputation);
		int num = gold;
		bool flag = true;
		if (logEvent != global::EventLogger.LogEvent.SHIPMENT_DELIVERY)
		{
			flag = this.currentFactionShipment.FactionCtrlr.Faction.SaveDelivery(weight, gold, rank, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, out num);
		}
		if (flag)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.AddHistory(date, logEvent, num);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.MEDIUM, new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR
			});
			global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR
			});
			global::ConfirmationPopupView confirmationPopupView = (global::ConfirmationPopupView)modulesPopup[0];
			confirmationPopupView.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_multiple_shipments_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_multiple_shipments_desc", new string[]
			{
				this.currentFactionShipment.FactionCtrlr.MaxReputationGain.ToString()
			}), null, false, false);
			confirmationPopupView.HideCancelButton();
		}
		if (date == global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.DoDelivery(this.currentFactionShipment.FactionCtrlr, num, logEvent == global::EventLogger.LogEvent.SHIPMENT_DELIVERY);
		}
	}

	private void RemoveWyrdstone()
	{
		global::WarbandChest warbandChest = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest;
		warbandChest.RemoveItem(global::ItemId.WYRDSTONE_FRAGMENT, this.currentFactionShipment.FragmentCount);
		warbandChest.RemoveItem(global::ItemId.WYRDSTONE_SHARD, this.currentFactionShipment.ShardCount);
		warbandChest.RemoveItem(global::ItemId.WYRDSTONE_CLUSTER, this.currentFactionShipment.ClusterCount);
		global::TreasuryModule moduleRight = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
		moduleRight.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
	}

	public global::UnityEngine.UI.Text factionType;

	public global::UnityEngine.UI.Text shipmentType;

	public global::UnityEngine.UI.Text reputation;

	public global::UnityEngine.UI.Text progression;

	public global::UnityEngine.UI.Slider progressBar;

	public global::UnityEngine.UI.Text description;

	public global::FactionShipment shipment;

	public global::FactionDelivery delivery;

	private global::FactionShipment currentFactionShipment;

	private global::System.Action onShipmentSent;
}
