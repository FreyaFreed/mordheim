using System;

public interface IState
{
	int StateId { get; }

	void StateEnter();

	void StateExit();

	void StateUpdate();
}
