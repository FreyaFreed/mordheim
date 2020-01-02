using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionMoraleController : global::CanvasGroupDisabler
{
	private void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.WARBAND_MORALE_CHANGED, new global::DelReceiveNotice(this.OnMoraleChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_ROUND_START, new global::DelReceiveNotice(this.OnStart));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_DEPLOY, new global::DelReceiveNotice(this.OnStart));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_CHANGED, new global::DelReceiveNotice(this.OnUnitChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TIMER_STARTING, new global::DelReceiveNotice(this.OnTimerStarting));
	}

	private void OnUnitChanged()
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() == null)
		{
			this.EnemyTurnText.text = string.Empty;
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().IsPlayed())
		{
			this.EnemyTurnText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_player_turn");
			this.EnemyTurnText.color = this.playerColor;
		}
		else
		{
			this.EnemyTurnText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_enemy_turn");
			this.EnemyTurnText.color = this.enemyColor;
		}
	}

	private void OnMoraleChanged()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i];
			if (this.playerWarbandController == warbandController)
			{
				this.playerMorale.normalizedValue = this.playerWarbandController.MoralRatio;
				this.playerMoraleText.text = this.playerWarbandController.MoralValue.ToConstantString();
				if (this.prevMorale >= global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold && this.playerWarbandController.MoralRatio < global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold)
				{
					global::PandoraSingleton<global::Pan>.Instance.Narrate("morale_low2");
				}
				else if (this.prevMorale >= 50f && this.playerWarbandController.MoralRatio < 50f)
				{
					global::PandoraSingleton<global::Pan>.Instance.Narrate("morale_low1");
				}
				this.prevMorale = this.playerWarbandController.MoralRatio;
			}
			else if ((!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto) && this.enemyWarbandController == warbandController)
			{
				this.enemyMorale.normalizedValue = this.enemyWarbandController.MoralRatio;
				this.enemyMoraleText.text = this.enemyWarbandController.MoralValue.ToConstantString();
			}
		}
	}

	private void OnStart()
	{
		float routThreshold = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold;
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.MISSION_ROUND_START, new global::DelReceiveNotice(this.OnStart));
		this.playerWarbandController = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr();
		this.playerMorale.maxValue = (float)this.playerWarbandController.MaxMoralValue;
		this.playerMorale.normalizedValue = this.playerWarbandController.MoralRatio;
		this.playerMoraleText.text = this.playerWarbandController.MoralValue.ToConstantString();
		this.playerMoraleThresholdText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_moral_threshold", new string[]
		{
			global::UnityEngine.Mathf.FloorToInt(routThreshold * (float)this.playerWarbandController.MaxMoralValue).ToConstantString()
		});
		this.playerRout.anchoredPosition = new global::UnityEngine.Vector2(0f, routThreshold * this.sliderHeight);
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign && !global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			this.enemyMoraleBar.SetActive(false);
		}
		else
		{
			this.enemyWarbandController = global::PandoraSingleton<global::MissionManager>.Instance.GetMainEnemyWarbandCtrlr();
			this.enemyMorale.maxValue = (float)this.enemyWarbandController.MaxMoralValue;
			this.enemyMorale.normalizedValue = this.enemyWarbandController.MoralRatio;
			this.enemyMoraleText.text = this.enemyWarbandController.MoralValue.ToConstantString();
			this.enemyMoraleThresholdText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_moral_threshold", new string[]
			{
				global::UnityEngine.Mathf.FloorToInt(routThreshold * (float)this.enemyWarbandController.MaxMoralValue).ToConstantString()
			});
			this.enemyRout.anchoredPosition = new global::UnityEngine.Vector2(0f, routThreshold * this.sliderHeight);
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.mapData != null)
		{
			this.mapNameText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(global::PandoraSingleton<global::MissionManager>.Instance.mapData.Name + "_name");
			if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign)
			{
				global::DeploymentScenarioMapLayoutData deploymentScenarioMapLayoutData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DeploymentScenarioMapLayoutData>(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.deployScenarioMapLayoutId);
				global::UnityEngine.UI.Text text = this.mapNameText;
				text.text = text.text + "\n" + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_title_" + deploymentScenarioMapLayoutData.DeploymentScenarioId.ToLowerString());
			}
		}
		else
		{
			this.mapNameText.text = string.Empty;
		}
	}

	private void Update()
	{
		if (this.turnTimer != null)
		{
			this.timer.enabled = !this.turnTimer.Paused;
			if (!this.turnTimer.Paused && this.oldTimer != this.turnTimer.Timer)
			{
				this.oldTimer = this.turnTimer.Timer;
				this.timer.text = this.turnTimer.Timer.ToString("###");
			}
		}
		float y = global::PandoraSingleton<global::MissionManager>.Instance.CamManager.transform.rotation.eulerAngles.y;
		this.compass.transform.rotation = global::UnityEngine.Quaternion.Euler(new global::UnityEngine.Vector3(0f, 0f, y));
	}

	private void OnTimerStarting()
	{
		this.turnTimer = (global::TurnTimer)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
	}

	public global::UnityEngine.Color enemyColor;

	public global::UnityEngine.Color playerColor;

	public global::UnityEngine.UI.Slider playerMorale;

	public global::UnityEngine.UI.Text playerMoraleText;

	public global::UnityEngine.UI.Text playerMoraleThresholdText;

	public global::UnityEngine.GameObject enemyMoraleBar;

	public global::UnityEngine.UI.Slider enemyMorale;

	public global::UnityEngine.UI.Text enemyMoraleText;

	public global::UnityEngine.UI.Text enemyMoraleThresholdText;

	public global::UnityEngine.UI.Image compass;

	private global::WarbandController playerWarbandController;

	private global::WarbandController enemyWarbandController;

	public global::UnityEngine.UI.Text mapNameText;

	public global::UnityEngine.UI.Text EnemyTurnText;

	public global::UnityEngine.UI.Text timer;

	private float oldTimer = -1f;

	public global::UnityEngine.RectTransform playerRout;

	public global::UnityEngine.RectTransform enemyRout;

	public float sliderHeight;

	private global::TurnTimer turnTimer;

	private float prevMorale = 1f;
}
