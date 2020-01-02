using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class MultiTargetPath : global::Pathfinding.ABPath
	{
		public static global::Pathfinding.MultiTargetPath Construct(global::UnityEngine.Vector3[] startPoints, global::UnityEngine.Vector3 target, global::Pathfinding.OnPathDelegate[] callbackDelegates, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.MultiTargetPath multiTargetPath = global::Pathfinding.MultiTargetPath.Construct(target, startPoints, callbackDelegates, callback);
			multiTargetPath.inverted = true;
			return multiTargetPath;
		}

		public static global::Pathfinding.MultiTargetPath Construct(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3[] targets, global::Pathfinding.OnPathDelegate[] callbackDelegates, global::Pathfinding.OnPathDelegate callback = null)
		{
			global::Pathfinding.MultiTargetPath path = global::Pathfinding.PathPool.GetPath<global::Pathfinding.MultiTargetPath>();
			path.Setup(start, targets, callbackDelegates, callback);
			return path;
		}

		protected void Setup(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3[] targets, global::Pathfinding.OnPathDelegate[] callbackDelegates, global::Pathfinding.OnPathDelegate callback)
		{
			this.inverted = false;
			this.callback = callback;
			this.callbacks = callbackDelegates;
			this.targetPoints = targets;
			this.originalStartPoint = start;
			this.startPoint = start;
			this.startIntPoint = (global::Pathfinding.Int3)start;
			if (targets.Length == 0)
			{
				base.Error();
				return;
			}
			this.endPoint = targets[0];
			this.originalTargetPoints = new global::UnityEngine.Vector3[this.targetPoints.Length];
			for (int i = 0; i < this.targetPoints.Length; i++)
			{
				this.originalTargetPoints[i] = this.targetPoints[i];
			}
		}

		public override void OnEnterPool()
		{
			if (this.vectorPaths != null)
			{
				for (int i = 0; i < this.vectorPaths.Length; i++)
				{
					if (this.vectorPaths[i] != null)
					{
						global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(this.vectorPaths[i]);
					}
				}
			}
			this.vectorPaths = null;
			this.vectorPath = null;
			if (this.nodePaths != null)
			{
				for (int j = 0; j < this.nodePaths.Length; j++)
				{
					if (this.nodePaths[j] != null)
					{
						global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(this.nodePaths[j]);
					}
				}
			}
			this.nodePaths = null;
			this.path = null;
			base.OnEnterPool();
		}

		private void ChooseShortestPath()
		{
			this.chosenTarget = -1;
			if (this.nodePaths != null)
			{
				uint num = 2147483647U;
				for (int i = 0; i < this.nodePaths.Length; i++)
				{
					global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = this.nodePaths[i];
					if (list != null)
					{
						uint g = base.pathHandler.GetPathNode(list[(!this.inverted) ? (list.Count - 1) : 0]).G;
						if (this.chosenTarget == -1 || g < num)
						{
							this.chosenTarget = i;
							num = g;
						}
					}
				}
			}
		}

		private void SetPathParametersForReturn(int target)
		{
			this.path = this.nodePaths[target];
			this.vectorPath = this.vectorPaths[target];
			if (this.inverted)
			{
				this.startNode = this.targetNodes[target];
				this.startPoint = this.targetPoints[target];
				this.originalStartPoint = this.originalTargetPoints[target];
			}
			else
			{
				this.endNode = this.targetNodes[target];
				this.endPoint = this.targetPoints[target];
				this.originalEndPoint = this.originalTargetPoints[target];
			}
		}

		public override void ReturnPath()
		{
			if (base.error)
			{
				if (this.callbacks != null)
				{
					for (int i = 0; i < this.callbacks.Length; i++)
					{
						if (this.callbacks[i] != null)
						{
							this.callbacks[i](this);
						}
					}
				}
				if (this.callback != null)
				{
					this.callback(this);
				}
				return;
			}
			bool flag = false;
			if (this.inverted)
			{
				this.endPoint = this.startPoint;
				this.endNode = this.startNode;
				this.originalEndPoint = this.originalStartPoint;
			}
			for (int j = 0; j < this.nodePaths.Length; j++)
			{
				if (this.nodePaths[j] != null)
				{
					base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
					flag = true;
				}
				else
				{
					base.CompleteState = global::Pathfinding.PathCompleteState.Error;
				}
				if (this.callbacks != null && this.callbacks[j] != null)
				{
					this.SetPathParametersForReturn(j);
					this.callbacks[j](this);
					this.vectorPaths[j] = this.vectorPath;
				}
			}
			if (flag)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
				this.SetPathParametersForReturn(this.chosenTarget);
			}
			else
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Error;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		protected void FoundTarget(global::Pathfinding.PathNode nodeR, int i)
		{
			nodeR.flag1 = false;
			this.Trace(nodeR);
			this.vectorPaths[i] = this.vectorPath;
			this.nodePaths[i] = this.path;
			this.vectorPath = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			this.path = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			this.targetsFound[i] = true;
			this.targetNodeCount--;
			if (!this.pathsForAll)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
				this.targetNodeCount = 0;
				return;
			}
			if (this.targetNodeCount <= 0)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
				return;
			}
			this.RecalculateHTarget(false);
		}

		protected void RebuildOpenList()
		{
			global::Pathfinding.BinaryHeap heap = base.pathHandler.heap;
			for (int i = 0; i < heap.numberOfItems; i++)
			{
				global::Pathfinding.PathNode node = heap.GetNode(i);
				node.H = base.CalculateHScore(node.node);
				heap.SetF(i, node.F);
			}
			base.pathHandler.heap.Rebuild();
		}

		public override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
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
			global::Pathfinding.PathNNConstraint pathNNConstraint = this.nnConstraint as global::Pathfinding.PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			this.vectorPaths = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>[this.targetPoints.Length];
			this.nodePaths = new global::System.Collections.Generic.List<global::Pathfinding.GraphNode>[this.targetPoints.Length];
			this.targetNodes = new global::Pathfinding.GraphNode[this.targetPoints.Length];
			this.targetsFound = new bool[this.targetPoints.Length];
			this.targetNodeCount = this.targetPoints.Length;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < this.targetPoints.Length; i++)
			{
				global::Pathfinding.NNInfo nearest2 = global::AstarPath.active.GetNearest(this.targetPoints[i], this.nnConstraint);
				this.targetNodes[i] = nearest2.node;
				this.targetPoints[i] = nearest2.position;
				if (this.targetNodes[i] != null)
				{
					flag3 = true;
					this.endNode = this.targetNodes[i];
				}
				bool flag4 = false;
				if (nearest2.node != null && nearest2.node.Walkable)
				{
					flag = true;
				}
				else
				{
					flag4 = true;
				}
				if (nearest2.node != null && nearest2.node.Area == this.startNode.Area)
				{
					flag2 = true;
				}
				else
				{
					flag4 = true;
				}
				if (flag4)
				{
					this.targetsFound[i] = true;
					this.targetNodeCount--;
				}
			}
			this.startPoint = nearest.position;
			this.startIntPoint = (global::Pathfinding.Int3)this.startPoint;
			if (this.startNode == null || !flag3)
			{
				base.Error();
				return;
			}
			if (!this.startNode.Walkable)
			{
				base.Error();
				return;
			}
			if (!flag)
			{
				base.Error();
				return;
			}
			if (!flag2)
			{
				base.Error();
				return;
			}
			this.RecalculateHTarget(true);
		}

		private void RecalculateHTarget(bool firstTime)
		{
			if (!this.pathsForAll)
			{
				this.heuristic = global::Pathfinding.Heuristic.None;
				this.heuristicScale = 0f;
				return;
			}
			switch (this.heuristicMode)
			{
			case global::Pathfinding.MultiTargetPath.HeuristicMode.None:
				this.heuristic = global::Pathfinding.Heuristic.None;
				this.heuristicScale = 0f;
				goto IL_269;
			case global::Pathfinding.MultiTargetPath.HeuristicMode.Average:
				if (!firstTime)
				{
					return;
				}
				break;
			case global::Pathfinding.MultiTargetPath.HeuristicMode.MovingAverage:
				break;
			case global::Pathfinding.MultiTargetPath.HeuristicMode.Midpoint:
				if (!firstTime)
				{
					return;
				}
				goto IL_EF;
			case global::Pathfinding.MultiTargetPath.HeuristicMode.MovingMidpoint:
				goto IL_EF;
			case global::Pathfinding.MultiTargetPath.HeuristicMode.Sequential:
			{
				if (!firstTime && !this.targetsFound[this.sequentialTarget])
				{
					return;
				}
				float num = 0f;
				for (int i = 0; i < this.targetPoints.Length; i++)
				{
					if (!this.targetsFound[i])
					{
						float sqrMagnitude = (this.targetNodes[i].position - this.startNode.position).sqrMagnitude;
						if (sqrMagnitude > num)
						{
							num = sqrMagnitude;
							this.hTarget = (global::Pathfinding.Int3)this.targetPoints[i];
							this.sequentialTarget = i;
						}
					}
				}
				goto IL_269;
			}
			default:
				goto IL_269;
			}
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			int num2 = 0;
			for (int j = 0; j < this.targetPoints.Length; j++)
			{
				if (!this.targetsFound[j])
				{
					vector += (global::UnityEngine.Vector3)this.targetNodes[j].position;
					num2++;
				}
			}
			if (num2 == 0)
			{
				throw new global::System.Exception("Should not happen");
			}
			vector /= (float)num2;
			this.hTarget = (global::Pathfinding.Int3)vector;
			goto IL_269;
			IL_EF:
			global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.zero;
			global::UnityEngine.Vector3 vector3 = global::UnityEngine.Vector3.zero;
			bool flag = false;
			for (int k = 0; k < this.targetPoints.Length; k++)
			{
				if (!this.targetsFound[k])
				{
					if (!flag)
					{
						vector2 = (global::UnityEngine.Vector3)this.targetNodes[k].position;
						vector3 = (global::UnityEngine.Vector3)this.targetNodes[k].position;
						flag = true;
					}
					else
					{
						vector2 = global::UnityEngine.Vector3.Min((global::UnityEngine.Vector3)this.targetNodes[k].position, vector2);
						vector3 = global::UnityEngine.Vector3.Max((global::UnityEngine.Vector3)this.targetNodes[k].position, vector3);
					}
				}
			}
			global::Pathfinding.Int3 hTarget = (global::Pathfinding.Int3)((vector2 + vector3) * 0.5f);
			this.hTarget = hTarget;
			IL_269:
			if (!firstTime)
			{
				this.RebuildOpenList();
			}
		}

		public override void Initialize()
		{
			global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			pathNode.pathID = base.pathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			for (int i = 0; i < this.targetNodes.Length; i++)
			{
				if (this.startNode == this.targetNodes[i])
				{
					this.FoundTarget(pathNode, i);
				}
				else if (this.targetNodes[i] != null)
				{
					base.pathHandler.GetPathNode(this.targetNodes[i]).flag1 = true;
				}
			}
			if (this.targetNodeCount <= 0)
			{
				base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
				return;
			}
			this.startNode.Open(this, pathNode, base.pathHandler);
			this.searchedNodes++;
			if (base.pathHandler.heap.isEmpty)
			{
				base.Error();
				return;
			}
			this.currentR = base.pathHandler.heap.Remove();
		}

		public override void Cleanup()
		{
			this.ChooseShortestPath();
			this.ResetFlags();
		}

		private void ResetFlags()
		{
			if (this.targetNodes != null)
			{
				for (int i = 0; i < this.targetNodes.Length; i++)
				{
					if (this.targetNodes[i] != null)
					{
						base.pathHandler.GetPathNode(this.targetNodes[i]).flag1 = false;
					}
				}
			}
		}

		public override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == global::Pathfinding.PathCompleteState.NotCalculated)
			{
				this.searchedNodes++;
				if (this.currentR.flag1)
				{
					for (int i = 0; i < this.targetNodes.Length; i++)
					{
						if (!this.targetsFound[i] && this.currentR.node == this.targetNodes[i])
						{
							this.FoundTarget(this.currentR, i);
							if (base.CompleteState != global::Pathfinding.PathCompleteState.NotCalculated)
							{
								break;
							}
						}
					}
					if (this.targetNodeCount <= 0)
					{
						base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
						break;
					}
				}
				this.currentR.node.Open(this, this.currentR, base.pathHandler);
				if (base.pathHandler.heap.isEmpty)
				{
					base.CompleteState = global::Pathfinding.PathCompleteState.Complete;
					break;
				}
				this.currentR = base.pathHandler.heap.Remove();
				if (num > 500)
				{
					if (global::System.DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
				}
				num++;
			}
		}

		protected override void Trace(global::Pathfinding.PathNode node)
		{
			base.Trace(node);
			if (this.inverted)
			{
				int num = this.path.Count / 2;
				for (int i = 0; i < num; i++)
				{
					global::Pathfinding.GraphNode value = this.path[i];
					this.path[i] = this.path[this.path.Count - i - 1];
					this.path[this.path.Count - i - 1] = value;
				}
				for (int j = 0; j < num; j++)
				{
					global::UnityEngine.Vector3 value2 = this.vectorPath[j];
					this.vectorPath[j] = this.vectorPath[this.vectorPath.Count - j - 1];
					this.vectorPath[this.vectorPath.Count - j - 1] = value2;
				}
			}
		}

		public override string DebugString(global::Pathfinding.PathLog logMode)
		{
			if (logMode == global::Pathfinding.PathLog.None || (!base.error && logMode == global::Pathfinding.PathLog.OnlyErrors))
			{
				return string.Empty;
			}
			global::System.Text.StringBuilder debugStringBuilder = base.pathHandler.DebugStringBuilder;
			debugStringBuilder.Length = 0;
			base.DebugStringPrefix(logMode, debugStringBuilder);
			if (!base.error)
			{
				debugStringBuilder.Append("\nShortest path was ");
				debugStringBuilder.Append((this.chosenTarget != -1) ? this.nodePaths[this.chosenTarget].Count.ToString() : "undefined");
				debugStringBuilder.Append(" nodes long");
				if (logMode == global::Pathfinding.PathLog.Heavy)
				{
					debugStringBuilder.Append("\nPaths (").Append(this.targetsFound.Length).Append("):");
					for (int i = 0; i < this.targetsFound.Length; i++)
					{
						debugStringBuilder.Append("\n\n\tPath ").Append(i).Append(" Found: ").Append(this.targetsFound[i]);
						if (this.nodePaths[i] != null)
						{
							debugStringBuilder.Append("\n\t\tLength: ");
							debugStringBuilder.Append(this.nodePaths[i].Count);
							global::Pathfinding.GraphNode graphNode = this.nodePaths[i][this.nodePaths[i].Count - 1];
							if (graphNode != null)
							{
								global::Pathfinding.PathNode pathNode = base.pathHandler.GetPathNode(this.endNode);
								if (pathNode != null)
								{
									debugStringBuilder.Append("\n\t\tEnd Node");
									debugStringBuilder.Append("\n\t\t\tG: ");
									debugStringBuilder.Append(pathNode.G);
									debugStringBuilder.Append("\n\t\t\tH: ");
									debugStringBuilder.Append(pathNode.H);
									debugStringBuilder.Append("\n\t\t\tF: ");
									debugStringBuilder.Append(pathNode.F);
									debugStringBuilder.Append("\n\t\t\tPoint: ");
									debugStringBuilder.Append(this.endPoint.ToString());
									debugStringBuilder.Append("\n\t\t\tGraph: ");
									debugStringBuilder.Append(this.endNode.GraphIndex);
								}
								else
								{
									debugStringBuilder.Append("\n\t\tEnd Node: Null");
								}
							}
						}
					}
					debugStringBuilder.Append("\nStart Node");
					debugStringBuilder.Append("\n\tPoint: ");
					debugStringBuilder.Append(this.endPoint.ToString());
					debugStringBuilder.Append("\n\tGraph: ");
					debugStringBuilder.Append(this.startNode.GraphIndex);
					debugStringBuilder.Append("\nBinary Heap size at completion: ");
					debugStringBuilder.AppendLine((base.pathHandler.heap != null) ? (base.pathHandler.heap.numberOfItems - 2).ToString() : "Null");
				}
			}
			base.DebugStringSuffix(logMode, debugStringBuilder);
			return debugStringBuilder.ToString();
		}

		public global::Pathfinding.OnPathDelegate[] callbacks;

		public global::Pathfinding.GraphNode[] targetNodes;

		protected int targetNodeCount;

		public bool[] targetsFound;

		public global::UnityEngine.Vector3[] targetPoints;

		public global::UnityEngine.Vector3[] originalTargetPoints;

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3>[] vectorPaths;

		public global::System.Collections.Generic.List<global::Pathfinding.GraphNode>[] nodePaths;

		public bool pathsForAll = true;

		public int chosenTarget = -1;

		private int sequentialTarget;

		public global::Pathfinding.MultiTargetPath.HeuristicMode heuristicMode = global::Pathfinding.MultiTargetPath.HeuristicMode.Sequential;

		public bool inverted = true;

		public enum HeuristicMode
		{
			None,
			Average,
			MovingAverage,
			Midpoint,
			MovingMidpoint,
			Sequential
		}
	}
}
