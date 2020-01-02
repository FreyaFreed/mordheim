using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public abstract class Path
	{
		public global::Pathfinding.PathHandler pathHandler { get; private set; }

		public global::Pathfinding.PathCompleteState CompleteState
		{
			get
			{
				return this.pathCompleteState;
			}
			protected set
			{
				this.pathCompleteState = value;
			}
		}

		public bool error
		{
			get
			{
				return this.CompleteState == global::Pathfinding.PathCompleteState.Error;
			}
		}

		public string errorLog
		{
			get
			{
				return this._errorLog;
			}
		}

		public global::System.DateTime callTime { get; private set; }

		[global::System.Obsolete("Has been renamed to 'pooled' to use more widely underestood terminology")]
		internal bool recycled
		{
			get
			{
				return this.pooled;
			}
			set
			{
				this.pooled = value;
			}
		}

		public ushort pathID { get; private set; }

		public int[] tagPenalties
		{
			get
			{
				return this.manualTagPenalties;
			}
			set
			{
				if (value == null || value.Length != 32)
				{
					this.manualTagPenalties = null;
					this.internalTagPenalties = global::Pathfinding.Path.ZeroTagPenalties;
				}
				else
				{
					this.manualTagPenalties = value;
					this.internalTagPenalties = value;
				}
			}
		}

		public virtual bool FloodingPath
		{
			get
			{
				return false;
			}
		}

		public float GetTotalLength()
		{
			if (this.vectorPath == null)
			{
				return float.PositiveInfinity;
			}
			float num = 0f;
			for (int i = 0; i < this.vectorPath.Count - 1; i++)
			{
				num += global::UnityEngine.Vector3.Distance(this.vectorPath[i], this.vectorPath[i + 1]);
			}
			return num;
		}

		public global::System.Collections.IEnumerator WaitForPath()
		{
			if (this.GetState() == global::Pathfinding.PathState.Created)
			{
				throw new global::System.InvalidOperationException("This path has not been started yet");
			}
			while (this.GetState() != global::Pathfinding.PathState.Returned)
			{
				yield return null;
			}
			yield break;
		}

		public uint CalculateHScore(global::Pathfinding.GraphNode node)
		{
			switch (this.heuristic)
			{
			case global::Pathfinding.Heuristic.Manhattan:
			{
				global::Pathfinding.Int3 position = node.position;
				uint num = (uint)((float)(global::System.Math.Abs(this.hTarget.x - position.x) + global::System.Math.Abs(this.hTarget.y - position.y) + global::System.Math.Abs(this.hTarget.z - position.z)) * this.heuristicScale);
				if (this.hTargetNode != null)
				{
					num = global::System.Math.Max(num, global::AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
				}
				return num;
			}
			case global::Pathfinding.Heuristic.DiagonalManhattan:
			{
				global::Pathfinding.Int3 @int = this.GetHTarget() - node.position;
				@int.x = global::System.Math.Abs(@int.x);
				@int.y = global::System.Math.Abs(@int.y);
				@int.z = global::System.Math.Abs(@int.z);
				int num2 = global::System.Math.Min(@int.x, @int.z);
				int num3 = global::System.Math.Max(@int.x, @int.z);
				uint num = (uint)((float)(14 * num2 / 10 + (num3 - num2) + @int.y) * this.heuristicScale);
				if (this.hTargetNode != null)
				{
					num = global::System.Math.Max(num, global::AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
				}
				return num;
			}
			case global::Pathfinding.Heuristic.Euclidean:
			{
				uint num = (uint)((float)(this.GetHTarget() - node.position).costMagnitude * this.heuristicScale);
				if (this.hTargetNode != null)
				{
					num = global::System.Math.Max(num, global::AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
				}
				return num;
			}
			default:
				return 0U;
			}
		}

		public uint GetTagPenalty(int tag)
		{
			return (uint)this.internalTagPenalties[tag];
		}

		public global::Pathfinding.Int3 GetHTarget()
		{
			return this.hTarget;
		}

		public bool CanTraverse(global::Pathfinding.GraphNode node)
		{
			return node.Walkable && (this.enabledTags >> (int)node.Tag & 1) != 0;
		}

		public uint GetTraversalCost(global::Pathfinding.GraphNode node)
		{
			return this.GetTagPenalty((int)node.Tag) + node.Penalty;
		}

		public virtual uint GetConnectionSpecialCost(global::Pathfinding.GraphNode a, global::Pathfinding.GraphNode b, uint currentCost)
		{
			return currentCost;
		}

		public bool IsDone()
		{
			return this.CompleteState != global::Pathfinding.PathCompleteState.NotCalculated;
		}

		public void AdvanceState(global::Pathfinding.PathState s)
		{
			object obj = this.stateLock;
			lock (obj)
			{
				this.state = (global::Pathfinding.PathState)global::System.Math.Max((int)this.state, (int)s);
			}
		}

		public global::Pathfinding.PathState GetState()
		{
			return this.state;
		}

		[global::System.Diagnostics.Conditional("DISABLED")]
		internal void LogError(string msg)
		{
			if (global::AstarPath.active.logPathResults != global::Pathfinding.PathLog.None)
			{
				this._errorLog += msg;
			}
			if (global::AstarPath.active.logPathResults != global::Pathfinding.PathLog.None && global::AstarPath.active.logPathResults != global::Pathfinding.PathLog.InGame)
			{
				global::UnityEngine.Debug.LogWarning(msg);
			}
		}

		internal void ForceLogError(string msg)
		{
			this.Error();
			this._errorLog += msg;
			global::UnityEngine.Debug.LogError(msg);
		}

		internal void Log(string msg)
		{
			if (global::AstarPath.active.logPathResults != global::Pathfinding.PathLog.None)
			{
				this._errorLog += msg;
			}
		}

		public void Error()
		{
			this.CompleteState = global::Pathfinding.PathCompleteState.Error;
		}

		private void ErrorCheck()
		{
			if (!this.hasBeenReset)
			{
				throw new global::System.Exception("The path has never been reset. Use pooling API or call Reset() after creating the path with the default constructor.");
			}
			if (this.pooled)
			{
				throw new global::System.Exception("The path is currently in a path pool. Are you sending the path for calculation twice?");
			}
			if (this.pathHandler == null)
			{
				throw new global::System.Exception("Field pathHandler is not set. Please report this bug.");
			}
			if (this.GetState() > global::Pathfinding.PathState.Processing)
			{
				throw new global::System.Exception("This path has already been processed. Do not request a path with the same path object twice.");
			}
		}

		public virtual void OnEnterPool()
		{
			if (this.vectorPath != null)
			{
				global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(this.vectorPath);
			}
			if (this.path != null)
			{
				global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(this.path);
			}
			this.vectorPath = null;
			this.path = null;
		}

		public virtual void Reset()
		{
			if (object.ReferenceEquals(global::AstarPath.active, null))
			{
				throw new global::System.NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
			}
			this.hasBeenReset = true;
			this.state = global::Pathfinding.PathState.Created;
			this.releasedNotSilent = false;
			this.pathHandler = null;
			this.callback = null;
			this._errorLog = string.Empty;
			this.pathCompleteState = global::Pathfinding.PathCompleteState.NotCalculated;
			this.path = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			this.vectorPath = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			this.currentR = null;
			this.duration = 0f;
			this.searchIterations = 0;
			this.searchedNodes = 0;
			this.nnConstraint = global::Pathfinding.PathNNConstraint.Default;
			this.next = null;
			this.heuristic = global::AstarPath.active.heuristic;
			this.heuristicScale = global::AstarPath.active.heuristicScale;
			this.enabledTags = -1;
			this.tagPenalties = null;
			this.callTime = global::System.DateTime.UtcNow;
			this.pathID = global::AstarPath.active.GetNextPathID();
			this.hTarget = global::Pathfinding.Int3.zero;
			this.hTargetNode = null;
		}

		protected bool HasExceededTime(int searchedNodes, long targetTime)
		{
			return global::System.DateTime.UtcNow.Ticks >= targetTime;
		}

		public void Claim(object o)
		{
			if (object.ReferenceEquals(o, null))
			{
				throw new global::System.ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimed.Count; i++)
			{
				if (object.ReferenceEquals(this.claimed[i], o))
				{
					throw new global::System.ArgumentException("You have already claimed the path with that object (" + o + "). Are you claiming the path with the same object twice?");
				}
			}
			this.claimed.Add(o);
		}

		[global::System.Obsolete("Use Release(o, true) instead")]
		public void ReleaseSilent(object o)
		{
			this.Release(o, true);
		}

		public void Release(object o, bool silent = false)
		{
			if (o == null)
			{
				throw new global::System.ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimed.Count; i++)
			{
				if (object.ReferenceEquals(this.claimed[i], o))
				{
					this.claimed.RemoveAt(i);
					if (!silent)
					{
						this.releasedNotSilent = true;
					}
					if (this.claimed.Count == 0 && this.releasedNotSilent)
					{
						global::Pathfinding.PathPool.Pool(this);
					}
					return;
				}
			}
			if (this.claimed.Count == 0)
			{
				throw new global::System.ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + o + ") twice?\nCheck out the documentation on path pooling for help.");
			}
			throw new global::System.ArgumentException("You are releasing a path which has not been claimed with this object (" + o + "). Are you releasing the path with the same object twice?\nCheck out the documentation on path pooling for help.");
		}

		protected virtual void Trace(global::Pathfinding.PathNode from)
		{
			global::Pathfinding.PathNode pathNode = from;
			int num = 0;
			while (pathNode != null)
			{
				pathNode = pathNode.parent;
				num++;
				if (num > 2048)
				{
					global::UnityEngine.Debug.LogWarning("Infinite loop? >2048 node path. Remove this message if you really have that long paths (Path.cs, Trace method)");
					break;
				}
			}
			if (this.path.Capacity < num)
			{
				this.path.Capacity = num;
			}
			if (this.vectorPath.Capacity < num)
			{
				this.vectorPath.Capacity = num;
			}
			pathNode = from;
			for (int i = 0; i < num; i++)
			{
				this.path.Add(pathNode.node);
				pathNode = pathNode.parent;
			}
			int num2 = num / 2;
			for (int j = 0; j < num2; j++)
			{
				global::Pathfinding.GraphNode value = this.path[j];
				this.path[j] = this.path[num - j - 1];
				this.path[num - j - 1] = value;
			}
			for (int k = 0; k < num; k++)
			{
				this.vectorPath.Add((global::UnityEngine.Vector3)this.path[k].position);
			}
		}

		protected void DebugStringPrefix(global::Pathfinding.PathLog logMode, global::System.Text.StringBuilder text)
		{
			text.Append((!this.error) ? "Path Completed : " : "Path Failed : ");
			text.Append("Computation Time ");
			text.Append(this.duration.ToString((logMode != global::Pathfinding.PathLog.Heavy) ? "0.00 ms " : "0.000 ms "));
			text.Append("Searched Nodes ").Append(this.searchedNodes);
			if (!this.error)
			{
				text.Append(" Path Length ");
				text.Append((this.path != null) ? this.path.Count.ToString() : "Null");
				if (logMode == global::Pathfinding.PathLog.Heavy)
				{
					text.Append("\nSearch Iterations ").Append(this.searchIterations);
				}
			}
		}

		protected void DebugStringSuffix(global::Pathfinding.PathLog logMode, global::System.Text.StringBuilder text)
		{
			if (this.error)
			{
				text.Append("\nError: ").Append(this.errorLog);
			}
			if (logMode == global::Pathfinding.PathLog.Heavy && !global::AstarPath.active.IsUsingMultithreading)
			{
				text.Append("\nCallback references ");
				if (this.callback != null)
				{
					text.Append(this.callback.Target.GetType().FullName).AppendLine();
				}
				else
				{
					text.AppendLine("NULL");
				}
			}
			text.Append("\nPath Number ").Append(this.pathID).Append(" (unique id)");
		}

		public virtual string DebugString(global::Pathfinding.PathLog logMode)
		{
			if (logMode == global::Pathfinding.PathLog.None || (!this.error && logMode == global::Pathfinding.PathLog.OnlyErrors))
			{
				return string.Empty;
			}
			global::System.Text.StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
			debugStringBuilder.Length = 0;
			this.DebugStringPrefix(logMode, debugStringBuilder);
			this.DebugStringSuffix(logMode, debugStringBuilder);
			return debugStringBuilder.ToString();
		}

		public virtual void ReturnPath()
		{
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		internal void PrepareBase(global::Pathfinding.PathHandler pathHandler)
		{
			if (pathHandler.PathID > this.pathID)
			{
				pathHandler.ClearPathIDs();
			}
			this.pathHandler = pathHandler;
			pathHandler.InitializeForPath(this);
			if (this.internalTagPenalties == null || this.internalTagPenalties.Length != 32)
			{
				this.internalTagPenalties = global::Pathfinding.Path.ZeroTagPenalties;
			}
			try
			{
				this.ErrorCheck();
			}
			catch (global::System.Exception ex)
			{
				this.ForceLogError(string.Concat(new object[]
				{
					"Exception in path ",
					this.pathID,
					"\n",
					ex
				}));
			}
		}

		public abstract void Prepare();

		public virtual void Cleanup()
		{
		}

		public abstract void Initialize();

		public abstract void CalculateStep(long targetTick);

		public global::Pathfinding.OnPathDelegate callback;

		public global::Pathfinding.OnPathDelegate immediateCallback;

		private global::Pathfinding.PathState state;

		private object stateLock = new object();

		private global::Pathfinding.PathCompleteState pathCompleteState;

		private string _errorLog = string.Empty;

		public global::System.Collections.Generic.List<global::Pathfinding.GraphNode> path;

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath;

		protected float maxFrameTime;

		protected global::Pathfinding.PathNode currentR;

		public float duration;

		public int searchIterations;

		public int searchedNodes;

		internal bool pooled;

		protected bool hasBeenReset;

		public global::Pathfinding.NNConstraint nnConstraint = global::Pathfinding.PathNNConstraint.Default;

		internal global::Pathfinding.Path next;

		public global::Pathfinding.Heuristic heuristic;

		public float heuristicScale = 1f;

		protected global::Pathfinding.GraphNode hTargetNode;

		protected global::Pathfinding.Int3 hTarget;

		public int enabledTags = -1;

		private static readonly int[] ZeroTagPenalties = new int[32];

		protected int[] internalTagPenalties;

		protected int[] manualTagPenalties;

		private global::System.Collections.Generic.List<object> claimed = new global::System.Collections.Generic.List<object>();

		private bool releasedNotSilent;
	}
}
