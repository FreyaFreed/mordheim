using System;

namespace Steamworks
{
	public struct AppId_t : global::System.IEquatable<global::Steamworks.AppId_t>, global::System.IComparable<global::Steamworks.AppId_t>
	{
		public AppId_t(uint value)
		{
			this.m_AppId = value;
		}

		public override string ToString()
		{
			return this.m_AppId.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.AppId_t && this == (global::Steamworks.AppId_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_AppId.GetHashCode();
		}

		public bool Equals(global::Steamworks.AppId_t other)
		{
			return this.m_AppId == other.m_AppId;
		}

		public int CompareTo(global::Steamworks.AppId_t other)
		{
			return this.m_AppId.CompareTo(other.m_AppId);
		}

		public static bool operator ==(global::Steamworks.AppId_t x, global::Steamworks.AppId_t y)
		{
			return x.m_AppId == y.m_AppId;
		}

		public static bool operator !=(global::Steamworks.AppId_t x, global::Steamworks.AppId_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.AppId_t(uint value)
		{
			return new global::Steamworks.AppId_t(value);
		}

		public static explicit operator uint(global::Steamworks.AppId_t that)
		{
			return that.m_AppId;
		}

		public static readonly global::Steamworks.AppId_t Invalid = new global::Steamworks.AppId_t(0U);

		public uint m_AppId;
	}
}
