using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(3407)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct UserFavoriteItemsListChanged_t
	{
		public const int k_iCallback = 3407;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;

		public global::Steamworks.EResult m_eResult;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bWasAddRequest;
	}
}
