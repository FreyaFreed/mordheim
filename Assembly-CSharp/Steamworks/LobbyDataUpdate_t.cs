using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(505)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LobbyDataUpdate_t
	{
		public const int k_iCallback = 505;

		public ulong m_ulSteamIDLobby;

		public ulong m_ulSteamIDMember;

		public byte m_bSuccess;
	}
}
