using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1307)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileShareResult_t
	{
		public const int k_iCallback = 1307;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.UGCHandle_t m_hFile;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchFilename;
	}
}
