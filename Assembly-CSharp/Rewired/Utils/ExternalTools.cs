using System;
using System.ComponentModel;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils
{
	[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
	public class ExternalTools : global::Rewired.Utils.Interfaces.IExternalTools
	{
		public event global::System.Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0U;
		}

		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}
	}
}
