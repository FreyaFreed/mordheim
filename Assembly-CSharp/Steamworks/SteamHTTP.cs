using System;

namespace Steamworks
{
	public static class SteamHTTP
	{
		public static global::Steamworks.HTTPRequestHandle CreateHTTPRequest(global::Steamworks.EHTTPMethod eHTTPRequestMethod, string pchAbsoluteURL)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.HTTPRequestHandle result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchAbsoluteURL))
			{
				result = (global::Steamworks.HTTPRequestHandle)global::Steamworks.NativeMethods.ISteamHTTP_CreateHTTPRequest(eHTTPRequestMethod, utf8StringHandle);
			}
			return result;
		}

		public static bool SetHTTPRequestContextValue(global::Steamworks.HTTPRequestHandle hRequest, ulong ulContextValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestContextValue(hRequest, ulContextValue);
		}

		public static bool SetHTTPRequestNetworkActivityTimeout(global::Steamworks.HTTPRequestHandle hRequest, uint unTimeoutSeconds)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(hRequest, unTimeoutSeconds);
		}

		public static bool SetHTTPRequestHeaderValue(global::Steamworks.HTTPRequestHandle hRequest, string pchHeaderName, string pchHeaderValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchHeaderValue))
				{
					result = global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestHeaderValue(hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static bool SetHTTPRequestGetOrPostParameter(global::Steamworks.HTTPRequestHandle hRequest, string pchParamName, string pchParamValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchParamName))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchParamValue))
				{
					result = global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestGetOrPostParameter(hRequest, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static bool SendHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest, out global::Steamworks.SteamAPICall_t pCallHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SendHTTPRequest(hRequest, out pCallHandle);
		}

		public static bool SendHTTPRequestAndStreamResponse(global::Steamworks.HTTPRequestHandle hRequest, out global::Steamworks.SteamAPICall_t pCallHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SendHTTPRequestAndStreamResponse(hRequest, out pCallHandle);
		}

		public static bool DeferHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_DeferHTTPRequest(hRequest);
		}

		public static bool PrioritizeHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_PrioritizeHTTPRequest(hRequest);
		}

		public static bool GetHTTPResponseHeaderSize(global::Steamworks.HTTPRequestHandle hRequest, string pchHeaderName, out uint unResponseHeaderSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				result = global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPResponseHeaderSize(hRequest, utf8StringHandle, out unResponseHeaderSize);
			}
			return result;
		}

		public static bool GetHTTPResponseHeaderValue(global::Steamworks.HTTPRequestHandle hRequest, string pchHeaderName, byte[] pHeaderValueBuffer, uint unBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchHeaderName))
			{
				result = global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPResponseHeaderValue(hRequest, utf8StringHandle, pHeaderValueBuffer, unBufferSize);
			}
			return result;
		}

		public static bool GetHTTPResponseBodySize(global::Steamworks.HTTPRequestHandle hRequest, out uint unBodySize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPResponseBodySize(hRequest, out unBodySize);
		}

		public static bool GetHTTPResponseBodyData(global::Steamworks.HTTPRequestHandle hRequest, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPResponseBodyData(hRequest, pBodyDataBuffer, unBufferSize);
		}

		public static bool GetHTTPStreamingResponseBodyData(global::Steamworks.HTTPRequestHandle hRequest, uint cOffset, byte[] pBodyDataBuffer, uint unBufferSize)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPStreamingResponseBodyData(hRequest, cOffset, pBodyDataBuffer, unBufferSize);
		}

		public static bool ReleaseHTTPRequest(global::Steamworks.HTTPRequestHandle hRequest)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_ReleaseHTTPRequest(hRequest);
		}

		public static bool GetHTTPDownloadProgressPct(global::Steamworks.HTTPRequestHandle hRequest, out float pflPercentOut)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPDownloadProgressPct(hRequest, out pflPercentOut);
		}

		public static bool SetHTTPRequestRawPostBody(global::Steamworks.HTTPRequestHandle hRequest, string pchContentType, byte[] pubBody, uint unBodyLen)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchContentType))
			{
				result = global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestRawPostBody(hRequest, utf8StringHandle, pubBody, unBodyLen);
			}
			return result;
		}

		public static global::Steamworks.HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return (global::Steamworks.HTTPCookieContainerHandle)global::Steamworks.NativeMethods.ISteamHTTP_CreateCookieContainer(bAllowResponsesToModify);
		}

		public static bool ReleaseCookieContainer(global::Steamworks.HTTPCookieContainerHandle hCookieContainer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_ReleaseCookieContainer(hCookieContainer);
		}

		public static bool SetCookie(global::Steamworks.HTTPCookieContainerHandle hCookieContainer, string pchHost, string pchUrl, string pchCookie)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchHost))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchUrl))
				{
					using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle3 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchCookie))
					{
						result = global::Steamworks.NativeMethods.ISteamHTTP_SetCookie(hCookieContainer, utf8StringHandle, utf8StringHandle2, utf8StringHandle3);
					}
				}
			}
			return result;
		}

		public static bool SetHTTPRequestCookieContainer(global::Steamworks.HTTPRequestHandle hRequest, global::Steamworks.HTTPCookieContainerHandle hCookieContainer)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestCookieContainer(hRequest, hCookieContainer);
		}

		public static bool SetHTTPRequestUserAgentInfo(global::Steamworks.HTTPRequestHandle hRequest, string pchUserAgentInfo)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			bool result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchUserAgentInfo))
			{
				result = global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestUserAgentInfo(hRequest, utf8StringHandle);
			}
			return result;
		}

		public static bool SetHTTPRequestRequiresVerifiedCertificate(global::Steamworks.HTTPRequestHandle hRequest, bool bRequireVerifiedCertificate)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(hRequest, bRequireVerifiedCertificate);
		}

		public static bool SetHTTPRequestAbsoluteTimeoutMS(global::Steamworks.HTTPRequestHandle hRequest, uint unMilliseconds)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(hRequest, unMilliseconds);
		}

		public static bool GetHTTPRequestWasTimedOut(global::Steamworks.HTTPRequestHandle hRequest, out bool pbWasTimedOut)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTTP_GetHTTPRequestWasTimedOut(hRequest, out pbWasTimedOut);
		}
	}
}
