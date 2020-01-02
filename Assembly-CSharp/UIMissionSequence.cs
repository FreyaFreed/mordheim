using System;

internal class UIMissionSequence : global::ICheapState
{
	public UIMissionSequence(global::UIMissionManager missionManager)
	{
		this.UiMissionManager = missionManager;
	}

	public global::UIMissionManager UiMissionManager { get; set; }

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.IsPlayed() || global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			this.UiMissionManager.ladder.OnDisable();
			this.UiMissionManager.morale.OnDisable();
			this.UiMissionManager.leftSequenceMessage.OnDisable();
			this.UiMissionManager.rightSequenceMessage.OnDisable();
			this.UiMissionManager.objectives.OnDisable();
			foreach (global::UISlideInElement uislideInElement in global::PandoraSingleton<global::UIMissionManager>.Instance.extraStats)
			{
				uislideInElement.OnDisable();
			}
		}
		else
		{
			this.UiMissionManager.unitAction.WaitingOpponent();
		}
	}

	public void Exit(int iTo)
	{
		this.UiMissionManager.ladder.OnEnable();
		this.UiMissionManager.morale.OnEnable();
		this.UiMissionManager.leftSequenceMessage.OnDisable();
		this.UiMissionManager.rightSequenceMessage.OnDisable();
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
	}
}
