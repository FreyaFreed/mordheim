using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1801)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsStored_t
	{
		public const int k_iCallback = 1801;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.CSteamID m_steamIDUser;
	}
}
