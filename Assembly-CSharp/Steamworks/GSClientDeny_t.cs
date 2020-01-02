using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(202)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct GSClientDeny_t
	{
		public const int k_iCallback = 202;

		public global::Steamworks.CSteamID m_SteamID;

		public global::Steamworks.EDenyReason m_eDenyReason;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchOptionalText;
	}
}
