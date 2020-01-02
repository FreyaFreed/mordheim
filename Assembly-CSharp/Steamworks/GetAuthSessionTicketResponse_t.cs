using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(163)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GetAuthSessionTicketResponse_t
	{
		public const int k_iCallback = 163;

		public global::Steamworks.HAuthTicket m_hAuthTicket;

		public global::Steamworks.EResult m_eResult;
	}
}
