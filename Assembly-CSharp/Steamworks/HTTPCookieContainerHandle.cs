using System;

namespace Steamworks
{
	public struct HTTPCookieContainerHandle : global::System.IEquatable<global::Steamworks.HTTPCookieContainerHandle>, global::System.IComparable<global::Steamworks.HTTPCookieContainerHandle>
	{
		public HTTPCookieContainerHandle(uint value)
		{
			this.m_HTTPCookieContainerHandle = value;
		}

		public override string ToString()
		{
			return this.m_HTTPCookieContainerHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HTTPCookieContainerHandle && this == (global::Steamworks.HTTPCookieContainerHandle)other;
		}

		public override int GetHashCode()
		{
			return this.m_HTTPCookieContainerHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;
		}

		public int CompareTo(global::Steamworks.HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
		}

		public static bool operator ==(global::Steamworks.HTTPCookieContainerHandle x, global::Steamworks.HTTPCookieContainerHandle y)
		{
			return x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;
		}

		public static bool operator !=(global::Steamworks.HTTPCookieContainerHandle x, global::Steamworks.HTTPCookieContainerHandle y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HTTPCookieContainerHandle(uint value)
		{
			return new global::Steamworks.HTTPCookieContainerHandle(value);
		}

		public static explicit operator uint(global::Steamworks.HTTPCookieContainerHandle that)
		{
			return that.m_HTTPCookieContainerHandle;
		}

		public static readonly global::Steamworks.HTTPCookieContainerHandle Invalid = new global::Steamworks.HTTPCookieContainerHandle(0U);

		public uint m_HTTPCookieContainerHandle;
	}
}
