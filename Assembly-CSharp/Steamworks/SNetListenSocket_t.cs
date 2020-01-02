using System;

namespace Steamworks
{
	public struct SNetListenSocket_t : global::System.IEquatable<global::Steamworks.SNetListenSocket_t>, global::System.IComparable<global::Steamworks.SNetListenSocket_t>
	{
		public SNetListenSocket_t(uint value)
		{
			this.m_SNetListenSocket = value;
		}

		public override string ToString()
		{
			return this.m_SNetListenSocket.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SNetListenSocket_t && this == (global::Steamworks.SNetListenSocket_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SNetListenSocket.GetHashCode();
		}

		public bool Equals(global::Steamworks.SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket == other.m_SNetListenSocket;
		}

		public int CompareTo(global::Steamworks.SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
		}

		public static bool operator ==(global::Steamworks.SNetListenSocket_t x, global::Steamworks.SNetListenSocket_t y)
		{
			return x.m_SNetListenSocket == y.m_SNetListenSocket;
		}

		public static bool operator !=(global::Steamworks.SNetListenSocket_t x, global::Steamworks.SNetListenSocket_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SNetListenSocket_t(uint value)
		{
			return new global::Steamworks.SNetListenSocket_t(value);
		}

		public static explicit operator uint(global::Steamworks.SNetListenSocket_t that)
		{
			return that.m_SNetListenSocket;
		}

		public uint m_SNetListenSocket;
	}
}
