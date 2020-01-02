using System;

namespace Steamworks
{
	public struct UGCQueryHandle_t : global::System.IEquatable<global::Steamworks.UGCQueryHandle_t>, global::System.IComparable<global::Steamworks.UGCQueryHandle_t>
	{
		public UGCQueryHandle_t(ulong value)
		{
			this.m_UGCQueryHandle = value;
		}

		public override string ToString()
		{
			return this.m_UGCQueryHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.UGCQueryHandle_t && this == (global::Steamworks.UGCQueryHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_UGCQueryHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle == other.m_UGCQueryHandle;
		}

		public int CompareTo(global::Steamworks.UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
		}

		public static bool operator ==(global::Steamworks.UGCQueryHandle_t x, global::Steamworks.UGCQueryHandle_t y)
		{
			return x.m_UGCQueryHandle == y.m_UGCQueryHandle;
		}

		public static bool operator !=(global::Steamworks.UGCQueryHandle_t x, global::Steamworks.UGCQueryHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.UGCQueryHandle_t(ulong value)
		{
			return new global::Steamworks.UGCQueryHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.UGCQueryHandle_t that)
		{
			return that.m_UGCQueryHandle;
		}

		public static readonly global::Steamworks.UGCQueryHandle_t Invalid = new global::Steamworks.UGCQueryHandle_t(ulong.MaxValue);

		public ulong m_UGCQueryHandle;
	}
}
