using System;

namespace Steamworks
{
	public struct UGCUpdateHandle_t : global::System.IEquatable<global::Steamworks.UGCUpdateHandle_t>, global::System.IComparable<global::Steamworks.UGCUpdateHandle_t>
	{
		public UGCUpdateHandle_t(ulong value)
		{
			this.m_UGCUpdateHandle = value;
		}

		public override string ToString()
		{
			return this.m_UGCUpdateHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.UGCUpdateHandle_t && this == (global::Steamworks.UGCUpdateHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_UGCUpdateHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;
		}

		public int CompareTo(global::Steamworks.UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
		}

		public static bool operator ==(global::Steamworks.UGCUpdateHandle_t x, global::Steamworks.UGCUpdateHandle_t y)
		{
			return x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;
		}

		public static bool operator !=(global::Steamworks.UGCUpdateHandle_t x, global::Steamworks.UGCUpdateHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.UGCUpdateHandle_t(ulong value)
		{
			return new global::Steamworks.UGCUpdateHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.UGCUpdateHandle_t that)
		{
			return that.m_UGCUpdateHandle;
		}

		public static readonly global::Steamworks.UGCUpdateHandle_t Invalid = new global::Steamworks.UGCUpdateHandle_t(ulong.MaxValue);

		public ulong m_UGCUpdateHandle;
	}
}
