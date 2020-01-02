using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
	public class AstarProfiler
	{
		private AstarProfiler()
		{
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void InitializeFastProfile(string[] profileNames)
		{
			global::Pathfinding.AstarProfiler.fastProfileNames = new string[profileNames.Length + 2];
			global::System.Array.Copy(profileNames, global::Pathfinding.AstarProfiler.fastProfileNames, profileNames.Length);
			global::Pathfinding.AstarProfiler.fastProfileNames[global::Pathfinding.AstarProfiler.fastProfileNames.Length - 2] = "__Control1__";
			global::Pathfinding.AstarProfiler.fastProfileNames[global::Pathfinding.AstarProfiler.fastProfileNames.Length - 1] = "__Control2__";
			global::Pathfinding.AstarProfiler.fastProfiles = new global::Pathfinding.AstarProfiler.ProfilePoint[global::Pathfinding.AstarProfiler.fastProfileNames.Length];
			for (int i = 0; i < global::Pathfinding.AstarProfiler.fastProfiles.Length; i++)
			{
				global::Pathfinding.AstarProfiler.fastProfiles[i] = new global::Pathfinding.AstarProfiler.ProfilePoint();
			}
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void StartFastProfile(int tag)
		{
			global::Pathfinding.AstarProfiler.fastProfiles[tag].watch.Start();
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void EndFastProfile(int tag)
		{
			global::Pathfinding.AstarProfiler.ProfilePoint profilePoint = global::Pathfinding.AstarProfiler.fastProfiles[tag];
			profilePoint.totalCalls++;
			profilePoint.watch.Stop();
		}

		[global::System.Diagnostics.Conditional("ASTAR_UNITY_PRO_PROFILER")]
		public static void EndProfile()
		{
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void StartProfile(string tag)
		{
			global::Pathfinding.AstarProfiler.ProfilePoint profilePoint;
			global::Pathfinding.AstarProfiler.profiles.TryGetValue(tag, out profilePoint);
			if (profilePoint == null)
			{
				profilePoint = new global::Pathfinding.AstarProfiler.ProfilePoint();
				global::Pathfinding.AstarProfiler.profiles[tag] = profilePoint;
			}
			profilePoint.tmpBytes = global::System.GC.GetTotalMemory(false);
			profilePoint.watch.Start();
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void EndProfile(string tag)
		{
			if (!global::Pathfinding.AstarProfiler.profiles.ContainsKey(tag))
			{
				global::UnityEngine.Debug.LogError("Can only end profiling for a tag which has already been started (tag was " + tag + ")");
				return;
			}
			global::Pathfinding.AstarProfiler.ProfilePoint profilePoint = global::Pathfinding.AstarProfiler.profiles[tag];
			profilePoint.totalCalls++;
			profilePoint.watch.Stop();
			profilePoint.totalBytes += global::System.GC.GetTotalMemory(false) - profilePoint.tmpBytes;
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void Reset()
		{
			global::Pathfinding.AstarProfiler.profiles.Clear();
			global::Pathfinding.AstarProfiler.startTime = global::System.DateTime.UtcNow;
			if (global::Pathfinding.AstarProfiler.fastProfiles != null)
			{
				for (int i = 0; i < global::Pathfinding.AstarProfiler.fastProfiles.Length; i++)
				{
					global::Pathfinding.AstarProfiler.fastProfiles[i] = new global::Pathfinding.AstarProfiler.ProfilePoint();
				}
			}
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void PrintFastResults()
		{
			if (global::Pathfinding.AstarProfiler.fastProfiles == null)
			{
				return;
			}
			for (int i = 0; i < 1000; i++)
			{
			}
			double num = global::Pathfinding.AstarProfiler.fastProfiles[global::Pathfinding.AstarProfiler.fastProfiles.Length - 2].watch.Elapsed.TotalMilliseconds / 1000.0;
			global::System.TimeSpan timeSpan = global::System.DateTime.UtcNow - global::Pathfinding.AstarProfiler.startTime;
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
			stringBuilder.Append("Name\t\t|\tTotal Time\t|\tTotal Calls\t|\tAvg/Call\t|\tBytes");
			for (int j = 0; j < global::Pathfinding.AstarProfiler.fastProfiles.Length; j++)
			{
				string text = global::Pathfinding.AstarProfiler.fastProfileNames[j];
				global::Pathfinding.AstarProfiler.ProfilePoint profilePoint = global::Pathfinding.AstarProfiler.fastProfiles[j];
				int totalCalls = profilePoint.totalCalls;
				double num2 = profilePoint.watch.Elapsed.TotalMilliseconds - num * (double)totalCalls;
				if (totalCalls >= 1)
				{
					stringBuilder.Append("\n").Append(text.PadLeft(10)).Append("|   ");
					stringBuilder.Append(num2.ToString("0.0 ").PadLeft(10)).Append(profilePoint.watch.Elapsed.TotalMilliseconds.ToString("(0.0)").PadLeft(10)).Append("|   ");
					stringBuilder.Append(totalCalls.ToString().PadLeft(10)).Append("|   ");
					stringBuilder.Append((num2 / (double)totalCalls).ToString("0.000").PadLeft(10));
				}
			}
			stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
			stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
			stringBuilder.Append(" seconds\n============================");
			global::UnityEngine.Debug.Log(stringBuilder.ToString());
		}

		[global::System.Diagnostics.Conditional("ProfileAstar")]
		public static void PrintResults()
		{
			global::System.TimeSpan timeSpan = global::System.DateTime.UtcNow - global::Pathfinding.AstarProfiler.startTime;
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			stringBuilder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
			int num = 5;
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Pathfinding.AstarProfiler.ProfilePoint> keyValuePair in global::Pathfinding.AstarProfiler.profiles)
			{
				num = global::System.Math.Max(keyValuePair.Key.Length, num);
			}
			stringBuilder.Append(" Name ".PadRight(num)).Append("|").Append(" Total Time\t".PadRight(20)).Append("|").Append(" Total Calls ".PadRight(20)).Append("|").Append(" Avg/Call ".PadRight(20));
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Pathfinding.AstarProfiler.ProfilePoint> keyValuePair2 in global::Pathfinding.AstarProfiler.profiles)
			{
				double totalMilliseconds = keyValuePair2.Value.watch.Elapsed.TotalMilliseconds;
				int totalCalls = keyValuePair2.Value.totalCalls;
				if (totalCalls >= 1)
				{
					string key = keyValuePair2.Key;
					stringBuilder.Append("\n").Append(key.PadRight(num)).Append("| ");
					stringBuilder.Append(totalMilliseconds.ToString("0.0").PadRight(20)).Append("| ");
					stringBuilder.Append(totalCalls.ToString().PadRight(20)).Append("| ");
					stringBuilder.Append((totalMilliseconds / (double)totalCalls).ToString("0.000").PadRight(20));
					stringBuilder.Append(global::Pathfinding.AstarMath.FormatBytesBinary((int)keyValuePair2.Value.totalBytes).PadLeft(10));
				}
			}
			stringBuilder.Append("\n\n============================\n\t\tTotal runtime: ");
			stringBuilder.Append(timeSpan.TotalSeconds.ToString("F3"));
			stringBuilder.Append(" seconds\n============================");
			global::UnityEngine.Debug.Log(stringBuilder.ToString());
		}

		private static readonly global::System.Collections.Generic.Dictionary<string, global::Pathfinding.AstarProfiler.ProfilePoint> profiles = new global::System.Collections.Generic.Dictionary<string, global::Pathfinding.AstarProfiler.ProfilePoint>();

		private static global::System.DateTime startTime = global::System.DateTime.UtcNow;

		public static global::Pathfinding.AstarProfiler.ProfilePoint[] fastProfiles;

		public static string[] fastProfileNames;

		public class ProfilePoint
		{
			public global::System.Diagnostics.Stopwatch watch = new global::System.Diagnostics.Stopwatch();

			public int totalCalls;

			public long tmpBytes;

			public long totalBytes;
		}
	}
}
