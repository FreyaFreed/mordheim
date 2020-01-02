using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(346)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct FriendsEnumerateFollowingList_t
	{
		public const int k_iCallback = 346;

		public global::Steamworks.EResult m_eResult;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 50)]
		public global::Steamworks.CSteamID[] m_rgSteamID;

		public int m_nResultsReturned;

		public int m_nTotalResultCount;
	}
}
