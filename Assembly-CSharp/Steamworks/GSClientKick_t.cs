using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(203)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct GSClientKick_t
	{
		public const int k_iCallback = 203;

		public global::Steamworks.CSteamID m_SteamID;

		public global::Steamworks.EDenyReason m_eDenyReason;
	}
}
