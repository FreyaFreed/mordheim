using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamGameServerUtils
	{
		public static uint GetSecondsSinceAppActive()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetSecondsSinceAppActive();
		}

		public static uint GetSecondsSinceComputerActive()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetSecondsSinceComputerActive();
		}

		public static global::Steamworks.EUniverse GetConnectedUniverse()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetConnectedUniverse();
		}

		public static uint GetServerRealTime()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetServerRealTime();
		}

		public static string GetIPCountry()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamGameServerUtils_GetIPCountry());
		}

		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetCSERIPPort(out unIP, out usPort);
		}

		public static byte GetCurrentBatteryPower()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetCurrentBatteryPower();
		}

		public static global::Steamworks.AppId_t GetAppID()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return (global::Steamworks.AppId_t)global::Steamworks.NativeMethods.ISteamGameServerUtils_GetAppID();
		}

		public static void SetOverlayNotificationPosition(global::Steamworks.ENotificationPosition eNotificationPosition)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServerUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		public static bool IsAPICallCompleted(global::Steamworks.SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		public static global::Steamworks.ESteamAPICallFailure GetAPICallFailureReason(global::Steamworks.SteamAPICall_t hSteamAPICall)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		public static bool GetAPICallResult(global::Steamworks.SteamAPICall_t hSteamAPICall, global::System.IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		public static uint GetIPCCallCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetIPCCallCount();
		}

		public static void SetWarningMessageHook(global::Steamworks.SteamAPIWarningMessageHook_t pFunction)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServerUtils_SetWarningMessageHook(pFunction);
		}

		public static bool IsOverlayEnabled()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_IsOverlayEnabled();
		}

		public static bool BOverlayNeedsPresent()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_BOverlayNeedsPresent();
		}

		public static global::Steamworks.SteamAPICall_t CheckFileSignature(string szFileName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(szFileName))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamGameServerUtils_CheckFileSignature(utf8StringHandle);
			}
			return result;
		}

		public static bool ShowGamepadTextInput(global::Steamworks.EGamepadTextInputMode eInputMode, global::Steamworks.EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchExistingText))
				{
					result = global::Steamworks.NativeMethods.ISteamGameServerUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return result;
		}

		public static uint GetEnteredGamepadTextLength()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextLength();
		}

		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchText);
			bool flag = global::Steamworks.NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static string GetSteamUILanguage()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamGameServerUtils_GetSteamUILanguage());
		}

		public static bool IsSteamRunningInVR()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerUtils_IsSteamRunningInVR();
		}

		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServerUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}
	}
}
