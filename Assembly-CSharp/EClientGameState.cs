﻿using System;

internal enum EClientGameState
{
	k_EClientGameStartServer,
	k_EClientGameActive,
	k_EClientGameWaitingForPlayers,
	k_EClientGameMenu,
	k_EClientGameQuitMenu,
	k_EClientGameExiting,
	k_EClientGameInstructions,
	k_EClientGameDraw,
	k_EClientGameWinner,
	k_EClientGameConnecting,
	k_EClientGameConnectionFailure,
	k_EClientFindInternetServers,
	k_EClientStatsAchievements,
	k_EClientCreatingLobby,
	k_EClientInLobby,
	k_EClientFindLobby,
	k_EClientJoiningLobby,
	k_EClientFindLANServers,
	k_EClientRemoteStorage,
	k_EClientLeaderboards,
	k_EClientMinidump,
	k_EClientConnectingToSteam,
	k_EClientLinkSteamAccount,
	k_EClientAutoCreateAccount,
	k_EClientRetrySteamConnection,
	k_EClientClanChatRoom,
	k_EClientWebCallback
}
