﻿using System;

namespace Steamworks
{
	[global::System.Flags]
	public enum EItemState
	{
		k_EItemStateNone = 0,
		k_EItemStateSubscribed = 1,
		k_EItemStateLegacyItem = 2,
		k_EItemStateInstalled = 4,
		k_EItemStateNeedsUpdate = 8,
		k_EItemStateDownloading = 16,
		k_EItemStateDownloadPending = 32
	}
}
