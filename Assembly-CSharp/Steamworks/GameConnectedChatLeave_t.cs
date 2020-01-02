using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(340)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
	public struct GameConnectedChatLeave_t
	{
		public const int k_iCallback = 340;

		public global::Steamworks.CSteamID m_steamIDClanChat;

		public global::Steamworks.CSteamID m_steamIDUser;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bKicked;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bDropped;
	}
}
