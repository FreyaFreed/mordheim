using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(333)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GameLobbyJoinRequested_t
	{
		public const int k_iCallback = 333;

		public global::Steamworks.CSteamID m_steamIDLobby;

		public global::Steamworks.CSteamID m_steamIDFriend;
	}
}
