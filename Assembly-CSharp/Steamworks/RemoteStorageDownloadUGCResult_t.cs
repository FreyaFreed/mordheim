using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1317)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDownloadUGCResult_t
	{
		public const int k_iCallback = 1317;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.UGCHandle_t m_hFile;

		public global::Steamworks.AppId_t m_nAppID;

		public int m_nSizeInBytes;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		public ulong m_ulSteamIDOwner;
	}
}
