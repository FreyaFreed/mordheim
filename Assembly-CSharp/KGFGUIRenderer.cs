using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFGUIRenderer : global::KGFObject, global::KGFIValidator
{
	protected override void KGFAwake()
	{
		this.itsGUIs = global::KGFAccessor.GetObjects<global::KGFIGui2D>();
		global::KGFAccessor.RegisterAddEvent<global::KGFIGui2D>(new global::System.Action<object, global::System.EventArgs>(this.OnRegisterKGFIGui2D));
		global::KGFAccessor.RegisterRemoveEvent<global::KGFIGui2D>(new global::System.Action<object, global::System.EventArgs>(this.OnUnregisterKGFIGui2D));
	}

	private void OnRegisterKGFIGui2D(object theSender, global::System.EventArgs theArgs)
	{
		global::KGFAccessor.KGFAccessorEventargs kgfaccessorEventargs = theArgs as global::KGFAccessor.KGFAccessorEventargs;
		if (kgfaccessorEventargs != null)
		{
			global::KGFIGui2D kgfigui2D = kgfaccessorEventargs.GetObject() as global::KGFIGui2D;
			if (kgfigui2D != null)
			{
				this.itsGUIs.Add(kgfigui2D);
				this.itsGUIs.Sort(new global::System.Comparison<global::KGFIGui2D>(this.CompareKGFIGui2D));
			}
		}
	}

	private void OnUnregisterKGFIGui2D(object theSender, global::System.EventArgs theArgs)
	{
		global::KGFAccessor.KGFAccessorEventargs kgfaccessorEventargs = theArgs as global::KGFAccessor.KGFAccessorEventargs;
		if (kgfaccessorEventargs != null)
		{
			global::KGFIGui2D kgfigui2D = kgfaccessorEventargs.GetObject() as global::KGFIGui2D;
			if (kgfigui2D != null && this.itsGUIs.Contains(kgfigui2D))
			{
				this.itsGUIs.Remove(kgfigui2D);
			}
		}
	}

	private int CompareKGFIGui2D(global::KGFIGui2D theGui1, global::KGFIGui2D theGui2)
	{
		return theGui1.GetLayer().CompareTo(theGui2.GetLayer());
	}

	protected void OnGUI()
	{
		float scaleFactor2D = global::KGFScreen.GetScaleFactor2D();
		global::UnityEngine.GUIUtility.ScaleAroundPivot(new global::UnityEngine.Vector2(scaleFactor2D, scaleFactor2D), global::UnityEngine.Vector2.zero);
		foreach (global::KGFIGui2D kgfigui2D in this.itsGUIs)
		{
			kgfigui2D.RenderGUI();
		}
		global::UnityEngine.GUI.matrix = global::UnityEngine.Matrix4x4.identity;
	}

	public virtual global::KGFMessageList Validate()
	{
		return new global::KGFMessageList();
	}

	private global::System.Collections.Generic.List<global::KGFIGui2D> itsGUIs = new global::System.Collections.Generic.List<global::KGFIGui2D>();
}
