using System;
using System.Collections.Generic;
using Pathfinding.ClipperLib;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_cut.php")]
	public class NavmeshCut : global::UnityEngine.MonoBehaviour
	{
		public static event global::System.Action<global::Pathfinding.NavmeshCut> OnDestroyCallback;

		private static void AddCut(global::Pathfinding.NavmeshCut obj)
		{
			global::Pathfinding.NavmeshCut.allCuts.Add(obj);
		}

		private static void RemoveCut(global::Pathfinding.NavmeshCut obj)
		{
			global::Pathfinding.NavmeshCut.allCuts.Remove(obj);
		}

		public static global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> GetAllInRange(global::UnityEngine.Bounds b)
		{
			global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> list = global::Pathfinding.Util.ListPool<global::Pathfinding.NavmeshCut>.Claim();
			for (int i = 0; i < global::Pathfinding.NavmeshCut.allCuts.Count; i++)
			{
				if (global::Pathfinding.NavmeshCut.allCuts[i].enabled && global::Pathfinding.NavmeshCut.Intersects(b, global::Pathfinding.NavmeshCut.allCuts[i].GetBounds()))
				{
					list.Add(global::Pathfinding.NavmeshCut.allCuts[i]);
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
			return min.x <= max2.x && max.x >= min2.x && min.y <= max2.y && max.y >= min2.y && min.z <= max2.z && max.z >= min2.z;
		}

		public static global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> GetAll()
		{
			return global::Pathfinding.NavmeshCut.allCuts;
		}

		public global::UnityEngine.Bounds LastBounds
		{
			get
			{
				return this.lastBounds;
			}
		}

		public void Awake()
		{
			this.tr = base.transform;
			global::Pathfinding.NavmeshCut.AddCut(this);
		}

		public void OnEnable()
		{
			this.lastPosition = new global::UnityEngine.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			this.lastRotation = this.tr.rotation;
		}

		public void OnDestroy()
		{
			if (global::Pathfinding.NavmeshCut.OnDestroyCallback != null)
			{
				global::Pathfinding.NavmeshCut.OnDestroyCallback(this);
			}
			global::Pathfinding.NavmeshCut.RemoveCut(this);
		}

		public void ForceUpdate()
		{
			this.lastPosition = new global::UnityEngine.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		public void ForceContourRefresh()
		{
			this.contours = null;
		}

		public bool RequiresUpdate()
		{
			return this.wasEnabled != base.enabled || (this.wasEnabled && ((this.tr.position - this.lastPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotation && global::UnityEngine.Quaternion.Angle(this.lastRotation, this.tr.rotation) > this.updateRotationDistance)));
		}

		public virtual void UsedForCut()
		{
		}

		public void NotifyUpdated()
		{
			this.wasEnabled = base.enabled;
			if (this.wasEnabled)
			{
				this.lastPosition = this.tr.position;
				this.lastBounds = this.GetBounds();
				if (this.useRotation)
				{
					this.lastRotation = this.tr.rotation;
				}
			}
		}

		private void CalculateMeshContour()
		{
			if (this.mesh == null)
			{
				return;
			}
			global::Pathfinding.NavmeshCut.edges.Clear();
			global::Pathfinding.NavmeshCut.pointers.Clear();
			global::UnityEngine.Vector3[] vertices = this.mesh.vertices;
			int[] triangles = this.mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (global::Pathfinding.VectorMath.IsClockwiseXZ(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
				global::Pathfinding.NavmeshCut.edges[new global::Pathfinding.Int2(triangles[i], triangles[i + 1])] = i;
				global::Pathfinding.NavmeshCut.edges[new global::Pathfinding.Int2(triangles[i + 1], triangles[i + 2])] = i;
				global::Pathfinding.NavmeshCut.edges[new global::Pathfinding.Int2(triangles[i + 2], triangles[i])] = i;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					if (!global::Pathfinding.NavmeshCut.edges.ContainsKey(new global::Pathfinding.Int2(triangles[j + (k + 1) % 3], triangles[j + k % 3])))
					{
						global::Pathfinding.NavmeshCut.pointers[triangles[j + k % 3]] = triangles[j + (k + 1) % 3];
					}
				}
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3[]> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3[]>();
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			for (int l = 0; l < vertices.Length; l++)
			{
				if (global::Pathfinding.NavmeshCut.pointers.ContainsKey(l))
				{
					list2.Clear();
					int num2 = l;
					do
					{
						int num3 = global::Pathfinding.NavmeshCut.pointers[num2];
						if (num3 == -1)
						{
							break;
						}
						global::Pathfinding.NavmeshCut.pointers[num2] = -1;
						list2.Add(vertices[num2]);
						num2 = num3;
						if (num2 == -1)
						{
							goto Block_9;
						}
					}
					while (num2 != l);
					IL_20C:
					if (list2.Count > 0)
					{
						list.Add(list2.ToArray());
						goto IL_227;
					}
					goto IL_227;
					Block_9:
					global::UnityEngine.Debug.LogError("Invalid Mesh '" + this.mesh.name + " in " + base.gameObject.name);
					goto IL_20C;
				}
				IL_227:;
			}
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
			this.contours = list.ToArray();
		}

		public global::UnityEngine.Bounds GetBounds()
		{
			global::UnityEngine.Bounds result;
			switch (this.type)
			{
			case global::Pathfinding.NavmeshCut.MeshType.Rectangle:
				if (this.useRotation)
				{
					global::UnityEngine.Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
					result = new global::UnityEngine.Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f), global::UnityEngine.Vector3.zero);
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f));
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
					result.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
				}
				else
				{
					result = new global::UnityEngine.Bounds(this.tr.position + this.center, new global::UnityEngine.Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y));
				}
				break;
			case global::Pathfinding.NavmeshCut.MeshType.Circle:
				if (this.useRotation)
				{
					result = new global::UnityEngine.Bounds(this.tr.localToWorldMatrix.MultiplyPoint3x4(this.center), new global::UnityEngine.Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
				}
				else
				{
					result = new global::UnityEngine.Bounds(base.transform.position + this.center, new global::UnityEngine.Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
				}
				break;
			case global::Pathfinding.NavmeshCut.MeshType.CustomMesh:
				if (this.mesh == null)
				{
					result = default(global::UnityEngine.Bounds);
				}
				else
				{
					global::UnityEngine.Bounds bounds = this.mesh.bounds;
					if (this.useRotation)
					{
						global::UnityEngine.Matrix4x4 localToWorldMatrix2 = this.tr.localToWorldMatrix;
						bounds.center *= this.meshScale;
						bounds.size *= this.meshScale;
						result = new global::UnityEngine.Bounds(localToWorldMatrix2.MultiplyPoint3x4(this.center + bounds.center), global::UnityEngine.Vector3.zero);
						global::UnityEngine.Vector3 max = bounds.max;
						global::UnityEngine.Vector3 min = bounds.min;
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(max.x, max.y, max.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(min.x, max.y, max.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(min.x, max.y, min.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(max.x, max.y, min.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(max.x, min.y, max.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(min.x, min.y, max.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(min.x, min.y, min.z)));
						result.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(max.x, min.y, min.z)));
						global::UnityEngine.Vector3 size = result.size;
						size.y = global::UnityEngine.Mathf.Max(size.y, this.height * this.tr.lossyScale.y);
						result.size = size;
					}
					else
					{
						global::UnityEngine.Vector3 size2 = bounds.size * this.meshScale;
						size2.y = global::UnityEngine.Mathf.Max(size2.y, this.height);
						result = new global::UnityEngine.Bounds(base.transform.position + this.center + bounds.center * this.meshScale, size2);
					}
				}
				break;
			default:
				throw new global::System.Exception("Invalid mesh type");
			}
			return result;
		}

		public void GetContour(global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint>> buffer)
		{
			if (this.circleResolution < 3)
			{
				this.circleResolution = 3;
			}
			global::UnityEngine.Vector3 a = this.tr.position;
			global::UnityEngine.Matrix4x4 matrix = global::UnityEngine.Matrix4x4.identity;
			bool flag = false;
			if (this.useRotation)
			{
				matrix = this.tr.localToWorldMatrix;
				flag = global::Pathfinding.VectorMath.ReversesFaceOrientationsXZ(matrix);
			}
			switch (this.type)
			{
			case global::Pathfinding.NavmeshCut.MeshType.Rectangle:
			{
				global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint> list = global::Pathfinding.Util.ListPool<global::Pathfinding.ClipperLib.IntPoint>.Claim();
				flag ^= (this.rectangleSize.x < 0f ^ this.rectangleSize.y < 0f);
				if (this.useRotation)
				{
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f)));
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f)));
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f)));
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f)));
				}
				else
				{
					a += this.center;
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(a + new global::UnityEngine.Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f));
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(a + new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f));
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(a + new global::UnityEngine.Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f));
					list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(a + new global::UnityEngine.Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f));
				}
				if (flag)
				{
					list.Reverse();
				}
				buffer.Add(list);
				break;
			}
			case global::Pathfinding.NavmeshCut.MeshType.Circle:
			{
				global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint> list = global::Pathfinding.Util.ListPool<global::Pathfinding.ClipperLib.IntPoint>.Claim(this.circleResolution);
				flag ^= (this.circleRadius < 0f);
				if (this.useRotation)
				{
					for (int i = 0; i < this.circleResolution; i++)
					{
						list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos((float)(i * 2) * 3.14159274f / (float)this.circleResolution), 0f, global::UnityEngine.Mathf.Sin((float)(i * 2) * 3.14159274f / (float)this.circleResolution)) * this.circleRadius)));
					}
				}
				else
				{
					a += this.center;
					for (int j = 0; j < this.circleResolution; j++)
					{
						list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(a + new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos((float)(j * 2) * 3.14159274f / (float)this.circleResolution), 0f, global::UnityEngine.Mathf.Sin((float)(j * 2) * 3.14159274f / (float)this.circleResolution)) * this.circleRadius));
					}
				}
				if (flag)
				{
					list.Reverse();
				}
				buffer.Add(list);
				break;
			}
			case global::Pathfinding.NavmeshCut.MeshType.CustomMesh:
				if (this.mesh != this.lastMesh || this.contours == null)
				{
					this.CalculateMeshContour();
					this.lastMesh = this.mesh;
				}
				if (this.contours != null)
				{
					a += this.center;
					flag ^= (this.meshScale < 0f);
					for (int k = 0; k < this.contours.Length; k++)
					{
						global::UnityEngine.Vector3[] array = this.contours[k];
						global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint> list = global::Pathfinding.Util.ListPool<global::Pathfinding.ClipperLib.IntPoint>.Claim(array.Length);
						if (this.useRotation)
						{
							for (int l = 0; l < array.Length; l++)
							{
								list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + array[l] * this.meshScale)));
							}
						}
						else
						{
							for (int m = 0; m < array.Length; m++)
							{
								list.Add(global::Pathfinding.NavmeshCut.V3ToIntPoint(a + array[m] * this.meshScale));
							}
						}
						if (flag)
						{
							list.Reverse();
						}
						buffer.Add(list);
					}
				}
				break;
			}
		}

		public static global::Pathfinding.ClipperLib.IntPoint V3ToIntPoint(global::UnityEngine.Vector3 p)
		{
			global::Pathfinding.Int3 @int = (global::Pathfinding.Int3)p;
			return new global::Pathfinding.ClipperLib.IntPoint((long)@int.x, (long)@int.z);
		}

		public static global::UnityEngine.Vector3 IntPointToV3(global::Pathfinding.ClipperLib.IntPoint p)
		{
			global::Pathfinding.Int3 ob = new global::Pathfinding.Int3((int)p.X, 0, (int)p.Y);
			return (global::UnityEngine.Vector3)ob;
		}

		public void OnDrawGizmos()
		{
			if (this.tr == null)
			{
				this.tr = base.transform;
			}
			global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint>> list = global::Pathfinding.Util.ListPool<global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint>>.Claim();
			this.GetContour(list);
			global::UnityEngine.Gizmos.color = global::Pathfinding.NavmeshCut.GizmoColor;
			global::UnityEngine.Bounds bounds = this.GetBounds();
			float y = bounds.min.y;
			global::UnityEngine.Vector3 b = global::UnityEngine.Vector3.up * (bounds.max.y - y);
			for (int i = 0; i < list.Count; i++)
			{
				global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					global::UnityEngine.Vector3 vector = global::Pathfinding.NavmeshCut.IntPointToV3(list2[j]);
					vector.y = y;
					global::UnityEngine.Vector3 vector2 = global::Pathfinding.NavmeshCut.IntPointToV3(list2[(j + 1) % list2.Count]);
					vector2.y = y;
					global::UnityEngine.Gizmos.DrawLine(vector, vector2);
					global::UnityEngine.Gizmos.DrawLine(vector + b, vector2 + b);
					global::UnityEngine.Gizmos.DrawLine(vector, vector + b);
					global::UnityEngine.Gizmos.DrawLine(vector2, vector2 + b);
				}
			}
			global::Pathfinding.Util.ListPool<global::System.Collections.Generic.List<global::Pathfinding.ClipperLib.IntPoint>>.Release(list);
		}

		public void OnDrawGizmosSelected()
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.Lerp(global::Pathfinding.NavmeshCut.GizmoColor, new global::UnityEngine.Color(1f, 1f, 1f, 0.2f), 0.9f);
			global::UnityEngine.Bounds bounds = this.GetBounds();
			global::UnityEngine.Gizmos.DrawCube(bounds.center, bounds.size);
			global::UnityEngine.Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		private static global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut> allCuts = new global::System.Collections.Generic.List<global::Pathfinding.NavmeshCut>();

		[global::UnityEngine.Tooltip("Shape of the cut")]
		public global::Pathfinding.NavmeshCut.MeshType type;

		[global::UnityEngine.Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
		public global::UnityEngine.Mesh mesh;

		public global::UnityEngine.Vector2 rectangleSize = new global::UnityEngine.Vector2(1f, 1f);

		public float circleRadius = 1f;

		public int circleResolution = 6;

		public float height = 1f;

		[global::UnityEngine.Tooltip("Scale of the custom mesh")]
		public float meshScale = 1f;

		public global::UnityEngine.Vector3 center;

		[global::UnityEngine.Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		[global::UnityEngine.Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
		public bool isDual;

		public bool cutsAddedGeom = true;

		[global::UnityEngine.Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		[global::UnityEngine.Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
		public bool useRotation;

		private global::UnityEngine.Vector3[][] contours;

		protected global::UnityEngine.Transform tr;

		private global::UnityEngine.Mesh lastMesh;

		private global::UnityEngine.Vector3 lastPosition;

		private global::UnityEngine.Quaternion lastRotation;

		private bool wasEnabled;

		private global::UnityEngine.Bounds lastBounds;

		private static readonly global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, int> edges = new global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, int>();

		private static readonly global::System.Collections.Generic.Dictionary<int, int> pointers = new global::System.Collections.Generic.Dictionary<int, int>();

		public static readonly global::UnityEngine.Color GizmoColor = new global::UnityEngine.Color(0.145098045f, 0.721568644f, 0.9372549f);

		public enum MeshType
		{
			Rectangle,
			Circle,
			CustomMesh
		}
	}
}
