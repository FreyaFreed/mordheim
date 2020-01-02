using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public class MMKVPMarshaller
	{
		public MMKVPMarshaller(global::Steamworks.MatchMakingKeyValuePair_t[] filters)
		{
			if (filters == null)
			{
				return;
			}
			int num = global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::Steamworks.MatchMakingKeyValuePair_t));
			this.m_pNativeArray = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(global::System.Runtime.InteropServices.Marshal.SizeOf(typeof(global::System.IntPtr)) * filters.Length);
			this.m_pArrayEntries = global::System.Runtime.InteropServices.Marshal.AllocHGlobal(num * filters.Length);
			for (int i = 0; i < filters.Length; i++)
			{
				global::System.Runtime.InteropServices.Marshal.StructureToPtr(filters[i], new global::System.IntPtr(this.m_pArrayEntries.ToInt64() + (long)(i * num)), false);
			}
			global::System.Runtime.InteropServices.Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
		}

		~MMKVPMarshaller()
		{
			if (this.m_pArrayEntries != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_pArrayEntries);
			}
			if (this.m_pNativeArray != global::System.IntPtr.Zero)
			{
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(this.m_pNativeArray);
			}
		}

		public static implicit operator global::System.IntPtr(global::Steamworks.MMKVPMarshaller that)
		{
			return that.m_pNativeArray;
		}

		private global::System.IntPtr m_pNativeArray;

		private global::System.IntPtr m_pArrayEntries;
	}
}
