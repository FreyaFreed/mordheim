using System;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemapButtonPopupView : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CONTROLLER_CONNECTED, new global::DelReceiveNotice(this.Hide));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CONTROLLER_DISCONNECTED, new global::DelReceiveNotice(this.Hide));
	}

	private void OnDestroy()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.CONTROLLER_CONNECTED, new global::DelReceiveNotice(this.Hide));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.CONTROLLER_DISCONNECTED, new global::DelReceiveNotice(this.Hide));
	}

	public void Show(global::System.Action<global::Rewired.Pole, global::Rewired.ControllerPollingInfo> callback, global::Rewired.ControllerType controller, string actionName)
	{
		switch (controller)
		{
		case global::Rewired.ControllerType.Keyboard:
			this.instructionsField.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_remap_action_instructions_keyboard", new string[]
			{
				actionName
			});
			break;
		case global::Rewired.ControllerType.Joystick:
			this.instructionsField.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_remap_action_instructions_controller", new string[]
			{
				actionName
			});
			break;
		}
		this.isShown = true;
		this.closeOnUp = false;
		this.controller = controller;
		this.callback = callback;
		base.gameObject.SetActive(true);
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.POP_UP);
		base.gameObject.SetSelected(true);
	}

	public void Hide()
	{
		if (this.isShown)
		{
			this.isShown = false;
			base.gameObject.SetActive(false);
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.POP_UP);
			if (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == base.gameObject)
			{
				global::UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			}
		}
	}

	private void Update()
	{
		switch (this.controller)
		{
		case global::Rewired.ControllerType.Keyboard:
			if ((this.closeOnUp && !this.keyFromMouse && global::Rewired.ReInput.controllers.Keyboard.GetKeyUp(this.pollInfo.keyboardKey)) || (this.keyFromMouse && global::Rewired.ReInput.controllers.Mouse.GetButtonUpById(this.pollInfo.elementIdentifierId)))
			{
				this.callback(global::Rewired.Pole.Positive, this.pollInfo);
				this.Hide();
			}
			else
			{
				this.PollKeyboardForAssignment();
			}
			break;
		case global::Rewired.ControllerType.Joystick:
			if (this.closeOnUp && global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.Joysticks[0].GetButtonUpById(this.pollInfo.elementIdentifierId))
			{
				this.callback(global::Rewired.Pole.Positive, this.pollInfo);
				this.Hide();
			}
			else
			{
				this.PollJoystickForAssignment();
			}
			break;
		}
	}

	private void PollKeyboardForAssignment()
	{
		global::Rewired.ControllerPollingInfo controllerPollingInfo = global::Rewired.ReInput.controllers.Keyboard.PollForFirstKey();
		if (controllerPollingInfo.success)
		{
			global::UnityEngine.KeyCode keyboardKey = controllerPollingInfo.keyboardKey;
			if (keyboardKey != global::UnityEngine.KeyCode.LeftWindows && keyboardKey != global::UnityEngine.KeyCode.RightWindows && keyboardKey != global::UnityEngine.KeyCode.Menu && keyboardKey != global::UnityEngine.KeyCode.LeftCommand && keyboardKey != global::UnityEngine.KeyCode.RightCommand && keyboardKey != global::UnityEngine.KeyCode.Escape && global::InputImageTable.butTable.ContainsKey(controllerPollingInfo.elementIdentifierName))
			{
				this.pollInfo = controllerPollingInfo;
				this.closeOnUp = true;
				this.keyFromMouse = false;
			}
		}
		else
		{
			global::Rewired.ControllerPollingInfo controllerPollingInfo2 = global::Rewired.ReInput.controllers.Mouse.PollForFirstButtonDown();
			if (controllerPollingInfo2.success && global::InputImageTable.butTable.ContainsKey(controllerPollingInfo2.elementIdentifierName))
			{
				this.pollInfo = controllerPollingInfo2;
				this.closeOnUp = true;
				this.keyFromMouse = true;
			}
		}
	}

	private void PollJoystickForAssignment()
	{
		global::Rewired.ControllerPollingInfo arg = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.polling.PollControllerForFirstElementDown(this.controller, 0);
		if (arg.success && global::InputImageTable.butTable.ContainsKey("joy_" + arg.elementIdentifierName))
		{
			if (arg.elementType != global::Rewired.ControllerElementType.Axis)
			{
				this.pollInfo = arg;
				this.closeOnUp = true;
			}
			else
			{
				string text = arg.elementIdentifierName.ToLowerInvariant().Replace(" ", "_");
				if (!text.Equals("left_stick_x") && !text.Equals("left_stick_y") && !text.Equals("right_stick_x") && !text.Equals("right_stick_y"))
				{
					this.callback(arg.axisPole, arg);
					this.Hide();
				}
			}
		}
	}

	private const string INSTRUCTIONS_KEYB_STRING_ID = "menu_remap_action_instructions_keyboard";

	private const string INSTRUCTIONS_CTRL_STRING_ID = "menu_remap_action_instructions_controller";

	private bool isShown;

	private global::System.Action<global::Rewired.Pole, global::Rewired.ControllerPollingInfo> callback;

	private global::Rewired.ControllerType controller;

	private bool closeOnUp;

	private bool keyFromMouse;

	private global::Rewired.ControllerPollingInfo pollInfo;

	public global::UnityEngine.UI.Text instructionsField;
}
