using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1318)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedFileDetailsResult_t
	{
		public const int k_iCallback = 1318;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;

		public global::Steamworks.AppId_t m_nCreatorAppID;

		public global::Steamworks.AppId_t m_nConsumerAppID;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		public global::Steamworks.UGCHandle_t m_hFile;

		public global::Steamworks.UGCHandle_t m_hPreviewFile;

		public ulong m_ulSteamIDOwner;

		public uint m_rtimeCreated;

		public uint m_rtimeUpdated;

		public global::Steamworks.ERemoteStoragePublishedFileVisibility m_eVisibility;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bBanned;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		public int m_nFileSize;

		public int m_nPreviewFileSize;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		public global::Steamworks.EWorkshopFileType m_eFileType;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bAcceptedForUse;
	}
}
