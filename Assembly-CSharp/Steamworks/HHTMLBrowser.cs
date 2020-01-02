using System;

namespace Steamworks
{
	public struct HHTMLBrowser : global::System.IEquatable<global::Steamworks.HHTMLBrowser>, global::System.IComparable<global::Steamworks.HHTMLBrowser>
	{
		public HHTMLBrowser(uint value)
		{
			this.m_HHTMLBrowser = value;
		}

		public override string ToString()
		{
			return this.m_HHTMLBrowser.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HHTMLBrowser && this == (global::Steamworks.HHTMLBrowser)other;
		}

		public override int GetHashCode()
		{
			return this.m_HHTMLBrowser.GetHashCode();
		}

		public bool Equals(global::Steamworks.HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser == other.m_HHTMLBrowser;
		}

		public int CompareTo(global::Steamworks.HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
		}

		public static bool operator ==(global::Steamworks.HHTMLBrowser x, global::Steamworks.HHTMLBrowser y)
		{
			return x.m_HHTMLBrowser == y.m_HHTMLBrowser;
		}

		public static bool operator !=(global::Steamworks.HHTMLBrowser x, global::Steamworks.HHTMLBrowser y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HHTMLBrowser(uint value)
		{
			return new global::Steamworks.HHTMLBrowser(value);
		}

		public static explicit operator uint(global::Steamworks.HHTMLBrowser that)
		{
			return that.m_HHTMLBrowser;
		}

		public static readonly global::Steamworks.HHTMLBrowser Invalid = new global::Steamworks.HHTMLBrowser(0U);

		public uint m_HHTMLBrowser;
	}
}
