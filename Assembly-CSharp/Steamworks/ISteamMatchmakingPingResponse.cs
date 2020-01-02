using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public class ISteamMatchmakingPingResponse
	{
		public ISteamMatchmakingPingResponse(global::Steamworks.ISteamMatchmakingPingResponse.ServerResponded onServerResponded, global::Steamworks.ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
		{
			if (onServerResponded == null || onServerFailedToRespond == null)
			{
				throw new global::System.ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_VTable = new global::Steamworks.ISteamMatchmakingPingResponse.VTable
			{
				m_VTServerResponded = new global::Steamworks.ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new global::Steamworks.ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
			};
			this.m_pVTable = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.ISteamMatchmakingPingResponse.VTable)));
			global::System.Runtime.InteropServices.Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(this.m_pVTable, global::System.Runtime.InteropServices.GCHandleType.Pinned);
		}

		~ISteamMatchmakingPingResponse()
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

		private void InternalOnServerResponded(global::System.IntPtr thisptr, global::Steamworks.gameserveritem_t server)
		{
			this.m_ServerResponded(server);
		}

		private void InternalOnServerFailedToRespond(global::System.IntPtr thisptr)
		{
			this.m_ServerFailedToRespond();
		}

		public static explicit operator global::System.IntPtr(global::Steamworks.ISteamMatchmakingPingResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		private global::Steamworks.ISteamMatchmakingPingResponse.VTable m_VTable;

		private global::System.IntPtr m_pVTable;

		private global::System.Runtime.InteropServices.GCHandle m_pGCHandle;

		private global::Steamworks.ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;

		private global::Steamworks.ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
		private class VTable
		{
			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
		}

		public delegate void ServerResponded(global::Steamworks.gameserveritem_t server);

		public delegate void ServerFailedToRespond();

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(global::System.IntPtr thisptr, global::Steamworks.gameserveritem_t server);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(global::System.IntPtr thisptr);
	}
}
