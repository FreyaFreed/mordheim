using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1101)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		public const int k_iCallback = 1101;

		[global::System.Runtime.InteropServices.FieldOffset(0)]
		public ulong m_nGameID;

		[global::System.Runtime.InteropServices.FieldOffset(8)]
		public global::Steamworks.EResult m_eResult;

		[global::System.Runtime.InteropServices.FieldOffset(12)]
		public global::Steamworks.CSteamID m_steamIDUser;
	}
}
