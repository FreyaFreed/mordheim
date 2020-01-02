using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_seeker.php")]
[global::UnityEngine.AddComponentMenu("Pathfinding/Seeker")]
public class Seeker : global::UnityEngine.MonoBehaviour, global::UnityEngine.ISerializationCallbackReceiver
{
	public Seeker()
	{
		this.onPathDelegate = new global::Pathfinding.OnPathDelegate(this.OnPathComplete);
		this.onPartialPathDelegate = new global::Pathfinding.OnPathDelegate(this.OnPartialPathComplete);
	}

	void global::UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void global::UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		if (this.traversableTagsCompatibility != null && this.traversableTagsCompatibility.tagsChange != -1)
		{
			this.traversableTags = this.traversableTagsCompatibility.tagsChange;
			this.traversableTagsCompatibility = new global::Pathfinding.TagMask(-1, -1);
		}
	}

	private void Awake()
	{
		this.startEndModifier.Awake(this);
	}

	public global::Pathfinding.Path GetCurrentPath()
	{
		return this.path;
	}

	public void OnDestroy()
	{
		this.ReleaseClaimedPath();
		this.startEndModifier.OnDestroy(this);
	}

	public void ReleaseClaimedPath()
	{
		if (this.prevPath != null)
		{
			this.prevPath.Release(this, true);
			this.prevPath = null;
		}
	}

	public void RegisterModifier(global::Pathfinding.IPathModifier mod)
	{
		this.modifiers.Add(mod);
		this.modifiers.Sort((global::Pathfinding.IPathModifier a, global::Pathfinding.IPathModifier b) => a.Order.CompareTo(b.Order));
	}

	public void DeregisterModifier(global::Pathfinding.IPathModifier mod)
	{
		this.modifiers.Remove(mod);
	}

	public void PostProcess(global::Pathfinding.Path p)
	{
		this.RunModifiers(global::Seeker.ModifierPass.PostProcess, p);
	}

	public void RunModifiers(global::Seeker.ModifierPass pass, global::Pathfinding.Path p)
	{
		if (pass == global::Seeker.ModifierPass.PreProcess && this.preProcessPath != null)
		{
			this.preProcessPath(p);
		}
		else if (pass == global::Seeker.ModifierPass.PostProcess && this.postProcessPath != null)
		{
			this.postProcessPath(p);
		}
		for (int i = 0; i < this.modifiers.Count; i++)
		{
			global::Pathfinding.MonoModifier monoModifier = this.modifiers[i] as global::Pathfinding.MonoModifier;
			if (!(monoModifier != null) || monoModifier.enabled)
			{
				if (pass == global::Seeker.ModifierPass.PreProcess)
				{
					this.modifiers[i].PreProcess(p);
				}
				else if (pass == global::Seeker.ModifierPass.PostProcess)
				{
					this.modifiers[i].Apply(p);
				}
			}
		}
	}

	public bool IsDone()
	{
		return this.path == null || this.path.GetState() >= global::Pathfinding.PathState.Returned;
	}

	private void OnPathComplete(global::Pathfinding.Path p)
	{
		this.OnPathComplete(p, true, true);
	}

	private void OnPathComplete(global::Pathfinding.Path p, bool runModifiers, bool sendCallbacks)
	{
		if (p != null && p != this.path && sendCallbacks)
		{
			return;
		}
		if (this == null || p == null || p != this.path)
		{
			return;
		}
		if (!this.path.error && runModifiers)
		{
			this.RunModifiers(global::Seeker.ModifierPass.PostProcess, this.path);
		}
		if (sendCallbacks)
		{
			p.Claim(this);
			this.lastCompletedNodePath = p.path;
			this.lastCompletedVectorPath = p.vectorPath;
			if (this.tmpPathCallback != null)
			{
				this.tmpPathCallback(p);
			}
			if (this.pathCallback != null)
			{
				this.pathCallback(p);
			}
			if (this.prevPath != null)
			{
				this.prevPath.Release(this, true);
			}
			this.prevPath = p;
			if (!this.drawGizmos)
			{
				this.ReleaseClaimedPath();
			}
		}
	}

	private void OnPartialPathComplete(global::Pathfinding.Path p)
	{
		this.OnPathComplete(p, true, false);
	}

	private void OnMultiPathComplete(global::Pathfinding.Path p)
	{
		this.OnPathComplete(p, false, true);
	}

	public global::Pathfinding.ABPath GetNewPath(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end)
	{
		return global::Pathfinding.ABPath.Construct(start, end, null);
	}

	public global::Pathfinding.Path StartPath(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end)
	{
		return this.StartPath(start, end, null, -1);
	}

	public global::Pathfinding.Path StartPath(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.OnPathDelegate callback)
	{
		return this.StartPath(start, end, callback, -1);
	}

	public global::Pathfinding.Path StartPath(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::Pathfinding.OnPathDelegate callback, int graphMask)
	{
		return this.StartPath(this.GetNewPath(start, end), callback, graphMask);
	}

	public global::Pathfinding.Path StartPath(global::Pathfinding.Path p, global::Pathfinding.OnPathDelegate callback = null, int graphMask = -1)
	{
		global::Pathfinding.MultiTargetPath multiTargetPath = p as global::Pathfinding.MultiTargetPath;
		if (multiTargetPath != null)
		{
			global::Pathfinding.OnPathDelegate[] array = new global::Pathfinding.OnPathDelegate[multiTargetPath.targetPoints.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.onPartialPathDelegate;
			}
			multiTargetPath.callbacks = array;
			p.callback = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Combine(p.callback, new global::Pathfinding.OnPathDelegate(this.OnMultiPathComplete));
		}
		else
		{
			p.callback = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Combine(p.callback, this.onPathDelegate);
		}
		p.enabledTags = this.traversableTags;
		p.tagPenalties = this.tagPenalties;
		p.nnConstraint.graphMask = graphMask;
		this.StartPathInternal(p, callback);
		return p;
	}

	private void StartPathInternal(global::Pathfinding.Path p, global::Pathfinding.OnPathDelegate callback)
	{
		if (this.path != null && this.path.GetState() <= global::Pathfinding.PathState.Processing && this.lastPathID == (uint)this.path.pathID)
		{
			this.path.Error();
		}
		this.path = p;
		this.tmpPathCallback = callback;
		this.lastPathID = (uint)this.path.pathID;
		this.RunModifiers(global::Seeker.ModifierPass.PreProcess, this.path);
		global::AstarPath.StartPath(this.path, false);
	}

	public global::Pathfinding.MultiTargetPath StartMultiTargetPath(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3[] endPoints, bool pathsForAll, global::Pathfinding.OnPathDelegate callback = null, int graphMask = -1)
	{
		global::Pathfinding.MultiTargetPath multiTargetPath = global::Pathfinding.MultiTargetPath.Construct(start, endPoints, null, null);
		multiTargetPath.pathsForAll = pathsForAll;
		this.StartPath(multiTargetPath, callback, graphMask);
		return multiTargetPath;
	}

	public global::Pathfinding.MultiTargetPath StartMultiTargetPath(global::UnityEngine.Vector3[] startPoints, global::UnityEngine.Vector3 end, bool pathsForAll, global::Pathfinding.OnPathDelegate callback = null, int graphMask = -1)
	{
		global::Pathfinding.MultiTargetPath multiTargetPath = global::Pathfinding.MultiTargetPath.Construct(startPoints, end, null, null);
		multiTargetPath.pathsForAll = pathsForAll;
		this.StartPath(multiTargetPath, callback, graphMask);
		return multiTargetPath;
	}

	[global::System.Obsolete("You can use StartPath instead of this method now. It will behave identically.")]
	public global::Pathfinding.MultiTargetPath StartMultiTargetPath(global::Pathfinding.MultiTargetPath p, global::Pathfinding.OnPathDelegate callback = null, int graphMask = -1)
	{
		this.StartPath(p, callback, graphMask);
		return p;
	}

	public void OnDrawGizmos()
	{
		if (this.lastCompletedNodePath == null || !this.drawGizmos)
		{
			return;
		}
		if (this.detailedGizmos)
		{
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(0.7f, 0.5f, 0.1f, 0.5f);
			if (this.lastCompletedNodePath != null)
			{
				for (int i = 0; i < this.lastCompletedNodePath.Count - 1; i++)
				{
					global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)this.lastCompletedNodePath[i].position, (global::UnityEngine.Vector3)this.lastCompletedNodePath[i + 1].position);
				}
			}
		}
		global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(0f, 1f, 0f, 1f);
		if (this.lastCompletedVectorPath != null)
		{
			for (int j = 0; j < this.lastCompletedVectorPath.Count - 1; j++)
			{
				global::UnityEngine.Gizmos.DrawLine(this.lastCompletedVectorPath[j], this.lastCompletedVectorPath[j + 1]);
			}
		}
	}

	public bool drawGizmos = true;

	public bool detailedGizmos;

	public global::Pathfinding.StartEndModifier startEndModifier = new global::Pathfinding.StartEndModifier();

	[global::UnityEngine.HideInInspector]
	public int traversableTags = -1;

	[global::UnityEngine.HideInInspector]
	[global::UnityEngine.SerializeField]
	[global::UnityEngine.Serialization.FormerlySerializedAs("traversableTags")]
	protected global::Pathfinding.TagMask traversableTagsCompatibility = new global::Pathfinding.TagMask(-1, -1);

	[global::UnityEngine.HideInInspector]
	public int[] tagPenalties = new int[32];

	public global::Pathfinding.OnPathDelegate pathCallback;

	public global::Pathfinding.OnPathDelegate preProcessPath;

	public global::Pathfinding.OnPathDelegate postProcessPath;

	[global::System.NonSerialized]
	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> lastCompletedVectorPath;

	[global::System.NonSerialized]
	private global::System.Collections.Generic.List<global::Pathfinding.GraphNode> lastCompletedNodePath;

	[global::System.NonSerialized]
	protected global::Pathfinding.Path path;

	[global::System.NonSerialized]
	private global::Pathfinding.Path prevPath;

	private readonly global::Pathfinding.OnPathDelegate onPathDelegate;

	private readonly global::Pathfinding.OnPathDelegate onPartialPathDelegate;

	private global::Pathfinding.OnPathDelegate tmpPathCallback;

	protected uint lastPathID;

	private readonly global::System.Collections.Generic.List<global::Pathfinding.IPathModifier> modifiers = new global::System.Collections.Generic.List<global::Pathfinding.IPathModifier>();

	public enum ModifierPass
	{
		PreProcess,
		PostProcess = 2
	}
}
