using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFDelegate
{
	public void Trigger(object theSender)
	{
		this.Trigger(theSender, null);
	}

	public void Trigger(object theSender, global::System.EventArgs theArgs)
	{
		for (int i = this.itsDelegateList.Count - 1; i >= 0; i--)
		{
			global::System.Action<object, global::System.EventArgs> action = this.itsDelegateList[i];
			if (action == null)
			{
				this.itsDelegateList.RemoveAt(i);
			}
			else if (action.Target == null)
			{
				this.itsDelegateList.RemoveAt(i);
			}
			else if (action.Target is global::UnityEngine.MonoBehaviour && (global::UnityEngine.MonoBehaviour)action.Target == null)
			{
				this.itsDelegateList.RemoveAt(i);
			}
			else
			{
				action(theSender, theArgs);
			}
		}
	}

	public void Clear()
	{
		this.itsDelegateList.Clear();
	}

	public static global::KGFDelegate operator +(global::KGFDelegate theMyDelegate, global::System.Action<object, global::System.EventArgs> theDelegate)
	{
		theMyDelegate.itsDelegateList.Add(theDelegate);
		return theMyDelegate;
	}

	public static global::KGFDelegate operator -(global::KGFDelegate theMyDelegate, global::System.Action<object, global::System.EventArgs> theDelegate)
	{
		theMyDelegate.itsDelegateList.Remove(theDelegate);
		return theMyDelegate;
	}

	private global::System.Collections.Generic.List<global::System.Action<object, global::System.EventArgs>> itsDelegateList = new global::System.Collections.Generic.List<global::System.Action<object, global::System.EventArgs>>();
}
