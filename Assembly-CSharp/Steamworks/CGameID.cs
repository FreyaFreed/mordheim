using System;

namespace Steamworks
{
	public struct CGameID : global::System.IEquatable<global::Steamworks.CGameID>, global::System.IComparable<global::Steamworks.CGameID>
	{
		public CGameID(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		public CGameID(global::Steamworks.AppId_t nAppID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
		}

		public CGameID(global::Steamworks.AppId_t nAppID, uint nModID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
			this.SetType(global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeGameMod);
			this.SetModID(nModID);
		}

		public bool IsSteamApp()
		{
			return this.Type() == global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeApp;
		}

		public bool IsMod()
		{
			return this.Type() == global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeGameMod;
		}

		public bool IsShortcut()
		{
			return this.Type() == global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeShortcut;
		}

		public bool IsP2PFile()
		{
			return this.Type() == global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeP2P;
		}

		public global::Steamworks.AppId_t AppID()
		{
			return new global::Steamworks.AppId_t((uint)(this.m_GameID & 16777215UL));
		}

		public global::Steamworks.CGameID.EGameIDType Type()
		{
			return (global::Steamworks.CGameID.EGameIDType)(this.m_GameID >> 24 & 255UL);
		}

		public uint ModID()
		{
			return (uint)(this.m_GameID >> 32 & (ulong)-1);
		}

		public bool IsValid()
		{
			switch (this.Type())
			{
			case global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeApp:
				return this.AppID() != global::Steamworks.AppId_t.Invalid;
			case global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeGameMod:
				return this.AppID() != global::Steamworks.AppId_t.Invalid && (this.ModID() & 2147483648U) != 0U;
			case global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeShortcut:
				return (this.ModID() & 2147483648U) != 0U;
			case global::Steamworks.CGameID.EGameIDType.k_EGameIDTypeP2P:
				return this.AppID() == global::Steamworks.AppId_t.Invalid && (this.ModID() & 2147483648U) != 0U;
			default:
				return false;
			}
		}

		public void Reset()
		{
			this.m_GameID = 0UL;
		}

		public void Set(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		private void SetAppID(global::Steamworks.AppId_t other)
		{
			this.m_GameID = ((this.m_GameID & 18446744073692774400UL) | ((ulong)((uint)other) & 16777215UL));
		}

		private void SetType(global::Steamworks.CGameID.EGameIDType other)
		{
			this.m_GameID = ((this.m_GameID & 18446744069431361535UL) | (ulong)((ulong)((long)other & 255L) << 24));
		}

		private void SetModID(uint other)
		{
			this.m_GameID = ((this.m_GameID & (ulong)-1) | ((ulong)other & (ulong)-1) << 32);
		}

		public override string ToString()
		{
			return this.m_GameID.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.CGameID && this == (global::Steamworks.CGameID)other;
		}

		public override int GetHashCode()
		{
			return this.m_GameID.GetHashCode();
		}

		public bool Equals(global::Steamworks.CGameID other)
		{
			return this.m_GameID == other.m_GameID;
		}

		public int CompareTo(global::Steamworks.CGameID other)
		{
			return this.m_GameID.CompareTo(other.m_GameID);
		}

		public static bool operator ==(global::Steamworks.CGameID x, global::Steamworks.CGameID y)
		{
			return x.m_GameID == y.m_GameID;
		}

		public static bool operator !=(global::Steamworks.CGameID x, global::Steamworks.CGameID y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.CGameID(ulong value)
		{
			return new global::Steamworks.CGameID(value);
		}

		public static explicit operator ulong(global::Steamworks.CGameID that)
		{
			return that.m_GameID;
		}

		public ulong m_GameID;

		public enum EGameIDType
		{
			k_EGameIDTypeApp,
			k_EGameIDTypeGameMod,
			k_EGameIDTypeShortcut,
			k_EGameIDTypeP2P
		}
	}
}
