using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;

public class PandoraInput : global::PandoraSingleton<global::PandoraInput>
{
	public int CurrentInputLayer { get; private set; }

	public global::PandoraInput.InputMode lastInputMode { get; private set; }

	public bool IsActive { get; private set; }

	public global::Rewired.Player player { get; private set; }

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.lastInputMode = global::PandoraInput.InputMode.NONE;
		this.popLayers = new global::System.Collections.Generic.List<global::PandoraInput.InputLayer>();
		this.inputLayers = new global::System.Collections.Generic.List<global::PandoraInput.InputLayer>();
		this.PushInputLayer(global::PandoraInput.InputLayer.NORMAL);
		this.IsActive = false;
		this.showCursor = true;
		global::UnityEngine.Cursor.lockState = global::UnityEngine.CursorLockMode.Confined;
		this.player = global::Rewired.ReInput.players.GetPlayer(0);
		this.player.controllers.AddController(global::Rewired.ControllerType.Joystick, 0, true);
		global::Rewired.ReInput.ControllerConnectedEvent += this.OnControllerConnected;
		global::Rewired.ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		this.keyRepeatData = new global::System.Collections.Generic.Dictionary<string, float>();
		this.initialized = true;
	}

	public void SetLastInputMode(global::PandoraInput.InputMode mode)
	{
		if (mode != this.lastInputMode)
		{
			this.lastInputMode = mode;
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.INPUT_TYPE_CHANGED);
		}
	}

	public void Rumble(float leftVibration, float rightVibration)
	{
		for (int i = 0; i < this.player.controllers.Joysticks.Count; i++)
		{
			if (this.player.controllers.Joysticks[i].supportsVibration)
			{
				this.player.controllers.Joysticks[i].SetVibration(leftVibration, rightVibration);
			}
		}
	}

	public void StopRumble()
	{
		for (int i = 0; i < this.player.controllers.Joysticks.Count; i++)
		{
			if (this.player.controllers.Joysticks[i].supportsVibration)
			{
				this.player.controllers.Joysticks[i].StopVibration();
			}
		}
	}

	private void OnControllerConnected(global::Rewired.ControllerStatusChangedEventArgs args)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"A controller was connected! Name = ",
			args.name,
			" Id = ",
			args.controllerId,
			" Type = ",
			args.controllerType
		}), "INPUT", null);
		this.player.controllers.AddController(global::Rewired.ReInput.controllers.GetController(args.controllerType, args.controllerId), true);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.CONTROLLER_CONNECTED);
	}

	private void OnControllerDisconnected(global::Rewired.ControllerStatusChangedEventArgs args)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"A controller was disconnected! Name = ",
			args.name,
			" Id = ",
			args.controllerId,
			" Type = ",
			args.controllerType
		}), "INPUT", null);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.CONTROLLER_DISCONNECTED);
	}

	public void LoadMappingFromXml(global::Rewired.ControllerType controller, global::System.Collections.Generic.List<string> mappings)
	{
		if (mappings != null && mappings.Count > 0)
		{
			this.player.controllers.maps.ClearMaps(controller, true);
			if (controller == global::Rewired.ControllerType.Joystick)
			{
				for (int i = 0; i < this.player.controllers.joystickCount; i++)
				{
					global::Rewired.Joystick joystick = this.player.controllers.Joysticks[i];
					this.player.controllers.maps.AddMapsFromXml(joystick.type, joystick.id, mappings);
				}
			}
			else
			{
				this.player.controllers.maps.AddMapsFromXml(controller, 0, mappings);
			}
			foreach (global::Rewired.ControllerMap controllerMap in this.player.controllers.maps.GetAllMaps())
			{
				controllerMap.enabled = (controllerMap.categoryId == (int)this.currentStateId);
			}
		}
	}

	public void RestoreDefaultMappings()
	{
		this.player.controllers.maps.LoadDefaultMaps(global::Rewired.ControllerType.Keyboard);
		this.player.controllers.maps.LoadDefaultMaps(global::Rewired.ControllerType.Joystick);
		this.player.controllers.maps.LoadDefaultMaps(global::Rewired.ControllerType.Mouse);
		if (this.leftHandedController)
		{
			this.leftHandedController = false;
			this.SetLeftHandedController(true, true);
		}
		if (this.leftHandedMouse)
		{
			this.leftHandedMouse = false;
			this.SetLeftHandedMouse(true, true);
		}
	}

	public global::Rewired.ActionElementMap GetInputForAction(string actionName, global::Rewired.ControllerType controller, global::Rewired.Pole inputPole = global::Rewired.Pole.Positive)
	{
		foreach (global::Rewired.ControllerMap controllerMap in this.player.controllers.maps.GetAllMapsInCategory((int)this.currentStateId, controller))
		{
			global::Rewired.ActionElementMap[] elementMapsWithAction = controllerMap.GetElementMapsWithAction(actionName);
			if (elementMapsWithAction != null)
			{
				for (int i = 0; i < elementMapsWithAction.Length; i++)
				{
					if (elementMapsWithAction[i].axisContribution == inputPole || (elementMapsWithAction[i].elementType == global::Rewired.ControllerElementType.Axis && elementMapsWithAction[i].axisRange == global::Rewired.AxisRange.Full))
					{
						return elementMapsWithAction[i];
					}
				}
			}
		}
		return null;
	}

	public void ActivateController(global::Rewired.ControllerType controllerType)
	{
		if (this.player != null)
		{
			this.player.controllers.AddController(controllerType, 0, true);
		}
	}

	public void DeactivateController(global::Rewired.ControllerType controllerType)
	{
		if (this.player != null)
		{
			this.player.controllers.RemoveController(controllerType, 0);
		}
	}

	public void SetActionInverted(string action, bool inverted = true)
	{
		int actionId = global::Rewired.ReInput.mapping.GetActionId(action);
		foreach (global::Rewired.ControllerMap controllerMap in this.player.controllers.maps.GetAllMaps())
		{
			foreach (global::Rewired.ActionElementMap actionElementMap in controllerMap.ElementMapsWithAction(actionId))
			{
				actionElementMap.invert = inverted;
			}
		}
	}

	public void SetLeftHandedMouse(bool leftHanded = true, bool includeUserAssignables = false)
	{
		if (leftHanded != this.leftHandedMouse)
		{
			this.leftHandedMouse = leftHanded;
			if (this.leftHandedMouse)
			{
				global::UnityEngine.EventSystems.PandoraInputModule.ActionInput = global::UnityEngine.EventSystems.PointerEventData.InputButton.Right;
			}
			else
			{
				global::UnityEngine.EventSystems.PandoraInputModule.ActionInput = global::UnityEngine.EventSystems.PointerEventData.InputButton.Left;
			}
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < global::Rewired.ReInput.controllers.Mouse.ButtonElementIdentifiers.Count; i++)
			{
				if (global::Rewired.ReInput.controllers.Mouse.ButtonElementIdentifiers[i].name.Equals("Left Mouse Button"))
				{
					num = global::Rewired.ReInput.controllers.Mouse.ButtonElementIdentifiers[i].id;
					break;
				}
			}
			for (int j = 0; j < global::Rewired.ReInput.controllers.Mouse.ButtonElementIdentifiers.Count; j++)
			{
				if (global::Rewired.ReInput.controllers.Mouse.ButtonElementIdentifiers[j].name.Equals("Right Mouse Button"))
				{
					num2 = global::Rewired.ReInput.controllers.Mouse.ButtonElementIdentifiers[j].id;
					break;
				}
			}
			foreach (global::Rewired.ControllerMap controllerMap in this.player.controllers.maps.GetAllMaps(global::Rewired.ControllerType.Mouse))
			{
				global::Rewired.ActionElementMap[] buttonMaps = controllerMap.GetButtonMaps();
				for (int k = 0; k < buttonMaps.Length; k++)
				{
					if (includeUserAssignables || !global::Rewired.ReInput.mapping.GetAction(buttonMaps[k].actionId).userAssignable)
					{
						if (buttonMaps[k].elementIdentifierId == num)
						{
							global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Mouse, global::Rewired.ControllerElementType.Button, num2, global::Rewired.AxisRange.Full, global::UnityEngine.KeyCode.None, global::Rewired.ModifierKeyFlags.None, buttonMaps[k].actionId, buttonMaps[k].axisContribution, buttonMaps[k].invert, buttonMaps[k].id);
							controllerMap.ReplaceElementMap(elementAssignment);
						}
						else if (buttonMaps[k].elementIdentifierId == num2)
						{
							global::Rewired.ElementAssignment elementAssignment2 = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Mouse, global::Rewired.ControllerElementType.Button, num, global::Rewired.AxisRange.Full, global::UnityEngine.KeyCode.None, global::Rewired.ModifierKeyFlags.None, buttonMaps[k].actionId, buttonMaps[k].axisContribution, buttonMaps[k].invert, buttonMaps[k].id);
							controllerMap.ReplaceElementMap(elementAssignment2);
						}
					}
				}
			}
		}
	}

	public void SetLeftHandedController(bool leftHanded = true, bool includeUserAssignables = false)
	{
		if (this.player.controllers.Joysticks.Count > 0 && leftHanded != this.leftHandedController)
		{
			this.leftHandedController = leftHanded;
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			for (int i = 0; i < global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers.Count; i++)
			{
				if (global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].name.Equals("Left Stick X"))
				{
					num = global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].id;
				}
				else if (global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].name.Equals("Left Stick Y"))
				{
					num2 = global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].id;
				}
				else if (global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].name.Equals("Right Stick X"))
				{
					num3 = global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].id;
				}
				else if (global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].name.Equals("Right Stick Y"))
				{
					num4 = global::Rewired.ReInput.controllers.Joysticks[0].AxisElementIdentifiers[i].id;
				}
			}
			foreach (global::Rewired.ControllerMap controllerMap in this.player.controllers.maps.GetAllMaps(global::Rewired.ControllerType.Joystick))
			{
				global::Rewired.ActionElementMap[] elementMaps = controllerMap.GetElementMaps();
				for (int j = 0; j < elementMaps.Length; j++)
				{
					if (includeUserAssignables || !global::Rewired.ReInput.mapping.GetAction(elementMaps[j].actionId).userAssignable)
					{
						if (elementMaps[j].elementIdentifierId == num)
						{
							global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Joystick, global::Rewired.ControllerElementType.Axis, num3, global::Rewired.AxisRange.Full, global::UnityEngine.KeyCode.None, global::Rewired.ModifierKeyFlags.None, elementMaps[j].actionId, elementMaps[j].axisContribution, elementMaps[j].invert, elementMaps[j].id);
							controllerMap.ReplaceElementMap(elementAssignment);
						}
						else if (elementMaps[j].elementIdentifierId == num2)
						{
							global::Rewired.ElementAssignment elementAssignment2 = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Joystick, global::Rewired.ControllerElementType.Axis, num4, global::Rewired.AxisRange.Full, global::UnityEngine.KeyCode.None, global::Rewired.ModifierKeyFlags.None, elementMaps[j].actionId, elementMaps[j].axisContribution, elementMaps[j].invert, elementMaps[j].id);
							controllerMap.ReplaceElementMap(elementAssignment2);
						}
						else if (elementMaps[j].elementIdentifierId == num3)
						{
							global::Rewired.ElementAssignment elementAssignment3 = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Joystick, global::Rewired.ControllerElementType.Axis, num, global::Rewired.AxisRange.Full, global::UnityEngine.KeyCode.None, global::Rewired.ModifierKeyFlags.None, elementMaps[j].actionId, elementMaps[j].axisContribution, elementMaps[j].invert, elementMaps[j].id);
							controllerMap.ReplaceElementMap(elementAssignment3);
						}
						else if (elementMaps[j].elementIdentifierId == num4)
						{
							global::Rewired.ElementAssignment elementAssignment4 = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Joystick, global::Rewired.ControllerElementType.Axis, num2, global::Rewired.AxisRange.Full, global::UnityEngine.KeyCode.None, global::Rewired.ModifierKeyFlags.None, elementMaps[j].actionId, elementMaps[j].axisContribution, elementMaps[j].invert, elementMaps[j].id);
							controllerMap.ReplaceElementMap(elementAssignment4);
						}
					}
				}
			}
		}
	}

	public void SetMouseSensitivity(float sensitivity)
	{
		global::System.Collections.Generic.IList<global::Rewired.InputBehavior> inputBehaviors = this.player.controllers.maps.InputBehaviors;
		inputBehaviors[1].mouseXYAxisSensitivity = sensitivity;
	}

	public void SetJoystickSensitivity(float sensitivity)
	{
		global::System.Collections.Generic.IList<global::Rewired.InputBehavior> inputBehaviors = this.player.controllers.maps.InputBehaviors;
		inputBehaviors[1].joystickAxisSensitivity = sensitivity * 5f;
	}

	public void MapKeyboardKey(string inputCategory, global::UnityEngine.KeyCode key, int actionId, bool isPositive = true)
	{
		this.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Keyboard, 0, inputCategory).CreateElementMap(actionId, (!isPositive) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive, key, global::Rewired.ModifierKeyFlags.None);
	}

	public global::Rewired.ActionElementMap GetFirstConflictingActionMap(int actionId, string inputCategory, global::Rewired.ControllerType controller, int keyIdentifier)
	{
		global::Rewired.ControllerMap firstMapInCategory = this.player.controllers.maps.GetFirstMapInCategory(controller, 0, inputCategory);
		foreach (global::Rewired.ActionElementMap actionElementMap in firstMapInCategory.GetElementMaps())
		{
			if (global::Rewired.ReInput.mapping.GetAction(actionElementMap.actionId).userAssignable)
			{
				int num = (controller != global::Rewired.ControllerType.Keyboard) ? actionElementMap.elementIdentifierId : ((int)actionElementMap.keyCode);
				if (num == keyIdentifier)
				{
					return actionElementMap;
				}
			}
		}
		return null;
	}

	public void PushInputLayer(global::PandoraInput.InputLayer layer)
	{
		this.inputLayers.Add(layer);
		this.CurrentInputLayer = (int)layer;
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"PUSH InputLayers.Count = ",
			this.inputLayers.Count,
			" current = ",
			(global::PandoraInput.InputLayer)this.CurrentInputLayer
		}), "PandoraInput", null);
	}

	public void PopInputLayer(global::PandoraInput.InputLayer layer)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Input Layer to be POPPED! = ",
			this.inputLayers.Count,
			" current = ",
			(global::PandoraInput.InputLayer)this.CurrentInputLayer,
			" To Be Popped ",
			layer
		}), "PandoraInput", null);
		this.popLayers.Add(layer);
	}

	public void ClearInputLayer()
	{
		this.popLayers.Clear();
		this.inputLayers.Clear();
		this.PushInputLayer(global::PandoraInput.InputLayer.NORMAL);
	}

	public global::PandoraInput.States GetCurrentState()
	{
		return this.currentStateId;
	}

	public void SetCurrentState(global::PandoraInput.States state, bool showMouse)
	{
		this.showCursor = showMouse;
		if (state < global::PandoraInput.States.NB_STATE)
		{
			this.currentStateId = state;
			this.IsActive = true;
		}
		else
		{
			this.IsActive = false;
		}
		if (this.showCursor)
		{
			global::UnityEngine.Cursor.lockState = global::UnityEngine.CursorLockMode.None;
		}
		else
		{
			global::UnityEngine.Cursor.lockState = global::UnityEngine.CursorLockMode.Locked;
		}
		global::UnityEngine.Cursor.visible = this.showCursor;
		foreach (global::Rewired.ControllerMap controllerMap in this.player.controllers.maps.GetAllMaps())
		{
			controllerMap.enabled = (controllerMap.categoryId == (int)state);
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			if (this.showCursor)
			{
				global::UnityEngine.Cursor.lockState = global::UnityEngine.CursorLockMode.None;
			}
			else
			{
				global::UnityEngine.Cursor.lockState = global::UnityEngine.CursorLockMode.Locked;
			}
			global::UnityEngine.Cursor.visible = this.showCursor;
		}
	}

	public void SetActive(bool active)
	{
		this.IsActive = active;
	}

	public float GetAxis(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			return this.player.GetAxis(name);
		}
		return 0f;
	}

	public float GetAxisRaw(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			return this.player.GetAxisRaw(name);
		}
		return 0f;
	}

	public bool GetKeyDown(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			bool flag = this.player.GetButtonDown(name);
			if (flag)
			{
				this.keyRepeatData[name] = this.firstRepeatDelay;
			}
			else if (this.keyRepeatData.ContainsKey(name))
			{
				float buttonTimePressed = this.player.GetButtonTimePressed(name);
				if (buttonTimePressed > this.keyRepeatData[name])
				{
					flag = true;
					this.keyRepeatData[name] = buttonTimePressed + this.subsequentRepeatRate - global::UnityEngine.Mathf.Repeat(buttonTimePressed - this.keyRepeatData[name], this.subsequentRepeatRate);
				}
			}
			return flag;
		}
		return false;
	}

	public bool GetNegKeyDown(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			bool flag = this.player.GetNegativeButtonDown(name);
			string key = "neg" + name;
			if (flag)
			{
				this.keyRepeatData[key] = this.firstRepeatDelay;
			}
			else if (this.keyRepeatData.ContainsKey(key))
			{
				float negativeButtonTimePressed = this.player.GetNegativeButtonTimePressed(name);
				if (negativeButtonTimePressed > this.keyRepeatData[key])
				{
					flag = true;
					this.keyRepeatData[key] = negativeButtonTimePressed + this.subsequentRepeatRate - global::UnityEngine.Mathf.Repeat(negativeButtonTimePressed - this.keyRepeatData[key], this.subsequentRepeatRate);
				}
			}
			return flag;
		}
		return false;
	}

	public bool GetKeyUp(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			return this.player.GetButtonUp(name);
		}
		return false;
	}

	public bool GetNegKeyUp(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			return this.player.GetNegativeButtonUp(name);
		}
		return false;
	}

	public bool GetKey(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			return this.player.GetButton(name);
		}
		return false;
	}

	public bool GetNegKey(string name, int layer = 0)
	{
		if (this.IsActive && (this.CurrentInputLayer == layer || layer == -1))
		{
			this.UpdateLastInputType();
			return this.player.GetNegativeButton(name);
		}
		return false;
	}

	private void UpdateLastInputType()
	{
		global::Rewired.Controller controller = null;
		if (this.player != null)
		{
			controller = this.player.controllers.GetLastActiveController();
		}
		if (controller != null)
		{
			switch (controller.type)
			{
			case global::Rewired.ControllerType.Keyboard:
				this.SetLastInputMode(global::PandoraInput.InputMode.KEYBOARD);
				break;
			case global::Rewired.ControllerType.Mouse:
				this.SetLastInputMode(global::PandoraInput.InputMode.MOUSE);
				break;
			case global::Rewired.ControllerType.Joystick:
				this.SetLastInputMode(global::PandoraInput.InputMode.JOYSTICK);
				break;
			}
		}
	}

	public string GetInputString()
	{
		return global::UnityEngine.Input.inputString;
	}

	public global::UnityEngine.Vector3 GetMousePosition()
	{
		return global::UnityEngine.Input.mousePosition;
	}

	public void LockPlayerPollKey(string actionName)
	{
	}

	private void LateUpdate()
	{
		if (!this.initialized)
		{
			return;
		}
		if (global::Rewired.ReInput.controllers.Mouse.isConnected)
		{
			bool flag = true;
			for (int i = 0; i < global::Rewired.ReInput.controllers.Mouse.axisCount; i++)
			{
				if (global::Rewired.ReInput.controllers.Mouse.GetAxisTimeInactive(i) < this.mousePointerInactiveHideDelay)
				{
					flag = false;
				}
			}
			if (flag)
			{
				if (global::UnityEngine.Cursor.visible)
				{
					global::UnityEngine.Cursor.visible = false;
				}
			}
			else if (this.showCursor)
			{
				global::UnityEngine.Cursor.visible = true;
			}
		}
		if (this.popLayers.Count > 0)
		{
			for (int j = 0; j < this.popLayers.Count; j++)
			{
				for (int k = this.inputLayers.Count - 1; k >= 0; k--)
				{
					if (this.inputLayers[k] == this.popLayers[j])
					{
						this.inputLayers.RemoveAt(k);
						break;
					}
				}
				this.CurrentInputLayer = (int)this.inputLayers[this.inputLayers.Count - 1];
			}
			this.popLayers.Clear();
		}
	}

	public float firstRepeatDelay = 0.4f;

	public float subsequentRepeatRate = 0.4f;

	public float mousePointerInactiveHideDelay = 5f;

	private global::System.Collections.Generic.List<global::PandoraInput.InputLayer> popLayers;

	private global::System.Collections.Generic.List<global::PandoraInput.InputLayer> inputLayers;

	private global::PandoraInput.States currentStateId;

	private bool showCursor = true;

	private float lastMouseInputTime;

	public bool leftHandedMouse;

	public bool leftHandedController;

	private global::System.Collections.Generic.Dictionary<string, float> keyRepeatData;

	public bool initialized;

	public enum InputLayer
	{
		NOTHING = -9999,
		NORMAL = 0,
		FLY_BY_CAM = 200,
		TRANSITION = 1000,
		POP_UP = 1,
		CHAT,
		END_GAME,
		LOOTING,
		WHEEL,
		MENU,
		LOG
	}

	public enum States
	{
		MENU,
		MISSION,
		NB_STATE,
		NONE = 2
	}

	public enum InputMode
	{
		NONE,
		KEYBOARD,
		MOUSE,
		JOYSTICK
	}
}
