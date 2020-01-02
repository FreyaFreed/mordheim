using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(3901)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppInstalled_t
	{
		public const int k_iCallback = 3901;

		public global::Steamworks.AppId_t m_nAppID;
	}
}
