using System;
using UnityEngine;

namespace mset
{
	[global::System.Serializable]
	public class SHEncoding
	{
		public SHEncoding()
		{
			this.clearToBlack();
		}

		public void clearToBlack()
		{
			for (int i = 0; i < 27; i++)
			{
				this.c[i] = 0f;
			}
			for (int j = 0; j < 9; j++)
			{
				this.cBuffer[j] = global::UnityEngine.Vector4.zero;
			}
		}

		public bool equals(global::mset.SHEncoding other)
		{
			for (int i = 0; i < 27; i++)
			{
				if (this.c[i] != other.c[i])
				{
					return false;
				}
			}
			return true;
		}

		public void copyFrom(global::mset.SHEncoding src)
		{
			for (int i = 0; i < 27; i++)
			{
				this.c[i] = src.c[i];
			}
			this.copyToBuffer();
		}

		public void copyToBuffer()
		{
			for (int i = 0; i < 9; i++)
			{
				float num = global::mset.SHEncoding.sEquationConstants[i];
				this.cBuffer[i].x = this.c[i * 3] * num;
				this.cBuffer[i].y = this.c[i * 3 + 1] * num;
				this.cBuffer[i].z = this.c[i * 3 + 2] * num;
			}
		}

		public float[] c = new float[27];

		public global::UnityEngine.Vector4[] cBuffer = new global::UnityEngine.Vector4[9];

		public static float[] sEquationConstants = new float[]
		{
			0.282094777f,
			0.4886025f,
			0.4886025f,
			0.4886025f,
			1.09254849f,
			1.09254849f,
			0.31539157f,
			1.09254849f,
			0.546274245f
		};
	}
}
