using System;

namespace Steamworks
{
	public static class SteamHTMLSurface
	{
		public static bool Init()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTMLSurface_Init();
		}

		public static bool Shutdown()
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			return global::Steamworks.NativeMethods.ISteamHTMLSurface_Shutdown();
		}

		public static global::Steamworks.SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.SteamAPICall_t result;
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchUserAgent))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchUserCSS))
				{
					result = (global::Steamworks.SteamAPICall_t)global::Steamworks.NativeMethods.ISteamHTMLSurface_CreateBrowser(utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		public static void RemoveBrowser(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_RemoveBrowser(unBrowserHandle);
		}

		public static void LoadURL(global::Steamworks.HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchURL))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPostData))
				{
					global::Steamworks.NativeMethods.ISteamHTMLSurface_LoadURL(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		public static void SetSize(global::Steamworks.HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_SetSize(unBrowserHandle, unWidth, unHeight);
		}

		public static void StopLoad(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_StopLoad(unBrowserHandle);
		}

		public static void Reload(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_Reload(unBrowserHandle);
		}

		public static void GoBack(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_GoBack(unBrowserHandle);
		}

		public static void GoForward(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_GoForward(unBrowserHandle);
		}

		public static void AddHeader(global::Steamworks.HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValue))
				{
					global::Steamworks.NativeMethods.ISteamHTMLSurface_AddHeader(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		public static void ExecuteJavascript(global::Steamworks.HHTMLBrowser unBrowserHandle, string pchScript)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchScript))
			{
				global::Steamworks.NativeMethods.ISteamHTMLSurface_ExecuteJavascript(unBrowserHandle, utf8StringHandle);
			}
		}

		public static void MouseUp(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.EHTMLMouseButton eMouseButton)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_MouseUp(unBrowserHandle, eMouseButton);
		}

		public static void MouseDown(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.EHTMLMouseButton eMouseButton)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_MouseDown(unBrowserHandle, eMouseButton);
		}

		public static void MouseDoubleClick(global::Steamworks.HHTMLBrowser unBrowserHandle, global::Steamworks.EHTMLMouseButton eMouseButton)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_MouseDoubleClick(unBrowserHandle, eMouseButton);
		}

		public static void MouseMove(global::Steamworks.HHTMLBrowser unBrowserHandle, int x, int y)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_MouseMove(unBrowserHandle, x, y);
		}

		public static void MouseWheel(global::Steamworks.HHTMLBrowser unBrowserHandle, int nDelta)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_MouseWheel(unBrowserHandle, nDelta);
		}

		public static void KeyDown(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, global::Steamworks.EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_KeyDown(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		public static void KeyUp(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, global::Steamworks.EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_KeyUp(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		public static void KeyChar(global::Steamworks.HHTMLBrowser unBrowserHandle, uint cUnicodeChar, global::Steamworks.EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_KeyChar(unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
		}

		public static void SetHorizontalScroll(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		public static void SetVerticalScroll(global::Steamworks.HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_SetVerticalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		public static void SetKeyFocus(global::Steamworks.HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_SetKeyFocus(unBrowserHandle, bHasKeyFocus);
		}

		public static void ViewSource(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_ViewSource(unBrowserHandle);
		}

		public static void CopyToClipboard(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_CopyToClipboard(unBrowserHandle);
		}

		public static void PasteFromClipboard(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_PasteFromClipboard(unBrowserHandle);
		}

		public static void Find(global::Steamworks.HHTMLBrowser unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchSearchStr))
			{
				global::Steamworks.NativeMethods.ISteamHTMLSurface_Find(unBrowserHandle, utf8StringHandle, bCurrentlyInFind, bReverse);
			}
		}

		public static void StopFind(global::Steamworks.HHTMLBrowser unBrowserHandle)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_StopFind(unBrowserHandle);
		}

		public static void GetLinkAtPosition(global::Steamworks.HHTMLBrowser unBrowserHandle, int x, int y)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(unBrowserHandle, x, y);
		}

		public static void SetCookie(string pchHostname, string pchKey, string pchValue, string pchPath = "/", uint nExpires = 0U, bool bSecure = false, bool bHTTPOnly = false)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle = new global::Steamworks.InteropHelp.UTF8StringHandle(pchHostname))
			{
				using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle2 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchKey))
				{
					using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle3 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchValue))
					{
						using (global::Steamworks.InteropHelp.UTF8StringHandle utf8StringHandle4 = new global::Steamworks.InteropHelp.UTF8StringHandle(pchPath))
						{
							global::Steamworks.NativeMethods.ISteamHTMLSurface_SetCookie(utf8StringHandle, utf8StringHandle2, utf8StringHandle3, utf8StringHandle4, nExpires, bSecure, bHTTPOnly);
						}
					}
				}
			}
		}

		public static void SetPageScaleFactor(global::Steamworks.HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(unBrowserHandle, flZoom, nPointX, nPointY);
		}

		public static void SetBackgroundMode(global::Steamworks.HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_SetBackgroundMode(unBrowserHandle, bBackgroundMode);
		}

		public static void AllowStartRequest(global::Steamworks.HHTMLBrowser unBrowserHandle, bool bAllowed)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_AllowStartRequest(unBrowserHandle, bAllowed);
		}

		public static void JSDialogResponse(global::Steamworks.HHTMLBrowser unBrowserHandle, bool bResult)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_JSDialogResponse(unBrowserHandle, bResult);
		}

		public static void FileLoadDialogResponse(global::Steamworks.HHTMLBrowser unBrowserHandle, global::System.IntPtr pchSelectedFiles)
		{
			global::Steamworks.InteropHelp.TestIfAvailableClient();
			global::Steamworks.NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(unBrowserHandle, pchSelectedFiles);
		}
	}
}
