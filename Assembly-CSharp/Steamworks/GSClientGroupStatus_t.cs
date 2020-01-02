using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(208)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
	public struct GSClientGroupStatus_t
	{
		public const int k_iCallback = 208;

		public global::Steamworks.CSteamID m_SteamIDUser;

		public global::Steamworks.CSteamID m_SteamIDGroup;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bMember;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bOfficer;
	}
}
