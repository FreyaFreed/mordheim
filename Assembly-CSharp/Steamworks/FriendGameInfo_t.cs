using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct FriendGameInfo_t
	{
		public global::Steamworks.CGameID m_gameID;

		public uint m_unGameIP;

		public ushort m_usGamePort;

		public ushort m_usQueryPort;

		public global::Steamworks.CSteamID m_steamIDLobby;
	}
}
