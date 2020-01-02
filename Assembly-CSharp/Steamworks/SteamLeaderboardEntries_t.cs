using System;

namespace Steamworks
{
	public struct SteamLeaderboardEntries_t : global::System.IEquatable<global::Steamworks.SteamLeaderboardEntries_t>, global::System.IComparable<global::Steamworks.SteamLeaderboardEntries_t>
	{
		public SteamLeaderboardEntries_t(ulong value)
		{
			this.m_SteamLeaderboardEntries = value;
		}

		public override string ToString()
		{
			return this.m_SteamLeaderboardEntries.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SteamLeaderboardEntries_t && this == (global::Steamworks.SteamLeaderboardEntries_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SteamLeaderboardEntries.GetHashCode();
		}

		public bool Equals(global::Steamworks.SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;
		}

		public int CompareTo(global::Steamworks.SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
		}

		public static bool operator ==(global::Steamworks.SteamLeaderboardEntries_t x, global::Steamworks.SteamLeaderboardEntries_t y)
		{
			return x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;
		}

		public static bool operator !=(global::Steamworks.SteamLeaderboardEntries_t x, global::Steamworks.SteamLeaderboardEntries_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SteamLeaderboardEntries_t(ulong value)
		{
			return new global::Steamworks.SteamLeaderboardEntries_t(value);
		}

		public static explicit operator ulong(global::Steamworks.SteamLeaderboardEntries_t that)
		{
			return that.m_SteamLeaderboardEntries;
		}

		public ulong m_SteamLeaderboardEntries;
	}
}
