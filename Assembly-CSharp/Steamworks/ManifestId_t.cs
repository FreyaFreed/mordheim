using System;

namespace Steamworks
{
	public struct ManifestId_t : global::System.IEquatable<global::Steamworks.ManifestId_t>, global::System.IComparable<global::Steamworks.ManifestId_t>
	{
		public ManifestId_t(ulong value)
		{
			this.m_ManifestId = value;
		}

		public override string ToString()
		{
			return this.m_ManifestId.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ManifestId_t && this == (global::Steamworks.ManifestId_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_ManifestId.GetHashCode();
		}

		public bool Equals(global::Steamworks.ManifestId_t other)
		{
			return this.m_ManifestId == other.m_ManifestId;
		}

		public int CompareTo(global::Steamworks.ManifestId_t other)
		{
			return this.m_ManifestId.CompareTo(other.m_ManifestId);
		}

		public static bool operator ==(global::Steamworks.ManifestId_t x, global::Steamworks.ManifestId_t y)
		{
			return x.m_ManifestId == y.m_ManifestId;
		}

		public static bool operator !=(global::Steamworks.ManifestId_t x, global::Steamworks.ManifestId_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ManifestId_t(ulong value)
		{
			return new global::Steamworks.ManifestId_t(value);
		}

		public static explicit operator ulong(global::Steamworks.ManifestId_t that)
		{
			return that.m_ManifestId;
		}

		public static readonly global::Steamworks.ManifestId_t Invalid = new global::Steamworks.ManifestId_t(0UL);

		public ulong m_ManifestId;
	}
}
