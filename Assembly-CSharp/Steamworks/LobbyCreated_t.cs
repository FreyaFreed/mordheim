using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(513)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LobbyCreated_t
	{
		public const int k_iCallback = 513;

		public global::Steamworks.EResult m_eResult;

		public ulong m_ulSteamIDLobby;
	}
}
