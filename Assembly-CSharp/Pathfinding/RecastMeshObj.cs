using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_mesh_obj.php")]
	public class RecastMeshObj : global::UnityEngine.MonoBehaviour
	{
		public static void GetAllInBounds(global::System.Collections.Generic.List<global::Pathfinding.RecastMeshObj> buffer, global::UnityEngine.Bounds bounds)
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				global::Pathfinding.RecastMeshObj[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.RecastMeshObj)) as global::Pathfinding.RecastMeshObj[];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RecalculateBounds();
					if (array[i].GetBounds().Intersects(bounds))
					{
						buffer.Add(array[i]);
					}
				}
				return;
			}
			if (global::UnityEngine.Time.timeSinceLevelLoad == 0f)
			{
				global::Pathfinding.RecastMeshObj[] array2 = global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.RecastMeshObj)) as global::Pathfinding.RecastMeshObj[];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Register();
				}
			}
			for (int k = 0; k < global::Pathfinding.RecastMeshObj.dynamicMeshObjs.Count; k++)
			{
				if (global::Pathfinding.RecastMeshObj.dynamicMeshObjs[k].GetBounds().Intersects(bounds))
				{
					buffer.Add(global::Pathfinding.RecastMeshObj.dynamicMeshObjs[k]);
				}
			}
			global::UnityEngine.Rect rect = global::UnityEngine.Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			global::Pathfinding.RecastMeshObj.tree.QueryInBounds(rect, buffer);
		}

		private void OnEnable()
		{
			this.Register();
		}

		private void Register()
		{
			if (this.registered)
			{
				return;
			}
			this.registered = true;
			this.area = global::UnityEngine.Mathf.Clamp(this.area, -1, 33554432);
			global::UnityEngine.Renderer component = base.GetComponent<global::UnityEngine.Renderer>();
			global::UnityEngine.Collider component2 = base.GetComponent<global::UnityEngine.Collider>();
			if (component == null && component2 == null)
			{
				throw new global::System.Exception("A renderer or a collider should be attached to the GameObject");
			}
			global::UnityEngine.MeshFilter component3 = base.GetComponent<global::UnityEngine.MeshFilter>();
			if (component != null && component3 == null)
			{
				throw new global::System.Exception("A renderer was attached but no mesh filter");
			}
			this.bounds = ((!(component != null)) ? component2.bounds : component.bounds);
			this._dynamic = this.dynamic;
			if (this._dynamic)
			{
				global::Pathfinding.RecastMeshObj.dynamicMeshObjs.Add(this);
			}
			else
			{
				global::Pathfinding.RecastMeshObj.tree.Insert(this);
			}
		}

		private void RecalculateBounds()
		{
			global::UnityEngine.Renderer component = base.GetComponent<global::UnityEngine.Renderer>();
			global::UnityEngine.Collider collider = this.GetCollider();
			if (component == null && collider == null)
			{
				throw new global::System.Exception("A renderer or a collider should be attached to the GameObject");
			}
			global::UnityEngine.MeshFilter component2 = base.GetComponent<global::UnityEngine.MeshFilter>();
			if (component != null && component2 == null)
			{
				throw new global::System.Exception("A renderer was attached but no mesh filter");
			}
			this.bounds = ((!(component != null)) ? collider.bounds : component.bounds);
		}

		public global::UnityEngine.Bounds GetBounds()
		{
			if (this._dynamic)
			{
				this.RecalculateBounds();
			}
			return this.bounds;
		}

		public global::UnityEngine.MeshFilter GetMeshFilter()
		{
			return base.GetComponent<global::UnityEngine.MeshFilter>();
		}

		public global::UnityEngine.Collider GetCollider()
		{
			return base.GetComponent<global::UnityEngine.Collider>();
		}

		private void OnDisable()
		{
			this.registered = false;
			if (this._dynamic)
			{
				global::Pathfinding.RecastMeshObj.dynamicMeshObjs.Remove(this);
			}
			else if (!global::Pathfinding.RecastMeshObj.tree.Remove(this))
			{
				throw new global::System.Exception("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
			}
			this._dynamic = this.dynamic;
		}

		protected static global::Pathfinding.RecastBBTree tree = new global::Pathfinding.RecastBBTree();

		protected static global::System.Collections.Generic.List<global::Pathfinding.RecastMeshObj> dynamicMeshObjs = new global::System.Collections.Generic.List<global::Pathfinding.RecastMeshObj>();

		[global::UnityEngine.HideInInspector]
		public global::UnityEngine.Bounds bounds;

		public bool dynamic = true;

		public int area;

		private bool _dynamic;

		private bool registered;
	}
}
