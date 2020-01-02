using System;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.RVO
{
	public class RVOQuadtree
	{
		public void Clear()
		{
			this.nodes[0] = default(global::Pathfinding.RVO.RVOQuadtree.Node);
			this.filledNodes = 1;
			this.maxRadius = 0f;
		}

		public void SetBounds(global::UnityEngine.Rect r)
		{
			this.bounds = r;
		}

		public int GetNodeIndex()
		{
			if (this.filledNodes == this.nodes.Length)
			{
				global::Pathfinding.RVO.RVOQuadtree.Node[] array = new global::Pathfinding.RVO.RVOQuadtree.Node[this.nodes.Length * 2];
				for (int i = 0; i < this.nodes.Length; i++)
				{
					array[i] = this.nodes[i];
				}
				this.nodes = array;
			}
			this.nodes[this.filledNodes] = default(global::Pathfinding.RVO.RVOQuadtree.Node);
			this.nodes[this.filledNodes].child00 = this.filledNodes;
			this.filledNodes++;
			return this.filledNodes - 1;
		}

		public void Insert(global::Pathfinding.RVO.Sampled.Agent agent)
		{
			int num = 0;
			global::UnityEngine.Rect r = this.bounds;
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(agent.position.x, agent.position.y);
			agent.next = null;
			this.maxRadius = global::System.Math.Max(agent.radius, this.maxRadius);
			int num2 = 0;
			for (;;)
			{
				num2++;
				if (this.nodes[num].child00 == num)
				{
					if (this.nodes[num].count < 15 || num2 > 10)
					{
						break;
					}
					global::Pathfinding.RVO.RVOQuadtree.Node node = this.nodes[num];
					node.child00 = this.GetNodeIndex();
					node.child01 = this.GetNodeIndex();
					node.child10 = this.GetNodeIndex();
					node.child11 = this.GetNodeIndex();
					this.nodes[num] = node;
					this.nodes[num].Distribute(this.nodes, r);
				}
				if (this.nodes[num].child00 != num)
				{
					global::UnityEngine.Vector2 center = r.center;
					if (vector.x > center.x)
					{
						if (vector.y > center.y)
						{
							num = this.nodes[num].child11;
							r = global::UnityEngine.Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax);
						}
						else
						{
							num = this.nodes[num].child10;
							r = global::UnityEngine.Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y);
						}
					}
					else if (vector.y > center.y)
					{
						num = this.nodes[num].child01;
						r = global::UnityEngine.Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax);
					}
					else
					{
						num = this.nodes[num].child00;
						r = global::UnityEngine.Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y);
					}
				}
			}
			this.nodes[num].Add(agent);
			global::Pathfinding.RVO.RVOQuadtree.Node[] array = this.nodes;
			int num3 = num;
			array[num3].count = array[num3].count + 1;
		}

		public void Query(global::UnityEngine.Vector2 p, float radius, global::Pathfinding.RVO.Sampled.Agent agent)
		{
			this.QueryRec(0, p, radius, agent, this.bounds);
		}

		private float QueryRec(int i, global::UnityEngine.Vector2 p, float radius, global::Pathfinding.RVO.Sampled.Agent agent, global::UnityEngine.Rect r)
		{
			if (this.nodes[i].child00 == i)
			{
				for (global::Pathfinding.RVO.Sampled.Agent agent2 = this.nodes[i].linkedList; agent2 != null; agent2 = agent2.next)
				{
					float num = agent.InsertAgentNeighbour(agent2, radius * radius);
					if (num < radius * radius)
					{
						radius = global::UnityEngine.Mathf.Sqrt(num);
					}
				}
			}
			else
			{
				global::UnityEngine.Vector2 center = r.center;
				if (p.x - radius < center.x)
				{
					if (p.y - radius < center.y)
					{
						radius = this.QueryRec(this.nodes[i].child00, p, radius, agent, global::UnityEngine.Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
					}
					if (p.y + radius > center.y)
					{
						radius = this.QueryRec(this.nodes[i].child01, p, radius, agent, global::UnityEngine.Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
					}
				}
				if (p.x + radius > center.x)
				{
					if (p.y - radius < center.y)
					{
						radius = this.QueryRec(this.nodes[i].child10, p, radius, agent, global::UnityEngine.Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
					}
					if (p.y + radius > center.y)
					{
						radius = this.QueryRec(this.nodes[i].child11, p, radius, agent, global::UnityEngine.Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
					}
				}
			}
			return radius;
		}

		public void DebugDraw()
		{
			this.DebugDrawRec(0, this.bounds);
		}

		private void DebugDrawRec(int i, global::UnityEngine.Rect r)
		{
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(r.xMin, 0f, r.yMin), new global::UnityEngine.Vector3(r.xMax, 0f, r.yMin), global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(r.xMax, 0f, r.yMin), new global::UnityEngine.Vector3(r.xMax, 0f, r.yMax), global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(r.xMax, 0f, r.yMax), new global::UnityEngine.Vector3(r.xMin, 0f, r.yMax), global::UnityEngine.Color.white);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(r.xMin, 0f, r.yMax), new global::UnityEngine.Vector3(r.xMin, 0f, r.yMin), global::UnityEngine.Color.white);
			if (this.nodes[i].child00 != i)
			{
				global::UnityEngine.Vector2 center = r.center;
				this.DebugDrawRec(this.nodes[i].child11, global::UnityEngine.Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
				this.DebugDrawRec(this.nodes[i].child10, global::UnityEngine.Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
				this.DebugDrawRec(this.nodes[i].child01, global::UnityEngine.Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
				this.DebugDrawRec(this.nodes[i].child00, global::UnityEngine.Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
			}
			for (global::Pathfinding.RVO.Sampled.Agent agent = this.nodes[i].linkedList; agent != null; agent = agent.next)
			{
				global::UnityEngine.Vector2 position = this.nodes[i].linkedList.position;
				global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(position.x, 0f, position.y) + global::UnityEngine.Vector3.up, new global::UnityEngine.Vector3(agent.position.x, 0f, agent.position.y) + global::UnityEngine.Vector3.up, new global::UnityEngine.Color(1f, 1f, 0f, 0.5f));
			}
		}

		private const int LeafSize = 15;

		private float maxRadius;

		private global::Pathfinding.RVO.RVOQuadtree.Node[] nodes = new global::Pathfinding.RVO.RVOQuadtree.Node[42];

		private int filledNodes = 1;

		private global::UnityEngine.Rect bounds;

		private struct Node
		{
			public void Add(global::Pathfinding.RVO.Sampled.Agent agent)
			{
				agent.next = this.linkedList;
				this.linkedList = agent;
			}

			public void Distribute(global::Pathfinding.RVO.RVOQuadtree.Node[] nodes, global::UnityEngine.Rect r)
			{
				global::UnityEngine.Vector2 center = r.center;
				while (this.linkedList != null)
				{
					global::Pathfinding.RVO.Sampled.Agent next = this.linkedList.next;
					if (this.linkedList.position.x > center.x)
					{
						if (this.linkedList.position.y > center.y)
						{
							nodes[this.child11].Add(this.linkedList);
						}
						else
						{
							nodes[this.child10].Add(this.linkedList);
						}
					}
					else if (this.linkedList.position.y > center.y)
					{
						nodes[this.child01].Add(this.linkedList);
					}
					else
					{
						nodes[this.child00].Add(this.linkedList);
					}
					this.linkedList = next;
				}
				this.count = 0;
			}

			public int child00;

			public int child01;

			public int child10;

			public int child11;

			public byte count;

			public global::Pathfinding.RVO.Sampled.Agent linkedList;
		}
	}
}
