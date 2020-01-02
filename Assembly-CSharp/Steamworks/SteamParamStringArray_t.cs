using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamParamStringArray_t
	{
		public global::System.IntPtr m_ppStrings;

		public int m_nNumStrings;
	}
}
