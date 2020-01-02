using System;

namespace Steamworks
{
	public struct HSteamUser : global::System.IEquatable<global::Steamworks.HSteamUser>, global::System.IComparable<global::Steamworks.HSteamUser>
	{
		public HSteamUser(int value)
		{
			this.m_HSteamUser = value;
		}

		public override string ToString()
		{
			return this.m_HSteamUser.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HSteamUser && this == (global::Steamworks.HSteamUser)other;
		}

		public override int GetHashCode()
		{
			return this.m_HSteamUser.GetHashCode();
		}

		public bool Equals(global::Steamworks.HSteamUser other)
		{
			return this.m_HSteamUser == other.m_HSteamUser;
		}

		public int CompareTo(global::Steamworks.HSteamUser other)
		{
			return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
		}

		public static bool operator ==(global::Steamworks.HSteamUser x, global::Steamworks.HSteamUser y)
		{
			return x.m_HSteamUser == y.m_HSteamUser;
		}

		public static bool operator !=(global::Steamworks.HSteamUser x, global::Steamworks.HSteamUser y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HSteamUser(int value)
		{
			return new global::Steamworks.HSteamUser(value);
		}

		public static explicit operator int(global::Steamworks.HSteamUser that)
		{
			return that.m_HSteamUser;
		}

		public int m_HSteamUser;
	}
}
