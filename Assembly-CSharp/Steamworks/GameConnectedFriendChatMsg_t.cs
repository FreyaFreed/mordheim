using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(343)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedFriendChatMsg_t
	{
		public const int k_iCallback = 343;

		public global::Steamworks.CSteamID m_steamIDUser;

		public int m_iMessageID;
	}
}
