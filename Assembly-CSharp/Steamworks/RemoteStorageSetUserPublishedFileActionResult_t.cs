using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1327)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSetUserPublishedFileActionResult_t
	{
		public const int k_iCallback = 1327;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;

		public global::Steamworks.EWorkshopFileAction m_eAction;
	}
}
