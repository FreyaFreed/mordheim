using System;

namespace Steamworks
{
	public struct CSteamID : global::System.IEquatable<global::Steamworks.CSteamID>, global::System.IComparable<global::Steamworks.CSteamID>
	{
		public CSteamID(global::Steamworks.AccountID_t unAccountID, global::Steamworks.EUniverse eUniverse, global::Steamworks.EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.Set(unAccountID, eUniverse, eAccountType);
		}

		public CSteamID(global::Steamworks.AccountID_t unAccountID, uint unAccountInstance, global::Steamworks.EUniverse eUniverse, global::Steamworks.EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
		}

		public CSteamID(ulong ulSteamID)
		{
			this.m_SteamID = ulSteamID;
		}

		public void Set(global::Steamworks.AccountID_t unAccountID, global::Steamworks.EUniverse eUniverse, global::Steamworks.EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			if (eAccountType == global::Steamworks.EAccountType.k_EAccountTypeClan || eAccountType == global::Steamworks.EAccountType.k_EAccountTypeGameServer)
			{
				this.SetAccountInstance(0U);
			}
			else
			{
				this.SetAccountInstance(1U);
			}
		}

		public void InstancedSet(global::Steamworks.AccountID_t unAccountID, uint unInstance, global::Steamworks.EUniverse eUniverse, global::Steamworks.EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			this.SetAccountInstance(unInstance);
		}

		public void Clear()
		{
			this.m_SteamID = 0UL;
		}

		public void CreateBlankAnonLogon(global::Steamworks.EUniverse eUniverse)
		{
			this.SetAccountID(new global::Steamworks.AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(global::Steamworks.EAccountType.k_EAccountTypeAnonGameServer);
			this.SetAccountInstance(0U);
		}

		public void CreateBlankAnonUserLogon(global::Steamworks.EUniverse eUniverse)
		{
			this.SetAccountID(new global::Steamworks.AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(global::Steamworks.EAccountType.k_EAccountTypeAnonUser);
			this.SetAccountInstance(0U);
		}

		public bool BBlankAnonAccount()
		{
			return this.GetAccountID() == new global::Steamworks.AccountID_t(0U) && this.BAnonAccount() && this.GetUnAccountInstance() == 0U;
		}

		public bool BGameServerAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeAnonGameServer;
		}

		public bool BPersistentGameServerAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeGameServer;
		}

		public bool BAnonGameServerAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeAnonGameServer;
		}

		public bool BContentServerAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeContentServer;
		}

		public bool BClanAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeClan;
		}

		public bool BChatAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeChat;
		}

		public bool IsLobby()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeChat && (this.GetUnAccountInstance() & 262144U) != 0U;
		}

		public bool BIndividualAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeConsoleUser;
		}

		public bool BAnonAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeAnonGameServer;
		}

		public bool BAnonUserAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeAnonUser;
		}

		public bool BConsoleUserAccount()
		{
			return this.GetEAccountType() == global::Steamworks.EAccountType.k_EAccountTypeConsoleUser;
		}

		public void SetAccountID(global::Steamworks.AccountID_t other)
		{
			this.m_SteamID = ((this.m_SteamID & 18446744069414584320UL) | ((ulong)((uint)other) & (ulong)-1));
		}

		public void SetAccountInstance(uint other)
		{
			this.m_SteamID = ((this.m_SteamID & 18442240478377148415UL) | ((ulong)other & 1048575UL) << 32);
		}

		public void SetEAccountType(global::Steamworks.EAccountType other)
		{
			this.m_SteamID = ((this.m_SteamID & 18379190079298994175UL) | (ulong)((ulong)((long)other & 15L) << 52));
		}

		public void SetEUniverse(global::Steamworks.EUniverse other)
		{
			this.m_SteamID = ((this.m_SteamID & 72057594037927935UL) | (ulong)((ulong)((long)other & 255L) << 56));
		}

		public void ClearIndividualInstance()
		{
			if (this.BIndividualAccount())
			{
				this.SetAccountInstance(0U);
			}
		}

		public bool HasNoIndividualInstance()
		{
			return this.BIndividualAccount() && this.GetUnAccountInstance() == 0U;
		}

		public global::Steamworks.AccountID_t GetAccountID()
		{
			return new global::Steamworks.AccountID_t((uint)(this.m_SteamID & (ulong)-1));
		}

		public uint GetUnAccountInstance()
		{
			return (uint)(this.m_SteamID >> 32 & 1048575UL);
		}

		public global::Steamworks.EAccountType GetEAccountType()
		{
			return (global::Steamworks.EAccountType)(this.m_SteamID >> 52 & 15UL);
		}

		public global::Steamworks.EUniverse GetEUniverse()
		{
			return (global::Steamworks.EUniverse)(this.m_SteamID >> 56 & 255UL);
		}

		public bool IsValid()
		{
			return this.GetEAccountType() > global::Steamworks.EAccountType.k_EAccountTypeInvalid && this.GetEAccountType() < global::Steamworks.EAccountType.k_EAccountTypeMax && this.GetEUniverse() > global::Steamworks.EUniverse.k_EUniverseInvalid && this.GetEUniverse() < global::Steamworks.EUniverse.k_EUniverseMax && (this.GetEAccountType() != global::Steamworks.EAccountType.k_EAccountTypeIndividual || (!(this.GetAccountID() == new global::Steamworks.AccountID_t(0U)) && this.GetUnAccountInstance() <= 4U)) && (this.GetEAccountType() != global::Steamworks.EAccountType.k_EAccountTypeClan || (!(this.GetAccountID() == new global::Steamworks.AccountID_t(0U)) && this.GetUnAccountInstance() == 0U)) && (this.GetEAccountType() != global::Steamworks.EAccountType.k_EAccountTypeGameServer || !(this.GetAccountID() == new global::Steamworks.AccountID_t(0U)));
		}

		public override string ToString()
		{
			return this.m_SteamID.ToString();
		}

		public override bool Equals(object other)
		{
			return other is global::Steamworks.CSteamID && this == (global::Steamworks.CSteamID)other;
		}

		public override int GetHashCode()
		{
			return this.m_SteamID.GetHashCode();
		}

		public bool Equals(global::Steamworks.CSteamID other)
		{
			return this.m_SteamID == other.m_SteamID;
		}

		public int CompareTo(global::Steamworks.CSteamID other)
		{
			return this.m_SteamID.CompareTo(other.m_SteamID);
		}

		public static bool operator ==(global::Steamworks.CSteamID x, global::Steamworks.CSteamID y)
		{
			return x.m_SteamID == y.m_SteamID;
		}

		public static bool operator !=(global::Steamworks.CSteamID x, global::Steamworks.CSteamID y)
		{
			return !(x == y);
		}

		public static explicit operator global::Steamworks.CSteamID(ulong value)
		{
			return new global::Steamworks.CSteamID(value);
		}

		public static explicit operator ulong(global::Steamworks.CSteamID that)
		{
			return that.m_SteamID;
		}

		public static readonly global::Steamworks.CSteamID Nil = default(global::Steamworks.CSteamID);

		public static readonly global::Steamworks.CSteamID OutofDateGS = new global::Steamworks.CSteamID(new global::Steamworks.AccountID_t(0U), 0U, global::Steamworks.EUniverse.k_EUniverseInvalid, global::Steamworks.EAccountType.k_EAccountTypeInvalid);

		public static readonly global::Steamworks.CSteamID LanModeGS = new global::Steamworks.CSteamID(new global::Steamworks.AccountID_t(0U), 0U, global::Steamworks.EUniverse.k_EUniversePublic, global::Steamworks.EAccountType.k_EAccountTypeInvalid);

		public static readonly global::Steamworks.CSteamID NotInitYetGS = new global::Steamworks.CSteamID(new global::Steamworks.AccountID_t(1U), 0U, global::Steamworks.EUniverse.k_EUniverseInvalid, global::Steamworks.EAccountType.k_EAccountTypeInvalid);

		public static readonly global::Steamworks.CSteamID NonSteamGS = new global::Steamworks.CSteamID(new global::Steamworks.AccountID_t(2U), 0U, global::Steamworks.EUniverse.k_EUniverseInvalid, global::Steamworks.EAccountType.k_EAccountTypeInvalid);

		public ulong m_SteamID;
	}
}
