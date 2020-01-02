using System;

namespace Steamworks
{
	[global::System.Flags]
	public enum EChatSteamIDInstanceFlags
	{
		k_EChatAccountInstanceMask = 4095,
		k_EChatInstanceFlagClan = 524288,
		k_EChatInstanceFlagLobby = 262144,
		k_EChatInstanceFlagMMSLobby = 131072
	}
}
