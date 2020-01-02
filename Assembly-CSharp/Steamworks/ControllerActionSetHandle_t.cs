using System;

namespace Steamworks
{
	public struct ControllerActionSetHandle_t : global::System.IEquatable<global::Steamworks.ControllerActionSetHandle_t>, global::System.IComparable<global::Steamworks.ControllerActionSetHandle_t>
	{
		public ControllerActionSetHandle_t(ulong value)
		{
			this.m_ControllerActionSetHandle = value;
		}

		public override string ToString()
		{
			return this.m_ControllerActionSetHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ControllerActionSetHandle_t && this == (global::Steamworks.ControllerActionSetHandle_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_ControllerActionSetHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.ControllerActionSetHandle_t other)
		{
			return this.m_ControllerActionSetHandle == other.m_ControllerActionSetHandle;
		}

		public int CompareTo(global::Steamworks.ControllerActionSetHandle_t other)
		{
			return this.m_ControllerActionSetHandle.CompareTo(other.m_ControllerActionSetHandle);
		}

		public static bool operator ==(global::Steamworks.ControllerActionSetHandle_t x, global::Steamworks.ControllerActionSetHandle_t y)
		{
			return x.m_ControllerActionSetHandle == y.m_ControllerActionSetHandle;
		}

		public static bool operator !=(global::Steamworks.ControllerActionSetHandle_t x, global::Steamworks.ControllerActionSetHandle_t y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ControllerActionSetHandle_t(ulong value)
		{
			return new global::Steamworks.ControllerActionSetHandle_t(value);
		}

		public static explicit operator ulong(global::Steamworks.ControllerActionSetHandle_t that)
		{
			return that.m_ControllerActionSetHandle;
		}

		public ulong m_ControllerActionSetHandle;
	}
}
