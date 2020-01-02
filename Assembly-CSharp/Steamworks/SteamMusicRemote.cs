using System;

namespace Steamworks
{
	public static class SteamMusicRemote
	{
		public static bool RegisterSteamMusicRemote(string pchName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchName))
			{
				result = global::Steamworks.NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(utf8StringHandle);
			}
			return result;
		}

		public static bool DeregisterSteamMusicRemote()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote();
		}

		public static bool BIsCurrentMusicRemote()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote();
		}

		public static bool BActivationSuccess(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_BActivationSuccess(bValue);
		}

		public static bool SetDisplayName(string pchDisplayName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDisplayName))
			{
				result = global::Steamworks.NativeMethods.ISteamMusicRemote_SetDisplayName(utf8StringHandle);
			}
			return result;
		}

		public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(pvBuffer, cbBufferLength);
		}

		public static bool EnablePlayPrevious(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_EnablePlayPrevious(bValue);
		}

		public static bool EnablePlayNext(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_EnablePlayNext(bValue);
		}

		public static bool EnableShuffled(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_EnableShuffled(bValue);
		}

		public static bool EnableLooped(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_EnableLooped(bValue);
		}

		public static bool EnableQueue(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_EnableQueue(bValue);
		}

		public static bool EnablePlaylists(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_EnablePlaylists(bValue);
		}

		public static bool UpdatePlaybackStatus(global::Steamworks.AudioPlayback_Status nStatus)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(nStatus);
		}

		public static bool UpdateShuffled(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_UpdateShuffled(bValue);
		}

		public static bool UpdateLooped(bool bValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_UpdateLooped(bValue);
		}

		public static bool UpdateVolume(float flValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_UpdateVolume(flValue);
		}

		public static bool CurrentEntryWillChange()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_CurrentEntryWillChange();
		}

		public static bool CurrentEntryIsAvailable(bool bAvailable)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(bAvailable);
		}

		public static bool UpdateCurrentEntryText(string pchText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchText))
			{
				result = global::Steamworks.NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(utf8StringHandle);
			}
			return result;
		}

		public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(nValue);
		}

		public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(pvBuffer, cbBufferLength);
		}

		public static bool CurrentEntryDidChange()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_CurrentEntryDidChange();
		}

		public static bool QueueWillChange()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_QueueWillChange();
		}

		public static bool ResetQueueEntries()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_ResetQueueEntries();
		}

		public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = global::Steamworks.NativeMethods.ISteamMusicRemote_SetQueueEntry(nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		public static bool SetCurrentQueueEntry(int nID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(nID);
		}

		public static bool QueueDidChange()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_QueueDidChange();
		}

		public static bool PlaylistWillChange()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_PlaylistWillChange();
		}

		public static bool ResetPlaylistEntries()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_ResetPlaylistEntries();
		}

		public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = global::Steamworks.NativeMethods.ISteamMusicRemote_SetPlaylistEntry(nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		public static bool SetCurrentPlaylistEntry(int nID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(nID);
		}

		public static bool PlaylistDidChange()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusicRemote_PlaylistDidChange();
		}
	}
}
