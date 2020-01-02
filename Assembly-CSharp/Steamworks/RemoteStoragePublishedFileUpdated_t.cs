using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1330)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUpdated_t
	{
		public const int k_iCallback = 1330;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;

		public global::Steamworks.AppId_t m_nAppID;

		public global::Steamworks.UGCHandle_t m_hFile;
	}
}
