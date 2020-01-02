using System;
using UnityEngine;

namespace FxProNS
{
	[global::System.Serializable]
	public class BloomHelperParams
	{
		public global::FxProNS.EffectsQuality Quality;

		public global::UnityEngine.Color BloomTint = global::UnityEngine.Color.white;

		[global::UnityEngine.Range(0f, 0.99f)]
		public float BloomThreshold = 0.8f;

		[global::UnityEngine.Range(0f, 3f)]
		public float BloomIntensity = 1.5f;

		[global::UnityEngine.Range(0.01f, 3f)]
		public float BloomSoftness = 0.5f;
	}
}
