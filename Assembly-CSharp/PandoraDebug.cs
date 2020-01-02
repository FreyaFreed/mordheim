using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PandoraDebug
{
	[global::System.Diagnostics.Conditional("MCOTD_DEBUG")]
	public static void Assert(bool condition, string message, bool fatal)
	{
	}

	public static void LogDebug(object msg, string category = "uncategorised", global::UnityEngine.MonoBehaviour monoObject = null)
	{
		if (!global::Pandora.fullLog)
		{
			return;
		}
		string message;
		if (monoObject)
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: {5}", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"DEBUG",
				category,
				msg,
				monoObject.name
			});
		}
		else
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: NULL", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"DEBUG",
				category,
				msg
			});
		}
		global::UnityEngine.Debug.Log(message);
	}

	public static void LogInfo(object msg, string category = "uncategorised", global::UnityEngine.MonoBehaviour monoObject = null)
	{
		if (!global::Pandora.fullLog)
		{
			return;
		}
		string message;
		if (monoObject)
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: {5}", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"INFO",
				category,
				msg,
				monoObject.name
			});
		}
		else
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: NULL", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"INFO",
				category,
				msg
			});
		}
		global::UnityEngine.Debug.Log(message);
	}

	public static void LogWarning(object msg, string category = "uncategorised", global::UnityEngine.MonoBehaviour monoObject = null)
	{
		string message;
		if (monoObject)
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: {5}", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"WARNING",
				category,
				msg,
				monoObject.name
			});
		}
		else
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: NULL", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"WARNING",
				category,
				msg
			});
		}
		global::UnityEngine.Debug.LogWarning(message);
	}

	public static void LogError(object msg, string category = "uncategorised", global::UnityEngine.MonoBehaviour monoObject = null)
	{
		string message;
		if (monoObject)
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: {5}", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"ERROR",
				category,
				msg,
				monoObject.name
			});
		}
		else
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: NULL", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"ERROR",
				category,
				msg
			});
		}
		global::UnityEngine.Debug.LogError(message);
	}

	public static void LogFatal(object msg, string category = "uncategorised", global::UnityEngine.MonoBehaviour monoObject = null)
	{
		string message;
		if (monoObject)
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: {5}", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"FATAL",
				category,
				msg,
				monoObject.name
			});
		}
		else
		{
			message = string.Format("{0} ({1}) - [{2}] {3} {4} for Object: NULL", new object[]
			{
				global::PandoraDebug.GetTimeString(),
				global::PandoraDebug.GetTyche(),
				"FATAL",
				category,
				msg
			});
		}
		global::UnityEngine.Debug.LogError(message);
	}

	public static void LogException(global::System.Exception e, bool fatal = true)
	{
		string text = string.Empty;
		string text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			global::System.TimeSpan.FromSeconds((double)global::UnityEngine.Time.time),
			": ",
			e
		});
		text = text + "\nInnerException: " + e.InnerException;
		text = text + "\nMessage: " + e.Message;
		text = text + "\nSource: " + e.Source;
		text = text + "\nStackTrace: " + e.StackTrace;
		text = text + "\nTargetSite: " + e.TargetSite;
		global::UnityEngine.Debug.LogException(e);
	}

	public static void LogStrings(string separator, params string[] strings)
	{
		string text = null;
		for (int i = 0; i < strings.Length; i++)
		{
			if (separator != null)
			{
				text += string.Format(strings[i] + "{0}", (i >= strings.Length - 1) ? string.Empty : separator);
			}
			else
			{
				text += string.Format(strings[i] + "{0}", (i >= strings.Length - 1) ? string.Empty : "\n");
			}
		}
		global::PandoraDebug.LogInfo("Length = " + strings.Length + text, "uncategorised", null);
	}

	public static void LogVector(global::UnityEngine.Vector3 vec)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Vector x = ",
			vec.x,
			", y = ",
			vec.y,
			", z = ",
			vec.z
		}), "uncategorised", null);
	}

	public static void LogArray<T>(T[] array)
	{
		global::PandoraDebug.LogArray<T>(null, array);
	}

	public static void LogArray<T>(string separator, T[] array)
	{
		string text = null;
		for (int i = 0; i < array.Length; i++)
		{
			if (separator != null)
			{
				text += string.Format(array[i].ToString() + "{0}", (i >= array.Length - 1) ? string.Empty : separator);
			}
			else
			{
				text += string.Format(array[i].ToString() + "{0}", (i >= array.Length - 1) ? string.Empty : "\n");
			}
		}
		global::PandoraDebug.LogInfo("Length = " + array.Length + text, "uncategorised", null);
	}

	public static void LogArray2D<T>(T[,] array)
	{
		global::PandoraDebug.LogArray2D<T>(null, array);
	}

	public static void LogArray2D<T>(string separator, T[,] array)
	{
		string text = null;
		for (int i = 0; i < array.GetUpperBound(0); i++)
		{
			for (int j = 0; j < array.GetUpperBound(1); j++)
			{
				if (separator != null)
				{
					text += string.Format(array[i, 0].ToString() + ", " + array[i, 1].ToString() + "{0}", (i * j >= array.Length - 1) ? string.Empty : separator);
				}
				else
				{
					text += string.Format(array[i, 0].ToString() + ", " + array[i, 1].ToString() + "{0}", (i * j >= array.Length - 1) ? string.Empty : "\n");
				}
			}
		}
		global::PandoraDebug.LogInfo("Length = " + array.Length + text, "uncategorised", null);
	}

	public static void LogArray3D<T>(T[,,] array)
	{
		global::PandoraDebug.LogArray3D<T>(null, array);
	}

	public static void LogArray3D<T>(string separator, T[,,] array)
	{
		string text = null;
		int num = 1;
		for (int i = 0; i < array.GetLength(2); i++)
		{
			for (int j = 0; j < array.GetLength(1); j++)
			{
				for (int k = 0; k < array.GetLength(0); k++)
				{
					if (separator != null)
					{
						text += string.Format(string.Concat(new object[]
						{
							num.ToString(),
							". [",
							k.ToString(),
							", ",
							j.ToString(),
							", ",
							i.ToString(),
							"] =",
							array[k, j, i],
							"{0}"
						}), (i * j * k >= array.Length - 1) ? string.Empty : separator);
					}
					else
					{
						text += string.Format(string.Concat(new object[]
						{
							num.ToString(),
							". [",
							k.ToString(),
							", ",
							j.ToString(),
							", ",
							i.ToString(),
							"] =",
							array[k, j, i],
							"{0}"
						}), (i * j * k >= array.Length - 1) ? string.Empty : "\n");
					}
					num++;
				}
			}
		}
		global::PandoraDebug.LogInfo("Length = " + array.Length + text, "uncategorised", null);
	}

	public static void LogList<T>(global::System.Collections.Generic.List<T> list)
	{
		global::PandoraDebug.LogList<T>(null, list);
	}

	public static void LogList<T>(string separator, global::System.Collections.Generic.List<T> list)
	{
		string text = null;
		for (int i = 0; i < list.Count; i++)
		{
			if (separator != null)
			{
				string str = text;
				T t = list[i];
				text = str + string.Format(t.ToString() + "{0}", (i >= list.Count - 1) ? string.Empty : separator);
			}
			else
			{
				string str2 = text;
				T t2 = list[i];
				text = str2 + string.Format(t2.ToString() + "{0}", (i >= list.Count - 1) ? string.Empty : "\n");
			}
		}
		global::PandoraDebug.LogInfo("Length = " + list.Count + text, "uncategorised", null);
	}

	public static void LogDictionary<T, K>(global::System.Collections.Generic.Dictionary<T, K> dictionary)
	{
		global::PandoraDebug.LogDictionary<T, K>(null, dictionary);
	}

	public static void LogDictionary<T, K>(string separator, global::System.Collections.Generic.Dictionary<T, K> dictionary)
	{
		string text = null;
		int num = 0;
		foreach (global::System.Collections.Generic.KeyValuePair<T, K> keyValuePair in dictionary)
		{
			if (separator != null)
			{
				string str = text;
				T key = keyValuePair.Key;
				string str2 = key.ToString();
				string str3 = ", ";
				K value = keyValuePair.Value;
				text = str + string.Format(str2 + str3 + value.ToString() + "{0}", (num >= dictionary.Count - 1) ? string.Empty : separator);
			}
			else
			{
				string str4 = text;
				T key2 = keyValuePair.Key;
				string str5 = key2.ToString();
				string str6 = ", ";
				K value2 = keyValuePair.Value;
				text = str4 + string.Format(str5 + str6 + value2.ToString() + "{0}", (num >= dictionary.Count - 1) ? string.Empty : "\n");
			}
			num++;
		}
		global::PandoraDebug.LogInfo("Length = " + dictionary.Count + text, "uncategorised", null);
	}

	private static string GetTimeString()
	{
		global::System.TimeSpan timeSpan = global::System.TimeSpan.FromSeconds((double)global::UnityEngine.Time.time);
		return string.Format("{0:00}:{1:00}:{2:00}:{3:000}", new object[]
		{
			timeSpan.Hours,
			timeSpan.Minutes,
			timeSpan.Seconds,
			timeSpan.Milliseconds
		});
	}

	private static string GetTyche()
	{
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			return global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Count.ToString();
		}
		return "0";
	}

	private const string msgWithObject = "{0} ({1}) - [{2}] {3} {4} for Object: {5}";

	private const string msgWithoutObject = "{0} ({1}) - [{2}] {3} {4} for Object: NULL";

	private const string defaultSeparator = "\n";
}
