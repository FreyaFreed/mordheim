using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(345)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct FriendsIsFollowing_t
	{
		public const int k_iCallback = 345;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.CSteamID m_steamID;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bIsFollowing;
	}
}
