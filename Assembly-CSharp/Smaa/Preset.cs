using System;
using UnityEngine;

namespace Smaa
{
	[global::System.Serializable]
	public class Preset
	{
		public bool DiagDetection = true;

		public bool CornerDetection = true;

		[global::UnityEngine.Range(0f, 0.5f)]
		public float Threshold = 0.1f;

		[global::Smaa.Min(0.0001f)]
		public float DepthThreshold = 0.01f;

		[global::UnityEngine.Range(0f, 112f)]
		public int MaxSearchSteps = 16;

		[global::UnityEngine.Range(0f, 20f)]
		public int MaxSearchStepsDiag = 8;

		[global::UnityEngine.Range(0f, 100f)]
		public int CornerRounding = 25;

		[global::Smaa.Min(0f)]
		public float LocalContrastAdaptationFactor = 2f;
	}
}
