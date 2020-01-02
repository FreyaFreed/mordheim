using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1202)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionRequest_t
	{
		public const int k_iCallback = 1202;

		public global::Steamworks.CSteamID m_steamIDRemote;
	}
}
