using System;
using System.Collections.Generic;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class MapContour : global::UnityEngine.MonoBehaviour
{
	public global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>> FlatEdges { get; private set; }

	private void Awake()
	{
		this.Refresh();
	}

	private void Update()
	{
		if (this.refresh)
		{
			this.Refresh();
		}
	}

	private void Refresh()
	{
		this.refresh = false;
		this.edges = new global::System.Collections.Generic.List<global::MapEdge>();
		this.edges.AddRange(base.GetComponentsInChildren<global::MapEdge>());
		this.edges.Sort(new global::EdgeSorter());
		this.FlatEdges = new global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>>();
		for (int i = 0; i < this.edges.Count; i++)
		{
			int index = (i + 1 >= this.edges.Count) ? 0 : (i + 1);
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(this.edges[i].transform.position.x, this.edges[i].transform.position.z);
			global::UnityEngine.Vector2 item = new global::UnityEngine.Vector2(this.edges[index].transform.position.x, this.edges[index].transform.position.z);
			this.FlatEdges.Add(new global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>(vector, item));
			this.center += vector;
		}
		this.center /= (float)this.FlatEdges.Count;
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < this.edges.Count; i++)
		{
			int index = (i + 1 >= this.edges.Count) ? 0 : (i + 1);
			global::UnityEngine.Debug.DrawLine(this.edges[i].transform.position, this.edges[index].transform.position, global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(this.edges[i].transform.position + global::UnityEngine.Vector3.up * 25f, this.edges[index].transform.position + global::UnityEngine.Vector3.up * 25f, global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(this.edges[i].transform.position + global::UnityEngine.Vector3.up * 50f, this.edges[index].transform.position + global::UnityEngine.Vector3.up * 50f, global::UnityEngine.Color.white);
		}
	}

	public global::UnityEngine.Vector2 center;

	private global::System.Collections.Generic.List<global::MapEdge> edges;

	public bool refresh;
}
