using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(703)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamAPICallCompleted_t
	{
		public const int k_iCallback = 703;

		public global::Steamworks.SteamAPICall_t m_hAsyncCall;
	}
}
