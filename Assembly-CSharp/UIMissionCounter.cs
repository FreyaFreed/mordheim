using System;

internal class UIMissionCounter : global::UIMissionTarget
{
	public UIMissionCounter(global::UIMissionManager uiMissionManager) : base(uiMissionManager)
	{
	}

	public override void Enter(int iFrom)
	{
		base.Enter(iFrom);
	}

	public override void Exit(int iTo)
	{
		base.Exit(iTo);
		base.UiMissionManager.leftSequenceMessage.OnDisable();
	}
}
