using System;

namespace Steamworks
{
	public struct HAuthTicket : global::System.IEquatable<global::Steamworks.HAuthTicket>, global::System.IComparable<global::Steamworks.HAuthTicket>
	{
		public HAuthTicket(uint value)
		{
			this.m_HAuthTicket = value;
		}

		public override string ToString()
		{
			return this.m_HAuthTicket.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HAuthTicket && this == (global::Steamworks.HAuthTicket)other;
		}

		public override int GetHashCode()
		{
			return this.m_HAuthTicket.GetHashCode();
		}

		public bool Equals(global::Steamworks.HAuthTicket other)
		{
			return this.m_HAuthTicket == other.m_HAuthTicket;
		}

		public int CompareTo(global::Steamworks.HAuthTicket other)
		{
			return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
		}

		public static bool operator ==(global::Steamworks.HAuthTicket x, global::Steamworks.HAuthTicket y)
		{
			return x.m_HAuthTicket == y.m_HAuthTicket;
		}

		public static bool operator !=(global::Steamworks.HAuthTicket x, global::Steamworks.HAuthTicket y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HAuthTicket(uint value)
		{
			return new global::Steamworks.HAuthTicket(value);
		}

		public static explicit operator uint(global::Steamworks.HAuthTicket that)
		{
			return that.m_HAuthTicket;
		}

		public static readonly global::Steamworks.HAuthTicket Invalid = new global::Steamworks.HAuthTicket(0U);

		public uint m_HAuthTicket;
	}
}
