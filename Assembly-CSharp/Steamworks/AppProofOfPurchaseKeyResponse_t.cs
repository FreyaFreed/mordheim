using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1013)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct AppProofOfPurchaseKeyResponse_t
	{
		public const int k_iCallback = 1013;

		public global::Steamworks.EResult m_eResult;

		public uint m_nAppID;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
		public string m_rgchKey;
	}
}
