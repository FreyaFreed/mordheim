using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct SteamItemDetails_t
	{
		public global::Steamworks.SteamItemInstanceID_t m_itemId;

		public global::Steamworks.SteamItemDef_t m_iDefinition;

		public ushort m_unQuantity;

		public ushort m_unFlags;
	}
}
