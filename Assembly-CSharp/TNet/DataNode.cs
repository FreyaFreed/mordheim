using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TNet
{
	[global::System.Serializable]
	public class DataNode
	{
		public DataNode()
		{
		}

		public DataNode(string name)
		{
			this.name = name;
		}

		public DataNode(string name, object value)
		{
			this.name = name;
			this.value = value;
		}

		public object value
		{
			get
			{
				if (!this.mResolved && !this.ResolveValue(null))
				{
					this.children.Clear();
				}
				return this.mValue;
			}
			set
			{
				this.mValue = value;
				this.mResolved = true;
			}
		}

		public bool isSerializable
		{
			get
			{
				return this.value != null || this.children.size > 0;
			}
		}

		public global::System.Type type
		{
			get
			{
				return (this.value == null) ? typeof(void) : this.mValue.GetType();
			}
		}

		public void Clear()
		{
			this.value = null;
			this.children.Clear();
		}

		public object Get(global::System.Type type)
		{
			return global::TNet.Serialization.ConvertValue(this.value, type);
		}

		public T Get<T>()
		{
			if (this.value is T)
			{
				return (T)((object)this.mValue);
			}
			object obj = this.Get(typeof(T));
			return (this.mValue == null) ? default(T) : ((T)((object)obj));
		}

		public global::TNet.DataNode AddChild()
		{
			global::TNet.DataNode dataNode = new global::TNet.DataNode();
			this.children.Add(dataNode);
			return dataNode;
		}

		public global::TNet.DataNode AddChild(string name)
		{
			global::TNet.DataNode dataNode = this.AddChild();
			dataNode.name = name;
			return dataNode;
		}

		public global::TNet.DataNode AddChild(string name, object value)
		{
			global::TNet.DataNode dataNode = this.AddChild();
			dataNode.name = name;
			dataNode.value = ((!(value is global::System.Enum)) ? value : value.ToString());
			return dataNode;
		}

		public global::TNet.DataNode SetChild(string name, object value)
		{
			global::TNet.DataNode dataNode = this.GetChild(name);
			if (dataNode == null)
			{
				dataNode = this.AddChild();
			}
			dataNode.name = name;
			dataNode.value = ((!(value is global::System.Enum)) ? value : value.ToString());
			return dataNode;
		}

		public global::TNet.DataNode GetChild(string name)
		{
			for (int i = 0; i < this.children.size; i++)
			{
				if (this.children[i].name == name)
				{
					return this.children[i];
				}
			}
			return null;
		}

		public global::TNet.DataNode GetChild(string name, bool createIfMissing)
		{
			for (int i = 0; i < this.children.size; i++)
			{
				if (this.children[i].name == name)
				{
					return this.children[i];
				}
			}
			if (createIfMissing)
			{
				return this.AddChild(name);
			}
			return null;
		}

		public T GetChild<T>(string name)
		{
			global::TNet.DataNode child = this.GetChild(name);
			if (child == null)
			{
				return default(T);
			}
			return child.Get<T>();
		}

		public T GetChild<T>(string name, T defaultValue)
		{
			global::TNet.DataNode child = this.GetChild(name);
			if (child == null)
			{
				return defaultValue;
			}
			return child.Get<T>();
		}

		public void RemoveChild(string name)
		{
			for (int i = 0; i < this.children.size; i++)
			{
				if (this.children[i].name == name)
				{
					this.children.RemoveAt(i);
					return;
				}
			}
		}

		public global::TNet.DataNode Clone()
		{
			global::TNet.DataNode dataNode = new global::TNet.DataNode(this.name);
			dataNode.mValue = this.mValue;
			dataNode.mResolved = this.mResolved;
			for (int i = 0; i < this.children.size; i++)
			{
				dataNode.children.Add(this.children[i].Clone());
			}
			return dataNode;
		}

		public void Write(string path)
		{
			this.Write(path, false);
		}

		public static global::TNet.DataNode Read(string path)
		{
			return global::TNet.DataNode.Read(path, false);
		}

		public void Write(string path, bool binary)
		{
			if (binary)
			{
				global::System.IO.FileStream output = global::System.IO.File.Create(path);
				global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(output);
				binaryWriter.WriteObject(this);
				binaryWriter.Close();
			}
			else
			{
				global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(path, false);
				this.Write(streamWriter, 0);
				streamWriter.Close();
			}
		}

		public static global::TNet.DataNode Read(string path, bool binary)
		{
			if (binary)
			{
				global::System.IO.FileStream input = global::System.IO.File.OpenRead(path);
				global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(input);
				global::TNet.DataNode result = binaryReader.ReadObject<global::TNet.DataNode>();
				binaryReader.Close();
				return result;
			}
			global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(path);
			global::TNet.DataNode result2 = global::TNet.DataNode.Read(streamReader);
			streamReader.Close();
			return result2;
		}

		public static global::TNet.DataNode Read(byte[] bytes, bool binary)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(bytes);
			if (binary)
			{
				global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream);
				global::TNet.DataNode result = binaryReader.ReadObject<global::TNet.DataNode>();
				binaryReader.Close();
				return result;
			}
			global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(memoryStream);
			global::TNet.DataNode result2 = global::TNet.DataNode.Read(streamReader);
			streamReader.Close();
			return result2;
		}

		public void Write(global::System.IO.StreamWriter writer)
		{
			this.Write(writer, 0);
		}

		public static global::TNet.DataNode Read(global::System.IO.StreamReader reader)
		{
			string nextLine = global::TNet.DataNode.GetNextLine(reader);
			int num = global::TNet.DataNode.CalculateTabs(nextLine);
			global::TNet.DataNode dataNode = new global::TNet.DataNode();
			dataNode.Read(reader, nextLine, ref num);
			return dataNode;
		}

		public override string ToString()
		{
			if (!this.isSerializable)
			{
				return string.Empty;
			}
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			global::System.IO.StreamWriter writer = new global::System.IO.StreamWriter(memoryStream);
			this.Write(writer, 0);
			memoryStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
			global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			memoryStream.Close();
			return result;
		}

		private void Write(global::System.IO.StreamWriter writer, int tab)
		{
			if (this.isSerializable)
			{
				global::TNet.DataNode.Write(writer, tab, this.name, this.value, true);
				for (int i = 0; i < this.children.size; i++)
				{
					this.children[i].Write(writer, tab + 1);
				}
			}
			if (tab == 0)
			{
				writer.Flush();
			}
		}

		private static void Write(global::System.IO.StreamWriter writer, int tab, string name, object value, bool writeType)
		{
			if (!string.IsNullOrEmpty(name))
			{
				global::TNet.DataNode.WriteTabs(writer, tab);
				writer.Write(global::TNet.DataNode.Escape(name));
				if (value == null)
				{
					writer.Write('\n');
					return;
				}
				global::System.Type type = value.GetType();
				if (type == typeof(string))
				{
					writer.Write(" = \"");
					writer.Write((string)value);
					writer.Write('"');
					writer.Write('\n');
				}
				else if (type == typeof(bool))
				{
					writer.Write(" = ");
					writer.Write((!(bool)value) ? "false" : "true");
					writer.Write('\n');
				}
				else if (type == typeof(int) || type == typeof(float) || type == typeof(uint) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort))
				{
					writer.Write(" = ");
					writer.Write(value.ToString());
					writer.Write('\n');
				}
				else if (type == typeof(global::UnityEngine.Vector2))
				{
					global::UnityEngine.Vector2 vector = (global::UnityEngine.Vector2)value;
					writer.Write(" = (");
					writer.Write(vector.x.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(vector.y.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(")\n");
				}
				else if (type == typeof(global::UnityEngine.Vector3))
				{
					global::UnityEngine.Vector3 vector2 = (global::UnityEngine.Vector3)value;
					writer.Write(" = (");
					writer.Write(vector2.x.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(vector2.y.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(vector2.z.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(")\n");
				}
				else if (type == typeof(global::UnityEngine.Color))
				{
					global::UnityEngine.Color color = (global::UnityEngine.Color)value;
					writer.Write(" = (");
					writer.Write(color.r.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(color.g.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(color.b.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(color.a.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(")\n");
				}
				else if (type == typeof(global::UnityEngine.Color32))
				{
					global::UnityEngine.Color32 color2 = (global::UnityEngine.Color32)value;
					writer.Write(" = 0x");
					if (color2.a == 255)
					{
						writer.Write(((int)color2.r << 16 | (int)color2.g << 8 | (int)color2.b).ToString("X6"));
					}
					else
					{
						writer.Write(((int)color2.r << 24 | (int)color2.g << 16 | (int)color2.b << 8 | (int)color2.a).ToString("X8"));
					}
					writer.Write('\n');
				}
				else if (type == typeof(global::UnityEngine.Vector4))
				{
					global::UnityEngine.Vector4 vector3 = (global::UnityEngine.Vector4)value;
					writer.Write(" = ");
					writer.Write(global::TNet.Serialization.TypeToName(type));
					writer.Write('(');
					writer.Write(vector3.x.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(vector3.y.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(vector3.z.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(vector3.w.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(")\n");
				}
				else if (type == typeof(global::UnityEngine.Quaternion))
				{
					global::UnityEngine.Vector3 eulerAngles = ((global::UnityEngine.Quaternion)value).eulerAngles;
					writer.Write(" = ");
					writer.Write(global::TNet.Serialization.TypeToName(type));
					writer.Write('(');
					writer.Write(eulerAngles.x.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(eulerAngles.y.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(eulerAngles.z.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(")\n");
				}
				else if (type == typeof(global::UnityEngine.Rect))
				{
					global::UnityEngine.Rect rect = (global::UnityEngine.Rect)value;
					writer.Write(" = ");
					writer.Write(global::TNet.Serialization.TypeToName(type));
					writer.Write('(');
					writer.Write(rect.x.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(rect.y.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(rect.width.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(", ");
					writer.Write(rect.height.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
					writer.Write(")\n");
				}
				else if (value is global::TNet.TList)
				{
					global::TNet.TList tlist = value as global::TNet.TList;
					writer.Write(" = ");
					writer.Write(global::TNet.Serialization.TypeToName(type));
					writer.Write('\n');
					if (tlist.Count > 0)
					{
						int i = 0;
						int count = tlist.Count;
						while (i < count)
						{
							global::TNet.DataNode.Write(writer, tab + 1, "Add", tlist.Get(i), false);
							i++;
						}
					}
				}
				else if (value is global::System.Collections.IList)
				{
					global::System.Collections.IList list = value as global::System.Collections.IList;
					writer.Write(" = ");
					writer.Write(global::TNet.Serialization.TypeToName(type));
					writer.Write('\n');
					if (list.Count > 0)
					{
						int j = 0;
						int count2 = list.Count;
						while (j < count2)
						{
							global::TNet.DataNode.Write(writer, tab + 1, "Add", list[j], false);
							j++;
						}
					}
				}
				else if (value is global::TNet.IDataNodeSerializable)
				{
					global::TNet.IDataNodeSerializable dataNodeSerializable = value as global::TNet.IDataNodeSerializable;
					global::TNet.DataNode dataNode = new global::TNet.DataNode();
					dataNodeSerializable.Serialize(dataNode);
					writer.Write(" = ");
					writer.Write(global::TNet.Serialization.TypeToName(type));
					writer.Write('\n');
					for (int k = 0; k < dataNode.children.size; k++)
					{
						global::TNet.DataNode dataNode2 = dataNode.children[k];
						dataNode2.Write(writer, tab + 1);
					}
				}
				else if (value is global::UnityEngine.GameObject)
				{
					global::UnityEngine.Debug.LogError("It's not possible to save game objects.");
					writer.Write('\n');
				}
				else if (value as global::UnityEngine.Component != null)
				{
					global::UnityEngine.Debug.LogError("It's not possible to save components.");
					writer.Write('\n');
				}
				else
				{
					if (writeType)
					{
						writer.Write(" = ");
						writer.Write(global::TNet.Serialization.TypeToName(type));
					}
					writer.Write('\n');
					global::TNet.List<global::System.Reflection.FieldInfo> serializableFields = type.GetSerializableFields();
					if (serializableFields.size > 0)
					{
						for (int l = 0; l < serializableFields.size; l++)
						{
							global::System.Reflection.FieldInfo fieldInfo = serializableFields[l];
							object value2 = fieldInfo.GetValue(value);
							if (value2 != null)
							{
								global::TNet.DataNode.Write(writer, tab + 1, fieldInfo.Name, value2, true);
							}
						}
					}
				}
			}
		}

		private static void WriteTabs(global::System.IO.StreamWriter writer, int count)
		{
			for (int i = 0; i < count; i++)
			{
				writer.Write('\t');
			}
		}

		private string Read(global::System.IO.StreamReader reader, string line, ref int offset)
		{
			if (line != null)
			{
				int num = offset;
				int num2 = line.IndexOf("=", num);
				if (num2 == -1)
				{
					this.name = global::TNet.DataNode.Unescape(line.Substring(offset)).Trim();
					this.value = null;
				}
				else
				{
					this.name = global::TNet.DataNode.Unescape(line.Substring(offset, num2 - offset)).Trim();
					this.mValue = line.Substring(num2 + 1).Trim();
					this.mResolved = false;
				}
				line = global::TNet.DataNode.GetNextLine(reader);
				offset = global::TNet.DataNode.CalculateTabs(line);
				while (line != null)
				{
					if (offset != num + 1)
					{
						break;
					}
					line = this.AddChild().Read(reader, line, ref offset);
				}
			}
			return line;
		}

		private bool ResolveValue(global::System.Type type)
		{
			this.mResolved = true;
			string text = this.mValue as string;
			if (string.IsNullOrEmpty(text))
			{
				return this.SetValue(text, type, null);
			}
			if (text.Length > 2)
			{
				if (text[0] == '"' && text[text.Length - 1] == '"')
				{
					this.mValue = text.Substring(1, text.Length - 2);
					return true;
				}
				if (text[0] == '0' && text[1] == 'x' && text.Length > 7)
				{
					this.mValue = global::TNet.DataNode.ParseColor32(text, 2);
					return true;
				}
				bool flag;
				if (text[0] == '(' && text[text.Length - 1] == ')')
				{
					text = text.Substring(1, text.Length - 2);
					string[] array = text.Split(new char[]
					{
						','
					});
					if (array.Length == 1)
					{
						return this.SetValue(text, typeof(float), null);
					}
					if (array.Length == 2)
					{
						return this.SetValue(text, typeof(global::UnityEngine.Vector2), array);
					}
					if (array.Length == 3)
					{
						return this.SetValue(text, typeof(global::UnityEngine.Vector3), array);
					}
					if (array.Length == 4)
					{
						return this.SetValue(text, typeof(global::UnityEngine.Color), array);
					}
					this.mValue = text;
					return true;
				}
				else if (bool.TryParse(text, out flag))
				{
					this.mValue = flag;
					return true;
				}
			}
			int num = text.IndexOf('(');
			if (num == -1)
			{
				if (text[0] == '-' || (text[0] >= '0' && text[0] <= '9'))
				{
					int num3;
					if (text.IndexOf('.') != -1)
					{
						float num2;
						if (float.TryParse(text, out num2))
						{
							this.mValue = num2;
							return true;
						}
					}
					else if (int.TryParse(text, out num3))
					{
						this.mValue = num3;
						return true;
					}
				}
			}
			else
			{
				int num4 = (text[text.Length - 1] != ')') ? text.LastIndexOf(')', num) : (text.Length - 1);
				if (num4 != -1 && text.Length > 2)
				{
					string text2 = text.Substring(0, num);
					type = global::TNet.Serialization.NameToType(text2);
					text = text.Substring(num + 1, num4 - num - 1);
				}
				else if (type == null)
				{
					type = typeof(string);
					this.mValue = text;
					return true;
				}
			}
			if (type == null)
			{
				type = global::TNet.Serialization.NameToType(text);
			}
			return this.SetValue(text, type, null);
		}

		private bool SetValue(string text, global::System.Type type, string[] parts)
		{
			if (type == null || type == typeof(void))
			{
				this.mValue = null;
			}
			else if (type == typeof(string))
			{
				this.mValue = text;
			}
			else if (type == typeof(bool))
			{
				bool flag = false;
				if (bool.TryParse(text, out flag))
				{
					this.mValue = flag;
				}
			}
			else if (type == typeof(byte))
			{
				byte b;
				if (byte.TryParse(text, out b))
				{
					this.mValue = b;
				}
			}
			else if (type == typeof(short))
			{
				short num;
				if (short.TryParse(text, out num))
				{
					this.mValue = num;
				}
			}
			else if (type == typeof(ushort))
			{
				ushort num2;
				if (ushort.TryParse(text, out num2))
				{
					this.mValue = num2;
				}
			}
			else if (type == typeof(int))
			{
				int num3;
				if (int.TryParse(text, out num3))
				{
					this.mValue = num3;
				}
			}
			else if (type == typeof(uint))
			{
				uint num4;
				if (uint.TryParse(text, out num4))
				{
					this.mValue = num4;
				}
			}
			else if (type == typeof(float))
			{
				float num5;
				if (float.TryParse(text, global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out num5))
				{
					this.mValue = num5;
				}
			}
			else if (type == typeof(double))
			{
				double num6;
				if (double.TryParse(text, global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out num6))
				{
					this.mValue = num6;
				}
			}
			else if (type == typeof(global::UnityEngine.Vector2))
			{
				if (parts == null)
				{
					parts = text.Split(new char[]
					{
						','
					});
				}
				global::UnityEngine.Vector2 vector;
				if (parts.Length == 2 && float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector.x) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector.y))
				{
					this.mValue = vector;
				}
			}
			else if (type == typeof(global::UnityEngine.Vector3))
			{
				if (parts == null)
				{
					parts = text.Split(new char[]
					{
						','
					});
				}
				global::UnityEngine.Vector3 vector2;
				if (parts.Length == 3 && float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector2.x) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector2.y) && float.TryParse(parts[2], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector2.z))
				{
					this.mValue = vector2;
				}
			}
			else if (type == typeof(global::UnityEngine.Vector4))
			{
				if (parts == null)
				{
					parts = text.Split(new char[]
					{
						','
					});
				}
				global::UnityEngine.Vector4 vector3;
				if (parts.Length == 4 && float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector3.x) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector3.y) && float.TryParse(parts[2], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector3.z) && float.TryParse(parts[3], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector3.w))
				{
					this.mValue = vector3;
				}
			}
			else if (type == typeof(global::UnityEngine.Quaternion))
			{
				if (parts == null)
				{
					parts = text.Split(new char[]
					{
						','
					});
				}
				global::UnityEngine.Quaternion quaternion;
				if (parts.Length == 3)
				{
					global::UnityEngine.Vector3 euler;
					if (float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out euler.x) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out euler.y) && float.TryParse(parts[2], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out euler.z))
					{
						this.mValue = global::UnityEngine.Quaternion.Euler(euler);
					}
				}
				else if (parts.Length == 4 && float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out quaternion.x) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out quaternion.y) && float.TryParse(parts[2], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out quaternion.z) && float.TryParse(parts[3], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out quaternion.w))
				{
					this.mValue = quaternion;
				}
			}
			else if (type == typeof(global::UnityEngine.Color))
			{
				if (parts == null)
				{
					parts = text.Split(new char[]
					{
						','
					});
				}
				global::UnityEngine.Color color;
				if (parts.Length == 4 && float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out color.r) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out color.g) && float.TryParse(parts[2], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out color.b) && float.TryParse(parts[3], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out color.a))
				{
					this.mValue = color;
				}
			}
			else if (type == typeof(global::UnityEngine.Rect))
			{
				if (parts == null)
				{
					parts = text.Split(new char[]
					{
						','
					});
				}
				global::UnityEngine.Vector4 vector4;
				if (parts.Length == 4 && float.TryParse(parts[0], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector4.x) && float.TryParse(parts[1], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector4.y) && float.TryParse(parts[2], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector4.z) && float.TryParse(parts[3], global::System.Globalization.NumberStyles.Float, global::System.Globalization.CultureInfo.InvariantCulture, out vector4.w))
				{
					this.mValue = new global::UnityEngine.Rect(vector4.x, vector4.y, vector4.z, vector4.w);
				}
			}
			else
			{
				if (type.Implements(typeof(global::TNet.IDataNodeSerializable)))
				{
					global::TNet.IDataNodeSerializable dataNodeSerializable = (global::TNet.IDataNodeSerializable)type.Create();
					dataNodeSerializable.Deserialize(this);
					this.mValue = dataNodeSerializable;
					return false;
				}
				if (!type.IsSubclassOf(typeof(global::UnityEngine.Component)))
				{
					bool flag2 = type.Implements(typeof(global::System.Collections.IList));
					bool flag3 = !flag2 && type.Implements(typeof(global::TNet.TList));
					this.mValue = ((!flag3 && !flag2) ? type.Create() : type.Create(this.children.size));
					if (this.mValue == null)
					{
						global::UnityEngine.Debug.LogError("Unable to create a " + type);
						return false;
					}
					if (flag3)
					{
						global::TNet.TList tlist = this.mValue as global::TNet.TList;
						global::System.Type genericArgument = type.GetGenericArgument();
						if (genericArgument != null)
						{
							int i = 0;
							while (i < this.children.size)
							{
								global::TNet.DataNode dataNode = this.children[i];
								if (dataNode.name == "Add")
								{
									dataNode.ResolveValue(genericArgument);
									tlist.Add(dataNode.mValue);
									this.children.RemoveAt(i);
								}
								else
								{
									i++;
								}
							}
						}
						else
						{
							global::UnityEngine.Debug.LogError("Unable to determine the element type of " + type);
						}
					}
					else if (flag2)
					{
						global::System.Collections.IList list = this.mValue as global::System.Collections.IList;
						global::System.Type type2 = type.GetGenericArgument();
						if (type2 == null)
						{
							type2 = type.GetElementType();
						}
						bool flag4 = list.Count == this.children.size;
						if (type2 != null)
						{
							int j = 0;
							int num7 = 0;
							while (j < this.children.size)
							{
								global::TNet.DataNode dataNode2 = this.children[j];
								if (dataNode2.name == "Add")
								{
									dataNode2.ResolveValue(type2);
									if (flag4)
									{
										list[num7] = dataNode2.mValue;
									}
									else
									{
										list.Add(dataNode2.mValue);
									}
									this.children.RemoveAt(j);
								}
								else
								{
									j++;
								}
								num7++;
							}
						}
						else
						{
							global::UnityEngine.Debug.LogError("Unable to determine the element type of " + type);
						}
					}
					else if (type.IsClass)
					{
						int k = 0;
						while (k < this.children.size)
						{
							global::TNet.DataNode dataNode3 = this.children[k];
							if (this.mValue.SetSerializableField(dataNode3.name, dataNode3.value))
							{
								dataNode3.mValue = null;
								this.children.RemoveAt(k);
							}
							else
							{
								k++;
							}
						}
					}
					else
					{
						global::UnityEngine.Debug.LogError("Unhandled type: " + type);
					}
				}
			}
			return true;
		}

		private static string GetNextLine(global::System.IO.StreamReader reader)
		{
			string text = reader.ReadLine();
			while (text != null && text.Trim().StartsWith("//"))
			{
				text = reader.ReadLine();
				if (text == null)
				{
					return null;
				}
			}
			return text;
		}

		private static int CalculateTabs(string line)
		{
			if (line != null)
			{
				for (int i = 0; i < line.Length; i++)
				{
					if (line[i] != '\t')
					{
						return i;
					}
				}
			}
			return 0;
		}

		private static string Escape(string val)
		{
			if (!string.IsNullOrEmpty(val))
			{
				val = val.Replace("\n", "\\n");
				val = val.Replace("\t", "\\t");
			}
			return val;
		}

		private static string Unescape(string val)
		{
			if (!string.IsNullOrEmpty(val))
			{
				val = val.Replace("\\n", "\n");
				val = val.Replace("\\t", "\t");
			}
			return val;
		}

		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.DebuggerHidden]
		private static int HexToDecimal(char ch)
		{
			switch (ch)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			default:
				switch (ch)
				{
				case 'a':
					break;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				default:
					return 15;
				}
				break;
			case 'A':
				break;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			}
			return 10;
		}

		private static global::UnityEngine.Color32 ParseColor32(string text, int offset)
		{
			byte r = (byte)(global::TNet.DataNode.HexToDecimal(text[offset]) << 4 | global::TNet.DataNode.HexToDecimal(text[offset + 1]));
			byte g = (byte)(global::TNet.DataNode.HexToDecimal(text[offset + 2]) << 4 | global::TNet.DataNode.HexToDecimal(text[offset + 3]));
			byte b = (byte)(global::TNet.DataNode.HexToDecimal(text[offset + 4]) << 4 | global::TNet.DataNode.HexToDecimal(text[offset + 5]));
			byte a = (byte)((offset + 8 > text.Length) ? 255 : (global::TNet.DataNode.HexToDecimal(text[offset + 6]) << 4 | global::TNet.DataNode.HexToDecimal(text[offset + 7])));
			return new global::UnityEngine.Color32(r, g, b, a);
		}

		private object mValue;

		[global::System.NonSerialized]
		private bool mResolved = true;

		public string name;

		public global::TNet.List<global::TNet.DataNode> children = new global::TNet.List<global::TNet.DataNode>();
	}
}
