using System;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
	public class ABPath : global::Pathfinding.Path
	{
		protected virtual bool hasEndPoint
		{
			get
			{
				return true;
			}
		}

		public static global::Pathfinding.ABPath Construct(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.ABPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.ABPath>();
			path.Setup(start, end, callback);
			return path;
		}

		protected void Setup(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.OnPathDelegate callbackDelegate)
		{
			this.callback = callbackDelegate;
			this.UpdateStartEnd(start, end);
		}

		protected void UpdateStartEnd(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end)
		{
			this.originalStartPoint = start;
			this.originalEndPoint = end;
			this.startPoint = start;
			this.endPoint = end;
			this.startIntPoint = (global::Pathfinding.Int3)start;
			this.hTarget = (global::Pathfinding.Int3)end;
		}

		public override uint GetConnectionSpecialCost(global::Pathfinding.GraphNode a, global::Pathfinding.GraphNode b, uint currentCost)
		{
			if (this.startNode != null && this.endNode != null)
			{
				if (a == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - ((b != this.endNode) ? b.position : this.hTarget)).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - ((a != this.endNode) ? a.position : this.hTarget)).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (a == this.endNode)
				{
					return (uint)((double)(this.hTarget - b.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == this.endNode)
				{
					return (uint)((double)(this.hTarget - a.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
			}
			else
			{
				if (a == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - b.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - a.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
			}
			return currentCost;
		}

		public override void Reset()
		{
			base.Reset();
			this.startNode = null;
			this.endNode = null;
			this.startHint = null;
			this.endHint = null;
			this.originalStartPoint = global::UnityEngine.Vector3.zero;
			this.originalEndPoint = global::UnityEngine.Vector3.zero;
			this.startPoint = global::UnityEngine.Vector3.zero;
			this.endPoint = global::UnityEngine.Vector3.zero;
			this.calculatePartial = false;
			this.partialBestTarget = null;
			this.startIntPoint = default(global::Pathfinding.Int3);
			this.hTarget = default(global::Pathfinding.Int3);
			this.endNodeCosts = null;
			this.gridSpecialCaseNode = null;
		}

		protected virtual bool EndPointGridGraphSpecialCase(global::Pathfinding.GraphNode closestWalkableEndNode)
		{
			global::Pathfinding.GridNode gridNode = closestWalkableEndNode as global::Pathfinding.GridNode;
			if (gridNode != null)
			{
				global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(gridNode.GraphIndex);
				global::Pathfinding.GridNode gridNode2 = global::AstarPath.active.GetNearest(this.originalEndPoint, global::Pathfinding.NNConstraint.None, this.endHint).node as global::Pathfinding.GridNode;
				if (gridNode != gridNode2 && gridNode2 != null && gridNode.GraphIndex == gridNode2.GraphIndex)
				{
					int num = gridNode.NodeInGridIndex % gridGraph.width;
					int num2 = gridNode.NodeInGridIndex / gridGraph.width;
					int num3 = gridNode2.NodeInGridIndex % gridGraph.width;
					int num4 = gridNode2.NodeInGridIndex / gridGraph.width;
					bool flag = false;
					switch (gridGraph.neighbours)
					{
					case global::Pathfinding.NumNeighbours.Four:
						if ((num == num3 && global::System.Math.Abs(num2 - num4) == 1) || (num2 == num4 && global::System.Math.Abs(num - num3) == 1))
						{
							flag = true;
						}
						break;
					case global::Pathfinding.NumNeighbours.Eight:
						if (global::System.Math.Abs(num - num3) <= 1 && global::System.Math.Abs(num2 - num4) <= 1)
						{
							flag = true;
						}
						break;
					case global::Pathfinding.NumNeighbours.Six:
						for (int i = 0; i < 6; i++)
						{
							int num5 = num3 + gridGraph.neighbourXOffsets[global::Pathfinding.GridGraph.hexagonNeighbourIndices[i]];
							int num6 = num4 + gridGraph.neighbourZOffsets[global::Pathfinding.GridGraph.hexagonNeighbourIndices[i]];
							if (num == num5 && num2 == num6)
							{
								flag = true;
								break;
							}
						}
						break;
					default:
						throw new global::System.Exception("Unhandled NumNeighbours");
					}
					if (flag)
					{
						this.SetFlagOnSurroundingGridNodes(gridNode2, 1, true);
						this.endPoint = (global::UnityEngine.Vector3)gridNode2.position;
						this.hTarget = gridNode2.position;
						this.endNode = gridNode2;
						this.hTargetNode = this.endNode;
						this.gridSpecialCaseNode = gridNode2;
						return true;
					}
				}
			}
			return false;
		}

		private void SetFlagOnSurroundingGridNodes(global::Pathfinding.GridNode gridNode, int flag, bool flagState)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.GridNode.GetGridGraph(gridNode.GraphIndex);
			int num = (gridGraph.neighbours != global::Pathfinding.NumNeighbours.Four) ? ((gridGraph.neighbours != global::Pathfinding.NumNeighbours.Eight) ? 6 : 8) : 4;
			int num2 = gridNode.NodeInGridIndex % gridGraph.width;
			int num3 = gridNode.NodeInGridIndex / gridGraph.width;
			if (flag != 1 && flag != 2)
			{
				throw new global::System.ArgumentOutOfRangeException("flag");
			}
			for (int i = 0; i < num; i++)
			{
				int num4;
				int num5;
				if (gridGraph.neighbours == global::Pathfinding.NumNeighbours.Six)
				{
					num4 = num2 + gridGraph.neighbourXOffsets[global::Pathfinding.GridGraph.hexagonNeighbourIndices[i]];
					num5 = num3 + gridGraph.neighbourZOffsets[global::Pathfinding.GridGraph.hexagonNeighbourIndices[i]];
				}
				else
				{
					num4 = num2 + gridGraph.neighbourXOffsets[i];
					num5 = num3 + gridGraph.neighbourZOffsets[i];
				}
				if (num4 >= 0 && num5 >= 0 && num4 < gridGraph.width && num5 < gridGraph.depth)
				{
					global::Pathfinding.GridNode node = gridGraph.nodes[num5 * gridGraph.width + num4];
					global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(node);
					if (flag == 1)
					{
						pathNode.flag1 = flagState;
					}
					else
					{
						pathNode.flag2 = flagState;
					}
				}
			}
		}

		public override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
			global::Pathfinding.PathNNConstraint pathNNConstraint = this.nnConstraint as global::Pathfinding.PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			this.startPoint = nearest.position;
			this.startIntPoint = (global::Pathfinding.Int3)this.startPoint;
			this.startNode = nearest.node;
			if (this.startNode == null)
			{
				base.Error();
				return;
			}
			if (!this.startNode.Walkable)
			{
				base.Error();
				return;
			}
			if (this.hasEndPoint)
			{
				global::Pathfinding.NNInfo nearest2 = global::AstarPath.active.GetNearest(this.endPoint, this.nnConstraint, this.endHint);
				this.endPoint = nearest2.position;
				this.endNode = nearest2.node;
				if (this.startNode == null && this.endNode == null)
				{
					base.Error();
					return;
				}
				if (this.endNode == null)
				{
					base.Error();
					return;
				}
				if (!this.endNode.Walkable)
				{
					base.Error();
					return;
				}
				if (this.startNode.Area != this.endNode.Area)
				{
					base.Error();
					return;
				}
				if (!this.EndPointGridGraphSpecialCase(nearest2.node))
				{
					this.hTarget = (global::Pathfinding.Int3)this.endPoint;
					this.hTargetNode = this.endNode;
					base.pathHandler.GetPathNode(this.endNode).flag1 = true;
				}
			}
		}

		protected virtual void CompletePathIfStartIsValidTarget()
		{
			if (this.hasEndPoint && base.pathHandler.GetPathNode(this.startNode).flag1)
			{
				this.CompleteWith(this.startNode);
				this.Trace(base.pathHandler.GetPathNode(this.startNode));
			}
		}

		public override void Initialize()
		{
			if (this.startNode != null)
			{
				base.pathHandler.GetPathNode(this.startNode).flag2 = true;
			}
			if (this.endNode != null)
			{
				base.pathHandler.GetPathNode(this.endNode).flag2 = true;
			}
			global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			pathNode.pathID = base.pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.CompletePathIfStartIsValidTarget();
			if (base.CompleteState == global::Pathfinding.PathCompleteState.Complete)
			{
				return;
			}
			this.startNode.Open(this, pathNode, base.pathHandler);
			this.searchedNodes++;
			this.partialBestTarget = pathNode;
			if (base.pathHandler.heap.isEmpty)
			{
				if (!this.calculatePartial)
				{
					base.Error();
					return;
				}
				base.CompleteState = global::Pathfinding.PathCompleteState.Partial;
				this.Trace(this.partialBestTarget);
			}
			this.currentR = base.pathHandler.heap.Remove();
		}

		public override void Cleanup()
		{
			if (this.startNode != null)
			{
				global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
				pathNode.flag1 = false;
				pathNode.flag2 = false;
			}
			if (this.endNode != null)
			{
				global::Pathfinding.PathNode pathNode2 = base.pathHandler.GetPathNode(this.endNode);
				pathNode2.flag1 = false;
				pathNode2.flag2 = false;
			}
			if (this.gridSpecialCaseNode != null)
			{
				global::Pathfinding.PathNode pathNode3 = base.pathHandler.GetPathNode(this.gridSpecialCaseNode);
				pathNode3.flag1 = false;
				pathNode3.flag2 = false;
				this.SetFlagOnSurroundingGridNodes(this.gridSpecialCaseNode, 1, false);
				this.SetFlagOnSurroundingGridNodes(this.gridSpecialCaseNode, 2, false);
			}
		}

		private void CompleteWith(global::Pathfinding.GraphNode node)
		{
			if (this.endNode != node)
			{
				global::Pathfinding.GridNode gridNode = node as global::Pathfinding.GridNode;
				if (gridNode == null)
				{
					throw new global::System.Exception("Some path is not cleaning up the flag1 field. This is a bug.");
				}
				this.endPoint = gridNode.ClosestPointOnNode(this.originalEndPoint);
				this.endNode = node;
			}
			base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
		}

		public override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == global::Pathfinding.PathCompleteState.NotCalculated)
			{
				this.searchedNodes++;
				if (this.currentR.flag1)
				{
					this.CompleteWith(this.currentR.node);
					break;
				}
				if (this.currentR.H < this.partialBestTarget.H)
				{
					this.partialBestTarget = this.currentR;
				}
				this.currentR.node.Open(this, this.currentR, base.pathHandler);
				if (base.pathHandler.heap.isEmpty)
				{
					base.Error();
					return;
				}
				this.currentR = base.pathHandler.heap.Remove();
				if (num > 500)
				{
					if (global::System.DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
					if (this.searchedNodes > 1000000)
					{
						throw new global::System.Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
			if (base.CompleteState == global::Pathfinding.PathCompleteState.Complete)
			{
				this.Trace(this.currentR);
			}
			else if (this.calculatePartial && this.partialBestTarget != null)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Partial;
				this.Trace(this.partialBestTarget);
			}
		}

		public void ResetCosts(global::Pathfinding.Path p)
		{
		}

		public override string DebugString(global::Pathfinding.PathLog logMode)
		{
			if (logMode == global::Pathfinding.PathLog.None || (!base.error && logMode == global::Pathfinding.PathLog.OnlyErrors))
			{
				return string.Empty;
			}
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			base.DebugStringPrefix(logMode, stringBuilder);
			if (!base.error && logMode == global::Pathfinding.PathLog.Heavy)
			{
				stringBuilder.Append("\nSearch Iterations " + this.searchIterations);
				if (this.hasEndPoint && this.endNode != null)
				{
					global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.endNode);
					stringBuilder.Append("\nEnd Node\n\tG: ");
					stringBuilder.Append(pathNode.G);
					stringBuilder.Append("\n\tH: ");
					stringBuilder.Append(pathNode.H);
					stringBuilder.Append("\n\tF: ");
					stringBuilder.Append(pathNode.F);
					stringBuilder.Append("\n\tPoint: ");
					stringBuilder.Append(this.endPoint.ToString());
					stringBuilder.Append("\n\tGraph: ");
					stringBuilder.Append(this.endNode.GraphIndex);
				}
				stringBuilder.Append("\nStart Node");
				stringBuilder.Append("\n\tPoint: ");
				stringBuilder.Append(this.startPoint.ToString());
				stringBuilder.Append("\n\tGraph: ");
				if (this.startNode != null)
				{
					stringBuilder.Append(this.startNode.GraphIndex);
				}
				else
				{
					stringBuilder.Append("< null startNode >");
				}
			}
			base.DebugStringSuffix(logMode, stringBuilder);
			return stringBuilder.ToString();
		}

		public global::UnityEngine.Vector3 GetMovementVector(global::UnityEngine.Vector3 point)
		{
			if (this.vectorPath == null || this.vectorPath.Count == 0)
			{
				return global::UnityEngine.Vector3.zero;
			}
			if (this.vectorPath.Count == 1)
			{
				return this.vectorPath[0] - point;
			}
			float num = float.PositiveInfinity;
			int num2 = 0;
			for (int i = 0; i < this.vectorPath.Count - 1; i++)
			{
				global::UnityEngine.Vector3 a = global::Pathfinding.VectorMath.ClosestPointOnSegment(this.vectorPath[i], this.vectorPath[i + 1], point);
				float sqrMagnitude = (a - point).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					num2 = i;
				}
			}
			return this.vectorPath[num2 + 1] - point;
		}

		public bool recalcStartEndCosts = true;

		public global::Pathfinding.GraphNode startNode;

		public global::Pathfinding.GraphNode endNode;

		public global::Pathfinding.GraphNode startHint;

		public global::Pathfinding.GraphNode endHint;

		public global::UnityEngine.Vector3 originalStartPoint;

		public global::UnityEngine.Vector3 originalEndPoint;

		public global::UnityEngine.Vector3 startPoint;

		public global::UnityEngine.Vector3 endPoint;

		public global::Pathfinding.Int3 startIntPoint;

		public bool calculatePartial;

		protected global::Pathfinding.PathNode partialBestTarget;

		protected int[] endNodeCosts;

		private global::Pathfinding.GridNode gridSpecialCaseNode;
	}
}
