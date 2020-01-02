using System;
using UnityEngine;

namespace mset
{
	public class ShaderIDs
	{
		public ShaderIDs()
		{
			this.SH = new int[9];
		}

		public bool valid
		{
			get
			{
				return this._valid;
			}
		}

		public void Link()
		{
			this.Link(string.Empty);
		}

		public void Link(string postfix)
		{
			this.specCubeIBL = global::UnityEngine.Shader.PropertyToID("_SpecCubeIBL" + postfix);
			this.skyCubeIBL = global::UnityEngine.Shader.PropertyToID("_SkyCubeIBL" + postfix);
			this.skyMatrix = global::UnityEngine.Shader.PropertyToID("_SkyMatrix" + postfix);
			this.invSkyMatrix = global::UnityEngine.Shader.PropertyToID("_InvSkyMatrix" + postfix);
			this.skyMin = global::UnityEngine.Shader.PropertyToID("_SkyMin" + postfix);
			this.skyMax = global::UnityEngine.Shader.PropertyToID("_SkyMax" + postfix);
			this.exposureIBL = global::UnityEngine.Shader.PropertyToID("_ExposureIBL" + postfix);
			this.exposureLM = global::UnityEngine.Shader.PropertyToID("_ExposureLM" + postfix);
			for (int i = 0; i < 9; i++)
			{
				this.SH[i] = global::UnityEngine.Shader.PropertyToID("_SH" + i + postfix);
			}
			this.blendWeightIBL = global::UnityEngine.Shader.PropertyToID("_BlendWeightIBL");
			this._valid = true;
		}

		public int specCubeIBL = -1;

		public int skyCubeIBL = -1;

		public int skyMatrix = -1;

		public int invSkyMatrix = -1;

		public int skySize = -1;

		public int skyMin = -1;

		public int skyMax = -1;

		public int[] SH;

		public int exposureIBL = -1;

		public int exposureLM = -1;

		public int oldExposureIBL = -1;

		public int blendWeightIBL = -1;

		private bool _valid;
	}
}
