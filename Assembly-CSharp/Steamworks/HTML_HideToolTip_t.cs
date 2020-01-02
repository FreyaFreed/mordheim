using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4526)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HideToolTip_t
	{
		public const int k_iCallback = 4526;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;
	}
}
