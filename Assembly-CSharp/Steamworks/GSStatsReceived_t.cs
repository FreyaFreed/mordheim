using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1800)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsReceived_t
	{
		public const int k_iCallback = 1800;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.CSteamID m_steamIDUser;
	}
}
