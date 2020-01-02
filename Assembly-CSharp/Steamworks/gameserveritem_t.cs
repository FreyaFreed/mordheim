using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4, Size = 372)]
	public class gameserveritem_t
	{
		public string GetGameDir()
		{
			return global::System.Text.Encoding.UTF8.GetString(this.m_szGameDir, 0, global::System.Array.IndexOf<byte>(this.m_szGameDir, 0));
		}

		public void SetGameDir(string dir)
		{
			this.m_szGameDir = global::System.Text.Encoding.UTF8.GetBytes(dir + '\0');
		}

		public string GetMap()
		{
			return global::System.Text.Encoding.UTF8.GetString(this.m_szMap, 0, global::System.Array.IndexOf<byte>(this.m_szMap, 0));
		}

		public void SetMap(string map)
		{
			this.m_szMap = global::System.Text.Encoding.UTF8.GetBytes(map + '\0');
		}

		public string GetGameDescription()
		{
			return global::System.Text.Encoding.UTF8.GetString(this.m_szGameDescription, 0, global::System.Array.IndexOf<byte>(this.m_szGameDescription, 0));
		}

		public void SetGameDescription(string desc)
		{
			this.m_szGameDescription = global::System.Text.Encoding.UTF8.GetBytes(desc + '\0');
		}

		public string GetServerName()
		{
			if (this.m_szServerName[0] == 0)
			{
				return this.m_NetAdr.GetConnectionAddressString();
			}
			return global::System.Text.Encoding.UTF8.GetString(this.m_szServerName, 0, global::System.Array.IndexOf<byte>(this.m_szServerName, 0));
		}

		public void SetServerName(string name)
		{
			this.m_szServerName = global::System.Text.Encoding.UTF8.GetBytes(name + '\0');
		}

		public string GetGameTags()
		{
			return global::System.Text.Encoding.UTF8.GetString(this.m_szGameTags, 0, global::System.Array.IndexOf<byte>(this.m_szGameTags, 0));
		}

		public void SetGameTags(string tags)
		{
			this.m_szGameTags = global::System.Text.Encoding.UTF8.GetBytes(tags + '\0');
		}

		public global::Steamworks.servernetadr_t m_NetAdr;

		public int m_nPing;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bHadSuccessfulResponse;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bDoNotRefresh;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szGameDir;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szMap;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szGameDescription;

		public uint m_nAppID;

		public int m_nPlayers;

		public int m_nMaxPlayers;

		public int m_nBotPlayers;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bPassword;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public bool m_bSecure;

		public uint m_ulTimeLastPlayed;

		public int m_nServerVersion;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szServerName;

		[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szGameTags;

		public global::Steamworks.CSteamID m_steamID;
	}
}
