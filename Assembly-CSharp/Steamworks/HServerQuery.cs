using System;

namespace Steamworks
{
	public struct HServerQuery : global::System.IEquatable<global::Steamworks.HServerQuery>, global::System.IComparable<global::Steamworks.HServerQuery>
	{
		public HServerQuery(int value)
		{
			this.m_HServerQuery = value;
		}

		public override string ToString()
		{
			return this.m_HServerQuery.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HServerQuery && this == (global::Steamworks.HServerQuery)other;
		}

		public override int GetHashCode()
		{
			return this.m_HServerQuery.GetHashCode();
		}

		public bool Equals(global::Steamworks.HServerQuery other)
		{
			return this.m_HServerQuery == other.m_HServerQuery;
		}

		public int CompareTo(global::Steamworks.HServerQuery other)
		{
			return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
		}

		public static bool operator ==(global::Steamworks.HServerQuery x, global::Steamworks.HServerQuery y)
		{
			return x.m_HServerQuery == y.m_HServerQuery;
		}

		public static bool operator !=(global::Steamworks.HServerQuery x, global::Steamworks.HServerQuery y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HServerQuery(int value)
		{
			return new global::Steamworks.HServerQuery(value);
		}

		public static explicit operator int(global::Steamworks.HServerQuery that)
		{
			return that.m_HServerQuery;
		}

		public static readonly global::Steamworks.HServerQuery Invalid = new global::Steamworks.HServerQuery(-1);

		public int m_HServerQuery;
	}
}
