using System;
using System.Collections.Generic;

public class UIStateMachineMonoBehaviour : global::PandoraSingleton<global::UIStateMachineMonoBehaviour>, global::IStateMachine
{
	public global::IState PrevState { get; private set; }

	public global::IState CurrentState { get; private set; }

	public global::IState NextState { get; private set; }

	public void Register(int stateId, global::IState state)
	{
		this._states.Add(stateId, state);
	}

	protected virtual void Update()
	{
		if (this.NextState != null)
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.StateExit();
				this.PrevState = this.CurrentState;
			}
			this.CurrentState = this.NextState;
			this.NextState = null;
			this.CurrentState.StateEnter();
		}
		else if (this.CurrentState != null)
		{
			this.CurrentState.StateUpdate();
		}
	}

	public void GoToPrev()
	{
		this.ChangeState(this.PrevState.StateId);
	}

	public void ChangeState(int stateId)
	{
		this.NextState = this._states[stateId];
	}

	private global::System.Collections.Generic.Dictionary<int, global::IState> _states = new global::System.Collections.Generic.Dictionary<int, global::IState>();
}
