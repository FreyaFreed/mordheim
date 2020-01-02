using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Flowmap
{
	public class TextureUtilities
	{
		public static string[] GetSupportedFormatsWithExtension()
		{
			string[] array = new string[global::Flowmap.TextureUtilities.SupportedFormats.Length];
			for (int i = 0; i < global::Flowmap.TextureUtilities.SupportedFormats.Length; i++)
			{
				array[i] = global::Flowmap.TextureUtilities.SupportedFormats[i].name + " (*." + global::Flowmap.TextureUtilities.SupportedFormats[i].extension + ")";
			}
			return array;
		}

		public static void WriteRenderTextureToFile(global::UnityEngine.RenderTexture textureToWrite, string filename, global::Flowmap.TextureUtilities.FileTextureFormat format)
		{
			global::Flowmap.TextureUtilities.WriteRenderTextureToFile(textureToWrite, filename, false, format);
		}

		public static void WriteRenderTextureToFile(global::UnityEngine.RenderTexture textureToWrite, string filename, bool linear, global::Flowmap.TextureUtilities.FileTextureFormat format)
		{
			global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(textureToWrite.width, textureToWrite.height, global::UnityEngine.TextureFormat.ARGB32, false, linear);
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(textureToWrite.width, textureToWrite.height, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear);
			global::UnityEngine.Graphics.Blit(textureToWrite, temporary);
			global::UnityEngine.RenderTexture.active = temporary;
			texture2D.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), 0, 0);
			texture2D.Apply(false);
			global::Flowmap.TextureUtilities.WriteTexture2DToFile(texture2D, filename, format);
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(texture2D);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(texture2D);
			}
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
		}

		public static void WriteRenderTextureToFile(global::UnityEngine.RenderTexture textureToWrite, string filename, bool linear, global::Flowmap.TextureUtilities.FileTextureFormat format, string customShader)
		{
			global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(textureToWrite.width, textureToWrite.height, global::UnityEngine.TextureFormat.ARGB32, false, linear);
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(textureToWrite.width, textureToWrite.height, 0, global::UnityEngine.RenderTextureFormat.ARGB32, global::UnityEngine.RenderTextureReadWrite.Linear);
			global::UnityEngine.Material material = new global::UnityEngine.Material(global::UnityEngine.Shader.Find(customShader));
			material.SetTexture("_RenderTex", textureToWrite);
			global::UnityEngine.Graphics.Blit(null, temporary, material);
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(material);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(material);
			}
			global::UnityEngine.RenderTexture.active = temporary;
			texture2D.ReadPixels(new global::UnityEngine.Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), 0, 0);
			texture2D.Apply(false);
			global::Flowmap.TextureUtilities.WriteTexture2DToFile(texture2D, filename, format);
			if (global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Object.Destroy(texture2D);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(texture2D);
			}
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
		}

		public static void WriteTexture2DToFile(global::UnityEngine.Texture2D textureToWrite, string filename, global::Flowmap.TextureUtilities.FileTextureFormat format)
		{
			byte[] bytes = null;
			string name = format.name;
			if (name != null)
			{
				if (global::Flowmap.TextureUtilities.<>f__switch$map7 == null)
				{
					global::Flowmap.TextureUtilities.<>f__switch$map7 = new global::System.Collections.Generic.Dictionary<string, int>(2)
					{
						{
							"Png",
							0
						},
						{
							"Tga",
							1
						}
					};
				}
				int num;
				if (global::Flowmap.TextureUtilities.<>f__switch$map7.TryGetValue(name, out num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							bytes = global::Flowmap.TextureUtilities.EncodeToTGA(textureToWrite);
						}
					}
					else
					{
						bytes = textureToWrite.EncodeToPNG();
					}
				}
			}
			if (!filename.EndsWith("." + format.extension))
			{
				filename = filename + "." + format.extension;
			}
			global::System.IO.File.WriteAllBytes(filename, bytes);
		}

		public static global::UnityEngine.Color SampleColorBilinear(global::UnityEngine.Color[] data, int resolutionX, int resolutionY, float u, float v)
		{
			u = global::UnityEngine.Mathf.Clamp(u * (float)(resolutionX - 1), 0f, (float)(resolutionX - 1));
			v = global::UnityEngine.Mathf.Clamp(v * (float)(resolutionY - 1), 0f, (float)(resolutionY - 1));
			if (global::UnityEngine.Mathf.FloorToInt(u) + resolutionX * global::UnityEngine.Mathf.FloorToInt(v) >= data.Length || global::UnityEngine.Mathf.FloorToInt(u) + resolutionX * global::UnityEngine.Mathf.FloorToInt(v) < 0)
			{
				global::UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"out of range ",
					u,
					" ",
					v,
					" ",
					resolutionX,
					" ",
					resolutionY
				}));
				return global::UnityEngine.Color.black;
			}
			global::UnityEngine.Color a = data[global::UnityEngine.Mathf.FloorToInt(u) + resolutionX * global::UnityEngine.Mathf.FloorToInt(v)];
			global::UnityEngine.Color a2 = data[global::UnityEngine.Mathf.CeilToInt(u) + resolutionX * global::UnityEngine.Mathf.FloorToInt(v)];
			global::UnityEngine.Color a3 = data[global::UnityEngine.Mathf.FloorToInt(u) + resolutionX * global::UnityEngine.Mathf.CeilToInt(v)];
			global::UnityEngine.Color a4 = data[global::UnityEngine.Mathf.CeilToInt(u) + resolutionX * global::UnityEngine.Mathf.CeilToInt(v)];
			float num = global::UnityEngine.Mathf.Floor(u);
			float num2 = global::UnityEngine.Mathf.Floor(u + 1f);
			float num3 = global::UnityEngine.Mathf.Floor(v);
			float num4 = global::UnityEngine.Mathf.Floor(v + 1f);
			global::UnityEngine.Color a5 = (num2 - u) / (num2 - num) * a + (u - num) / (num2 - num) * a2;
			global::UnityEngine.Color a6 = (num2 - u) / (num2 - num) * a3 + (u - num) / (num2 - num) * a4;
			return (num4 - v) / (num4 - num3) * a5 + (v - num3) / (num4 - num3) * a6;
		}

		public static float[,] ReadRawImage(string path, int resX, int resY, bool pcByteOrder)
		{
			float[,] array = new float[resX, resY];
			global::System.IO.FileStream input = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read);
			global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(input);
			for (int i = resY - 1; i > -1; i--)
			{
				for (int j = 0; j < resX; j++)
				{
					byte[] array2 = binaryReader.ReadBytes(2);
					if (!pcByteOrder)
					{
						byte b = array2[0];
						array2[0] = array2[1];
						array2[1] = b;
					}
					ushort num = global::System.BitConverter.ToUInt16(array2, 0);
					array[j, i] = (float)num / 65536f;
				}
			}
			binaryReader.Close();
			return array;
		}

		public static global::UnityEngine.Texture2D ReadRawImageToTexture(string path, int resX, int resY, bool pcByteOrder)
		{
			float[,] heights = global::Flowmap.TextureUtilities.ReadRawImage(path, resX, resY, pcByteOrder);
			global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(resX, resY, global::UnityEngine.TextureFormat.ARGB32, false, true);
			texture2D.wrapMode = global::UnityEngine.TextureWrapMode.Clamp;
			texture2D.anisoLevel = 9;
			texture2D.filterMode = global::UnityEngine.FilterMode.Trilinear;
			int processorCount = global::UnityEngine.SystemInfo.processorCount;
			int num = global::UnityEngine.Mathf.CeilToInt((float)(resY / processorCount));
			global::UnityEngine.Color[] pixels = new global::UnityEngine.Color[resX * resY];
			global::System.Threading.ManualResetEvent[] array = new global::System.Threading.ManualResetEvent[processorCount];
			for (int i = 0; i < processorCount; i++)
			{
				int start = i * num;
				int length = (i != processorCount - 1) ? num : (resX - 1 - i * num);
				array[i] = new global::System.Threading.ManualResetEvent(false);
				global::System.Threading.ThreadPool.QueueUserWorkItem(new global::System.Threading.WaitCallback(global::Flowmap.TextureUtilities.ThreadedEncodeFloat), new global::Flowmap.TextureUtilities.ColorArrayThreadedInfo(start, length, ref pixels, resX, resY, heights, array[i]));
			}
			global::System.Threading.WaitHandle.WaitAll(array);
			texture2D.SetPixels(pixels);
			texture2D.Apply(false);
			return texture2D;
		}

		private static void ThreadedEncodeFloat(object info)
		{
			global::Flowmap.TextureUtilities.ColorArrayThreadedInfo colorArrayThreadedInfo = info as global::Flowmap.TextureUtilities.ColorArrayThreadedInfo;
			try
			{
				for (int i = colorArrayThreadedInfo.start; i < colorArrayThreadedInfo.start + colorArrayThreadedInfo.length; i++)
				{
					for (int j = 0; j < colorArrayThreadedInfo.resY; j++)
					{
						colorArrayThreadedInfo.colorArray[i + j * colorArrayThreadedInfo.resX] = global::Flowmap.TextureUtilities.EncodeFloatRGBA(colorArrayThreadedInfo.heightArray[i, j]);
					}
				}
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.Log(ex.ToString());
			}
			colorArrayThreadedInfo.resetEvent.Set();
		}

		public static global::UnityEngine.Texture2D GetRawPreviewTexture(global::UnityEngine.Texture2D rawTexture)
		{
			global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(rawTexture.width, rawTexture.height, global::UnityEngine.TextureFormat.ARGB32, true, true);
			global::UnityEngine.Color[] array = new global::UnityEngine.Color[texture2D.width * texture2D.height];
			for (int i = 0; i < texture2D.height; i++)
			{
				for (int j = 0; j < texture2D.width; j++)
				{
					float num = global::Flowmap.TextureUtilities.DecodeFloatRGBA(rawTexture.GetPixel(j, i));
					array[j + i * texture2D.width] = new global::UnityEngine.Color(num, num, num, 1f);
				}
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		public static global::UnityEngine.Color EncodeFloatRGBA(float v)
		{
			v = global::UnityEngine.Mathf.Min(v, 0.999f);
			global::UnityEngine.Color a = new global::UnityEngine.Color(1f, 255f, 65025f, 160581376f);
			float num = 0.003921569f;
			global::UnityEngine.Color result = a * v;
			result.r -= global::UnityEngine.Mathf.Floor(result.r);
			result.g -= global::UnityEngine.Mathf.Floor(result.g);
			result.b -= global::UnityEngine.Mathf.Floor(result.b);
			result.a -= global::UnityEngine.Mathf.Floor(result.a);
			result.r -= result.g * num;
			result.g -= result.b * num;
			result.b -= result.a * num;
			result.a -= result.a * num;
			return result;
		}

		public static float DecodeFloatRGBA(global::UnityEngine.Color enc)
		{
			global::UnityEngine.Color c = new global::UnityEngine.Color(1f, 0.003921569f, 1.53787E-05f, 6.2273724E-09f);
			return global::UnityEngine.Vector4.Dot(enc, c);
		}

		public static global::UnityEngine.Texture2D ImportTGA(string path)
		{
			global::UnityEngine.Texture2D result;
			try
			{
				global::System.IO.FileStream input = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read);
				global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(input);
				binaryReader.ReadByte();
				binaryReader.ReadByte();
				binaryReader.ReadByte();
				binaryReader.ReadInt16();
				binaryReader.ReadInt16();
				binaryReader.ReadByte();
				binaryReader.ReadInt16();
				binaryReader.ReadInt16();
				short num = binaryReader.ReadInt16();
				short num2 = binaryReader.ReadInt16();
				byte b = binaryReader.ReadByte();
				binaryReader.ReadByte();
				global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D((int)num, (int)num2, (b != 32) ? global::UnityEngine.TextureFormat.RGB24 : global::UnityEngine.TextureFormat.ARGB32, true);
				global::UnityEngine.Color32[] array = new global::UnityEngine.Color32[(int)(num * num2)];
				for (int i = 0; i < (int)num2; i++)
				{
					for (int j = 0; j < (int)num; j++)
					{
						if (b == 32)
						{
							byte b2 = binaryReader.ReadByte();
							byte g = binaryReader.ReadByte();
							byte r = binaryReader.ReadByte();
							byte a = binaryReader.ReadByte();
							array[j + i * (int)num] = new global::UnityEngine.Color32(r, g, b2, a);
						}
						else
						{
							byte b3 = binaryReader.ReadByte();
							byte g2 = binaryReader.ReadByte();
							byte r2 = binaryReader.ReadByte();
							array[j + i * (int)num] = new global::UnityEngine.Color32(r2, g2, b3, 1);
						}
					}
				}
				texture2D.SetPixels32(array);
				texture2D.Apply();
				result = texture2D;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public static byte[] EncodeToTGA(global::UnityEngine.Texture2D texture)
		{
			global::System.Collections.Generic.List<byte> list = new global::System.Collections.Generic.List<byte>();
			list.Add(0);
			list.Add(0);
			list.Add(2);
			list.AddRange(global::System.BitConverter.GetBytes(0));
			list.AddRange(global::System.BitConverter.GetBytes(0));
			list.Add(0);
			list.AddRange(global::System.BitConverter.GetBytes(0));
			list.AddRange(global::System.BitConverter.GetBytes(0));
			list.AddRange(global::System.BitConverter.GetBytes((short)texture.width));
			list.AddRange(global::System.BitConverter.GetBytes((short)texture.height));
			short num = 0;
			switch (texture.format)
			{
			case global::UnityEngine.TextureFormat.RGB24:
				num = 24;
				break;
			case global::UnityEngine.TextureFormat.ARGB32:
				num = 32;
				break;
			}
			list.AddRange(global::System.BitConverter.GetBytes(num));
			short num2 = num;
			if (num2 != 24)
			{
				if (num2 == 32)
				{
					list.Add(8);
				}
			}
			else
			{
				list.Add(0);
			}
			global::UnityEngine.Color32[] pixels = texture.GetPixels32();
			for (int i = 0; i < texture.height; i++)
			{
				for (int j = 0; j < texture.width; j++)
				{
					list.Add(pixels[j + i * texture.width].g);
					list.Add(pixels[j + i * texture.width].r);
					if (num == 32)
					{
						list.Add(pixels[j + i * texture.width].a);
					}
					list.Add(pixels[j + i * texture.width].b);
				}
			}
			return list.ToArray();
		}

		public static global::Flowmap.TextureUtilities.FileTextureFormat[] SupportedFormats = new global::Flowmap.TextureUtilities.FileTextureFormat[]
		{
			new global::Flowmap.TextureUtilities.FileTextureFormat("Tga", "tga"),
			new global::Flowmap.TextureUtilities.FileTextureFormat("Png", "png")
		};

		public static global::Flowmap.TextureUtilities.FileTextureFormat[] SupportedRawFormats = new global::Flowmap.TextureUtilities.FileTextureFormat[]
		{
			new global::Flowmap.TextureUtilities.FileTextureFormat("Raw", "raw")
		};

		public struct FileTextureFormat
		{
			public FileTextureFormat(string name, string extension)
			{
				this.name = name;
				this.extension = extension;
			}

			public string name;

			public string extension;
		}

		private class ColorArrayThreadedInfo
		{
			public ColorArrayThreadedInfo(int start, int length, ref global::UnityEngine.Color[] colors, int resX, int resY, float[,] heights, global::System.Threading.ManualResetEvent resetEvent)
			{
				this.start = start;
				this.length = length;
				this.resetEvent = resetEvent;
				this.colorArray = colors;
				this.resX = resX;
				this.resY = resY;
				this.heightArray = heights;
			}

			public int start;

			public int length;

			public global::System.Threading.ManualResetEvent resetEvent;

			public global::UnityEngine.Color[] colorArray;

			public float[,] heightArray;

			public int resX;

			public int resY;
		}
	}
}
