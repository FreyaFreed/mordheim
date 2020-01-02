using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4521)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NewWindow_t
	{
		public const int k_iCallback = 4521;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public string pchURL;

		public uint unX;

		public uint unY;

		public uint unWide;

		public uint unTall;

		public global::Steamworks.HHTMLBrowser unNewWindow_BrowserHandle;
	}
}
