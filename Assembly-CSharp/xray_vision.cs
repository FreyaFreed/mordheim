using System;
using UnityEngine;

public class xray_vision : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (this.xray == null && global::UnityEngine.GameObject.Find("xray") != null)
		{
			this.xray = global::UnityEngine.GameObject.Find("xray");
		}
	}

	private void OnTriggerEnter(global::UnityEngine.Collider col)
	{
		if (col.gameObject == this.xray)
		{
			global::UnityEngine.Vector3 v = base.GetComponent<global::UnityEngine.Collider>().ClosestPointOnBounds(this.xray.transform.position);
			base.GetComponent<global::UnityEngine.Renderer>().material.SetVector("_ObjPos", v);
			global::UnityEngine.MonoBehaviour.print("xray vision has collided");
			base.GetComponent<global::UnityEngine.Renderer>().material.SetFloat("_Radius", 5f);
		}
	}

	private void OnTriggerStay(global::UnityEngine.Collider col)
	{
		if (col.gameObject == this.xray)
		{
			global::UnityEngine.Vector3 v = base.GetComponent<global::UnityEngine.Collider>().ClosestPointOnBounds(this.xray.transform.position);
			base.GetComponent<global::UnityEngine.Renderer>().material.SetVector("_ObjPos", v);
			base.GetComponent<global::UnityEngine.Renderer>().material.SetFloat("_Radius", 5f);
		}
	}

	private void OnTriggerExit(global::UnityEngine.Collider col)
	{
		if (col.gameObject == this.xray)
		{
			base.GetComponent<global::UnityEngine.Renderer>().material.SetFloat("_Radius", 0.1f);
			global::UnityEngine.MonoBehaviour.print("xray vision has exited");
		}
	}

	public global::UnityEngine.GameObject xray;
}
