using System;
using System.Diagnostics;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class BBTree : global::Pathfinding.Util.IAstarPooledObject
	{
		public global::UnityEngine.Rect Size
		{
			get
			{
				if (this.count == 0)
				{
					return new global::UnityEngine.Rect(0f, 0f, 0f, 0f);
				}
				global::Pathfinding.IntRect rect = this.arr[0].rect;
				return global::UnityEngine.Rect.MinMaxRect((float)rect.xmin * 0.001f, (float)rect.ymin * 0.001f, (float)rect.xmax * 0.001f, (float)rect.ymax * 0.001f);
			}
		}

		public void Clear()
		{
			this.count = 0;
		}

		public void OnEnterPool()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i].node = null;
			}
			this.Clear();
		}

		private void EnsureCapacity(int c)
		{
			if (this.arr.Length < c)
			{
				global::Pathfinding.BBTree.BBTreeBox[] array = new global::Pathfinding.BBTree.BBTreeBox[global::System.Math.Max(c, (int)((float)this.arr.Length * 2f))];
				for (int i = 0; i < this.count; i++)
				{
					array[i] = this.arr[i];
				}
				this.arr = array;
			}
		}

		private int GetBox(global::Pathfinding.MeshNode node)
		{
			if (this.count >= this.arr.Length)
			{
				this.EnsureCapacity(this.count + 1);
			}
			this.arr[this.count] = new global::Pathfinding.BBTree.BBTreeBox(node);
			this.count++;
			return this.count - 1;
		}

		private int GetBox(global::Pathfinding.IntRect rect)
		{
			if (this.count >= this.arr.Length)
			{
				this.EnsureCapacity(this.count + 1);
			}
			this.arr[this.count] = new global::Pathfinding.BBTree.BBTreeBox(rect);
			this.count++;
			return this.count - 1;
		}

		public void RebuildFrom(global::Pathfinding.MeshNode[] nodes)
		{
			this.Clear();
			if (nodes.Length == 0)
			{
				return;
			}
			if (nodes.Length == 1)
			{
				this.GetBox(nodes[0]);
				return;
			}
			this.EnsureCapacity(global::UnityEngine.Mathf.CeilToInt((float)nodes.Length * 2.1f));
			global::Pathfinding.MeshNode[] array = new global::Pathfinding.MeshNode[nodes.Length];
			for (int i = 0; i < nodes.Length; i++)
			{
				array[i] = nodes[i];
			}
			this.RebuildFromInternal(array, 0, nodes.Length, false);
		}

		private static int SplitByX(global::Pathfinding.MeshNode[] nodes, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				if (nodes[i].position.x > divider)
				{
					num--;
					global::Pathfinding.MeshNode meshNode = nodes[num];
					nodes[num] = nodes[i];
					nodes[i] = meshNode;
					i--;
				}
			}
			return num;
		}

		private static int SplitByZ(global::Pathfinding.MeshNode[] nodes, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				if (nodes[i].position.z > divider)
				{
					num--;
					global::Pathfinding.MeshNode meshNode = nodes[num];
					nodes[num] = nodes[i];
					nodes[i] = meshNode;
					i--;
				}
			}
			return num;
		}

		private int RebuildFromInternal(global::Pathfinding.MeshNode[] nodes, int from, int to, bool odd)
		{
			if (to - from <= 0)
			{
				throw new global::System.ArgumentException();
			}
			if (to - from == 1)
			{
				return this.GetBox(nodes[from]);
			}
			global::Pathfinding.IntRect rect = global::Pathfinding.BBTree.NodeBounds(nodes, from, to);
			int box = this.GetBox(rect);
			if (to - from == 2)
			{
				this.arr[box].left = this.GetBox(nodes[from]);
				this.arr[box].right = this.GetBox(nodes[from + 1]);
				return box;
			}
			int num;
			if (odd)
			{
				int divider = (rect.xmin + rect.xmax) / 2;
				num = global::Pathfinding.BBTree.SplitByX(nodes, from, to, divider);
			}
			else
			{
				int divider2 = (rect.ymin + rect.ymax) / 2;
				num = global::Pathfinding.BBTree.SplitByZ(nodes, from, to, divider2);
			}
			if (num == from || num == to)
			{
				if (!odd)
				{
					int divider3 = (rect.xmin + rect.xmax) / 2;
					num = global::Pathfinding.BBTree.SplitByX(nodes, from, to, divider3);
				}
				else
				{
					int divider4 = (rect.ymin + rect.ymax) / 2;
					num = global::Pathfinding.BBTree.SplitByZ(nodes, from, to, divider4);
				}
				if (num == from || num == to)
				{
					num = (from + to) / 2;
				}
			}
			this.arr[box].left = this.RebuildFromInternal(nodes, from, num, !odd);
			this.arr[box].right = this.RebuildFromInternal(nodes, num, to, !odd);
			return box;
		}

		private static global::Pathfinding.IntRect NodeBounds(global::Pathfinding.MeshNode[] nodes, int from, int to)
		{
			if (to - from <= 0)
			{
				throw new global::System.ArgumentException();
			}
			global::Pathfinding.Int3 vertex = nodes[from].GetVertex(0);
			global::Pathfinding.Int2 @int = new global::Pathfinding.Int2(vertex.x, vertex.z);
			global::Pathfinding.Int2 int2 = @int;
			for (int i = from; i < to; i++)
			{
				global::Pathfinding.MeshNode meshNode = nodes[i];
				int vertexCount = meshNode.GetVertexCount();
				for (int j = 0; j < vertexCount; j++)
				{
					global::Pathfinding.Int3 vertex2 = meshNode.GetVertex(j);
					@int.x = global::System.Math.Min(@int.x, vertex2.x);
					@int.y = global::System.Math.Min(@int.y, vertex2.z);
					int2.x = global::System.Math.Max(int2.x, vertex2.x);
					int2.y = global::System.Math.Max(int2.y, vertex2.z);
				}
			}
			return new global::Pathfinding.IntRect(@int.x, @int.y, int2.x, int2.y);
		}

		[global::System.Diagnostics.Conditional("ASTARDEBUG")]
		private static void DrawDebugRect(global::Pathfinding.IntRect rect)
		{
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3((float)rect.xmin, 0f, (float)rect.ymin), new global::UnityEngine.Vector3((float)rect.xmax, 0f, (float)rect.ymin), global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3((float)rect.xmin, 0f, (float)rect.ymax), new global::UnityEngine.Vector3((float)rect.xmax, 0f, (float)rect.ymax), global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3((float)rect.xmin, 0f, (float)rect.ymin), new global::UnityEngine.Vector3((float)rect.xmin, 0f, (float)rect.ymax), global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3((float)rect.xmax, 0f, (float)rect.ymin), new global::UnityEngine.Vector3((float)rect.xmax, 0f, (float)rect.ymax), global::UnityEngine.Color.white);
		}

		[global::System.Diagnostics.Conditional("ASTARDEBUG")]
		private static void DrawDebugNode(global::Pathfinding.MeshNode node, float yoffset, global::UnityEngine.Color color)
		{
			global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)node.GetVertex(1) + global::UnityEngine.Vector3.up * yoffset, (global::UnityEngine.Vector3)node.GetVertex(2) + global::UnityEngine.Vector3.up * yoffset, color);
			global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)node.GetVertex(0) + global::UnityEngine.Vector3.up * yoffset, (global::UnityEngine.Vector3)node.GetVertex(1) + global::UnityEngine.Vector3.up * yoffset, color);
			global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)node.GetVertex(2) + global::UnityEngine.Vector3.up * yoffset, (global::UnityEngine.Vector3)node.GetVertex(0) + global::UnityEngine.Vector3.up * yoffset, color);
		}

		public void Insert(global::Pathfinding.MeshNode node)
		{
			int box = this.GetBox(node);
			if (box == 0)
			{
				return;
			}
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[box];
			int num = 0;
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox2;
			for (;;)
			{
				bbtreeBox2 = this.arr[num];
				bbtreeBox2.rect = global::Pathfinding.BBTree.ExpandToContain(bbtreeBox2.rect, bbtreeBox.rect);
				if (bbtreeBox2.node != null)
				{
					break;
				}
				this.arr[num] = bbtreeBox2;
				int num2 = global::Pathfinding.BBTree.ExpansionRequired(this.arr[bbtreeBox2.left].rect, bbtreeBox.rect);
				int num3 = global::Pathfinding.BBTree.ExpansionRequired(this.arr[bbtreeBox2.right].rect, bbtreeBox.rect);
				if (num2 < num3)
				{
					num = bbtreeBox2.left;
				}
				else if (num3 < num2)
				{
					num = bbtreeBox2.right;
				}
				else
				{
					num = ((global::Pathfinding.BBTree.RectArea(this.arr[bbtreeBox2.left].rect) >= global::Pathfinding.BBTree.RectArea(this.arr[bbtreeBox2.right].rect)) ? bbtreeBox2.right : bbtreeBox2.left);
				}
			}
			bbtreeBox2.left = box;
			int box2 = this.GetBox(bbtreeBox2.node);
			bbtreeBox2.right = box2;
			bbtreeBox2.node = null;
			this.arr[num] = bbtreeBox2;
		}

		public global::Pathfinding.NNInfoInternal Query(global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint)
		{
			if (this.count == 0)
			{
				return new global::Pathfinding.NNInfoInternal(null);
			}
			global::Pathfinding.NNInfoInternal result = default(global::Pathfinding.NNInfoInternal);
			this.SearchBox(0, p, constraint, ref result);
			result.UpdateInfo();
			return result;
		}

		public global::Pathfinding.NNInfoInternal QueryCircle(global::UnityEngine.Vector3 p, float radius, global::Pathfinding.NNConstraint constraint)
		{
			if (this.count == 0)
			{
				return new global::Pathfinding.NNInfoInternal(null);
			}
			global::Pathfinding.NNInfoInternal result = new global::Pathfinding.NNInfoInternal(null);
			this.SearchBoxCircle(0, p, radius, constraint, ref result);
			result.UpdateInfo();
			return result;
		}

		public global::Pathfinding.NNInfoInternal QueryClosest(global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint, out float distance)
		{
			distance = float.PositiveInfinity;
			return this.QueryClosest(p, constraint, ref distance, new global::Pathfinding.NNInfoInternal(null));
		}

		public global::Pathfinding.NNInfoInternal QueryClosestXZ(global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint, ref float distance, global::Pathfinding.NNInfoInternal previous)
		{
			if (this.count == 0)
			{
				return previous;
			}
			this.SearchBoxClosestXZ(0, p, ref distance, constraint, ref previous);
			return previous;
		}

		private void SearchBoxClosestXZ(int boxi, global::UnityEngine.Vector3 p, ref float closestDist, global::Pathfinding.NNConstraint constraint, ref global::Pathfinding.NNInfoInternal nnInfo)
		{
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				global::UnityEngine.Vector3 constClampedPosition = bbtreeBox.node.ClosestPointOnNodeXZ(p);
				if (constraint == null || constraint.Suitable(bbtreeBox.node))
				{
					float num = (constClampedPosition.x - p.x) * (constClampedPosition.x - p.x) + (constClampedPosition.z - p.z) * (constClampedPosition.z - p.z);
					if (nnInfo.constrainedNode == null || num < closestDist * closestDist)
					{
						nnInfo.constrainedNode = bbtreeBox.node;
						nnInfo.constClampedPosition = constClampedPosition;
						closestDist = (float)global::System.Math.Sqrt((double)num);
					}
				}
			}
			else
			{
				if (global::Pathfinding.BBTree.RectIntersectsCircle(this.arr[bbtreeBox.left].rect, p, closestDist))
				{
					this.SearchBoxClosestXZ(bbtreeBox.left, p, ref closestDist, constraint, ref nnInfo);
				}
				if (global::Pathfinding.BBTree.RectIntersectsCircle(this.arr[bbtreeBox.right].rect, p, closestDist))
				{
					this.SearchBoxClosestXZ(bbtreeBox.right, p, ref closestDist, constraint, ref nnInfo);
				}
			}
		}

		public global::Pathfinding.NNInfoInternal QueryClosest(global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint, ref float distance, global::Pathfinding.NNInfoInternal previous)
		{
			if (this.count == 0)
			{
				return previous;
			}
			this.SearchBoxClosest(0, p, ref distance, constraint, ref previous);
			return previous;
		}

		private void SearchBoxClosest(int boxi, global::UnityEngine.Vector3 p, ref float closestDist, global::Pathfinding.NNConstraint constraint, ref global::Pathfinding.NNInfoInternal nnInfo)
		{
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (global::Pathfinding.BBTree.NodeIntersectsCircle(bbtreeBox.node, p, closestDist))
				{
					global::UnityEngine.Vector3 vector = bbtreeBox.node.ClosestPointOnNode(p);
					if (constraint == null || constraint.Suitable(bbtreeBox.node))
					{
						float sqrMagnitude = (vector - p).sqrMagnitude;
						if (nnInfo.constrainedNode == null || sqrMagnitude < closestDist * closestDist)
						{
							nnInfo.constrainedNode = bbtreeBox.node;
							nnInfo.constClampedPosition = vector;
							closestDist = (float)global::System.Math.Sqrt((double)sqrMagnitude);
						}
					}
				}
			}
			else
			{
				if (global::Pathfinding.BBTree.RectIntersectsCircle(this.arr[bbtreeBox.left].rect, p, closestDist))
				{
					this.SearchBoxClosest(bbtreeBox.left, p, ref closestDist, constraint, ref nnInfo);
				}
				if (global::Pathfinding.BBTree.RectIntersectsCircle(this.arr[bbtreeBox.right].rect, p, closestDist))
				{
					this.SearchBoxClosest(bbtreeBox.right, p, ref closestDist, constraint, ref nnInfo);
				}
			}
		}

		public global::Pathfinding.MeshNode QueryInside(global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint)
		{
			return (this.count == 0) ? null : this.SearchBoxInside(0, p, constraint);
		}

		private global::Pathfinding.MeshNode SearchBoxInside(int boxi, global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint)
		{
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (bbtreeBox.node.ContainsPoint((global::Pathfinding.Int3)p))
				{
					if (constraint == null || constraint.Suitable(bbtreeBox.node))
					{
						return bbtreeBox.node;
					}
				}
			}
			else
			{
				if (this.arr[bbtreeBox.left].Contains(p))
				{
					global::Pathfinding.MeshNode meshNode = this.SearchBoxInside(bbtreeBox.left, p, constraint);
					if (meshNode != null)
					{
						return meshNode;
					}
				}
				if (this.arr[bbtreeBox.right].Contains(p))
				{
					global::Pathfinding.MeshNode meshNode = this.SearchBoxInside(bbtreeBox.right, p, constraint);
					if (meshNode != null)
					{
						return meshNode;
					}
				}
			}
			return null;
		}

		private void SearchBoxCircle(int boxi, global::UnityEngine.Vector3 p, float radius, global::Pathfinding.NNConstraint constraint, ref global::Pathfinding.NNInfoInternal nnInfo)
		{
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (global::Pathfinding.BBTree.NodeIntersectsCircle(bbtreeBox.node, p, radius))
				{
					global::UnityEngine.Vector3 vector = bbtreeBox.node.ClosestPointOnNode(p);
					float sqrMagnitude = (vector - p).sqrMagnitude;
					if (nnInfo.node == null)
					{
						nnInfo.node = bbtreeBox.node;
						nnInfo.clampedPosition = vector;
					}
					else if (sqrMagnitude < (nnInfo.clampedPosition - p).sqrMagnitude)
					{
						nnInfo.node = bbtreeBox.node;
						nnInfo.clampedPosition = vector;
					}
					if ((constraint == null || constraint.Suitable(bbtreeBox.node)) && (nnInfo.constrainedNode == null || sqrMagnitude < (nnInfo.constClampedPosition - p).sqrMagnitude))
					{
						nnInfo.constrainedNode = bbtreeBox.node;
						nnInfo.constClampedPosition = vector;
					}
				}
				return;
			}
			if (global::Pathfinding.BBTree.RectIntersectsCircle(this.arr[bbtreeBox.left].rect, p, radius))
			{
				this.SearchBoxCircle(bbtreeBox.left, p, radius, constraint, ref nnInfo);
			}
			if (global::Pathfinding.BBTree.RectIntersectsCircle(this.arr[bbtreeBox.right].rect, p, radius))
			{
				this.SearchBoxCircle(bbtreeBox.right, p, radius, constraint, ref nnInfo);
			}
		}

		private void SearchBox(int boxi, global::UnityEngine.Vector3 p, global::Pathfinding.NNConstraint constraint, ref global::Pathfinding.NNInfoInternal nnInfo)
		{
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (bbtreeBox.node.ContainsPoint((global::Pathfinding.Int3)p))
				{
					if (nnInfo.node == null)
					{
						nnInfo.node = bbtreeBox.node;
					}
					else if (global::UnityEngine.Mathf.Abs(((global::UnityEngine.Vector3)bbtreeBox.node.position).y - p.y) < global::UnityEngine.Mathf.Abs(((global::UnityEngine.Vector3)nnInfo.node.position).y - p.y))
					{
						nnInfo.node = bbtreeBox.node;
					}
					if (constraint.Suitable(bbtreeBox.node) && (nnInfo.constrainedNode == null || global::UnityEngine.Mathf.Abs((float)bbtreeBox.node.position.y - p.y) < global::UnityEngine.Mathf.Abs((float)nnInfo.constrainedNode.position.y - p.y)))
					{
						nnInfo.constrainedNode = bbtreeBox.node;
					}
				}
				return;
			}
			if (this.arr[bbtreeBox.left].Contains(p))
			{
				this.SearchBox(bbtreeBox.left, p, constraint, ref nnInfo);
			}
			if (this.arr[bbtreeBox.right].Contains(p))
			{
				this.SearchBox(bbtreeBox.right, p, constraint, ref nnInfo);
			}
		}

		public void OnDrawGizmos()
		{
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(1f, 1f, 1f, 0.5f);
			if (this.count == 0)
			{
				return;
			}
			this.OnDrawGizmos(0, 0);
		}

		private void OnDrawGizmos(int boxi, int depth)
		{
			global::Pathfinding.BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			global::UnityEngine.Vector3 a = (global::UnityEngine.Vector3)new global::Pathfinding.Int3(bbtreeBox.rect.xmin, 0, bbtreeBox.rect.ymin);
			global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)new global::Pathfinding.Int3(bbtreeBox.rect.xmax, 0, bbtreeBox.rect.ymax);
			global::UnityEngine.Vector3 vector2 = (a + vector) * 0.5f;
			global::UnityEngine.Vector3 size = (vector - vector2) * 2f;
			size = new global::UnityEngine.Vector3(size.x, 1f, size.z);
			vector2.y += (float)(depth * 2);
			global::UnityEngine.Gizmos.color = global::Pathfinding.AstarMath.IntToColor(depth, 1f);
			global::UnityEngine.Gizmos.DrawCube(vector2, size);
			if (bbtreeBox.node == null)
			{
				this.OnDrawGizmos(bbtreeBox.left, depth + 1);
				this.OnDrawGizmos(bbtreeBox.right, depth + 1);
			}
		}

		private static bool NodeIntersectsCircle(global::Pathfinding.MeshNode node, global::UnityEngine.Vector3 p, float radius)
		{
			return float.IsPositiveInfinity(radius) || (p - node.ClosestPointOnNode(p)).sqrMagnitude < radius * radius;
		}

		private static bool RectIntersectsCircle(global::Pathfinding.IntRect r, global::UnityEngine.Vector3 p, float radius)
		{
			if (float.IsPositiveInfinity(radius))
			{
				return true;
			}
			global::UnityEngine.Vector3 vector = p;
			p.x = global::System.Math.Max(p.x, (float)r.xmin * 0.001f);
			p.x = global::System.Math.Min(p.x, (float)r.xmax * 0.001f);
			p.z = global::System.Math.Max(p.z, (float)r.ymin * 0.001f);
			p.z = global::System.Math.Min(p.z, (float)r.ymax * 0.001f);
			return (p.x - vector.x) * (p.x - vector.x) + (p.z - vector.z) * (p.z - vector.z) < radius * radius;
		}

		private static int ExpansionRequired(global::Pathfinding.IntRect r, global::Pathfinding.IntRect r2)
		{
			int num = global::System.Math.Min(r.xmin, r2.xmin);
			int num2 = global::System.Math.Max(r.xmax, r2.xmax);
			int num3 = global::System.Math.Min(r.ymin, r2.ymin);
			int num4 = global::System.Math.Max(r.ymax, r2.ymax);
			return (num2 - num) * (num4 - num3) - global::Pathfinding.BBTree.RectArea(r);
		}

		private static global::Pathfinding.IntRect ExpandToContain(global::Pathfinding.IntRect r, global::Pathfinding.IntRect r2)
		{
			return global::Pathfinding.IntRect.Union(r, r2);
		}

		private static int RectArea(global::Pathfinding.IntRect r)
		{
			return r.Width * r.Height;
		}

		private global::Pathfinding.BBTree.BBTreeBox[] arr = new global::Pathfinding.BBTree.BBTreeBox[6];

		private int count;

		private struct BBTreeBox
		{
			public BBTreeBox(global::Pathfinding.IntRect rect)
			{
				this.node = null;
				this.rect = rect;
				this.left = (this.right = -1);
			}

			public BBTreeBox(global::Pathfinding.MeshNode node)
			{
				this.node = node;
				global::Pathfinding.Int3 vertex = node.GetVertex(0);
				global::Pathfinding.Int2 @int = new global::Pathfinding.Int2(vertex.x, vertex.z);
				global::Pathfinding.Int2 int2 = @int;
				for (int i = 1; i < node.GetVertexCount(); i++)
				{
					global::Pathfinding.Int3 vertex2 = node.GetVertex(i);
					@int.x = global::System.Math.Min(@int.x, vertex2.x);
					@int.y = global::System.Math.Min(@int.y, vertex2.z);
					int2.x = global::System.Math.Max(int2.x, vertex2.x);
					int2.y = global::System.Math.Max(int2.y, vertex2.z);
				}
				this.rect = new global::Pathfinding.IntRect(@int.x, @int.y, int2.x, int2.y);
				this.left = (this.right = -1);
			}

			public bool IsLeaf
			{
				get
				{
					return this.node != null;
				}
			}

			public bool Contains(global::UnityEngine.Vector3 p)
			{
				global::Pathfinding.Int3 @int = (global::Pathfinding.Int3)p;
				return this.rect.Contains(@int.x, @int.z);
			}

			public global::Pathfinding.IntRect rect;

			public global::Pathfinding.MeshNode node;

			public int left;

			public int right;
		}
	}
}
