using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4505)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_URLChanged_t
	{
		public const int k_iCallback = 4505;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public string pchURL;

		public string pchPostData;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool bIsRedirect;

		public string pchPageTitle;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool bNewNavigation;
	}
}
