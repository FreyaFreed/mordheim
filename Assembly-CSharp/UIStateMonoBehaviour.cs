using System;
using UnityEngine;

public abstract class UIStateMonoBehaviour<TStateMachine> : global::UnityEngine.MonoBehaviour, global::IState where TStateMachine : global::IStateMachine
{
	public TStateMachine StateMachine { get; private set; }

	public abstract int StateId { get; }

	public virtual void Awake()
	{
		if (this.canvasGroup == null)
		{
			this.canvasGroup = base.GetComponent<global::UnityEngine.CanvasGroup>();
		}
		this.cachedTransform = this.canvasGroup.transform;
		this.lastPosition = this.cachedTransform.localPosition;
		this.StateMachine = base.GetComponentInParent<TStateMachine>();
		TStateMachine stateMachine = this.StateMachine;
		stateMachine.Register(this.StateId, this);
	}

	protected virtual void Start()
	{
		this.Show(false);
	}

	public abstract void StateEnter();

	public virtual void StateUpdate()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
		{
			this.OnInputCancel();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			this.OnInputAction(false);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("mouse_click", 0))
		{
			this.OnInputAction(true);
		}
	}

	public abstract void StateExit();

	public virtual void OnInputAction(bool isMouse)
	{
	}

	public virtual void OnInputCancel()
	{
	}

	public void Show(bool visible)
	{
		this.canvasGroup.gameObject.SetActive(visible);
		this.canvasGroup.interactable = visible;
		this.cachedTransform.localPosition = ((!visible) ? this.lastPosition : global::UnityEngine.Vector3.zero);
		this.canvasGroup.interactable = visible;
	}

	public global::UnityEngine.CanvasGroup canvasGroup;

	private global::UnityEngine.Transform cachedTransform;

	private global::UnityEngine.Vector3 lastPosition;
}
