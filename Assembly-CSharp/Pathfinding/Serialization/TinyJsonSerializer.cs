using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using UnityEngine;

namespace Pathfinding.Serialization
{
	public class TinyJsonSerializer
	{
		private TinyJsonSerializer()
		{
			this.serializers[typeof(float)] = delegate(object v)
			{
				this.output.Append(((float)v).ToString("R"));
			};
			global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Action<object>> dictionary = this.serializers;
			global::System.Type typeFromHandle = typeof(global::System.Version);
			global::System.Action<object> action = delegate(object v)
			{
				this.output.Append(v.ToString());
			};
			this.serializers[typeof(int)] = action;
			action = action;
			this.serializers[typeof(uint)] = action;
			action = action;
			this.serializers[typeof(bool)] = action;
			dictionary[typeFromHandle] = action;
			this.serializers[typeof(string)] = delegate(object v)
			{
				this.output.AppendFormat("\"{0}\"", v);
			};
			this.serializers[typeof(global::UnityEngine.Vector2)] = delegate(object v)
			{
				this.output.AppendFormat("{{ \"x\": {0}, \"y\": {1} }}", ((global::UnityEngine.Vector2)v).x.ToString("R"), ((global::UnityEngine.Vector2)v).y.ToString("R"));
			};
			this.serializers[typeof(global::UnityEngine.Vector3)] = delegate(object v)
			{
				this.output.AppendFormat("{{ \"x\": {0}, \"y\": {1}, \"z\": {2} }}", ((global::UnityEngine.Vector3)v).x.ToString("R"), ((global::UnityEngine.Vector3)v).y.ToString("R"), ((global::UnityEngine.Vector3)v).z.ToString("R"));
			};
			this.serializers[typeof(global::Pathfinding.Util.Guid)] = delegate(object v)
			{
				this.output.AppendFormat("{{ \"value\": \"{0}\" }}", v.ToString());
			};
			this.serializers[typeof(global::UnityEngine.LayerMask)] = delegate(object v)
			{
				this.output.AppendFormat("{{ \"value\": {0} }}", ((global::UnityEngine.LayerMask)v).ToString());
			};
		}

		public static void Serialize(object obj, global::System.Text.StringBuilder output)
		{
			new global::Pathfinding.Serialization.TinyJsonSerializer
			{
				output = output
			}.Serialize(obj);
		}

		private void Serialize(object obj)
		{
			if (obj == null)
			{
				this.output.Append("null");
				return;
			}
			global::System.Type typeInfo = global::Pathfinding.WindowsStore.WindowsStoreCompatibility.GetTypeInfo(obj.GetType());
			if (this.serializers.ContainsKey(typeInfo))
			{
				this.serializers[typeInfo](obj);
			}
			else if (typeInfo.IsEnum)
			{
				this.output.Append('"' + obj.ToString() + '"');
			}
			else if (obj is global::System.Collections.IList)
			{
				this.output.Append("[");
				global::System.Collections.IList list = obj as global::System.Collections.IList;
				for (int i = 0; i < list.Count; i++)
				{
					if (i != 0)
					{
						this.output.Append(", ");
					}
					this.Serialize(list[i]);
				}
				this.output.Append("]");
			}
			else if (obj is global::UnityEngine.Object)
			{
				this.SerializeUnityObject(obj as global::UnityEngine.Object);
			}
			else
			{
				bool flag = typeInfo.GetCustomAttributes(typeof(global::Pathfinding.Serialization.JsonOptInAttribute), true).Length > 0;
				this.output.Append("{");
				global::System.Reflection.FieldInfo[] fields = typeInfo.GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				bool flag2 = false;
				foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
				{
					if ((!flag && fieldInfo.IsPublic) || fieldInfo.GetCustomAttributes(typeof(global::Pathfinding.Serialization.JsonMemberAttribute), true).Length > 0)
					{
						if (flag2)
						{
							this.output.Append(", ");
						}
						flag2 = true;
						this.output.AppendFormat("\"{0}\": ", fieldInfo.Name);
						this.Serialize(fieldInfo.GetValue(obj));
					}
				}
				this.output.Append("}");
			}
		}

		private void QuotedField(string name, string contents)
		{
			this.output.AppendFormat("\"{0}\": \"{1}\"", name, contents);
		}

		private void SerializeUnityObject(global::UnityEngine.Object obj)
		{
			if (obj == null)
			{
				this.Serialize(null);
				return;
			}
			this.output.Append("{");
			this.QuotedField("Name", obj.name);
			this.output.Append(", ");
			this.QuotedField("Type", obj.GetType().FullName);
			global::UnityEngine.Component component = obj as global::UnityEngine.Component;
			global::UnityEngine.GameObject gameObject = obj as global::UnityEngine.GameObject;
			if (component != null || gameObject != null)
			{
				if (component != null && gameObject == null)
				{
					gameObject = component.gameObject;
				}
				global::Pathfinding.UnityReferenceHelper unityReferenceHelper = gameObject.GetComponent<global::Pathfinding.UnityReferenceHelper>();
				if (unityReferenceHelper == null)
				{
					global::UnityEngine.Debug.Log("Adding UnityReferenceHelper to Unity Reference '" + obj.name + "'");
					unityReferenceHelper = gameObject.AddComponent<global::Pathfinding.UnityReferenceHelper>();
				}
				unityReferenceHelper.Reset();
				this.output.Append(", ");
				this.QuotedField("GUID", unityReferenceHelper.GetGUID().ToString());
			}
			this.output.Append("}");
		}

		private global::System.Text.StringBuilder output = new global::System.Text.StringBuilder();

		private global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Action<object>> serializers = new global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Action<object>>();
	}
}
