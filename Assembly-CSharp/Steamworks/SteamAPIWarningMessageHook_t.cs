using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void SteamAPIWarningMessageHook_t(int nSeverity, global::System.Text.StringBuilder pchDebugText);
}
