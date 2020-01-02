using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(335)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct ClanOfficerListResponse_t
	{
		public const int k_iCallback = 335;

		public global::Steamworks.CSteamID m_steamIDClan;

		public int m_cOfficers;

		public byte m_bSuccess;
	}
}
