using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(339)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GameConnectedChatJoin_t
	{
		public const int k_iCallback = 339;

		public global::Steamworks.CSteamID m_steamIDClanChat;

		public global::Steamworks.CSteamID m_steamIDUser;
	}
}
