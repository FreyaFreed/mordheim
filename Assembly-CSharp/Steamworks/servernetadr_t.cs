﻿using System;

namespace Steamworks
{
	public struct servernetadr_t
	{
		public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
		{
			this.m_unIP = ip;
			this.m_usQueryPort = usQueryPort;
			this.m_usConnectionPort = usConnectionPort;
		}

		public ushort GetQueryPort()
		{
			return this.m_usQueryPort;
		}

		public void SetQueryPort(ushort usPort)
		{
			this.m_usQueryPort = usPort;
		}

		public ushort GetConnectionPort()
		{
			return this.m_usConnectionPort;
		}

		public void SetConnectionPort(ushort usPort)
		{
			this.m_usConnectionPort = usPort;
		}

		public uint GetIP()
		{
			return this.m_unIP;
		}

		public void SetIP(uint unIP)
		{
			this.m_unIP = unIP;
		}

		public string GetConnectionAddressString()
		{
			return global::Steamworks.servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
		}

		public string GetQueryAddressString()
		{
			return global::Steamworks.servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
		}

		public static string ToString(uint unIP, ushort usPort)
		{
			return string.Format("{0}.{1}.{2}.{3}:{4}", new object[]
			{
				(ulong)(unIP >> 24) & 255UL,
				(ulong)(unIP >> 16) & 255UL,
				(ulong)(unIP >> 8) & 255UL,
				(ulong)unIP & 255UL,
				usPort
			});
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.servernetadr_t && this == (global::Steamworks.servernetadr_t)other;
		}

		public override int GetHashCode()
		{
			return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
		}

		public bool Equals(global::Steamworks.servernetadr_t other)
		{
			return this.m_unIP == other.m_unIP && this.m_usQueryPort == other.m_usQueryPort && this.m_usConnectionPort == other.m_usConnectionPort;
		}

		public int CompareTo(global::Steamworks.servernetadr_t other)
		{
			return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
		}

		public static bool operator <(global::Steamworks.servernetadr_t x, global::Steamworks.servernetadr_t y)
		{
			return x.m_unIP < y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort);
		}

		public static bool operator >(global::Steamworks.servernetadr_t x, global::Steamworks.servernetadr_t y)
		{
			return x.m_unIP > y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort);
		}

		public static bool operator ==(global::Steamworks.servernetadr_t x, global::Steamworks.servernetadr_t y)
		{
			return x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;
		}

		public static bool operator !=(global::Steamworks.servernetadr_t x, global::Steamworks.servernetadr_t y)
		{
			return !(x == y);
		}

		private ushort m_usConnectionPort;

		private ushort m_usQueryPort;

		private uint m_unIP;
	}
}
