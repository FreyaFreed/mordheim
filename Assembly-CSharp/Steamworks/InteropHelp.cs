using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	public class InteropHelp
	{
		public static void TestIfPlatformSupported()
		{
		}

		public static void TestIfAvailableClient()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			if (global::Steamworks.NativeMethods.SteamClient() == global::System.IntPtr.Zero)
			{
				throw new global::System.InvalidOperationException("Steamworks is not initialized.");
			}
		}

		public static void TestIfAvailableGameServer()
		{
			global::Steamworks.InteropHelp.TestIfPlatformSupported();
			if (global::Steamworks.NativeMethods.SteamClientGameServer() == global::System.IntPtr.Zero)
			{
				throw new global::System.InvalidOperationException("Steamworks is not initialized.");
			}
		}

		public static string PtrToStringUTF8(global::System.IntPtr nativeUtf8)
		{
			if (nativeUtf8 == global::System.IntPtr.Zero)
			{
				return string.Empty;
			}
			int num = 0;
			while (global::System.Runtime.InteropServices.Marshal.ReadByte(nativeUtf8, num) != 0)
			{
				num++;
			}
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			global::System.Runtime.InteropServices.Marshal.Copy(nativeUtf8, array, 0, array.Length);
			return global::System.Text.Encoding.UTF8.GetString(array);
		}

		public class UTF8StringHandle : global::Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
		{
			public UTF8StringHandle(string str) : base(true)
			{
				if (str == null)
				{
					base.SetHandle(global::System.IntPtr.Zero);
					return;
				}
				byte[] array = new byte[global::System.Text.Encoding.UTF8.GetByteCount(str) + 1];
				global::System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
				global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(array.Length);
				global::System.Runtime.InteropServices.Marshal.Copy(array, 0, intPtr, array.Length);
				base.SetHandle(intPtr);
			}

			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		public class SteamParamStringArray
		{
			public SteamParamStringArray(global::System.Collections.Generic.IList<string> strings)
			{
				if (strings == null)
				{
					this.m_pSteamParamStringArray = global::System.IntPtr.Zero;
					return;
				}
				this.m_Strings = new global::System.IntPtr[strings.Count];
				for (int i = 0; i < strings.Count; i++)
				{
					byte[] array = new byte[global::System.Text.Encoding.UTF8.GetByteCount(strings[i]) + 1];
					global::System.Text.Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, array, 0);
					this.m_Strings[i] = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(array.Length);
					global::System.Runtime.InteropServices.Marshal.Copy(array, 0, this.m_Strings[i], array.Length);
				}
				this.m_ptrStrings = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)) * this.m_Strings.Length);
				global::Steamworks.SteamParamStringArray_t steamParamStringArray_t = new global::Steamworks.SteamParamStringArray_t
				{
					m_ppStrings = this.m_ptrStrings,
					m_nNumStrings = this.m_Strings.Length
				};
				global::System.Runtime.InteropServices.Marshal.Copy(this.m_Strings, 0, steamParamStringArray_t.m_ppStrings, this.m_Strings.Length);
				this.m_pSteamParamStringArray = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.SteamParamStringArray_t)));
				global::System.Runtime.InteropServices.Marshal.StructureToPtr(steamParamStringArray_t, this.m_pSteamParamStringArray, false);
			}

			protected override void Finalize()
			{
				try
				{
					foreach (global::System.IntPtr hglobal in this.m_Strings)
					{
						global::System.Runtime.InteropServices.Marshal.FreeHGlobal(hglobal);
					}
					if (this.m_ptrStrings != global::System.IntPtr.Zero)
					{
						global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_ptrStrings);
					}
					if (this.m_pSteamParamStringArray != global::System.IntPtr.Zero)
					{
						global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			public static implicit operator global::System.IntPtr(global::Steamworks.InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}

			private global::System.IntPtr[] m_Strings;

			private global::System.IntPtr m_ptrStrings;

			private global::System.IntPtr m_pSteamParamStringArray;
		}
	}
}
