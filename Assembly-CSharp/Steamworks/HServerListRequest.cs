using System;

namespace Steamworks
{
	public struct HServerListRequest : global::System.IEquatable<global::Steamworks.HServerListRequest>
	{
		public HServerListRequest(global::System.IntPtr value)
		{
			this.m_HServerListRequest = value;
		}

		public override string ToString()
		{
			return this.m_HServerListRequest.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.HServerListRequest && this == (global::Steamworks.HServerListRequest)other;
		}

		public override int GetHashCode()
		{
			return this.m_HServerListRequest.GetHashCode();
		}

		public bool Equals(global::Steamworks.HServerListRequest other)
		{
			return this.m_HServerListRequest == other.m_HServerListRequest;
		}

		public static bool operator ==(global::Steamworks.HServerListRequest x, global::Steamworks.HServerListRequest y)
		{
			return x.m_HServerListRequest == y.m_HServerListRequest;
		}

		public static bool operator !=(global::Steamworks.HServerListRequest x, global::Steamworks.HServerListRequest y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.HServerListRequest(global::System.IntPtr value)
		{
			return new global::Steamworks.HServerListRequest(value);
		}

		public static explicit operator global::System.IntPtr(global::Steamworks.HServerListRequest that)
		{
			return that.m_HServerListRequest;
		}

		public static readonly global::Steamworks.HServerListRequest Invalid = new global::Steamworks.HServerListRequest(global::System.IntPtr.Zero);

		public global::System.IntPtr m_HServerListRequest;
	}
}
