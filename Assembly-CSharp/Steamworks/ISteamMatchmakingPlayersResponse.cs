using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public class ISteamMatchmakingPlayersResponse
	{
		public ISteamMatchmakingPlayersResponse(global::Steamworks.ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList, global::Steamworks.ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond, global::Steamworks.ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
		{
			if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
			{
				throw new global::System.ArgumentNullException();
			}
			this.m_AddPlayerToList = onAddPlayerToList;
			this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
			this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
			this.m_VTable = new global::Steamworks.ISteamMatchmakingPlayersResponse.VTable
			{
				m_VTAddPlayerToList = new global::Steamworks.ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(this.InternalOnAddPlayerToList),
				m_VTPlayersFailedToRespond = new global::Steamworks.ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
				m_VTPlayersRefreshComplete = new global::Steamworks.ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
			};
			this.m_pVTable = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.ISteamMatchmakingPlayersResponse.VTable)));
			global::System.Runtime.InteropServices.Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(this.m_pVTable, global::System.Runtime.InteropServices.GCHandleType.Pinned);
		}

		~ISteamMatchmakingPlayersResponse()
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

		private void InternalOnAddPlayerToList(global::System.IntPtr thisptr, global::System.IntPtr pchName, int nScore, float flTimePlayed)
		{
			this.m_AddPlayerToList(global::Steamworks.InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
		}

		private void InternalOnPlayersFailedToRespond(global::System.IntPtr thisptr)
		{
			this.m_PlayersFailedToRespond();
		}

		private void InternalOnPlayersRefreshComplete(global::System.IntPtr thisptr)
		{
			this.m_PlayersRefreshComplete();
		}

		public static explicit operator global::System.IntPtr(global::Steamworks.ISteamMatchmakingPlayersResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		private global::Steamworks.ISteamMatchmakingPlayersResponse.VTable m_VTable;

		private global::System.IntPtr m_pVTable;

		private global::System.Runtime.InteropServices.GCHandle m_pGCHandle;

		private global::Steamworks.ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;

		private global::Steamworks.ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;

		private global::Steamworks.ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential)]
		private class VTable
		{
			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

			[global::System.NonSerialized]
			[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.FunctionPtr)]
			public global::Steamworks.ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
		}

		public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

		public delegate void PlayersFailedToRespond();

		public delegate void PlayersRefreshComplete();

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		public delegate void InternalAddPlayerToList(global::System.IntPtr thisptr, global::System.IntPtr pchName, int nScore, float flTimePlayed);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		public delegate void InternalPlayersFailedToRespond(global::System.IntPtr thisptr);

		[global::System.Runtime.InteropServices.UnmanagedFunctionPointer(global::System.Runtime.InteropServices.CallingConvention.ThisCall)]
		public delegate void InternalPlayersRefreshComplete(global::System.IntPtr thisptr);
	}
}
