using System;

namespace Steamworks
{
	public struct HTTPRequestHandle : global::System.IEquatable<global::Steamworks.HTTPRequestHandle>, global::System.IComparable<global::Steamworks.HTTPRequestHandle>
	{
		public HTTPRequestHandle(uint value)
		{
			this.m_HTTPRequestHandle = value;
		}

		public override string ToString()
		{
			return this.m_HTTPRequestHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HTTPRequestHandle && this == (global::Steamworks.HTTPRequestHandle)other;
		}

		public override int GetHashCode()
		{
			return this.m_HTTPRequestHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;
		}

		public int CompareTo(global::Steamworks.HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
		}

		public static bool operator ==(global::Steamworks.HTTPRequestHandle x, global::Steamworks.HTTPRequestHandle y)
		{
			return x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;
		}

		public static bool operator !=(global::Steamworks.HTTPRequestHandle x, global::Steamworks.HTTPRequestHandle y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HTTPRequestHandle(uint value)
		{
			return new global::Steamworks.HTTPRequestHandle(value);
		}

		public static explicit operator uint(global::Steamworks.HTTPRequestHandle that)
		{
			return that.m_HTTPRequestHandle;
		}

		public static readonly global::Steamworks.HTTPRequestHandle Invalid = new global::Steamworks.HTTPRequestHandle(0U);

		public uint m_HTTPRequestHandle;
	}
}
