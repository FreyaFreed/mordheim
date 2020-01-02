using System;

namespace Steamworks
{
	public struct ClientUnifiedMessageHandle : global::System.IEquatable<global::Steamworks.ClientUnifiedMessageHandle>, global::System.IComparable<global::Steamworks.ClientUnifiedMessageHandle>
	{
		public ClientUnifiedMessageHandle(ulong value)
		{
			this.m_ClientUnifiedMessageHandle = value;
		}

		public override string ToString()
		{
			return this.m_ClientUnifiedMessageHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ClientUnifiedMessageHandle && this == (global::Steamworks.ClientUnifiedMessageHandle)other;
		}

		public override int GetHashCode()
		{
			return this.m_ClientUnifiedMessageHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle == other.m_ClientUnifiedMessageHandle;
		}

		public int CompareTo(global::Steamworks.ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
		}

		public static bool operator ==(global::Steamworks.ClientUnifiedMessageHandle x, global::Steamworks.ClientUnifiedMessageHandle y)
		{
			return x.m_ClientUnifiedMessageHandle == y.m_ClientUnifiedMessageHandle;
		}

		public static bool operator !=(global::Steamworks.ClientUnifiedMessageHandle x, global::Steamworks.ClientUnifiedMessageHandle y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ClientUnifiedMessageHandle(ulong value)
		{
			return new global::Steamworks.ClientUnifiedMessageHandle(value);
		}

		public static explicit operator ulong(global::Steamworks.ClientUnifiedMessageHandle that)
		{
			return that.m_ClientUnifiedMessageHandle;
		}

		public static readonly global::Steamworks.ClientUnifiedMessageHandle Invalid = new global::Steamworks.ClientUnifiedMessageHandle(0UL);

		public ulong m_ClientUnifiedMessageHandle;
	}
}
