using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class UGUIExtensions
{
	public static void SetSelected(this global::UnityEngine.EventSystems.EventSystem eventSystem, global::UnityEngine.MonoBehaviour selected, bool force = false)
	{
		eventSystem.SetSelected(selected.gameObject, force);
	}

	public static void SetSelected(this global::UnityEngine.EventSystems.EventSystem eventSystem, global::UnityEngine.GameObject selected, bool force = false)
	{
		if (eventSystem == null)
		{
			return;
		}
		global::UnityEngine.EventSystems.BaseEventData baseEventData = new global::UnityEngine.EventSystems.BaseEventData(eventSystem);
		if (!eventSystem.alreadySelecting)
		{
			eventSystem.SetSelectedGameObject(selected, baseEventData);
		}
		else if (force && !global::UGUIExtensions.waitingToSelect)
		{
			global::UGUIExtensions.waitingToSelect = true;
			eventSystem.StartCoroutine(global::UGUIExtensions.SelectOnNextTrame(eventSystem, selected, baseEventData));
		}
	}

	private static global::System.Collections.IEnumerator SelectOnNextTrame(global::UnityEngine.EventSystems.EventSystem eventSystem, global::UnityEngine.GameObject selected, global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		yield return null;
		eventSystem.SetSelectedGameObject(selected, eventData);
		global::UGUIExtensions.waitingToSelect = false;
		yield break;
	}

	public static void SetSelected(this global::UnityEngine.MonoBehaviour selected, bool force = false)
	{
		global::UnityEngine.EventSystems.EventSystem.current.SetSelected(selected, force);
	}

	public static void SetSelected(this global::UnityEngine.GameObject selected, bool force = false)
	{
		global::UnityEngine.EventSystems.EventSystem.current.SetSelected(selected, force);
	}

	public static void AddUnityEvent(this global::UnityEngine.EventSystems.EventTrigger trigger, global::UnityEngine.EventSystems.EventTriggerType triggerType, global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData> action)
	{
		global::UnityEngine.EventSystems.EventTrigger.Entry entry = new global::UnityEngine.EventSystems.EventTrigger.Entry();
		entry.eventID = triggerType;
		entry.callback.AddListener(action);
		if (trigger.triggers == null)
		{
			trigger.triggers = new global::System.Collections.Generic.List<global::UnityEngine.EventSystems.EventTrigger.Entry>();
		}
		trigger.triggers.Add(entry);
	}

	public static void AddUnityEvent(this global::UnityEngine.MonoBehaviour mono, global::UnityEngine.EventSystems.EventTriggerType triggerType, global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData> action)
	{
		mono.gameObject.AddUnityEvent(triggerType, action);
	}

	public static void AddUnityEvent(this global::UnityEngine.GameObject go, global::UnityEngine.EventSystems.EventTriggerType triggerType, global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData> action)
	{
		global::UnityEngine.EventSystems.EventTrigger eventTrigger = go.GetComponent<global::UnityEngine.EventSystems.EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = go.AddComponent<global::UnityEngine.EventSystems.EventTrigger>();
		}
		eventTrigger.AddUnityEvent(triggerType, action);
	}

	public static void ResetUnityEvent(this global::UnityEngine.GameObject go)
	{
		global::UnityEngine.EventSystems.EventTrigger component = go.GetComponent<global::UnityEngine.EventSystems.EventTrigger>();
		if (component != null)
		{
			component.triggers.Clear();
		}
	}

	public static void ResetUnityEvent(this global::UnityEngine.MonoBehaviour go)
	{
		go.gameObject.ResetUnityEvent();
	}

	private static bool waitingToSelect;
}
