using System;

namespace Steamworks
{
	public static class SteamMusic
	{
		public static bool BIsEnabled()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusic_BIsEnabled();
		}

		public static bool BIsPlaying()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusic_BIsPlaying();
		}

		public static global::Steamworks.AudioPlayback_Status GetPlaybackStatus()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusic_GetPlaybackStatus();
		}

		public static void Play()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMusic_Play();
		}

		public static void Pause()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMusic_Pause();
		}

		public static void PlayPrevious()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMusic_PlayPrevious();
		}

		public static void PlayNext()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMusic_PlayNext();
		}

		public static void SetVolume(float flVolume)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamMusic_SetVolume(flVolume);
		}

		public static float GetVolume()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamMusic_GetVolume();
		}
	}
}
