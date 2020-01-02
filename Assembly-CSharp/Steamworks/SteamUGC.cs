using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamUGC
	{
		public static global::Steamworks.UGCQueryHandle_t CreateQueryUserUGCRequest(global::Steamworks.AccountID_t unAccountID, global::Steamworks.EUserUGCList eListType, global::Steamworks.EUGCMatchingUGCType eMatchingUGCType, global::Steamworks.EUserUGCListSortOrder eSortOrder, global::Steamworks.AppId_t nCreatorAppID, global::Steamworks.AppId_t nConsumerAppID, uint unPage)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.UGCQueryHandle_t)global::Steamworks.NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		public static global::Steamworks.UGCQueryHandle_t CreateQueryAllUGCRequest(global::Steamworks.EUGCQuery eQueryType, global::Steamworks.EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, global::Steamworks.AppId_t nCreatorAppID, global::Steamworks.AppId_t nConsumerAppID, uint unPage)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.UGCQueryHandle_t)global::Steamworks.NativeMethods.ISteamUGC_CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		public static global::Steamworks.UGCQueryHandle_t CreateQueryUGCDetailsRequest(global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.UGCQueryHandle_t)global::Steamworks.NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public static global::Steamworks.SteamAPICall_t SendQueryUGCRequest(global::Steamworks.UGCQueryHandle_t handle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_SendQueryUGCRequest(handle);
		}

		public static bool GetQueryUGCResult(global::Steamworks.UGCQueryHandle_t handle, uint index, out global::Steamworks.SteamUGCDetails_t pDetails)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCResult(handle, index, out pDetails);
		}

		public static bool GetQueryUGCPreviewURL(global::Steamworks.UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(handle, index, intPtr, cchURLSize);
			pchURL = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static bool GetQueryUGCMetadata(global::Steamworks.UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCMetadata(handle, index, intPtr, cchMetadatasize);
			pchMetadata = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static bool GetQueryUGCChildren(global::Steamworks.UGCQueryHandle_t handle, uint index, global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
		}

		public static bool GetQueryUGCStatistic(global::Steamworks.UGCQueryHandle_t handle, uint index, global::Steamworks.EItemStatistic eStatType, out uint pStatValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
		}

		public static uint GetQueryUGCNumAdditionalPreviews(global::Steamworks.UGCQueryHandle_t handle, uint index)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(handle, index);
		}

		public static bool GetQueryUGCAdditionalPreview(global::Steamworks.UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out bool pbIsImage)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(handle, index, previewIndex, intPtr, cchURLSize, out pbIsImage);
			pchURLOrVideoID = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static uint GetQueryUGCNumKeyValueTags(global::Steamworks.UGCQueryHandle_t handle, uint index)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(handle, index);
		}

		public static bool GetQueryUGCKeyValueTag(global::Steamworks.UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchKeySize);
			global::System.IntPtr intPtr2 = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = global::Steamworks.NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			pchValue = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr2));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		public static bool ReleaseQueryUGCRequest(global::Steamworks.UGCQueryHandle_t handle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(handle);
		}

		public static bool AddRequiredTag(global::Steamworks.UGCQueryHandle_t handle, string pTagName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pTagName))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_AddRequiredTag(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool AddExcludedTag(global::Steamworks.UGCQueryHandle_t handle, string pTagName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pTagName))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_AddExcludedTag(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetReturnKeyValueTags(global::Steamworks.UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetReturnKeyValueTags(handle, bReturnKeyValueTags);
		}

		public static bool SetReturnLongDescription(global::Steamworks.UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetReturnLongDescription(handle, bReturnLongDescription);
		}

		public static bool SetReturnMetadata(global::Steamworks.UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetReturnMetadata(handle, bReturnMetadata);
		}

		public static bool SetReturnChildren(global::Steamworks.UGCQueryHandle_t handle, bool bReturnChildren)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetReturnChildren(handle, bReturnChildren);
		}

		public static bool SetReturnAdditionalPreviews(global::Steamworks.UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
		}

		public static bool SetReturnTotalOnly(global::Steamworks.UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetReturnTotalOnly(handle, bReturnTotalOnly);
		}

		public static bool SetLanguage(global::Steamworks.UGCQueryHandle_t handle, string pchLanguage)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchLanguage))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetLanguage(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetAllowCachedResponse(global::Steamworks.UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetAllowCachedResponse(handle, unMaxAgeSeconds);
		}

		public static bool SetCloudFileNameFilter(global::Steamworks.UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetCloudFileNameFilter(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetMatchAnyTag(global::Steamworks.UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetMatchAnyTag(handle, bMatchAnyTag);
		}

		public static bool SetSearchText(global::Steamworks.UGCQueryHandle_t handle, string pSearchText)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pSearchText))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetSearchText(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetRankedByTrendDays(global::Steamworks.UGCQueryHandle_t handle, uint unDays)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetRankedByTrendDays(handle, unDays);
		}

		public static bool AddRequiredKeyValueTag(global::Steamworks.UGCQueryHandle_t handle, string pKey, string pValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pValue))
				{
					result = global::Steamworks.NativeMethods.ISteamUGC_AddRequiredKeyValueTag(handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t RequestUGCDetails(global::Steamworks.PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
		}

		public static global::Steamworks.SteamAPICall_t CreateItem(global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.EWorkshopFileType eFileType)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_CreateItem(nConsumerAppId, eFileType);
		}

		public static global::Steamworks.UGCUpdateHandle_t StartItemUpdate(global::Steamworks.AppId_t nConsumerAppId, global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.UGCUpdateHandle_t)global::Steamworks.NativeMethods.ISteamUGC_StartItemUpdate(nConsumerAppId, nPublishedFileID);
		}

		public static bool SetItemTitle(global::Steamworks.UGCUpdateHandle_t handle, string pchTitle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchTitle))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetItemTitle(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetItemDescription(global::Steamworks.UGCUpdateHandle_t handle, string pchDescription)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchDescription))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetItemDescription(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetItemUpdateLanguage(global::Steamworks.UGCUpdateHandle_t handle, string pchLanguage)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchLanguage))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetItemUpdateLanguage(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetItemMetadata(global::Steamworks.UGCUpdateHandle_t handle, string pchMetaData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchMetaData))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetItemMetadata(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetItemVisibility(global::Steamworks.UGCUpdateHandle_t handle, global::Steamworks.ERemoteStoragePublishedFileVisibility eVisibility)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetItemVisibility(handle, eVisibility);
		}

		public static bool SetItemTags(global::Steamworks.UGCUpdateHandle_t updateHandle, global::System.Collections.Generic.IList<string> pTags)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_SetItemTags(updateHandle, new global::Steamworks.InteropHelp.SteamParamStringArray(pTags));
		}

		public static bool SetItemContent(global::Steamworks.UGCUpdateHandle_t handle, string pszContentFolder)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetItemContent(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool SetItemPreview(global::Steamworks.UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_SetItemPreview(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool RemoveItemKeyValueTags(global::Steamworks.UGCUpdateHandle_t handle, string pchKey)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_RemoveItemKeyValueTags(handle, utf8StringHandle);
			}
			return result;
		}

		public static bool AddItemKeyValueTag(global::Steamworks.UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValue))
				{
					result = global::Steamworks.NativeMethods.ISteamUGC_AddItemKeyValueTag(handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static global::Steamworks.SteamAPICall_t SubmitItemUpdate(global::Steamworks.UGCUpdateHandle_t handle, string pchChangeNote)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_SubmitItemUpdate(handle, utf8StringHandle);
			}
			return result;
		}

		public static global::Steamworks.EItemUpdateStatus GetItemUpdateProgress(global::Steamworks.UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
		}

		public static global::Steamworks.SteamAPICall_t SetUserItemVote(global::Steamworks.PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_SetUserItemVote(nPublishedFileID, bVoteUp);
		}

		public static global::Steamworks.SteamAPICall_t GetUserItemVote(global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_GetUserItemVote(nPublishedFileID);
		}

		public static global::Steamworks.SteamAPICall_t AddItemToFavorites(global::Steamworks.AppId_t nAppId, global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_AddItemToFavorites(nAppId, nPublishedFileID);
		}

		public static global::Steamworks.SteamAPICall_t RemoveItemFromFavorites(global::Steamworks.AppId_t nAppId, global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_RemoveItemFromFavorites(nAppId, nPublishedFileID);
		}

		public static global::Steamworks.SteamAPICall_t SubscribeItem(global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_SubscribeItem(nPublishedFileID);
		}

		public static global::Steamworks.SteamAPICall_t UnsubscribeItem(global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamUGC_UnsubscribeItem(nPublishedFileID);
		}

		public static uint GetNumSubscribedItems()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetNumSubscribedItems();
		}

		public static uint GetSubscribedItems(global::Steamworks.PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
		}

		public static uint GetItemState(global::Steamworks.PublishedFileId_t nPublishedFileID)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetItemState(nPublishedFileID);
		}

		public static bool GetItemInstallInfo(global::Steamworks.PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = global::Steamworks.NativeMethods.ISteamUGC_GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
			global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		public static bool GetItemDownloadInfo(global::Steamworks.PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		public static bool DownloadItem(global::Steamworks.PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamUGC_DownloadItem(nPublishedFileID, bHighPriority);
		}

		public static bool BInitWorkshopForGameServer(global::Steamworks.DepotId_t unWorkshopDepotID, string pszFolder)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pszFolder))
			{
				result = global::Steamworks.NativeMethods.ISteamUGC_BInitWorkshopForGameServer(unWorkshopDepotID, utf8StringHandle);
			}
			return result;
		}

		public static void SuspendDownloads(bool bSuspend)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamUGC_SuspendDownloads(bSuspend);
		}
	}
}
