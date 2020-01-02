using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectHandler : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.selected.Invoke();
	}

	public global::UnityEngine.Events.UnityEvent selected;
}
