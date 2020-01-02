using System;
using System.Reflection;
using UnityEngine;

namespace TNet
{
	public static class UnityTools
	{
		public static void Clear(object[] objs)
		{
			int i = 0;
			int num = objs.Length;
			while (i < num)
			{
				objs[i] = null;
				i++;
			}
		}

		private static void PrintException(global::System.Exception ex, global::TNet.CachedFunc ent, int funcID, string funcName, params object[] parameters)
		{
			string text = string.Empty;
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (i != 0)
					{
						text += ", ";
					}
					text += parameters[i].GetType().ToString();
				}
			}
			string text2 = string.Empty;
			if (ent.parameters != null)
			{
				for (int j = 0; j < ent.parameters.Length; j++)
				{
					if (j != 0)
					{
						text2 += ", ";
					}
					text2 += ent.parameters[j].ParameterType.ToString();
				}
			}
			string text3 = "Failed to call RFC ";
			if (string.IsNullOrEmpty(funcName))
			{
				string text4 = text3;
				text3 = string.Concat(new object[]
				{
					text4,
					"#",
					funcID,
					" on ",
					(ent.obj == null) ? "<null>" : ent.obj.GetType().ToString()
				});
			}
			else
			{
				string text4 = text3;
				text3 = string.Concat(new object[]
				{
					text4,
					ent.obj.GetType(),
					".",
					funcName
				});
			}
			if (ex.InnerException != null)
			{
				text3 = text3 + ": " + ex.InnerException.Message + "\n";
			}
			else
			{
				text3 = text3 + ": " + ex.Message + "\n";
			}
			if (text != text2)
			{
				text3 = text3 + "  Expected args: " + text2 + "\n";
				text3 = text3 + "  Received args: " + text + "\n\n";
			}
			if (ex.InnerException != null)
			{
				text3 = text3 + ex.InnerException.StackTrace + "\n";
			}
			else
			{
				text3 = text3 + ex.StackTrace + "\n";
			}
			global::UnityEngine.Debug.LogError(text3);
		}

		public static bool ExecuteFirst(global::TNet.List<global::TNet.CachedFunc> rfcs, byte funcID, out object retVal, params object[] parameters)
		{
			retVal = null;
			for (int i = 0; i < rfcs.size; i++)
			{
				global::TNet.CachedFunc ent = rfcs[i];
				if (ent.id == funcID)
				{
					if (ent.parameters == null)
					{
						ent.parameters = ent.func.GetParameters();
					}
					try
					{
						retVal = ((ent.parameters.Length != 1 || ent.parameters[0].ParameterType != typeof(object[])) ? ent.func.Invoke(ent.obj, parameters) : ent.func.Invoke(ent.obj, new object[]
						{
							parameters
						}));
						return retVal != null;
					}
					catch (global::System.Exception ex)
					{
						global::TNet.UnityTools.PrintException(ex, ent, (int)funcID, string.Empty, parameters);
						return false;
					}
				}
			}
			global::UnityEngine.Debug.LogError("[TNet] Unable to find an function with ID of " + funcID);
			return false;
		}

		public static bool ExecuteAll(global::TNet.List<global::TNet.CachedFunc> rfcs, byte funcID, params object[] parameters)
		{
			for (int i = 0; i < rfcs.size; i++)
			{
				global::TNet.CachedFunc ent = rfcs[i];
				if (ent.id == funcID)
				{
					if (ent.parameters == null)
					{
						ent.parameters = ent.func.GetParameters();
					}
					try
					{
						if (ent.parameters.Length == 1 && ent.parameters[0].ParameterType == typeof(object[]))
						{
							ent.func.Invoke(ent.obj, new object[]
							{
								parameters
							});
						}
						else
						{
							ent.func.Invoke(ent.obj, parameters);
						}
						return true;
					}
					catch (global::System.Exception ex)
					{
						global::TNet.UnityTools.PrintException(ex, ent, (int)funcID, string.Empty, parameters);
						return false;
					}
				}
			}
			global::UnityEngine.Debug.LogError("[TNet] Unable to find an function with ID of " + funcID);
			return false;
		}

		public static bool ExecuteAll(global::TNet.List<global::TNet.CachedFunc> rfcs, string funcName, params object[] parameters)
		{
			bool result = false;
			for (int i = 0; i < rfcs.size; i++)
			{
				global::TNet.CachedFunc ent = rfcs[i];
				if (ent.func.Name == funcName)
				{
					result = true;
					if (ent.parameters == null)
					{
						ent.parameters = ent.func.GetParameters();
					}
					try
					{
						if (ent.parameters.Length == 1 && ent.parameters[0].ParameterType == typeof(object[]))
						{
							ent.func.Invoke(ent.obj, new object[]
							{
								parameters
							});
						}
						else
						{
							ent.func.Invoke(ent.obj, parameters);
						}
						return true;
					}
					catch (global::System.Exception ex)
					{
						global::TNet.UnityTools.PrintException(ex, ent, 0, funcName, parameters);
					}
				}
			}
			global::UnityEngine.Debug.LogError("[TNet] Unable to find a function called '" + funcName + "'");
			return result;
		}

		public static void Broadcast(string methodName, params object[] parameters)
		{
			global::UnityEngine.MonoBehaviour[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::UnityEngine.MonoBehaviour)) as global::UnityEngine.MonoBehaviour[];
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				global::UnityEngine.MonoBehaviour monoBehaviour = array[i];
				global::System.Reflection.MethodInfo method = monoBehaviour.GetType().GetMethod(methodName, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
				if (method != null)
				{
					try
					{
						method.Invoke(monoBehaviour, parameters);
					}
					catch (global::System.Exception ex)
					{
						global::UnityEngine.Debug.LogError(string.Concat(new object[]
						{
							ex.Message,
							" (",
							monoBehaviour.GetType(),
							".",
							methodName,
							")"
						}), monoBehaviour);
					}
				}
				i++;
			}
		}

		public static float SpringLerp(float from, float to, float strength, float deltaTime)
		{
			if (deltaTime > 1f)
			{
				deltaTime = 1f;
			}
			int num = global::UnityEngine.Mathf.RoundToInt(deltaTime * 1000f);
			deltaTime = 0.001f * strength;
			for (int i = 0; i < num; i++)
			{
				from = global::UnityEngine.Mathf.Lerp(from, to, deltaTime);
			}
			return from;
		}

		public static global::UnityEngine.Rect PadRect(global::UnityEngine.Rect rect, float padding)
		{
			global::UnityEngine.Rect result = rect;
			result.xMin -= padding;
			result.xMax += padding;
			result.yMin -= padding;
			result.yMax += padding;
			return result;
		}

		public static bool IsParentChild(global::UnityEngine.GameObject parent, global::UnityEngine.GameObject child)
		{
			return !(parent == null) && !(child == null) && global::TNet.UnityTools.IsParentChild(parent.transform, child.transform);
		}

		public static bool IsParentChild(global::UnityEngine.Transform parent, global::UnityEngine.Transform child)
		{
			if (parent == null || child == null)
			{
				return false;
			}
			while (child != null)
			{
				if (parent == child)
				{
					return true;
				}
				child = child.parent;
			}
			return false;
		}

		public static global::UnityEngine.GameObject Instantiate(global::UnityEngine.GameObject go, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, global::UnityEngine.Vector3 velocity, global::UnityEngine.Vector3 angularVelocity)
		{
			if (go != null)
			{
				go = (global::UnityEngine.Object.Instantiate(go, pos, rot) as global::UnityEngine.GameObject);
				global::UnityEngine.Rigidbody component = go.GetComponent<global::UnityEngine.Rigidbody>();
				if (component != null)
				{
					if (component.isKinematic)
					{
						component.isKinematic = false;
						component.velocity = velocity;
						component.angularVelocity = angularVelocity;
						component.isKinematic = true;
					}
					else
					{
						component.velocity = velocity;
						component.angularVelocity = angularVelocity;
					}
				}
			}
			return go;
		}
	}
}
