using System;

namespace Steamworks
{
	public struct ScreenshotHandle : global::System.IEquatable<global::Steamworks.ScreenshotHandle>, global::System.IComparable<global::Steamworks.ScreenshotHandle>
	{
		public ScreenshotHandle(uint value)
		{
			this.m_ScreenshotHandle = value;
		}

		public override string ToString()
		{
			return this.m_ScreenshotHandle.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.ScreenshotHandle && this == (global::Steamworks.ScreenshotHandle)other;
		}

		public override int GetHashCode()
		{
			return this.m_ScreenshotHandle.GetHashCode();
		}

		public bool Equals(global::Steamworks.ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle == other.m_ScreenshotHandle;
		}

		public int CompareTo(global::Steamworks.ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
		}

		public static bool operator ==(global::Steamworks.ScreenshotHandle x, global::Steamworks.ScreenshotHandle y)
		{
			return x.m_ScreenshotHandle == y.m_ScreenshotHandle;
		}

		public static bool operator !=(global::Steamworks.ScreenshotHandle x, global::Steamworks.ScreenshotHandle y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.ScreenshotHandle(uint value)
		{
			return new global::Steamworks.ScreenshotHandle(value);
		}

		public static explicit operator uint(global::Steamworks.ScreenshotHandle that)
		{
			return that.m_ScreenshotHandle;
		}

		public static readonly global::Steamworks.ScreenshotHandle Invalid = new global::Steamworks.ScreenshotHandle(0U);

		public uint m_ScreenshotHandle;
	}
}
