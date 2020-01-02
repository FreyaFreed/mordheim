using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	internal static class NativeMethods
	{
		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "Shutdown")]
		public static extern void SteamAPI_Shutdown();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "IsSteamRunning")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool SteamAPI_IsSteamRunning();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "RestartAppIfNecessary")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool SteamAPI_RestartAppIfNecessary(global::Steamworks.AppId_t unOwnAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "WriteMiniDump")]
		public static extern void SteamAPI_WriteMiniDump(uint uStructuredExceptionCode, global::System.IntPtr pvExceptionInfo, uint uBuildID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SetMiniDumpComment")]
		public static extern void SteamAPI_SetMiniDumpComment(global::Steamworks.InteropHelp.UTF8StringHandle pchMsg);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamClient_")]
		public static extern global::System.IntPtr SteamClient();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "InitSafe")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool SteamAPI_InitSafe();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "ReleaseCurrentThreadMemory")]
		public static extern void SteamAPI_ReleaseCurrentThreadMemory();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "RunCallbacks")]
		public static extern void SteamAPI_RunCallbacks();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "RegisterCallback")]
		public static extern void SteamAPI_RegisterCallback(global::System.IntPtr pCallback, int iCallback);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "UnregisterCallback")]
		public static extern void SteamAPI_UnregisterCallback(global::System.IntPtr pCallback);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "RegisterCallResult")]
		public static extern void SteamAPI_RegisterCallResult(global::System.IntPtr pCallback, ulong hAPICall);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "UnregisterCallResult")]
		public static extern void SteamAPI_UnregisterCallResult(global::System.IntPtr pCallback, ulong hAPICall);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "Steam_RunCallbacks_")]
		public static extern void Steam_RunCallbacks(global::Steamworks.HSteamPipe hSteamPipe, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bGameServerCallbacks);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "Steam_RegisterInterfaceFuncs_")]
		public static extern void Steam_RegisterInterfaceFuncs(global::System.IntPtr hModule);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "Steam_GetHSteamUserCurrent_")]
		public static extern int Steam_GetHSteamUserCurrent();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GetSteamInstallPath")]
		public static extern int SteamAPI_GetSteamInstallPath();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GetHSteamPipe_")]
		public static extern int SteamAPI_GetHSteamPipe();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SetTryCatchCallbacks")]
		public static extern void SteamAPI_SetTryCatchCallbacks([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bTryCatchCallbacks);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GetHSteamUser_")]
		public static extern int SteamAPI_GetHSteamUser();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "UseBreakpadCrashHandler")]
		public static extern void SteamAPI_UseBreakpadCrashHandler(global::Steamworks.InteropHelp.UTF8StringHandle pchVersion, global::Steamworks.InteropHelp.UTF8StringHandle pchDate, global::Steamworks.InteropHelp.UTF8StringHandle pchTime, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bFullMemoryDumps, global::System.IntPtr pvContext, global::System.IntPtr m_pfnPreMinidumpCallback);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamUser();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamFriends();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamUtils();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamMatchmaking();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamUserStats();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamApps();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamNetworking();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamMatchmakingServers();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamRemoteStorage();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamScreenshots();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamHTTP();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamUnifiedMessages();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamController();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamUGC();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamAppList();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamMusic();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamMusicRemote();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamHTMLSurface();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamInventory();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamVideo();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_InitSafe")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool SteamGameServer_InitSafe(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, global::Steamworks.EServerMode eServerMode, global::Steamworks.InteropHelp.UTF8StringHandle pchVersionString);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_Shutdown")]
		public static extern void SteamGameServer_Shutdown();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_RunCallbacks")]
		public static extern void SteamGameServer_RunCallbacks();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_BSecure")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool SteamGameServer_BSecure();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_GetSteamID")]
		public static extern ulong SteamGameServer_GetSteamID();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_GetHSteamPipe")]
		public static extern int SteamGameServer_GetHSteamPipe();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "GameServer_GetHSteamUser")]
		public static extern int SteamGameServer_GetHSteamUser();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamClientGameServer();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServer();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServerUtils();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServerNetworking();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServerStats();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServerHTTP();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServerInventory();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr SteamGameServerUGC();

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BDecryptTicket")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool BDecryptTicket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketEncrypted, uint cubTicketEncrypted, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPArray, SizeConst = 32)] byte[] rgubKey, int cubKey);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BIsTicketForApp")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool BIsTicketForApp([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetTicketIssueTime")]
		public static extern uint GetTicketIssueTime([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetTicketSteamID")]
		public static extern void GetTicketSteamID([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out global::Steamworks.CSteamID psteamID);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetTicketAppID")]
		public static extern uint GetTicketAppID([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BUserOwnsAppInTicket")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool BUserOwnsAppInTicket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BUserIsVacBanned")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool BUserIsVacBanned([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		[global::System.Runtime.InteropServices.DllImport("sdkencryptedappticket", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetUserVariableData")]
		public static extern global::System.IntPtr GetUserVariableData([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamAppList_GetNumInstalledApps();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamAppList_GetInstalledApps([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.AppId_t[] pvecAppID, uint unMaxAppIDs);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppName(global::Steamworks.AppId_t nAppID, global::System.IntPtr pchName, int cchNameMax);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppInstallDir(global::Steamworks.AppId_t nAppID, global::System.IntPtr pchDirectory, int cchNameMax);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppBuildId(global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribed();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsLowViolence();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsCybercafe();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsVACBanned();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamApps_GetCurrentGameLanguage();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamApps_GetAvailableGameLanguages();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribedApp(global::Steamworks.AppId_t appID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsDlcInstalled(global::Steamworks.AppId_t appID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetEarliestPurchaseUnixTime(global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribedFromFreeWeekend();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamApps_GetDLCCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BGetDLCDataByIndex(int iDLC, out global::Steamworks.AppId_t pAppID, out bool pbAvailable, global::System.IntPtr pchName, int cchNameBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamApps_InstallDLC(global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamApps_UninstallDLC(global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamApps_RequestAppProofOfPurchaseKey(global::Steamworks.AppId_t nAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_GetCurrentBetaName(global::System.IntPtr pchName, int cchNameBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_MarkContentCorrupt([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bMissingFilesOnly);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetInstalledDepots(global::Steamworks.AppId_t appID, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.DepotId_t[] pvecDepots, uint cMaxDepots);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetAppInstallDir(global::Steamworks.AppId_t appID, global::System.IntPtr pchFolder, uint cchFolderBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsAppInstalled(global::Steamworks.AppId_t appID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamApps_GetAppOwner();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamApps_GetLaunchQueryParam(global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamApps_GetDlcDownloadProgress(global::Steamworks.AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamApps_GetAppBuildId();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamClient_CreateSteamPipe();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamClient_BReleaseSteamPipe(global::Steamworks.HSteamPipe hSteamPipe);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamClient_ConnectToGlobalUser(global::Steamworks.HSteamPipe hSteamPipe);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamClient_CreateLocalUser(out global::Steamworks.HSteamPipe phSteamPipe, global::Steamworks.EAccountType eAccountType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamClient_ReleaseUser(global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.HSteamUser hUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamUser(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamGameServer(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamClient_SetLocalIPBinding(uint unIP, ushort usPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamFriends(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamUtils(global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamMatchmaking(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamMatchmakingServers(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamGenericInterface(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamUserStats(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamGameServerStats(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamApps(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamNetworking(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamRemoteStorage(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamScreenshots(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamClient_GetIPCCallCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamClient_SetWarningMessageHook(global::Steamworks.SteamAPIWarningMessageHook_t pFunction);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamClient_BShutdownIfAllPipesClosed();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamHTTP(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamUnifiedMessages(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamController(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamUGC(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamAppList(global::Steamworks.HSteamUser hSteamUser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamMusic(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamMusicRemote(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamHTMLSurface(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamInventory(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamClient_GetISteamVideo(global::Steamworks.HSteamUser hSteamuser, global::Steamworks.HSteamPipe hSteamPipe, global::Steamworks.InteropHelp.UTF8StringHandle pchVersion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamController_Init();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamController_Shutdown();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamController_RunFrame();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamController_GetConnectedControllers([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.ControllerHandle_t[] handlesOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamController_ShowBindingPanel(global::Steamworks.ControllerHandle_t controllerHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetActionSetHandle(global::Steamworks.InteropHelp.UTF8StringHandle pszActionSetName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamController_ActivateActionSet(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerActionSetHandle_t actionSetHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetCurrentActionSet(global::Steamworks.ControllerHandle_t controllerHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetDigitalActionHandle(global::Steamworks.InteropHelp.UTF8StringHandle pszActionName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ControllerDigitalActionData_t ISteamController_GetDigitalActionData(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerDigitalActionHandle_t digitalActionHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamController_GetDigitalActionOrigins(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerActionSetHandle_t actionSetHandle, global::Steamworks.ControllerDigitalActionHandle_t digitalActionHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.EControllerActionOrigin[] originsOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamController_GetAnalogActionHandle(global::Steamworks.InteropHelp.UTF8StringHandle pszActionName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ControllerAnalogActionData_t ISteamController_GetAnalogActionData(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerAnalogActionHandle_t analogActionHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamController_GetAnalogActionOrigins(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerActionSetHandle_t actionSetHandle, global::Steamworks.ControllerAnalogActionHandle_t analogActionHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.EControllerActionOrigin[] originsOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamController_StopAnalogActionMomentum(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ControllerAnalogActionHandle_t eAction);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamController_TriggerHapticPulse(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ESteamControllerPad eTargetPad, ushort usDurationMicroSec);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamController_TriggerRepeatedHapticPulse(global::Steamworks.ControllerHandle_t controllerHandle, global::Steamworks.ESteamControllerPad eTargetPad, ushort usDurationMicroSec, ushort usOffMicroSec, ushort unRepeat, uint nFlags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetPersonaName();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_SetPersonaName(global::Steamworks.InteropHelp.UTF8StringHandle pchPersonaName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EPersonaState ISteamFriends_GetPersonaState();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCount(global::Steamworks.EFriendFlags iFriendFlags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFriendByIndex(int iFriend, global::Steamworks.EFriendFlags iFriendFlags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EFriendRelationship ISteamFriends_GetFriendRelationship(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EPersonaState ISteamFriends_GetFriendPersonaState(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetFriendPersonaName(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_GetFriendGamePlayed(global::Steamworks.CSteamID steamIDFriend, out global::Steamworks.FriendGameInfo_t pFriendGameInfo);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetFriendPersonaNameHistory(global::Steamworks.CSteamID steamIDFriend, int iPersonaName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendSteamLevel(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetPlayerNickname(global::Steamworks.CSteamID steamIDPlayer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendsGroupCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern short ISteamFriends_GetFriendsGroupIDByIndex(int iFG);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetFriendsGroupName(global::Steamworks.FriendsGroupID_t friendsGroupID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendsGroupMembersCount(global::Steamworks.FriendsGroupID_t friendsGroupID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_GetFriendsGroupMembersList(global::Steamworks.FriendsGroupID_t friendsGroupID, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.CSteamID[] pOutSteamIDMembers, int nMembersCount);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_HasFriend(global::Steamworks.CSteamID steamIDFriend, global::Steamworks.EFriendFlags iFriendFlags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanByIndex(int iClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetClanName(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetClanTag(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_GetClanActivityCounts(global::Steamworks.CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_DownloadClanActivityCounts([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.CSteamID[] psteamIDClans, int cClansToRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCountFromSource(global::Steamworks.CSteamID steamIDSource);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFriendFromSourceByIndex(global::Steamworks.CSteamID steamIDSource, int iFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsUserInSource(global::Steamworks.CSteamID steamIDUser, global::Steamworks.CSteamID steamIDSource);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_SetInGameVoiceSpeaking(global::Steamworks.CSteamID steamIDUser, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bSpeaking);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlay(global::Steamworks.InteropHelp.UTF8StringHandle pchDialog);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToUser(global::Steamworks.InteropHelp.UTF8StringHandle pchDialog, global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToWebPage(global::Steamworks.InteropHelp.UTF8StringHandle pchURL);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToStore(global::Steamworks.AppId_t nAppID, global::Steamworks.EOverlayToStoreFlag eFlag);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_SetPlayedWith(global::Steamworks.CSteamID steamIDUserPlayedWith);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayInviteDialog(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetSmallFriendAvatar(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetMediumFriendAvatar(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetLargeFriendAvatar(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_RequestUserInformation(global::Steamworks.CSteamID steamIDUser, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bRequireNameOnly);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_RequestClanOfficerList(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanOwner(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanOfficerCount(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanOfficerByIndex(global::Steamworks.CSteamID steamIDClan, int iOfficer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamFriends_GetUserRestrictions();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_SetRichPresence(global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ClearRichPresence();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetFriendRichPresence(global::Steamworks.CSteamID steamIDFriend, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendRichPresenceKeyCount(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamFriends_GetFriendRichPresenceKeyByIndex(global::Steamworks.CSteamID steamIDFriend, int iKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamFriends_RequestFriendRichPresence(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_InviteUserToGame(global::Steamworks.CSteamID steamIDFriend, global::Steamworks.InteropHelp.UTF8StringHandle pchConnectString);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetCoplayFriendCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetCoplayFriend(int iCoplayFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCoplayTime(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamFriends_GetFriendCoplayGame(global::Steamworks.CSteamID steamIDFriend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_JoinClanChatRoom(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_LeaveClanChatRoom(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanChatMemberCount(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetChatMemberByIndex(global::Steamworks.CSteamID steamIDClan, int iUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_SendClanChatMessage(global::Steamworks.CSteamID steamIDClanChat, global::Steamworks.InteropHelp.UTF8StringHandle pchText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanChatMessage(global::Steamworks.CSteamID steamIDClanChat, int iMessage, global::System.IntPtr prgchText, int cchTextMax, out global::Steamworks.EChatEntryType peChatEntryType, out global::Steamworks.CSteamID psteamidChatter);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsClanChatAdmin(global::Steamworks.CSteamID steamIDClanChat, global::Steamworks.CSteamID steamIDUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsClanChatWindowOpenInSteam(global::Steamworks.CSteamID steamIDClanChat);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_OpenClanChatWindowInSteam(global::Steamworks.CSteamID steamIDClanChat);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_CloseClanChatWindowInSteam(global::Steamworks.CSteamID steamIDClanChat);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_SetListenForFriendsMessages([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bInterceptEnabled);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamFriends_ReplyToFriendMessage(global::Steamworks.CSteamID steamIDFriend, global::Steamworks.InteropHelp.UTF8StringHandle pchMsgToSend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendMessage(global::Steamworks.CSteamID steamIDFriend, int iMessageID, global::System.IntPtr pvData, int cubData, out global::Steamworks.EChatEntryType peChatEntryType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFollowerCount(global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_IsFollowing(global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_EnumerateFollowingList(uint unStartIndex);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, global::Steamworks.AppId_t nGameAppId, global::Steamworks.InteropHelp.UTF8StringHandle pchVersionString);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetProduct(global::Steamworks.InteropHelp.UTF8StringHandle pszProduct);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameDescription(global::Steamworks.InteropHelp.UTF8StringHandle pszGameDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetModDir(global::Steamworks.InteropHelp.UTF8StringHandle pszModDir);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetDedicatedServer([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bDedicated);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOn(global::Steamworks.InteropHelp.UTF8StringHandle pszToken);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOnAnonymous();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOff();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BLoggedOn();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BSecure();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_GetSteamID();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_WasRestartRequested();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetMaxPlayerCount(int cPlayersMax);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetBotPlayerCount(int cBotplayers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetServerName(global::Steamworks.InteropHelp.UTF8StringHandle pszServerName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetMapName(global::Steamworks.InteropHelp.UTF8StringHandle pszMapName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetPasswordProtected([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bPasswordProtected);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetSpectatorPort(ushort unSpectatorPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetSpectatorServerName(global::Steamworks.InteropHelp.UTF8StringHandle pszSpectatorServerName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_ClearAllKeyValues();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetKeyValue(global::Steamworks.InteropHelp.UTF8StringHandle pKey, global::Steamworks.InteropHelp.UTF8StringHandle pValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameTags(global::Steamworks.InteropHelp.UTF8StringHandle pchGameTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameData(global::Steamworks.InteropHelp.UTF8StringHandle pchGameData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetRegion(global::Steamworks.InteropHelp.UTF8StringHandle pszRegion);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_SendUserConnectAndAuthenticate(uint unIPClient, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvAuthBlob, uint cubAuthBlobSize, out global::Steamworks.CSteamID pSteamIDUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_CreateUnauthenticatedUserConnection();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SendUserDisconnect(global::Steamworks.CSteamID steamIDUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BUpdateUserData(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchPlayerName, uint uScore);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServer_GetAuthSessionTicket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EBeginAuthSessionResult ISteamGameServer_BeginAuthSession([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pAuthTicket, int cbAuthTicket, global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_EndAuthSession(global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_CancelAuthTicket(global::Steamworks.HAuthTicket hAuthTicket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EUserHasLicenseForAppResult ISteamGameServer_UserHasLicenseForApp(global::Steamworks.CSteamID steamID, global::Steamworks.AppId_t appID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_RequestUserGroupStatus(global::Steamworks.CSteamID steamIDUser, global::Steamworks.CSteamID steamIDGroup);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_GetGameplayStats();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_GetServerReputation();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServer_GetPublicIP();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServer_HandleIncomingPacket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pData, int cbData, uint srcIP, ushort srcPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamGameServer_GetNextOutgoingPacket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_EnableHeartbeats([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bActive);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetHeartbeatInterval(int iHeartbeatInterval);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_ForceHeartbeat();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_AssociateWithClan(global::Steamworks.CSteamID steamIDClan);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_ComputeNewPlayerCompatibility(global::Steamworks.CSteamID steamIDNewPlayer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerStats_RequestUserStats(global::Steamworks.CSteamID steamIDUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserStat(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out int pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserStat_(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out float pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserAchievement(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserStat(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, int nData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserStat_(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, float fData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_UpdateUserAvgRateStat(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserAchievement(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_ClearUserAchievement(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerStats_StoreUserStats(global::Steamworks.CSteamID steamIDUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTMLSurface_Init();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTMLSurface_Shutdown();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamHTMLSurface_CreateBrowser(global::Steamworks.InteropHelp.UTF8StringHandle pchUserAgent, global::Steamworks.InteropHelp.UTF8StringHandle pchUserCSS);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_RemoveBrowser(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_LoadURL(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchURL, global::Steamworks.InteropHelp.UTF8StringHandle pchPostData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetSize(global::Steamworks.HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_StopLoad(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_Reload(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GoBack(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GoForward(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_AddHeader(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_ExecuteJavascript(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchScript);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseUp(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.EHTMLMouseButton eMouseButton);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseDown(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.EHTMLMouseButton eMouseButton);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseDoubleClick(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.EHTMLMouseButton eMouseButton);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseMove(global::Steamworks.HHTMLBrowser unBrowserHandle, int x, int y);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseWheel(global::Steamworks.HHTMLBrowser unBrowserHandle, int nDelta);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyDown(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, global::Steamworks.EHTMLKeyModifiers eHTMLKeyModifiers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyUp(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, global::Steamworks.EHTMLKeyModifiers eHTMLKeyModifiers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyChar(global::Steamworks.HHTMLBrowser unBrowserHandle, uint cUnicodeChar, global::Steamworks.EHTMLKeyModifiers eHTMLKeyModifiers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetHorizontalScroll(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetVerticalScroll(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetKeyFocus(global::Steamworks.HHTMLBrowser unBrowserHandle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bHasKeyFocus);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_ViewSource(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_CopyToClipboard(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_PasteFromClipboard(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_Find(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchSearchStr, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bCurrentlyInFind, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReverse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_StopFind(global::Steamworks.HHTMLBrowser unBrowserHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GetLinkAtPosition(global::Steamworks.HHTMLBrowser unBrowserHandle, int x, int y);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetCookie(global::Steamworks.InteropHelp.UTF8StringHandle pchHostname, global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue, global::Steamworks.InteropHelp.UTF8StringHandle pchPath, uint nExpires, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bSecure, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bHTTPOnly);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetPageScaleFactor(global::Steamworks.HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetBackgroundMode(global::Steamworks.HHTMLBrowser unBrowserHandle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bBackgroundMode);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_AllowStartRequest(global::Steamworks.HHTMLBrowser unBrowserHandle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowed);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_JSDialogResponse(global::Steamworks.HHTMLBrowser unBrowserHandle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bResult);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_FileLoadDialogResponse(global::Steamworks.HHTMLBrowser unBrowserHandle, global::System.IntPtr pchSelectedFiles);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamHTTP_CreateHTTPRequest(global::Steamworks.EHTTPMethod eHTTPRequestMethod, global::Steamworks.InteropHelp.UTF8StringHandle pchAbsoluteURL);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestContextValue(global::Steamworks.HTTPRequestHandle hRequest, ulong ulContextValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(global::Steamworks.HTTPRequestHandle hRequest, uint unTimeoutSeconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestHeaderValue(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderName, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestGetOrPostParameter(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchParamName, global::Steamworks.InteropHelp.UTF8StringHandle pchParamValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SendHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest, out global::Steamworks.SteamAPICall_t pCallHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SendHTTPRequestAndStreamResponse(global::Steamworks.HTTPRequestHandle hRequest, out global::Steamworks.SteamAPICall_t pCallHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_DeferHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_PrioritizeHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseHeaderSize(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseHeaderValue(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderName, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pHeaderValueBuffer, uint unBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseBodySize(global::Steamworks.HTTPRequestHandle hRequest, out uint unBodySize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseBodyData(global::Steamworks.HTTPRequestHandle hRequest, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pBodyDataBuffer, uint unBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPStreamingResponseBodyData(global::Steamworks.HTTPRequestHandle hRequest, uint cOffset, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pBodyDataBuffer, uint unBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_ReleaseHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPDownloadProgressPct(global::Steamworks.HTTPRequestHandle hRequest, out float pflPercentOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestRawPostBody(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchContentType, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubBody, uint unBodyLen);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamHTTP_CreateCookieContainer([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowResponsesToModify);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_ReleaseCookieContainer(global::Steamworks.HTTPCookieContainerHandle hCookieContainer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetCookie(global::Steamworks.HTTPCookieContainerHandle hCookieContainer, global::Steamworks.InteropHelp.UTF8StringHandle pchHost, global::Steamworks.InteropHelp.UTF8StringHandle pchUrl, global::Steamworks.InteropHelp.UTF8StringHandle pchCookie);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestCookieContainer(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.HTTPCookieContainerHandle hCookieContainer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestUserAgentInfo(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchUserAgentInfo);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(global::Steamworks.HTTPRequestHandle hRequest, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bRequireVerifiedCertificate);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(global::Steamworks.HTTPRequestHandle hRequest, uint unMilliseconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPRequestWasTimedOut(global::Steamworks.HTTPRequestHandle hRequest, out bool pbWasTimedOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EResult ISteamInventory_GetResultStatus(global::Steamworks.SteamInventoryResult_t resultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetResultItems(global::Steamworks.SteamInventoryResult_t resultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamInventory_GetResultTimestamp(global::Steamworks.SteamInventoryResult_t resultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_CheckResultSteamID(global::Steamworks.SteamInventoryResult_t resultHandle, global::Steamworks.CSteamID steamIDExpected);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamInventory_DestroyResult(global::Steamworks.SteamInventoryResult_t resultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetAllItems(out global::Steamworks.SteamInventoryResult_t pResultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemsByID(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_SerializeResult(global::Steamworks.SteamInventoryResult_t resultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pOutBuffer, out uint punOutBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_DeserializeResult(out global::Steamworks.SteamInventoryResult_t pOutResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pBuffer, uint unBufferSize, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GenerateItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pArrayItemDefs, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] punArrayQuantity, uint unArrayLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GrantPromoItems(out global::Steamworks.SteamInventoryResult_t pResultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_AddPromoItem(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t itemDef);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_AddPromoItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_ConsumeItem(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t itemConsume, uint unQuantity);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_ExchangeItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pArrayGenerate, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pArrayDestroy, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_TransferItemQuantity(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t itemIdSource, uint unQuantity, global::Steamworks.SteamItemInstanceID_t itemIdDest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamInventory_SendItemDropHeartbeat();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_TriggerItemDrop(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t dropListDefinition);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_TradeItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.CSteamID steamIDTradePartner, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pArrayGive, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pArrayGet, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_LoadItemDefinitions();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemDefinitionIDs([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemDefinitionProperty(global::Steamworks.SteamItemDef_t iDefinition, global::Steamworks.InteropHelp.UTF8StringHandle pchPropertyName, global::System.IntPtr pchValueBuffer, ref uint punValueBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetFavoriteGameCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetFavoriteGame(int iGame, out global::Steamworks.AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_AddFavoriteGame(global::Steamworks.AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_RemoveFavoriteGame(global::Steamworks.AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_RequestLobbyList();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListStringFilter(global::Steamworks.InteropHelp.UTF8StringHandle pchKeyToMatch, global::Steamworks.InteropHelp.UTF8StringHandle pchValueToMatch, global::Steamworks.ELobbyComparison eComparisonType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListNumericalFilter(global::Steamworks.InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToMatch, global::Steamworks.ELobbyComparison eComparisonType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListNearValueFilter(global::Steamworks.InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToBeCloseTo);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListDistanceFilter(global::Steamworks.ELobbyDistanceFilter eLobbyDistanceFilter);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListResultCountFilter(int cMaxResults);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyByIndex(int iLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_CreateLobby(global::Steamworks.ELobbyType eLobbyType, int cMaxMembers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_JoinLobby(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_LeaveLobby(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_InviteUserToLobby(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDInvitee);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetNumLobbyMembers(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyMemberByIndex(global::Steamworks.CSteamID steamIDLobby, int iMember);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmaking_GetLobbyData(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyData(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyDataCount(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetLobbyDataByIndex(global::Steamworks.CSteamID steamIDLobby, int iLobbyData, global::System.IntPtr pchKey, int cchKeyBufferSize, global::System.IntPtr pchValue, int cchValueBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_DeleteLobbyData(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmaking_GetLobbyMemberData(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_SetLobbyMemberData(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SendLobbyChatMsg(global::Steamworks.CSteamID steamIDLobby, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvMsgBody, int cubMsgBody);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyChatEntry(global::Steamworks.CSteamID steamIDLobby, int iChatID, out global::Steamworks.CSteamID pSteamIDUser, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvData, int cubData, out global::Steamworks.EChatEntryType peChatEntryType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_RequestLobbyData(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_SetLobbyGameServer(global::Steamworks.CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, global::Steamworks.CSteamID steamIDGameServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetLobbyGameServer(global::Steamworks.CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out global::Steamworks.CSteamID psteamIDGameServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyMemberLimit(global::Steamworks.CSteamID steamIDLobby, int cMaxMembers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyMemberLimit(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyType(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.ELobbyType eLobbyType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyJoinable(global::Steamworks.CSteamID steamIDLobby, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bLobbyJoinable);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyOwner(global::Steamworks.CSteamID steamIDLobby);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyOwner(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDNewOwner);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLinkedLobby(global::Steamworks.CSteamID steamIDLobby, global::Steamworks.CSteamID steamIDLobbyDependent);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_RequestInternetServerList(global::Steamworks.AppId_t iApp, global::System.IntPtr ppchFilters, uint nFilters, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_RequestLANServerList(global::Steamworks.AppId_t iApp, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_RequestFriendsServerList(global::Steamworks.AppId_t iApp, global::System.IntPtr ppchFilters, uint nFilters, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_RequestFavoritesServerList(global::Steamworks.AppId_t iApp, global::System.IntPtr ppchFilters, uint nFilters, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_RequestHistoryServerList(global::Steamworks.AppId_t iApp, global::System.IntPtr ppchFilters, uint nFilters, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_RequestSpectatorServerList(global::Steamworks.AppId_t iApp, global::System.IntPtr ppchFilters, uint nFilters, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_ReleaseRequest(global::Steamworks.HServerListRequest hServerListRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamMatchmakingServers_GetServerDetails(global::Steamworks.HServerListRequest hRequest, int iServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_CancelQuery(global::Steamworks.HServerListRequest hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_RefreshQuery(global::Steamworks.HServerListRequest hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMatchmakingServers_IsRefreshing(global::Steamworks.HServerListRequest hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_GetServerCount(global::Steamworks.HServerListRequest hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_RefreshServer(global::Steamworks.HServerListRequest hRequest, int iServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_PingServer(uint unIP, ushort usPort, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_PlayerDetails(uint unIP, ushort usPort, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_ServerRules(uint unIP, ushort usPort, global::System.IntPtr pRequestServersResponse);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_CancelServerQuery(global::Steamworks.HServerQuery hServerQuery);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusic_BIsEnabled();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusic_BIsPlaying();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.AudioPlayback_Status ISteamMusic_GetPlaybackStatus();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMusic_Play();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMusic_Pause();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMusic_PlayPrevious();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMusic_PlayNext();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamMusic_SetVolume(float flVolume);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern float ISteamMusic_GetVolume();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_RegisterSteamMusicRemote(global::Steamworks.InteropHelp.UTF8StringHandle pchName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_DeregisterSteamMusicRemote();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_BIsCurrentMusicRemote();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_BActivationSuccess([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetDisplayName(global::Steamworks.InteropHelp.UTF8StringHandle pchDisplayName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetPNGIcon_64x64([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvBuffer, uint cbBufferLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlayPrevious([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlayNext([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableShuffled([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableLooped([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableQueue([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlaylists([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdatePlaybackStatus(global::Steamworks.AudioPlayback_Status nStatus);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateShuffled([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateLooped([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateVolume(float flValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryWillChange();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryIsAvailable([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAvailable);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryText(global::Steamworks.InteropHelp.UTF8StringHandle pchText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(int nValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryCoverArt([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvBuffer, uint cbBufferLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryDidChange();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_QueueWillChange();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_ResetQueueEntries();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetQueueEntry(int nID, int nPosition, global::Steamworks.InteropHelp.UTF8StringHandle pchEntryText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetCurrentQueueEntry(int nID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_QueueDidChange();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_PlaylistWillChange();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_ResetPlaylistEntries();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetPlaylistEntry(int nID, int nPosition, global::Steamworks.InteropHelp.UTF8StringHandle pchEntryText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetCurrentPlaylistEntry(int nID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_PlaylistDidChange();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_SendP2PPacket(global::Steamworks.CSteamID steamIDRemote, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubData, uint cubData, global::Steamworks.EP2PSend eP2PSendType, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_ReadP2PPacket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out global::Steamworks.CSteamID psteamIDRemote, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_AcceptP2PSessionWithUser(global::Steamworks.CSteamID steamIDRemote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_CloseP2PSessionWithUser(global::Steamworks.CSteamID steamIDRemote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_CloseP2PChannelWithUser(global::Steamworks.CSteamID steamIDRemote, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetP2PSessionState(global::Steamworks.CSteamID steamIDRemote, out global::Steamworks.P2PSessionState_t pConnectionState);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_AllowP2PPacketRelay([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllow);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateP2PConnectionSocket(global::Steamworks.CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_DestroySocket(global::Steamworks.SNetSocket_t hSocket, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bNotifyRemoteEnd);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_DestroyListenSocket(global::Steamworks.SNetListenSocket_t hSocket, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bNotifyRemoteEnd);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_SendDataOnSocket(global::Steamworks.SNetSocket_t hSocket, global::System.IntPtr pubData, uint cubData, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReliable);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsDataAvailableOnSocket(global::Steamworks.SNetSocket_t hSocket, out uint pcubMsgSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_RetrieveDataFromSocket(global::Steamworks.SNetSocket_t hSocket, global::System.IntPtr pubDest, uint cubDest, out uint pcubMsgSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsDataAvailable(global::Steamworks.SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out global::Steamworks.SNetSocket_t phSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_RetrieveData(global::Steamworks.SNetListenSocket_t hListenSocket, global::System.IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out global::Steamworks.SNetSocket_t phSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetSocketInfo(global::Steamworks.SNetSocket_t hSocket, out global::Steamworks.CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetListenSocketInfo(global::Steamworks.SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ESNetSocketConnectionType ISteamNetworking_GetSocketConnectionType(global::Steamworks.SNetSocket_t hSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamNetworking_GetMaxPacketSize(global::Steamworks.SNetSocket_t hSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWrite(global::Steamworks.InteropHelp.UTF8StringHandle pchFile, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvData, int cubData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_FileRead(global::Steamworks.InteropHelp.UTF8StringHandle pchFile, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvData, int cubDataToRead);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileWriteAsync(global::Steamworks.InteropHelp.UTF8StringHandle pchFile, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvData, uint cubData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileReadAsync(global::Steamworks.InteropHelp.UTF8StringHandle pchFile, uint nOffset, uint cubToRead);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileReadAsyncComplete(global::Steamworks.SteamAPICall_t hReadCall, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvBuffer, uint cubToRead);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileForget(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileDelete(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileShare(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_SetSyncPlatforms(global::Steamworks.InteropHelp.UTF8StringHandle pchFile, global::Steamworks.ERemoteStoragePlatform eRemoteStoragePlatform);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileWriteStreamOpen(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamWriteChunk(global::Steamworks.UGCFileWriteStreamHandle_t writeHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvData, int cubData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamClose(global::Steamworks.UGCFileWriteStreamHandle_t writeHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamCancel(global::Steamworks.UGCFileWriteStreamHandle_t writeHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileExists(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FilePersisted(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetFileSize(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern long ISteamRemoteStorage_GetFileTimestamp(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ERemoteStoragePlatform ISteamRemoteStorage_GetSyncPlatforms(global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetFileCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamRemoteStorage_GetFileNameAndSize(int iFile, out int pnFileSizeInBytes);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetQuota(out int pnTotalBytes, out int puAvailableBytes);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_IsCloudEnabledForAccount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_IsCloudEnabledForApp();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamRemoteStorage_SetCloudEnabledForApp([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bEnabled);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UGCDownload(global::Steamworks.UGCHandle_t hContent, uint unPriority);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetUGCDownloadProgress(global::Steamworks.UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetUGCDetails(global::Steamworks.UGCHandle_t hContent, out global::Steamworks.AppId_t pnAppID, out global::System.IntPtr ppchName, out int pnFileSizeInBytes, out global::Steamworks.CSteamID pSteamIDOwner);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_UGCRead(global::Steamworks.UGCHandle_t hContent, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pvData, int cubDataToRead, uint cOffset, global::Steamworks.EUGCReadAction eAction);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetCachedUGCCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetCachedUGCHandle(int iCachedContent);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_PublishWorkshopFile(global::Steamworks.InteropHelp.UTF8StringHandle pchFile, global::Steamworks.InteropHelp.UTF8StringHandle pchPreviewFile, global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.InteropHelp.UTF8StringHandle pchTitle, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility, global::System.IntPtr pTags, global::Steamworks.EWorkshopFileType eWorkshopFileType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_CreatePublishedFileUpdateRequest(global::Steamworks.PublishedFileId_t unPublishedFileId);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileFile(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFilePreviewFile(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchPreviewFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileTitle(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchTitle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileDescription(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileVisibility(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileTags(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::System.IntPtr pTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_CommitPublishedFileUpdate(global::Steamworks.PublishedFileUpdateHandle_t updateHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetPublishedFileDetails(global::Steamworks.PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_DeletePublishedFile(global::Steamworks.PublishedFileId_t unPublishedFileId);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserPublishedFiles(uint unStartIndex);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_SubscribePublishedFile(global::Steamworks.PublishedFileId_t unPublishedFileId);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserSubscribedFiles(uint unStartIndex);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UnsubscribePublishedFile(global::Steamworks.PublishedFileId_t unPublishedFileId);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.InteropHelp.UTF8StringHandle pchChangeDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetPublishedItemVoteDetails(global::Steamworks.PublishedFileId_t unPublishedFileId);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UpdateUserPublishedItemVote(global::Steamworks.PublishedFileId_t unPublishedFileId, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bVoteUp);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetUserPublishedItemVoteDetails(global::Steamworks.PublishedFileId_t unPublishedFileId);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(global::Steamworks.CSteamID steamId, uint unStartIndex, global::System.IntPtr pRequiredTags, global::System.IntPtr pExcludedTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_PublishVideo(global::Steamworks.EWorkshopVideoProvider eVideoProvider, global::Steamworks.InteropHelp.UTF8StringHandle pchVideoAccount, global::Steamworks.InteropHelp.UTF8StringHandle pchVideoIdentifier, global::Steamworks.InteropHelp.UTF8StringHandle pchPreviewFile, global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.InteropHelp.UTF8StringHandle pchTitle, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility, global::System.IntPtr pTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_SetUserPublishedFileAction(global::Steamworks.PublishedFileId_t unPublishedFileId, global::Steamworks.EWorkshopFileAction eAction);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(global::Steamworks.EWorkshopFileAction eAction, uint unStartIndex);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(global::Steamworks.EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, global::System.IntPtr pTags, global::System.IntPtr pUserTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UGCDownloadToLocation(global::Steamworks.UGCHandle_t hContent, global::Steamworks.InteropHelp.UTF8StringHandle pchLocation, uint unPriority);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_WriteScreenshot([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubRGB, uint cubRGB, int nWidth, int nHeight);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_AddScreenshotToLibrary(global::Steamworks.InteropHelp.UTF8StringHandle pchFilename, global::Steamworks.InteropHelp.UTF8StringHandle pchThumbnailFilename, int nWidth, int nHeight);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamScreenshots_TriggerScreenshot();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamScreenshots_HookScreenshots([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bHook);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_SetLocation(global::Steamworks.ScreenshotHandle hScreenshot, global::Steamworks.InteropHelp.UTF8StringHandle pchLocation);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_TagUser(global::Steamworks.ScreenshotHandle hScreenshot, global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_TagPublishedFile(global::Steamworks.ScreenshotHandle hScreenshot, global::Steamworks.PublishedFileId_t unPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryUserUGCRequest(global::Steamworks.AccountID_t unAccountID, global::Steamworks.EUserUGCList eListType, global::Steamworks.EUGCMatchingUGCType eMatchingUGCType, global::Steamworks.EUserUGCListSortOrder eSortOrder, global::Steamworks.AppId_t nCreatorAppID, global::Steamworks.AppId_t nConsumerAppID, uint unPage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryAllUGCRequest(global::Steamworks.EUGCQuery eQueryType, global::Steamworks.EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, global::Steamworks.AppId_t nCreatorAppID, global::Steamworks.AppId_t nConsumerAppID, uint unPage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryUGCDetailsRequest([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SendQueryUGCRequest(global::Steamworks.UGCQueryHandle_t handle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCResult(global::Steamworks.UGCQueryHandle_t handle, uint index, out global::Steamworks.SteamUGCDetails_t pDetails);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCPreviewURL(global::Steamworks.UGCQueryHandle_t handle, uint index, global::System.IntPtr pchURL, uint cchURLSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCMetadata(global::Steamworks.UGCQueryHandle_t handle, uint index, global::System.IntPtr pchMetadata, uint cchMetadatasize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCChildren(global::Steamworks.UGCQueryHandle_t handle, uint index, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCStatistic(global::Steamworks.UGCQueryHandle_t handle, uint index, global::Steamworks.EItemStatistic eStatType, out uint pStatValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetQueryUGCNumAdditionalPreviews(global::Steamworks.UGCQueryHandle_t handle, uint index);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCAdditionalPreview(global::Steamworks.UGCQueryHandle_t handle, uint index, uint previewIndex, global::System.IntPtr pchURLOrVideoID, uint cchURLSize, out bool pbIsImage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetQueryUGCNumKeyValueTags(global::Steamworks.UGCQueryHandle_t handle, uint index);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCKeyValueTag(global::Steamworks.UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, global::System.IntPtr pchKey, uint cchKeySize, global::System.IntPtr pchValue, uint cchValueSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_ReleaseQueryUGCRequest(global::Steamworks.UGCQueryHandle_t handle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddRequiredTag(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pTagName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddExcludedTag(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pTagName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnKeyValueTags(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnKeyValueTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnLongDescription(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnLongDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnMetadata(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnMetadata);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnChildren(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnChildren);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnAdditionalPreviews(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnAdditionalPreviews);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnTotalOnly(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnTotalOnly);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetLanguage(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchLanguage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetAllowCachedResponse(global::Steamworks.UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetCloudFileNameFilter(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pMatchCloudFileName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetMatchAnyTag(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bMatchAnyTag);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetSearchText(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pSearchText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetRankedByTrendDays(global::Steamworks.UGCQueryHandle_t handle, uint unDays);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddRequiredKeyValueTag(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pKey, global::Steamworks.InteropHelp.UTF8StringHandle pValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RequestUGCDetails(global::Steamworks.PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateItem(global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.EWorkshopFileType eFileType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_StartItemUpdate(global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemTitle(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchTitle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemDescription(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemUpdateLanguage(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchLanguage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemMetadata(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchMetaData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemVisibility(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemTags(global::Steamworks.UGCUpdateHandle_t updateHandle, global::System.IntPtr pTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemContent(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pszContentFolder);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemPreview(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pszPreviewFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_RemoveItemKeyValueTags(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddItemKeyValueTag(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SubmitItemUpdate(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchChangeNote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EItemUpdateStatus ISteamUGC_GetItemUpdateProgress(global::Steamworks.UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SetUserItemVote(global::Steamworks.PublishedFileId_t nPublishedFileID, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bVoteUp);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_GetUserItemVote(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_AddItemToFavorites(global::Steamworks.AppId_t nAppId, global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RemoveItemFromFavorites(global::Steamworks.AppId_t nAppId, global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SubscribeItem(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_UnsubscribeItem(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetNumSubscribedItems();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetSubscribedItems([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetItemState(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetItemInstallInfo(global::Steamworks.PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, global::System.IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetItemDownloadInfo(global::Steamworks.PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_DownloadItem(global::Steamworks.PublishedFileId_t nPublishedFileID, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bHighPriority);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUGC_BInitWorkshopForGameServer(global::Steamworks.DepotId_t unWorkshopDepotID, global::Steamworks.InteropHelp.UTF8StringHandle pszFolder);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUGC_SuspendDownloads([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bSuspend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUnifiedMessages_SendMethod(global::Steamworks.InteropHelp.UTF8StringHandle pchServiceMethod, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_GetMethodResponseInfo(global::Steamworks.ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out global::Steamworks.EResult peResult);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_GetMethodResponseData(global::Steamworks.ClientUnifiedMessageHandle hHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pResponseBuffer, uint unResponseBufferSize, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAutoRelease);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_ReleaseMethod(global::Steamworks.ClientUnifiedMessageHandle hHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_SendNotification(global::Steamworks.InteropHelp.UTF8StringHandle pchServiceNotification, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pNotificationBuffer, uint unNotificationBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetHSteamUser();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUser_BLoggedOn();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_GetSteamID();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUser_InitiateGameConnection([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pAuthBlob, int cbMaxAuthBlob, global::Steamworks.CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bSecure);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_TerminateGameConnection(uint unIPServer, ushort usPortServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_TrackAppUsageEvent(global::Steamworks.CGameID gameID, int eAppUsageEvent, global::Steamworks.InteropHelp.UTF8StringHandle pchExtraInfo);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUser_GetUserDataFolder(global::System.IntPtr pchBuffer, int cubBuffer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_StartVoiceRecording();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_StopVoiceRecording();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EVoiceResult ISteamUser_GetAvailableVoice(out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EVoiceResult ISteamUser_GetVoice([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bWantCompressed, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bWantUncompressed, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EVoiceResult ISteamUser_DecompressVoice([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pCompressed, uint cbCompressed, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUser_GetVoiceOptimalSampleRate();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUser_GetAuthSessionTicket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EBeginAuthSessionResult ISteamUser_BeginAuthSession([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pAuthTicket, int cbAuthTicket, global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_EndAuthSession(global::Steamworks.CSteamID steamID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_CancelAuthTicket(global::Steamworks.HAuthTicket hAuthTicket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EUserHasLicenseForAppResult ISteamUser_UserHasLicenseForApp(global::Steamworks.CSteamID steamID, global::Steamworks.AppId_t appID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsBehindNAT();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUser_AdvertiseGame(global::Steamworks.CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_RequestEncryptedAppTicket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pDataToInclude, int cbDataToInclude);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUser_GetEncryptedAppTicket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetGameBadgeLevel(int nSeries, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bFoil);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetPlayerSteamLevel();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_RequestStoreAuthURL(global::Steamworks.InteropHelp.UTF8StringHandle pchRedirectURL);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_RequestCurrentStats();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetStat(global::Steamworks.InteropHelp.UTF8StringHandle pchName, out int pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetStat_(global::Steamworks.InteropHelp.UTF8StringHandle pchName, out float pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetStat(global::Steamworks.InteropHelp.UTF8StringHandle pchName, int nData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetStat_(global::Steamworks.InteropHelp.UTF8StringHandle pchName, float fData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_UpdateAvgRateStat(global::Steamworks.InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievement(global::Steamworks.InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetAchievement(global::Steamworks.InteropHelp.UTF8StringHandle pchName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_ClearAchievement(global::Steamworks.InteropHelp.UTF8StringHandle pchName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievementAndUnlockTime(global::Steamworks.InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_StoreStats();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetAchievementIcon(global::Steamworks.InteropHelp.UTF8StringHandle pchName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamUserStats_GetAchievementDisplayAttribute(global::Steamworks.InteropHelp.UTF8StringHandle pchName, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_IndicateAchievementProgress(global::Steamworks.InteropHelp.UTF8StringHandle pchName, uint nCurProgress, uint nMaxProgress);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUserStats_GetNumAchievements();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamUserStats_GetAchievementName(uint iAchievement);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestUserStats(global::Steamworks.CSteamID steamIDUser);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserStat(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out int pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserStat_(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out float pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserAchievement(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserAchievementAndUnlockTime(global::Steamworks.CSteamID steamIDUser, global::Steamworks.InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_ResetAllStats([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAchievementsToo);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_FindOrCreateLeaderboard(global::Steamworks.InteropHelp.UTF8StringHandle pchLeaderboardName, global::Steamworks.ELeaderboardSortMethod eLeaderboardSortMethod, global::Steamworks.ELeaderboardDisplayType eLeaderboardDisplayType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_FindLeaderboard(global::Steamworks.InteropHelp.UTF8StringHandle pchLeaderboardName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamUserStats_GetLeaderboardName(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetLeaderboardEntryCount(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ELeaderboardSortMethod ISteamUserStats_GetLeaderboardSortMethod(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ELeaderboardDisplayType ISteamUserStats_GetLeaderboardDisplayType(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_DownloadLeaderboardEntries(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_DownloadLeaderboardEntriesForUsers(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.CSteamID[] prgUsers, int cUsers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetDownloadedLeaderboardEntry(global::Steamworks.SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out global::Steamworks.LeaderboardEntry_t pLeaderboardEntry, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] int[] pDetails, int cDetailsMax);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_UploadLeaderboardScore(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] int[] pScoreDetails, int cScoreDetailsCount);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_AttachLeaderboardUGC(global::Steamworks.SteamLeaderboard_t hSteamLeaderboard, global::Steamworks.UGCHandle_t hUGC);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_GetNumberOfCurrentPlayers();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestGlobalAchievementPercentages();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetMostAchievedAchievementInfo(global::System.IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetNextMostAchievedAchievementInfo(int iIteratorPrevious, global::System.IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievementAchievedPercent(global::Steamworks.InteropHelp.UTF8StringHandle pchName, out float pflPercent);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestGlobalStats(int nHistoryDays);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetGlobalStat(global::Steamworks.InteropHelp.UTF8StringHandle pchStatName, out long pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetGlobalStat_(global::Steamworks.InteropHelp.UTF8StringHandle pchStatName, out double pData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetGlobalStatHistory(global::Steamworks.InteropHelp.UTF8StringHandle pchStatName, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] long[] pData, uint cubData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetGlobalStatHistory_(global::Steamworks.InteropHelp.UTF8StringHandle pchStatName, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] double[] pData, uint cubData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetSecondsSinceAppActive();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetSecondsSinceComputerActive();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EUniverse ISteamUtils_GetConnectedUniverse();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetServerRealTime();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamUtils_GetIPCountry();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetImageRGBA(int iImage, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubDest, int nDestBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern byte ISteamUtils_GetCurrentBatteryPower();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetAppID();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetOverlayNotificationPosition(global::Steamworks.ENotificationPosition eNotificationPosition);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsAPICallCompleted(global::Steamworks.SteamAPICall_t hSteamAPICall, out bool pbFailed);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ESteamAPICallFailure ISteamUtils_GetAPICallFailureReason(global::Steamworks.SteamAPICall_t hSteamAPICall);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetAPICallResult(global::Steamworks.SteamAPICall_t hSteamAPICall, global::System.IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetIPCCallCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetWarningMessageHook(global::Steamworks.SteamAPIWarningMessageHook_t pFunction);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsOverlayEnabled();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_BOverlayNeedsPresent();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamUtils_CheckFileSignature(global::Steamworks.InteropHelp.UTF8StringHandle szFileName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_ShowGamepadTextInput(global::Steamworks.EGamepadTextInputMode eInputMode, global::Steamworks.EGamepadTextInputLineMode eLineInputMode, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, global::Steamworks.InteropHelp.UTF8StringHandle pchExistingText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetEnteredGamepadTextLength();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetEnteredGamepadTextInput(global::System.IntPtr pchText, uint cchText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamUtils_GetSteamUILanguage();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsSteamRunningInVR();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamVideo_GetVideoURL(global::Steamworks.AppId_t unVideoAppID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamVideo_IsBroadcasting(out int pnNumViewers);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerHTTP_CreateHTTPRequest(global::Steamworks.EHTTPMethod eHTTPRequestMethod, global::Steamworks.InteropHelp.UTF8StringHandle pchAbsoluteURL);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestContextValue(global::Steamworks.HTTPRequestHandle hRequest, ulong ulContextValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestNetworkActivityTimeout(global::Steamworks.HTTPRequestHandle hRequest, uint unTimeoutSeconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestHeaderValue(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderName, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestGetOrPostParameter(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchParamName, global::Steamworks.InteropHelp.UTF8StringHandle pchParamValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SendHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest, out global::Steamworks.SteamAPICall_t pCallHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SendHTTPRequestAndStreamResponse(global::Steamworks.HTTPRequestHandle hRequest, out global::Steamworks.SteamAPICall_t pCallHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_DeferHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_PrioritizeHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderSize(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderValue(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchHeaderName, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pHeaderValueBuffer, uint unBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodySize(global::Steamworks.HTTPRequestHandle hRequest, out uint unBodySize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodyData(global::Steamworks.HTTPRequestHandle hRequest, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pBodyDataBuffer, uint unBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPStreamingResponseBodyData(global::Steamworks.HTTPRequestHandle hRequest, uint cOffset, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pBodyDataBuffer, uint unBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_ReleaseHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPDownloadProgressPct(global::Steamworks.HTTPRequestHandle hRequest, out float pflPercentOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestRawPostBody(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchContentType, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubBody, uint unBodyLen);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerHTTP_CreateCookieContainer([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowResponsesToModify);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_ReleaseCookieContainer(global::Steamworks.HTTPCookieContainerHandle hCookieContainer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetCookie(global::Steamworks.HTTPCookieContainerHandle hCookieContainer, global::Steamworks.InteropHelp.UTF8StringHandle pchHost, global::Steamworks.InteropHelp.UTF8StringHandle pchUrl, global::Steamworks.InteropHelp.UTF8StringHandle pchCookie);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestCookieContainer(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.HTTPCookieContainerHandle hCookieContainer);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestUserAgentInfo(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.InteropHelp.UTF8StringHandle pchUserAgentInfo);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestRequiresVerifiedCertificate(global::Steamworks.HTTPRequestHandle hRequest, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bRequireVerifiedCertificate);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestAbsoluteTimeoutMS(global::Steamworks.HTTPRequestHandle hRequest, uint unMilliseconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPRequestWasTimedOut(global::Steamworks.HTTPRequestHandle hRequest, out bool pbWasTimedOut);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EResult ISteamGameServerInventory_GetResultStatus(global::Steamworks.SteamInventoryResult_t resultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetResultItems(global::Steamworks.SteamInventoryResult_t resultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerInventory_GetResultTimestamp(global::Steamworks.SteamInventoryResult_t resultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_CheckResultSteamID(global::Steamworks.SteamInventoryResult_t resultHandle, global::Steamworks.CSteamID steamIDExpected);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServerInventory_DestroyResult(global::Steamworks.SteamInventoryResult_t resultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetAllItems(out global::Steamworks.SteamInventoryResult_t pResultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemsByID(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_SerializeResult(global::Steamworks.SteamInventoryResult_t resultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pOutBuffer, out uint punOutBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_DeserializeResult(out global::Steamworks.SteamInventoryResult_t pOutResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pBuffer, uint unBufferSize, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GenerateItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pArrayItemDefs, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] punArrayQuantity, uint unArrayLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GrantPromoItems(out global::Steamworks.SteamInventoryResult_t pResultHandle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_AddPromoItem(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t itemDef);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_AddPromoItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_ConsumeItem(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t itemConsume, uint unQuantity);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_ExchangeItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pArrayGenerate, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pArrayDestroy, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TransferItemQuantity(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t itemIdSource, uint unQuantity, global::Steamworks.SteamItemInstanceID_t itemIdDest);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServerInventory_SendItemDropHeartbeat();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TriggerItemDrop(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t dropListDefinition);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TradeItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.CSteamID steamIDTradePartner, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pArrayGive, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemInstanceID_t[] pArrayGet, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_LoadItemDefinitions();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemDefinitionIDs([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemDefinitionProperty(global::Steamworks.SteamItemDef_t iDefinition, global::Steamworks.InteropHelp.UTF8StringHandle pchPropertyName, global::System.IntPtr pchValueBuffer, ref uint punValueBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_SendP2PPacket(global::Steamworks.CSteamID steamIDRemote, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubData, uint cubData, global::Steamworks.EP2PSend eP2PSendType, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_ReadP2PPacket([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out global::Steamworks.CSteamID psteamIDRemote, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_AcceptP2PSessionWithUser(global::Steamworks.CSteamID steamIDRemote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_CloseP2PSessionWithUser(global::Steamworks.CSteamID steamIDRemote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_CloseP2PChannelWithUser(global::Steamworks.CSteamID steamIDRemote, int nChannel);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetP2PSessionState(global::Steamworks.CSteamID steamIDRemote, out global::Steamworks.P2PSessionState_t pConnectionState);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_AllowP2PPacketRelay([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllow);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateP2PConnectionSocket(global::Steamworks.CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_DestroySocket(global::Steamworks.SNetSocket_t hSocket, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bNotifyRemoteEnd);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_DestroyListenSocket(global::Steamworks.SNetListenSocket_t hSocket, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bNotifyRemoteEnd);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_SendDataOnSocket(global::Steamworks.SNetSocket_t hSocket, global::System.IntPtr pubData, uint cubData, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReliable);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsDataAvailableOnSocket(global::Steamworks.SNetSocket_t hSocket, out uint pcubMsgSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_RetrieveDataFromSocket(global::Steamworks.SNetSocket_t hSocket, global::System.IntPtr pubDest, uint cubDest, out uint pcubMsgSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsDataAvailable(global::Steamworks.SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out global::Steamworks.SNetSocket_t phSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_RetrieveData(global::Steamworks.SNetListenSocket_t hListenSocket, global::System.IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out global::Steamworks.SNetSocket_t phSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetSocketInfo(global::Steamworks.SNetSocket_t hSocket, out global::Steamworks.CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetListenSocketInfo(global::Steamworks.SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ESNetSocketConnectionType ISteamGameServerNetworking_GetSocketConnectionType(global::Steamworks.SNetSocket_t hSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern int ISteamGameServerNetworking_GetMaxPacketSize(global::Steamworks.SNetSocket_t hSocket);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryUserUGCRequest(global::Steamworks.AccountID_t unAccountID, global::Steamworks.EUserUGCList eListType, global::Steamworks.EUGCMatchingUGCType eMatchingUGCType, global::Steamworks.EUserUGCListSortOrder eSortOrder, global::Steamworks.AppId_t nCreatorAppID, global::Steamworks.AppId_t nConsumerAppID, uint unPage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryAllUGCRequest(global::Steamworks.EUGCQuery eQueryType, global::Steamworks.EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, global::Steamworks.AppId_t nCreatorAppID, global::Steamworks.AppId_t nConsumerAppID, uint unPage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryUGCDetailsRequest([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SendQueryUGCRequest(global::Steamworks.UGCQueryHandle_t handle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCResult(global::Steamworks.UGCQueryHandle_t handle, uint index, out global::Steamworks.SteamUGCDetails_t pDetails);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCPreviewURL(global::Steamworks.UGCQueryHandle_t handle, uint index, global::System.IntPtr pchURL, uint cchURLSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCMetadata(global::Steamworks.UGCQueryHandle_t handle, uint index, global::System.IntPtr pchMetadata, uint cchMetadatasize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCChildren(global::Steamworks.UGCQueryHandle_t handle, uint index, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCStatistic(global::Steamworks.UGCQueryHandle_t handle, uint index, global::Steamworks.EItemStatistic eStatType, out uint pStatValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetQueryUGCNumAdditionalPreviews(global::Steamworks.UGCQueryHandle_t handle, uint index);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCAdditionalPreview(global::Steamworks.UGCQueryHandle_t handle, uint index, uint previewIndex, global::System.IntPtr pchURLOrVideoID, uint cchURLSize, out bool pbIsImage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetQueryUGCNumKeyValueTags(global::Steamworks.UGCQueryHandle_t handle, uint index);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCKeyValueTag(global::Steamworks.UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, global::System.IntPtr pchKey, uint cchKeySize, global::System.IntPtr pchValue, uint cchValueSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_ReleaseQueryUGCRequest(global::Steamworks.UGCQueryHandle_t handle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddRequiredTag(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pTagName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddExcludedTag(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pTagName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnKeyValueTags(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnKeyValueTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnLongDescription(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnLongDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnMetadata(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnMetadata);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnChildren(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnChildren);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnAdditionalPreviews(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnAdditionalPreviews);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnTotalOnly(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bReturnTotalOnly);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetLanguage(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchLanguage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetAllowCachedResponse(global::Steamworks.UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetCloudFileNameFilter(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pMatchCloudFileName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetMatchAnyTag(global::Steamworks.UGCQueryHandle_t handle, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bMatchAnyTag);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetSearchText(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pSearchText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetRankedByTrendDays(global::Steamworks.UGCQueryHandle_t handle, uint unDays);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddRequiredKeyValueTag(global::Steamworks.UGCQueryHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pKey, global::Steamworks.InteropHelp.UTF8StringHandle pValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RequestUGCDetails(global::Steamworks.PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateItem(global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.EWorkshopFileType eFileType);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_StartItemUpdate(global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemTitle(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchTitle);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemDescription(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemUpdateLanguage(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchLanguage);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemMetadata(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchMetaData);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemVisibility(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemTags(global::Steamworks.UGCUpdateHandle_t updateHandle, global::System.IntPtr pTags);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemContent(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pszContentFolder);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemPreview(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pszPreviewFile);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_RemoveItemKeyValueTags(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchKey);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddItemKeyValueTag(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchKey, global::Steamworks.InteropHelp.UTF8StringHandle pchValue);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SubmitItemUpdate(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.InteropHelp.UTF8StringHandle pchChangeNote);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EItemUpdateStatus ISteamGameServerUGC_GetItemUpdateProgress(global::Steamworks.UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SetUserItemVote(global::Steamworks.PublishedFileId_t nPublishedFileID, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bVoteUp);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_GetUserItemVote(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_AddItemToFavorites(global::Steamworks.AppId_t nAppId, global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RemoveItemFromFavorites(global::Steamworks.AppId_t nAppId, global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SubscribeItem(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_UnsubscribeItem(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetNumSubscribedItems();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetSubscribedItems([global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetItemState(global::Steamworks.PublishedFileId_t nPublishedFileID);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetItemInstallInfo(global::Steamworks.PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, global::System.IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetItemDownloadInfo(global::Steamworks.PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_DownloadItem(global::Steamworks.PublishedFileId_t nPublishedFileID, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bHighPriority);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_BInitWorkshopForGameServer(global::Steamworks.DepotId_t unWorkshopDepotID, global::Steamworks.InteropHelp.UTF8StringHandle pszFolder);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUGC_SuspendDownloads([global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool bSuspend);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetSecondsSinceAppActive();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetSecondsSinceComputerActive();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.EUniverse ISteamGameServerUtils_GetConnectedUniverse();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetServerRealTime();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamGameServerUtils_GetIPCountry();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetImageRGBA(int iImage, [global::System.Runtime.InteropServices.In] [global::System.Runtime.InteropServices.Out] byte[] pubDest, int nDestBufferSize);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern byte ISteamGameServerUtils_GetCurrentBatteryPower();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetAppID();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetOverlayNotificationPosition(global::Steamworks.ENotificationPosition eNotificationPosition);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsAPICallCompleted(global::Steamworks.SteamAPICall_t hSteamAPICall, out bool pbFailed);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::Steamworks.ESteamAPICallFailure ISteamGameServerUtils_GetAPICallFailureReason(global::Steamworks.SteamAPICall_t hSteamAPICall);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetAPICallResult(global::Steamworks.SteamAPICall_t hSteamAPICall, global::System.IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetIPCCallCount();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetWarningMessageHook(global::Steamworks.SteamAPIWarningMessageHook_t pFunction);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsOverlayEnabled();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_BOverlayNeedsPresent();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUtils_CheckFileSignature(global::Steamworks.InteropHelp.UTF8StringHandle szFileName);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_ShowGamepadTextInput(global::Steamworks.EGamepadTextInputMode eInputMode, global::Steamworks.EGamepadTextInputLineMode eLineInputMode, global::Steamworks.InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, global::Steamworks.InteropHelp.UTF8StringHandle pchExistingText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetEnteredGamepadTextLength();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetEnteredGamepadTextInput(global::System.IntPtr pchText, uint cchText);

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern global::System.IntPtr ISteamGameServerUtils_GetSteamUILanguage();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsSteamRunningInVR();

		[global::System.Runtime.InteropServices.DllImport("CSteamworks", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);

		internal const string NativeLibraryName = "CSteamworks";
	}
}
