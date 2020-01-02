using System;

internal class UIMissionDeploy : global::ICheapState
{
	public UIMissionDeploy(global::UIMissionManager uiMissionManager)
	{
		this.UiMissionManager = uiMissionManager;
	}

	protected global::UIMissionManager UiMissionManager { get; set; }

	public void Destroy()
	{
	}

	public virtual void Enter(int iFrom)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_CHANGED, new global::DelReceiveNotice(this.ShowUnitUi));
		this.UiMissionManager.morale.OnEnable();
		this.UiMissionManager.ladder.OnEnable();
		this.UiMissionManager.objectives.OnDisable();
		this.ShowUnitUi();
	}

	private void ShowUnitUi()
	{
		if (this.UiMissionManager.CurrentUnitController != null && this.UiMissionManager.CurrentUnitController.IsPlayed())
		{
			this.UiMissionManager.unitCombatStats.OnEnable();
			this.UiMissionManager.ShowUnitExtraStats();
			this.UiMissionManager.deployControls.OnEnable();
			this.UiMissionManager.unitAction.OnDisable();
		}
		else
		{
			this.UiMissionManager.unitCombatStats.OnDisable();
			this.UiMissionManager.unitAlternateWeapon.OnDisable();
			this.UiMissionManager.unitStats.OnDisable();
			this.UiMissionManager.unitEnchantments.OnDisable();
			this.UiMissionManager.unitEnchantmentsDebuffs.OnDisable();
			this.UiMissionManager.deployControls.OnDisable();
			this.UiMissionManager.unitAction.WaitingOpponent();
		}
	}

	public virtual void Exit(int iTo)
	{
		this.UiMissionManager.deployControls.OnDisable();
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.CURRENT_UNIT_CHANGED, new global::DelReceiveNotice(this.ShowUnitUi));
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
	}
}
