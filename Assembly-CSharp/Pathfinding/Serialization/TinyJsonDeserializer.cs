using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using UnityEngine;

namespace Pathfinding.Serialization
{
	public class TinyJsonDeserializer
	{
		public static object Deserialize(string text, global::System.Type type, object populate = null)
		{
			return new global::Pathfinding.Serialization.TinyJsonDeserializer
			{
				reader = new global::System.IO.StringReader(text)
			}.Deserialize(type, populate);
		}

		private object Deserialize(global::System.Type tp, object populate = null)
		{
			global::System.Type typeInfo = global::Pathfinding.WindowsStore.WindowsStoreCompatibility.GetTypeInfo(tp);
			if (typeInfo.IsEnum)
			{
				return global::System.Enum.Parse(tp, this.EatField());
			}
			if (this.TryEat('n'))
			{
				this.Eat("ull");
				return null;
			}
			if (object.Equals(tp, typeof(float)))
			{
				return float.Parse(this.EatField(), global::System.Globalization.CultureInfo.InvariantCulture);
			}
			if (object.Equals(tp, typeof(int)))
			{
				return int.Parse(this.EatField());
			}
			if (object.Equals(tp, typeof(uint)))
			{
				return uint.Parse(this.EatField());
			}
			if (object.Equals(tp, typeof(bool)))
			{
				return bool.Parse(this.EatField());
			}
			if (object.Equals(tp, typeof(string)))
			{
				return this.EatField();
			}
			if (object.Equals(tp, typeof(global::System.Version)))
			{
				return new global::System.Version(this.EatField());
			}
			if (object.Equals(tp, typeof(global::UnityEngine.Vector2)))
			{
				this.Eat("{");
				global::UnityEngine.Vector2 vector = default(global::UnityEngine.Vector2);
				this.EatField();
				vector.x = float.Parse(this.EatField(), global::System.Globalization.CultureInfo.InvariantCulture);
				this.EatField();
				vector.y = float.Parse(this.EatField(), global::System.Globalization.CultureInfo.InvariantCulture);
				this.Eat("}");
				return vector;
			}
			if (object.Equals(tp, typeof(global::UnityEngine.Vector3)))
			{
				this.Eat("{");
				global::UnityEngine.Vector3 vector2 = default(global::UnityEngine.Vector3);
				this.EatField();
				vector2.x = float.Parse(this.EatField(), global::System.Globalization.CultureInfo.InvariantCulture);
				this.EatField();
				vector2.y = float.Parse(this.EatField(), global::System.Globalization.CultureInfo.InvariantCulture);
				this.EatField();
				vector2.z = float.Parse(this.EatField(), global::System.Globalization.CultureInfo.InvariantCulture);
				this.Eat("}");
				return vector2;
			}
			if (object.Equals(tp, typeof(global::Pathfinding.Util.Guid)))
			{
				this.Eat("{");
				this.EatField();
				global::Pathfinding.Util.Guid guid = global::Pathfinding.Util.Guid.Parse(this.EatField());
				this.Eat("}");
				return guid;
			}
			if (object.Equals(tp, typeof(global::UnityEngine.LayerMask)))
			{
				this.Eat("{");
				this.EatField();
				global::UnityEngine.LayerMask layerMask = int.Parse(this.EatField());
				this.Eat("}");
				return layerMask;
			}
			if (object.Equals(tp, typeof(global::System.Collections.Generic.List<string>)))
			{
				global::System.Collections.IList list = new global::System.Collections.Generic.List<string>();
				this.Eat("[");
				while (!this.TryEat(']'))
				{
					list.Add(this.Deserialize(typeof(string), null));
				}
				return list;
			}
			if (typeInfo.IsArray)
			{
				global::System.Collections.Generic.List<object> list2 = new global::System.Collections.Generic.List<object>();
				this.Eat("[");
				while (!this.TryEat(']'))
				{
					list2.Add(this.Deserialize(tp.GetElementType(), null));
				}
				global::System.Array array = global::System.Array.CreateInstance(tp.GetElementType(), list2.Count);
				list2.ToArray().CopyTo(array, 0);
				return array;
			}
			if (object.Equals(tp, typeof(global::UnityEngine.Mesh)) || object.Equals(tp, typeof(global::UnityEngine.Texture2D)) || object.Equals(tp, typeof(global::UnityEngine.Transform)) || object.Equals(tp, typeof(global::UnityEngine.GameObject)))
			{
				return this.DeserializeUnityObject();
			}
			object obj = populate ?? global::System.Activator.CreateInstance(tp);
			this.Eat("{");
			while (!this.TryEat('}'))
			{
				string name = this.EatField();
				global::System.Reflection.FieldInfo field = typeInfo.GetField(name);
				if (field == null)
				{
					this.SkipFieldData();
				}
				else
				{
					field.SetValue(obj, this.Deserialize(field.FieldType, null));
				}
				this.TryEat(',');
			}
			return obj;
		}

		private global::UnityEngine.Object DeserializeUnityObject()
		{
			this.Eat("{");
			global::UnityEngine.Object result = this.DeserializeUnityObjectInner();
			this.Eat("}");
			return result;
		}

		private global::UnityEngine.Object DeserializeUnityObjectInner()
		{
			if (this.EatField() != "Name")
			{
				throw new global::System.Exception("Expected 'Name' field");
			}
			string text = this.EatField();
			if (text == null)
			{
				return null;
			}
			if (this.EatField() != "Type")
			{
				throw new global::System.Exception("Expected 'Type' field");
			}
			string text2 = this.EatField();
			if (text2.IndexOf(',') != -1)
			{
				text2 = text2.Substring(0, text2.IndexOf(','));
			}
			global::System.Type type = global::Pathfinding.WindowsStore.WindowsStoreCompatibility.GetTypeInfo(typeof(global::AstarPath)).Assembly.GetType(text2);
			type = (type ?? global::Pathfinding.WindowsStore.WindowsStoreCompatibility.GetTypeInfo(typeof(global::UnityEngine.Transform)).Assembly.GetType(text2));
			if (object.Equals(type, null))
			{
				global::UnityEngine.Debug.LogError("Could not find type '" + text2 + "'. Cannot deserialize Unity reference");
				return null;
			}
			this.EatWhitespace();
			if ((ushort)this.reader.Peek() == 34)
			{
				if (this.EatField() != "GUID")
				{
					throw new global::System.Exception("Expected 'GUID' field");
				}
				string b = this.EatField();
				global::Pathfinding.UnityReferenceHelper[] array = global::UnityEngine.Object.FindObjectsOfType<global::Pathfinding.UnityReferenceHelper>();
				int i = 0;
				while (i < array.Length)
				{
					global::Pathfinding.UnityReferenceHelper unityReferenceHelper = array[i];
					if (unityReferenceHelper.GetGUID() == b)
					{
						if (object.Equals(type, typeof(global::UnityEngine.GameObject)))
						{
							return unityReferenceHelper.gameObject;
						}
						return unityReferenceHelper.GetComponent(type);
					}
					else
					{
						i++;
					}
				}
			}
			global::UnityEngine.Object[] array2 = global::UnityEngine.Resources.LoadAll(text, type);
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j].name == text || array2.Length == 1)
				{
					return array2[j];
				}
			}
			return null;
		}

		private void EatWhitespace()
		{
			while (char.IsWhiteSpace((char)this.reader.Peek()))
			{
				this.reader.Read();
			}
		}

		private void Eat(string s)
		{
			this.EatWhitespace();
			for (int i = 0; i < s.Length; i++)
			{
				char c = (char)this.reader.Read();
				if (c != s[i])
				{
					throw new global::System.Exception(string.Concat(new object[]
					{
						"Expected '",
						s[i],
						"' found '",
						c,
						"'\n\n...",
						this.reader.ReadLine()
					}));
				}
			}
		}

		private string EatUntil(string c, bool inString)
		{
			this.builder.Length = 0;
			bool flag = false;
			for (;;)
			{
				int num = this.reader.Peek();
				if (!flag && (ushort)num == 34)
				{
					inString = !inString;
				}
				char c2 = (char)num;
				if (num == -1)
				{
					break;
				}
				if (!flag && c2 == '\\')
				{
					flag = true;
					this.reader.Read();
				}
				else
				{
					if (!inString && c.IndexOf(c2) != -1)
					{
						goto Block_7;
					}
					this.builder.Append(c2);
					this.reader.Read();
					flag = false;
				}
			}
			throw new global::System.Exception("Unexpected EOF");
			Block_7:
			return this.builder.ToString();
		}

		private bool TryEat(char c)
		{
			this.EatWhitespace();
			if ((char)this.reader.Peek() == c)
			{
				this.reader.Read();
				return true;
			}
			return false;
		}

		private string EatField()
		{
			string result = this.EatUntil("\",}]", this.TryEat('"'));
			this.TryEat('"');
			this.TryEat(':');
			this.TryEat(',');
			return result;
		}

		private void SkipFieldData()
		{
			int num = 0;
			for (;;)
			{
				this.EatUntil(",{}[]", false);
				char c = (char)this.reader.Peek();
				char c2 = c;
				switch (c2)
				{
				case '[':
					goto IL_55;
				default:
					switch (c2)
					{
					case '{':
						goto IL_55;
					default:
						if (c2 != ',')
						{
							goto Block_1;
						}
						if (num == 0)
						{
							goto Block_3;
						}
						break;
					case '}':
						goto IL_5E;
					}
					break;
				case ']':
					goto IL_5E;
				}
				IL_92:
				this.reader.Read();
				continue;
				IL_55:
				num++;
				goto IL_92;
				IL_5E:
				num--;
				if (num < 0)
				{
					return;
				}
				goto IL_92;
			}
			Block_1:
			throw new global::System.Exception("Should not reach this part");
			Block_3:
			this.reader.Read();
		}

		private global::System.IO.TextReader reader;

		private global::System.Text.StringBuilder builder = new global::System.Text.StringBuilder();
	}
}
