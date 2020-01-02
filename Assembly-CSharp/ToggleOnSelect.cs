using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Selectable))]
public class ToggleOnSelect : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	private void Awake()
	{
		if (this.toggle == null)
		{
			this.toggle = base.GetComponent<global::UnityEngine.UI.Toggle>();
		}
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.toggle.isOn = true;
	}

	public global::UnityEngine.UI.Toggle toggle;
}
