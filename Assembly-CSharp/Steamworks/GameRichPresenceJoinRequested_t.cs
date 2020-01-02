using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(337)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GameRichPresenceJoinRequested_t
	{
		public const int k_iCallback = 337;

		public global::Steamworks.CSteamID m_steamIDFriend;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchConnect;
	}
}
