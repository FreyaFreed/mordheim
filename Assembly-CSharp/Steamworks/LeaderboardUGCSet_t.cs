using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1111)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardUGCSet_t
	{
		public const int k_iCallback = 1111;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.SteamLeaderboard_t m_hSteamLeaderboard;
	}
}
