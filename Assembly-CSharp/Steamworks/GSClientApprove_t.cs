using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(201)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GSClientApprove_t
	{
		public const int k_iCallback = 201;

		public global::Steamworks.CSteamID m_SteamID;

		public global::Steamworks.CSteamID m_OwnerSteamID;
	}
}
