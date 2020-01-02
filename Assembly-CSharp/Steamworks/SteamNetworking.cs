﻿using System;

namespace Steamworks
{
	public static class SteamNetworking
	{
		public static bool SendP2PPacket(global::Steamworks.CSteamID steamIDRemote, byte[] pubData, uint cubData, global::Steamworks.EP2PSend eP2PSendType, int nChannel = 0)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_SendP2PPacket(steamIDRemote, pubData, cubData, eP2PSendType, nChannel);
		}

		public static bool IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel = 0)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_IsP2PPacketAvailable(out pcubMsgSize, nChannel);
		}

		public static bool ReadP2PPacket(byte[] pubDest, uint cubDest, out uint pcubMsgSize, out global::Steamworks.CSteamID psteamIDRemote, int nChannel = 0)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_ReadP2PPacket(pubDest, cubDest, out pcubMsgSize, out psteamIDRemote, nChannel);
		}

		public static bool AcceptP2PSessionWithUser(global::Steamworks.CSteamID steamIDRemote)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_AcceptP2PSessionWithUser(steamIDRemote);
		}

		public static bool CloseP2PSessionWithUser(global::Steamworks.CSteamID steamIDRemote)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_CloseP2PSessionWithUser(steamIDRemote);
		}

		public static bool CloseP2PChannelWithUser(global::Steamworks.CSteamID steamIDRemote, int nChannel)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_CloseP2PChannelWithUser(steamIDRemote, nChannel);
		}

		public static bool GetP2PSessionState(global::Steamworks.CSteamID steamIDRemote, out global::Steamworks.P2PSessionState_t pConnectionState)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_GetP2PSessionState(steamIDRemote, out pConnectionState);
		}

		public static bool AllowP2PPacketRelay(bool bAllow)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_AllowP2PPacketRelay(bAllow);
		}

		public static global::Steamworks.SNetListenSocket_t CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, bool bAllowUseOfPacketRelay)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SNetListenSocket_t)global::Steamworks.NativeMethods.ISteamNetworking_CreateListenSocket(nVirtualP2PPort, nIP, nPort, bAllowUseOfPacketRelay);
		}

		public static global::Steamworks.SNetSocket_t CreateP2PConnectionSocket(global::Steamworks.CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, bool bAllowUseOfPacketRelay)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SNetSocket_t)global::Steamworks.NativeMethods.ISteamNetworking_CreateP2PConnectionSocket(steamIDTarget, nVirtualPort, nTimeoutSec, bAllowUseOfPacketRelay);
		}

		public static global::Steamworks.SNetSocket_t CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SNetSocket_t)global::Steamworks.NativeMethods.ISteamNetworking_CreateConnectionSocket(nIP, nPort, nTimeoutSec);
		}

		public static bool DestroySocket(global::Steamworks.SNetSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_DestroySocket(hSocket, bNotifyRemoteEnd);
		}

		public static bool DestroyListenSocket(global::Steamworks.SNetListenSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_DestroyListenSocket(hSocket, bNotifyRemoteEnd);
		}

		public static bool SendDataOnSocket(global::Steamworks.SNetSocket_t hSocket, global::System.IntPtr pubData, uint cubData, bool bReliable)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_SendDataOnSocket(hSocket, pubData, cubData, bReliable);
		}

		public static bool IsDataAvailableOnSocket(global::Steamworks.SNetSocket_t hSocket, out uint pcubMsgSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_IsDataAvailableOnSocket(hSocket, out pcubMsgSize);
		}

		public static bool RetrieveDataFromSocket(global::Steamworks.SNetSocket_t hSocket, global::System.IntPtr pubDest, uint cubDest, out uint pcubMsgSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_RetrieveDataFromSocket(hSocket, pubDest, cubDest, out pcubMsgSize);
		}

		public static bool IsDataAvailable(global::Steamworks.SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out global::Steamworks.SNetSocket_t phSocket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_IsDataAvailable(hListenSocket, out pcubMsgSize, out phSocket);
		}

		public static bool RetrieveData(global::Steamworks.SNetListenSocket_t hListenSocket, global::System.IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out global::Steamworks.SNetSocket_t phSocket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_RetrieveData(hListenSocket, pubDest, cubDest, out pcubMsgSize, out phSocket);
		}

		public static bool GetSocketInfo(global::Steamworks.SNetSocket_t hSocket, out global::Steamworks.CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_GetSocketInfo(hSocket, out pSteamIDRemote, out peSocketStatus, out punIPRemote, out punPortRemote);
		}

		public static bool GetListenSocketInfo(global::Steamworks.SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_GetListenSocketInfo(hListenSocket, out pnIP, out pnPort);
		}

		public static global::Steamworks.ESNetSocketConnectionType GetSocketConnectionType(global::Steamworks.SNetSocket_t hSocket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_GetSocketConnectionType(hSocket);
		}

		public static int GetMaxPacketSize(global::Steamworks.SNetSocket_t hSocket)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamNetworking_GetMaxPacketSize(hSocket);
		}
	}
}
