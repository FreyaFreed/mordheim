using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.StdCall)]
	public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
