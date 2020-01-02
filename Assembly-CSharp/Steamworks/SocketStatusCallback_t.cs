using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1201)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct SocketStatusCallback_t
	{
		public const int k_iCallback = 1201;

		public global::Steamworks.SNetSocket_t m_hSocket;

		public global::Steamworks.SNetListenSocket_t m_hListenSocket;

		public global::Steamworks.CSteamID m_steamIDRemote;

		public int m_eSNetSocketState;
	}
}
