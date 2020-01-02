using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(3405)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct ItemInstalled_t
	{
		public const int k_iCallback = 3405;

		public global::Steamworks.AppId_t m_unAppID;

		public global::Steamworks.PublishedFileId_t m_nPublishedFileId;
	}
}
