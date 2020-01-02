using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class KGFUtility
{
	public static T[] GetComponentsInterface<T>(this global::UnityEngine.MonoBehaviour theMonobehaviour) where T : class
	{
		global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
		foreach (global::UnityEngine.MonoBehaviour monoBehaviour in theMonobehaviour.GetComponents<global::UnityEngine.MonoBehaviour>())
		{
			T t = monoBehaviour as T;
			if (t != null)
			{
				list.Add(t);
			}
		}
		return list.ToArray();
	}

	public static T GetComponentInterface<T>(this global::UnityEngine.MonoBehaviour theMonobehaviour) where T : class
	{
		T[] componentsInterface = theMonobehaviour.GetComponentsInterface<T>();
		if (componentsInterface.Length > 0)
		{
			return componentsInterface[0];
		}
		return (T)((object)null);
	}

	public static global::System.Collections.Generic.List<T> Sorted<T>(this global::System.Collections.Generic.List<T> theList)
	{
		global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>(theList);
		list.Sort();
		return list;
	}

	public static bool ContainsItem<T>(this global::System.Collections.Generic.IEnumerable<T> theList, T theNeedle) where T : class
	{
		foreach (T t in theList)
		{
			if (theNeedle.Equals(t))
			{
				return true;
			}
		}
		return false;
	}

	public static string JoinToString<T>(this global::System.Collections.Generic.IEnumerable<T> theList, string theSeparator)
	{
		if (theList == null)
		{
			return string.Empty;
		}
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		foreach (T t in theList)
		{
			list.Add(t.ToString());
		}
		return string.Join(theSeparator, list.ToArray());
	}

	public static global::System.Collections.Generic.IEnumerable<T> InsertItem<T>(this global::System.Collections.Generic.IEnumerable<T> theList, T theItem, int thePosition)
	{
		int i = 0;
		bool anInserted = false;
		foreach (T anElement in theList)
		{
			if (i == thePosition)
			{
				yield return theItem;
				anInserted = true;
			}
			yield return anElement;
			i++;
		}
		if (!anInserted)
		{
			yield return theItem;
		}
		yield break;
	}

	public static global::System.Collections.Generic.IEnumerable<T> AppendItem<T>(this global::System.Collections.Generic.IEnumerable<T> theList, T theItem)
	{
		foreach (T anElement in theList)
		{
			yield return anElement;
		}
		yield return theItem;
		yield break;
	}

	public static global::System.Collections.Generic.IEnumerable<T> Distinct<T>(this global::System.Collections.Generic.IEnumerable<T> theList)
	{
		global::System.Collections.Generic.List<T> aDistinctList = new global::System.Collections.Generic.List<T>();
		foreach (T anElement in theList)
		{
			if (!aDistinctList.Contains(anElement))
			{
				aDistinctList.Add(anElement);
				yield return anElement;
			}
		}
		yield break;
	}

	public static global::System.Collections.Generic.IEnumerable<T> Remove<T>(this global::System.Collections.Generic.IEnumerable<T> theMainList, T[] theListToRemove)
	{
		global::System.Collections.Generic.List<T> aListToRemove = new global::System.Collections.Generic.List<T>(theListToRemove);
		foreach (T anElement in theMainList)
		{
			if (!aListToRemove.Contains(anElement))
			{
				yield return anElement;
			}
		}
		yield break;
	}

	public static global::System.Collections.Generic.IEnumerable<T> Sorted<T>(this global::System.Collections.Generic.IEnumerable<T> theList)
	{
		global::System.Collections.Generic.List<T> aList = new global::System.Collections.Generic.List<T>(theList);
		aList.Sort();
		foreach (T aT in aList)
		{
			yield return aT;
		}
		yield break;
	}

	public static global::System.Collections.Generic.IEnumerable<T> Sorted<T>(this global::System.Collections.Generic.IEnumerable<T> theList, global::System.Comparison<T> theComparison)
	{
		global::System.Collections.Generic.List<T> aList = new global::System.Collections.Generic.List<T>(theList);
		aList.Sort(theComparison);
		foreach (T aT in aList)
		{
			yield return aT;
		}
		yield break;
	}

	public static global::System.Collections.Generic.List<T> ToDynList<T>(this global::System.Collections.Generic.IEnumerable<T> theList)
	{
		return new global::System.Collections.Generic.List<T>(theList);
	}

	public static void SetScaleRecursively(this global::UnityEngine.Transform theTransform, global::UnityEngine.Vector3 theScale)
	{
		foreach (object obj in theTransform)
		{
			global::UnityEngine.Transform theTransform2 = (global::UnityEngine.Transform)obj;
			theTransform2.SetScaleRecursively(theScale);
		}
		theTransform.localScale = theScale;
	}

	public static void SetChildrenActiveRecursively(this global::UnityEngine.GameObject theGameObject, bool theActive)
	{
		foreach (object obj in theGameObject.transform)
		{
			global::UnityEngine.Transform transform = (global::UnityEngine.Transform)obj;
			transform.gameObject.SetActiveRecursively(theActive);
		}
	}

	public static void SetLayerRecursively(this global::UnityEngine.GameObject theGameObject, int theLayer)
	{
		theGameObject.layer = theLayer;
		foreach (object obj in theGameObject.transform)
		{
			global::UnityEngine.Transform transform = (global::UnityEngine.Transform)obj;
			global::UnityEngine.GameObject gameObject = transform.gameObject;
			gameObject.SetLayerRecursively(theLayer);
		}
	}

	public static long DateToUnix(this global::System.DateTime theDate)
	{
		return (long)(global::System.DateTime.UtcNow - new global::System.DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static string Shortened(this string theString, int theMaxLength)
	{
		if (theString.Length > theMaxLength)
		{
			return theString.Substring(0, theMaxLength - 2) + "..";
		}
		return theString;
	}

	public static string Join(this string theSeparator, params string[] theItems)
	{
		return string.Join(theSeparator, theItems);
	}

	public static string Join(this string theSeparator, global::System.Collections.Generic.IEnumerable<string> theItems)
	{
		return string.Join(theSeparator, new global::System.Collections.Generic.List<string>(theItems).ToArray());
	}

	public static string RemoveRight(this string theString, char theSeparator)
	{
		string text = string.Empty + theString;
		while (text.Length > 0 && text[text.Length - 1] != theSeparator)
		{
			text = text.Remove(text.Length - 1);
		}
		return text;
	}

	public static string GetLastPart(this string theString, char theSeparator)
	{
		string[] array = theString.Split(new char[]
		{
			theSeparator
		});
		return array[array.Length - 1];
	}

	public static string ConvertPathToUnity(string thePlatformPath)
	{
		return thePlatformPath.Replace(global::System.IO.Path.DirectorySeparatorChar, '/');
	}

	public static string ConvertPathToPlatformSpecific(string theUnityPath)
	{
		return theUnityPath.Replace('/', global::System.IO.Path.DirectorySeparatorChar);
	}

	[global::System.Runtime.InteropServices.DllImport("user32.dll")]
	[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool GetCursorPos(out global::KGFUtility.Point _point);

	[global::System.Runtime.InteropServices.DllImport("user32.dll")]
	[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool SetCursorPos(int _x, int _y);

	[global::System.Runtime.InteropServices.DllImport("user32.dll", CharSet = global::System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
	[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool ClipCursor(ref global::KGFUtility.RECT rcClip);

	[global::System.Runtime.InteropServices.DllImport("user32.dll")]
	[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool GetClipCursor(out global::KGFUtility.RECT rcClip);

	[global::System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern int GetForegroundWindow();

	[global::System.Runtime.InteropServices.DllImport("user32.dll")]
	[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.Bool)]
	private static extern bool GetWindowRect(int hWnd, ref global::KGFUtility.RECT lpRect);

	public static void SetMouseRect(global::UnityEngine.Rect theRect)
	{
		global::KGFUtility.RECT rect = new global::KGFUtility.RECT((int)theRect.x, (int)theRect.y, (int)theRect.xMax, (int)theRect.yMax);
		if (global::KGFUtility.itsMouseRectActive)
		{
			global::KGFUtility.ClearMouseRect();
		}
		global::KGFUtility.itsOriginalClippingRect = default(global::KGFUtility.RECT);
		global::KGFUtility.GetClipCursor(out global::KGFUtility.itsOriginalClippingRect);
		global::KGFUtility.ClipCursor(ref rect);
		global::KGFUtility.itsMouseRectActive = true;
	}

	public static void ClearMouseRect()
	{
		if (global::KGFUtility.itsMouseRectActive)
		{
			global::KGFUtility.ClipCursor(ref global::KGFUtility.itsOriginalClippingRect);
			global::KGFUtility.itsMouseRectActive = false;
		}
	}

	public static global::UnityEngine.Rect GetWindowRect()
	{
		int foregroundWindow = global::KGFUtility.GetForegroundWindow();
		global::KGFUtility.RECT rect = default(global::KGFUtility.RECT);
		global::KGFUtility.GetWindowRect(foregroundWindow, ref rect);
		return new global::UnityEngine.Rect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
	}

	public static float PingPong(float theTime, float theMaxValue, float thePingStayTime, float thePongStayTime, float theTransitionTime)
	{
		float num = thePingStayTime + thePongStayTime + 2f * theTransitionTime;
		float num2 = theTime % num;
		if (num2 < thePingStayTime)
		{
			return 0f;
		}
		if (num2 < thePingStayTime + theTransitionTime)
		{
			return (num2 - thePingStayTime) * theMaxValue / theTransitionTime;
		}
		if (num2 < thePingStayTime + theTransitionTime + thePongStayTime)
		{
			return theMaxValue;
		}
		return theMaxValue - (num2 - (thePingStayTime + theTransitionTime + thePongStayTime)) * theMaxValue / theTransitionTime;
	}

	private static global::UnityEngine.Color32[] BlockBlur1D(global::UnityEngine.Color32[] thePixels, int theWidth, int theHeight, int theBlurRadius)
	{
		global::UnityEngine.Color32[] array = new global::UnityEngine.Color32[thePixels.Length];
		for (int i = 0; i < theHeight; i++)
		{
			for (int j = 0; j < theWidth; j++)
			{
				int num3;
				int num2;
				int num = num2 = (num3 = 0);
				int num4 = 0;
				for (int k = j - theBlurRadius; k <= j + theBlurRadius; k++)
				{
					global::UnityEngine.Color32 color = thePixels[global::UnityEngine.Mathf.Clamp(k, 0, theWidth - 1) + i * theWidth];
					num2 += (int)color.r;
					num += (int)color.g;
					num3 += (int)color.b;
					num4++;
				}
				global::UnityEngine.Color32 color2 = thePixels[j + i * theWidth];
				color2.r = (byte)(num2 / num4);
				color2.g = (byte)(num / num4);
				color2.b = (byte)(num3 / num4);
				array[j + i * theWidth] = color2;
			}
		}
		return array;
	}

	private static global::UnityEngine.Color32[] BlockBlur2D(global::UnityEngine.Color32[] thePixels, int theWidth, int theHeight, int theBlurRadiusX, int theBlurRadiusY)
	{
		global::UnityEngine.Color32[] array = new global::UnityEngine.Color32[thePixels.Length];
		for (int i = 0; i < theHeight; i++)
		{
			for (int j = 0; j < theWidth; j++)
			{
				int num3;
				int num2;
				int num = num2 = (num3 = 0);
				int num4 = (j - theBlurRadiusX < 0) ? 0 : (j - theBlurRadiusX);
				int num5 = (i - theBlurRadiusY < 0) ? 0 : (i - theBlurRadiusY);
				int num6 = 0;
				int num7 = num5;
				while (num7 < theHeight && num7 <= i + theBlurRadiusY)
				{
					int num8 = num4;
					while (num8 < theWidth && num8 <= j + theBlurRadiusX)
					{
						global::UnityEngine.Color32 color = thePixels[num8 + num7 * theWidth];
						num2 += (int)color.r;
						num += (int)color.g;
						num3 += (int)color.b;
						num6++;
						num8++;
					}
					num7++;
				}
				global::UnityEngine.Color32 color2 = thePixels[j + i * theWidth];
				color2.r = (byte)(num2 / num6);
				color2.g = (byte)(num / num6);
				color2.b = (byte)(num3 / num6);
				array[j + i * theWidth] = color2;
			}
		}
		return array;
	}

	public static global::UnityEngine.Rect GetCachedRect(float theX, float theY, float theWidth, float theHeight)
	{
		global::KGFUtility.itsCachedRect.x = theX;
		global::KGFUtility.itsCachedRect.y = theY;
		global::KGFUtility.itsCachedRect.width = theWidth;
		global::KGFUtility.itsCachedRect.height = theHeight;
		return global::KGFUtility.itsCachedRect;
	}

	public static global::UnityEngine.Rect GetCachedRect(global::UnityEngine.Rect theRect)
	{
		global::KGFUtility.itsCachedRect.x = theRect.x;
		global::KGFUtility.itsCachedRect.y = theRect.y;
		global::KGFUtility.itsCachedRect.width = theRect.width;
		global::KGFUtility.itsCachedRect.height = theRect.height;
		return global::KGFUtility.itsCachedRect;
	}

	public static global::UnityEngine.Vector3 GetCachedVector3(float theX, float theY, float theZ)
	{
		global::KGFUtility.itsCachedVector3.x = theX;
		global::KGFUtility.itsCachedVector3.y = theY;
		global::KGFUtility.itsCachedVector3.z = theZ;
		return global::KGFUtility.itsCachedVector3;
	}

	public static global::UnityEngine.Vector2 GetCachedVector2(float theX, float theY)
	{
		global::KGFUtility.itsCachedVector2.x = theX;
		global::KGFUtility.itsCachedVector2.y = theY;
		return global::KGFUtility.itsCachedVector2;
	}

	public static global::System.DateTime DateFromUnix(long theSeconds)
	{
		global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1);
		return dateTime.AddSeconds((double)theSeconds);
	}

	public static string ToHexString(byte[] buffer)
	{
		string text = string.Empty;
		foreach (byte b in buffer)
		{
			text += string.Format("{0:x02}", b);
		}
		return text;
	}

	public static global::UnityEngine.Texture2D GetBestAspectMatchingTexture(float theAspectRatio, params global::UnityEngine.Texture2D[] theTextures)
	{
		global::UnityEngine.Texture2D texture2D = null;
		if (theTextures.Length > 0)
		{
			texture2D = theTextures[0];
			for (int i = 1; i < theTextures.Length; i++)
			{
				global::UnityEngine.Texture2D texture2D2 = theTextures[i];
				if (!(texture2D2 == null))
				{
					float num = global::UnityEngine.Mathf.Abs(theAspectRatio - (float)texture2D.width / (float)texture2D.height);
					float num2 = global::UnityEngine.Mathf.Abs(theAspectRatio - (float)texture2D2.width / (float)texture2D2.height);
					if (num2 < num)
					{
						texture2D = texture2D2;
					}
				}
			}
		}
		return texture2D;
	}

	public static global::UnityEngine.Quaternion SetLookRotationSafe(global::UnityEngine.Quaternion theQuaternion, global::UnityEngine.Vector3 theUpVector, global::UnityEngine.Vector3 theLookRotation, global::UnityEngine.Vector3 theAlternativeLookDirection)
	{
		if (theAlternativeLookDirection.magnitude == 0f)
		{
			throw new global::System.Exception("Alternative look vector can never be 0!");
		}
		if (theLookRotation.magnitude != 0f)
		{
			theQuaternion.SetLookRotation(theLookRotation, theUpVector);
			return theQuaternion;
		}
		theQuaternion.SetLookRotation(theAlternativeLookDirection, theUpVector);
		return theQuaternion;
	}

	private static bool itsMouseRectActive = false;

	private static global::KGFUtility.RECT itsOriginalClippingRect;

	private static global::UnityEngine.Rect itsCachedRect = default(global::UnityEngine.Rect);

	private static global::UnityEngine.Vector3 itsCachedVector3 = default(global::UnityEngine.Vector3);

	private static global::UnityEngine.Vector2 itsCachedVector2 = default(global::UnityEngine.Vector2);

	public struct Point
	{
		public int X;

		public int Y;
	}

	public struct RECT
	{
		public RECT(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}
}
