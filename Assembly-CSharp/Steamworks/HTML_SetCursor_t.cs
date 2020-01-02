using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4522)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SetCursor_t
	{
		public const int k_iCallback = 4522;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public uint eMouseCursor;
	}
}
