using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChatLog : global::CanvasGroupDisabler
{
	private void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKey("scroll", 2))
		{
			this.messages.scrollRect.OnScroll(this.scrollUpData);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKey("scroll", 2))
		{
			this.messages.scrollRect.OnScroll(this.scrollDownData);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKey("scroll", 7))
		{
			this.combatLog.scrollRect.OnScroll(this.scrollUpData);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKey("scroll", 7))
		{
			this.combatLog.scrollRect.OnScroll(this.scrollDownData);
		}
	}

	public void Setup()
	{
		this.messages.Setup(this.item, true);
		this.messageEntries = new global::System.Collections.Generic.List<global::UIChatLogItem>();
		this.combatLog.Setup(this.item, true);
		this.combatLogEntries = new global::System.Collections.Generic.List<global::UIChatLogItem>();
		this.text.onEndEdit.AddListener(new global::UnityEngine.Events.UnityAction<string>(this.SendChat));
		this.combatLog.HideScrollbar();
		this.scrollUpData = new global::UnityEngine.EventSystems.PointerEventData(global::UnityEngine.EventSystems.EventSystem.current);
		this.scrollUpData.scrollDelta = new global::UnityEngine.Vector2(0f, 1f);
		this.scrollDownData = new global::UnityEngine.EventSystems.PointerEventData(global::UnityEngine.EventSystems.EventSystem.current);
		this.scrollDownData.scrollDelta = new global::UnityEngine.Vector2(0f, -1f);
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.HERMES_CHAT, new global::DelReceiveNotice(this.GetChat));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.COMBAT_LOG, new global::DelReceiveNotice(this.NewLog));
	}

	public void ShowChat(bool blockInput)
	{
		if (this.enlarged)
		{
			this.ShrinkLog();
		}
		base.OnEnable();
		this.messages.gameObject.SetActive(true);
		this.combatLog.gameObject.SetActive(false);
		this.lastListPos = 0f;
		if (blockInput)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode != global::PandoraInput.InputMode.JOYSTICK || !global::PandoraSingleton<global::Hephaestus>.Instance.ShowVirtualKeyboard(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_chat"), string.Empty, 100U, false, new global::Hephaestus.OnVirtualKeyboardCallback(this.OnVirtualKeyboard), true))
			{
				this.text.gameObject.SetActive(true);
				this.text.ActivateInputField();
				this.messages.ShowScrollbar(false);
			}
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.CHAT);
		}
		else
		{
			this.messages.HideScrollbar();
		}
		base.StopCoroutine("TurnOffChat");
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_chat");
	}

	private void OnVirtualKeyboard(bool success, string newString)
	{
		if (success)
		{
			this.SendChat(newString);
		}
		else
		{
			this.SendChat(null);
		}
	}

	public void HideLog()
	{
		if (this.enlarged)
		{
			this.ShrinkLog();
		}
		this.combatLog.gameObject.SetActive(false);
		this.showLog = false;
		base.OnDisable();
	}

	public void ToggleLogDisplay()
	{
		if (this.showLog && this.combatLog.gameObject.activeSelf)
		{
			if (this.enlarged)
			{
				this.HideLog();
			}
			else
			{
				this.EnlargeLog();
			}
		}
		else
		{
			this.messages.gameObject.SetActive(false);
			this.combatLog.gameObject.SetActive(true);
			this.showLog = true;
			base.OnEnable();
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_log");
		}
		this.lastListPos = 0f;
		base.StartCoroutine("ScrollToLastOnNextFrame", this.combatLog);
	}

	public void ShowLog()
	{
		if (this.showLog && this.combatLog.gameObject.activeSelf)
		{
			if (this.enlarged)
			{
				this.ShrinkLog();
			}
			this.combatLog.gameObject.SetActive(false);
			this.showLog = false;
			base.OnDisable();
			return;
		}
		this.messages.gameObject.SetActive(false);
		this.combatLog.gameObject.SetActive(true);
		this.showLog = true;
		base.OnEnable();
		this.lastListPos = 0f;
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_log");
	}

	public void EnlargeLog()
	{
		if (!this.enlarged)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.LOG);
			this.combatLog.ShowScrollbar(true);
			this.combatLog.OnEnable();
			global::UnityEngine.Vector2 sizeDelta = ((global::UnityEngine.RectTransform)base.transform).sizeDelta;
			sizeDelta.x *= this.enlargeRatioX;
			sizeDelta.y *= this.enlargeRatioY;
			((global::UnityEngine.RectTransform)base.transform).sizeDelta = sizeDelta;
			this.enlarged = true;
		}
	}

	public void ShrinkLog()
	{
		if (this.enlarged)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.LOG);
			this.combatLog.HideScrollbar();
			global::UnityEngine.Vector2 sizeDelta = ((global::UnityEngine.RectTransform)base.transform).sizeDelta;
			sizeDelta.x /= this.enlargeRatioX;
			sizeDelta.y /= this.enlargeRatioY;
			((global::UnityEngine.RectTransform)base.transform).sizeDelta = sizeDelta;
			this.enlarged = false;
		}
	}

	private void NewLog()
	{
		string content = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		global::UnityEngine.GameObject gameObject = this.combatLog.AddToList(null, null);
		global::UIChatLogItem component = gameObject.GetComponent<global::UIChatLogItem>();
		component.Init(content);
		this.combatLogEntries.Add(component);
		if (this.combatLogEntries.Count > this.combatLog.items.Count)
		{
			this.combatLogEntries.RemoveAt(0);
		}
		if (!this.enlarged)
		{
			base.StartCoroutine("ScrollToLastOnNextFrame", this.combatLog);
		}
	}

	private global::System.Collections.IEnumerator ScrollToLastOnNextFrame(global::ScrollGroup scrollGroup)
	{
		if (scrollGroup.items.Count == 0)
		{
			yield break;
		}
		yield return null;
		scrollGroup.RealignList(true, scrollGroup.items.Count - 1, true);
		yield return null;
		yield break;
	}

	private void GetChat()
	{
		ulong num = (ulong)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		string content = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		global::UnityEngine.GameObject gameObject = this.messages.AddToList(null, null);
		this.messageEntries.Add(gameObject.GetComponent<global::UIChatLogItem>());
		if (this.messageEntries.Count > this.messages.items.Count)
		{
			this.messageEntries.RemoveAt(0);
		}
		global::UIChatLogItem component = gameObject.GetComponent<global::UIChatLogItem>();
		if (num != 0UL)
		{
			component.GetComponent<global::UnityEngine.UI.Text>().color = new global::UnityEngine.Color(127f, 127f, 0f);
		}
		component.Init(content);
		base.StartCoroutine("ScrollToLastOnNextFrame", this.messages);
		this.ShowChat(false);
		if (!this.text.isFocused)
		{
			this.StartTurnOff();
		}
	}

	private void OnEndChatEdit(string message)
	{
		if (!this.showLog)
		{
			this.SendChat(message);
		}
	}

	private void SendChat(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			global::PandoraSingleton<global::Hermes>.Instance.SendChat(message);
			this.text.text = string.Empty;
		}
		else
		{
			this.StartTurnOff();
		}
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.CHAT);
		this.text.DeactivateInputField();
		this.text.gameObject.SetActive(false);
	}

	private void StartTurnOff()
	{
		base.StopCoroutine("TurnOffChat");
		base.StartCoroutine("TurnOffChat");
	}

	private global::System.Collections.IEnumerator TurnOffChat()
	{
		yield return new global::UnityEngine.WaitForSeconds(5f);
		if (this.showLog)
		{
			this.showLog = false;
			this.ShowLog();
		}
		else
		{
			this.messages.gameObject.SetActive(false);
			base.OnDisable();
		}
		yield break;
	}

	private const float CHAT_UPTIME = 5f;

	public float enlargeRatioX = 1f;

	public float enlargeRatioY = 2f;

	public global::ScrollGroup messages;

	private global::System.Collections.Generic.List<global::UIChatLogItem> messageEntries;

	public global::ScrollGroup combatLog;

	private global::System.Collections.Generic.List<global::UIChatLogItem> combatLogEntries;

	private global::UnityEngine.EventSystems.PointerEventData scrollUpData;

	private global::UnityEngine.EventSystems.PointerEventData scrollDownData;

	public global::UnityEngine.GameObject item;

	public global::UnityEngine.RectTransform maskTransform;

	public global::UnityEngine.UI.InputField text;

	public global::UnityEngine.UI.Text title;

	private bool showLog;

	private bool enlarged;

	private float lastListPos;
}
