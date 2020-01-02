using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public sealed class Callback<T>
	{
		public Callback(global::Steamworks.Callback<T>.DispatchDelegate func, bool bGameServer = false)
		{
			this.m_bGameServer = bGameServer;
			this.BuildCCallbackBase();
			this.Register(func);
		}

		private event global::Steamworks.Callback<T>.DispatchDelegate m_Func;

		public static global::Steamworks.Callback<T> Create(global::Steamworks.Callback<T>.DispatchDelegate func)
		{
			return new global::Steamworks.Callback<T>(func, false);
		}

		public static global::Steamworks.Callback<T> CreateGameServer(global::Steamworks.Callback<T>.DispatchDelegate func)
		{
			return new global::Steamworks.Callback<T>(func, true);
		}

		~Callback()
		{
			this.Unregister();
			if (this.m_pVTable != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pCCallbackBase.IsAllocated)
			{
				this.m_pCCallbackBase.Free();
			}
		}

		public void Register(global::Steamworks.Callback<T>.DispatchDelegate func)
		{
			if (func == null)
			{
				throw new global::System.Exception("Callback function must not be null.");
			}
			if ((this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
			{
				this.Unregister();
			}
			if (this.m_bGameServer)
			{
				this.SetGameserverFlag();
			}
			this.m_Func = func;
			global::Steamworks.NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), global::Steamworks.CallbackIdentities.GetCallbackIdentity(typeof(T)));
		}

		public void Unregister()
		{
			global::Steamworks.NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
		}

		public void SetGameserverFlag()
		{
			global::Steamworks.CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		private void OnRunCallback(global::System.IntPtr thisptr, global::System.IntPtr pvParam)
		{
			try
			{
				this.m_Func((T)((object)global::System.Runtime.InteropServices.Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (global::System.Exception e)
			{
				global::Steamworks.CallbackDispatcher.ExceptionHandler(e);
			}
		}

		private void OnRunCallResult(global::System.IntPtr thisptr, global::System.IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
		{
			try
			{
				this.m_Func((T)((object)global::System.Runtime.InteropServices.Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (global::System.Exception e)
			{
				global::Steamworks.CallbackDispatcher.ExceptionHandler(e);
			}
		}

		private int OnGetCallbackSizeBytes(global::System.IntPtr thisptr)
		{
			return this.m_size;
		}

		private void BuildCCallbackBase()
		{
			this.VTable = new global::Steamworks.CCallbackBaseVTable
			{
				m_RunCallResult = new global::Steamworks.CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
				m_RunCallback = new global::Steamworks.CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
				m_GetCallbackSizeBytes = new global::Steamworks.CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
			};
			this.m_pVTable = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.CCallbackBaseVTable)));
			global::System.Runtime.InteropServices.Marshal.StructureToPtr(this.VTable, this.m_pVTable, false);
			this.m_CCallbackBase = new global::Steamworks.CCallbackBase
			{
				m_vfptr = this.m_pVTable,
				m_nCallbackFlags = 0,
				m_iCallback = global::Steamworks.CallbackIdentities.GetCallbackIdentity(typeof(T))
			};
			this.m_pCCallbackBase = global::System.Runtime.InteropServices.GCHandle.Alloc(this.m_CCallbackBase, global::System.Runtime.InteropServices.GCHandleType.Pinned);
		}

		private global::Steamworks.CCallbackBaseVTable VTable;

		private global::System.IntPtr m_pVTable = global::System.IntPtr.Zero;

		private global::Steamworks.CCallbackBase m_CCallbackBase;

		private global::System.Runtime.InteropServices.GCHandle m_pCCallbackBase;

		private bool m_bGameServer;

		private readonly int m_size = global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

		public delegate void DispatchDelegate(T param);
	}
}
