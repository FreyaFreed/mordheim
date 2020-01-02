using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1303)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncProgress_t
	{
		public const int k_iCallback = 1303;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchCurrentFile;

		public global::Steamworks.AppId_t m_nAppID;

		public uint m_uBytesTransferredThisChunk;

		public double m_dAppPercentComplete;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bUploading;
	}
}
