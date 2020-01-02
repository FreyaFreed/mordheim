using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(503)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LobbyInvite_t
	{
		public const int k_iCallback = 503;

		public ulong m_ulSteamIDUser;

		public ulong m_ulSteamIDLobby;

		public ulong m_ulGameID;
	}
}
