using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1332)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileReadAsyncComplete_t
	{
		public const int k_iCallback = 1332;

		public global::Steamworks.SteamAPICall_t m_hFileReadAsync;

		public global::Steamworks.EResult m_eResult;

		public uint m_nOffset;

		public uint m_cubRead;
	}
}
