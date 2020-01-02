using System;

namespace Steamworks
{
	[global::System.Flags]
	public enum ESteamItemFlags
	{
		k_ESteamItemNoTrade = 1,
		k_ESteamItemRemoved = 256,
		k_ESteamItemConsumed = 512
	}
}
