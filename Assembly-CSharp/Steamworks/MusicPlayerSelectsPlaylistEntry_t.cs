using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4013)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsPlaylistEntry_t
	{
		public const int k_iCallback = 4013;

		public int nID;
	}
}
