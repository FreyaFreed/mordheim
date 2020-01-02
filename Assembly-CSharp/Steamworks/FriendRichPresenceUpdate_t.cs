using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(336)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct FriendRichPresenceUpdate_t
	{
		public const int k_iCallback = 336;

		public global::Steamworks.CSteamID m_steamIDFriend;

		public global::Steamworks.AppId_t m_nAppID;
	}
}
