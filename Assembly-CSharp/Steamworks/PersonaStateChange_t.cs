using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(304)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct PersonaStateChange_t
	{
		public const int k_iCallback = 304;

		public ulong m_ulSteamID;

		public global::Steamworks.EPersonaChange m_nChangeFlags;
	}
}
