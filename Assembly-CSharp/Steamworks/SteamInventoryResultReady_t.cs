using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4700)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryResultReady_t
	{
		public const int k_iCallback = 4700;

		public global::Steamworks.SteamInventoryResult_t m_handle;

		public global::Steamworks.EResult m_result;
	}
}
