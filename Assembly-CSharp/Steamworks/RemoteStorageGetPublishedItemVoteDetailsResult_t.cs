using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(1320)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
	{
		public const int k_iCallback = 1320;

		public global::Steamworks.EResult m_eResult;

		public global::Steamworks.PublishedFileId_t m_unPublishedFileId;

		public int m_nVotesFor;

		public int m_nVotesAgainst;

		public int m_nReports;

		public float m_fScore;
	}
}
