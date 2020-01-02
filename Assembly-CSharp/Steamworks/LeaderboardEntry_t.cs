using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardEntry_t
	{
		public global::Steamworks.CSteamID m_steamIDUser;

		public int m_nGlobalRank;

		public int m_nScore;

		public int m_cDetails;

		public global::Steamworks.UGCHandle_t m_hUGC;
	}
}
