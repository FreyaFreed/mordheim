using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
	internal class CCallbackBaseVTable
	{
		private const global::System.Runtime.InteropServices.CallingConvention cc = global::System.Runtime.InteropServices.CallingConvention.Cdecl;

		[global::System.NonSerialized]
		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
		public global::Steamworks.CCallbackBaseVTable.RunCRDel m_RunCallResult;

		[global::System.NonSerialized]
		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
		public global::Steamworks.CCallbackBaseVTable.RunCBDel m_RunCallback;

		[global::System.NonSerialized]
		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
		public global::Steamworks.CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public delegate void RunCBDel(global::System.IntPtr thisptr, global::System.IntPtr pvParam);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public delegate void RunCRDel(global::System.IntPtr thisptr, global::System.IntPtr pvParam, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public delegate int GetCallbackSizeBytesDel(global::System.IntPtr thisptr);
	}
}
