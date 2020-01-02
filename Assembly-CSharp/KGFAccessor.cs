using System;
using System.Collections.Generic;
using UnityEngine;

public static class KGFAccessor
{
	public static void AddKGFObject(global::KGFObject theObjectScript)
	{
		global::System.Type type = theObjectScript.GetType();
		if (!global::KGFAccessor.itsListSorted.ContainsKey(type))
		{
			global::KGFAccessor.itsListSorted[type] = new global::System.Collections.Generic.List<global::KGFObject>();
		}
		global::KGFAccessor.itsListSorted[type].Add(theObjectScript);
		foreach (global::System.Type type2 in global::KGFAccessor.itsListEventsAdd.Keys)
		{
			if (type2.IsAssignableFrom(type))
			{
				global::KGFAccessor.itsListEventsAdd[type2].Trigger(null, new global::KGFAccessor.KGFAccessorEventargs(theObjectScript));
			}
		}
		if (global::KGFAccessor.itsListEventsAddOnce.Count > 0)
		{
			global::System.Collections.Generic.List<global::System.Type> list = new global::System.Collections.Generic.List<global::System.Type>();
			foreach (global::System.Type type3 in global::KGFAccessor.itsListEventsAddOnce.Keys)
			{
				if (type3.IsAssignableFrom(type))
				{
					list.Add(type3);
				}
			}
			foreach (global::System.Type key in list)
			{
				global::KGFAccessor.itsListEventsAddOnce[key].Trigger(null, new global::KGFAccessor.KGFAccessorEventargs(theObjectScript));
				global::KGFAccessor.itsListEventsAddOnce.Remove(key);
			}
		}
	}

	public static void RemoveKGFObject(global::KGFObject theObjectScript)
	{
		global::System.Type type = theObjectScript.GetType();
		try
		{
			global::KGFAccessor.itsListSorted[type].Remove(theObjectScript);
		}
		catch
		{
		}
		foreach (global::System.Type type2 in global::KGFAccessor.itsListEventsRemove.Keys)
		{
			if (type2.IsAssignableFrom(type))
			{
				global::KGFAccessor.itsListEventsRemove[type2].Trigger(null, new global::KGFAccessor.KGFAccessorEventargs(theObjectScript));
			}
		}
	}

	public static void GetExternal<T>(global::System.Action<object, global::System.EventArgs> theRegisterCallback)
	{
		T @object = global::KGFAccessor.GetObject<T>();
		if (@object != null)
		{
			theRegisterCallback(null, new global::KGFAccessor.KGFAccessorEventargs(@object));
		}
		else
		{
			global::KGFAccessor.RegisterAddOnceEvent<T>(theRegisterCallback);
		}
	}

	public static void RegisterAddEvent<T>(global::System.Action<object, global::System.EventArgs> theCallback)
	{
		if (theCallback == null)
		{
			return;
		}
		global::System.Type typeFromHandle = typeof(T);
		if (!global::KGFAccessor.itsListEventsAdd.ContainsKey(typeFromHandle))
		{
			global::KGFAccessor.itsListEventsAdd[typeFromHandle] = new global::KGFDelegate();
		}
		global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary2;
		global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary = dictionary2 = global::KGFAccessor.itsListEventsAdd;
		global::System.Type key2;
		global::System.Type key = key2 = typeFromHandle;
		global::KGFDelegate theMyDelegate = dictionary2[key2];
		dictionary[key] = theMyDelegate + theCallback;
	}

	public static void RegisterAddOnceEvent<T>(global::System.Action<object, global::System.EventArgs> theCallback)
	{
		if (theCallback == null)
		{
			return;
		}
		global::System.Type typeFromHandle = typeof(T);
		if (!global::KGFAccessor.itsListEventsAddOnce.ContainsKey(typeFromHandle))
		{
			global::KGFAccessor.itsListEventsAddOnce[typeFromHandle] = new global::KGFDelegate();
		}
		global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary2;
		global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary = dictionary2 = global::KGFAccessor.itsListEventsAddOnce;
		global::System.Type key2;
		global::System.Type key = key2 = typeFromHandle;
		global::KGFDelegate theMyDelegate = dictionary2[key2];
		dictionary[key] = theMyDelegate + theCallback;
	}

	public static void UnregisterAddEvent<T>(global::System.Action<object, global::System.EventArgs> theCallback)
	{
		global::System.Type typeFromHandle = typeof(T);
		if (global::KGFAccessor.itsListEventsAdd.ContainsKey(typeFromHandle))
		{
			global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary2;
			global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary = dictionary2 = global::KGFAccessor.itsListEventsAdd;
			global::System.Type key2;
			global::System.Type key = key2 = typeFromHandle;
			global::KGFDelegate theMyDelegate = dictionary2[key2];
			dictionary[key] = theMyDelegate - theCallback;
		}
	}

	public static void RegisterRemoveEvent<T>(global::System.Action<object, global::System.EventArgs> theCallback)
	{
		if (theCallback == null)
		{
			return;
		}
		global::System.Type typeFromHandle = typeof(T);
		if (!global::KGFAccessor.itsListEventsRemove.ContainsKey(typeFromHandle))
		{
			global::KGFAccessor.itsListEventsRemove[typeFromHandle] = new global::KGFDelegate();
		}
		global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary2;
		global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary = dictionary2 = global::KGFAccessor.itsListEventsRemove;
		global::System.Type key2;
		global::System.Type key = key2 = typeFromHandle;
		global::KGFDelegate theMyDelegate = dictionary2[key2];
		dictionary[key] = theMyDelegate + theCallback;
	}

	public static void UnregisterRemoveEvent<T>(global::System.Action<object, global::System.EventArgs> theCallback)
	{
		global::System.Type typeFromHandle = typeof(T);
		if (global::KGFAccessor.itsListEventsRemove.ContainsKey(typeFromHandle))
		{
			global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary2;
			global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> dictionary = dictionary2 = global::KGFAccessor.itsListEventsRemove;
			global::System.Type key2;
			global::System.Type key = key2 = typeFromHandle;
			global::KGFDelegate theMyDelegate = dictionary2[key2];
			dictionary[key] = theMyDelegate - theCallback;
		}
	}

	public static global::System.Collections.Generic.IEnumerable<T> GetObjectsEnumerable<T>()
	{
		foreach (object anObject in global::KGFAccessor.GetObjectsEnumerable(typeof(T)))
		{
			yield return (T)((object)anObject);
		}
		yield break;
	}

	public static global::System.Collections.Generic.IEnumerable<object> GetObjectsEnumerable(global::System.Type theType)
	{
		foreach (global::System.Type aType in global::KGFAccessor.itsListSorted.Keys)
		{
			if (theType.IsAssignableFrom(aType))
			{
				global::System.Collections.Generic.List<global::KGFObject> aListObjectScripts = global::KGFAccessor.itsListSorted[aType];
				for (int i = aListObjectScripts.Count - 1; i >= 0; i--)
				{
					object anObject = aListObjectScripts[i];
					global::UnityEngine.MonoBehaviour aMonobehaviour = aListObjectScripts[i];
					if (aMonobehaviour == null)
					{
						aListObjectScripts.RemoveAt(i);
					}
					else if (aMonobehaviour.gameObject == null)
					{
						aListObjectScripts.RemoveAt(i);
					}
					else
					{
						yield return anObject;
					}
				}
			}
		}
		yield break;
	}

	public static global::System.Collections.Generic.List<T> GetObjects<T>()
	{
		return new global::System.Collections.Generic.List<T>(global::KGFAccessor.GetObjectsEnumerable<T>());
	}

	public static global::System.Collections.Generic.List<object> GetObjects(global::System.Type theType)
	{
		return new global::System.Collections.Generic.List<object>(global::KGFAccessor.GetObjectsEnumerable(theType));
	}

	public static global::System.Collections.Generic.IEnumerable<string> GetObjectsNames<T>() where T : global::KGFObject
	{
		foreach (T t in global::KGFAccessor.GetObjects<T>())
		{
			global::KGFObject anObject = t;
			yield return anObject.name;
		}
		yield break;
	}

	public static T GetObject<T>()
	{
		using (global::System.Collections.Generic.IEnumerator<T> enumerator = global::KGFAccessor.GetObjectsEnumerable<T>().GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return default(T);
	}

	public static object GetObject(global::System.Type theType)
	{
		using (global::System.Collections.Generic.IEnumerator<object> enumerator = global::KGFAccessor.GetObjectsEnumerable(theType).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return null;
	}

	public static int GetAddHandlerCount()
	{
		return global::KGFAccessor.itsListEventsAdd.Count;
	}

	public static int GetAddOnceHandlerCount()
	{
		return global::KGFAccessor.itsListEventsAddOnce.Count;
	}

	public static global::System.Collections.Generic.IEnumerable<global::System.Type> GetObjectCacheListTypes()
	{
		foreach (global::System.Type aType in global::KGFAccessor.itsListSorted.Keys)
		{
			yield return aType;
		}
		yield break;
	}

	public static int GetObjectCacheListCountByType(global::System.Type theType)
	{
		if (global::KGFAccessor.itsListSorted.ContainsKey(theType))
		{
			return global::KGFAccessor.itsListSorted[theType].Count;
		}
		return 0;
	}

	private static global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.List<global::KGFObject>> itsListSorted = new global::System.Collections.Generic.Dictionary<global::System.Type, global::System.Collections.Generic.List<global::KGFObject>>();

	private static global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> itsListEventsAdd = new global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate>();

	private static global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> itsListEventsAddOnce = new global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate>();

	private static global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate> itsListEventsRemove = new global::System.Collections.Generic.Dictionary<global::System.Type, global::KGFDelegate>();

	public class KGFAccessorEventargs : global::System.EventArgs
	{
		public KGFAccessorEventargs(object theObject)
		{
			this.itsObject = theObject;
		}

		public object GetObject()
		{
			return this.itsObject;
		}

		private object itsObject;
	}
}
