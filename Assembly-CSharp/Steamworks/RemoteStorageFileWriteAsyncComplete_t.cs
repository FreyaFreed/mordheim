using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1331)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileWriteAsyncComplete_t
	{
		public const int k_iCallback = 1331;

		public global::Steamworks.EResult m_eResult;
	}
}
