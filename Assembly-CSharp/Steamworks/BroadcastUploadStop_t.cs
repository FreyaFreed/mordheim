using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4605)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStop_t
	{
		public const int k_iCallback = 4605;

		public global::Steamworks.EBroadcastUploadResult m_eResult;
	}
}
