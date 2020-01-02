using System;

namespace Steamworks
{
	public struct SteamInventoryResult_t : global::System.IEquatable<global::Steamworks.SteamInventoryResult_t>, global::System.IComparable<global::Steamworks.SteamInventoryResult_t>
	{
		public SteamInventoryResult_t(int value)
		{
			this.m_SteamInventoryResult = value;
		}

		public override string ToString()
		{
			return this.m_SteamInventoryResult.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.SteamInventoryResult_t && this == (global::Steamworks.SteamInventoryResult_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_SteamInventoryResult.GetHashCode();
		}

		public bool Equals(global::Steamworks.SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
		}

		public int CompareTo(global::Steamworks.SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
		}

		public static bool operator ==(global::Steamworks.SteamInventoryResult_t x, global::Steamworks.SteamInventoryResult_t y)
		{
			return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
		}

		public static bool operator !=(global::Steamworks.SteamInventoryResult_t x, global::Steamworks.SteamInventoryResult_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.SteamInventoryResult_t(int value)
		{
			return new global::Steamworks.SteamInventoryResult_t(value);
		}

		public static explicit operator int(global::Steamworks.SteamInventoryResult_t that)
		{
			return that.m_SteamInventoryResult;
		}

		public static readonly global::Steamworks.SteamInventoryResult_t Invalid = new global::Steamworks.SteamInventoryResult_t(-1);

		public int m_SteamInventoryResult;
	}
}
