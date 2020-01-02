using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1112)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GlobalStatsReceived_t
	{
		public const int k_iCallback = 1112;

		public ulong m_nGameID;

		public global::Steamworks.EResult m_eResult;
	}
}
