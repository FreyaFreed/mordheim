using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(3401)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCQueryCompleted_t
	{
		public const int k_iCallback = 3401;

		public global::Steamworks.UGCQueryHandle_t m_handle;

		public global::Steamworks.EResult m_eResult;

		public uint m_unNumResultsReturned;

		public uint m_unTotalMatchingResults;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
