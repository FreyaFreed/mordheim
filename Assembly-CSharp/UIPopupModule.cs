using System;
using UnityEngine;

public class UIPopupModule : global::UnityEngine.MonoBehaviour
{
	public bool initialized { get; private set; }

	protected virtual void Awake()
	{
		this.canvas = base.GetComponent<global::UnityEngine.CanvasGroup>();
	}

	public virtual void Init()
	{
		this.initialized = true;
	}

	public void SetInteractable(bool interactable)
	{
		if (this.canvas != null)
		{
			this.canvas.interactable = interactable;
			this.canvas.blocksRaycasts = interactable;
		}
	}

	public global::PopupModuleId popupModuleId;

	protected global::UnityEngine.CanvasGroup canvas;
}
