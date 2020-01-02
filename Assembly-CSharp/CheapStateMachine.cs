using System;

public class CheapStateMachine
{
	public CheapStateMachine(int stateCount)
	{
		this.activeState = null;
		this.activeStateIdx = -1;
		this.blockingStateIdx = -999;
		this.states = new global::ICheapState[stateCount];
	}

	public virtual void Destroy()
	{
		this.Clear();
	}

	public void Clear()
	{
		for (int i = this.states.Length - 1; i >= 0; i--)
		{
			if (this.states[i] != null)
			{
				this.states[i].Destroy();
				this.states[i] = null;
			}
		}
		this.states = null;
	}

	public void AddState(global::ICheapState state, int stateIndex)
	{
		this.states[stateIndex] = state;
	}

	public void ExitCurrentState()
	{
		if (this.activeState != null)
		{
			this.activeState.Exit(-1);
		}
		this.activeState = null;
		this.activeStateIdx = -1;
	}

	public void FixedUpdate()
	{
		if (this.activeState != null)
		{
			this.activeState.FixedUpdate();
		}
	}

	public void Update()
	{
		if (this.activeState != null && !this.skipFrame)
		{
			this.activeState.Update();
		}
		this.skipFrame = false;
	}

	public void ChangeState(int stateIndex)
	{
		if (this.activeStateIdx == this.blockingStateIdx)
		{
			return;
		}
		if (this.activeState != null)
		{
			this.activeState.Exit(stateIndex);
		}
		int iFrom = this.activeStateIdx;
		this.activeStateIdx = stateIndex;
		this.activeState = this.states[stateIndex];
		this.activeState.Enter(iFrom);
		this.skipFrame = true;
	}

	public global::ICheapState GetState(int stateIndex)
	{
		return this.states[stateIndex];
	}

	public int GetActiveStateId()
	{
		return this.activeStateIdx;
	}

	public global::ICheapState GetActiveState()
	{
		return this.activeState;
	}

	public void SetBlockingStateIdx(int idx)
	{
		this.blockingStateIdx = idx;
	}

	private int activeStateIdx;

	private global::ICheapState activeState;

	private global::ICheapState[] states;

	private bool skipFrame;

	private int blockingStateIdx;
}
