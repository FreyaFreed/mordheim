using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(2103)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestDataReceived_t
	{
		public const int k_iCallback = 2103;

		public global::Steamworks.HTTPRequestHandle m_hRequest;

		public ulong m_ulContextValue;

		public uint m_cOffset;

		public uint m_cBytesReceived;
	}
}
