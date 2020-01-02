using System;
using UnityEngine;

public abstract class BaseUIState : global::ICheapState
{
	protected BaseUIState(global::UnityEngine.GameObject view)
	{
		this.View = view;
		if (this.View != null)
		{
			this.Transform = this.View.transform;
			this._startPosition = this.Transform.localPosition;
			this.View.SetActive(false);
		}
	}

	public abstract void Destroy();

	public abstract void InputAction();

	public abstract void InputCancel();

	public virtual void Enter(int iFrom)
	{
		if (this.View != null)
		{
			this.Transform.localPosition = global::UnityEngine.Vector3.zero;
			this.View.SetActive(true);
		}
	}

	public virtual void Exit(int iTo)
	{
		if (this.View != null)
		{
			this.View.SetActive(false);
			this.Transform.localPosition = this._startPosition;
		}
	}

	public virtual void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
		{
			this.InputCancel();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			this.InputAction();
		}
	}

	public abstract void FixedUpdate();

	protected global::UnityEngine.GameObject View;

	protected global::UnityEngine.Transform Transform;

	private global::UnityEngine.Vector3 _startPosition;
}
