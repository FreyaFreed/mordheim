using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(2102)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestHeadersReceived_t
	{
		public const int k_iCallback = 2102;

		public global::Steamworks.HTTPRequestHandle m_hRequest;

		public ulong m_ulContextValue;
	}
}
