using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1701)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct GCMessageAvailable_t
	{
		public const int k_iCallback = 1701;

		public uint m_nMessageSize;
	}
}
