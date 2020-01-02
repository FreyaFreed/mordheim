using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class RichPath
	{
		public void Initialize(global::Seeker s, global::Pathfinding.Path p, bool mergePartEndpoints, global::Pathfinding.RichFunnel.FunnelSimplification simplificationMode)
		{
			if (p.error)
			{
				throw new global::System.ArgumentException("Path has an error");
			}
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> path = p.path;
			if (path.Count == 0)
			{
				throw new global::System.ArgumentException("Path traverses no nodes");
			}
			this.seeker = s;
			for (int i = 0; i < this.parts.Count; i++)
			{
				global::Pathfinding.RichFunnel richFunnel = this.parts[i] as global::Pathfinding.RichFunnel;
				global::Pathfinding.RichSpecial richSpecial = this.parts[i] as global::Pathfinding.RichSpecial;
				if (richFunnel != null)
				{
					global::Pathfinding.Util.ObjectPool<global::Pathfinding.RichFunnel>.Release(ref richFunnel);
				}
				else if (richSpecial != null)
				{
					global::Pathfinding.Util.ObjectPool<global::Pathfinding.RichSpecial>.Release(ref richSpecial);
				}
			}
			this.parts.Clear();
			this.currentPart = 0;
			for (int j = 0; j < path.Count; j++)
			{
				if (path[j] is global::Pathfinding.TriangleMeshNode)
				{
					global::Pathfinding.NavGraph graph = global::Pathfinding.AstarData.GetGraph(path[j]);
					global::Pathfinding.RichFunnel richFunnel2 = global::Pathfinding.Util.ObjectPool<global::Pathfinding.RichFunnel>.Claim().Initialize(this, graph);
					richFunnel2.funnelSimplificationMode = simplificationMode;
					int num = j;
					uint graphIndex = path[num].GraphIndex;
					while (j < path.Count)
					{
						if (path[j].GraphIndex != graphIndex && !(path[j] is global::Pathfinding.NodeLink3Node))
						{
							break;
						}
						j++;
					}
					j--;
					if (num == 0)
					{
						richFunnel2.exactStart = p.vectorPath[0];
					}
					else
					{
						richFunnel2.exactStart = (global::UnityEngine.Vector3)path[(!mergePartEndpoints) ? num : (num - 1)].position;
					}
					if (j == path.Count - 1)
					{
						richFunnel2.exactEnd = p.vectorPath[p.vectorPath.Count - 1];
					}
					else
					{
						richFunnel2.exactEnd = (global::UnityEngine.Vector3)path[(!mergePartEndpoints) ? j : (j + 1)].position;
					}
					richFunnel2.BuildFunnelCorridor(path, num, j);
					this.parts.Add(richFunnel2);
				}
				else if (global::Pathfinding.NodeLink2.GetNodeLink(path[j]) != null)
				{
					global::Pathfinding.NodeLink2 nodeLink = global::Pathfinding.NodeLink2.GetNodeLink(path[j]);
					int num2 = j;
					uint graphIndex2 = path[num2].GraphIndex;
					for (j++; j < path.Count; j++)
					{
						if (path[j].GraphIndex != graphIndex2)
						{
							break;
						}
					}
					j--;
					if (j - num2 > 1)
					{
						throw new global::System.Exception("NodeLink2 path length greater than two (2) nodes. " + (j - num2));
					}
					if (j - num2 != 0)
					{
						global::Pathfinding.RichSpecial item = global::Pathfinding.Util.ObjectPool<global::Pathfinding.RichSpecial>.Claim().Initialize(nodeLink, path[num2]);
						this.parts.Add(item);
					}
				}
			}
		}

		public bool PartsLeft()
		{
			return this.currentPart < this.parts.Count;
		}

		public void NextPart()
		{
			this.currentPart++;
			if (this.currentPart >= this.parts.Count)
			{
				this.currentPart = this.parts.Count;
			}
		}

		public global::Pathfinding.RichPathPart GetCurrentPart()
		{
			return (this.currentPart >= this.parts.Count) ? null : this.parts[this.currentPart];
		}

		private int currentPart;

		private readonly global::System.Collections.Generic.List<global::Pathfinding.RichPathPart> parts = new global::System.Collections.Generic.List<global::Pathfinding.RichPathPart>();

		public global::Seeker seeker;
	}
}
