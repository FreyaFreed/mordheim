using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4511)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HorizontalScroll_t
	{
		public const int k_iCallback = 4511;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public uint unScrollMax;

		public uint unScrollCurrent;

		public float flPageScale;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool bVisible;

		public uint unPageSize;
	}
}
