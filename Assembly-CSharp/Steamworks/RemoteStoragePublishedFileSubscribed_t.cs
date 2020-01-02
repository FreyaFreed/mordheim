using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1321)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileSubscribed_t
	{
		public const int k_iCallback = 1321;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;

		public global::Steamworks.AppId_t m_nAppID;
	}
}
