using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4611)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GetVideoURLResult_t
	{
		public const int k_iCallback = 4611;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.AppId_t m_unVideoAppID;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;
	}
}
