using System;
using UnityEngine;

namespace mset
{
	public class CubeBuffer
	{
		public CubeBuffer()
		{
			this.filterMode = global::mset.CubeBuffer.FilterMode.BILINEAR;
			this.clear();
		}

		public global::mset.CubeBuffer.FilterMode filterMode
		{
			get
			{
				return this._filterMode;
			}
			set
			{
				this._filterMode = value;
				switch (this._filterMode)
				{
				case global::mset.CubeBuffer.FilterMode.NEAREST:
					this.sample = new global::mset.CubeBuffer.SampleFunc(this.sampleNearest);
					break;
				case global::mset.CubeBuffer.FilterMode.BILINEAR:
					this.sample = new global::mset.CubeBuffer.SampleFunc(this.sampleBilinear);
					break;
				case global::mset.CubeBuffer.FilterMode.BICUBIC:
					this.sample = new global::mset.CubeBuffer.SampleFunc(this.sampleBicubic);
					break;
				}
			}
		}

		public int width
		{
			get
			{
				return this.faceSize;
			}
		}

		public int height
		{
			get
			{
				return this.faceSize * 6;
			}
		}

		~CubeBuffer()
		{
		}

		public void clear()
		{
			this.pixels = null;
			this.faceSize = 0;
		}

		public bool empty()
		{
			return this.pixels == null || this.pixels.Length == 0;
		}

		public static void pixelCopy(ref global::UnityEngine.Color[] dst, int dst_offset, global::UnityEngine.Color[] src, int src_offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				dst[dst_offset + i] = src[src_offset + i];
			}
		}

		public static void pixelCopy(ref global::UnityEngine.Color[] dst, int dst_offset, global::UnityEngine.Color32[] src, int src_offset, int count)
		{
			float num = 0.003921569f;
			for (int i = 0; i < count; i++)
			{
				dst[dst_offset + i].r = (float)src[src_offset + i].r * num;
				dst[dst_offset + i].g = (float)src[src_offset + i].g * num;
				dst[dst_offset + i].b = (float)src[src_offset + i].b * num;
				dst[dst_offset + i].a = (float)src[src_offset + i].a * num;
			}
		}

		public static void pixelCopy(ref global::UnityEngine.Color32[] dst, int dst_offset, global::UnityEngine.Color[] src, int src_offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				dst[dst_offset + i].r = (byte)global::UnityEngine.Mathf.Clamp(src[src_offset + i].r * 255f, 0f, 255f);
				dst[dst_offset + i].g = (byte)global::UnityEngine.Mathf.Clamp(src[src_offset + i].g * 255f, 0f, 255f);
				dst[dst_offset + i].b = (byte)global::UnityEngine.Mathf.Clamp(src[src_offset + i].b * 255f, 0f, 255f);
				dst[dst_offset + i].a = (byte)global::UnityEngine.Mathf.Clamp(src[src_offset + i].a * 255f, 0f, 255f);
			}
		}

		public static void pixelCopyBlock<T>(ref T[] dst, int dst_x, int dst_y, int dst_w, T[] src, int src_x, int src_y, int src_w, int block_w, int block_h, bool flip)
		{
			if (flip)
			{
				for (int i = 0; i < block_w; i++)
				{
					for (int j = 0; j < block_h; j++)
					{
						int num = (dst_y + j) * dst_w + dst_x + i;
						int num2 = (src_y + (block_h - j - 1)) * src_w + src_x + i;
						dst[num] = src[num2];
					}
				}
			}
			else
			{
				for (int k = 0; k < block_w; k++)
				{
					for (int l = 0; l < block_h; l++)
					{
						int num3 = (dst_y + l) * dst_w + dst_x + k;
						int num4 = (src_y + l) * src_w + src_x + k;
						dst[num3] = src[num4];
					}
				}
			}
		}

		public static void encode(ref global::UnityEngine.Color[] dst, global::UnityEngine.Color[] src, global::mset.ColorMode outMode, bool useGamma)
		{
			if (outMode == global::mset.ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					global::mset.RGB.toRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else if (useGamma)
			{
				global::mset.Util.applyGamma(ref dst, src, global::mset.Gamma.toSRGB);
			}
			else
			{
				global::mset.CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
			}
		}

		public static void encode(ref global::UnityEngine.Color32[] dst, global::UnityEngine.Color[] src, global::mset.ColorMode outMode, bool useGamma)
		{
			if (outMode == global::mset.ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					global::mset.RGB.toRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else
			{
				if (useGamma)
				{
					global::mset.Util.applyGamma(ref src, src, global::mset.Gamma.toSRGB);
				}
				global::mset.CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
			}
		}

		public static void decode(ref global::UnityEngine.Color[] dst, global::UnityEngine.Color[] src, global::mset.ColorMode inMode, bool useGamma)
		{
			if (inMode == global::mset.ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					global::mset.RGB.fromRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else
			{
				if (useGamma)
				{
					global::mset.Util.applyGamma(ref dst, src, global::mset.Gamma.toLinear);
				}
				else
				{
					global::mset.CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
				}
				global::mset.CubeBuffer.clearAlpha(ref dst);
			}
		}

		public static void decode(ref global::UnityEngine.Color[] dst, global::UnityEngine.Color32[] src, global::mset.ColorMode inMode, bool useGamma)
		{
			if (inMode == global::mset.ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					global::mset.RGB.fromRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else
			{
				global::mset.CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
				if (useGamma)
				{
					global::mset.Util.applyGamma(ref dst, global::mset.Gamma.toLinear);
				}
				global::mset.CubeBuffer.clearAlpha(ref dst);
			}
		}

		public static void decode(ref global::UnityEngine.Color[] dst, int dst_offset, global::UnityEngine.Color[] src, int src_offset, int count, global::mset.ColorMode inMode, bool useGamma)
		{
			if (inMode == global::mset.ColorMode.RGBM8)
			{
				for (int i = 0; i < count; i++)
				{
					global::mset.RGB.fromRGBM(ref dst[i + dst_offset], src[i + src_offset], useGamma);
				}
			}
			else
			{
				if (useGamma)
				{
					global::mset.Util.applyGamma(ref dst, dst_offset, src, src_offset, count, global::mset.Gamma.toLinear);
				}
				else
				{
					global::mset.CubeBuffer.pixelCopy(ref dst, dst_offset, src, src_offset, count);
				}
				global::mset.CubeBuffer.clearAlpha(ref dst, dst_offset, count);
			}
		}

		public static void decode(ref global::UnityEngine.Color[] dst, int dst_offset, global::UnityEngine.Color32[] src, int src_offset, int count, global::mset.ColorMode inMode, bool useGamma)
		{
			if (inMode == global::mset.ColorMode.RGBM8)
			{
				for (int i = 0; i < count; i++)
				{
					global::mset.RGB.fromRGBM(ref dst[i + dst_offset], src[i + src_offset], useGamma);
				}
			}
			else
			{
				global::mset.CubeBuffer.pixelCopy(ref dst, dst_offset, src, src_offset, count);
				if (useGamma)
				{
					global::mset.Util.applyGamma(ref dst, dst_offset, dst, dst_offset, count, global::mset.Gamma.toLinear);
				}
				global::mset.CubeBuffer.clearAlpha(ref dst, dst_offset, count);
			}
		}

		public static void clearAlpha(ref global::UnityEngine.Color[] dst)
		{
			global::mset.CubeBuffer.clearAlpha(ref dst, 0, dst.Length);
		}

		public static void clearAlpha(ref global::UnityEngine.Color[] dst, int offset, int count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				dst[i].a = 1f;
			}
		}

		public static void clearAlpha(ref global::UnityEngine.Color32[] dst)
		{
			global::mset.CubeBuffer.clearAlpha(ref dst, 0, dst.Length);
		}

		public static void clearAlpha(ref global::UnityEngine.Color32[] dst, int offset, int count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				dst[i].a = byte.MaxValue;
			}
		}

		public static void applyExposure(ref global::UnityEngine.Color[] pixels, float mult)
		{
			for (int i = 0; i < pixels.Length; i++)
			{
				global::UnityEngine.Color[] array = pixels;
				int num = i;
				array[num].r = array[num].r * mult;
				global::UnityEngine.Color[] array2 = pixels;
				int num2 = i;
				array2[num2].g = array2[num2].g * mult;
				global::UnityEngine.Color[] array3 = pixels;
				int num3 = i;
				array3[num3].b = array3[num3].b * mult;
			}
		}

		public void applyExposure(float mult)
		{
			for (int i = 0; i < this.pixels.Length; i++)
			{
				global::UnityEngine.Color[] array = this.pixels;
				int num = i;
				array[num].r = array[num].r * mult;
				global::UnityEngine.Color[] array2 = this.pixels;
				int num2 = i;
				array2[num2].g = array2[num2].g * mult;
				global::UnityEngine.Color[] array3 = this.pixels;
				int num3 = i;
				array3[num3].b = array3[num3].b * mult;
			}
		}

		public int toIndex(int face, int x, int y)
		{
			x = global::UnityEngine.Mathf.Clamp(x, 0, this.faceSize - 1);
			y = global::UnityEngine.Mathf.Clamp(y, 0, this.faceSize - 1);
			return this.faceSize * this.faceSize * face + this.faceSize * y + x;
		}

		public int toIndex(global::UnityEngine.CubemapFace face, int x, int y)
		{
			x = global::UnityEngine.Mathf.Clamp(x, 0, this.faceSize - 1);
			y = global::UnityEngine.Mathf.Clamp(y, 0, this.faceSize - 1);
			return this.faceSize * this.faceSize * (int)face + this.faceSize * y + x;
		}

		private static void linkEdges()
		{
			if (global::mset.CubeBuffer._leftEdges == null)
			{
				global::mset.CubeBuffer._leftEdges = new global::mset.CubeBuffer.CubeEdge[6];
				global::mset.CubeBuffer._leftEdges[1] = new global::mset.CubeBuffer.CubeEdge(5, false, false);
				global::mset.CubeBuffer._leftEdges[0] = new global::mset.CubeBuffer.CubeEdge(4, false, false);
				global::mset.CubeBuffer._leftEdges[3] = new global::mset.CubeBuffer.CubeEdge(1, true, true);
				global::mset.CubeBuffer._leftEdges[2] = new global::mset.CubeBuffer.CubeEdge(1, false, true, true);
				global::mset.CubeBuffer._leftEdges[5] = new global::mset.CubeBuffer.CubeEdge(0, false, false);
				global::mset.CubeBuffer._leftEdges[4] = new global::mset.CubeBuffer.CubeEdge(1, false, false);
				global::mset.CubeBuffer._rightEdges = new global::mset.CubeBuffer.CubeEdge[6];
				global::mset.CubeBuffer._rightEdges[1] = new global::mset.CubeBuffer.CubeEdge(4, false, false);
				global::mset.CubeBuffer._rightEdges[0] = new global::mset.CubeBuffer.CubeEdge(5, false, false);
				global::mset.CubeBuffer._rightEdges[3] = new global::mset.CubeBuffer.CubeEdge(0, false, true, true);
				global::mset.CubeBuffer._rightEdges[2] = new global::mset.CubeBuffer.CubeEdge(0, true, true);
				global::mset.CubeBuffer._rightEdges[5] = new global::mset.CubeBuffer.CubeEdge(1, false, false);
				global::mset.CubeBuffer._rightEdges[4] = new global::mset.CubeBuffer.CubeEdge(0, false, false);
				global::mset.CubeBuffer._upEdges = new global::mset.CubeBuffer.CubeEdge[6];
				global::mset.CubeBuffer._upEdges[1] = new global::mset.CubeBuffer.CubeEdge(2, false, true, true);
				global::mset.CubeBuffer._upEdges[0] = new global::mset.CubeBuffer.CubeEdge(2, true, true);
				global::mset.CubeBuffer._upEdges[3] = new global::mset.CubeBuffer.CubeEdge(4, false, false);
				global::mset.CubeBuffer._upEdges[2] = new global::mset.CubeBuffer.CubeEdge(5, true, false, true);
				global::mset.CubeBuffer._upEdges[5] = new global::mset.CubeBuffer.CubeEdge(2, true, false, true);
				global::mset.CubeBuffer._upEdges[4] = new global::mset.CubeBuffer.CubeEdge(2, false, false);
				global::mset.CubeBuffer._downEdges = new global::mset.CubeBuffer.CubeEdge[6];
				global::mset.CubeBuffer._downEdges[1] = new global::mset.CubeBuffer.CubeEdge(3, true, true);
				global::mset.CubeBuffer._downEdges[0] = new global::mset.CubeBuffer.CubeEdge(3, false, true, true);
				global::mset.CubeBuffer._downEdges[3] = new global::mset.CubeBuffer.CubeEdge(5, true, false, true);
				global::mset.CubeBuffer._downEdges[2] = new global::mset.CubeBuffer.CubeEdge(4, false, false);
				global::mset.CubeBuffer._downEdges[5] = new global::mset.CubeBuffer.CubeEdge(3, true, false, true);
				global::mset.CubeBuffer._downEdges[4] = new global::mset.CubeBuffer.CubeEdge(3, false, false);
				for (int i = 0; i < 6; i++)
				{
					global::mset.CubeBuffer._leftEdges[i].minEdge = (global::mset.CubeBuffer._upEdges[i].minEdge = true);
					global::mset.CubeBuffer._rightEdges[i].minEdge = (global::mset.CubeBuffer._downEdges[i].minEdge = false);
				}
			}
		}

		public int toIndexLinked(int face, int u, int v)
		{
			global::mset.CubeBuffer.linkEdges();
			int num = face;
			global::mset.CubeBuffer._leftEdges[num].transmogrify(ref u, ref v, ref num, this.faceSize);
			global::mset.CubeBuffer._upEdges[num].transmogrify(ref v, ref u, ref num, this.faceSize);
			global::mset.CubeBuffer._rightEdges[num].transmogrify(ref u, ref v, ref num, this.faceSize);
			global::mset.CubeBuffer._downEdges[num].transmogrify(ref v, ref u, ref num, this.faceSize);
			u = global::UnityEngine.Mathf.Clamp(u, 0, this.faceSize - 1);
			v = global::UnityEngine.Mathf.Clamp(v, 0, this.faceSize - 1);
			return this.toIndex(num, u, v);
		}

		public void sampleNearest(ref global::UnityEngine.Color dst, float u, float v, int face)
		{
			int num = global::UnityEngine.Mathf.FloorToInt((float)this.faceSize * u);
			int num2 = global::UnityEngine.Mathf.FloorToInt((float)this.faceSize * v);
			dst = this.pixels[this.faceSize * this.faceSize * face + this.faceSize * num2 + num];
		}

		public void sampleBilinear(ref global::UnityEngine.Color dst, float u, float v, int face)
		{
			u = (float)this.faceSize * u + 0.5f;
			int num = global::UnityEngine.Mathf.FloorToInt(u) - 1;
			u = global::UnityEngine.Mathf.Repeat(u, 1f);
			v = (float)this.faceSize * v + 0.5f;
			int num2 = global::UnityEngine.Mathf.FloorToInt(v) - 1;
			v = global::UnityEngine.Mathf.Repeat(v, 1f);
			int num3 = this.toIndexLinked(face, num, num2);
			int num4 = this.toIndexLinked(face, num + 1, num2);
			int num5 = this.toIndexLinked(face, num + 1, num2 + 1);
			int num6 = this.toIndexLinked(face, num, num2 + 1);
			global::UnityEngine.Color a = global::UnityEngine.Color.Lerp(this.pixels[num3], this.pixels[num4], u);
			global::UnityEngine.Color b = global::UnityEngine.Color.Lerp(this.pixels[num6], this.pixels[num5], u);
			dst = global::UnityEngine.Color.Lerp(a, b, v);
		}

		public void sampleBicubic(ref global::UnityEngine.Color dst, float u, float v, int face)
		{
			u = (float)this.faceSize * u + 0.5f;
			int num = global::UnityEngine.Mathf.FloorToInt(u) - 1;
			u = global::UnityEngine.Mathf.Repeat(u, 1f);
			v = (float)this.faceSize * v + 0.5f;
			int num2 = global::UnityEngine.Mathf.FloorToInt(v) - 1;
			v = global::UnityEngine.Mathf.Repeat(v, 1f);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int num3 = this.toIndexLinked(face, num - 1 + i, num2 - 1 + j);
					global::mset.CubeBuffer.cubicKernel[i, j] = this.pixels[num3];
				}
			}
			float t = 0.85f;
			float b = 0.333f;
			global::UnityEngine.Color color;
			global::UnityEngine.Color a;
			global::UnityEngine.Color color2;
			global::UnityEngine.Color color3;
			global::UnityEngine.Color a2;
			global::UnityEngine.Color color4;
			global::UnityEngine.Color b2;
			for (int k = 0; k < 4; k++)
			{
				color = global::mset.CubeBuffer.cubicKernel[0, k];
				a = global::mset.CubeBuffer.cubicKernel[1, k];
				color2 = global::mset.CubeBuffer.cubicKernel[2, k];
				color3 = global::mset.CubeBuffer.cubicKernel[3, k];
				color = global::UnityEngine.Color.Lerp(a, color, t);
				color3 = global::UnityEngine.Color.Lerp(color2, color3, t);
				color = a + b * (a - color);
				color3 = color2 + b * (color2 - color3);
				a2 = global::UnityEngine.Color.Lerp(a, color, u);
				color4 = global::UnityEngine.Color.Lerp(color, color3, u);
				b2 = global::UnityEngine.Color.Lerp(color3, color2, u);
				a2 = global::UnityEngine.Color.Lerp(a2, color4, u);
				color4 = global::UnityEngine.Color.Lerp(color4, b2, u);
				global::mset.CubeBuffer.cubicKernel[0, k] = global::UnityEngine.Color.Lerp(a2, color4, u);
			}
			color = global::mset.CubeBuffer.cubicKernel[0, 0];
			a = global::mset.CubeBuffer.cubicKernel[0, 1];
			color2 = global::mset.CubeBuffer.cubicKernel[0, 2];
			color3 = global::mset.CubeBuffer.cubicKernel[0, 3];
			color = global::UnityEngine.Color.Lerp(a, color, t);
			color3 = global::UnityEngine.Color.Lerp(color2, color3, t);
			color = a + b * (a - color);
			color3 = color2 + b * (color2 - color3);
			a2 = global::UnityEngine.Color.Lerp(a, color, v);
			color4 = global::UnityEngine.Color.Lerp(color, color3, v);
			b2 = global::UnityEngine.Color.Lerp(color3, color2, v);
			a2 = global::UnityEngine.Color.Lerp(a2, color4, v);
			color4 = global::UnityEngine.Color.Lerp(color4, b2, v);
			dst = global::UnityEngine.Color.Lerp(a2, color4, v);
		}

		public void resize(int newFaceSize)
		{
			if (newFaceSize == this.faceSize)
			{
				return;
			}
			this.faceSize = newFaceSize;
			this.pixels = null;
			this.pixels = new global::UnityEngine.Color[this.faceSize * this.faceSize * 6];
			global::mset.Util.clearTo(ref this.pixels, global::UnityEngine.Color.black);
		}

		public void resize(int newFaceSize, global::UnityEngine.Color clearColor)
		{
			this.resize(newFaceSize);
			global::mset.Util.clearTo(ref this.pixels, clearColor);
		}

		public void resample(int newSize)
		{
			if (newSize == this.faceSize)
			{
				return;
			}
			global::UnityEngine.Color[] array = new global::UnityEngine.Color[newSize * newSize * 6];
			this.resample(ref array, newSize);
			this.pixels = array;
			this.faceSize = newSize;
		}

		public void resample(ref global::UnityEngine.Color[] dst, int newSize)
		{
			int num = newSize * newSize;
			float num2 = 1f / (float)newSize;
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < newSize; j++)
				{
					float v = ((float)j + 0.5f) * num2;
					for (int k = 0; k < newSize; k++)
					{
						float u = ((float)k + 0.5f) * num2;
						int num3 = num * i + j * newSize + k;
						this.sample(ref dst[num3], u, v, i);
					}
				}
			}
		}

		public void resampleFace(ref global::UnityEngine.Color[] dst, int face, int newSize, bool flipY)
		{
			this.resampleFace(ref dst, 0, face, newSize, flipY);
		}

		public void resampleFace(ref global::UnityEngine.Color[] dst, int dstOffset, int face, int newSize, bool flipY)
		{
			if (newSize == this.faceSize)
			{
				global::mset.CubeBuffer.pixelCopy(ref dst, dstOffset, this.pixels, face * this.faceSize * this.faceSize, this.faceSize * this.faceSize);
				return;
			}
			float num = 1f / (float)newSize;
			if (flipY)
			{
				for (int i = 0; i < newSize; i++)
				{
					float v = 1f - ((float)i + 0.5f) * num;
					for (int j = 0; j < newSize; j++)
					{
						float u = ((float)j + 0.5f) * num;
						int num2 = i * newSize + j + dstOffset;
						this.sample(ref dst[num2], u, v, face);
					}
				}
			}
			else
			{
				for (int k = 0; k < newSize; k++)
				{
					float v2 = ((float)k + 0.5f) * num;
					for (int l = 0; l < newSize; l++)
					{
						float u2 = ((float)l + 0.5f) * num;
						int num3 = k * newSize + l + dstOffset;
						this.sample(ref dst[num3], u2, v2, face);
					}
				}
			}
		}

		public void fromCube(global::UnityEngine.Cubemap cube, int mip, global::mset.ColorMode cubeColorMode, bool useGamma)
		{
			int num = cube.width >> mip;
			if (this.pixels == null || this.faceSize != num)
			{
				this.resize(num);
			}
			for (int i = 0; i < 6; i++)
			{
				global::UnityEngine.Color[] array = cube.GetPixels((global::UnityEngine.CubemapFace)i, mip);
				global::mset.CubeBuffer.pixelCopy(ref this.pixels, i * this.faceSize * this.faceSize, array, 0, array.Length);
			}
			global::mset.CubeBuffer.decode(ref this.pixels, this.pixels, cubeColorMode, useGamma);
		}

		public void toCube(ref global::UnityEngine.Cubemap cube, int mip, global::mset.ColorMode cubeColorMode, bool useGamma)
		{
			int num = this.faceSize * this.faceSize;
			global::UnityEngine.Color[] array = new global::UnityEngine.Color[num];
			for (int i = 0; i < 6; i++)
			{
				global::mset.CubeBuffer.pixelCopy(ref array, 0, this.pixels, i * num, num);
				global::mset.CubeBuffer.encode(ref array, array, cubeColorMode, useGamma);
				cube.SetPixels(array, (global::UnityEngine.CubemapFace)i, mip);
			}
			cube.Apply(false);
		}

		public void resampleToCube(ref global::UnityEngine.Cubemap cube, int mip, global::mset.ColorMode cubeColorMode, bool useGamma, float exposureMult)
		{
			int num = cube.width >> mip;
			int num2 = num * num * 6;
			global::UnityEngine.Color[] array = new global::UnityEngine.Color[num2];
			for (int i = 0; i < 6; i++)
			{
				this.resampleFace(ref array, i, num, false);
				if (exposureMult != 1f)
				{
					global::mset.CubeBuffer.applyExposure(ref array, exposureMult);
				}
				global::mset.CubeBuffer.encode(ref array, array, cubeColorMode, useGamma);
				cube.SetPixels(array, (global::UnityEngine.CubemapFace)i, mip);
			}
			cube.Apply(false);
		}

		public void resampleToBuffer(ref global::mset.CubeBuffer dst, float exposureMult)
		{
			int num = dst.faceSize * dst.faceSize;
			for (int i = 0; i < 6; i++)
			{
				this.resampleFace(ref dst.pixels, i * num, i, dst.faceSize, false);
				dst.applyExposure(exposureMult);
			}
		}

		public void fromBuffer(global::mset.CubeBuffer src)
		{
			this.clear();
			this.faceSize = src.faceSize;
			this.pixels = new global::UnityEngine.Color[src.pixels.Length];
			global::mset.CubeBuffer.pixelCopy(ref this.pixels, 0, src.pixels, 0, this.pixels.Length);
		}

		public void fromPanoTexture(global::UnityEngine.Texture2D tex, int _faceSize, global::mset.ColorMode texColorMode, bool useGamma)
		{
			this.resize(_faceSize);
			ulong num = (ulong)((long)this.faceSize);
			for (ulong num2 = 0UL; num2 < 6UL; num2 += 1UL)
			{
				for (ulong num3 = 0UL; num3 < num; num3 += 1UL)
				{
					for (ulong num4 = 0UL; num4 < num; num4 += 1UL)
					{
						float u = 0f;
						float num5 = 0f;
						global::mset.Util.cubeToLatLongLookup(ref u, ref num5, num2, num4, num3, num);
						float num6 = 1f / (float)this.faceSize;
						num5 = global::UnityEngine.Mathf.Clamp(num5, num6, 1f - num6);
						this.pixels[(int)(checked((global::System.IntPtr)(unchecked(num2 * num * num + num3 * num + num4))))] = tex.GetPixelBilinear(u, num5);
					}
				}
			}
			global::mset.CubeBuffer.decode(ref this.pixels, this.pixels, texColorMode, useGamma);
		}

		public void fromColTexture(global::UnityEngine.Texture2D tex, global::mset.ColorMode texColorMode, bool useGamma)
		{
			this.fromColTexture(tex, 0, texColorMode, useGamma);
		}

		public void fromColTexture(global::UnityEngine.Texture2D tex, int mip, global::mset.ColorMode texColorMode, bool useGamma)
		{
			if (tex.width * 6 != tex.height)
			{
				global::UnityEngine.Debug.LogError("CubeBuffer.fromColTexture takes textures of a 1x6 aspect ratio");
				return;
			}
			int num = tex.width >> mip;
			if (this.pixels == null || this.faceSize != num)
			{
				this.resize(num);
			}
			global::UnityEngine.Color32[] pixels = tex.GetPixels32(mip);
			if ((float)pixels[0].a != 1f)
			{
				global::mset.CubeBuffer.clearAlpha(ref pixels);
			}
			global::mset.CubeBuffer.decode(ref this.pixels, pixels, texColorMode, useGamma);
		}

		public void fromHorizCrossTexture(global::UnityEngine.Texture2D tex, global::mset.ColorMode texColorMode, bool useGamma)
		{
			this.fromHorizCrossTexture(tex, 0, texColorMode, useGamma);
		}

		public void fromHorizCrossTexture(global::UnityEngine.Texture2D tex, int mip, global::mset.ColorMode texColorMode, bool useGamma)
		{
			if (tex.width * 3 != tex.height * 4)
			{
				global::UnityEngine.Debug.LogError("CubeBuffer.fromHorizCrossTexture takes textures of a 4x3 aspect ratio");
				return;
			}
			int num = tex.width / 4 >> mip;
			if (this.pixels == null || this.faceSize != num)
			{
				this.resize(num);
			}
			global::UnityEngine.Color32[] pixels = tex.GetPixels32(mip);
			if ((float)pixels[0].a != 1f)
			{
				global::mset.CubeBuffer.clearAlpha(ref pixels);
			}
			global::UnityEngine.Color32[] src = new global::UnityEngine.Color32[this.faceSize * this.faceSize];
			for (int i = 0; i < 6; i++)
			{
				global::UnityEngine.CubemapFace cubemapFace = (global::UnityEngine.CubemapFace)i;
				int src_x = 0;
				int src_y = 0;
				int dst_offset = i * this.faceSize * this.faceSize;
				switch (cubemapFace)
				{
				case global::UnityEngine.CubemapFace.PositiveX:
					src_x = this.faceSize * 2;
					src_y = this.faceSize * 1;
					break;
				case global::UnityEngine.CubemapFace.NegativeX:
					src_x = 0;
					src_y = this.faceSize * 1;
					break;
				case global::UnityEngine.CubemapFace.PositiveY:
					src_x = this.faceSize * 1;
					src_y = this.faceSize * 2;
					break;
				case global::UnityEngine.CubemapFace.NegativeY:
					src_x = this.faceSize * 1;
					src_y = 0;
					break;
				case global::UnityEngine.CubemapFace.PositiveZ:
					src_x = this.faceSize * 1;
					src_y = this.faceSize * 1;
					break;
				case global::UnityEngine.CubemapFace.NegativeZ:
					src_x = this.faceSize * 3;
					src_y = this.faceSize * 1;
					break;
				}
				global::mset.CubeBuffer.pixelCopyBlock<global::UnityEngine.Color32>(ref src, 0, 0, this.faceSize, pixels, src_x, src_y, this.faceSize * 4, this.faceSize, this.faceSize, true);
				global::mset.CubeBuffer.decode(ref this.pixels, dst_offset, src, 0, this.faceSize * this.faceSize, texColorMode, useGamma);
			}
		}

		public void toColTexture(ref global::UnityEngine.Texture2D tex, global::mset.ColorMode texColorMode, bool useGamma)
		{
			if (tex.width != this.faceSize || tex.height != this.faceSize * 6)
			{
				tex.Resize(this.faceSize, 6 * this.faceSize);
			}
			global::UnityEngine.Color32[] pixels = tex.GetPixels32();
			global::mset.CubeBuffer.encode(ref pixels, this.pixels, texColorMode, useGamma);
			tex.SetPixels32(pixels);
			tex.Apply(false);
		}

		public void toPanoTexture(ref global::UnityEngine.Texture2D tex, global::mset.ColorMode texColorMode, bool useGamma)
		{
			ulong num = (ulong)((long)tex.width);
			ulong num2 = (ulong)((long)tex.height);
			global::UnityEngine.Color[] array = tex.GetPixels();
			for (ulong num3 = 0UL; num3 < num; num3 += 1UL)
			{
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					float u = 0f;
					float v = 0f;
					ulong num5 = 0UL;
					global::mset.Util.latLongToCubeLookup(ref u, ref v, ref num5, num3, num4, num, num2);
					this.sample(ref array[(int)(checked((global::System.IntPtr)(unchecked(num4 * num + num3))))], u, v, (int)num5);
				}
			}
			global::mset.CubeBuffer.encode(ref array, array, texColorMode, useGamma);
			tex.SetPixels(array);
			tex.Apply(tex.mipmapCount > 1);
		}

		public void toPanoBuffer(ref global::UnityEngine.Color[] buffer, int width, int height)
		{
			ulong num = (ulong)((long)width);
			ulong num2 = (ulong)((long)height);
			for (ulong num3 = 0UL; num3 < num; num3 += 1UL)
			{
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					float u = 0f;
					float v = 0f;
					ulong num5 = 0UL;
					global::mset.Util.latLongToCubeLookup(ref u, ref v, ref num5, num3, num4, num, num2);
					this.sample(ref buffer[(int)(checked((global::System.IntPtr)(unchecked(num4 * num + num3))))], u, v, (int)num5);
				}
			}
		}

		public global::mset.CubeBuffer.SampleFunc sample;

		private global::mset.CubeBuffer.FilterMode _filterMode;

		public int faceSize;

		public global::UnityEngine.Color[] pixels;

		private static global::mset.CubeBuffer.CubeEdge[] _leftEdges = null;

		private static global::mset.CubeBuffer.CubeEdge[] _rightEdges = null;

		private static global::mset.CubeBuffer.CubeEdge[] _upEdges = null;

		private static global::mset.CubeBuffer.CubeEdge[] _downEdges = null;

		private static global::UnityEngine.Color[,] cubicKernel = new global::UnityEngine.Color[4, 4];

		public enum FilterMode
		{
			NEAREST,
			BILINEAR,
			BICUBIC
		}

		private class CubeEdge
		{
			public CubeEdge(int Other, bool flip, bool swizzle)
			{
				this.other = Other;
				this.flipped = flip;
				this.swizzled = swizzle;
				this.mirrored = false;
				this.minEdge = false;
			}

			public CubeEdge(int Other, bool flip, bool swizzle, bool mirror)
			{
				this.other = Other;
				this.flipped = flip;
				this.swizzled = swizzle;
				this.mirrored = mirror;
				this.minEdge = false;
			}

			public void transmogrify(ref int primary, ref int secondary, ref int face, int faceSize)
			{
				bool flag = false;
				if (this.minEdge && primary < 0)
				{
					primary = faceSize + primary;
					flag = true;
				}
				else if (!this.minEdge && primary >= faceSize)
				{
					primary %= faceSize;
					flag = true;
				}
				if (flag)
				{
					if (this.mirrored)
					{
						primary = faceSize - primary - 1;
					}
					if (this.flipped)
					{
						secondary = faceSize - secondary - 1;
					}
					if (this.swizzled)
					{
						int num = secondary;
						secondary = primary;
						primary = num;
					}
					face = this.other;
				}
			}

			public void transmogrify(ref int primary_i, ref int primary_j, ref int secondary_i, ref int secondary_j, ref int face_i, ref int face_j, int faceSize)
			{
				if (primary_i < 0)
				{
					primary_i = (primary_j = faceSize - 1);
				}
				else
				{
					primary_i = (primary_j = 0);
				}
				if (this.mirrored)
				{
					primary_i = faceSize - primary_i - 1;
					primary_j = faceSize - primary_j - 1;
				}
				if (this.flipped)
				{
					secondary_i = faceSize - secondary_i - 1;
					secondary_j = faceSize - secondary_j - 1;
				}
				if (this.swizzled)
				{
					int num = secondary_i;
					secondary_i = primary_i;
					primary_i = num;
					num = secondary_j;
					secondary_j = primary_j;
					primary_j = num;
				}
				face_i = (face_j = this.other);
			}

			public int other;

			public bool flipped;

			public bool swizzled;

			public bool mirrored;

			public bool minEdge;
		}

		public delegate void SampleFunc(ref global::UnityEngine.Color dst, float u, float v, int face);
	}
}
