using System;

public class StoreMainMenuState : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override void StateEnter()
	{
	}

	public override void OnInputCancel()
	{
		base.StateMachine.GoToPrev();
	}

	public override void StateExit()
	{
	}

	public override int StateId
	{
		get
		{
			return 2;
		}
	}
}
