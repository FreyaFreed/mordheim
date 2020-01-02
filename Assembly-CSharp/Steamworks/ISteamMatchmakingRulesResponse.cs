using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public class ISteamMatchmakingRulesResponse
	{
		public ISteamMatchmakingRulesResponse(global::Steamworks.ISteamMatchmakingRulesResponse.RulesResponded onRulesResponded, global::Steamworks.ISteamMatchmakingRulesResponse.RulesFailedToRespond onRulesFailedToRespond, global::Steamworks.ISteamMatchmakingRulesResponse.RulesRefreshComplete onRulesRefreshComplete)
		{
			if (onRulesResponded == null || onRulesFailedToRespond == null || onRulesRefreshComplete == null)
			{
				throw new global::System.ArgumentNullException();
			}
			this.m_RulesResponded = onRulesResponded;
			this.m_RulesFailedToRespond = onRulesFailedToRespond;
			this.m_RulesRefreshComplete = onRulesRefreshComplete;
			this.m_VTable = new global::Steamworks.ISteamMatchmakingRulesResponse.VTable
			{
				m_VTRulesResponded = new global::Steamworks.ISteamMatchmakingRulesResponse.InternalRulesResponded(this.InternalOnRulesResponded),
				m_VTRulesFailedToRespond = new global::Steamworks.ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond(this.InternalOnRulesFailedToRespond),
				m_VTRulesRefreshComplete = new global::Steamworks.ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete(this.InternalOnRulesRefreshComplete)
			};
			this.m_pVTable = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.ISteamMatchmakingRulesResponse.VTable)));
			global::System.Runtime.InteropServices.Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(this.m_pVTable, global::System.Runtime.InteropServices.GCHandleType.Pinned);
		}

		~ISteamMatchmakingRulesResponse()
		{
			if (this.m_pVTable != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pGCHandle.IsAllocated)
			{
				this.m_pGCHandle.Free();
			}
		}

		private void InternalOnRulesResponded(global::System.IntPtr thisptr, global::System.IntPtr pchRule, global::System.IntPtr pchValue)
		{
			this.m_RulesResponded(global::Steamworks.InteropHelp.PtrToStringUTF8(pchRule), global::Steamworks.InteropHelp.PtrToStringUTF8(pchValue));
		}

		private void InternalOnRulesFailedToRespond(global::System.IntPtr thisptr)
		{
			this.m_RulesFailedToRespond();
		}

		private void InternalOnRulesRefreshComplete(global::System.IntPtr thisptr)
		{
			this.m_RulesRefreshComplete();
		}

		public static explicit operator global::System.IntPtr(global::Steamworks.ISteamMatchmakingRulesResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		private global::Steamworks.ISteamMatchmakingRulesResponse.VTable m_VTable;

		private global::System.IntPtr m_pVTable;

		private global::System.Runtime.InteropServices.GCHandle m_pGCHandle;

		private global::Steamworks.ISteamMatchmakingRulesResponse.RulesResponded m_RulesResponded;

		private global::Steamworks.ISteamMatchmakingRulesResponse.RulesFailedToRespond m_RulesFailedToRespond;

		private global::Steamworks.ISteamMatchmakingRulesResponse.RulesRefreshComplete m_RulesRefreshComplete;

		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
		private class VTable
		{
			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
		}

		public delegate void RulesResponded(string pchRule, string pchValue);

		public delegate void RulesFailedToRespond();

		public delegate void RulesRefreshComplete();

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		public delegate void InternalRulesResponded(global::System.IntPtr thisptr, global::System.IntPtr pchRule, global::System.IntPtr pchValue);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		public delegate void InternalRulesFailedToRespond(global::System.IntPtr thisptr);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		public delegate void InternalRulesRefreshComplete(global::System.IntPtr thisptr);
	}
}
