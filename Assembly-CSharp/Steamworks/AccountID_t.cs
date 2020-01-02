using System;

namespace Steamworks
{
	public struct AccountID_t : global::System.IEquatable<global::Steamworks.AccountID_t>, global::System.IComparable<global::Steamworks.AccountID_t>
	{
		public AccountID_t(uint value)
		{
			this.m_AccountID = value;
		}

		public override string ToString()
		{
			return this.m_AccountID.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.AccountID_t && this == (global::Steamworks.AccountID_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_AccountID.GetHashCode();
		}

		public bool Equals(global::Steamworks.AccountID_t other)
		{
			return this.m_AccountID == other.m_AccountID;
		}

		public int CompareTo(global::Steamworks.AccountID_t other)
		{
			return this.m_AccountID.CompareTo(other.m_AccountID);
		}

		public static bool operator ==(global::Steamworks.AccountID_t x, global::Steamworks.AccountID_t y)
		{
			return x.m_AccountID == y.m_AccountID;
		}

		public static bool operator !=(global::Steamworks.AccountID_t x, global::Steamworks.AccountID_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.AccountID_t(uint value)
		{
			return new global::Steamworks.AccountID_t(value);
		}

		public static explicit operator uint(global::Steamworks.AccountID_t that)
		{
			return that.m_AccountID;
		}

		public uint m_AccountID;
	}
}
