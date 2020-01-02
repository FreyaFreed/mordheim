using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1104)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardFindResult_t
	{
		public const int k_iCallback = 1104;

		public global::Steamworks.SteamLeaderboard_t m_hSteamLeaderboard;

		public byte m_bLeaderboardFound;
	}
}
