using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1106)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoreUploaded_t
	{
		public const int k_iCallback = 1106;

		public byte m_bSuccess;

		public global::Steamworks.SteamLeaderboard_t m_hSteamLeaderboard;

		public int m_nScore;

		public byte m_bScoreChanged;

		public int m_nGlobalRankNew;

		public int m_nGlobalRankPrevious;
	}
}
