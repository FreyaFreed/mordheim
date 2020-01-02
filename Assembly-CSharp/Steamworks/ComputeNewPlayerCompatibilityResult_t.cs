using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[global::Steamworks.CallbackIdentity(211)]
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct ComputeNewPlayerCompatibilityResult_t
	{
		public const int k_iCallback = 211;

		public global::Steamworks.EResult m_eResult;

		public int m_cPlayersThatDontLikeCandidate;

		public int m_cPlayersThatCandidateDoesntLike;

		public int m_cClanPlayersThatDontLikeCandidate;

		public global::Steamworks.CSteamID m_SteamIDCandidate;
	}
}
