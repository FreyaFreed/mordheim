using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4507)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_OpenLinkInNewTab_t
	{
		public const int k_iCallback = 4507;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public string pchURL;
	}
}
