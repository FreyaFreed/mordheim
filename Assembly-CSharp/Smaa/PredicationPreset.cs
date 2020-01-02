using System;
using UnityEngine;

namespace Smaa
{
	[global::System.Serializable]
	public class PredicationPreset
	{
		[global::Smaa.Min(0.0001f)]
		public float Threshold = 0.01f;

		[global::UnityEngine.Range(1f, 5f)]
		public float Scale = 2f;

		[global::UnityEngine.Range(0f, 1f)]
		public float Strength = 0.4f;
	}
}
