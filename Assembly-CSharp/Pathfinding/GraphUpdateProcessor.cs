using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	internal class GraphUpdateProcessor
	{
		public GraphUpdateProcessor(global::AstarPath astar)
		{
			this.astar = astar;
		}

		public event global::System.Action OnGraphsUpdated;

		public bool IsAnyGraphUpdateQueued
		{
			get
			{
				return this.graphUpdateQueue.Count > 0;
			}
		}

		public bool IsAnyGraphUpdateInProgress
		{
			get
			{
				return this.anyGraphUpdateInProgress;
			}
		}

		public global::Pathfinding.AstarWorkItem GetWorkItem()
		{
			return new global::Pathfinding.AstarWorkItem(new global::System.Action(this.QueueGraphUpdatesInternal), new global::System.Func<bool, bool>(this.ProcessGraphUpdates));
		}

		public void EnableMultithreading()
		{
			if (this.graphUpdateThread == null || !this.graphUpdateThread.IsAlive)
			{
				this.graphUpdateThread = new global::System.Threading.Thread(new global::System.Threading.ThreadStart(this.ProcessGraphUpdatesAsync));
				this.graphUpdateThread.IsBackground = true;
				this.graphUpdateThread.Priority = global::System.Threading.ThreadPriority.Lowest;
				this.graphUpdateThread.Start(this);
			}
		}

		public void DisableMultithreading()
		{
			if (this.graphUpdateThread != null && this.graphUpdateThread.IsAlive)
			{
				this.exitAsyncThread.Set();
				if (!this.graphUpdateThread.Join(5000))
				{
					global::UnityEngine.Debug.LogError("Graph update thread did not exit in 5 seconds");
				}
				this.graphUpdateThread = null;
			}
		}

		public void UpdateGraphs(global::Pathfinding.GraphUpdateObject ob)
		{
			this.graphUpdateQueue.Enqueue(ob);
		}

		private void QueueGraphUpdatesInternal()
		{
			bool flag = false;
			while (this.graphUpdateQueue.Count > 0)
			{
				global::Pathfinding.GraphUpdateObject graphUpdateObject = this.graphUpdateQueue.Dequeue();
				if (graphUpdateObject.requiresFloodFill)
				{
					flag = true;
				}
				foreach (object obj in this.astar.astarData.GetUpdateableGraphs())
				{
					global::Pathfinding.IUpdatableGraph updatableGraph = (global::Pathfinding.IUpdatableGraph)obj;
					global::Pathfinding.NavGraph graph = updatableGraph as global::Pathfinding.NavGraph;
					if (graphUpdateObject.nnConstraint == null || graphUpdateObject.nnConstraint.SuitableGraph(this.astar.astarData.GetGraphIndex(graph), graph))
					{
						global::Pathfinding.GraphUpdateProcessor.GUOSingle item = default(global::Pathfinding.GraphUpdateProcessor.GUOSingle);
						item.order = global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder.GraphUpdate;
						item.obj = graphUpdateObject;
						item.graph = updatableGraph;
						this.graphUpdateQueueRegular.Enqueue(item);
					}
				}
			}
			if (flag)
			{
				global::Pathfinding.GraphUpdateProcessor.GUOSingle item2 = default(global::Pathfinding.GraphUpdateProcessor.GUOSingle);
				item2.order = global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder.FloodFill;
				this.graphUpdateQueueRegular.Enqueue(item2);
			}
			global::Pathfinding.GraphModifier.TriggerEvent(global::Pathfinding.GraphModifier.EventType.PreUpdate);
			this.anyGraphUpdateInProgress = true;
		}

		private bool ProcessGraphUpdates(bool force)
		{
			if (force)
			{
				this.asyncGraphUpdatesComplete.WaitOne();
			}
			else if (!this.asyncGraphUpdatesComplete.WaitOne(0))
			{
				return false;
			}
			this.ProcessPostUpdates();
			if (!this.ProcessRegularUpdates(force))
			{
				return false;
			}
			global::Pathfinding.GraphModifier.TriggerEvent(global::Pathfinding.GraphModifier.EventType.PostUpdate);
			if (this.OnGraphsUpdated != null)
			{
				this.OnGraphsUpdated();
			}
			this.anyGraphUpdateInProgress = false;
			return true;
		}

		private bool ProcessRegularUpdates(bool force)
		{
			while (this.graphUpdateQueueRegular.Count > 0)
			{
				global::Pathfinding.GraphUpdateProcessor.GUOSingle item = this.graphUpdateQueueRegular.Peek();
				global::Pathfinding.GraphUpdateThreading graphUpdateThreading = (item.order != global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder.FloodFill) ? item.graph.CanUpdateAsync(item.obj) : global::Pathfinding.GraphUpdateThreading.SeparateThread;
				if (force || !global::UnityEngine.Application.isPlaying || this.graphUpdateThread == null || !this.graphUpdateThread.IsAlive)
				{
					graphUpdateThreading &= (global::Pathfinding.GraphUpdateThreading)(-2);
				}
				if ((graphUpdateThreading & global::Pathfinding.GraphUpdateThreading.UnityInit) != global::Pathfinding.GraphUpdateThreading.UnityThread)
				{
					if (this.StartAsyncUpdatesIfQueued())
					{
						return false;
					}
					item.graph.UpdateAreaInit(item.obj);
				}
				if ((graphUpdateThreading & global::Pathfinding.GraphUpdateThreading.SeparateThread) != global::Pathfinding.GraphUpdateThreading.UnityThread)
				{
					this.graphUpdateQueueRegular.Dequeue();
					this.graphUpdateQueueAsync.Enqueue(item);
					if ((graphUpdateThreading & global::Pathfinding.GraphUpdateThreading.UnityPost) != global::Pathfinding.GraphUpdateThreading.UnityThread && this.StartAsyncUpdatesIfQueued())
					{
						return false;
					}
				}
				else
				{
					if (this.StartAsyncUpdatesIfQueued())
					{
						return false;
					}
					this.graphUpdateQueueRegular.Dequeue();
					if (item.order == global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder.FloodFill)
					{
						this.FloodFill();
					}
					else
					{
						try
						{
							item.graph.UpdateArea(item.obj);
						}
						catch (global::System.Exception arg)
						{
							global::UnityEngine.Debug.LogError("Error while updating graphs\n" + arg);
						}
					}
					if ((graphUpdateThreading & global::Pathfinding.GraphUpdateThreading.UnityPost) != global::Pathfinding.GraphUpdateThreading.UnityThread)
					{
						item.graph.UpdateAreaPost(item.obj);
					}
				}
			}
			return !this.StartAsyncUpdatesIfQueued();
		}

		private bool StartAsyncUpdatesIfQueued()
		{
			if (this.graphUpdateQueueAsync.Count > 0)
			{
				this.asyncGraphUpdatesComplete.Reset();
				this.graphUpdateAsyncEvent.Set();
				return true;
			}
			return false;
		}

		private void ProcessPostUpdates()
		{
			while (this.graphUpdateQueuePost.Count > 0)
			{
				global::Pathfinding.GraphUpdateProcessor.GUOSingle guosingle = this.graphUpdateQueuePost.Dequeue();
				global::Pathfinding.GraphUpdateThreading graphUpdateThreading = guosingle.graph.CanUpdateAsync(guosingle.obj);
				if ((graphUpdateThreading & global::Pathfinding.GraphUpdateThreading.UnityPost) != global::Pathfinding.GraphUpdateThreading.UnityThread)
				{
					try
					{
						guosingle.graph.UpdateAreaPost(guosingle.obj);
					}
					catch (global::System.Exception arg)
					{
						global::UnityEngine.Debug.LogError("Error while updating graphs (post step)\n" + arg);
					}
				}
			}
		}

		private void ProcessGraphUpdatesAsync()
		{
			global::System.Threading.AutoResetEvent[] waitHandles = new global::System.Threading.AutoResetEvent[]
			{
				this.graphUpdateAsyncEvent,
				this.exitAsyncThread
			};
			for (;;)
			{
				int num = global::System.Threading.WaitHandle.WaitAny(waitHandles);
				if (num == 1)
				{
					break;
				}
				while (this.graphUpdateQueueAsync.Count > 0)
				{
					global::Pathfinding.GraphUpdateProcessor.GUOSingle item = this.graphUpdateQueueAsync.Dequeue();
					try
					{
						if (item.order == global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder.GraphUpdate)
						{
							item.graph.UpdateArea(item.obj);
							this.graphUpdateQueuePost.Enqueue(item);
						}
						else
						{
							if (item.order != global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder.FloodFill)
							{
								throw new global::System.NotSupportedException(string.Empty + item.order);
							}
							this.FloodFill();
						}
					}
					catch (global::System.Exception arg)
					{
						global::UnityEngine.Debug.LogError("Exception while updating graphs:\n" + arg);
					}
				}
				this.asyncGraphUpdatesComplete.Set();
			}
			this.graphUpdateQueueAsync.Clear();
			this.asyncGraphUpdatesComplete.Set();
		}

		public void FloodFill(global::Pathfinding.GraphNode seed)
		{
			this.FloodFill(seed, this.lastUniqueAreaIndex + 1U);
			this.lastUniqueAreaIndex += 1U;
		}

		public void FloodFill(global::Pathfinding.GraphNode seed, uint area)
		{
			if (area > 131071U)
			{
				global::UnityEngine.Debug.LogError("Too high area index - The maximum area index is " + 131071U);
				return;
			}
			if (area < 0U)
			{
				global::UnityEngine.Debug.LogError("Too low area index - The minimum area index is 0");
				return;
			}
			global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack = global::Pathfinding.Util.StackPool<global::Pathfinding.GraphNode>.Claim();
			stack.Push(seed);
			seed.Area = area;
			while (stack.Count > 0)
			{
				stack.Pop().FloodFill(stack, area);
			}
			global::Pathfinding.Util.StackPool<global::Pathfinding.GraphNode>.Release(stack);
		}

		[global::UnityEngine.ContextMenu("Flood Fill Graphs")]
		public void FloodFill()
		{
			global::Pathfinding.NavGraph[] graphs = this.astar.graphs;
			if (graphs == null)
			{
				return;
			}
			foreach (global::Pathfinding.NavGraph navGraph in graphs)
			{
				if (navGraph != null)
				{
					navGraph.GetNodes(delegate(global::Pathfinding.GraphNode node)
					{
						node.Area = 0U;
						return true;
					});
				}
			}
			this.lastUniqueAreaIndex = 0U;
			uint area = 0U;
			int forcedSmallAreas = 0;
			global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack = global::Pathfinding.Util.StackPool<global::Pathfinding.GraphNode>.Claim();
			foreach (global::Pathfinding.NavGraph navGraph2 in graphs)
			{
				if (navGraph2 != null)
				{
					global::Pathfinding.GraphNodeDelegateCancelable del = delegate(global::Pathfinding.GraphNode node)
					{
						if (node.Walkable && node.Area == 0U)
						{
							uint area;
							area += 1U;
							area = area;
							if (area > 131071U)
							{
								area -= 1U;
								area = area;
								if (forcedSmallAreas == 0)
								{
									forcedSmallAreas = 1;
								}
								forcedSmallAreas++;
							}
							stack.Clear();
							stack.Push(node);
							int num = 1;
							node.Area = area;
							while (stack.Count > 0)
							{
								num++;
								stack.Pop().FloodFill(stack, area);
							}
						}
						return true;
					};
					navGraph2.GetNodes(del);
				}
			}
			this.lastUniqueAreaIndex = area;
			if (forcedSmallAreas > 0)
			{
				global::UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					forcedSmallAreas,
					" areas had to share IDs. This usually doesn't affect pathfinding in any significant way (you might get 'Searched whole area but could not find target' as a reason for path failure) however some path requests may take longer to calculate (specifically those that fail with the 'Searched whole area' error).The maximum number of areas is ",
					131071U,
					"."
				}));
			}
			global::Pathfinding.Util.StackPool<global::Pathfinding.GraphNode>.Release(stack);
		}

		private readonly global::AstarPath astar;

		private global::System.Threading.Thread graphUpdateThread;

		private bool anyGraphUpdateInProgress;

		private readonly global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateObject> graphUpdateQueue = new global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateObject>();

		private readonly global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateProcessor.GUOSingle> graphUpdateQueueAsync = new global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateProcessor.GUOSingle>();

		private readonly global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateProcessor.GUOSingle> graphUpdateQueuePost = new global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateProcessor.GUOSingle>();

		private readonly global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateProcessor.GUOSingle> graphUpdateQueueRegular = new global::System.Collections.Generic.Queue<global::Pathfinding.GraphUpdateProcessor.GUOSingle>();

		private readonly global::System.Threading.ManualResetEvent asyncGraphUpdatesComplete = new global::System.Threading.ManualResetEvent(true);

		private readonly global::System.Threading.AutoResetEvent graphUpdateAsyncEvent = new global::System.Threading.AutoResetEvent(false);

		private readonly global::System.Threading.AutoResetEvent exitAsyncThread = new global::System.Threading.AutoResetEvent(false);

		private uint lastUniqueAreaIndex;

		private enum GraphUpdateOrder
		{
			GraphUpdate,
			FloodFill
		}

		private struct GUOSingle
		{
			public global::Pathfinding.GraphUpdateProcessor.GraphUpdateOrder order;

			public global::Pathfinding.IUpdatableGraph graph;

			public global::Pathfinding.GraphUpdateObject obj;
		}
	}
}
