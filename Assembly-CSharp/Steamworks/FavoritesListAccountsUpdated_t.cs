using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(516)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListAccountsUpdated_t
	{
		public const int k_iCallback = 516;

		public global::Steamworks.EResult m_eResult;
	}
}
