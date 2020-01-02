using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4501)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_BrowserReady_t
	{
		public const int k_iCallback = 4501;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;
	}
}
