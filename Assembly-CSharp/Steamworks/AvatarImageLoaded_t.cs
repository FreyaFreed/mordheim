using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(334)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4)]
	public struct AvatarImageLoaded_t
	{
		public const int k_iCallback = 334;

		public global::Steamworks.CSteamID m_steamID;

		public int m_iImage;

		public int m_iWide;

		public int m_iTall;
	}
}
