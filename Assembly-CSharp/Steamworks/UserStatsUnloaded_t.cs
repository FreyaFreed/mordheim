using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1108)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsUnloaded_t
	{
		public const int k_iCallback = 1108;

		public global::Steamworks.CSteamID m_steamIDUser;
	}
}
