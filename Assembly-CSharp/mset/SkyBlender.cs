using System;
using UnityEngine;

namespace mset
{
	[global::System.Serializable]
	public class SkyBlender
	{
		public float BlendTime
		{
			get
			{
				return this.blendTime;
			}
			set
			{
				this.blendTime = value;
			}
		}

		private float blendTimer
		{
			get
			{
				return this.endStamp - global::UnityEngine.Time.time;
			}
			set
			{
				this.endStamp = global::UnityEngine.Time.time + value;
			}
		}

		public float BlendWeight
		{
			get
			{
				return 1f - global::UnityEngine.Mathf.Clamp01(this.blendTimer / this.currentBlendTime);
			}
		}

		public bool IsBlending
		{
			get
			{
				return global::UnityEngine.Time.time < this.endStamp;
			}
		}

		public bool WasBlending(float secAgo)
		{
			return global::UnityEngine.Time.time - secAgo < this.endStamp;
		}

		public void Apply()
		{
			if (this.IsBlending)
			{
				global::mset.Sky.EnableGlobalProjection(this.CurrentSky.HasDimensions || this.PreviousSky.HasDimensions);
				global::mset.Sky.EnableGlobalBlending(true);
				this.CurrentSky.Apply(0);
				this.PreviousSky.Apply(1);
				global::mset.Sky.SetBlendWeight(this.BlendWeight);
			}
			else
			{
				global::mset.Sky.EnableGlobalProjection(this.CurrentSky.HasDimensions);
				global::mset.Sky.EnableGlobalBlending(false);
				this.CurrentSky.Apply(0);
			}
		}

		public void Apply(global::UnityEngine.Material target)
		{
			if (this.IsBlending)
			{
				global::mset.Sky.EnableBlending(target, true);
				global::mset.Sky.EnableProjection(target, this.CurrentSky.HasDimensions || this.PreviousSky.HasDimensions);
				this.CurrentSky.Apply(target, 0);
				this.PreviousSky.Apply(target, 1);
				global::mset.Sky.SetBlendWeight(target, this.BlendWeight);
			}
			else
			{
				global::mset.Sky.EnableBlending(target, false);
				global::mset.Sky.EnableProjection(target, this.CurrentSky.HasDimensions);
				this.CurrentSky.Apply(target, 0);
			}
		}

		public void Apply(global::UnityEngine.Renderer target, global::UnityEngine.Material[] materials)
		{
			if (this.IsBlending)
			{
				global::mset.Sky.EnableBlending(target, materials, true);
				global::mset.Sky.EnableProjection(target, materials, this.CurrentSky.HasDimensions || this.PreviousSky.HasDimensions);
				this.CurrentSky.ApplyFast(target, 0);
				this.PreviousSky.ApplyFast(target, 1);
				global::mset.Sky.SetBlendWeight(target, this.BlendWeight);
			}
			else
			{
				global::mset.Sky.EnableBlending(target, materials, false);
				global::mset.Sky.EnableProjection(target, materials, this.CurrentSky.HasDimensions);
				this.CurrentSky.ApplyFast(target, 0);
			}
		}

		public void ApplyToTerrain()
		{
			if (this.IsBlending)
			{
				global::mset.Sky.EnableTerrainBlending(true);
			}
			else
			{
				global::mset.Sky.EnableTerrainBlending(false);
			}
		}

		public void SnapToSky(global::mset.Sky nusky)
		{
			if (nusky == null)
			{
				return;
			}
			this.PreviousSky = nusky;
			this.CurrentSky = nusky;
			this.blendTimer = 0f;
		}

		public void BlendToSky(global::mset.Sky nusky)
		{
			if (nusky == null)
			{
				return;
			}
			if (this.CurrentSky != nusky)
			{
				if (this.CurrentSky == null)
				{
					this.CurrentSky = nusky;
					this.PreviousSky = nusky;
					this.blendTimer = 0f;
				}
				else
				{
					this.PreviousSky = this.CurrentSky;
					this.CurrentSky = nusky;
					this.currentBlendTime = this.blendTime;
					this.blendTimer = this.currentBlendTime;
				}
			}
		}

		public void SkipTime(float sec)
		{
			this.blendTimer -= sec;
		}

		public global::mset.Sky CurrentSky;

		public global::mset.Sky PreviousSky;

		[global::UnityEngine.SerializeField]
		private float blendTime = 0.25f;

		private float currentBlendTime = 0.25f;

		private float endStamp;
	}
}
