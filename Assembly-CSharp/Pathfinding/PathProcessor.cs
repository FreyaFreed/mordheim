using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Pathfinding
{
	internal class PathProcessor
	{
		public PathProcessor(global::AstarPath astar, global::Pathfinding.PathReturnQueue returnQueue, int processors, bool multithreaded)
		{
			this.astar = astar;
			this.returnQueue = returnQueue;
			if (processors < 0)
			{
				throw new global::System.ArgumentOutOfRangeException("processors");
			}
			if (!multithreaded && processors != 1)
			{
				throw new global::System.Exception("Only a single non-multithreaded processor is allowed");
			}
			this.queue = new global::Pathfinding.ThreadControlQueue(processors);
			this.threadInfos = new global::Pathfinding.PathThreadInfo[processors];
			for (int i = 0; i < processors; i++)
			{
				this.threadInfos[i] = new global::Pathfinding.PathThreadInfo(i, astar, new global::Pathfinding.PathHandler(i, processors));
			}
			if (multithreaded)
			{
				this.threads = new global::System.Threading.Thread[processors];
				for (int j = 0; j < processors; j++)
				{
					int threadIndex = j;
					global::System.Threading.Thread thread = new global::System.Threading.Thread(delegate()
					{
						this.CalculatePathsThreaded(this.threadInfos[threadIndex]);
					});
					thread.Name = "Pathfinding Thread " + j;
					thread.IsBackground = true;
					this.threads[j] = thread;
					thread.Start();
				}
			}
			else
			{
				this.threadCoroutine = this.CalculatePaths(this.threadInfos[0]);
			}
		}

		public event global::System.Action<global::Pathfinding.Path> OnPathPreSearch;

		public event global::System.Action<global::Pathfinding.Path> OnPathPostSearch;

		public int NumThreads
		{
			get
			{
				return this.threadInfos.Length;
			}
		}

		public bool IsUsingMultithreading
		{
			get
			{
				return this.threads != null;
			}
		}

		public void BlockUntilPathQueueBlocked()
		{
			this.queue.Block();
			if (!global::UnityEngine.Application.isPlaying)
			{
				return;
			}
			while (!this.queue.AllReceiversBlocked)
			{
				if (this.IsUsingMultithreading)
				{
					global::System.Threading.Thread.Sleep(1);
				}
				else
				{
					this.TickNonMultithreaded();
				}
			}
		}

		public void TickNonMultithreaded()
		{
			if (this.threadCoroutine != null)
			{
				try
				{
					this.threadCoroutine.MoveNext();
				}
				catch (global::System.Exception ex)
				{
					this.threadCoroutine = null;
					if (!(ex is global::Pathfinding.ThreadControlQueue.QueueTerminationException))
					{
						global::UnityEngine.Debug.LogException(ex);
						global::UnityEngine.Debug.LogError("Unhandled exception during pathfinding. Terminating.");
						this.queue.TerminateReceivers();
						try
						{
							this.queue.PopNoBlock(false);
						}
						catch
						{
						}
					}
				}
			}
		}

		public void JoinThreads()
		{
			if (this.threads != null)
			{
				for (int i = 0; i < this.threads.Length; i++)
				{
					if (!this.threads[i].Join(50))
					{
						global::UnityEngine.Debug.LogError("Could not terminate pathfinding thread[" + i + "] in 50ms, trying Thread.Abort");
						this.threads[i].Abort();
					}
				}
			}
		}

		public void AbortThreads()
		{
			if (this.threads == null)
			{
				return;
			}
			for (int i = 0; i < this.threads.Length; i++)
			{
				if (this.threads[i] != null && this.threads[i].IsAlive)
				{
					this.threads[i].Abort();
				}
			}
		}

		public int GetNewNodeIndex()
		{
			return (this.nodeIndexPool.Count <= 0) ? this.nextNodeIndex++ : this.nodeIndexPool.Pop();
		}

		public void InitializeNode(global::Pathfinding.GraphNode node)
		{
			if (!this.queue.AllReceiversBlocked)
			{
				throw new global::System.Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.php#direct");
			}
			for (int i = 0; i < this.threadInfos.Length; i++)
			{
				this.threadInfos[i].runData.InitializeNode(node);
			}
		}

		public void DestroyNode(global::Pathfinding.GraphNode node)
		{
			if (node.NodeIndex == -1)
			{
				return;
			}
			this.nodeIndexPool.Push(node.NodeIndex);
			for (int i = 0; i < this.threadInfos.Length; i++)
			{
				this.threadInfos[i].runData.DestroyNode(node);
			}
		}

		private void CalculatePathsThreaded(global::Pathfinding.PathThreadInfo threadInfo)
		{
			try
			{
				global::Pathfinding.PathHandler runData = threadInfo.runData;
				if (runData.nodes == null)
				{
					throw new global::System.NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
				}
				long num = (long)(this.astar.maxFrameTime * 10000f);
				long num2 = global::System.DateTime.UtcNow.Ticks + num;
				for (;;)
				{
					global::Pathfinding.Path path = this.queue.Pop();
					num = (long)(this.astar.maxFrameTime * 10000f);
					path.PrepareBase(runData);
					path.AdvanceState(global::Pathfinding.PathState.Processing);
					if (this.OnPathPreSearch != null)
					{
						this.OnPathPreSearch(path);
					}
					long ticks = global::System.DateTime.UtcNow.Ticks;
					long num3 = 0L;
					path.Prepare();
					if (!path.IsDone())
					{
						this.astar.debugPath = path;
						path.Initialize();
						while (!path.IsDone())
						{
							path.CalculateStep(num2);
							path.searchIterations++;
							if (path.IsDone())
							{
								break;
							}
							num3 += global::System.DateTime.UtcNow.Ticks - ticks;
							global::System.Threading.Thread.Sleep(0);
							ticks = global::System.DateTime.UtcNow.Ticks;
							num2 = ticks + num;
							if (this.queue.IsTerminating)
							{
								path.Error();
							}
						}
						num3 += global::System.DateTime.UtcNow.Ticks - ticks;
						path.duration = (float)num3 * 0.0001f;
					}
					path.Cleanup();
					if (path.immediateCallback != null)
					{
						path.immediateCallback(path);
					}
					if (this.OnPathPostSearch != null)
					{
						this.OnPathPostSearch(path);
					}
					this.returnQueue.Enqueue(path);
					path.AdvanceState(global::Pathfinding.PathState.ReturnQueue);
					if (global::System.DateTime.UtcNow.Ticks > num2)
					{
						global::System.Threading.Thread.Sleep(1);
						num2 = global::System.DateTime.UtcNow.Ticks + num;
					}
				}
			}
			catch (global::System.Exception ex)
			{
				if (ex is global::System.Threading.ThreadAbortException || ex is global::Pathfinding.ThreadControlQueue.QueueTerminationException)
				{
					if (this.astar.logPathResults == global::Pathfinding.PathLog.Heavy)
					{
						global::UnityEngine.Debug.LogWarning("Shutting down pathfinding thread #" + threadInfo.threadIndex);
					}
					return;
				}
				global::UnityEngine.Debug.LogException(ex);
				global::UnityEngine.Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				this.queue.TerminateReceivers();
			}
			global::UnityEngine.Debug.LogError("Error : This part should never be reached.");
			this.queue.ReceiverTerminated();
		}

		private global::System.Collections.IEnumerator CalculatePaths(global::Pathfinding.PathThreadInfo threadInfo)
		{
			int numPaths = 0;
			global::Pathfinding.PathHandler runData = threadInfo.runData;
			if (runData.nodes == null)
			{
				throw new global::System.NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
			}
			long maxTicks = (long)(this.astar.maxFrameTime * 10000f);
			long targetTick = global::System.DateTime.UtcNow.Ticks + maxTicks;
			for (;;)
			{
				global::Pathfinding.Path p = null;
				bool blockedBefore = false;
				while (p == null)
				{
					try
					{
						p = this.queue.PopNoBlock(blockedBefore);
						blockedBefore |= (p == null);
					}
					catch (global::Pathfinding.ThreadControlQueue.QueueTerminationException)
					{
						yield break;
					}
					if (p == null)
					{
						yield return null;
					}
				}
				maxTicks = (long)(this.astar.maxFrameTime * 10000f);
				p.PrepareBase(runData);
				p.AdvanceState(global::Pathfinding.PathState.Processing);
				global::System.Action<global::Pathfinding.Path> tmpOnPathPreSearch = this.OnPathPreSearch;
				if (tmpOnPathPreSearch != null)
				{
					tmpOnPathPreSearch(p);
				}
				numPaths++;
				long startTicks = global::System.DateTime.UtcNow.Ticks;
				long totalTicks = 0L;
				p.Prepare();
				if (!p.IsDone())
				{
					this.astar.debugPath = p;
					p.Initialize();
					while (!p.IsDone())
					{
						p.CalculateStep(targetTick);
						p.searchIterations++;
						if (p.IsDone())
						{
							break;
						}
						totalTicks += global::System.DateTime.UtcNow.Ticks - startTicks;
						yield return null;
						startTicks = global::System.DateTime.UtcNow.Ticks;
						if (this.queue.IsTerminating)
						{
							p.Error();
						}
						targetTick = global::System.DateTime.UtcNow.Ticks + maxTicks;
					}
					totalTicks += global::System.DateTime.UtcNow.Ticks - startTicks;
					p.duration = (float)totalTicks * 0.0001f;
				}
				p.Cleanup();
				global::Pathfinding.OnPathDelegate tmpImmediateCallback = p.immediateCallback;
				if (tmpImmediateCallback != null)
				{
					tmpImmediateCallback(p);
				}
				global::System.Action<global::Pathfinding.Path> tmpOnPathPostSearch = this.OnPathPostSearch;
				if (tmpOnPathPostSearch != null)
				{
					tmpOnPathPostSearch(p);
				}
				this.returnQueue.Enqueue(p);
				p.AdvanceState(global::Pathfinding.PathState.ReturnQueue);
				if (global::System.DateTime.UtcNow.Ticks > targetTick)
				{
					yield return null;
					targetTick = global::System.DateTime.UtcNow.Ticks + maxTicks;
					numPaths = 0;
				}
			}
			yield break;
		}

		public readonly global::Pathfinding.ThreadControlQueue queue;

		private readonly global::AstarPath astar;

		private readonly global::Pathfinding.PathReturnQueue returnQueue;

		private readonly global::Pathfinding.PathThreadInfo[] threadInfos;

		private readonly global::System.Threading.Thread[] threads;

		private global::System.Collections.IEnumerator threadCoroutine;

		private int nextNodeIndex = 1;

		private readonly global::System.Collections.Generic.Stack<int> nodeIndexPool = new global::System.Collections.Generic.Stack<int>();
	}
}
