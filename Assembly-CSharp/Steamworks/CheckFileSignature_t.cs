using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(705)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct CheckFileSignature_t
	{
		public const int k_iCallback = 705;

		public global::Steamworks.ECheckFileSignature m_eCheckFileSignature;
	}
}
