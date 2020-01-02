using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1316)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdatePublishedFileResult_t
	{
		public const int k_iCallback = 1316;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
