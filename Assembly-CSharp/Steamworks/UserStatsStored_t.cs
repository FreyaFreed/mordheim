using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1102)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsStored_t
	{
		public const int k_iCallback = 1102;

		public ulong m_nGameID;

		public global::Steamworks.EResult m_eResult;
	}
}
