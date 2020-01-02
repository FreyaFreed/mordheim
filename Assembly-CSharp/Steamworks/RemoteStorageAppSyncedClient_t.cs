using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1301)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedClient_t
	{
		public const int k_iCallback = 1301;

		public global::Steamworks.AppId_t m_nAppID;

		public global::Steamworks.EResult m_eResult;

		public int m_unNumDownloads;
	}
}
