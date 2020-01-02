using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1203)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
	public struct P2PSessionConnectFail_t
	{
		public const int k_iCallback = 1203;

		public global::Steamworks.CSteamID m_steamIDRemote;

		public byte m_eP2PSessionError;
	}
}
