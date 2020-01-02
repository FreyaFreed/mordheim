using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(2301)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct ScreenshotReady_t
	{
		public const int k_iCallback = 2301;

		public global::Steamworks.ScreenshotHandle m_hLocal;

		public global::Steamworks.EResult m_eResult;
	}
}
