using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_add.php")]
	public class NavmeshAdd : global::UnityEngine.MonoBehaviour
	{
		private static void Add(global::Pathfinding.NavmeshAdd obj)
		{
			global::Pathfinding.NavmeshAdd.allCuts.Add(obj);
		}

		private static void Remove(global::Pathfinding.NavmeshAdd obj)
		{
			global::Pathfinding.NavmeshAdd.allCuts.Remove(obj);
		}

		public static global::System.Collections.Generic.List<global::Pathfinding.NavmeshAdd> GetAllInRange(global::UnityEngine.Bounds b)
		{
			global::System.Collections.Generic.List<global::Pathfinding.NavmeshAdd> list = global::Pathfinding.Util.ListPool<global::Pathfinding.NavmeshAdd>.Claim();
			for (int i = 0; i < global::Pathfinding.NavmeshAdd.allCuts.Count; i++)
			{
				if (global::Pathfinding.NavmeshAdd.allCuts[i].enabled && global::Pathfinding.NavmeshAdd.Intersects(b, global::Pathfinding.NavmeshAdd.allCuts[i].GetBounds()))
				{
					list.Add(global::Pathfinding.NavmeshAdd.allCuts[i]);
				}
			}
			return list;
		}

		private static bool Intersects(global::UnityEngine.Bounds b1, global::UnityEngine.Bounds b2)
		{
			global::UnityEngine.Vector3 min = b1.min;
			global::UnityEngine.Vector3 max = b1.max;
			global::UnityEngine.Vector3 min2 = b2.min;
			global::UnityEngine.Vector3 max2 = b2.max;
			return min.x <= max2.x && max.x >= min2.x && min.z <= max2.z && max.z >= min2.z;
		}

		public static global::System.Collections.Generic.List<global::Pathfinding.NavmeshAdd> GetAll()
		{
			return global::Pathfinding.NavmeshAdd.allCuts;
		}

		public void Awake()
		{
			global::Pathfinding.NavmeshAdd.Add(this);
		}

		public void OnEnable()
		{
			this.tr = base.transform;
		}

		public void OnDestroy()
		{
			global::Pathfinding.NavmeshAdd.Remove(this);
		}

		public global::UnityEngine.Vector3 Center
		{
			get
			{
				return this.tr.position + ((!this.useRotation) ? this.center : this.tr.TransformPoint(this.center));
			}
		}

		[global::UnityEngine.ContextMenu("Rebuild Mesh")]
		public void RebuildMesh()
		{
			if (this.type == global::Pathfinding.NavmeshAdd.MeshType.CustomMesh)
			{
				if (this.mesh == null)
				{
					this.verts = null;
					this.tris = null;
				}
				else
				{
					this.verts = this.mesh.vertices;
					this.tris = this.mesh.triangles;
				}
			}
			else
			{
				if (this.verts == null || this.verts.Length != 4 || this.tris == null || this.tris.Length != 6)
				{
					this.verts = new global::UnityEngine.Vector3[4];
					this.tris = new int[6];
				}
				this.tris[0] = 0;
				this.tris[1] = 1;
				this.tris[2] = 2;
				this.tris[3] = 0;
				this.tris[4] = 2;
				this.tris[5] = 3;
				this.verts[0] = new global::UnityEngine.Vector3(-this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[1] = new global::UnityEngine.Vector3(this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[2] = new global::UnityEngine.Vector3(this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				this.verts[3] = new global::UnityEngine.Vector3(-this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
			}
		}

		public global::UnityEngine.Bounds GetBounds()
		{
			global::Pathfinding.NavmeshAdd.MeshType meshType = this.type;
			if (meshType != global::Pathfinding.NavmeshAdd.MeshType.Rectangle)
			{
				if (meshType == global::Pathfinding.NavmeshAdd.MeshType.CustomMesh)
				{
					if (!(this.mesh == null))
					{
						global::UnityEngine.Bounds bounds = this.mesh.bounds;
						if (this.useRotation)
						{
							global::UnityEngine.Matrix4x4 matrix4x = global::UnityEngine.Matrix4x4.TRS(this.tr.position, this.tr.rotation, global::UnityEngine.Vector3.one * this.meshScale);
							this.bounds = new global::UnityEngine.Bounds(matrix4x.MultiplyPoint3x4(this.center + bounds.center), global::UnityEngine.Vector3.zero);
							global::UnityEngine.Vector3 max = bounds.max;
							global::UnityEngine.Vector3 min = bounds.min;
							this.bounds.Encapsulate(matrix4x.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(max.x, min.y, max.z)));
							this.bounds.Encapsulate(matrix4x.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(min.x, min.y, max.z)));
							this.bounds.Encapsulate(matrix4x.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(min.x, max.y, min.z)));
							this.bounds.Encapsulate(matrix4x.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(max.x, max.y, min.z)));
						}
						else
						{
							global::UnityEngine.Vector3 size = bounds.size * this.meshScale;
							this.bounds = new global::UnityEngine.Bounds(base.transform.position + this.center + bounds.center * this.meshScale, size);
						}
					}
				}
			}
			else if (this.useRotation)
			{
				global::UnityEngine.Matrix4x4 matrix4x2 = global::UnityEngine.Matrix4x4.TRS(this.tr.position, this.tr.rotation, global::UnityEngine.Vector3.one);
				this.bounds = new global::UnityEngine.Bounds(matrix4x2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f), global::UnityEngine.Vector3.zero);
				this.bounds.Encapsulate(matrix4x2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(matrix4x2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(matrix4x2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f));
			}
			else
			{
				this.bounds = new global::UnityEngine.Bounds(this.tr.position + this.center, new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y));
			}
			return this.bounds;
		}

		public void GetMesh(global::Pathfinding.Int3 offset, ref global::Pathfinding.Int3[] vbuffer, out int[] tbuffer)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			if (this.verts == null)
			{
				tbuffer = new int[0];
				return;
			}
			if (vbuffer == null || vbuffer.Length < this.verts.Length)
			{
				vbuffer = new global::Pathfinding.Int3[this.verts.Length];
			}
			tbuffer = this.tris;
			if (this.useRotation)
			{
				global::UnityEngine.Matrix4x4 matrix4x = global::UnityEngine.Matrix4x4.TRS(this.tr.position + this.center, this.tr.rotation, this.tr.localScale * this.meshScale);
				for (int i = 0; i < this.verts.Length; i++)
				{
					vbuffer[i] = offset + (global::Pathfinding.Int3)matrix4x.MultiplyPoint3x4(this.verts[i]);
				}
			}
			else
			{
				global::UnityEngine.Vector3 a = this.tr.position + this.center;
				for (int j = 0; j < this.verts.Length; j++)
				{
					vbuffer[j] = offset + (global::Pathfinding.Int3)(a + this.verts[j] * this.meshScale);
				}
			}
		}

		private static global::System.Collections.Generic.List<global::Pathfinding.NavmeshAdd> allCuts = new global::System.Collections.Generic.List<global::Pathfinding.NavmeshAdd>();

		public global::Pathfinding.NavmeshAdd.MeshType type;

		public global::UnityEngine.Mesh mesh;

		private global::UnityEngine.Vector3[] verts;

		private int[] tris;

		public global::UnityEngine.Vector2 rectangleSize = new global::UnityEngine.Vector2(1f, 1f);

		public float meshScale = 1f;

		public global::UnityEngine.Vector3 center;

		private global::UnityEngine.Bounds bounds;

		public bool useRotation;

		protected global::UnityEngine.Transform tr;

		public static readonly global::UnityEngine.Color GizmoColor = new global::UnityEngine.Color(0.368627459f, 0.9372549f, 0.145098045f);

		public enum MeshType
		{
			Rectangle,
			CustomMesh
		}
	}
}
