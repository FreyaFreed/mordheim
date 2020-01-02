using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Modifiers/Raycast Simplifier")]
	[global::System.Serializable]
	public class RayPathModifier : global::Pathfinding.MonoModifier
	{
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		public void SetRadius(float newRadius)
		{
			this.radius = newRadius + 0.1f;
		}

		public override void Apply(global::Pathfinding.Path p)
		{
			this.nodes.Clear();
			this.graphIndexes.Clear();
			this.nodes.Add((p as global::Pathfinding.ABPath).originalStartPoint);
			this.graphIndexes.Add(0);
			for (int i = 0; i < p.path.Count; i++)
			{
				this.nodes.Add(p.vectorPath[i]);
				this.graphIndexes.Add((int)p.path[i].GraphIndex);
				if (i + 1 < p.path.Count && p.path[i].GraphIndex == 0U)
				{
					global::UnityEngine.Vector3 a = p.vectorPath[i + 1] - p.vectorPath[i];
					a /= (float)(this.divideIterations + 1);
					for (int j = 1; j <= this.divideIterations; j++)
					{
						this.nodes.Add(p.vectorPath[i] + a * (float)j);
						this.graphIndexes.Add((int)p.path[i].GraphIndex);
					}
				}
			}
			this.nodes.Add((p as global::Pathfinding.ABPath).originalEndPoint);
			this.graphIndexes.Add(0);
			int k = 0;
			while (k < this.nodes.Count - 2)
			{
				if (this.graphIndexes[k] == this.graphIndexes[k + 2] && this.SimplifyPath(k, k + 2))
				{
					this.nodes.RemoveAt(k + 1);
					this.graphIndexes.RemoveAt(k + 1);
				}
				else
				{
					k++;
				}
			}
			this.nodes.RemoveAt(0);
			p.vectorPath.Clear();
			p.vectorPath.AddRange(this.nodes);
		}

		private bool SimplifyPath(int i, int j)
		{
			float num = global::UnityEngine.Vector3.Distance(this.nodes[i], this.nodes[j]);
			global::UnityEngine.RaycastHit raycastHit;
			return num < this.radius || global::PandoraUtils.RectCast(this.nodes[i] + global::UnityEngine.Vector3.up * (this.radius + 0.2f), this.nodes[j] - this.nodes[i], num, this.radius * 2f, this.radius * 2f + 0.05f, global::LayerMaskManager.pathMask, this.traversableColliders, out raycastHit, true);
		}

		public int divideIterations = 2;

		public global::System.Collections.Generic.List<global::UnityEngine.Collider> traversableColliders = new global::System.Collections.Generic.List<global::UnityEngine.Collider>();

		private float radius = 0.2f;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

		private global::System.Collections.Generic.List<int> graphIndexes = new global::System.Collections.Generic.List<int>();
	}
}
