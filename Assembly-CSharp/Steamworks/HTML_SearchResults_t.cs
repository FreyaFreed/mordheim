using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4509)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SearchResults_t
	{
		public const int k_iCallback = 4509;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public uint unResults;

		public uint unCurrentMatch;
	}
}
