using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(3902)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppUninstalled_t
	{
		public const int k_iCallback = 3902;

		public global::Steamworks.AppId_t m_nAppID;
	}
}
