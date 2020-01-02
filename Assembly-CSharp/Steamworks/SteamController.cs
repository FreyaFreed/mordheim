using System;

namespace Steamworks
{
	public static class SteamController
	{
		public static bool Init()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_Init();
		}

		public static bool Shutdown()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_Shutdown();
		}

		public static void RunFrame()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamController_RunFrame();
		}

		public static int GetConnectedControllers(global::Steamworks.ControllerHandle_t[] handlesOut)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_GetConnectedControllers(handlesOut);
		}

		public static bool ShowBindingPanel(global::Steamworks.ControllerHandle_t controllerHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_ShowBindingPanel(controllerHandle);
		}

		public static global::Steamworks.ControllerActionSetHandle_t GetActionSetHandle(string pszActionSetName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.ControllerActionSetHandle_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszActionSetName))
			{
				result = (global::Steamworks.ControllerActionSetHandle_t)global::Steamworks.NativeMethods.ISteamController_GetActionSetHandle(utf8StringHandle);
			}
			return result;
		}

		public static void ActivateActionSet(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerActionSetHandle_t actionSetHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamController_ActivateActionSet(controllerHandle, actionSetHandle);
		}

		public static global::Steamworks.ControllerActionSetHandle_t GetCurrentActionSet(global::Steamworks.ControllerHandle_t controllerHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.ControllerActionSetHandle_t)global::Steamworks.NativeMethods.ISteamController_GetCurrentActionSet(controllerHandle);
		}

		public static global::Steamworks.ControllerDigitalActionHandle_t GetDigitalActionHandle(string pszActionName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.ControllerDigitalActionHandle_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszActionName))
			{
				result = (global::Steamworks.ControllerDigitalActionHandle_t)global::Steamworks.NativeMethods.ISteamController_GetDigitalActionHandle(utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.ControllerDigitalActionData_t GetDigitalActionData(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerDigitalActionHandle_t digitalActionHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_GetDigitalActionData(controllerHandle, digitalActionHandle);
		}

		public static int GetDigitalActionOrigins(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerActionSetHandle_t actionSetHandle, global::Steamworks.ControllerDigitalActionHandle_t digitalActionHandle, global::Steamworks.EControllerActionOrigin[] originsOut)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_GetDigitalActionOrigins(controllerHandle, actionSetHandle, digitalActionHandle, originsOut);
		}

		public static global::Steamworks.ControllerAnalogActionHandle_t GetAnalogActionHandle(string pszActionName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.ControllerAnalogActionHandle_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszActionName))
			{
				result = (global::Steamworks.ControllerAnalogActionHandle_t)global::Steamworks.NativeMethods.ISteamController_GetAnalogActionHandle(utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.ControllerAnalogActionData_t GetAnalogActionData(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerAnalogActionHandle_t analogActionHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_GetAnalogActionData(controllerHandle, analogActionHandle);
		}

		public static int GetAnalogActionOrigins(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerActionSetHandle_t actionSetHandle, global::Steamworks.ControllerAnalogActionHandle_t analogActionHandle, global::Steamworks.EControllerActionOrigin[] originsOut)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamController_GetAnalogActionOrigins(controllerHandle, actionSetHandle, analogActionHandle, originsOut);
		}

		public static void StopAnalogActionMomentum(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerAnalogActionHandle_t eAction)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamController_StopAnalogActionMomentum(controllerHandle, eAction);
		}

		public static void TriggerHapticPulse(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamController_TriggerHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec);
		}

		public static void TriggerRepeatedHapticPulse(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamController_TriggerRepeatedHapticPulse(controllerHandle, eTargetPad, usDurationMicroSec, usOffMicroSec, unRepeat, nFlags);
		}
	}
}
