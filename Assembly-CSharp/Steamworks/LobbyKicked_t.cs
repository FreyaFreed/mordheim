using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(512)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LobbyKicked_t
	{
		public const int k_iCallback = 512;

		public ulong m_ulSteamIDLobby;

		public ulong m_ulSteamIDAdmin;

		public byte m_bKickedDueToDisconnect;
	}
}
