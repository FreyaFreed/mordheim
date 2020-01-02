using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
	public struct ControllerAnalogActionData_t
	{
		public global::Steamworks.EControllerSourceMode eMode;

		public float x;

		public float y;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool bActive;
	}
}
