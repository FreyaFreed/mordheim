using System;

namespace Steamworks
{
	public struct ControllerHandle_t : global::System.IEquatable<global::Steamworks.ControllerHandle_t>, global::System.IComparable<global::Steamworks.ControllerHandle_t>
	{
		public ControllerHandle_t(ulong value)
		{
			this.m_ControllerHandle = value;
		}

		public override string ToString()
		{
			return this.m_ControllerHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ControllerHandle_t && this == (global::Steamworks.ControllerHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_ControllerHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.ControllerHandle_t other)
		{
			return this.m_ControllerHandle == other.m_ControllerHandle;
		}

		public int CompareTo(global::Steamworks.ControllerHandle_t other)
		{
			return this.m_ControllerHandle.CompareTo(other.m_ControllerHandle);
		}

		public static bool operator ==(global::Steamworks.ControllerHandle_t x, global::Steamworks.ControllerHandle_t y)
		{
			return x.m_ControllerHandle == y.m_ControllerHandle;
		}

		public static bool operator !=(global::Steamworks.ControllerHandle_t x, global::Steamworks.ControllerHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ControllerHandle_t(ulong value)
		{
			return new global::Steamworks.ControllerHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.ControllerHandle_t that)
		{
			return that.m_ControllerHandle;
		}

		public ulong m_ControllerHandle;
	}
}
