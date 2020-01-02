using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1109)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementIconFetched_t
	{
		public const int k_iCallback = 1109;

		public global::Steamworks.CGameID m_nGameID;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bAchieved;

		public int m_nIconHandle;
	}
}
