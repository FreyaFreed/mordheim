﻿using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4508)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ChangedTitle_t
	{
		public const int k_iCallback = 4508;

		public global::Steamworks.HHTMLBrowser unBrowserHandle;

		public string pchTitle;
	}
}
