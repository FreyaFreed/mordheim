using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[global::System.Serializable]
public class KGFEvent : global::KGFEventBase, global::KGFIValidator
{
	public void SetDestination(global::UnityEngine.GameObject theGameObject, string theComponentName, string theMethodString)
	{
		this.itsEventData.itsObject = theGameObject;
		this.itsEventData.itsComponentName = theComponentName;
		this.itsEventData.itsMethodName = theMethodString;
	}

	private static bool FindMethod(global::KGFEvent.KGFEventData theEventData, out global::System.Reflection.MethodInfo theMethod, out global::UnityEngine.MonoBehaviour theComponent)
	{
		theMethod = null;
		theComponent = null;
		if (theEventData.itsRuntimeObjectSearch)
		{
			foreach (global::System.Reflection.MethodInfo methodInfo in global::KGFEvent.GetMethods(theEventData.GetRuntimeType(), theEventData))
			{
				string methodString = global::KGFEvent.GetMethodString(methodInfo);
				if (methodString == theEventData.itsMethodName)
				{
					theMethod = methodInfo;
					return true;
				}
			}
		}
		else if (theEventData.itsObject != null)
		{
			global::UnityEngine.MonoBehaviour[] components = theEventData.itsObject.GetComponents<global::UnityEngine.MonoBehaviour>();
			foreach (global::UnityEngine.MonoBehaviour monoBehaviour in components)
			{
				if (monoBehaviour.GetType().Name == theEventData.itsComponentName)
				{
					theComponent = monoBehaviour;
					foreach (global::System.Reflection.MethodInfo methodInfo2 in global::KGFEvent.GetMethods(monoBehaviour.GetType(), theEventData))
					{
						string methodString2 = global::KGFEvent.GetMethodString(methodInfo2);
						if (methodString2 == theEventData.itsMethodName)
						{
							theMethod = methodInfo2;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public override void Trigger()
	{
		this.itsEventData.Trigger(this, new object[0]);
	}

	private static bool SearchInstanceForVariable(global::System.Type theType, object theInstance, string theName, ref object theValue)
	{
		global::System.Reflection.FieldInfo field = theType.GetField(theName);
		if (field != null)
		{
			theValue = field.GetValue(theInstance);
			return true;
		}
		return false;
	}

	private static object[] ConvertParameters(global::System.Reflection.ParameterInfo[] theMethodParametersList, global::KGFEvent.EventParameter[] theParametersList)
	{
		object[] array = new object[theMethodParametersList.Length];
		for (int i = 0; i < theMethodParametersList.Length; i++)
		{
			if (typeof(global::UnityEngine.Object).IsAssignableFrom(theMethodParametersList[i].ParameterType))
			{
				array[i] = theParametersList[i].itsValueUnityObject;
			}
			else if (!global::KGFEvent.SearchInstanceForVariable(typeof(global::KGFEvent.EventParameter), theParametersList[i], "itsValue" + theMethodParametersList[i].ParameterType.Name, ref array[i]))
			{
				global::UnityEngine.Debug.LogError("could not find variable for type:" + theMethodParametersList[i].ParameterType.Name);
			}
		}
		return array;
	}

	public static global::System.Reflection.MethodInfo[] GetMethods(global::System.Type theType, global::KGFEvent.KGFEventData theData)
	{
		global::System.Collections.Generic.List<global::System.Reflection.MethodInfo> list = new global::System.Collections.Generic.List<global::System.Reflection.MethodInfo>();
		for (global::System.Type type = theType; type != null; type = type.BaseType)
		{
			global::System.Reflection.MethodInfo[] methods = type.GetMethods(global::System.Reflection.BindingFlags.DeclaredOnly | global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);
			foreach (global::System.Reflection.MethodInfo methodInfo in methods)
			{
				if (methodInfo.GetCustomAttributes(typeof(global::KGFEventExpose), true).Length > 0 && theData.CheckMethod(methodInfo))
				{
					list.Add(methodInfo);
				}
			}
		}
		return list.ToArray();
	}

	public static string GetMethodString(global::System.Reflection.MethodInfo theMethod)
	{
		return theMethod.ToString();
	}

	public static void LogError(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theCaller)
	{
		global::UnityEngine.Debug.LogError(theMessage);
	}

	public static void LogDebug(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theCaller)
	{
		global::UnityEngine.Debug.Log(theMessage);
	}

	public static void LogWarning(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theCaller)
	{
		global::UnityEngine.Debug.LogWarning(theMessage);
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		if ((string.Empty + this.itsEventData.itsMethodName).Trim() == string.Empty)
		{
			kgfmessageList.AddError("itsMethod is empty");
		}
		if (!this.itsEventData.itsRuntimeObjectSearch)
		{
			if (this.itsEventData.itsObject == null)
			{
				kgfmessageList.AddError("itsObject == null");
			}
			if ((string.Empty + this.itsEventData.itsComponentName).Trim() == string.Empty)
			{
				kgfmessageList.AddError("itsScript is empty");
			}
			global::System.Reflection.MethodInfo methodInfo;
			global::UnityEngine.MonoBehaviour monoBehaviour;
			if (this.itsEventData.itsObject != null && !global::KGFEvent.FindMethod(this.itsEventData, out methodInfo, out monoBehaviour))
			{
				kgfmessageList.AddError("method could not be found");
			}
		}
		if (this.itsEventData.itsRuntimeObjectSearch)
		{
			global::System.Type runtimeType = this.itsEventData.GetRuntimeType();
			if (runtimeType == null)
			{
				kgfmessageList.AddError("could not find type");
			}
			else if (runtimeType.IsInterface)
			{
				kgfmessageList.AddWarning("you used an interface, please ensure that the objects you want to call the method on are derrived from KGFObject");
			}
			else
			{
				if (!typeof(global::UnityEngine.MonoBehaviour).IsAssignableFrom(runtimeType))
				{
					kgfmessageList.AddError("type must be derrived from Monobehaviour");
				}
				if (!typeof(global::KGFObject).IsAssignableFrom(runtimeType))
				{
					kgfmessageList.AddWarning("please derrive from KGFObject because it will be faster to search");
				}
			}
		}
		return kgfmessageList;
	}

	private const string itsEventCategory = "KGFEventSystem";

	public global::KGFEvent.KGFEventData itsEventData = new global::KGFEvent.KGFEventData();

	[global::System.Serializable]
	public class KGFEventData
	{
		public KGFEventData()
		{
		}

		public KGFEventData(bool thePassThroughMode, params global::KGFEvent.EventParameterType[] theParameterTypes)
		{
			this.itsParameterTypes = theParameterTypes;
			this.itsPassthroughMode = thePassThroughMode;
		}

		public global::System.Type GetRuntimeType()
		{
			return global::System.Type.GetType(this.itsRuntimeObjectSearchType);
		}

		public bool GetDirectPassThroughMode()
		{
			return this.itsPassthroughMode;
		}

		public void SetDirectPassThroughMode(bool thePassThroughMode)
		{
			this.itsPassthroughMode = thePassThroughMode;
		}

		public void SetRuntimeParameterInfos(params global::KGFEvent.EventParameterType[] theParameterTypes)
		{
			if (theParameterTypes == null)
			{
				this.itsParameterTypes = new global::KGFEvent.EventParameterType[0];
			}
			else
			{
				this.itsParameterTypes = theParameterTypes;
			}
		}

		public global::KGFEvent.EventParameterType[] GetParameterLinkTypes()
		{
			return this.itsParameterTypes;
		}

		public bool GetSupportsRuntimeParameterInfos()
		{
			return this.itsParameterTypes.Length > 0;
		}

		public bool GetIsParameterLinked(int theParameterIndex)
		{
			return this.GetSupportsRuntimeParameterInfos() && theParameterIndex < this.itsParameters.Length && this.itsParameters[theParameterIndex].itsLinked;
		}

		public void SetIsParameterLinked(int theParameterIndex, bool theLinkState)
		{
			if (theParameterIndex >= this.itsParameters.Length)
			{
				return;
			}
			this.itsParameters[theParameterIndex].itsLinked = theLinkState;
		}

		public int GetParameterLink(int theParameterIndex)
		{
			if (theParameterIndex >= this.itsParameters.Length)
			{
				return 0;
			}
			return this.itsParameters[theParameterIndex].itsLink;
		}

		public void SetParameterLink(int theParameterIndex, int theLink)
		{
			if (theParameterIndex >= this.itsParameters.Length)
			{
				return;
			}
			this.itsParameters[theParameterIndex].itsLink = theLink;
		}

		public global::KGFEvent.EventParameter[] GetParameters()
		{
			return this.itsParameters;
		}

		public void SetParameters(global::KGFEvent.EventParameter[] theParameters)
		{
			this.itsParameters = theParameters;
		}

		public global::UnityEngine.GameObject GetGameObject()
		{
			return this.itsObject;
		}

		private object GetFieldValueByReflection(global::UnityEngine.MonoBehaviour theCaller, string theMemberName)
		{
			global::System.Type type = theCaller.GetType();
			global::System.Reflection.FieldInfo field = type.GetField(theMemberName);
			if (field != null)
			{
				return field.GetValue(theCaller);
			}
			return null;
		}

		public void Trigger(global::UnityEngine.MonoBehaviour theCaller, params object[] theParameters)
		{
			global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>(theParameters);
			foreach (global::KGFEvent.EventParameterType eventParameterType in this.itsParameterTypes)
			{
				if (eventParameterType.GetCopyFromSourceObject())
				{
					list.Add(this.GetFieldValueByReflection(theCaller, eventParameterType.itsName));
				}
			}
			if (this.itsRuntimeObjectSearch)
			{
				this.TriggerRuntimeSearch(theCaller, list.ToArray());
			}
			else
			{
				this.TriggerDefault(theCaller, list.ToArray());
			}
		}

		private int GetParameterIndexWithType(int theIndex, string theType)
		{
			int num = 0;
			for (int i = 0; i < this.itsParameterTypes.Length; i++)
			{
				global::KGFEvent.EventParameterType eventParameterType = this.itsParameterTypes[i];
				if (eventParameterType.itsTypeName == theType)
				{
					if (num == theIndex)
					{
						return i;
					}
					num++;
				}
			}
			return 0;
		}

		private bool CheckRuntimeObjectName(global::UnityEngine.MonoBehaviour theMonobehaviour)
		{
			return this.itsRuntimeObjectSearchFilter.Trim() == string.Empty || this.itsRuntimeObjectSearchFilter == theMonobehaviour.name;
		}

		private void TriggerRuntimeSearch(global::UnityEngine.MonoBehaviour theCaller, object[] theRuntimeParameters)
		{
			global::System.Type runtimeType = this.GetRuntimeType();
			if (runtimeType == null)
			{
				global::KGFEvent.LogError("could not find type", "KGFEventSystem", theCaller);
				return;
			}
			if (this.itsMethodName == null)
			{
				global::KGFEvent.LogError("event has no selected method", "KGFEventSystem", theCaller);
				return;
			}
			global::System.Reflection.MethodInfo methodInfo;
			global::UnityEngine.MonoBehaviour monoBehaviour;
			if (!global::KGFEvent.FindMethod(this, out methodInfo, out monoBehaviour))
			{
				global::KGFEvent.LogError("Could not find method on object.", "KGFEventSystem", theCaller);
				return;
			}
			object[] array = null;
			if (this.GetDirectPassThroughMode())
			{
				array = theRuntimeParameters;
			}
			else
			{
				global::System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
				array = global::KGFEvent.ConvertParameters(parameters, this.itsParameters);
				for (int i = 0; i < this.itsParameters.Length; i++)
				{
					if (this.GetIsParameterLinked(i))
					{
						int parameterIndexWithType = this.GetParameterIndexWithType(this.GetParameterLink(i), parameters[i].ParameterType.FullName);
						if (parameterIndexWithType < theRuntimeParameters.Length)
						{
							array[i] = theRuntimeParameters[parameterIndexWithType];
						}
						else
						{
							global::UnityEngine.Debug.LogError("you did not give enough parameters");
						}
					}
				}
			}
			global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> list = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();
			try
			{
				if (runtimeType.IsInterface || typeof(global::KGFObject).IsAssignableFrom(runtimeType))
				{
					foreach (object obj in global::KGFAccessor.GetObjects(runtimeType))
					{
						global::UnityEngine.MonoBehaviour monoBehaviour2 = obj as global::UnityEngine.MonoBehaviour;
						if (monoBehaviour2 != null && this.CheckRuntimeObjectName(monoBehaviour2))
						{
							methodInfo.Invoke(obj, array);
							list.Add(monoBehaviour2);
						}
					}
				}
				else if (!runtimeType.IsInterface)
				{
					foreach (global::UnityEngine.Object obj2 in global::UnityEngine.Object.FindObjectsOfType(runtimeType))
					{
						global::UnityEngine.MonoBehaviour monoBehaviour3 = obj2 as global::UnityEngine.MonoBehaviour;
						if (monoBehaviour3 != null && this.CheckRuntimeObjectName(monoBehaviour3))
						{
							methodInfo.Invoke(obj2, array);
							list.Add(monoBehaviour3);
						}
					}
				}
			}
			catch (global::System.Exception arg)
			{
				global::KGFEvent.LogError("invoked method caused exception in event_generic:" + arg, "KGFEventSystem", theCaller);
			}
			global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
			if (array != null)
			{
				foreach (object arg2 in array)
				{
					list2.Add(string.Empty + arg2);
				}
			}
			foreach (global::UnityEngine.MonoBehaviour monoBehaviour4 in list)
			{
				string theMessage = string.Format("{0}({1}): {2} ({3})", new object[]
				{
					monoBehaviour4.name,
					this.itsRuntimeObjectSearchType,
					methodInfo.Name,
					string.Join(",", list2.ToArray())
				});
				global::KGFEvent.LogDebug(theMessage, "KGFEventSystem", theCaller);
			}
		}

		private void TriggerDefault(global::UnityEngine.MonoBehaviour theCaller, params object[] theRuntimeParameters)
		{
			if (this.itsObject == null)
			{
				global::KGFEvent.LogError("event has null object", "KGFEventSystem", theCaller);
				return;
			}
			if (this.itsComponentName == null)
			{
				global::KGFEvent.LogError("event has no selected component", "KGFEventSystem", theCaller);
				return;
			}
			if (this.itsMethodName == null)
			{
				global::KGFEvent.LogError("event has no selected method", "KGFEventSystem", theCaller);
				return;
			}
			global::System.Reflection.MethodInfo methodInfo;
			global::UnityEngine.MonoBehaviour monoBehaviour;
			if (!global::KGFEvent.FindMethod(this, out methodInfo, out monoBehaviour))
			{
				global::KGFEvent.LogError("Could not find method on object.", "KGFEventSystem", theCaller);
				return;
			}
			object[] array = null;
			if (this.GetDirectPassThroughMode())
			{
				array = theRuntimeParameters;
			}
			else
			{
				global::System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
				array = global::KGFEvent.ConvertParameters(parameters, this.itsParameters);
				for (int i = 0; i < this.itsParameters.Length; i++)
				{
					if (this.GetIsParameterLinked(i))
					{
						int parameterIndexWithType = this.GetParameterIndexWithType(this.GetParameterLink(i), parameters[i].ParameterType.FullName);
						if (parameterIndexWithType < theRuntimeParameters.Length)
						{
							array[i] = theRuntimeParameters[parameterIndexWithType];
						}
						else
						{
							global::UnityEngine.Debug.LogError("you did not give enough parameters");
						}
					}
				}
			}
			try
			{
				methodInfo.Invoke(monoBehaviour, array);
			}
			catch (global::System.Exception arg)
			{
				global::KGFEvent.LogError("invoked method caused exception in event_generic:" + arg, "KGFEventSystem", theCaller);
			}
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			if (array != null)
			{
				foreach (object arg2 in array)
				{
					list.Add(string.Empty + arg2);
				}
			}
			string theMessage = string.Format("{0}({1}): {2} ({3})", new object[]
			{
				this.itsObject.name,
				monoBehaviour.GetType().Name,
				methodInfo.Name,
				string.Join(",", list.ToArray())
			});
			global::KGFEvent.LogDebug(theMessage, "KGFEventSystem", theCaller);
		}

		public void SetMethodFilter(global::KGFEvent.KGFEventFilterMethod theFilter)
		{
			this.itsFilterMethod = theFilter;
		}

		public void ClearMethodFilter()
		{
			this.itsFilterMethod = null;
		}

		private global::KGFEvent.KGFEventFilterMethod GetFilterMethod()
		{
			return this.itsFilterMethod;
		}

		public bool CheckMethod(global::System.Reflection.MethodInfo theMethod)
		{
			if (this.itsFilterMethod != null && !this.GetFilterMethod()(theMethod))
			{
				return false;
			}
			if (this.GetSupportsRuntimeParameterInfos() && this.GetDirectPassThroughMode())
			{
				global::System.Reflection.ParameterInfo[] parameters = theMethod.GetParameters();
				if (parameters.Length != this.itsParameterTypes.Length)
				{
					return false;
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					if (!this.itsParameterTypes[i].GetIsMatchingType(parameters[i].ParameterType))
					{
						return false;
					}
				}
			}
			return true;
		}

		public global::KGFMessageList GetErrors()
		{
			global::KGFMessageList kgfmessageList = new global::KGFMessageList();
			if (string.IsNullOrEmpty(this.itsMethodName))
			{
				kgfmessageList.AddError("Empty method name");
			}
			if (this.itsRuntimeObjectSearch && string.IsNullOrEmpty(this.itsRuntimeObjectSearchType))
			{
				kgfmessageList.AddError("Empty type field");
			}
			global::System.Reflection.MethodInfo methodInfo;
			global::UnityEngine.MonoBehaviour monoBehaviour;
			if (!global::KGFEvent.FindMethod(this, out methodInfo, out monoBehaviour))
			{
				kgfmessageList.AddError("Could not find method on object.");
			}
			else
			{
				global::System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
				for (int i = 0; i < this.itsParameters.Length; i++)
				{
					if (!this.GetIsParameterLinked(i) && typeof(global::UnityEngine.Object).IsAssignableFrom(parameters[i].ParameterType) && this.itsParameters[i].itsValueUnityObject == null)
					{
						kgfmessageList.AddError("Empty unity object in parameters");
					}
				}
			}
			return kgfmessageList;
		}

		public bool itsRuntimeObjectSearch;

		public string itsRuntimeObjectSearchType = string.Empty;

		public string itsRuntimeObjectSearchFilter = string.Empty;

		public global::UnityEngine.GameObject itsObject;

		public string itsComponentName = string.Empty;

		public string itsMethodName = string.Empty;

		public string itsMethodNameShort = string.Empty;

		public global::KGFEvent.EventParameter[] itsParameters = new global::KGFEvent.EventParameter[0];

		public global::KGFEvent.EventParameterType[] itsParameterTypes = new global::KGFEvent.EventParameterType[0];

		public bool itsPassthroughMode;

		private global::KGFEvent.KGFEventFilterMethod itsFilterMethod;
	}

	[global::System.Serializable]
	public class EventParameterType
	{
		public EventParameterType()
		{
		}

		public EventParameterType(string theName, global::System.Type theType)
		{
			this.itsName = theName;
			this.itsTypeName = theType.FullName;
		}

		public void SetCopyFromSourceObject(bool theCopy)
		{
			this.itsCopyFromSourceObject = theCopy;
		}

		public bool GetCopyFromSourceObject()
		{
			return this.itsCopyFromSourceObject;
		}

		public bool GetIsMatchingType(global::System.Type theOtherParameterType)
		{
			return this.itsTypeName == theOtherParameterType.FullName;
		}

		public string itsName;

		public string itsTypeName;

		public bool itsCopyFromSourceObject;
	}

	[global::System.Serializable]
	public class EventParameter
	{
		public EventParameter()
		{
			this.itsValueUnityObject = null;
		}

		public int itsValueInt32;

		public string itsValueString;

		public float itsValueSingle;

		public double itsValueDouble;

		public global::UnityEngine.Color itsValueColor;

		public global::UnityEngine.Rect itsValueRect;

		public global::UnityEngine.Vector2 itsValueVector2;

		public global::UnityEngine.Vector3 itsValueVector3;

		public global::UnityEngine.Vector4 itsValueVector4;

		public bool itsValueBoolean;

		public global::UnityEngine.Object itsValueUnityObject;

		public bool itsLinked;

		public int itsLink;
	}

	public delegate bool KGFEventFilterMethod(global::System.Reflection.MethodInfo theMethod);
}
