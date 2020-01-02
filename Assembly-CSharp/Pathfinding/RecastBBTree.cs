using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class RecastBBTree
	{
		public void QueryInBounds(global::UnityEngine.Rect bounds, global::System.Collections.Generic.List<global::Pathfinding.RecastMeshObj> buffer)
		{
			if (this.root == null)
			{
				return;
			}
			this.QueryBoxInBounds(this.root, bounds, buffer);
		}

		private void QueryBoxInBounds(global::Pathfinding.RecastBBTreeBox box, global::UnityEngine.Rect bounds, global::System.Collections.Generic.List<global::Pathfinding.RecastMeshObj> boxes)
		{
			if (box.mesh != null)
			{
				if (global::Pathfinding.RecastBBTree.RectIntersectsRect(box.rect, bounds))
				{
					boxes.Add(box.mesh);
				}
			}
			else
			{
				if (global::Pathfinding.RecastBBTree.RectIntersectsRect(box.c1.rect, bounds))
				{
					this.QueryBoxInBounds(box.c1, bounds, boxes);
				}
				if (global::Pathfinding.RecastBBTree.RectIntersectsRect(box.c2.rect, bounds))
				{
					this.QueryBoxInBounds(box.c2, bounds, boxes);
				}
			}
		}

		public bool Remove(global::Pathfinding.RecastMeshObj mesh)
		{
			if (mesh == null)
			{
				throw new global::System.ArgumentNullException("mesh");
			}
			if (this.root == null)
			{
				return false;
			}
			bool result = false;
			global::UnityEngine.Bounds bounds = mesh.GetBounds();
			global::UnityEngine.Rect bounds2 = global::UnityEngine.Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			this.root = this.RemoveBox(this.root, mesh, bounds2, ref result);
			return result;
		}

		private global::Pathfinding.RecastBBTreeBox RemoveBox(global::Pathfinding.RecastBBTreeBox c, global::Pathfinding.RecastMeshObj mesh, global::UnityEngine.Rect bounds, ref bool found)
		{
			if (!global::Pathfinding.RecastBBTree.RectIntersectsRect(c.rect, bounds))
			{
				return c;
			}
			if (c.mesh == mesh)
			{
				found = true;
				return null;
			}
			if (c.mesh == null && !found)
			{
				c.c1 = this.RemoveBox(c.c1, mesh, bounds, ref found);
				if (c.c1 == null)
				{
					return c.c2;
				}
				if (!found)
				{
					c.c2 = this.RemoveBox(c.c2, mesh, bounds, ref found);
					if (c.c2 == null)
					{
						return c.c1;
					}
				}
				if (found)
				{
					c.rect = global::Pathfinding.RecastBBTree.ExpandToContain(c.c1.rect, c.c2.rect);
				}
			}
			return c;
		}

		public void Insert(global::Pathfinding.RecastMeshObj mesh)
		{
			global::Pathfinding.RecastBBTreeBox recastBBTreeBox = new global::Pathfinding.RecastBBTreeBox(mesh);
			if (this.root == null)
			{
				this.root = recastBBTreeBox;
				return;
			}
			global::Pathfinding.RecastBBTreeBox recastBBTreeBox2 = this.root;
			for (;;)
			{
				recastBBTreeBox2.rect = global::Pathfinding.RecastBBTree.ExpandToContain(recastBBTreeBox2.rect, recastBBTreeBox.rect);
				if (recastBBTreeBox2.mesh != null)
				{
					break;
				}
				float num = global::Pathfinding.RecastBBTree.ExpansionRequired(recastBBTreeBox2.c1.rect, recastBBTreeBox.rect);
				float num2 = global::Pathfinding.RecastBBTree.ExpansionRequired(recastBBTreeBox2.c2.rect, recastBBTreeBox.rect);
				if (num < num2)
				{
					recastBBTreeBox2 = recastBBTreeBox2.c1;
				}
				else if (num2 < num)
				{
					recastBBTreeBox2 = recastBBTreeBox2.c2;
				}
				else
				{
					recastBBTreeBox2 = ((global::Pathfinding.RecastBBTree.RectArea(recastBBTreeBox2.c1.rect) >= global::Pathfinding.RecastBBTree.RectArea(recastBBTreeBox2.c2.rect)) ? recastBBTreeBox2.c2 : recastBBTreeBox2.c1);
				}
			}
			recastBBTreeBox2.c1 = recastBBTreeBox;
			global::Pathfinding.RecastBBTreeBox c = new global::Pathfinding.RecastBBTreeBox(recastBBTreeBox2.mesh);
			recastBBTreeBox2.c2 = c;
			recastBBTreeBox2.mesh = null;
		}

		private static bool RectIntersectsRect(global::UnityEngine.Rect r, global::UnityEngine.Rect r2)
		{
			return r.xMax > r2.xMin && r.yMax > r2.yMin && r2.xMax > r.xMin && r2.yMax > r.yMin;
		}

		private static float ExpansionRequired(global::UnityEngine.Rect r, global::UnityEngine.Rect r2)
		{
			float num = global::UnityEngine.Mathf.Min(r.xMin, r2.xMin);
			float num2 = global::UnityEngine.Mathf.Max(r.xMax, r2.xMax);
			float num3 = global::UnityEngine.Mathf.Min(r.yMin, r2.yMin);
			float num4 = global::UnityEngine.Mathf.Max(r.yMax, r2.yMax);
			return (num2 - num) * (num4 - num3) - global::Pathfinding.RecastBBTree.RectArea(r);
		}

		private static global::UnityEngine.Rect ExpandToContain(global::UnityEngine.Rect r, global::UnityEngine.Rect r2)
		{
			float xmin = global::UnityEngine.Mathf.Min(r.xMin, r2.xMin);
			float xmax = global::UnityEngine.Mathf.Max(r.xMax, r2.xMax);
			float ymin = global::UnityEngine.Mathf.Min(r.yMin, r2.yMin);
			float ymax = global::UnityEngine.Mathf.Max(r.yMax, r2.yMax);
			return global::UnityEngine.Rect.MinMaxRect(xmin, ymin, xmax, ymax);
		}

		private static float RectArea(global::UnityEngine.Rect r)
		{
			return r.width * r.height;
		}

		private global::Pathfinding.RecastBBTreeBox root;
	}
}
