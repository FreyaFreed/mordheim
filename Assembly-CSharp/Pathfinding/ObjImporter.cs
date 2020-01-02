using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pathfinding
{
	public class ObjImporter
	{
		public static global::UnityEngine.Mesh ImportFile(string filePath)
		{
			if (!global::System.IO.File.Exists(filePath))
			{
				global::UnityEngine.Debug.LogError("No file was found at '" + filePath + "'");
				return null;
			}
			global::Pathfinding.ObjImporter.meshStruct meshStruct = global::Pathfinding.ObjImporter.createMeshStruct(filePath);
			global::Pathfinding.ObjImporter.populateMeshStruct(ref meshStruct);
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[meshStruct.faceData.Length];
			global::UnityEngine.Vector2[] array2 = new global::UnityEngine.Vector2[meshStruct.faceData.Length];
			global::UnityEngine.Vector3[] array3 = new global::UnityEngine.Vector3[meshStruct.faceData.Length];
			int num = 0;
			foreach (global::UnityEngine.Vector3 vector in meshStruct.faceData)
			{
				array[num] = meshStruct.vertices[(int)vector.x - 1];
				if (vector.y >= 1f)
				{
					array2[num] = meshStruct.uv[(int)vector.y - 1];
				}
				if (vector.z >= 1f)
				{
					array3[num] = meshStruct.normals[(int)vector.z - 1];
				}
				num++;
			}
			global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.normals = array3;
			mesh.triangles = meshStruct.triangles;
			mesh.RecalculateBounds();
			return mesh;
		}

		private static global::Pathfinding.ObjImporter.meshStruct createMeshStruct(string filename)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			global::Pathfinding.ObjImporter.meshStruct result = default(global::Pathfinding.ObjImporter.meshStruct);
			result.fileName = filename;
			global::System.IO.StreamReader streamReader = global::System.IO.File.OpenText(filename);
			string s = streamReader.ReadToEnd();
			streamReader.Dispose();
			using (global::System.IO.StringReader stringReader = new global::System.IO.StringReader(s))
			{
				string text = stringReader.ReadLine();
				char[] separator = new char[]
				{
					' '
				};
				while (text != null)
				{
					if (!text.StartsWith("f ") && !text.StartsWith("v ") && !text.StartsWith("vt ") && !text.StartsWith("vn "))
					{
						text = stringReader.ReadLine();
						if (text != null)
						{
							text = text.Replace("  ", " ");
						}
					}
					else
					{
						text = text.Trim();
						string[] array = text.Split(separator, 50);
						string text2 = array[0];
						switch (text2)
						{
						case "v":
							num2++;
							break;
						case "vt":
							num3++;
							break;
						case "vn":
							num4++;
							break;
						case "f":
							num5 = num5 + array.Length - 1;
							num += 3 * (array.Length - 2);
							break;
						}
						text = stringReader.ReadLine();
						if (text != null)
						{
							text = text.Replace("  ", " ");
						}
					}
				}
			}
			result.triangles = new int[num];
			result.vertices = new global::UnityEngine.Vector3[num2];
			result.uv = new global::UnityEngine.Vector2[num3];
			result.normals = new global::UnityEngine.Vector3[num4];
			result.faceData = new global::UnityEngine.Vector3[num5];
			return result;
		}

		private static void populateMeshStruct(ref global::Pathfinding.ObjImporter.meshStruct mesh)
		{
			global::System.IO.StreamReader streamReader = global::System.IO.File.OpenText(mesh.fileName);
			string s = streamReader.ReadToEnd();
			streamReader.Close();
			using (global::System.IO.StringReader stringReader = new global::System.IO.StringReader(s))
			{
				string text = stringReader.ReadLine();
				char[] separator = new char[]
				{
					' '
				};
				char[] separator2 = new char[]
				{
					'/'
				};
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				while (text != null)
				{
					if (!text.StartsWith("f ") && !text.StartsWith("v ") && !text.StartsWith("vt ") && !text.StartsWith("vn ") && !text.StartsWith("g ") && !text.StartsWith("usemtl ") && !text.StartsWith("mtllib ") && !text.StartsWith("vt1 ") && !text.StartsWith("vt2 ") && !text.StartsWith("vc ") && !text.StartsWith("usemap "))
					{
						text = stringReader.ReadLine();
						if (text != null)
						{
							text = text.Replace("  ", " ");
						}
					}
					else
					{
						text = text.Trim();
						string[] array = text.Split(separator, 50);
						string text2 = array[0];
						switch (text2)
						{
						case "v":
							mesh.vertices[num3] = new global::UnityEngine.Vector3(global::System.Convert.ToSingle(array[1]), global::System.Convert.ToSingle(array[2]), global::System.Convert.ToSingle(array[3]));
							num3++;
							break;
						case "vt":
							mesh.uv[num5] = new global::UnityEngine.Vector2(global::System.Convert.ToSingle(array[1]), global::System.Convert.ToSingle(array[2]));
							num5++;
							break;
						case "vt1":
							mesh.uv[num6] = new global::UnityEngine.Vector2(global::System.Convert.ToSingle(array[1]), global::System.Convert.ToSingle(array[2]));
							num6++;
							break;
						case "vt2":
							mesh.uv[num7] = new global::UnityEngine.Vector2(global::System.Convert.ToSingle(array[1]), global::System.Convert.ToSingle(array[2]));
							num7++;
							break;
						case "vn":
							mesh.normals[num4] = new global::UnityEngine.Vector3(global::System.Convert.ToSingle(array[1]), global::System.Convert.ToSingle(array[2]), global::System.Convert.ToSingle(array[3]));
							num4++;
							break;
						case "f":
						{
							int num9 = 1;
							global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
							while (num9 < array.Length && (string.Empty + array[num9]).Length > 0)
							{
								global::UnityEngine.Vector3 vector = default(global::UnityEngine.Vector3);
								string[] array2 = array[num9].Split(separator2, 3);
								vector.x = (float)global::System.Convert.ToInt32(array2[0]);
								if (array2.Length > 1)
								{
									if (array2[1] != string.Empty)
									{
										vector.y = (float)global::System.Convert.ToInt32(array2[1]);
									}
									vector.z = (float)global::System.Convert.ToInt32(array2[2]);
								}
								num9++;
								mesh.faceData[num2] = vector;
								list.Add(num2);
								num2++;
							}
							num9 = 1;
							while (num9 + 2 < array.Length)
							{
								mesh.triangles[num] = list[0];
								num++;
								mesh.triangles[num] = list[num9];
								num++;
								mesh.triangles[num] = list[num9 + 1];
								num++;
								num9++;
							}
							break;
						}
						}
						text = stringReader.ReadLine();
						if (text != null)
						{
							text = text.Replace("  ", " ");
						}
					}
				}
			}
		}

		private struct meshStruct
		{
			public global::UnityEngine.Vector3[] vertices;

			public global::UnityEngine.Vector3[] normals;

			public global::UnityEngine.Vector2[] uv;

			public global::UnityEngine.Vector2[] uv1;

			public global::UnityEngine.Vector2[] uv2;

			public int[] triangles;

			public int[] faceVerts;

			public int[] faceUVs;

			public global::UnityEngine.Vector3[] faceData;

			public string name;

			public string fileName;
		}
	}
}
