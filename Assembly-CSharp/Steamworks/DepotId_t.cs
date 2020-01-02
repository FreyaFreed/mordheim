using System;

namespace Steamworks
{
	public struct DepotId_t : global::System.IEquatable<global::Steamworks.DepotId_t>, global::System.IComparable<global::Steamworks.DepotId_t>
	{
		public DepotId_t(uint value)
		{
			this.m_DepotId = value;
		}

		public override string ToString()
		{
			return this.m_DepotId.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.DepotId_t && this == (global::Steamworks.DepotId_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_DepotId.GetHashCode();
		}

		public bool Equals(global::Steamworks.DepotId_t other)
		{
			return this.m_DepotId == other.m_DepotId;
		}

		public int CompareTo(global::Steamworks.DepotId_t other)
		{
			return this.m_DepotId.CompareTo(other.m_DepotId);
		}

		public static bool operator ==(global::Steamworks.DepotId_t x, global::Steamworks.DepotId_t y)
		{
			return x.m_DepotId == y.m_DepotId;
		}

		public static bool operator !=(global::Steamworks.DepotId_t x, global::Steamworks.DepotId_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.DepotId_t(uint value)
		{
			return new global::Steamworks.DepotId_t(value);
		}

		public static explicit operator uint(global::Steamworks.DepotId_t that)
		{
			return that.m_DepotId;
		}

		public static readonly global::Steamworks.DepotId_t Invalid = new global::Steamworks.DepotId_t(0U);

		public uint m_DepotId;
	}
}
