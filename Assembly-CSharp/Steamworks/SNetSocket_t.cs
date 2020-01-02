using System;

namespace Steamworks
{
	public struct SNetSocket_t : global::System.IEquatable<global::Steamworks.SNetSocket_t>, global::System.IComparable<global::Steamworks.SNetSocket_t>
	{
		public SNetSocket_t(uint value)
		{
			this.m_SNetSocket = value;
		}

		public override string ToString()
		{
			return this.m_SNetSocket.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SNetSocket_t && this == (global::Steamworks.SNetSocket_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SNetSocket.GetHashCode();
		}

		public bool Equals(global::Steamworks.SNetSocket_t other)
		{
			return this.m_SNetSocket == other.m_SNetSocket;
		}

		public int CompareTo(global::Steamworks.SNetSocket_t other)
		{
			return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
		}

		public static bool operator ==(global::Steamworks.SNetSocket_t x, global::Steamworks.SNetSocket_t y)
		{
			return x.m_SNetSocket == y.m_SNetSocket;
		}

		public static bool operator !=(global::Steamworks.SNetSocket_t x, global::Steamworks.SNetSocket_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SNetSocket_t(uint value)
		{
			return new global::Steamworks.SNetSocket_t(value);
		}

		public static explicit operator uint(global::Steamworks.SNetSocket_t that)
		{
			return that.m_SNetSocket;
		}

		public uint m_SNetSocket;
	}
}
