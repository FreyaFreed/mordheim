using System;
using UnityEngine;

internal class UIMissionEndGame : global::ICheapState
{
	public UIMissionEndGame(global::UIMissionManager missionManager)
	{
		this.UiMissionManager = missionManager;
	}

	public global::UIMissionManager UiMissionManager { get; set; }

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.UiMissionManager.ladder.gameObject.SetActive(false);
		this.UiMissionManager.morale.gameObject.SetActive(false);
		this.UiMissionManager.wheel.gameObject.SetActive(false);
		this.UiMissionManager.objectives.gameObject.SetActive(false);
		this.UiMissionManager.unitAction.gameObject.SetActive(false);
		this.UiMissionManager.unitStats.gameObject.SetActive(false);
		this.UiMissionManager.unitCombatStats.gameObject.SetActive(false);
		this.UiMissionManager.unitAlternateWeapon.gameObject.SetActive(false);
		this.UiMissionManager.unitEnchantments.gameObject.SetActive(false);
		this.UiMissionManager.unitEnchantmentsDebuffs.gameObject.SetActive(false);
		this.UiMissionManager.targetAlternateWeapon.gameObject.SetActive(false);
		this.UiMissionManager.targetCombatStats.gameObject.SetActive(false);
		this.UiMissionManager.targetEnchantments.gameObject.SetActive(false);
		this.UiMissionManager.targetStats.gameObject.SetActive(false);
		this.UiMissionManager.leftSequenceMessage.gameObject.SetActive(false);
		this.UiMissionManager.rightSequenceMessage.gameObject.SetActive(false);
		this.UiMissionManager.HideOptions();
		this.timer = 2.5f;
	}

	public void Exit(int iTo)
	{
	}

	public void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= global::UnityEngine.Time.deltaTime;
			if (this.timer <= 0f)
			{
				this.UiMissionManager.endGameReport.Show();
			}
		}
	}

	public void FixedUpdate()
	{
	}

	private float timer;
}
