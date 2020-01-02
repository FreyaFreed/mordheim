using System;

namespace Steamworks
{
	public struct FriendsGroupID_t : global::System.IEquatable<global::Steamworks.FriendsGroupID_t>, global::System.IComparable<global::Steamworks.FriendsGroupID_t>
	{
		public FriendsGroupID_t(short value)
		{
			this.m_FriendsGroupID = value;
		}

		public override string ToString()
		{
			return this.m_FriendsGroupID.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.FriendsGroupID_t && this == (global::Steamworks.FriendsGroupID_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_FriendsGroupID.GetHashCode();
		}

		public bool Equals(global::Steamworks.FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID == other.m_FriendsGroupID;
		}

		public int CompareTo(global::Steamworks.FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
		}

		public static bool operator ==(global::Steamworks.FriendsGroupID_t x, global::Steamworks.FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID == y.m_FriendsGroupID;
		}

		public static bool operator !=(global::Steamworks.FriendsGroupID_t x, global::Steamworks.FriendsGroupID_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.FriendsGroupID_t(short value)
		{
			return new global::Steamworks.FriendsGroupID_t(value);
		}

		public static explicit operator short(global::Steamworks.FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID;
		}

		public static readonly global::Steamworks.FriendsGroupID_t Invalid = new global::Steamworks.FriendsGroupID_t(-1);

		public short m_FriendsGroupID;
	}
}
