using System;

public interface IStateMachine
{
	void Register(int stateId, global::IState state);
}
