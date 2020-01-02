using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
	internal class CCallbackBase
	{
		public const byte k_ECallbackFlagsRegistered = 1;

		public const byte k_ECallbackFlagsGameServer = 2;

		public global::System.IntPtr m_vfptr;

		public byte m_nCallbackFlags;

		public int m_iCallback;
	}
}
