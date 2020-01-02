using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class Packsize
	{
		public static bool Test()
		{
			int num = global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.Packsize.ValvePackingSentinel_t));
			int num2 = global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.RemoteStorageEnumerateUserSubscribedFilesResult_t));
			return num == 32 && num2 == 616;
		}

		public const int value = 8;

		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
		private struct ValvePackingSentinel_t
		{
			private uint m_u32;

			private ulong m_u64;

			private ushort m_u16;

			private double m_d;
		}
	}
}
