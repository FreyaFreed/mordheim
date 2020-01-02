using System;

public interface ICheapState
{
	void Destroy();

	void Enter(int iFrom);

	void Exit(int iTo);

	void Update();

	void FixedUpdate();
}
