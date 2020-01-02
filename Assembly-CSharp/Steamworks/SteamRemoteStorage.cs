using System;
using System.Collections.Generic;

namespace Steamworks
{
	public static class SteamRemoteStorage
	{
		public static bool FileWrite(string pchFile, byte[] pvData, int cubData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_FileWrite(utf8StringHandle, pvData, cubData);
			}
			return result;
		}

		public static int FileRead(string pchFile, byte[] pvData, int cubDataToRead)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			int result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_FileRead(utf8StringHandle, pvData, cubDataToRead);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t FileWriteAsync(string pchFile, byte[] pvData, uint cubData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_FileWriteAsync(utf8StringHandle, pvData, cubData);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t FileReadAsync(string pchFile, uint nOffset, uint cubToRead)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_FileReadAsync(utf8StringHandle, nOffset, cubToRead);
			}
			return result;
		}

		public static bool FileReadAsyncComplete(global::Steamworks.SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_FileReadAsyncComplete(hReadCall, pvBuffer, cubToRead);
		}

		public static bool FileForget(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_FileForget(utf8StringHandle);
			}
			return result;
		}

		public static bool FileDelete(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_FileDelete(utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t FileShare(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_FileShare(utf8StringHandle);
			}
			return result;
		}

		public static bool SetSyncPlatforms(string pchFile, global::Steamworks.ERemoteStoragePlatform eRemoteStoragePlatform)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_SetSyncPlatforms(utf8StringHandle, eRemoteStoragePlatform);
			}
			return result;
		}

		public static global::Steamworks.UGCFileWriteStreamHandle_t FileWriteStreamOpen(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.UGCFileWriteStreamHandle_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (global::Steamworks.UGCFileWriteStreamHandle_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_FileWriteStreamOpen(utf8StringHandle);
			}
			return result;
		}

		public static bool FileWriteStreamWriteChunk(global::Steamworks.UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_FileWriteStreamWriteChunk(writeHandle, pvData, cubData);
		}

		public static bool FileWriteStreamClose(global::Steamworks.UGCFileWriteStreamHandle_t writeHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_FileWriteStreamClose(writeHandle);
		}

		public static bool FileWriteStreamCancel(global::Steamworks.UGCFileWriteStreamHandle_t writeHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_FileWriteStreamCancel(writeHandle);
		}

		public static bool FileExists(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_FileExists(utf8StringHandle);
			}
			return result;
		}

		public static bool FilePersisted(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_FilePersisted(utf8StringHandle);
			}
			return result;
		}

		public static int GetFileSize(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			int result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_GetFileSize(utf8StringHandle);
			}
			return result;
		}

		public static long GetFileTimestamp(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			long result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_GetFileTimestamp(utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.ERemoteStoragePlatform GetSyncPlatforms(string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.ERemoteStoragePlatform result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_GetSyncPlatforms(utf8StringHandle);
			}
			return result;
		}

		public static int GetFileCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_GetFileCount();
		}

		public static string GetFileNameAndSize(int iFile, out int pnFileSizeInBytes)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.InteropHelp.PtrToStringUTF8(global::Steamworks.NativeMethods.ISteamRemoteStorage_GetFileNameAndSize(iFile, out pnFileSizeInBytes));
		}

		public static bool GetQuota(out int pnTotalBytes, out int puAvailableBytes)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_GetQuota(out pnTotalBytes, out puAvailableBytes);
		}

		public static bool IsCloudEnabledForAccount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_IsCloudEnabledForAccount();
		}

		public static bool IsCloudEnabledForApp()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_IsCloudEnabledForApp();
		}

		public static void SetCloudEnabledForApp(bool bEnabled)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamRemoteStorage_SetCloudEnabledForApp(bEnabled);
		}

		public static global::Steamworks.SteamAPICall_t UGCDownload(global::Steamworks.UGCHandle_t hContent, uint unPriority)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_UGCDownload(hContent, unPriority);
		}

		public static bool GetUGCDownloadProgress(global::Steamworks.UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_GetUGCDownloadProgress(hContent, out pnBytesDownloaded, out pnBytesExpected);
		}

		public static bool GetUGCDetails(global::Steamworks.UGCHandle_t hContent, out global::Steamworks.AppId_t pnAppID, out string ppchName, out int pnFileSizeInBytes, out global::Steamworks.CSteamID pSteamIDOwner)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr nativeUtf;
			bool flag = global::Steamworks.NativeMethods.ISteamRemoteStorage_GetUGCDetails(hContent, out pnAppID, out nativeUtf, out pnFileSizeInBytes, out pSteamIDOwner);
			ppchName = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(nativeUtf));
			return flag;
		}

		public static int UGCRead(global::Steamworks.UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset, global::Steamworks.EUGCReadAction eAction)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_UGCRead(hContent, pvData, cubDataToRead, cOffset, eAction);
		}

		public static int GetCachedUGCCount()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_GetCachedUGCCount();
		}

		public static global::Steamworks.UGCHandle_t GetCachedUGCHandle(int iCachedContent)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.UGCHandle_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_GetCachedUGCHandle(iCachedContent);
		}

		public static global::Steamworks.SteamAPICall_t PublishWorkshopFile(string pchFile, string pchPreviewFile, global::Steamworks.AppId_t nConsumerAppId, string pchTitle, string pchDescription, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility, global::System.Collections.Generic.IList<string> pTags, global::Steamworks.EWorkshopFileType eWorkshopFileType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPreviewFile))
				{
					using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle3 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchTitle))
					{
						using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle4 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDescription))
						{
							result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_PublishWorkshopFile(utf8StringHandle, utf8StringHandle2, nConsumerAppId, utf8StringHandle3, utf8StringHandle4, eVisibility, new global::Steamworks.InteropHelp.SteamParamStringArray(pTags), eWorkshopFileType);
						}
					}
				}
			}
			return result;
		}

		public static global::Steamworks.PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest(global::Steamworks.PublishedFileId_t unPublishedFileId)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.PublishedFileUpdateHandle_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_CreatePublishedFileUpdateRequest(unPublishedFileId);
		}

		public static bool UpdatePublishedFileFile(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, string pchFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFileFile(updateHandle, utf8StringHandle);
			}
			return result;
		}

		public static bool UpdatePublishedFilePreviewFile(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, string pchPreviewFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPreviewFile))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFilePreviewFile(updateHandle, utf8StringHandle);
			}
			return result;
		}

		public static bool UpdatePublishedFileTitle(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, string pchTitle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchTitle))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTitle(updateHandle, utf8StringHandle);
			}
			return result;
		}

		public static bool UpdatePublishedFileDescription(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, string pchDescription)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDescription))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFileDescription(updateHandle, utf8StringHandle);
			}
			return result;
		}

		public static bool UpdatePublishedFileVisibility(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFileVisibility(updateHandle, eVisibility);
		}

		public static bool UpdatePublishedFileTags(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, global::System.Collections.Generic.IList<string> pTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTags(updateHandle, new global::Steamworks.InteropHelp.SteamParamStringArray(pTags));
		}

		public static global::Steamworks.SteamAPICall_t CommitPublishedFileUpdate(global::Steamworks.PublishedFileUpdateHandle_t updateHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_CommitPublishedFileUpdate(updateHandle);
		}

		public static global::Steamworks.SteamAPICall_t GetPublishedFileDetails(global::Steamworks.PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_GetPublishedFileDetails(unPublishedFileId, unMaxSecondsOld);
		}

		public static global::Steamworks.SteamAPICall_t DeletePublishedFile(global::Steamworks.PublishedFileId_t unPublishedFileId)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_DeletePublishedFile(unPublishedFileId);
		}

		public static global::Steamworks.SteamAPICall_t EnumerateUserPublishedFiles(uint unStartIndex)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_EnumerateUserPublishedFiles(unStartIndex);
		}

		public static global::Steamworks.SteamAPICall_t SubscribePublishedFile(global::Steamworks.PublishedFileId_t unPublishedFileId)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_SubscribePublishedFile(unPublishedFileId);
		}

		public static global::Steamworks.SteamAPICall_t EnumerateUserSubscribedFiles(uint unStartIndex)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_EnumerateUserSubscribedFiles(unStartIndex);
		}

		public static global::Steamworks.SteamAPICall_t UnsubscribePublishedFile(global::Steamworks.PublishedFileId_t unPublishedFileId)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_UnsubscribePublishedFile(unPublishedFileId);
		}

		public static bool UpdatePublishedFileSetChangeDescription(global::Steamworks.PublishedFileUpdateHandle_t updateHandle, string pchChangeDescription)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchChangeDescription))
			{
				result = global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(updateHandle, utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t GetPublishedItemVoteDetails(global::Steamworks.PublishedFileId_t unPublishedFileId)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_GetPublishedItemVoteDetails(unPublishedFileId);
		}

		public static global::Steamworks.SteamAPICall_t UpdateUserPublishedItemVote(global::Steamworks.PublishedFileId_t unPublishedFileId, bool bVoteUp)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_UpdateUserPublishedItemVote(unPublishedFileId, bVoteUp);
		}

		public static global::Steamworks.SteamAPICall_t GetUserPublishedItemVoteDetails(global::Steamworks.PublishedFileId_t unPublishedFileId)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_GetUserPublishedItemVoteDetails(unPublishedFileId);
		}

		public static global::Steamworks.SteamAPICall_t EnumerateUserSharedWorkshopFiles(global::Steamworks.CSteamID steamId, uint unStartIndex, global::System.Collections.Generic.IList<string> pRequiredTags, global::System.Collections.Generic.IList<string> pExcludedTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(steamId, unStartIndex, new global::Steamworks.InteropHelp.SteamParamStringArray(pRequiredTags), new global::Steamworks.InteropHelp.SteamParamStringArray(pExcludedTags));
		}

		public static global::Steamworks.SteamAPICall_t PublishVideo(global::Steamworks.EWorkshopVideoProvider eVideoProvider, string pchVideoAccount, string pchVideoIdentifier, string pchPreviewFile, global::Steamworks.AppId_t nConsumerAppId, string pchTitle, string pchDescription, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility, global::System.Collections.Generic.IList<string> pTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVideoAccount))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchVideoIdentifier))
				{
					using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle3 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPreviewFile))
					{
						using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle4 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchTitle))
						{
							using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle5 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDescription))
							{
								result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_PublishVideo(eVideoProvider, utf8StringHandle, utf8StringHandle2, utf8StringHandle3, nConsumerAppId, utf8StringHandle4, utf8StringHandle5, eVisibility, new global::Steamworks.InteropHelp.SteamParamStringArray(pTags));
							}
						}
					}
				}
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t SetUserPublishedFileAction(global::Steamworks.PublishedFileId_t unPublishedFileId, global::Steamworks.EWorkshopFileAction eAction)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_SetUserPublishedFileAction(unPublishedFileId, eAction);
		}

		public static global::Steamworks.SteamAPICall_t EnumeratePublishedFilesByUserAction(global::Steamworks.EWorkshopFileAction eAction, uint unStartIndex)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(eAction, unStartIndex);
		}

		public static global::Steamworks.SteamAPICall_t EnumeratePublishedWorkshopFiles(global::Steamworks.EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, global::System.Collections.Generic.IList<string> pTags, global::System.Collections.Generic.IList<string> pUserTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(eEnumerationType, unStartIndex, unCount, unDays, new global::Steamworks.InteropHelp.SteamParamStringArray(pTags), new global::Steamworks.InteropHelp.SteamParamStringArray(pUserTags));
		}

		public static global::Steamworks.SteamAPICall_t UGCDownloadToLocation(global::Steamworks.UGCHandle_t hContent, string pchLocation, uint unPriority)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchLocation))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamRemoteStorage_UGCDownloadToLocation(hContent, utf8StringHandle, unPriority);
			}
			return result;
		}
	}
}
