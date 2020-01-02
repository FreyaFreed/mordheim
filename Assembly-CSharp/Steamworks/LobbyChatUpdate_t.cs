using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(506)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatUpdate_t
	{
		public const int k_iCallback = 506;

		public ulong m_ulSteamIDLobby;

		public ulong m_ulSteamIDUserChanged;

		public ulong m_ulSteamIDMakingChange;

		public uint m_rgfChatMemberStateChange;
	}
}
