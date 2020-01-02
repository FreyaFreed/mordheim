using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.StdCall)]
	public delegate void SteamAPI_PostAPIResultInProcess_t(global::Steamworks.SteamAPICall_t callHandle, global::System.IntPtr pUnknown, uint unCallbackSize, int iCallbackNum);
}
