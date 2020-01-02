using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamGameServerInventory
	{
		public static global::Steamworks.EResult GetResultStatus(global::Steamworks.SteamInventoryResult_t resultHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GetResultStatus(resultHandle);
		}

		public static bool GetResultItems(global::Steamworks.SteamInventoryResult_t resultHandle, global::Steamworks.SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GetResultItems(resultHandle, pOutItemsArray, ref punOutItemsArraySize);
		}

		public static uint GetResultTimestamp(global::Steamworks.SteamInventoryResult_t resultHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GetResultTimestamp(resultHandle);
		}

		public static bool CheckResultSteamID(global::Steamworks.SteamInventoryResult_t resultHandle, global::Steamworks.CSteamID steamIDExpected)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_CheckResultSteamID(resultHandle, steamIDExpected);
		}

		public static void DestroyResult(global::Steamworks.SteamInventoryResult_t resultHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServerInventory_DestroyResult(resultHandle);
		}

		public static bool GetAllItems(out global::Steamworks.SteamInventoryResult_t pResultHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GetAllItems(out pResultHandle);
		}

		public static bool GetItemsByID(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GetItemsByID(out pResultHandle, pInstanceIDs, unCountInstanceIDs);
		}

		public static bool SerializeResult(global::Steamworks.SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_SerializeResult(resultHandle, pOutBuffer, out punOutBufferSize);
		}

		public static bool DeserializeResult(out global::Steamworks.SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, bool bRESERVED_MUST_BE_FALSE = false)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_DeserializeResult(out pOutResultHandle, pBuffer, unBufferSize, bRESERVED_MUST_BE_FALSE);
		}

		public static bool GenerateItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t[] pArrayItemDefs, uint[] punArrayQuantity, uint unArrayLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GenerateItems(out pResultHandle, pArrayItemDefs, punArrayQuantity, unArrayLength);
		}

		public static bool GrantPromoItems(out global::Steamworks.SteamInventoryResult_t pResultHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GrantPromoItems(out pResultHandle);
		}

		public static bool AddPromoItem(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t itemDef)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_AddPromoItem(out pResultHandle, itemDef);
		}

		public static bool AddPromoItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t[] pArrayItemDefs, uint unArrayLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_AddPromoItems(out pResultHandle, pArrayItemDefs, unArrayLength);
		}

		public static bool ConsumeItem(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t itemConsume, uint unQuantity)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_ConsumeItem(out pResultHandle, itemConsume, unQuantity);
		}

		public static bool ExchangeItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t[] pArrayGenerate, uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, global::Steamworks.SteamItemInstanceID_t[] pArrayDestroy, uint[] punArrayDestroyQuantity, uint unArrayDestroyLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_ExchangeItems(out pResultHandle, pArrayGenerate, punArrayGenerateQuantity, unArrayGenerateLength, pArrayDestroy, punArrayDestroyQuantity, unArrayDestroyLength);
		}

		public static bool TransferItemQuantity(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemInstanceID_t itemIdSource, uint unQuantity, global::Steamworks.SteamItemInstanceID_t itemIdDest)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_TransferItemQuantity(out pResultHandle, itemIdSource, unQuantity, itemIdDest);
		}

		public static void SendItemDropHeartbeat()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::Steamworks.NativeMethods.ISteamGameServerInventory_SendItemDropHeartbeat();
		}

		public static bool TriggerItemDrop(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.SteamItemDef_t dropListDefinition)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_TriggerItemDrop(out pResultHandle, dropListDefinition);
		}

		public static bool TradeItems(out global::Steamworks.SteamInventoryResult_t pResultHandle, global::Steamworks.CSteamID steamIDTradePartner, global::Steamworks.SteamItemInstanceID_t[] pArrayGive, uint[] pArrayGiveQuantity, uint nArrayGiveLength, global::Steamworks.SteamItemInstanceID_t[] pArrayGet, uint[] pArrayGetQuantity, uint nArrayGetLength)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_TradeItems(out pResultHandle, steamIDTradePartner, pArrayGive, pArrayGiveQuantity, nArrayGiveLength, pArrayGet, pArrayGetQuantity, nArrayGetLength);
		}

		public static bool LoadItemDefinitions()
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_LoadItemDefinitions();
		}

		public static bool GetItemDefinitionIDs(global::Steamworks.SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			return global::Steamworks.NativeMethods.ISteamGameServerInventory_GetItemDefinitionIDs(pItemDefIDs, out punItemDefIDsArraySize);
		}

		public static bool GetItemDefinitionProperty(global::Steamworks.SteamItemDef_t iDefinition, string pchPropertyName, out string pchValueBuffer, ref uint punValueBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableGameServer();
			global::System.IntPtr intPtr = global::System.Runtime.InteropServices.Marshal.AllocHGlobal((int)punValueBufferSize);
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPropertyName))
			{
				bool flag = global::Steamworks.NativeMethods.ISteamGameServerInventory_GetItemDefinitionProperty(iDefinition, utf8StringHandle, intPtr, ref punValueBufferSize);
				pchValueBuffer = ((!flag) ? null : global::Steamworks.InteropHelp.PtrToStringUTF8(intPtr));
				global::System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
				result = flag;
			}
			return result;
		}
	}
}
