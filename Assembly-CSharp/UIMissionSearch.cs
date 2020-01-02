using System;

internal class UIMissionSearch : global::ICheapState
{
	public UIMissionSearch(global::UIMissionManager missionManager)
	{
		this.UiMissionManager = missionManager;
	}

	public global::UIMissionManager UiMissionManager { get; set; }

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.IsPlayed())
		{
			this.UiMissionManager.ladder.OnDisable();
			this.UiMissionManager.morale.OnDisable();
			this.UiMissionManager.leftSequenceMessage.OnDisable();
			this.UiMissionManager.rightSequenceMessage.OnDisable();
			this.UiMissionManager.objectives.OnDisable();
			this.UiMissionManager.inventory.Show();
			foreach (global::UISlideInElement uislideInElement in global::PandoraSingleton<global::UIMissionManager>.Instance.extraStats)
			{
				uislideInElement.OnDisable();
			}
		}
	}

	public void Exit(int iTo)
	{
		this.UiMissionManager.inventory.OnDisable();
		this.UiMissionManager.ladder.OnEnable();
		this.UiMissionManager.morale.OnEnable();
		this.UiMissionManager.leftSequenceMessage.OnDisable();
		this.UiMissionManager.rightSequenceMessage.OnDisable();
		this.UiMissionManager.ShowObjectives();
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
	}
}
