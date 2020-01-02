using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct CallbackMsg_t
	{
		public int m_hSteamUser;

		public int m_iCallback;

		public global::System.IntPtr m_pubParam;

		public int m_cubParam;
	}
}
