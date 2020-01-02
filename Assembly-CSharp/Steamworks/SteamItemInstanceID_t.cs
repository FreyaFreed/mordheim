using System;

namespace Steamworks
{
	public struct SteamItemInstanceID_t : global::System.IEquatable<global::Steamworks.SteamItemInstanceID_t>, global::System.IComparable<global::Steamworks.SteamItemInstanceID_t>
	{
		public SteamItemInstanceID_t(ulong value)
		{
			this.m_SteamItemInstanceID = value;
		}

		public override string ToString()
		{
			return this.m_SteamItemInstanceID.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SteamItemInstanceID_t && this == (global::Steamworks.SteamItemInstanceID_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SteamItemInstanceID.GetHashCode();
		}

		public bool Equals(global::Steamworks.SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;
		}

		public int CompareTo(global::Steamworks.SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
		}

		public static bool operator ==(global::Steamworks.SteamItemInstanceID_t x, global::Steamworks.SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;
		}

		public static bool operator !=(global::Steamworks.SteamItemInstanceID_t x, global::Steamworks.SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SteamItemInstanceID_t(ulong value)
		{
			return new global::Steamworks.SteamItemInstanceID_t(value);
		}

		public static explicit operator ulong(global::Steamworks.SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID;
		}

		public static readonly global::Steamworks.SteamItemInstanceID_t Invalid = new global::Steamworks.SteamItemInstanceID_t(ulong.MaxValue);

		public ulong m_SteamItemInstanceID;
	}
}
