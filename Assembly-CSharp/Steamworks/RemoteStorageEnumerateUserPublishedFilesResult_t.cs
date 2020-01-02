using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1312)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserPublishedFilesResult_t
	{
		public const int k_iCallback = 1312;

		public global::Steamworks.EResult m_eResult;

		public int m_nResultsReturned;

		public int m_nTotalResultCount;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 50)]
		public global::Steamworks.PublishedFileId_t[] m_rgPublishedFileId;
	}
}
