using System;

namespace Steamworks
{
	public struct ControllerDigitalActionHandle_t : global::System.IEquatable<global::Steamworks.ControllerDigitalActionHandle_t>, global::System.IComparable<global::Steamworks.ControllerDigitalActionHandle_t>
	{
		public ControllerDigitalActionHandle_t(ulong value)
		{
			this.m_ControllerDigitalActionHandle = value;
		}

		public override string ToString()
		{
			return this.m_ControllerDigitalActionHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ControllerDigitalActionHandle_t && this == (global::Steamworks.ControllerDigitalActionHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_ControllerDigitalActionHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.ControllerDigitalActionHandle_t other)
		{
			return this.m_ControllerDigitalActionHandle == other.m_ControllerDigitalActionHandle;
		}

		public int CompareTo(global::Steamworks.ControllerDigitalActionHandle_t other)
		{
			return this.m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);
		}

		public static bool operator ==(global::Steamworks.ControllerDigitalActionHandle_t x, global::Steamworks.ControllerDigitalActionHandle_t y)
		{
			return x.m_ControllerDigitalActionHandle == y.m_ControllerDigitalActionHandle;
		}

		public static bool operator !=(global::Steamworks.ControllerDigitalActionHandle_t x, global::Steamworks.ControllerDigitalActionHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ControllerDigitalActionHandle_t(ulong value)
		{
			return new global::Steamworks.ControllerDigitalActionHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.ControllerDigitalActionHandle_t that)
		{
			return that.m_ControllerDigitalActionHandle;
		}

		public ulong m_ControllerDigitalActionHandle;
	}
}
