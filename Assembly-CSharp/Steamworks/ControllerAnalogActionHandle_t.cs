using System;

namespace Steamworks
{
	public struct ControllerAnalogActionHandle_t : global::System.IEquatable<global::Steamworks.ControllerAnalogActionHandle_t>, global::System.IComparable<global::Steamworks.ControllerAnalogActionHandle_t>
	{
		public ControllerAnalogActionHandle_t(ulong value)
		{
			this.m_ControllerAnalogActionHandle = value;
		}

		public override string ToString()
		{
			return this.m_ControllerAnalogActionHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ControllerAnalogActionHandle_t && this == (global::Steamworks.ControllerAnalogActionHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_ControllerAnalogActionHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.ControllerAnalogActionHandle_t other)
		{
			return this.m_ControllerAnalogActionHandle == other.m_ControllerAnalogActionHandle;
		}

		public int CompareTo(global::Steamworks.ControllerAnalogActionHandle_t other)
		{
			return this.m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);
		}

		public static bool operator ==(global::Steamworks.ControllerAnalogActionHandle_t x, global::Steamworks.ControllerAnalogActionHandle_t y)
		{
			return x.m_ControllerAnalogActionHandle == y.m_ControllerAnalogActionHandle;
		}

		public static bool operator !=(global::Steamworks.ControllerAnalogActionHandle_t x, global::Steamworks.ControllerAnalogActionHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ControllerAnalogActionHandle_t(ulong value)
		{
			return new global::Steamworks.ControllerAnalogActionHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.ControllerAnalogActionHandle_t that)
		{
			return that.m_ControllerAnalogActionHandle;
		}

		public ulong m_ControllerAnalogActionHandle;
	}
}
