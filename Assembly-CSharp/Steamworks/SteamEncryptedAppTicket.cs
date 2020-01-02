using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamEncryptedAppTicket
	{
		public static bool BDecryptTicket(byte[] rgubTicketEncrypted, uint cubTicketEncrypted, byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, byte[] rgubKey, int cubKey)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.BDecryptTicket(rgubTicketEncrypted, cubTicketEncrypted, rgubTicketDecrypted, ref pcubTicketDecrypted, rgubKey, cubKey);
		}

		public static bool BIsTicketForApp(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.BIsTicketForApp(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
		}

		public static uint GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.GetTicketIssueTime(rgubTicketDecrypted, cubTicketDecrypted);
		}

		public static void GetTicketSteamID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out global::Steamworks.CSteamID psteamID)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::Steamworks.NativeMethods.GetTicketSteamID(rgubTicketDecrypted, cubTicketDecrypted, out psteamID);
		}

		public static uint GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.GetTicketAppID(rgubTicketDecrypted, cubTicketDecrypted);
		}

		public static bool BUserOwnsAppInTicket(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, global::Steamworks.AppId_t nAppID)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.BUserOwnsAppInTicket(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
		}

		public static bool BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			return global::Steamworks.NativeMethods.BUserIsVacBanned(rgubTicketDecrypted, cubTicketDecrypted);
		}

		public static byte[] GetUserVariableData(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData)
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			global::System.IntPtr userVariableData = global::Steamworks.NativeMethods.GetUserVariableData(rgubTicketDecrypted, cubTicketDecrypted, out pcubUserData);
			byte[] array = new byte[pcubUserData];
			global::System.Runtime.InteropServices.Marshal.Copy(userVariableData, array, 0, (int)pcubUserData);
			return array;
		}
	}
}
