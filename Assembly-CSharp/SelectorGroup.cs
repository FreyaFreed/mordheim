using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectorGroup : global::UnityEngine.MonoBehaviour
{
	public int CurSel { get; private set; }

	private void Awake()
	{
		if (!this.init)
		{
			this.Init();
		}
	}

	private void Init()
	{
		this.init = true;
		this.isActive = true;
		global::UnityEngine.UI.Text[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.UI.Text>(true);
		foreach (global::UnityEngine.UI.Text text in componentsInChildren)
		{
			if (text.gameObject.name.Equals("value", global::System.StringComparison.OrdinalIgnoreCase))
			{
				this.text = text;
				break;
			}
		}
		if (this.text == null)
		{
			this.text = componentsInChildren[0];
		}
		global::UnityEngine.UI.Button[] componentsInChildren2 = base.GetComponentsInChildren<global::UnityEngine.UI.Button>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j].name.Contains("left"))
			{
				this.left = componentsInChildren2[j];
			}
			else if (componentsInChildren2[j].name.Contains("right"))
			{
				this.right = componentsInChildren2[j];
			}
		}
		this.selection = this.text.GetComponent<global::UnityEngine.UI.Button>();
		global::UnityEngine.EventSystems.EventTrigger trigger = this.left.gameObject.AddComponent<global::UnityEngine.EventSystems.EventTrigger>();
		global::UnityEngine.EventSystems.EventTrigger trigger2 = this.right.gameObject.AddComponent<global::UnityEngine.EventSystems.EventTrigger>();
		trigger.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.Select, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.OnLeft));
		trigger.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerClick, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.OnLeft));
		if (this.repeat)
		{
			trigger.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerDown, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.StartLeft));
			trigger.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerExit, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.EndLeft));
			trigger.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerUp, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.EndLeft));
		}
		trigger2.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.Select, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.OnRight));
		trigger2.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerClick, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.OnRight));
		if (this.repeat)
		{
			trigger2.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerDown, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.StartRight));
			trigger2.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerExit, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.EndRight));
			trigger2.AddUnityEvent(global::UnityEngine.EventSystems.EventTriggerType.PointerUp, new global::UnityEngine.Events.UnityAction<global::UnityEngine.EventSystems.BaseEventData>(this.EndRight));
		}
		this.CurSel = 0;
	}

	private void StartLeft(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.isCycling = true;
		this.time = 0f;
		this.currentInputWait = this.clampInputWait.y;
		this.mod = -1;
	}

	private void EndLeft(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.isCycling = false;
		this.selection.SetSelected(true);
	}

	private void OnLeft(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.isActive)
		{
			this.CurSel = ((this.CurSel - 1 < 0) ? ((!this.loopAround) ? 0 : (this.selections.Count - 1)) : (this.CurSel - 1));
			this.UpdateSelection();
		}
		this.selection.SetSelected(true);
	}

	private void StartRight(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.isCycling = true;
		this.time = 0f;
		this.currentInputWait = this.clampInputWait.y;
		this.mod = 1;
	}

	private void EndRight(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.isCycling = false;
		this.selection.SetSelected(true);
	}

	private void OnRight(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.isActive)
		{
			this.CurSel = ((this.CurSel + 1 >= this.selections.Count) ? ((!this.loopAround) ? (this.selections.Count - 1) : 0) : (this.CurSel + 1));
			this.UpdateSelection();
		}
		this.selection.SetSelected(true);
	}

	private void UpdateSelection()
	{
		this.text.text = this.selections[this.CurSel];
		if (this.onValueChanged != null)
		{
			this.onValueChanged(this.id, this.CurSel);
		}
	}

	public void SetButtonsVisible(bool show)
	{
		if (!this.init)
		{
			this.Init();
		}
		this.isActive = show;
		this.left.image.enabled = show;
		this.right.image.enabled = show;
	}

	public void SetCurrentSel(int sel)
	{
		this.CurSel = sel;
		this.text.text = this.selections[this.CurSel];
	}

	public void Update()
	{
		if (this.isActive && this.isCycling)
		{
			this.time += global::UnityEngine.Time.deltaTime;
			if (this.time > this.currentInputWait)
			{
				this.currentInputWait = global::UnityEngine.Mathf.Clamp(this.currentInputWait - 0.01f, this.clampInputWait.x, this.clampInputWait.y);
				this.time = 0f;
				int curSel = this.CurSel;
				if (this.mod > 0)
				{
					this.CurSel = ((this.CurSel + 1 >= this.selections.Count) ? ((!this.loopAround) ? (this.selections.Count - 1) : 0) : (this.CurSel + 1));
				}
				else
				{
					this.CurSel = ((this.CurSel - 1 < 0) ? ((!this.loopAround) ? 0 : (this.selections.Count - 1)) : (this.CurSel - 1));
				}
				if (this.CurSel != curSel)
				{
					this.UpdateSelection();
				}
			}
		}
	}

	private global::UnityEngine.UI.Text text;

	private global::UnityEngine.UI.Button left;

	private global::UnityEngine.UI.Button right;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.UI.Button selection;

	public int id;

	public bool loopAround = true;

	public bool repeat;

	public global::System.Collections.Generic.List<string> selections = new global::System.Collections.Generic.List<string>();

	public global::SelectorGroup.OnValueChanged onValueChanged;

	private bool init;

	private bool isActive = true;

	private int mod;

	private float time;

	private bool isCycling;

	private float currentInputWait = 0.2f;

	private global::UnityEngine.Vector2 clampInputWait = new global::UnityEngine.Vector2(0.05f, 0.2f);

	public delegate void OnValueChanged(int id, int index);
}
