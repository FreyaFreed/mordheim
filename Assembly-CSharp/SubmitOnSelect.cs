using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Button))]
public class SubmitOnSelect : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	private void Awake()
	{
		this.selectable = base.GetComponent<global::UnityEngine.UI.Button>();
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.selectable.OnSubmit(eventData);
	}

	private global::UnityEngine.UI.Button selectable;
}
