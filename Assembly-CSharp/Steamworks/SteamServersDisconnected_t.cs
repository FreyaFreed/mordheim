using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(103)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamServersDisconnected_t
	{
		public const int k_iCallback = 103;

		public global::Steamworks.EResult m_eResult;
	}
}
