using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public class ISteamMatchmakingServerListResponse
	{
		public ISteamMatchmakingServerListResponse(global::Steamworks.ISteamMatchmakingServerListResponse.ServerResponded onServerResponded, global::Steamworks.ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond, global::Steamworks.ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
		{
			if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
			{
				throw new global::System.ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_RefreshComplete = onRefreshComplete;
			this.m_VTable = new global::Steamworks.ISteamMatchmakingServerListResponse.VTable
			{
				m_VTServerResponded = new global::Steamworks.ISteamMatchmakingServerListResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new global::Steamworks.ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
				m_VTRefreshComplete = new global::Steamworks.ISteamMatchmakingServerListResponse.InternalRefreshComplete(this.InternalOnRefreshComplete)
			};
			this.m_pVTable = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.ISteamMatchmakingServerListResponse.VTable)));
			global::System.Runtime.InteropServices.Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(this.m_pVTable, global::System.Runtime.InteropServices.GCHandleType.Pinned);
		}

		~ISteamMatchmakingServerListResponse()
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

		private void InternalOnServerResponded(global::System.IntPtr thisptr, global::Steamworks.HServerListRequest hRequest, int iServer)
		{
			this.m_ServerResponded(hRequest, iServer);
		}

		private void InternalOnServerFailedToRespond(global::System.IntPtr thisptr, global::Steamworks.HServerListRequest hRequest, int iServer)
		{
			this.m_ServerFailedToRespond(hRequest, iServer);
		}

		private void InternalOnRefreshComplete(global::System.IntPtr thisptr, global::Steamworks.HServerListRequest hRequest, global::Steamworks.EMatchMakingServerResponse response)
		{
			this.m_RefreshComplete(hRequest, response);
		}

		public static explicit operator global::System.IntPtr(global::Steamworks.ISteamMatchmakingServerListResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		private global::Steamworks.ISteamMatchmakingServerListResponse.VTable m_VTable;

		private global::System.IntPtr m_pVTable;

		private global::System.Runtime.InteropServices.GCHandle m_pGCHandle;

		private global::Steamworks.ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;

		private global::Steamworks.ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;

		private global::Steamworks.ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
		private class VTable
		{
			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
		}

		public delegate void ServerResponded(global::Steamworks.HServerListRequest hRequest, int iServer);

		public delegate void ServerFailedToRespond(global::Steamworks.HServerListRequest hRequest, int iServer);

		public delegate void RefreshComplete(global::Steamworks.HServerListRequest hRequest, global::Steamworks.EMatchMakingServerResponse response);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(global::System.IntPtr thisptr, global::Steamworks.HServerListRequest hRequest, int iServer);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(global::System.IntPtr thisptr, global::Steamworks.HServerListRequest hRequest, int iServer);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		private delegate void InternalRefreshComplete(global::System.IntPtr thisptr, global::Steamworks.HServerListRequest hRequest, global::Steamworks.EMatchMakingServerResponse response);
	}
}
