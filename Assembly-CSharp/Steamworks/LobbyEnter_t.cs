using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(504)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LobbyEnter_t
	{
		public const int k_iCallback = 504;

		public ulong m_ulSteamIDLobby;

		public uint m_rgfChatPermissions;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bLocked;

		public uint m_EChatRoomEnterResponse;
	}
}
