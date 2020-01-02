using System;

namespace Steamworks
{
	public struct UGCFileWriteStreamHandle_t : global::System.IEquatable<global::Steamworks.UGCFileWriteStreamHandle_t>, global::System.IComparable<global::Steamworks.UGCFileWriteStreamHandle_t>
	{
		public UGCFileWriteStreamHandle_t(ulong value)
		{
			this.m_UGCFileWriteStreamHandle = value;
		}

		public override string ToString()
		{
			return this.m_UGCFileWriteStreamHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.UGCFileWriteStreamHandle_t && this == (global::Steamworks.UGCFileWriteStreamHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_UGCFileWriteStreamHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;
		}

		public int CompareTo(global::Steamworks.UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
		}

		public static bool operator ==(global::Steamworks.UGCFileWriteStreamHandle_t x, global::Steamworks.UGCFileWriteStreamHandle_t y)
		{
			return x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;
		}

		public static bool operator !=(global::Steamworks.UGCFileWriteStreamHandle_t x, global::Steamworks.UGCFileWriteStreamHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.UGCFileWriteStreamHandle_t(ulong value)
		{
			return new global::Steamworks.UGCFileWriteStreamHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.UGCFileWriteStreamHandle_t that)
		{
			return that.m_UGCFileWriteStreamHandle;
		}

		public static readonly global::Steamworks.UGCFileWriteStreamHandle_t Invalid = new global::Steamworks.UGCFileWriteStreamHandle_t(ulong.MaxValue);

		public ulong m_UGCFileWriteStreamHandle;
	}
}
