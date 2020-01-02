using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(342)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct JoinClanChatRoomCompletionResult_t
	{
		public const int k_iCallback = 342;

		public global::Steamworks.CSteamID m_steamIDClanChat;

		public global::Steamworks.EChatRoomEnterResponse m_eChatRoomEnterResponse;
	}
}
