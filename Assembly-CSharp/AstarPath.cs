using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Pathfinding;
using UnityEngine;

[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_astar_path.php")]
[global::UnityEngine.AddComponentMenu("Pathfinding/Pathfinder")]
[global::UnityEngine.ExecuteInEditMode]
public class AstarPath : global::UnityEngine.MonoBehaviour
{
	private AstarPath()
	{
		this.pathProcessor = new global::Pathfinding.PathProcessor(this, this.pathReturnQueue, 0, true);
		this.pathReturnQueue = new global::Pathfinding.PathReturnQueue(this);
		this.workItems = new global::Pathfinding.WorkItemProcessor(this);
		this.graphUpdates = new global::Pathfinding.GraphUpdateProcessor(this);
		this.graphUpdates.OnGraphsUpdated += delegate()
		{
			if (global::AstarPath.OnGraphsUpdated != null)
			{
				global::AstarPath.OnGraphsUpdated(this);
			}
		};
	}

	public static global::System.Version Version
	{
		get
		{
			return new global::System.Version(3, 8, 5);
		}
	}

	[global::System.Obsolete]
	public global::System.Type[] graphTypes
	{
		get
		{
			return this.astarData.graphTypes;
		}
	}

	public global::Pathfinding.NavGraph[] graphs
	{
		get
		{
			if (this.astarData == null)
			{
				this.astarData = new global::Pathfinding.AstarData();
			}
			return this.astarData.graphs;
		}
		set
		{
			if (this.astarData == null)
			{
				this.astarData = new global::Pathfinding.AstarData();
			}
			this.astarData.graphs = value;
		}
	}

	public float maxNearestNodeDistanceSqr
	{
		get
		{
			return this.maxNearestNodeDistance * this.maxNearestNodeDistance;
		}
	}

	[global::System.Obsolete("This field has been renamed to 'batchGraphUpdates'")]
	public bool limitGraphUpdates
	{
		get
		{
			return this.batchGraphUpdates;
		}
		set
		{
			this.batchGraphUpdates = value;
		}
	}

	[global::System.Obsolete("This field has been renamed to 'graphUpdateBatchingInterval'")]
	public float maxGraphUpdateFreq
	{
		get
		{
			return this.graphUpdateBatchingInterval;
		}
		set
		{
			this.graphUpdateBatchingInterval = value;
		}
	}

	public float lastScanTime { get; private set; }

	public global::Pathfinding.PathHandler debugPathData
	{
		get
		{
			if (this.debugPath == null)
			{
				return null;
			}
			return this.debugPath.pathHandler;
		}
	}

	public bool isScanning { get; private set; }

	public int NumParallelThreads
	{
		get
		{
			return this.pathProcessor.NumThreads;
		}
	}

	public bool IsUsingMultithreading
	{
		get
		{
			return this.pathProcessor.IsUsingMultithreading;
		}
	}

	[global::System.Obsolete("Fixed grammar, use IsAnyGraphUpdateQueued instead")]
	public bool IsAnyGraphUpdatesQueued
	{
		get
		{
			return this.IsAnyGraphUpdateQueued;
		}
	}

	public bool IsAnyGraphUpdateQueued
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateQueued;
		}
	}

	public bool IsAnyGraphUpdateInProgress
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateInProgress;
		}
	}

	public bool IsAnyWorkItemInProgress
	{
		get
		{
			return this.workItems.workItemsInProgress;
		}
	}

	public string[] GetTagNames()
	{
		if (this.tagNames == null || this.tagNames.Length != 32)
		{
			this.tagNames = new string[32];
			for (int i = 0; i < this.tagNames.Length; i++)
			{
				this.tagNames[i] = string.Empty + i;
			}
			this.tagNames[0] = "Basic Ground";
		}
		return this.tagNames;
	}

	public static string[] FindTagNames()
	{
		if (global::AstarPath.active != null)
		{
			return global::AstarPath.active.GetTagNames();
		}
		global::AstarPath astarPath = global::UnityEngine.Object.FindObjectOfType<global::AstarPath>();
		if (astarPath != null)
		{
			global::AstarPath.active = astarPath;
			return astarPath.GetTagNames();
		}
		return new string[]
		{
			"There is no AstarPath component in the scene"
		};
	}

	internal ushort GetNextPathID()
	{
		if (this.nextFreePathID == 0)
		{
			this.nextFreePathID += 1;
			global::UnityEngine.Debug.Log("65K cleanup (this message is harmless, it just means you have searched a lot of paths)");
			if (global::AstarPath.On65KOverflow != null)
			{
				global::System.Action on65KOverflow = global::AstarPath.On65KOverflow;
				global::AstarPath.On65KOverflow = null;
				on65KOverflow();
			}
		}
		ushort result;
		this.nextFreePathID = (result = this.nextFreePathID) + 1;
		return result;
	}

	private void RecalculateDebugLimits()
	{
		this.debugFloor = float.PositiveInfinity;
		this.debugRoof = float.NegativeInfinity;
		for (int i = 0; i < this.graphs.Length; i++)
		{
			if (this.graphs[i] != null && this.graphs[i].drawGizmos)
			{
				this.graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
				{
					if (!this.showSearchTree || this.debugPathData == null || global::Pathfinding.NavGraph.InSearchTree(node, this.debugPath))
					{
						global::Pathfinding.PathNode pathNode = (this.debugPathData == null) ? null : this.debugPathData.GetPathNode(node);
						if (pathNode != null || this.debugMode == global::Pathfinding.GraphDebugMode.Penalty)
						{
							switch (this.debugMode)
							{
							case global::Pathfinding.GraphDebugMode.G:
								this.debugFloor = global::UnityEngine.Mathf.Min(this.debugFloor, pathNode.G);
								this.debugRoof = global::UnityEngine.Mathf.Max(this.debugRoof, pathNode.G);
								break;
							case global::Pathfinding.GraphDebugMode.H:
								this.debugFloor = global::UnityEngine.Mathf.Min(this.debugFloor, pathNode.H);
								this.debugRoof = global::UnityEngine.Mathf.Max(this.debugRoof, pathNode.H);
								break;
							case global::Pathfinding.GraphDebugMode.F:
								this.debugFloor = global::UnityEngine.Mathf.Min(this.debugFloor, pathNode.F);
								this.debugRoof = global::UnityEngine.Mathf.Max(this.debugRoof, pathNode.F);
								break;
							case global::Pathfinding.GraphDebugMode.Penalty:
								this.debugFloor = global::UnityEngine.Mathf.Min(this.debugFloor, node.Penalty);
								this.debugRoof = global::UnityEngine.Mathf.Max(this.debugRoof, node.Penalty);
								break;
							}
						}
					}
					return true;
				});
			}
		}
		if (float.IsInfinity(this.debugFloor))
		{
			this.debugFloor = 0f;
			this.debugRoof = 1f;
		}
		if (this.debugRoof - this.debugFloor < 1f)
		{
			this.debugRoof += 1f;
		}
	}

	private void OnDrawGizmos()
	{
		if (this.isScanning)
		{
			return;
		}
		if (global::AstarPath.active == null)
		{
			global::AstarPath.active = this;
		}
		else if (global::AstarPath.active != this)
		{
			return;
		}
		if (this.graphs == null)
		{
			return;
		}
		if (this.workItems.workItemsInProgress)
		{
			return;
		}
		if (this.showNavGraphs && !this.manualDebugFloorRoof)
		{
			this.RecalculateDebugLimits();
		}
		for (int i = 0; i < this.graphs.Length; i++)
		{
			if (this.graphs[i] != null && this.graphs[i].drawGizmos)
			{
				this.graphs[i].OnDrawGizmos(this.showNavGraphs);
			}
		}
		if (this.showNavGraphs)
		{
			this.euclideanEmbedding.OnDrawGizmos();
			if (this.showUnwalkableNodes)
			{
				global::UnityEngine.Gizmos.color = global::Pathfinding.AstarColor.UnwalkableNode;
				global::Pathfinding.GraphNodeDelegateCancelable del = new global::Pathfinding.GraphNodeDelegateCancelable(this.DrawUnwalkableNode);
				for (int j = 0; j < this.graphs.Length; j++)
				{
					if (this.graphs[j] != null && this.graphs[j].drawGizmos)
					{
						this.graphs[j].GetNodes(del);
					}
				}
			}
		}
		if (this.OnDrawGizmosCallback != null)
		{
			this.OnDrawGizmosCallback();
		}
	}

	private bool DrawUnwalkableNode(global::Pathfinding.GraphNode node)
	{
		if (!node.Walkable)
		{
			global::UnityEngine.Gizmos.DrawCube((global::UnityEngine.Vector3)node.position, global::UnityEngine.Vector3.one * this.unwalkableNodeDebugSize);
		}
		return true;
	}

	private void OnGUI()
	{
		if (this.logPathResults == global::Pathfinding.PathLog.InGame && this.inGameDebugPath != string.Empty)
		{
			global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
		}
	}

	internal void Log(string s)
	{
		if (object.ReferenceEquals(global::AstarPath.active, null))
		{
			global::UnityEngine.Debug.Log("No AstarPath object was found : " + s);
			return;
		}
		if (global::AstarPath.active.logPathResults != global::Pathfinding.PathLog.None && global::AstarPath.active.logPathResults != global::Pathfinding.PathLog.OnlyErrors)
		{
			global::UnityEngine.Debug.Log(s);
		}
	}

	private void LogPathResults(global::Pathfinding.Path p)
	{
		if (this.logPathResults == global::Pathfinding.PathLog.None || (this.logPathResults == global::Pathfinding.PathLog.OnlyErrors && !p.error))
		{
			return;
		}
		string message = p.DebugString(this.logPathResults);
		if (this.logPathResults == global::Pathfinding.PathLog.InGame)
		{
			this.inGameDebugPath = message;
		}
		else
		{
			global::UnityEngine.Debug.Log(message);
		}
	}

	private void Update()
	{
		if (!global::UnityEngine.Application.isPlaying)
		{
			return;
		}
		if (!this.isScanning)
		{
			this.PerformBlockingActions(false, true);
		}
		this.pathProcessor.TickNonMultithreaded();
		this.pathReturnQueue.ReturnPaths(true);
	}

	private void PerformBlockingActions(bool force = false, bool unblockOnComplete = true)
	{
		if (this.pathProcessor.queue.AllReceiversBlocked)
		{
			this.pathReturnQueue.ReturnPaths(false);
			if (global::AstarPath.OnThreadSafeCallback != null)
			{
				global::System.Action onThreadSafeCallback = global::AstarPath.OnThreadSafeCallback;
				global::AstarPath.OnThreadSafeCallback = null;
				onThreadSafeCallback();
			}
			if (this.pathProcessor.queue.AllReceiversBlocked && this.workItems.ProcessWorkItems(force))
			{
				this.workItemsQueued = false;
				if (unblockOnComplete)
				{
					if (this.euclideanEmbedding.dirty)
					{
						this.euclideanEmbedding.RecalculateCosts();
					}
					this.pathProcessor.queue.Unblock();
				}
			}
		}
	}

	[global::System.Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
	public void QueueWorkItemFloodFill()
	{
		throw new global::System.Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
	}

	[global::System.Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
	public void EnsureValidFloodFill()
	{
		throw new global::System.Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
	}

	public void AddWorkItem(global::Pathfinding.AstarWorkItem itm)
	{
		this.workItems.AddWorkItem(itm);
		if (!this.workItemsQueued)
		{
			this.workItemsQueued = true;
			if (!this.isScanning)
			{
				this.InterruptPathfinding();
			}
		}
	}

	public void QueueGraphUpdates()
	{
		if (!this.graphUpdatesWorkItemAdded)
		{
			this.graphUpdatesWorkItemAdded = true;
			global::Pathfinding.AstarWorkItem workItem = this.graphUpdates.GetWorkItem();
			this.AddWorkItem(new global::Pathfinding.AstarWorkItem(delegate()
			{
				this.graphUpdatesWorkItemAdded = false;
				this.lastGraphUpdate = global::UnityEngine.Time.realtimeSinceStartup;
				this.debugPath = null;
				workItem.init();
			}, workItem.update));
		}
	}

	private global::System.Collections.IEnumerator DelayedGraphUpdate()
	{
		this.graphUpdateRoutineRunning = true;
		yield return new global::UnityEngine.WaitForSeconds(this.graphUpdateBatchingInterval - (global::UnityEngine.Time.realtimeSinceStartup - this.lastGraphUpdate));
		this.QueueGraphUpdates();
		this.graphUpdateRoutineRunning = false;
		yield break;
	}

	public void UpdateGraphs(global::UnityEngine.Bounds bounds, float t)
	{
		this.UpdateGraphs(new global::Pathfinding.GraphUpdateObject(bounds), t);
	}

	public void UpdateGraphs(global::Pathfinding.GraphUpdateObject ob, float t)
	{
		base.StartCoroutine(this.UpdateGraphsInteral(ob, t));
	}

	private global::System.Collections.IEnumerator UpdateGraphsInteral(global::Pathfinding.GraphUpdateObject ob, float t)
	{
		yield return new global::UnityEngine.WaitForSeconds(t);
		this.UpdateGraphs(ob);
		yield break;
	}

	public void UpdateGraphs(global::UnityEngine.Bounds bounds)
	{
		this.UpdateGraphs(new global::Pathfinding.GraphUpdateObject(bounds));
	}

	public void UpdateGraphs(global::Pathfinding.GraphUpdateObject ob)
	{
		this.graphUpdates.UpdateGraphs(ob);
		if (this.batchGraphUpdates && global::UnityEngine.Time.realtimeSinceStartup - this.lastGraphUpdate < this.graphUpdateBatchingInterval)
		{
			if (!this.graphUpdateRoutineRunning)
			{
				base.StartCoroutine(this.DelayedGraphUpdate());
			}
		}
		else
		{
			this.QueueGraphUpdates();
		}
	}

	public void FlushGraphUpdates()
	{
		if (this.IsAnyGraphUpdateQueued)
		{
			this.QueueGraphUpdates();
			this.FlushWorkItems();
		}
	}

	public void FlushWorkItems()
	{
		this.FlushWorkItemsInternal(true);
	}

	[global::System.Obsolete("Use FlushWorkItems() instead or use FlushWorkItemsInternal if you really need to")]
	public void FlushWorkItems(bool unblockOnComplete, bool block)
	{
		this.BlockUntilPathQueueBlocked();
		this.PerformBlockingActions(block, unblockOnComplete);
	}

	internal void FlushWorkItemsInternal(bool unblockOnComplete)
	{
		this.BlockUntilPathQueueBlocked();
		this.PerformBlockingActions(true, unblockOnComplete);
	}

	public void FlushThreadSafeCallbacks()
	{
		if (global::AstarPath.OnThreadSafeCallback != null)
		{
			this.BlockUntilPathQueueBlocked();
			this.PerformBlockingActions(false, true);
		}
	}

	public static int CalculateThreadCount(global::Pathfinding.ThreadCount count)
	{
		if (count != global::Pathfinding.ThreadCount.AutomaticLowLoad && count != global::Pathfinding.ThreadCount.AutomaticHighLoad)
		{
			return (int)count;
		}
		int num = global::UnityEngine.Mathf.Max(1, global::UnityEngine.SystemInfo.processorCount);
		int num2 = global::UnityEngine.SystemInfo.systemMemorySize;
		if (num2 <= 0)
		{
			global::UnityEngine.Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
			num2 = 1024;
		}
		if (num <= 1)
		{
			return 0;
		}
		if (num2 <= 512)
		{
			return 0;
		}
		if (count == global::Pathfinding.ThreadCount.AutomaticHighLoad)
		{
			if (num2 <= 1024)
			{
				num = global::System.Math.Min(num, 2);
			}
		}
		else
		{
			num /= 2;
			num = global::UnityEngine.Mathf.Max(1, num);
			if (num2 <= 1024)
			{
				num = global::System.Math.Min(num, 2);
			}
			num = global::System.Math.Min(num, 6);
		}
		return num;
	}

	private void Awake()
	{
		global::AstarPath.active = this;
		if (global::UnityEngine.Object.FindObjectsOfType(typeof(global::AstarPath)).Length > 1)
		{
			global::UnityEngine.Debug.LogError("You should NOT have more than one AstarPath component in the scene at any time.\nThis can cause serious errors since the AstarPath component builds around a singleton pattern.");
		}
		base.useGUILayout = false;
		if (!global::UnityEngine.Application.isPlaying)
		{
			return;
		}
		if (global::AstarPath.OnAwakeSettings != null)
		{
			global::AstarPath.OnAwakeSettings();
		}
		global::Pathfinding.GraphModifier.FindAllModifiers();
		global::Pathfinding.RelevantGraphSurface.FindAllGraphSurfaces();
		this.InitializePathProcessor();
		this.InitializeProfiler();
		this.SetUpReferences();
		this.InitializeAstarData();
		this.FlushWorkItems();
		this.euclideanEmbedding.dirty = true;
		if (this.scanOnStartup && (!this.astarData.cacheStartup || this.astarData.file_cachedStartup == null))
		{
			this.Scan();
		}
	}

	private void InitializePathProcessor()
	{
		int num = global::AstarPath.CalculateThreadCount(this.threadCount);
		int processors = global::UnityEngine.Mathf.Max(num, 1);
		bool flag = num > 0;
		this.pathProcessor = new global::Pathfinding.PathProcessor(this, this.pathReturnQueue, processors, flag);
		this.pathProcessor.OnPathPreSearch += delegate(global::Pathfinding.Path path)
		{
			global::Pathfinding.OnPathDelegate onPathPreSearch = global::AstarPath.OnPathPreSearch;
			if (onPathPreSearch != null)
			{
				onPathPreSearch(path);
			}
		};
		this.pathProcessor.OnPathPostSearch += delegate(global::Pathfinding.Path path)
		{
			this.LogPathResults(path);
			global::Pathfinding.OnPathDelegate onPathPostSearch = global::AstarPath.OnPathPostSearch;
			if (onPathPostSearch != null)
			{
				onPathPostSearch(path);
			}
		};
		if (flag)
		{
			this.graphUpdates.EnableMultithreading();
		}
	}

	internal void VerifyIntegrity()
	{
		if (global::AstarPath.active != this)
		{
			throw new global::System.Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
		}
		if (this.astarData == null)
		{
			throw new global::System.NullReferenceException("AstarData is null... Astar not set up correctly?");
		}
		if (this.astarData.graphs == null)
		{
			this.astarData.graphs = new global::Pathfinding.NavGraph[0];
		}
	}

	public void SetUpReferences()
	{
		global::AstarPath.active = this;
		if (this.astarData == null)
		{
			this.astarData = new global::Pathfinding.AstarData();
		}
		if (this.colorSettings == null)
		{
			this.colorSettings = new global::Pathfinding.AstarColor();
		}
		this.colorSettings.OnEnable();
	}

	private void InitializeProfiler()
	{
	}

	private void InitializeAstarData()
	{
		this.astarData.FindGraphTypes();
		this.astarData.Awake();
		this.astarData.UpdateShortcuts();
		for (int i = 0; i < this.astarData.graphs.Length; i++)
		{
			if (this.astarData.graphs[i] != null)
			{
				this.astarData.graphs[i].Awake();
			}
		}
	}

	private void OnDisable()
	{
		if (this.OnUnloadGizmoMeshes != null)
		{
			this.OnUnloadGizmoMeshes();
		}
	}

	private void OnDestroy()
	{
		if (!global::UnityEngine.Application.isPlaying)
		{
			return;
		}
		if (this.logPathResults == global::Pathfinding.PathLog.Heavy)
		{
			global::UnityEngine.Debug.Log("+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
		}
		if (global::AstarPath.active != this)
		{
			return;
		}
		this.BlockUntilPathQueueBlocked();
		this.euclideanEmbedding.dirty = false;
		this.FlushWorkItemsInternal(false);
		this.pathProcessor.queue.TerminateReceivers();
		if (this.logPathResults == global::Pathfinding.PathLog.Heavy)
		{
			global::UnityEngine.Debug.Log("Processing Possible Work Items");
		}
		this.graphUpdates.DisableMultithreading();
		this.pathProcessor.JoinThreads();
		if (this.logPathResults == global::Pathfinding.PathLog.Heavy)
		{
			global::UnityEngine.Debug.Log("Returning Paths");
		}
		this.pathReturnQueue.ReturnPaths(false);
		if (this.logPathResults == global::Pathfinding.PathLog.Heavy)
		{
			global::UnityEngine.Debug.Log("Destroying Graphs");
		}
		this.astarData.OnDestroy();
		if (this.logPathResults == global::Pathfinding.PathLog.Heavy)
		{
			global::UnityEngine.Debug.Log("Cleaning up variables");
		}
		this.OnDrawGizmosCallback = null;
		global::AstarPath.OnAwakeSettings = null;
		global::AstarPath.OnGraphPreScan = null;
		global::AstarPath.OnGraphPostScan = null;
		global::AstarPath.OnPathPreSearch = null;
		global::AstarPath.OnPathPostSearch = null;
		global::AstarPath.OnPreScan = null;
		global::AstarPath.OnPostScan = null;
		global::AstarPath.OnLatePostScan = null;
		global::AstarPath.On65KOverflow = null;
		global::AstarPath.OnGraphsUpdated = null;
		global::AstarPath.OnThreadSafeCallback = null;
		global::AstarPath.active = null;
	}

	public void FloodFill(global::Pathfinding.GraphNode seed)
	{
		this.graphUpdates.FloodFill(seed);
	}

	public void FloodFill(global::Pathfinding.GraphNode seed, uint area)
	{
		this.graphUpdates.FloodFill(seed, area);
	}

	[global::UnityEngine.ContextMenu("Flood Fill Graphs")]
	public void FloodFill()
	{
		this.graphUpdates.FloodFill();
		this.workItems.OnFloodFill();
	}

	internal int GetNewNodeIndex()
	{
		return this.pathProcessor.GetNewNodeIndex();
	}

	internal void InitializeNode(global::Pathfinding.GraphNode node)
	{
		this.pathProcessor.InitializeNode(node);
	}

	internal void DestroyNode(global::Pathfinding.GraphNode node)
	{
		this.pathProcessor.DestroyNode(node);
	}

	public void BlockUntilPathQueueBlocked()
	{
		this.pathProcessor.BlockUntilPathQueueBlocked();
	}

	public void Scan()
	{
		foreach (global::Pathfinding.Progress progress in this.ScanAsync())
		{
		}
	}

	[global::System.Obsolete("ScanLoop is now named ScanAsync and is an IEnumerable<Progress>. Use foreach to iterate over the progress insead")]
	public void ScanLoop(global::Pathfinding.OnScanStatus statusCallback)
	{
		foreach (global::Pathfinding.Progress progress in this.ScanAsync())
		{
			statusCallback(progress);
		}
	}

	public global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanAsync()
	{
		if (this.graphs == null)
		{
			yield break;
		}
		this.isScanning = true;
		this.euclideanEmbedding.dirty = false;
		this.VerifyIntegrity();
		this.BlockUntilPathQueueBlocked();
		this.pathReturnQueue.ReturnPaths(false);
		this.BlockUntilPathQueueBlocked();
		if (!global::UnityEngine.Application.isPlaying)
		{
			global::Pathfinding.GraphModifier.FindAllModifiers();
			global::Pathfinding.RelevantGraphSurface.FindAllGraphSurfaces();
		}
		global::Pathfinding.RelevantGraphSurface.UpdateAllPositions();
		this.astarData.UpdateShortcuts();
		yield return new global::Pathfinding.Progress(0.05f, "Pre processing graphs");
		if (global::AstarPath.OnPreScan != null)
		{
			global::AstarPath.OnPreScan(this);
		}
		global::Pathfinding.GraphModifier.TriggerEvent(global::Pathfinding.GraphModifier.EventType.PreScan);
		global::System.Diagnostics.Stopwatch watch = global::System.Diagnostics.Stopwatch.StartNew();
		for (int i = 0; i < this.graphs.Length; i++)
		{
			if (this.graphs[i] != null)
			{
				this.graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
				{
					node.Destroy();
					return true;
				});
			}
		}
		for (int j = 0; j < this.graphs.Length; j++)
		{
			if (this.graphs[j] != null)
			{
				float minp = global::UnityEngine.Mathf.Lerp(0.1f, 0.8f, (float)j / (float)this.graphs.Length);
				float maxp = global::UnityEngine.Mathf.Lerp(0.1f, 0.8f, ((float)j + 0.95f) / (float)this.graphs.Length);
				string progressDescriptionPrefix = string.Concat(new object[]
				{
					"Scanning graph ",
					j + 1,
					" of ",
					this.graphs.Length,
					" - "
				});
				foreach (global::Pathfinding.Progress progress in this.ScanGraph(this.graphs[j]))
				{
					yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(minp, maxp, progress.progress), progressDescriptionPrefix + progress.description);
				}
			}
		}
		yield return new global::Pathfinding.Progress(0.8f, "Post processing graphs");
		if (global::AstarPath.OnPostScan != null)
		{
			global::AstarPath.OnPostScan(this);
		}
		global::Pathfinding.GraphModifier.TriggerEvent(global::Pathfinding.GraphModifier.EventType.PostScan);
		try
		{
			this.FlushWorkItemsInternal(false);
		}
		catch (global::System.Exception ex)
		{
			global::System.Exception e = ex;
			global::UnityEngine.Debug.LogException(e);
		}
		yield return new global::Pathfinding.Progress(0.9f, "Computing areas");
		this.FloodFill();
		this.VerifyIntegrity();
		yield return new global::Pathfinding.Progress(0.95f, "Late post processing");
		this.isScanning = false;
		if (global::AstarPath.OnLatePostScan != null)
		{
			global::AstarPath.OnLatePostScan(this);
		}
		global::Pathfinding.GraphModifier.TriggerEvent(global::Pathfinding.GraphModifier.EventType.LatePostScan);
		this.euclideanEmbedding.dirty = true;
		this.euclideanEmbedding.RecalculatePivots();
		this.PerformBlockingActions(true, true);
		watch.Stop();
		this.lastScanTime = (float)watch.Elapsed.TotalSeconds;
		global::System.GC.Collect();
		this.Log("Scanning - Process took " + (this.lastScanTime * 1000f).ToString("0") + " ms to complete");
		yield break;
	}

	private global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanGraph(global::Pathfinding.NavGraph graph)
	{
		if (global::AstarPath.OnGraphPreScan != null)
		{
			yield return new global::Pathfinding.Progress(0f, "Pre processing");
			global::AstarPath.OnGraphPreScan(graph);
		}
		yield return new global::Pathfinding.Progress(0f, string.Empty);
		foreach (global::Pathfinding.Progress p in graph.ScanInternal())
		{
			yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(0f, 0.95f, p.progress), p.description);
		}
		yield return new global::Pathfinding.Progress(0.95f, "Assigning graph indices");
		graph.GetNodes(delegate(global::Pathfinding.GraphNode node)
		{
			node.GraphIndex = graph.graphIndex;
			return true;
		});
		if (global::AstarPath.OnGraphPostScan != null)
		{
			yield return new global::Pathfinding.Progress(0.99f, "Post processing");
			global::AstarPath.OnGraphPostScan(graph);
		}
		yield break;
	}

	public static void WaitForPath(global::Pathfinding.Path p)
	{
		if (global::AstarPath.active == null)
		{
			throw new global::System.Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
		}
		if (p == null)
		{
			throw new global::System.ArgumentNullException("Path must not be null");
		}
		if (global::AstarPath.active.pathProcessor.queue.IsTerminating)
		{
			return;
		}
		if (p.GetState() == global::Pathfinding.PathState.Created)
		{
			throw new global::System.Exception("The specified path has not been started yet.");
		}
		global::AstarPath.waitForPathDepth++;
		if (global::AstarPath.waitForPathDepth == 5)
		{
			global::UnityEngine.Debug.LogError("You are calling the WaitForPath function recursively (maybe from a path callback). Please don't do this.");
		}
		if (p.GetState() < global::Pathfinding.PathState.ReturnQueue)
		{
			if (global::AstarPath.active.IsUsingMultithreading)
			{
				while (p.GetState() < global::Pathfinding.PathState.ReturnQueue)
				{
					if (global::AstarPath.active.pathProcessor.queue.IsTerminating)
					{
						global::AstarPath.waitForPathDepth--;
						throw new global::System.Exception("Pathfinding Threads seems to have crashed.");
					}
					global::System.Threading.Thread.Sleep(1);
					global::AstarPath.active.PerformBlockingActions(false, true);
				}
			}
			else
			{
				while (p.GetState() < global::Pathfinding.PathState.ReturnQueue)
				{
					if (global::AstarPath.active.pathProcessor.queue.IsEmpty && p.GetState() != global::Pathfinding.PathState.Processing)
					{
						global::AstarPath.waitForPathDepth--;
						throw new global::System.Exception("Critical error. Path Queue is empty but the path state is '" + p.GetState() + "'");
					}
					global::AstarPath.active.pathProcessor.TickNonMultithreaded();
					global::AstarPath.active.PerformBlockingActions(false, true);
				}
			}
		}
		global::AstarPath.active.pathReturnQueue.ReturnPaths(false);
		global::AstarPath.waitForPathDepth--;
	}

	[global::System.Obsolete("The threadSafe parameter has been deprecated")]
	public static void RegisterSafeUpdate(global::System.Action callback, bool threadSafe)
	{
		global::AstarPath.RegisterSafeUpdate(callback);
	}

	public static void RegisterSafeUpdate(global::System.Action callback)
	{
		if (callback == null || !global::UnityEngine.Application.isPlaying)
		{
			return;
		}
		if (global::AstarPath.active.pathProcessor.queue.AllReceiversBlocked)
		{
			global::AstarPath.active.pathProcessor.queue.Lock();
			try
			{
				if (global::AstarPath.active.pathProcessor.queue.AllReceiversBlocked)
				{
					callback();
					return;
				}
			}
			finally
			{
				global::AstarPath.active.pathProcessor.queue.Unlock();
			}
		}
		object obj = global::AstarPath.safeUpdateLock;
		lock (obj)
		{
			global::AstarPath.OnThreadSafeCallback = (global::System.Action)global::System.Delegate.Combine(global::AstarPath.OnThreadSafeCallback, callback);
		}
		global::AstarPath.active.pathProcessor.queue.Block();
	}

	private void InterruptPathfinding()
	{
		this.pathProcessor.queue.Block();
	}

	public static void StartPath(global::Pathfinding.Path p, bool pushToFront = false)
	{
		global::AstarPath astarPath = global::AstarPath.active;
		if (object.ReferenceEquals(astarPath, null))
		{
			global::UnityEngine.Debug.LogError("There is no AstarPath object in the scene or it has not been initialized yet");
			return;
		}
		if (p.GetState() != global::Pathfinding.PathState.Created)
		{
			throw new global::System.Exception(string.Concat(new object[]
			{
				"The path has an invalid state. Expected ",
				global::Pathfinding.PathState.Created,
				" found ",
				p.GetState(),
				"\nMake sure you are not requesting the same path twice"
			}));
		}
		if (astarPath.pathProcessor.queue.IsTerminating)
		{
			p.Error();
			return;
		}
		if (astarPath.graphs == null || astarPath.graphs.Length == 0)
		{
			global::UnityEngine.Debug.LogError("There are no graphs in the scene");
			p.Error();
			global::UnityEngine.Debug.LogError(p.errorLog);
			return;
		}
		p.Claim(astarPath);
		p.AdvanceState(global::Pathfinding.PathState.PathQueue);
		if (pushToFront)
		{
			astarPath.pathProcessor.queue.PushFront(p);
		}
		else
		{
			astarPath.pathProcessor.queue.Push(p);
		}
	}

	private void OnApplicationQuit()
	{
		this.OnDestroy();
		this.pathProcessor.AbortThreads();
	}

	public global::Pathfinding.NNInfo GetNearest(global::UnityEngine.Vector3 position)
	{
		return this.GetNearest(position, global::Pathfinding.NNConstraint.None);
	}

	public global::Pathfinding.NNInfo GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
	{
		return this.GetNearest(position, constraint, null);
	}

	public global::Pathfinding.NNInfo GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
	{
		global::Pathfinding.NavGraph[] graphs = this.graphs;
		float num = float.PositiveInfinity;
		global::Pathfinding.NNInfoInternal internalInfo = default(global::Pathfinding.NNInfoInternal);
		int num2 = -1;
		if (graphs != null)
		{
			for (int i = 0; i < graphs.Length; i++)
			{
				global::Pathfinding.NavGraph navGraph = graphs[i];
				if (navGraph != null && constraint.SuitableGraph(i, navGraph))
				{
					global::Pathfinding.NNInfoInternal nninfoInternal;
					if (this.fullGetNearestSearch)
					{
						nninfoInternal = navGraph.GetNearestForce(position, constraint);
					}
					else
					{
						nninfoInternal = navGraph.GetNearest(position, constraint);
					}
					if (nninfoInternal.node != null)
					{
						float magnitude = (nninfoInternal.clampedPosition - position).magnitude;
						if (this.prioritizeGraphs && magnitude < this.prioritizeGraphsLimit)
						{
							internalInfo = nninfoInternal;
							num2 = i;
							break;
						}
						if (magnitude < num)
						{
							num = magnitude;
							internalInfo = nninfoInternal;
							num2 = i;
						}
					}
				}
			}
		}
		if (num2 == -1)
		{
			return default(global::Pathfinding.NNInfo);
		}
		if (internalInfo.constrainedNode != null)
		{
			internalInfo.node = internalInfo.constrainedNode;
			internalInfo.clampedPosition = internalInfo.constClampedPosition;
		}
		if (!this.fullGetNearestSearch && internalInfo.node != null && !constraint.Suitable(internalInfo.node))
		{
			global::Pathfinding.NNInfoInternal nearestForce = graphs[num2].GetNearestForce(position, constraint);
			if (nearestForce.node != null)
			{
				internalInfo = nearestForce;
			}
		}
		if (!constraint.Suitable(internalInfo.node) || (constraint.constrainDistance && (internalInfo.clampedPosition - position).sqrMagnitude > this.maxNearestNodeDistanceSqr))
		{
			return default(global::Pathfinding.NNInfo);
		}
		return new global::Pathfinding.NNInfo(internalInfo);
	}

	public global::Pathfinding.GraphNode GetNearest(global::UnityEngine.Ray ray)
	{
		if (this.graphs == null)
		{
			return null;
		}
		float minDist = float.PositiveInfinity;
		global::Pathfinding.GraphNode nearestNode = null;
		global::UnityEngine.Vector3 lineDirection = ray.direction;
		global::UnityEngine.Vector3 lineOrigin = ray.origin;
		for (int i = 0; i < this.graphs.Length; i++)
		{
			global::Pathfinding.NavGraph navGraph = this.graphs[i];
			navGraph.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)node.position;
				global::UnityEngine.Vector3 a = lineOrigin + global::UnityEngine.Vector3.Dot(vector - lineOrigin, lineDirection) * lineDirection;
				float num = global::UnityEngine.Mathf.Abs(a.x - vector.x);
				num *= num;
				if (num > minDist)
				{
					return true;
				}
				num = global::UnityEngine.Mathf.Abs(a.z - vector.z);
				num *= num;
				if (num > minDist)
				{
					return true;
				}
				float sqrMagnitude = (a - vector).sqrMagnitude;
				if (sqrMagnitude < minDist)
				{
					minDist = sqrMagnitude;
					nearestNode = node;
				}
				return true;
			});
		}
		return nearestNode;
	}

	public static readonly global::AstarPath.AstarDistribution Distribution = global::AstarPath.AstarDistribution.WebsiteDownload;

	public static readonly string Branch = "rvo_fix_Pro";

	public global::Pathfinding.AstarData astarData;

	public static global::AstarPath active;

	public bool showNavGraphs = true;

	public bool showUnwalkableNodes = true;

	public global::Pathfinding.GraphDebugMode debugMode;

	public float debugFloor;

	public float debugRoof = 20000f;

	public bool manualDebugFloorRoof;

	public bool showSearchTree;

	public float unwalkableNodeDebugSize = 0.3f;

	public global::Pathfinding.PathLog logPathResults = global::Pathfinding.PathLog.Normal;

	public float maxNearestNodeDistance = 100f;

	public bool scanOnStartup = true;

	public bool fullGetNearestSearch;

	public bool prioritizeGraphs;

	public float prioritizeGraphsLimit = 1f;

	public global::Pathfinding.AstarColor colorSettings;

	[global::UnityEngine.SerializeField]
	protected string[] tagNames;

	public global::Pathfinding.Heuristic heuristic = global::Pathfinding.Heuristic.Euclidean;

	public float heuristicScale = 1f;

	public global::Pathfinding.ThreadCount threadCount;

	public float maxFrameTime = 1f;

	[global::System.Obsolete("Minimum area size is mostly obsolete since the limit has been raised significantly, and the edge cases are handled automatically")]
	public int minAreaSize;

	public bool batchGraphUpdates;

	public float graphUpdateBatchingInterval = 0.2f;

	[global::System.NonSerialized]
	public global::Pathfinding.Path debugPath;

	private string inGameDebugPath;

	public static global::System.Action OnAwakeSettings;

	public static global::Pathfinding.OnGraphDelegate OnGraphPreScan;

	public static global::Pathfinding.OnGraphDelegate OnGraphPostScan;

	public static global::Pathfinding.OnPathDelegate OnPathPreSearch;

	public static global::Pathfinding.OnPathDelegate OnPathPostSearch;

	public static global::Pathfinding.OnScanDelegate OnPreScan;

	public static global::Pathfinding.OnScanDelegate OnPostScan;

	public static global::Pathfinding.OnScanDelegate OnLatePostScan;

	public static global::Pathfinding.OnScanDelegate OnGraphsUpdated;

	public static global::System.Action On65KOverflow;

	private static global::System.Action OnThreadSafeCallback;

	public global::System.Action OnDrawGizmosCallback;

	public global::System.Action OnUnloadGizmoMeshes;

	[global::System.Obsolete]
	public global::System.Action OnGraphsWillBeUpdated;

	[global::System.Obsolete]
	public global::System.Action OnGraphsWillBeUpdated2;

	private readonly global::Pathfinding.GraphUpdateProcessor graphUpdates;

	private readonly global::Pathfinding.WorkItemProcessor workItems;

	private global::Pathfinding.PathProcessor pathProcessor;

	private bool graphUpdateRoutineRunning;

	private bool graphUpdatesWorkItemAdded;

	private float lastGraphUpdate = -9999f;

	private bool workItemsQueued;

	private readonly global::Pathfinding.PathReturnQueue pathReturnQueue;

	public global::Pathfinding.EuclideanEmbedding euclideanEmbedding = new global::Pathfinding.EuclideanEmbedding();

	public bool showGraphs;

	private static readonly object safeUpdateLock = new object();

	private ushort nextFreePathID = 1;

	private static int waitForPathDepth = 0;

	public enum AstarDistribution
	{
		WebsiteDownload,
		AssetStore
	}
}
