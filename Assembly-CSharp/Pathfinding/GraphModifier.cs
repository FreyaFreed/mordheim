using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.ExecuteInEditMode]
	public abstract class GraphModifier : global::UnityEngine.MonoBehaviour
	{
		protected static global::System.Collections.Generic.List<T> GetModifiersOfType<T>() where T : global::Pathfinding.GraphModifier
		{
			global::Pathfinding.GraphModifier graphModifier = global::Pathfinding.GraphModifier.root;
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
			while (graphModifier != null)
			{
				T t = graphModifier as T;
				if (t != null)
				{
					list.Add(t);
				}
				graphModifier = graphModifier.next;
			}
			return list;
		}

		public static void FindAllModifiers()
		{
			global::Pathfinding.GraphModifier[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.GraphModifier)) as global::Pathfinding.GraphModifier[];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].enabled)
				{
					array[i].OnEnable();
				}
			}
		}

		public static void TriggerEvent(global::Pathfinding.GraphModifier.EventType type)
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				global::Pathfinding.GraphModifier.FindAllModifiers();
			}
			global::Pathfinding.GraphModifier graphModifier = global::Pathfinding.GraphModifier.root;
			switch (type)
			{
			case global::Pathfinding.GraphModifier.EventType.PostScan:
				while (graphModifier != null)
				{
					graphModifier.OnPostScan();
					graphModifier = graphModifier.next;
				}
				break;
			case global::Pathfinding.GraphModifier.EventType.PreScan:
				while (graphModifier != null)
				{
					graphModifier.OnPreScan();
					graphModifier = graphModifier.next;
				}
				break;
			default:
				if (type != global::Pathfinding.GraphModifier.EventType.PostUpdate)
				{
					if (type == global::Pathfinding.GraphModifier.EventType.PostCacheLoad)
					{
						while (graphModifier != null)
						{
							graphModifier.OnPostCacheLoad();
							graphModifier = graphModifier.next;
						}
					}
				}
				else
				{
					while (graphModifier != null)
					{
						graphModifier.OnGraphsPostUpdate();
						graphModifier = graphModifier.next;
					}
				}
				break;
			case global::Pathfinding.GraphModifier.EventType.LatePostScan:
				while (graphModifier != null)
				{
					graphModifier.OnLatePostScan();
					graphModifier = graphModifier.next;
				}
				break;
			case global::Pathfinding.GraphModifier.EventType.PreUpdate:
				while (graphModifier != null)
				{
					graphModifier.OnGraphsPreUpdate();
					graphModifier = graphModifier.next;
				}
				break;
			}
		}

		protected virtual void OnEnable()
		{
			this.RemoveFromLinkedList();
			this.AddToLinkedList();
			this.ConfigureUniqueID();
		}

		protected virtual void OnDisable()
		{
			this.RemoveFromLinkedList();
		}

		protected virtual void Awake()
		{
			this.ConfigureUniqueID();
		}

		private void ConfigureUniqueID()
		{
			global::Pathfinding.GraphModifier x;
			if (global::Pathfinding.GraphModifier.usedIDs.TryGetValue(this.uniqueID, out x) && x != this)
			{
				this.Reset();
			}
			global::Pathfinding.GraphModifier.usedIDs[this.uniqueID] = this;
		}

		private void AddToLinkedList()
		{
			if (global::Pathfinding.GraphModifier.root == null)
			{
				global::Pathfinding.GraphModifier.root = this;
			}
			else
			{
				this.next = global::Pathfinding.GraphModifier.root;
				global::Pathfinding.GraphModifier.root.prev = this;
				global::Pathfinding.GraphModifier.root = this;
			}
		}

		private void RemoveFromLinkedList()
		{
			if (global::Pathfinding.GraphModifier.root == this)
			{
				global::Pathfinding.GraphModifier.root = this.next;
				if (global::Pathfinding.GraphModifier.root != null)
				{
					global::Pathfinding.GraphModifier.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		protected virtual void OnDestroy()
		{
			global::Pathfinding.GraphModifier.usedIDs.Remove(this.uniqueID);
		}

		public virtual void OnPostScan()
		{
		}

		public virtual void OnPreScan()
		{
		}

		public virtual void OnLatePostScan()
		{
		}

		public virtual void OnPostCacheLoad()
		{
		}

		public virtual void OnGraphsPreUpdate()
		{
		}

		public virtual void OnGraphsPostUpdate()
		{
		}

		private void Reset()
		{
			this.uniqueID = (ulong)((long)global::UnityEngine.Random.Range(0, int.MaxValue) | (long)global::UnityEngine.Random.Range(0, int.MaxValue) << 32);
			global::Pathfinding.GraphModifier.usedIDs[this.uniqueID] = this;
		}

		private static global::Pathfinding.GraphModifier root;

		private global::Pathfinding.GraphModifier prev;

		private global::Pathfinding.GraphModifier next;

		[global::UnityEngine.SerializeField]
		[global::UnityEngine.HideInInspector]
		protected ulong uniqueID;

		protected static global::System.Collections.Generic.Dictionary<ulong, global::Pathfinding.GraphModifier> usedIDs = new global::System.Collections.Generic.Dictionary<ulong, global::Pathfinding.GraphModifier>();

		public enum EventType
		{
			PostScan = 1,
			PreScan,
			LatePostScan = 4,
			PreUpdate = 8,
			PostUpdate = 16,
			PostCacheLoad = 32
		}
	}
}
