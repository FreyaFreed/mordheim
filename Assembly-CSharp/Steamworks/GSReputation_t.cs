using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(209)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GSReputation_t
	{
		public const int k_iCallback = 209;

		public global::Steamworks.EResult m_eResult;

		public uint m_unReputationScore;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bBanned;

		public uint m_unBannedIP;

		public ushort m_usBannedPort;

		public ulong m_ulBannedGameID;

		public uint m_unBanExpires;
	}
}
