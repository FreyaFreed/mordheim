using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(143)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct ValidateAuthTicketResponse_t
	{
		public const int k_iCallback = 143;

		public global::Steamworks.CSteamID m_SteamID;

		public global::Steamworks.EAuthSessionResponse m_eAuthSessionResponse;

		public global::Steamworks.CSteamID m_OwnerSteamID;
	}
}
