using System;
using UnityEngine;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VersionInfo
	{
		private VersionInfo()
		{
			this.m_major = 1;
			this.m_minor = 5;
			this.m_release = 1;
		}

		private VersionInfo(byte major, byte minor, byte release)
		{
			this.m_major = (int)major;
			this.m_minor = (int)minor;
			this.m_release = (int)release;
		}

		public static string StaticToString()
		{
			return string.Format("{0}.{1}.{2}", 1, 5, 1) + global::AmplifyColor.VersionInfo.StageSuffix + global::AmplifyColor.VersionInfo.TrialSuffix;
		}

		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", this.m_major, this.m_minor, this.m_release) + global::AmplifyColor.VersionInfo.StageSuffix + global::AmplifyColor.VersionInfo.TrialSuffix;
		}

		public int Number
		{
			get
			{
				return this.m_major * 100 + this.m_minor * 10 + this.m_release;
			}
		}

		public static global::AmplifyColor.VersionInfo Current()
		{
			return new global::AmplifyColor.VersionInfo(1, 5, 1);
		}

		public static bool Matches(global::AmplifyColor.VersionInfo version)
		{
			return version.m_major == 1 && version.m_minor == 5 && 1 == version.m_release;
		}

		public const byte Major = 1;

		public const byte Minor = 5;

		public const byte Release = 1;

		private static string StageSuffix = "_dev007";

		private static string TrialSuffix = string.Empty;

		[global::UnityEngine.SerializeField]
		private int m_major;

		[global::UnityEngine.SerializeField]
		private int m_minor;

		[global::UnityEngine.SerializeField]
		private int m_release;
	}
}
