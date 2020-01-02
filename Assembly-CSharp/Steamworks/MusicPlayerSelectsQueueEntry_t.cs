using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4012)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsQueueEntry_t
	{
		public const int k_iCallback = 4012;

		public int nID;
	}
}
