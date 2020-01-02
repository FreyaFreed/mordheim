using System;
using UnityEngine;

namespace Pathfinding
{
	[global::System.Serializable]
	public abstract class MonoModifier : global::UnityEngine.MonoBehaviour, global::Pathfinding.IPathModifier
	{
		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}

		public abstract int Order { get; }

		public void Awake()
		{
			this.seeker = base.GetComponent<global::Seeker>();
			if (this.seeker != null)
			{
				this.seeker.RegisterModifier(this);
			}
		}

		public void OnDestroy()
		{
			if (this.seeker != null)
			{
				this.seeker.DeregisterModifier(this);
			}
		}

		public virtual void PreProcess(global::Pathfinding.Path p)
		{
		}

		public abstract void Apply(global::Pathfinding.Path p);

		[global::System.NonSerialized]
		public global::Seeker seeker;
	}
}
