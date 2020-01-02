using System;

namespace Steamworks
{
	public struct SteamItemDef_t : global::System.IEquatable<global::Steamworks.SteamItemDef_t>, global::System.IComparable<global::Steamworks.SteamItemDef_t>
	{
		public SteamItemDef_t(int value)
		{
			this.m_SteamItemDef = value;
		}

		public override string ToString()
		{
			return this.m_SteamItemDef.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SteamItemDef_t && this == (global::Steamworks.SteamItemDef_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SteamItemDef.GetHashCode();
		}

		public bool Equals(global::Steamworks.SteamItemDef_t other)
		{
			return this.m_SteamItemDef == other.m_SteamItemDef;
		}

		public int CompareTo(global::Steamworks.SteamItemDef_t other)
		{
			return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
		}

		public static bool operator ==(global::Steamworks.SteamItemDef_t x, global::Steamworks.SteamItemDef_t y)
		{
			return x.m_SteamItemDef == y.m_SteamItemDef;
		}

		public static bool operator !=(global::Steamworks.SteamItemDef_t x, global::Steamworks.SteamItemDef_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SteamItemDef_t(int value)
		{
			return new global::Steamworks.SteamItemDef_t(value);
		}

		public static explicit operator int(global::Steamworks.SteamItemDef_t that)
		{
			return that.m_SteamItemDef;
		}

		public int m_SteamItemDef;
	}
}
