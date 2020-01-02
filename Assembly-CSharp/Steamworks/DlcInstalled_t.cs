using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1005)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct DlcInstalled_t
	{
		public const int k_iCallback = 1005;

		public global::Steamworks.AppId_t m_nAppID;
	}
}
