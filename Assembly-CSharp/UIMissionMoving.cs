using System;

internal class UIMissionMoving : global::ICheapState
{
	public UIMissionMoving(global::UIMissionManager uiMissionManager)
	{
		this.UiMissionManager = uiMissionManager;
	}

	protected global::UIMissionManager UiMissionManager { get; set; }

	public void Destroy()
	{
	}

	public virtual void Enter(int iFrom)
	{
		this.UiMissionManager.interactiveMessage.OnEnable();
		this.UiMissionManager.morale.OnEnable();
		this.UiMissionManager.leftSequenceMessage.OnDisable();
		this.UiMissionManager.rightSequenceMessage.OnDisable();
		if (this.UiMissionManager.CurrentUnitController != null && (this.UiMissionManager.CurrentUnitController.IsPlayed() || this.UiMissionManager.ShowingOverview))
		{
			if (this.UiMissionManager.ShowingOverview)
			{
				this.UiMissionManager.unitAction.OnDisable();
			}
			else
			{
				this.UiMissionManager.unitAction.OnEnable();
			}
			this.UiMissionManager.unitCombatStats.OnEnable();
			this.UiMissionManager.ShowUnitExtraStats();
		}
		else
		{
			if (this.UiMissionManager.ShowingOverview)
			{
				this.UiMissionManager.unitAction.OnDisable();
			}
			else
			{
				this.UiMissionManager.unitAction.WaitingOpponent();
			}
			this.UiMissionManager.unitCombatStats.OnDisable();
			this.UiMissionManager.unitAlternateWeapon.OnDisable();
			this.UiMissionManager.unitStats.OnDisable();
			this.UiMissionManager.unitEnchantments.OnDisable();
			this.UiMissionManager.unitEnchantmentsDebuffs.OnDisable();
		}
		this.UiMissionManager.ShowObjectives();
	}

	public virtual void Exit(int iTo)
	{
		if (iTo != 0 && iTo != 1)
		{
			this.UiMissionManager.interactiveMessage.OnDisable();
			this.UiMissionManager.unitAction.OnDisable();
			this.UiMissionManager.unitCombatStats.OnDisable();
			this.UiMissionManager.unitAlternateWeapon.OnDisable();
			this.UiMissionManager.unitStats.OnDisable();
			this.UiMissionManager.unitEnchantments.OnDisable();
			this.UiMissionManager.unitEnchantmentsDebuffs.OnDisable();
		}
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
	}
}
