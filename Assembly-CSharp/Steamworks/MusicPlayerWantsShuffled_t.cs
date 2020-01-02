using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(4109)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsShuffled_t
	{
		public const int k_iCallback = 4109;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bShuffled;
	}
}
