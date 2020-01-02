using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CampaignFlagView : global::UnityEngine.MonoBehaviour
{
	public void Load(int idx, global::UnityEngine.Events.UnityAction<int, int> select, global::UnityEngine.Events.UnityAction<int, global::WarbandSave> confirm, int uiIdx)
	{
		this.loaded = false;
		if (global::PandoraSingleton<global::GameManager>.Instance.Save.CampaignExist(idx))
		{
			this.saveIndex = idx;
			this.loadingTitle.gameObject.SetActive(true);
			this.info.SetActive(false);
			this.campaignOver.SetActive(false);
			global::PandoraSingleton<global::GameManager>.Instance.Save.GetCampaignInfo(idx, new global::System.Action<global::WarbandSave>(this.OnSaveLoaded));
			this.selectCb = select;
			this.confirmCb = confirm;
			global::ToggleEffects toggleEffects = base.GetComponentsInChildren<global::ToggleEffects>()[0];
			toggleEffects.onSelect.RemoveAllListeners();
			toggleEffects.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnSelect));
			toggleEffects.onAction.RemoveAllListeners();
			toggleEffects.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnConfirm));
			if (uiIdx == 0)
			{
				toggleEffects.SetOn();
			}
		}
	}

	private void OnSaveLoaded(global::WarbandSave warSave)
	{
		this.loadingTitle.gameObject.SetActive(false);
		this.info.SetActive(true);
		this.cachedWarbandId = warSave.id;
		global::System.DateTime cachedSaveTimeStamp = global::PandoraSingleton<global::GameManager>.Instance.Save.GetCachedSaveTimeStamp(this.saveIndex);
		string text = cachedSaveTimeStamp.ToShortDateString() + " - " + cachedSaveTimeStamp.ToShortTimeString();
		this.textTitle.text = warSave.Name;
		this.textRank.text = warSave.rank.ToString();
		this.lastSaveTime.text = text;
		this.icon.sprite = global::Warband.GetIcon((global::WarbandId)warSave.id);
		this.warband = warSave;
		this.loaded = true;
		this.isValid = true;
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
		{
			global::Warband warband = new global::Warband(warSave);
			if (warband.ValidateWarbandForInvite(false))
			{
				this.campaignOver.SetActive(false);
			}
			else
			{
				this.isValid = false;
				this.campaignOver.SetActive(true);
				this.campaignOverTxt.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("invite_select_warband_invalid");
			}
		}
		else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether())
		{
			global::Warband warband2 = new global::Warband(warSave);
			string text2;
			if (!warSave.inMission && (warband2.IsSkirmishAvailable(out text2) || warband2.IsContestAvailable(out text2)))
			{
				this.campaignOver.SetActive(false);
			}
			else
			{
				this.isValid = false;
				this.campaignOver.SetActive(true);
				this.campaignOverTxt.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("invite_select_warband_invalid");
			}
		}
		else if (warSave.lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			this.campaignOver.SetActive(true);
			this.campaignOverTxt.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("game_over");
		}
		else
		{
			this.campaignOver.SetActive(false);
		}
	}

	private void OnSelect()
	{
		if (this.selectCb != null)
		{
			this.selectCb(this.saveIndex, this.cachedWarbandId);
		}
	}

	private void OnConfirm()
	{
		if (this.confirmCb != null && this.isValid)
		{
			this.confirmCb(this.saveIndex, this.warband);
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text loadingTitle;

	public global::UnityEngine.GameObject info;

	public global::UnityEngine.UI.Text textTitle;

	public global::UnityEngine.UI.Text textRank;

	public global::UnityEngine.UI.Text lastSaveTime;

	public global::UnityEngine.GameObject campaignOver;

	public global::UnityEngine.UI.Text campaignOverTxt;

	[global::UnityEngine.HideInInspector]
	public bool loaded;

	[global::UnityEngine.HideInInspector]
	public bool isValid = true;

	private global::WarbandSave warband;

	private int saveIndex;

	private global::UnityEngine.Events.UnityAction<int, int> selectCb;

	private global::UnityEngine.Events.UnityAction<int, global::WarbandSave> confirmCb;

	private int cachedWarbandId;
}
