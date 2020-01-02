using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(502)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListChanged_t
	{
		public const int k_iCallback = 502;

		public uint m_nIP;

		public uint m_nQueryPort;

		public uint m_nConnPort;

		public uint m_nAppID;

		public uint m_nFlags;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bAdd;

		public global::Steamworks.AccountID_t m_unAccountId;
	}
}
