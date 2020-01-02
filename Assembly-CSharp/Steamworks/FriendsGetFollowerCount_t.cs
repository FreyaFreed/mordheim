using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(344)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct FriendsGetFollowerCount_t
	{
		public const int k_iCallback = 344;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.CSteamID m_steamID;

		public int m_nCount;
	}
}
