using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public sealed class CallResult<T>
	{
		public CallResult(global::Steamworks.CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
			this.BuildCCallbackBase();
		}

		private event global::Steamworks.CallResult<T>.APIDispatchDelegate m_Func;

		public global::Steamworks.SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		public static global::Steamworks.CallResult<T> Create(global::Steamworks.CallResult<T>.APIDispatchDelegate func = null)
		{
			return new global::Steamworks.CallResult<T>(func);
		}

		~CallResult()
		{
			this.Cancel();
			if (this.m_pVTable != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pCCallbackBase.IsAllocated)
			{
				this.m_pCCallbackBase.Free();
			}
		}

		public void Set(global::Steamworks.SteamAPICall_t hAPICall, global::Steamworks.CallResult<T>.APIDispatchDelegate func = null)
		{
			if (func != null)
			{
				this.m_Func = func;
			}
			if (this.m_Func == null)
			{
				throw new global::System.Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
			}
			if (this.m_hAPICall != global::Steamworks.SteamAPICall_t.Invalid)
			{
				global::Steamworks.NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
			}
			this.m_hAPICall = hAPICall;
			if (hAPICall != global::Steamworks.SteamAPICall_t.Invalid)
			{
				global::Steamworks.NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)hAPICall);
			}
		}

		public bool IsActive()
		{
			return this.m_hAPICall != global::Steamworks.SteamAPICall_t.Invalid;
		}

		public void Cancel()
		{
			if (this.m_hAPICall != global::Steamworks.SteamAPICall_t.Invalid)
			{
				global::Steamworks.NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
				this.m_hAPICall = global::Steamworks.SteamAPICall_t.Invalid;
			}
		}

		public void SetGameserverFlag()
		{
			global::Steamworks.CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		private void OnRunCallback(global::System.IntPtr thisptr, global::System.IntPtr pvParam)
		{
			this.m_hAPICall = global::Steamworks.SteamAPICall_t.Invalid;
			try
			{
				this.m_Func((T)((object)global::System.Runtime.InteropServices.Marshal.PtrToStructure(pvParam, typeof(T))), false);
			}
			catch (global::System.Exception e)
			{
				global::Steamworks.CallbackDispatcher.ExceptionHandler(e);
			}
		}

		private void OnRunCallResult(global::System.IntPtr thisptr, global::System.IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
		{
			global::Steamworks.SteamAPICall_t x = (global::Steamworks.SteamAPICall_t)hSteamAPICall;
			if (x == this.m_hAPICall)
			{
				try
				{
					this.m_Func((T)((object)global::System.Runtime.InteropServices.Marshal.PtrToStructure(pvParam, typeof(T))), bFailed);
				}
				catch (global::System.Exception e)
				{
					global::Steamworks.CallbackDispatcher.ExceptionHandler(e);
				}
				if (x == this.m_hAPICall)
				{
					this.m_hAPICall = global::Steamworks.SteamAPICall_t.Invalid;
				}
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
				m_RunCallback = new global::Steamworks.CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
				m_RunCallResult = new global::Steamworks.CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
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

		private global::Steamworks.SteamAPICall_t m_hAPICall = global::Steamworks.SteamAPICall_t.Invalid;

		private readonly int m_size = global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
