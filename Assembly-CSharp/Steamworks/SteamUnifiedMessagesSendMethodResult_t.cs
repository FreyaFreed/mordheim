using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(2501)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamUnifiedMessagesSendMethodResult_t
	{
		public const int k_iCallback = 2501;

		public global::Steamworks.ClientUnifiedMessageHandle m_hHandle;

		public ulong m_unContext;

		public global::Steamworks.EResult m_eResult;

		public uint m_unResponseSize;
	}
}
