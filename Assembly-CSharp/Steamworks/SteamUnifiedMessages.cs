using System;

namespace Steamworks
{
	public static class SteamUnifiedMessages
	{
		public static global::Steamworks.ClientUnifiedMessageHandle SendMethod(string pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.ClientUnifiedMessageHandle result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchServiceMethod))
			{
				result = (global::Steamworks.ClientUnifiedMessageHandle)global::Steamworks.NativeMethods.ISteamUnifiedMessages_SendMethod(utf8StringHandle, pRequestBuffer, unRequestBufferSize, unContext);
			}
			return result;
		}

		public static bool GetMethodResponseInfo(global::Steamworks.ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out global::Steamworks.EResult peResult)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
		}

		public static bool GetMethodResponseData(global::Steamworks.ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
		}

		public static bool ReleaseMethod(global::Steamworks.ClientUnifiedMessageHandle hHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
		}

		public static bool SendNotification(string pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchServiceNotification))
			{
				result = global::Steamworks.NativeMethods.ISteamUnifiedMessages_SendNotification(utf8StringHandle, pNotificationBuffer, unNotificationBufferSize);
			}
			return result;
		}
	}
}
