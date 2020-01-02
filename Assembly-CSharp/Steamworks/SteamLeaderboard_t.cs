using System;

namespace Steamworks
{
	public struct SteamLeaderboard_t : global::System.IEquatable<global::Steamworks.SteamLeaderboard_t>, global::System.IComparable<global::Steamworks.SteamLeaderboard_t>
	{
		public SteamLeaderboard_t(ulong value)
		{
			this.m_SteamLeaderboard = value;
		}

		public override string ToString()
		{
			return this.m_SteamLeaderboard.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SteamLeaderboard_t && this == (global::Steamworks.SteamLeaderboard_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SteamLeaderboard.GetHashCode();
		}

		public bool Equals(global::Steamworks.SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard == other.m_SteamLeaderboard;
		}

		public int CompareTo(global::Steamworks.SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
		}

		public static bool operator ==(global::Steamworks.SteamLeaderboard_t x, global::Steamworks.SteamLeaderboard_t y)
		{
			return x.m_SteamLeaderboard == y.m_SteamLeaderboard;
		}

		public static bool operator !=(global::Steamworks.SteamLeaderboard_t x, global::Steamworks.SteamLeaderboard_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SteamLeaderboard_t(ulong value)
		{
			return new global::Steamworks.SteamLeaderboard_t(value);
		}

		public static explicit operator ulong(global::Steamworks.SteamLeaderboard_t that)
		{
			return that.m_SteamLeaderboard;
		}

		public ulong m_SteamLeaderboard;
	}
}
