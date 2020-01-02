using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1008)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RegisterActivationCodeResponse_t
	{
		public const int k_iCallback = 1008;

		public global::Steamworks.ERegisterActivationCodeResult m_eResult;

		public uint m_unPackageRegistered;
	}
}
