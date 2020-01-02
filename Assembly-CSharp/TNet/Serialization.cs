using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TNet
{
	public static class Serialization
	{
		public static global::System.Type NameToType(string name)
		{
			global::System.Type type = null;
			if (!global::TNet.Serialization.mNameToType.TryGetValue(name, out type))
			{
				if (name == "Vector2")
				{
					type = typeof(global::UnityEngine.Vector2);
				}
				else if (name == "Vector3")
				{
					type = typeof(global::UnityEngine.Vector3);
				}
				else if (name == "Vector4")
				{
					type = typeof(global::UnityEngine.Vector4);
				}
				else if (name == "Euler" || name == "Quaternion")
				{
					type = typeof(global::UnityEngine.Quaternion);
				}
				else if (name == "Rect")
				{
					type = typeof(global::UnityEngine.Rect);
				}
				else if (name == "Color")
				{
					type = typeof(global::UnityEngine.Color);
				}
				else if (name == "Color32")
				{
					type = typeof(global::UnityEngine.Color32);
				}
				else if (name.StartsWith("IList"))
				{
					if (name.Length > 7 && name[5] == '<' && name[name.Length - 1] == '>')
					{
						global::System.Type type2 = global::TNet.Serialization.NameToType(name.Substring(6, name.Length - 7));
						type = typeof(global::System.Collections.Generic.List<>).MakeGenericType(new global::System.Type[]
						{
							type2
						});
					}
					else
					{
						global::UnityEngine.Debug.LogWarning("Malformed type: " + name);
					}
				}
				else if (name.StartsWith("TList"))
				{
					if (name.Length > 7 && name[5] == '<' && name[name.Length - 1] == '>')
					{
						global::System.Type type3 = global::TNet.Serialization.NameToType(name.Substring(6, name.Length - 7));
						type = typeof(global::TNet.List<>).MakeGenericType(new global::System.Type[]
						{
							type3
						});
					}
					else
					{
						global::UnityEngine.Debug.LogWarning("Malformed type: " + name);
					}
				}
				else
				{
					type = global::System.Type.GetType(name);
				}
				global::TNet.Serialization.mNameToType[name] = type;
			}
			return type;
		}

		public static string TypeToName(global::System.Type type)
		{
			if (type == null)
			{
				global::UnityEngine.Debug.LogError("Type cannot be null");
				return null;
			}
			string text;
			if (!global::TNet.Serialization.mTypeToName.TryGetValue(type, out text))
			{
				if (type == typeof(global::UnityEngine.Vector2))
				{
					text = "Vector2";
				}
				else if (type == typeof(global::UnityEngine.Vector3))
				{
					text = "Vector3";
				}
				else if (type == typeof(global::UnityEngine.Vector4))
				{
					text = "Vector4";
				}
				else if (type == typeof(global::UnityEngine.Quaternion))
				{
					text = "Euler";
				}
				else if (type == typeof(global::UnityEngine.Rect))
				{
					text = "Rect";
				}
				else if (type == typeof(global::UnityEngine.Color))
				{
					text = "Color";
				}
				else if (type == typeof(global::UnityEngine.Color32))
				{
					text = "Color32";
				}
				else if (type.Implements(typeof(global::System.Collections.IList)))
				{
					global::System.Type genericArgument = type.GetGenericArgument();
					if (genericArgument != null)
					{
						text = "IList<" + genericArgument.ToString() + ">";
					}
					else
					{
						text = type.ToString();
					}
				}
				else if (type.Implements(typeof(global::TNet.TList)))
				{
					global::System.Type genericArgument2 = type.GetGenericArgument();
					if (genericArgument2 != null)
					{
						text = "TList<" + genericArgument2.ToString() + ">";
					}
					else
					{
						text = type.ToString();
					}
				}
				else
				{
					text = type.ToString();
				}
				global::TNet.Serialization.mTypeToName[type] = text;
			}
			return text;
		}

		public static object ConvertValue(object value, global::System.Type desiredType)
		{
			if (value == null)
			{
				return null;
			}
			global::System.Type type = value.GetType();
			if (desiredType.IsAssignableFrom(type))
			{
				return value;
			}
			if (type == typeof(int))
			{
				if (desiredType == typeof(byte))
				{
					return (byte)((int)value);
				}
				if (desiredType == typeof(short))
				{
					return (short)((int)value);
				}
				if (desiredType == typeof(ushort))
				{
					return (ushort)((int)value);
				}
			}
			else if (type == typeof(float))
			{
				if (desiredType == typeof(byte))
				{
					return (byte)global::UnityEngine.Mathf.RoundToInt((float)value);
				}
				if (desiredType == typeof(short))
				{
					return (short)global::UnityEngine.Mathf.RoundToInt((float)value);
				}
				if (desiredType == typeof(ushort))
				{
					return (ushort)global::UnityEngine.Mathf.RoundToInt((float)value);
				}
				if (desiredType == typeof(int))
				{
					return global::UnityEngine.Mathf.RoundToInt((float)value);
				}
			}
			else if (type == typeof(global::UnityEngine.Color32))
			{
				if (desiredType == typeof(global::UnityEngine.Color))
				{
					global::UnityEngine.Color32 color = (global::UnityEngine.Color32)value;
					return new global::UnityEngine.Color((float)color.r / 255f, (float)color.g / 255f, (float)color.b / 255f, (float)color.a / 255f);
				}
			}
			else if (type == typeof(global::UnityEngine.Vector3))
			{
				if (desiredType == typeof(global::UnityEngine.Color))
				{
					global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)value;
					return new global::UnityEngine.Color(vector.x, vector.y, vector.z);
				}
				if (desiredType == typeof(global::UnityEngine.Quaternion))
				{
					return global::UnityEngine.Quaternion.Euler((global::UnityEngine.Vector3)value);
				}
			}
			else if (type == typeof(global::UnityEngine.Color))
			{
				if (desiredType == typeof(global::UnityEngine.Quaternion))
				{
					global::UnityEngine.Color color2 = (global::UnityEngine.Color)value;
					return new global::UnityEngine.Quaternion(color2.r, color2.g, color2.b, color2.a);
				}
				if (desiredType == typeof(global::UnityEngine.Rect))
				{
					global::UnityEngine.Color color3 = (global::UnityEngine.Color)value;
					return new global::UnityEngine.Rect(color3.r, color3.g, color3.b, color3.a);
				}
				if (desiredType == typeof(global::UnityEngine.Vector4))
				{
					global::UnityEngine.Color color4 = (global::UnityEngine.Color)value;
					return new global::UnityEngine.Vector4(color4.r, color4.g, color4.b, color4.a);
				}
			}
			if (desiredType.IsEnum)
			{
				if (type == typeof(int))
				{
					return value;
				}
				if (type == typeof(string))
				{
					string text = (string)value;
					if (!string.IsNullOrEmpty(text))
					{
						string[] names = global::System.Enum.GetNames(desiredType);
						for (int i = 0; i < names.Length; i++)
						{
							if (names[i] == text)
							{
								return global::System.Enum.GetValues(desiredType).GetValue(i);
							}
						}
					}
				}
			}
			global::UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Unable to convert ",
				value.GetType(),
				" to ",
				desiredType
			}));
			return null;
		}

		public static global::System.Type GetGenericArgument(this global::System.Type type)
		{
			global::System.Type[] genericArguments = type.GetGenericArguments();
			return (genericArguments == null || genericArguments.Length != 1) ? null : genericArguments[0];
		}

		public static object Create(this global::System.Type type)
		{
			object result;
			try
			{
				result = global::System.Activator.CreateInstance(type);
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.LogError(ex.Message);
				result = null;
			}
			return result;
		}

		public static object Create(this global::System.Type type, int size)
		{
			object result;
			try
			{
				result = global::System.Activator.CreateInstance(type, new object[]
				{
					size
				});
			}
			catch (global::System.Exception)
			{
				result = type.Create();
			}
			return result;
		}

		public static global::TNet.List<global::System.Reflection.FieldInfo> GetSerializableFields(this global::System.Type type)
		{
			global::TNet.List<global::System.Reflection.FieldInfo> list;
			if (!global::TNet.Serialization.mFieldDict.TryGetValue(type, out list))
			{
				list = new global::TNet.List<global::System.Reflection.FieldInfo>();
				global::System.Reflection.FieldInfo[] fields = type.GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				bool flag = type.IsDefined(typeof(global::System.SerializableAttribute), true);
				int i = 0;
				int num = fields.Length;
				while (i < num)
				{
					global::System.Reflection.FieldInfo fieldInfo = fields[i];
					if ((fieldInfo.Attributes & global::System.Reflection.FieldAttributes.Static) == global::System.Reflection.FieldAttributes.PrivateScope)
					{
						if (!fieldInfo.IsDefined(typeof(global::UnityEngine.SerializeField), true))
						{
							if (!flag)
							{
								goto IL_B1;
							}
							if ((fieldInfo.Attributes & global::System.Reflection.FieldAttributes.Public) == global::System.Reflection.FieldAttributes.PrivateScope)
							{
								goto IL_B1;
							}
						}
						if (!fieldInfo.IsDefined(typeof(global::System.NonSerializedAttribute), true))
						{
							list.Add(fieldInfo);
						}
					}
					IL_B1:
					i++;
				}
				global::TNet.Serialization.mFieldDict[type] = list;
			}
			return list;
		}

		public static global::System.Reflection.FieldInfo GetSerializableField(this global::System.Type type, string name)
		{
			global::TNet.List<global::System.Reflection.FieldInfo> serializableFields = type.GetSerializableFields();
			int i = 0;
			int size = serializableFields.size;
			while (i < size)
			{
				global::System.Reflection.FieldInfo fieldInfo = serializableFields[i];
				if (fieldInfo.Name == name)
				{
					return fieldInfo;
				}
				i++;
			}
			return null;
		}

		public static bool SetSerializableField(this object obj, string name, object value)
		{
			if (obj == null)
			{
				return false;
			}
			global::System.Reflection.FieldInfo serializableField = obj.GetType().GetSerializableField(name);
			if (serializableField == null)
			{
				return false;
			}
			try
			{
				serializableField.SetValue(obj, global::TNet.Serialization.ConvertValue(value, serializableField.FieldType));
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.LogError(ex.Message);
				return false;
			}
			return true;
		}

		public static int Deserialize(this object obj, string path)
		{
			global::System.IO.FileStream fileStream = global::System.IO.File.Open(path, global::System.IO.FileMode.Create);
			if (fileStream == null)
			{
				return 0;
			}
			global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(fileStream);
			binaryWriter.WriteObject(obj);
			int result = (int)fileStream.Position;
			binaryWriter.Close();
			return result;
		}

		public static void WriteInt(this global::System.IO.BinaryWriter bw, int val)
		{
			if (val < 255)
			{
				bw.Write((byte)val);
			}
			else
			{
				bw.Write(byte.MaxValue);
				bw.Write(val);
			}
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::UnityEngine.Vector2 v)
		{
			writer.Write(v.x);
			writer.Write(v.y);
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::UnityEngine.Vector3 v)
		{
			writer.Write(v.x);
			writer.Write(v.y);
			writer.Write(v.z);
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::UnityEngine.Vector4 v)
		{
			writer.Write(v.x);
			writer.Write(v.y);
			writer.Write(v.z);
			writer.Write(v.w);
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::UnityEngine.Quaternion q)
		{
			writer.Write(q.x);
			writer.Write(q.y);
			writer.Write(q.z);
			writer.Write(q.w);
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::UnityEngine.Color32 c)
		{
			writer.Write(c.r);
			writer.Write(c.g);
			writer.Write(c.b);
			writer.Write(c.a);
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::UnityEngine.Color c)
		{
			writer.Write(c.r);
			writer.Write(c.g);
			writer.Write(c.b);
			writer.Write(c.a);
		}

		public static void Write(this global::System.IO.BinaryWriter writer, global::TNet.DataNode node)
		{
			writer.Write(node.name);
			writer.WriteObject(node.value);
			writer.WriteInt(node.children.size);
			int i = 0;
			int size = node.children.size;
			while (i < size)
			{
				writer.Write(node.children[i]);
				i++;
			}
		}

		private static int GetPrefix(global::System.Type type)
		{
			if (type == typeof(bool))
			{
				return 1;
			}
			if (type == typeof(byte))
			{
				return 2;
			}
			if (type == typeof(ushort))
			{
				return 3;
			}
			if (type == typeof(int))
			{
				return 4;
			}
			if (type == typeof(uint))
			{
				return 5;
			}
			if (type == typeof(float))
			{
				return 6;
			}
			if (type == typeof(string))
			{
				return 7;
			}
			if (type == typeof(global::UnityEngine.Vector2))
			{
				return 8;
			}
			if (type == typeof(global::UnityEngine.Vector3))
			{
				return 9;
			}
			if (type == typeof(global::UnityEngine.Vector4))
			{
				return 10;
			}
			if (type == typeof(global::UnityEngine.Quaternion))
			{
				return 11;
			}
			if (type == typeof(global::UnityEngine.Color32))
			{
				return 12;
			}
			if (type == typeof(global::UnityEngine.Color))
			{
				return 13;
			}
			if (type == typeof(global::TNet.DataNode))
			{
				return 14;
			}
			if (type == typeof(double))
			{
				return 15;
			}
			if (type == typeof(short))
			{
				return 16;
			}
			if (type == typeof(bool[]))
			{
				return 101;
			}
			if (type == typeof(byte[]))
			{
				return 102;
			}
			if (type == typeof(ushort[]))
			{
				return 103;
			}
			if (type == typeof(int[]))
			{
				return 104;
			}
			if (type == typeof(uint[]))
			{
				return 105;
			}
			if (type == typeof(float[]))
			{
				return 106;
			}
			if (type == typeof(string[]))
			{
				return 107;
			}
			if (type == typeof(global::UnityEngine.Vector2[]))
			{
				return 108;
			}
			if (type == typeof(global::UnityEngine.Vector3[]))
			{
				return 109;
			}
			if (type == typeof(global::UnityEngine.Vector4[]))
			{
				return 110;
			}
			if (type == typeof(global::UnityEngine.Quaternion[]))
			{
				return 111;
			}
			if (type == typeof(global::UnityEngine.Color32[]))
			{
				return 112;
			}
			if (type == typeof(global::UnityEngine.Color[]))
			{
				return 113;
			}
			if (type == typeof(double[]))
			{
				return 115;
			}
			if (type == typeof(short[]))
			{
				return 116;
			}
			return 254;
		}

		private static global::System.Type GetType(int prefix)
		{
			switch (prefix)
			{
			case 1:
				return typeof(bool);
			case 2:
				return typeof(byte);
			case 3:
				return typeof(ushort);
			case 4:
				return typeof(int);
			case 5:
				return typeof(uint);
			case 6:
				return typeof(float);
			case 7:
				return typeof(string);
			case 8:
				return typeof(global::UnityEngine.Vector2);
			case 9:
				return typeof(global::UnityEngine.Vector3);
			case 10:
				return typeof(global::UnityEngine.Vector4);
			case 11:
				return typeof(global::UnityEngine.Quaternion);
			case 12:
				return typeof(global::UnityEngine.Color32);
			case 13:
				return typeof(global::UnityEngine.Color);
			case 14:
				return typeof(global::TNet.DataNode);
			case 15:
				return typeof(double);
			case 16:
				return typeof(short);
			default:
				switch (prefix)
				{
				case 101:
					return typeof(bool[]);
				case 102:
					return typeof(byte[]);
				case 103:
					return typeof(ushort[]);
				case 104:
					return typeof(int[]);
				case 105:
					return typeof(uint[]);
				case 106:
					return typeof(float[]);
				case 107:
					return typeof(string[]);
				case 108:
					return typeof(global::UnityEngine.Vector2[]);
				case 109:
					return typeof(global::UnityEngine.Vector3[]);
				case 110:
					return typeof(global::UnityEngine.Vector4[]);
				case 111:
					return typeof(global::UnityEngine.Quaternion[]);
				case 112:
					return typeof(global::UnityEngine.Color32[]);
				case 113:
					return typeof(global::UnityEngine.Color[]);
				case 115:
					return typeof(double[]);
				case 116:
					return typeof(short[]);
				}
				return null;
			}
		}

		public static void Write(this global::System.IO.BinaryWriter bw, global::System.Type type)
		{
			int prefix = global::TNet.Serialization.GetPrefix(type);
			bw.Write((byte)prefix);
			if (prefix > 250)
			{
				bw.Write(global::TNet.Serialization.TypeToName(type));
			}
		}

		public static void Write(this global::System.IO.BinaryWriter bw, int prefix, global::System.Type type)
		{
			bw.Write((byte)prefix);
			if (prefix > 250)
			{
				bw.Write(global::TNet.Serialization.TypeToName(type));
			}
		}

		public static void WriteObject(this global::System.IO.BinaryWriter bw, object obj)
		{
			bw.WriteObject(obj, 255, false, true);
		}

		public static void WriteObject(this global::System.IO.BinaryWriter bw, object obj, bool useReflection)
		{
			bw.WriteObject(obj, 255, false, useReflection);
		}

		private static void WriteObject(this global::System.IO.BinaryWriter bw, object obj, int prefix, bool typeIsKnown, bool useReflection)
		{
			if (obj == null)
			{
				bw.Write(0);
				return;
			}
			if (obj is global::TNet.IBinarySerializable)
			{
				if (!typeIsKnown)
				{
					bw.Write(253, obj.GetType());
				}
				(obj as global::TNet.IBinarySerializable).Serialize(bw);
				return;
			}
			global::System.Type type;
			if (!typeIsKnown)
			{
				type = obj.GetType();
				prefix = global::TNet.Serialization.GetPrefix(type);
			}
			else
			{
				type = global::TNet.Serialization.GetType(prefix);
			}
			if (prefix > 250)
			{
				if (obj is global::TNet.TList)
				{
					if (useReflection)
					{
						global::System.Type genericArgument = type.GetGenericArgument();
						if (genericArgument != null)
						{
							global::TNet.TList tlist = obj as global::TNet.TList;
							int prefix2 = global::TNet.Serialization.GetPrefix(genericArgument);
							bool flag = true;
							int i = 0;
							int count = tlist.Count;
							while (i < count)
							{
								object obj2 = tlist.Get(i);
								if (obj2 != null && genericArgument != obj2.GetType())
								{
									flag = false;
									prefix2 = 255;
									break;
								}
								i++;
							}
							if (!typeIsKnown)
							{
								bw.Write(98);
							}
							bw.Write(genericArgument);
							bw.Write((!flag) ? 0 : 1);
							bw.WriteInt(tlist.Count);
							int j = 0;
							int count2 = tlist.Count;
							while (j < count2)
							{
								bw.WriteObject(tlist.Get(j), prefix2, flag, useReflection);
								j++;
							}
							return;
						}
					}
					if (!typeIsKnown)
					{
						bw.Write(byte.MaxValue);
					}
					global::TNet.Serialization.formatter.Serialize(bw.BaseStream, obj);
					return;
				}
				if (obj is global::System.Collections.IList)
				{
					if (useReflection)
					{
						global::System.Type type2 = type.GetGenericArgument();
						bool flag2 = false;
						if (type2 == null)
						{
							type2 = type.GetElementType();
							flag2 = (type != null);
						}
						if (flag2 || type2 != null)
						{
							int prefix3 = global::TNet.Serialization.GetPrefix(type2);
							global::System.Collections.IList list = obj as global::System.Collections.IList;
							bool flag3 = true;
							foreach (object obj3 in list)
							{
								if (obj3 != null && type2 != obj3.GetType())
								{
									flag3 = false;
									prefix3 = 255;
									break;
								}
							}
							if (!typeIsKnown)
							{
								bw.Write((!flag2) ? 99 : 100);
							}
							bw.Write(type);
							bw.Write((!flag3) ? 0 : 1);
							bw.WriteInt(list.Count);
							foreach (object obj4 in list)
							{
								bw.WriteObject(obj4, prefix3, flag3, useReflection);
							}
							return;
						}
					}
					if (!typeIsKnown)
					{
						bw.Write(byte.MaxValue);
					}
					global::TNet.Serialization.formatter.Serialize(bw.BaseStream, obj);
					return;
				}
			}
			if (!typeIsKnown)
			{
				bw.Write(prefix, type);
			}
			int num = prefix;
			switch (num)
			{
			case 1:
				bw.Write((bool)obj);
				break;
			case 2:
				bw.Write((byte)obj);
				break;
			case 3:
				bw.Write((ushort)obj);
				break;
			case 4:
				bw.Write((int)obj);
				break;
			case 5:
				bw.Write((uint)obj);
				break;
			case 6:
				bw.Write((float)obj);
				break;
			case 7:
				bw.Write((string)obj);
				break;
			case 8:
				bw.Write((global::UnityEngine.Vector2)obj);
				break;
			case 9:
				bw.Write((global::UnityEngine.Vector3)obj);
				break;
			case 10:
				bw.Write((global::UnityEngine.Vector4)obj);
				break;
			case 11:
				bw.Write((global::UnityEngine.Quaternion)obj);
				break;
			case 12:
				bw.Write((global::UnityEngine.Color32)obj);
				break;
			case 13:
				bw.Write((global::UnityEngine.Color)obj);
				break;
			case 14:
				bw.Write((global::TNet.DataNode)obj);
				break;
			case 15:
				bw.Write((double)obj);
				break;
			case 16:
				bw.Write((short)obj);
				break;
			default:
				switch (num)
				{
				case 101:
				{
					bool[] array = (bool[])obj;
					bw.WriteInt(array.Length);
					int k = 0;
					int num2 = array.Length;
					while (k < num2)
					{
						bw.Write(array[k]);
						k++;
					}
					break;
				}
				case 102:
				{
					byte[] array2 = (byte[])obj;
					bw.WriteInt(array2.Length);
					bw.Write(array2);
					break;
				}
				case 103:
				{
					ushort[] array3 = (ushort[])obj;
					bw.WriteInt(array3.Length);
					int l = 0;
					int num3 = array3.Length;
					while (l < num3)
					{
						bw.Write(array3[l]);
						l++;
					}
					break;
				}
				case 104:
				{
					int[] array4 = (int[])obj;
					bw.WriteInt(array4.Length);
					int m = 0;
					int num4 = array4.Length;
					while (m < num4)
					{
						bw.Write(array4[m]);
						m++;
					}
					break;
				}
				case 105:
				{
					uint[] array5 = (uint[])obj;
					bw.WriteInt(array5.Length);
					int n = 0;
					int num5 = array5.Length;
					while (n < num5)
					{
						bw.Write(array5[n]);
						n++;
					}
					break;
				}
				case 106:
				{
					float[] array6 = (float[])obj;
					bw.WriteInt(array6.Length);
					int num6 = 0;
					int num7 = array6.Length;
					while (num6 < num7)
					{
						bw.Write(array6[num6]);
						num6++;
					}
					break;
				}
				case 107:
				{
					string[] array7 = (string[])obj;
					bw.WriteInt(array7.Length);
					int num8 = 0;
					int num9 = array7.Length;
					while (num8 < num9)
					{
						bw.Write(array7[num8]);
						num8++;
					}
					break;
				}
				case 108:
				{
					global::UnityEngine.Vector2[] array8 = (global::UnityEngine.Vector2[])obj;
					bw.WriteInt(array8.Length);
					int num10 = 0;
					int num11 = array8.Length;
					while (num10 < num11)
					{
						bw.Write(array8[num10]);
						num10++;
					}
					break;
				}
				case 109:
				{
					global::UnityEngine.Vector3[] array9 = (global::UnityEngine.Vector3[])obj;
					bw.WriteInt(array9.Length);
					int num12 = 0;
					int num13 = array9.Length;
					while (num12 < num13)
					{
						bw.Write(array9[num12]);
						num12++;
					}
					break;
				}
				case 110:
				{
					global::UnityEngine.Vector4[] array10 = (global::UnityEngine.Vector4[])obj;
					bw.WriteInt(array10.Length);
					int num14 = 0;
					int num15 = array10.Length;
					while (num14 < num15)
					{
						bw.Write(array10[num14]);
						num14++;
					}
					break;
				}
				case 111:
				{
					global::UnityEngine.Quaternion[] array11 = (global::UnityEngine.Quaternion[])obj;
					bw.WriteInt(array11.Length);
					int num16 = 0;
					int num17 = array11.Length;
					while (num16 < num17)
					{
						bw.Write(array11[num16]);
						num16++;
					}
					break;
				}
				case 112:
				{
					global::UnityEngine.Color32[] array12 = (global::UnityEngine.Color32[])obj;
					bw.WriteInt(array12.Length);
					int num18 = 0;
					int num19 = array12.Length;
					while (num18 < num19)
					{
						bw.Write(array12[num18]);
						num18++;
					}
					break;
				}
				case 113:
				{
					global::UnityEngine.Color[] array13 = (global::UnityEngine.Color[])obj;
					bw.WriteInt(array13.Length);
					int num20 = 0;
					int num21 = array13.Length;
					while (num20 < num21)
					{
						bw.Write(array13[num20]);
						num20++;
					}
					break;
				}
				default:
					if (num != 254)
					{
						if (num != 255)
						{
							global::UnityEngine.Debug.LogError("Prefix " + prefix + " is not supported");
						}
						else
						{
							global::TNet.Serialization.formatter.Serialize(bw.BaseStream, obj);
						}
					}
					else
					{
						global::TNet.Serialization.FilterFields(obj);
						bw.WriteInt(global::TNet.Serialization.mFieldNames.size);
						int num22 = 0;
						int size = global::TNet.Serialization.mFieldNames.size;
						while (num22 < size)
						{
							bw.Write(global::TNet.Serialization.mFieldNames[num22]);
							bw.WriteObject(global::TNet.Serialization.mFieldValues[num22]);
							num22++;
						}
					}
					break;
				case 115:
				{
					double[] array14 = (double[])obj;
					bw.WriteInt(array14.Length);
					int num23 = 0;
					int num24 = array14.Length;
					while (num23 < num24)
					{
						bw.Write(array14[num23]);
						num23++;
					}
					break;
				}
				case 116:
				{
					short[] array15 = (short[])obj;
					bw.WriteInt(array15.Length);
					int num25 = 0;
					int num26 = array15.Length;
					while (num25 < num26)
					{
						bw.Write(array15[num25]);
						num25++;
					}
					break;
				}
				}
				break;
			}
		}

		private static void FilterFields(object obj)
		{
			global::System.Type type = obj.GetType();
			global::TNet.List<global::System.Reflection.FieldInfo> serializableFields = type.GetSerializableFields();
			global::TNet.Serialization.mFieldNames.Clear();
			global::TNet.Serialization.mFieldValues.Clear();
			for (int i = 0; i < serializableFields.size; i++)
			{
				global::System.Reflection.FieldInfo fieldInfo = serializableFields[i];
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					global::TNet.Serialization.mFieldNames.Add(fieldInfo.Name);
					global::TNet.Serialization.mFieldValues.Add(value);
				}
			}
		}

		public static bool Implements(this global::System.Type t, global::System.Type interfaceType)
		{
			return interfaceType != null && interfaceType.IsAssignableFrom(t);
		}

		public static int ReadInt(this global::System.IO.BinaryReader reader)
		{
			int num = (int)reader.ReadByte();
			if (num == 255)
			{
				num = reader.ReadInt32();
			}
			return num;
		}

		public static global::UnityEngine.Vector2 ReadVector2(this global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Vector2(reader.ReadSingle(), reader.ReadSingle());
		}

		public static global::UnityEngine.Vector3 ReadVector3(this global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static global::UnityEngine.Vector4 ReadVector4(this global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static global::UnityEngine.Quaternion ReadQuaternion(this global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static global::UnityEngine.Color32 ReadColor32(this global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
		}

		public static global::UnityEngine.Color ReadColor(this global::System.IO.BinaryReader reader)
		{
			return new global::UnityEngine.Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static global::TNet.DataNode ReadDataNode(this global::System.IO.BinaryReader reader)
		{
			global::TNet.DataNode dataNode = new global::TNet.DataNode();
			dataNode.name = reader.ReadString();
			dataNode.value = reader.ReadObject();
			int num = reader.ReadInt();
			for (int i = 0; i < num; i++)
			{
				dataNode.children.Add(reader.ReadDataNode());
			}
			return dataNode;
		}

		public static global::System.Type ReadType(this global::System.IO.BinaryReader reader)
		{
			int num = (int)reader.ReadByte();
			return (num <= 250) ? global::TNet.Serialization.GetType(num) : global::TNet.Serialization.NameToType(reader.ReadString());
		}

		public static global::System.Type ReadType(this global::System.IO.BinaryReader reader, out int prefix)
		{
			prefix = (int)reader.ReadByte();
			return (prefix <= 250) ? global::TNet.Serialization.GetType(prefix) : global::TNet.Serialization.NameToType(reader.ReadString());
		}

		public static T ReadObject<T>(this global::System.IO.BinaryReader reader)
		{
			object obj = reader.ReadObject();
			if (obj == null)
			{
				return default(T);
			}
			return (T)((object)obj);
		}

		public static object ReadObject(this global::System.IO.BinaryReader reader)
		{
			return reader.ReadObject(null, 0, null, false);
		}

		public static object ReadObject(this global::System.IO.BinaryReader reader, object obj)
		{
			return reader.ReadObject(obj, 0, null, false);
		}

		private static object ReadObject(this global::System.IO.BinaryReader reader, object obj, int prefix, global::System.Type type, bool typeIsKnown)
		{
			if (!typeIsKnown)
			{
				type = reader.ReadType(out prefix);
			}
			if (type.Implements(typeof(global::TNet.IBinarySerializable)))
			{
				prefix = 253;
			}
			int num = prefix;
			switch (num)
			{
			case 98:
			{
				type = reader.ReadType(out prefix);
				bool typeIsKnown2 = reader.ReadByte() == 1;
				int num2 = reader.ReadInt();
				global::TNet.TList tlist;
				if (obj != null)
				{
					tlist = (global::TNet.TList)obj;
				}
				else
				{
					global::System.Type type2 = typeof(global::TNet.List<>).MakeGenericType(new global::System.Type[]
					{
						type
					});
					tlist = (global::TNet.TList)global::System.Activator.CreateInstance(type2);
				}
				for (int i = 0; i < num2; i++)
				{
					object obj2 = reader.ReadObject(null, prefix, type, typeIsKnown2);
					if (tlist != null)
					{
						tlist.Add(obj2);
					}
				}
				return tlist;
			}
			case 99:
			{
				type = reader.ReadType(out prefix);
				bool typeIsKnown3 = reader.ReadByte() == 1;
				int num3 = reader.ReadInt();
				global::System.Collections.IList list;
				if (obj != null)
				{
					list = (global::System.Collections.IList)obj;
				}
				else
				{
					global::System.Type type3 = typeof(global::System.Collections.Generic.List<>).MakeGenericType(new global::System.Type[]
					{
						type
					});
					list = (global::System.Collections.IList)global::System.Activator.CreateInstance(type3);
				}
				for (int j = 0; j < num3; j++)
				{
					object value = reader.ReadObject(null, prefix, type, typeIsKnown3);
					if (list != null)
					{
						list.Add(value);
					}
				}
				return list;
			}
			case 100:
			{
				type = reader.ReadType(out prefix);
				bool typeIsKnown4 = reader.ReadByte() == 1;
				int num4 = reader.ReadInt();
				global::System.Collections.IList list2 = (global::System.Collections.IList)type.Create(num4);
				if (list2 != null)
				{
					type = type.GetElementType();
					prefix = global::TNet.Serialization.GetPrefix(type);
					for (int k = 0; k < num4; k++)
					{
						list2[k] = reader.ReadObject(null, prefix, type, typeIsKnown4);
					}
				}
				else
				{
					global::UnityEngine.Debug.LogError("Failed to create a " + type);
				}
				return list2;
			}
			case 101:
			{
				int num5 = reader.ReadInt();
				bool[] array = new bool[num5];
				for (int l = 0; l < num5; l++)
				{
					array[l] = reader.ReadBoolean();
				}
				return array;
			}
			case 102:
			{
				int count = reader.ReadInt();
				return reader.ReadBytes(count);
			}
			case 103:
			{
				int num6 = reader.ReadInt();
				ushort[] array2 = new ushort[num6];
				for (int m = 0; m < num6; m++)
				{
					array2[m] = reader.ReadUInt16();
				}
				return array2;
			}
			case 104:
			{
				int num7 = reader.ReadInt();
				int[] array3 = new int[num7];
				for (int n = 0; n < num7; n++)
				{
					array3[n] = reader.ReadInt32();
				}
				return array3;
			}
			case 105:
			{
				int num8 = reader.ReadInt();
				uint[] array4 = new uint[num8];
				for (int num9 = 0; num9 < num8; num9++)
				{
					array4[num9] = reader.ReadUInt32();
				}
				return array4;
			}
			case 106:
			{
				int num10 = reader.ReadInt();
				float[] array5 = new float[num10];
				for (int num11 = 0; num11 < num10; num11++)
				{
					array5[num11] = reader.ReadSingle();
				}
				return array5;
			}
			case 107:
			{
				int num12 = reader.ReadInt();
				string[] array6 = new string[num12];
				for (int num13 = 0; num13 < num12; num13++)
				{
					array6[num13] = reader.ReadString();
				}
				return array6;
			}
			case 108:
			{
				int num14 = reader.ReadInt();
				global::UnityEngine.Vector2[] array7 = new global::UnityEngine.Vector2[num14];
				for (int num15 = 0; num15 < num14; num15++)
				{
					array7[num15] = reader.ReadVector2();
				}
				return array7;
			}
			case 109:
			{
				int num16 = reader.ReadInt();
				global::UnityEngine.Vector3[] array8 = new global::UnityEngine.Vector3[num16];
				for (int num17 = 0; num17 < num16; num17++)
				{
					array8[num17] = reader.ReadVector3();
				}
				return array8;
			}
			case 110:
			{
				int num18 = reader.ReadInt();
				global::UnityEngine.Vector4[] array9 = new global::UnityEngine.Vector4[num18];
				for (int num19 = 0; num19 < num18; num19++)
				{
					array9[num19] = reader.ReadVector4();
				}
				return array9;
			}
			case 111:
			{
				int num20 = reader.ReadInt();
				global::UnityEngine.Quaternion[] array10 = new global::UnityEngine.Quaternion[num20];
				for (int num21 = 0; num21 < num20; num21++)
				{
					array10[num21] = reader.ReadQuaternion();
				}
				return array10;
			}
			case 112:
			{
				int num22 = reader.ReadInt();
				global::UnityEngine.Color32[] array11 = new global::UnityEngine.Color32[num22];
				for (int num23 = 0; num23 < num22; num23++)
				{
					array11[num23] = reader.ReadColor32();
				}
				return array11;
			}
			case 113:
			{
				int num24 = reader.ReadInt();
				global::UnityEngine.Color[] array12 = new global::UnityEngine.Color[num24];
				for (int num25 = 0; num25 < num24; num25++)
				{
					array12[num25] = reader.ReadColor();
				}
				return array12;
			}
			default:
				switch (num)
				{
				case 0:
					return null;
				case 1:
					return reader.ReadBoolean();
				case 2:
					return reader.ReadByte();
				case 3:
					return reader.ReadUInt16();
				case 4:
					return reader.ReadInt32();
				case 5:
					return reader.ReadUInt32();
				case 6:
					return reader.ReadSingle();
				case 7:
					return reader.ReadString();
				case 8:
					return reader.ReadVector2();
				case 9:
					return reader.ReadVector3();
				case 10:
					return reader.ReadVector4();
				case 11:
					return reader.ReadQuaternion();
				case 12:
					return reader.ReadColor32();
				case 13:
					return reader.ReadColor();
				case 14:
					return reader.ReadDataNode();
				case 15:
					return reader.ReadDouble();
				case 16:
					return reader.ReadInt16();
				default:
					switch (num)
					{
					case 253:
					{
						global::TNet.IBinarySerializable binarySerializable2;
						if (obj != null)
						{
							global::TNet.IBinarySerializable binarySerializable = (global::TNet.IBinarySerializable)obj;
							binarySerializable2 = binarySerializable;
						}
						else
						{
							binarySerializable2 = (global::TNet.IBinarySerializable)type.Create();
						}
						global::TNet.IBinarySerializable binarySerializable3 = binarySerializable2;
						if (binarySerializable3 != null)
						{
							binarySerializable3.Deserialize(reader);
						}
						return binarySerializable3;
					}
					case 254:
						if (obj == null)
						{
							obj = type.Create();
							if (obj == null)
							{
								global::UnityEngine.Debug.LogError("Unable to create an instance of " + type);
							}
						}
						if (obj != null)
						{
							int num26 = reader.ReadInt();
							for (int num27 = 0; num27 < num26; num27++)
							{
								string text = reader.ReadString();
								if (string.IsNullOrEmpty(text))
								{
									global::UnityEngine.Debug.LogError("Null field specified when serializing " + type);
								}
								else
								{
									global::System.Reflection.FieldInfo field = type.GetField(text, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
									object value2 = reader.ReadObject();
									if (field != null)
									{
										field.SetValue(obj, global::TNet.Serialization.ConvertValue(value2, field.FieldType));
									}
									else
									{
										global::UnityEngine.Debug.LogError(string.Concat(new object[]
										{
											"Unable to set field ",
											type,
											".",
											text
										}));
									}
								}
							}
						}
						return obj;
					case 255:
						return global::TNet.Serialization.formatter.Deserialize(reader.BaseStream);
					default:
						global::UnityEngine.Debug.LogError("Unknown prefix: " + prefix);
						return null;
					}
					break;
				}
				break;
			case 115:
			{
				int num28 = reader.ReadInt();
				double[] array13 = new double[num28];
				for (int num29 = 0; num29 < num28; num29++)
				{
					array13[num29] = reader.ReadDouble();
				}
				return array13;
			}
			case 116:
			{
				int num30 = reader.ReadInt();
				short[] array14 = new short[num30];
				for (int num31 = 0; num31 < num30; num31++)
				{
					array14[num31] = reader.ReadInt16();
				}
				return array14;
			}
			}
		}

		public static global::System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new global::System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

		private static global::System.Collections.Generic.Dictionary<string, global::System.Type> mNameToType = new global::System.Collections.Generic.Dictionary<string, global::System.Type>();

		private static global::System.Collections.Generic.Dictionary<global::System.Type, string> mTypeToName = new global::System.Collections.Generic.Dictionary<global::System.Type, string>();

		private static global::System.Collections.Generic.Dictionary<global::System.Type, global::TNet.List<global::System.Reflection.FieldInfo>> mFieldDict = new global::System.Collections.Generic.Dictionary<global::System.Type, global::TNet.List<global::System.Reflection.FieldInfo>>();

		private static global::TNet.List<string> mFieldNames = new global::TNet.List<string>();

		private static global::TNet.List<object> mFieldValues = new global::TNet.List<object>();
	}
}
