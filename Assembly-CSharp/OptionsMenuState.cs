using System;
using UnityEngine;

public class OptionsMenuState : global::BaseUIState
{
	public OptionsMenuState(global::CheapStateMachine parentStateMachine, bool hasQuit, global::UnityEngine.GameObject view = null) : base(view)
	{
		this.parentStateMachine = parentStateMachine;
		this.canQuit = hasQuit;
		this.stateMachine = new global::CheapStateMachine(7);
		this.stateMachine.AddState(new global::GraphicOptionsMenuState(this), 0);
		this.stateMachine.AddState(new global::GameplayOptionsMenuState(this), 2);
		this.stateMachine.AddState(new global::ToMainMenuOptionsMenuState(this), 4);
		this.stateMachine.AddState(new global::QuitOptionsMenuState(this), 5);
		this.stateMachine.AddState(new global::BrowseOptionsMenuState(this), 6);
	}

	public override void Enter(int iFrom)
	{
		base.Enter(iFrom);
		this.GoTo(global::OptionsMenuState.State.BROWSE);
	}

	public override void Exit(int iTo)
	{
		this.stateMachine.ExitCurrentState();
		base.Exit(iTo);
	}

	public override void Destroy()
	{
		this.stateMachine.Destroy();
	}

	public override void InputAction()
	{
	}

	public override void InputCancel()
	{
	}

	public override void FixedUpdate()
	{
		this.stateMachine.FixedUpdate();
	}

	public override void Update()
	{
		this.stateMachine.Update();
		base.Update();
	}

	public void GoTo(global::OptionsMenuState.State state)
	{
		this.stateMachine.ChangeState((int)state);
	}

	public void ExitState()
	{
		this.parentStateMachine.ExitCurrentState();
	}

	public bool canQuit;

	private global::CheapStateMachine stateMachine;

	private global::CheapStateMachine parentStateMachine;

	public enum State
	{
		GRAPHICS,
		AUDIO,
		GAMEPLAY,
		CONTROL,
		TO_MAIN_MENU,
		QUIT,
		BROWSE,
		NB_STATE
	}
}
