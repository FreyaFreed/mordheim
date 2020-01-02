using System;

namespace Steamworks
{
	public struct HSteamPipe : global::System.IEquatable<global::Steamworks.HSteamPipe>, global::System.IComparable<global::Steamworks.HSteamPipe>
	{
		public HSteamPipe(int value)
		{
			this.m_HSteamPipe = value;
		}

		public override string ToString()
		{
			return this.m_HSteamPipe.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HSteamPipe && this == (global::Steamworks.HSteamPipe)other;
		}

		public override int GetHashCode()
		{
			return this.m_HSteamPipe.GetHashCode();
		}

		public bool Equals(global::Steamworks.HSteamPipe other)
		{
			return this.m_HSteamPipe == other.m_HSteamPipe;
		}

		public int CompareTo(global::Steamworks.HSteamPipe other)
		{
			return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
		}

		public static bool operator ==(global::Steamworks.HSteamPipe x, global::Steamworks.HSteamPipe y)
		{
			return x.m_HSteamPipe == y.m_HSteamPipe;
		}

		public static bool operator !=(global::Steamworks.HSteamPipe x, global::Steamworks.HSteamPipe y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HSteamPipe(int value)
		{
			return new global::Steamworks.HSteamPipe(value);
		}

		public static explicit operator int(global::Steamworks.HSteamPipe that)
		{
			return that.m_HSteamPipe;
		}

		public int m_HSteamPipe;
	}
}
