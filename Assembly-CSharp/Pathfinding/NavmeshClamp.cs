using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_clamp.php")]
	public class NavmeshClamp : global::UnityEngine.MonoBehaviour
	{
		private void LateUpdate()
		{
			if (this.prevNode == null)
			{
				this.prevNode = global::AstarPath.active.GetNearest(base.transform.position).node;
				this.prevPos = base.transform.position;
			}
			if (this.prevNode == null)
			{
				return;
			}
			if (this.prevNode != null)
			{
				global::Pathfinding.IRaycastableGraph raycastableGraph = global::Pathfinding.AstarData.GetGraph(this.prevNode) as global::Pathfinding.IRaycastableGraph;
				if (raycastableGraph != null)
				{
					global::Pathfinding.GraphHitInfo graphHitInfo;
					if (raycastableGraph.Linecast(this.prevPos, base.transform.position, this.prevNode, out graphHitInfo))
					{
						graphHitInfo.point.y = base.transform.position.y;
						global::UnityEngine.Vector3 vector = global::Pathfinding.VectorMath.ClosestPointOnLine(graphHitInfo.tangentOrigin, graphHitInfo.tangentOrigin + graphHitInfo.tangent, base.transform.position);
						global::UnityEngine.Vector3 vector2 = graphHitInfo.point;
						vector2 += global::UnityEngine.Vector3.ClampMagnitude((global::UnityEngine.Vector3)graphHitInfo.node.position - vector2, 0.008f);
						if (raycastableGraph.Linecast(vector2, vector, graphHitInfo.node, out graphHitInfo))
						{
							graphHitInfo.point.y = base.transform.position.y;
							base.transform.position = graphHitInfo.point;
						}
						else
						{
							vector.y = base.transform.position.y;
							base.transform.position = vector;
						}
					}
					this.prevNode = graphHitInfo.node;
				}
			}
			this.prevPos = base.transform.position;
		}

		private global::Pathfinding.GraphNode prevNode;

		private global::UnityEngine.Vector3 prevPos;
	}
}
